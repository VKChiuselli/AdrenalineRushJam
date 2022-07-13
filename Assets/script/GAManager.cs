using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;

public class GAManager : MonoBehaviour {
    public static GAManager istance;

    private void Awake() {

        //   GameAnalytics.initializeWithGameKey(activity, "[game key]", "[secret key]");
        istance = this;
        DontDestroyOnLoad(this);
    }


    void Start() {
        GameAnalytics.Initialize();
    }

}
