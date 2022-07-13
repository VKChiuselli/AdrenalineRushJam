#import <AppsFlyerLib/AppsFlyerLib.h>
#import <StoreKit/StoreKit.h>
#import <WebKit/WebKit.h>

#import <uSDK/uSDK-Swift.h>

@interface uSDKAppDelegate : NSObject
@end

@implementation uSDKAppDelegate

-(void)uSDKConfigureWithAppsFlyerDevKey:(NSString *)appsFlyerDevKey appStoreId:(NSString *)appStoreId {
	AppsFlyerLib* appsFlyerLib = [AppsFlyerLib shared];
	
	appsFlyerLib.appsFlyerDevKey = appsFlyerDevKey;
	appsFlyerLib.appleAppID = appStoreId;
	
	[[UADManager shared] configureWithContext:[UIApplication sharedApplication] apikey:@""];
	appsFlyerLib.deepLinkDelegate = (id<AppsFlyerDeepLinkDelegate>)[UADManager shared].deeplinkDelegate;
	
	[[NSNotificationCenter defaultCenter] addObserver:self
		 selector:@selector(applicationDidBecomeActive:)
		 name:UIApplicationDidBecomeActiveNotification
		 object:nil];
	
	[appsFlyerLib start];
	
	if (@available(iOS 11.3, *)) {
		[SKAdNetwork registerAppForAdNetworkAttribution];
	}
}

-(void)applicationDidBecomeActive:(UIApplication*)application {
	[[AppsFlyerLib shared] start];
}

@end

NSString* CreateNSString (const char* string)
{
	if (string != NULL) {
		return [NSString stringWithUTF8String:string];
	}

	return [NSString stringWithUTF8String:""];
}

void USDKInit(const char * appsFlyerDevKeyCStr, const char * appStoreIdCStr) {
	static uSDKAppDelegate* delegateObject = nil;
	if (delegateObject == nil) {
		delegateObject = [uSDKAppDelegate alloc];
	}
	
	NSString *appsFlyerDevKey = CreateNSString(appsFlyerDevKeyCStr);
	NSString *appStoreId = CreateNSString(appStoreIdCStr);
	[delegateObject uSDKConfigureWithAppsFlyerDevKey:appsFlyerDevKey appStoreId:appStoreId];
}
