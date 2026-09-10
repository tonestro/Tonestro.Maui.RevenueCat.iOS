# Maui.RevenueCat.iOS

.NET 8 iOS (MAUI) Bindings for RevenueCat ([Website](https://www.revenuecat.com/), [Setup Instructions](https://docs.revenuecat.com/docs/ios)).

## About this Repository

This repository is the successor of https://github.com/thisisthekap/Xamarin.RevenueCat.iOS, which contains the Xamarin.iOS Bindings for the RevenueCat SDK.

## NuGet Feeds

* Binding: https://www.nuget.org/packages/Tonestro.Maui.RevenueCat.iOS/
* Extensions: https://www.nuget.org/packages/Tonestro.Maui.RevenueCat.iOS.Extensions/

## Native SDK

Package version **5.89.0.1** bundles [RevenueCat iOS 5.89.0](https://github.com/RevenueCat/purchases-ios/releases/tag/5.89.0).
It includes the iOS arm64 device and arm64/x86_64 simulator frameworks and their matching debug symbols.
The minimum iOS version remains 13.0; the projects continue to target .NET 8.

The framework comes from the release's `RevenueCat.xcframework.zip` asset, with SHA-256
`8f6dee89945d4efe847d567686aa3a9bcedd263a89ca80a02907999028b6815a`.
Only the two iOS slices are retained, and `AvailableLibraries` in the outer `Info.plist` is filtered accordingly.
The outer XCFramework signature is removed because it covers the untrimmed, multi-platform archive;
the contained frameworks are copied unchanged.

The bindings include the new authentication and identity APIs, IAM keychain access group configuration,
virtual currency spending (requires IAM), Singular device ID attribution, and non-subscription transaction metadata.
Use `RCDangerousSettings.CreateWithTestStore(autoSyncPurchases, forceAllowTestStoreInReleaseBuilds)` for
the new Test Store setting. The existing `(bool, bool)` constructor still configures custom entitlement computation.
Ad tracking and rewarded ad APIs are Swift-only in this release and are not exposed by these Objective-C bindings.

## Validation

Build and pack both library projects in Release, copy the generated packages to `local-nugets`, then build
and run `Tonestro.Maui.RevenueCat.iOS.UsageChecker` on an iOS simulator. It checks the native SDK version,
new settings and selectors, and the existing configuration and async error callbacks using a fake API key.
The usage checker consumes the NuGet packages so that validation also exercises native framework packaging.
