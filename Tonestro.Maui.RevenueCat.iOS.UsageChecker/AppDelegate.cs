using RevenueCat;
using Tonestro.Maui.RevenueCat.iOS.Extensions;

namespace Tonestro.Maui.RevenueCat.iOS.UsageChecker;

[Register("AppDelegate")]
public class AppDelegate : UIApplicationDelegate
{
    public override UIWindow? Window { get; set; }

    private UITextView? _output;

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        Window = new UIWindow(UIScreen.MainScreen.Bounds);

        _output = new UITextView(Window.Frame)
        {
            Editable = false,
            Font = UIFont.GetMonospacedSystemFont(13, UIFontWeight.Regular),
            BackgroundColor = UIColor.SystemBackground,
            TextContainerInset = new UIEdgeInsets(60, 12, 12, 12),
            AutoresizingMask = UIViewAutoresizing.All,
        };

        var vc = new UIViewController();
        vc.View!.AddSubview(_output);
        Window.RootViewController = vc;
        Window.MakeKeyAndVisible();

        RunBindingChecks();

        return true;
    }

    private void AppendLine(string line)
    {
        Console.WriteLine(line);
        _output!.Text += line + Environment.NewLine;
    }

    private void RunBindingChecks()
    {
        AppendLine("== RevenueCat binding runtime check ==");
        AppendLine($"FrameworkVersion:        {RCPurchases.FrameworkVersion}");

        RCPurchases.LogLevel = RCLogLevel.Debug;
        AppendLine($"LogLevel:                {RCPurchases.LogLevel}");
        AppendLine($"IsConfigured (before):   {RCPurchases.IsConfigured}");

        var builder = RCConfiguration.BuilderWithAPIKey("appl_fakeApiKeyForBindingCheck")
            .WithAppUserID("binding-check-user")
            .WithPurchasesAreCompletedBy(RCPurchasesAreCompletedBy.RevenueCat, RCStoreKitVersion.StoreKit2);
        var purchases = RCPurchases.ConfigureWithConfiguration(builder.Build());

        AppendLine($"IsConfigured (after):    {RCPurchases.IsConfigured}");
        AppendLine($"CanMakePayments:         {RCPurchases.CanMakePayments}");
        AppendLine($"AppUserID:               {purchases.AppUserID}");
        AppendLine($"IsAnonymous:             {purchases.IsAnonymous}");
        AppendLine($"IsSandbox:               {purchases.IsSandbox}");
        AppendLine($"PurchasesAreCompletedBy: {purchases.PurchasesAreCompletedBy}");
        AppendLine($"PackageType roundtrip:   Annual -> \"{RCPackage.StringFrom(RCPackageType.Annual)}\"");

        purchases.GetStorefront(storefront => InvokeOnMainThread(() =>
            AppendLine(storefront is null
                ? "Storefront:              (null)"
                : $"Storefront:              {storefront.CountryCode} (id {storefront.Identifier})")));

        _ = ChecksAsync(purchases);
    }

    private async Task ChecksAsync(RCPurchases purchases)
    {
        // With a fake API key this must surface a real RevenueCat error through the
        // async Extensions API - which proves callback marshaling and NSError binding work.
        try
        {
            var offerings = await purchases.GetOfferingsAsync();
            InvokeOnMainThread(() => AppendLine($"GetOfferingsAsync:       {offerings.All.Count} offerings"));
        }
        catch (PurchasesErrorException ex)
        {
            InvokeOnMainThread(() =>
            {
                AppendLine($"GetOfferingsAsync error: {ex.PurchasesErrorCode} (code {(int)ex.PurchasesErrorCode})");
                AppendLine($"  readable error code:   {ex.ReadableErrorCode}");
            });
        }
    }
}
