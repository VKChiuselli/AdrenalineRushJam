package com.myappfree.usdk.unity;

import android.app.Activity;
import android.app.Application;
import android.content.Context;
import android.util.Log;
import java.util.HashMap;


import com.appsflyer.AppsFlyerLib;
import com.myappfree.usdk.adserver.UADManager;
import com.myappfree.usdk.adserver.delegate.UADManagerDelegate;
import com.myappfree.usdk.adserver.model.AdRequest;
import com.unity3d.player.UnityPlayer;



public class uSDKUnityBridge implements UADManagerDelegate {
	private static uSDKUnityBridge instance;

	public static uSDKUnityBridge Instance()
	{
		if (instance == null)
		{
			instance = new uSDKUnityBridge();
		}
		return instance;
	}

	public void Init(String appsFlyerDevKey) {
		Log.d("uSDKUnityBridge", "Initializing");

		AppsFlyerLib appsFlyer = AppsFlyerLib.getInstance();
		appsFlyer.setDebugLog(true);

		UADManager uadManager = UADManager.Shared();
		uadManager.delegate = this;

		Activity unityActivity = UnityPlayer.currentActivity;
		if (unityActivity != null) {
			Log.d("uSDKUnityBridge", "Configuring AppsFlyer and UAD Manager");
			appsFlyer.init(appsFlyerDevKey, null, unityActivity);
		
			uadManager.Configure(unityActivity, "");

			if (uadManager.deeplinkDelegate != null) {
				Log.d("uSDKUnityBridge", "Subscribing UAD Manager to AppsFlyer deeplink");
				appsFlyer.subscribeForDeepLink(uadManager.deeplinkDelegate);
			}

			StartAF();
		}
	}



    public void UpdateServerUninstallTokenAF(String s){
		Activity unityActivity = UnityPlayer.currentActivity;
		if (unityActivity != null) {
			Log.d("uSDKUnityBridge", "Starting AppsFlyer");
			AppsFlyerLib.getInstance().updateServerUninstallToken(unityActivity ,s);
		}
	}


	public void SendEvent(String event_name, HashMap<String, Object> map){
		Activity unityActivity = UnityPlayer.currentActivity;
		if (unityActivity != null) {
			Log.d("uSDKUnityBridge", "Starting AppsFlyer");
			AppsFlyerLib.getInstance().logEvent(unityActivity,event_name,map);
		}
	}


	public void StartAF() {
	    Activity unityActivity = UnityPlayer.currentActivity;
		if (unityActivity != null) {
			Log.d("uSDKUnityBridge", "Starting AppsFlyer");
			AppsFlyerLib.getInstance().start(unityActivity);
		}
	}

	@Override
	public void DidClickOnAd(AdRequest request) {
		
	}

	@Override
	public void DeepLinkOfferwallIsReady(String url){
		Log.d("uSDK - DeepLinkOfferwallIsReady", "init");
		/**/
	}

}