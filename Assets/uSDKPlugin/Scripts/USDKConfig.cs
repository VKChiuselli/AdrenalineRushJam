using UnityEngine;

namespace MyAppFree.uSDK
{
    [CreateAssetMenu(fileName = "uSDK Config", menuName = "MyAppFree/uSDK Config")]
    public class USDKConfig : ScriptableObject
    {
        [SerializeField]
        private string appsFlyerKey;
        public string AppsFlyerKey => appsFlyerKey;

        [SerializeField]
        private string appleId;
        public string AppleId => appleId;

        [SerializeField]
        private string androidURIScheme;
        public string AndroidURIScheme => androidURIScheme;

        [SerializeField]
        private string androidSchemeHost;
        public string AndroidSchemeHost => androidSchemeHost;

        [SerializeField]
        private string iOSAssociatedDomain;
        public string IOSAssociatedDomain => iOSAssociatedDomain;
    }
}