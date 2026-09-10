using Foundation;

namespace RevenueCat;

public partial class RCDangerousSettings
{
    /// <summary>
    /// Creates settings that can allow Test Store API keys in internal Release builds.
    /// Do not enable this for builds uploaded to the App Store.
    /// </summary>
    /// <remarks>
    /// The existing two-boolean constructor configures custom entitlement computation.
    /// This named factory keeps that constructor's meaning unchanged.
    /// </remarks>
    public static RCDangerousSettings CreateWithTestStore(bool autoSyncPurchases,
        bool forceAllowTestStoreInReleaseBuilds)
    {
        var settings = new RCDangerousSettings(NSObjectFlag.Empty);
        settings.InitializeHandle(
            settings.InitWithTestStore(autoSyncPurchases, forceAllowTestStoreInReleaseBuilds),
            "initWithAutoSyncPurchases:forceAllowTestStoreInReleaseBuilds:");
        return settings;
    }
}
