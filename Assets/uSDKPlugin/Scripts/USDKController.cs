using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace MyAppFree.uSDK
{
    public class USDKController : MonoBehaviour
    {
#if UNITY_IOS && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void USDKInit(string appsFlyerKey, string appStoreId);
#endif

        [SerializeField]
        private USDKConfig uSDKConfig;

        private bool isInitialized;

        private void Start()
        {
            DontDestroyOnLoad(this);
            Init();
        }

        private void Init()
        {
            if (isInitialized)
            {
                return;
            }

            if (uSDKConfig == null)
            {
                Debug.LogError("USDKController - uSDKConfig null, make sure to create your USDKConfig through the MenuItem, and link it to your USDKController");
                return;
            }

            bool configDataIssue = false;
            if (string.IsNullOrWhiteSpace(uSDKConfig.AppsFlyerKey))
            {
                Debug.LogError("USDKController - uSDKConfig.AppsFlyerKey is empty, make sure to set it on your USDKConfig");
                configDataIssue = true;
            }

            if (string.IsNullOrWhiteSpace(uSDKConfig.AppleId) && Application.platform == RuntimePlatform.IPhonePlayer)
            {
                Debug.LogError("USDKController - uSDKConfig.AppleId is empty, make sure to set it on your USDKConfig");
                configDataIssue = true;
            }

            if (configDataIssue)
            {
                return;
            }

            InitUSDK();

            isInitialized = true;
        }


        public static void UpdateServerUninstallToken(string token)
        {

#if UNITY_ANDROID && !UNITY_EDITOR
            AndroidJavaClass usdkBridge = new AndroidJavaClass("com.myappfree.usdk.unity.uSDKUnityBridge");
            AndroidJavaObject nameObj = new AndroidJavaObject("java.lang.String", token);
            AndroidJavaObject instance = usdkBridge.CallStatic<AndroidJavaObject>("Instance");
            instance.Call("UpdateServerUninstallTokenAF", nameObj);
#elif UNITY_IOS && !UNITY_EDITOR

#endif
        }

        public static void SendEvent(string name,Dictionary<string,object> map)
        {

#if UNITY_ANDROID && !UNITY_EDITOR
            AndroidJavaClass usdkBridge = new AndroidJavaClass("com.myappfree.usdk.unity.uSDKUnityBridge");
            AndroidJavaObject nameObj = new AndroidJavaObject("java.lang.String", name);
            AndroidJavaObject mapObj = CreateJavaMapFromDictainary(map);
            AndroidJavaObject instance = usdkBridge.CallStatic<AndroidJavaObject>("Instance");
            instance.Call("SendEvent", nameObj, mapObj);
#elif UNITY_IOS && !UNITY_EDITOR

#endif
        }

#if UNITY_ANDROID && !UNITY_EDITOR

        public static AndroidJavaObject CreateJavaMapFromDictainary(IDictionary<string, object> parameters)
        {
            AndroidJavaObject javaMap = new AndroidJavaObject("java.util.HashMap");
            System.IntPtr putMethod = AndroidJNIHelper.GetMethodID(
                    javaMap.GetRawClass(), "put",
                    "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");

            object[] args = new object[2];
            foreach (KeyValuePair<string, object> kvp in parameters)
            {

                using (AndroidJavaObject k = new AndroidJavaObject(
                        "java.lang.String", kvp.Key))
                {
                    using (AndroidJavaObject v = new AndroidJavaObject(
                            "java.lang.Object", kvp.Value))
                    {
                        args[0] = k;
                        args[1] = v;
                        AndroidJNI.CallObjectMethod(javaMap.GetRawObject(),
                                putMethod, AndroidJNIHelper.CreateJNIArgArray(args));
                    }
                }
            }

            return javaMap;
        }
#endif


        private void InitUSDK()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            AndroidJavaClass usdkBridge = new AndroidJavaClass("com.myappfree.usdk.unity.uSDKUnityBridge");
            AndroidJavaObject appsFlyerDevKey = new AndroidJavaObject("java.lang.String", uSDKConfig.AppsFlyerKey);
            AndroidJavaObject instance = usdkBridge.CallStatic<AndroidJavaObject>("Instance");
            instance.Call("Init", appsFlyerDevKey);
#elif UNITY_IOS && !UNITY_EDITOR
            USDKInit(uSDKConfig.AppsFlyerKey, uSDKConfig.AppleId);
#endif
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause || !isInitialized)
            {
                return;
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            AndroidJavaClass usdkBridge = new AndroidJavaClass("com.myappfree.usdk.unity.uSDKUnityBridge");
            AndroidJavaObject instance = usdkBridge.CallStatic<AndroidJavaObject>("Instance");
            instance.Call("StartAF");
#endif
        }
    }


    public class AppsflyerInAppEvents
    {
        /**
         * Event Type
         * */
        public const string LEVEL_ACHIEVED = "af_level_achieved";
        public const string ADD_PAYMENT_INFO = "af_add_payment_info";
        public const string ADD_TO_CART = "af_add_to_cart";
        public const string ADD_TO_WISH_LIST = "af_add_to_wishlist";
        public const string COMPLETE_REGISTRATION = "af_complete_registration";
        public const string TUTORIAL_COMPLETION = "af_tutorial_completion";
        public const string INITIATED_CHECKOUT = "af_initiated_checkout";
        public const string PURCHASE = "af_purchase";
        public const string RATE = "af_rate";
        public const string SEARCH = "af_search";
        public const string SPENT_CREDIT = "af_spent_credits";
        public const string ACHIEVEMENT_UNLOCKED = "af_achievement_unlocked";
        public const string CONTENT_VIEW = "af_content_view";
        public const string TRAVEL_BOOKING = "af_travel_booking";
        public const string SHARE = "af_share";
        public const string INVITE = "af_invite";
        public const string LOGIN = "af_login";
        public const string RE_ENGAGE = "af_re_engage";
        public const string UPDATE = "af_update";
        public const string OPENED_FROM_PUSH_NOTIFICATION = "af_opened_from_push_notification";
        public const string LOCATION_CHANGED = "af_location_changed";
        public const string LOCATION_COORDINATES = "af_location_coordinates";
        public const string ORDER_ID = "af_order_id";
        /**
         * Event Parameter Name
         * **/
        public const string LEVEL = "af_level";
        public const string SCORE = "af_score";
        public const string SUCCESS = "af_success";
        public const string PRICE = "af_price";
        public const string CONTENT_TYPE = "af_content_type";
        public const string CONTENT_ID = "af_content_id";
        public const string CONTENT_LIST = "af_content_list";
        public const string CURRENCY = "af_currency";
        public const string QUANTITY = "af_quantity";
        public const string REGSITRATION_METHOD = "af_registration_method";
        public const string PAYMENT_INFO_AVAILIBLE = "af_payment_info_available";
        public const string MAX_RATING_VALUE = "af_max_rating_value";
        public const string RATING_VALUE = "af_rating_value";
        public const string SEARCH_STRING = "af_search_string";
        public const string DATE_A = "af_date_a";
        public const string DATE_B = "af_date_b";
        public const string DESTINATION_A = "af_destination_a";
        public const string DESTINATION_B = "af_destination_b";
        public const string DESCRIPTION = "af_description";
        public const string CLASS = "af_class";
        public const string EVENT_START = "af_event_start";
        public const string EVENT_END = "af_event_end";
        public const string LATITUDE = "af_lat";
        public const string LONGTITUDE = "af_long";
        public const string CUSTOMER_USER_ID = "af_customer_user_id";
        public const string VALIDATED = "af_validated";
        public const string REVENUE = "af_revenue";
        public const string RECEIPT_ID = "af_receipt_id";
        public const string PARAM_1 = "af_param_1";
        public const string PARAM_2 = "af_param_2";
        public const string PARAM_3 = "af_param_3";
        public const string PARAM_4 = "af_param_4";
        public const string PARAM_5 = "af_param_5";
        public const string PARAM_6 = "af_param_6";
        public const string PARAM_7 = "af_param_7";
        public const string PARAM_8 = "af_param_8";
        public const string PARAM_9 = "af_param_9";
        public const string PARAM_10 = "af_param_10";
    }
}