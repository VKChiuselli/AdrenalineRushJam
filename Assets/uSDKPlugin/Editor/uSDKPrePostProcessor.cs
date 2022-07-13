using System.IO;
using UnityEditor;
#if UNITY_IOS
    using UnityEditor.iOS.Xcode;
    using UnityEditor.iOS.Xcode.Extensions;
#endif
using UnityEngine;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace MyAppFree.uSDK.Editor
{
    public class uSDKPrePostProcessor : IPreprocessBuildWithReport
    {
        private const string uSDKProjectPropertiesTemplateName = "uSDKProjectPropertiesTemplate";
        private const string uSDKAndroidManifestTemplateName = "uSDKAndroidManifestTemplate";
        private const string androidManifestURISchemeTag = "__uSDKURIScheme__";
        private const string androidManifestSchemeHostTag = "__uSDKSchemeHost__";

        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
#if UNITY_ANDROID
            USDKConfig uSDKConfig = GetUSDKConfig(BuildTarget.Android);
            if (uSDKConfig == null)
            {
                Debug.LogError("uSDKPrePostProcessor interrupted.");
                return;
            }

            string[] manifestTemplateGUIDs = AssetDatabase.FindAssets(uSDKAndroidManifestTemplateName);
            if (manifestTemplateGUIDs.Length == 0)
            {
                Debug.LogError("uSDKPrePostProcessor - Template manifest (" + uSDKAndroidManifestTemplateName + ") not found. Preprocess interrupted.");
                return;
            }

            string manifestTemplatePath = AssetDatabase.GUIDToAssetPath(manifestTemplateGUIDs[0]);
            
            string manifest = File.ReadAllText(manifestTemplatePath);
            if (manifest.Contains(androidManifestURISchemeTag))
            {
                manifest = manifest.Replace(androidManifestURISchemeTag, uSDKConfig.AndroidURIScheme);
            }
            else
            {
                Debug.LogWarning("uSDKPrePostProcessor - No tag " + androidManifestURISchemeTag + " found in " + manifestTemplatePath);
            }

            if (manifest.Contains(androidManifestSchemeHostTag))
            {
                manifest = manifest.Replace(androidManifestSchemeHostTag, uSDKConfig.AndroidSchemeHost);
            }
            else
            {
                Debug.LogWarning("uSDKPrePostProcessor - No tag " + androidManifestSchemeHostTag + " found in " + manifestTemplatePath);
            }

            string[] projectPropertiesTemplateGUIDs = AssetDatabase.FindAssets(uSDKProjectPropertiesTemplateName);
            if (projectPropertiesTemplateGUIDs.Length == 0)
            {
                Debug.LogError("uSDKPrePostProcessor - Template project properties (" + uSDKProjectPropertiesTemplateName + ") not found. Preprocess interrupted.");
                return;
            }

            string projectPropertiesTemplatePath = AssetDatabase.GUIDToAssetPath(projectPropertiesTemplateGUIDs[0]);
            string projectProperties = File.ReadAllText(projectPropertiesTemplatePath);
            
            string finalPath = "Assets" + Path.DirectorySeparatorChar + "Plugins" + Path.DirectorySeparatorChar + "Android" + Path.DirectorySeparatorChar + "uSDKManifest.androidlib" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(finalPath))
            {
                Directory.CreateDirectory(finalPath);
            }

            File.WriteAllText(finalPath + "AndroidManifest.xml", manifest);
            File.WriteAllText(finalPath + "project.properties", projectProperties);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }

        private static USDKConfig GetUSDKConfig(BuildTarget buildTarget)
        {
            string[] uSDKConfigGUIDs = AssetDatabase.FindAssets("t:" + typeof(USDKConfig).Name);
            if (uSDKConfigGUIDs.Length == 0)
            {
                Debug.LogError("uSDKPrePostProcessor - No USDKConfig found. Make sure to create it through the MenuItem, and populate its data.");
                return null;
            }

            string uSDKConfigPath = AssetDatabase.GUIDToAssetPath(uSDKConfigGUIDs[0]);
            if (uSDKConfigGUIDs.Length > 1)
            {
                Debug.LogWarning("uSDKPrePostProcessor - More than one USDKConfig found, there should be only one. The one that will be used is: " + uSDKConfigPath + ", make sure it is the one you wanted to be used, and delete the other ones.");
            }

            USDKConfig uSDKConfig = AssetDatabase.LoadAssetAtPath<USDKConfig>(uSDKConfigPath);
            if (string.IsNullOrWhiteSpace(uSDKConfig.IOSAssociatedDomain) && buildTarget == BuildTarget.iOS)
            {
                Debug.LogError("uSDKPrePostProcessor - USDKConfig.IOSAssociatedDomain is empty.");
                return null;
            }
            else if (string.IsNullOrWhiteSpace(uSDKConfig.AndroidURIScheme) && buildTarget == BuildTarget.Android)
            {
                Debug.LogError("uSDKPrePostProcessor - USDKConfig.AndroidURIScheme is empty.");
                return null;
            }

            if (string.IsNullOrWhiteSpace(uSDKConfig.AndroidSchemeHost) && buildTarget == BuildTarget.Android)
            {
                Debug.LogError("uSDKPrePostProcessor - USDKConfig.AndroidSchemeHost is empty.");
                return null;
            }

            return uSDKConfig;
        }

        [UnityEditor.Callbacks.PostProcessBuild]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
        {
            if (buildTarget == BuildTarget.iOS)
            {
#if UNITY_IOS
                string projPath = PBXProject.GetPBXProjectPath(path);

                PBXProject proj = new PBXProject();
                proj.ReadFromString(File.ReadAllText(projPath));

                string targetGuid = proj.GetUnityMainTargetGuid();

                USDKConfig uSDKConfig = GetUSDKConfig(buildTarget);
                if (uSDKConfig == null)
                {
                    Debug.LogError("uSDKPrePostProcessor interrupted.");
                }

                SetupProjectCapabilities(projPath, targetGuid, uSDKConfig);
                SetupSwiftProperties(projPath);
                SetupFrameworkEmbed(projPath);
                SetupInfoPlist(path, uSDKConfig);
#endif
            }
        }

        private static void SetupProjectCapabilities(string projPath, string targetGuid, USDKConfig uSDKConfig)
        {
#if UNITY_IOS
            ProjectCapabilityManager pcm = new ProjectCapabilityManager(projPath, "Entitlements.entitlements", null, targetGuid);
            pcm.AddAssociatedDomains(new string[] { uSDKConfig.IOSAssociatedDomain });
            pcm.WriteToFile();
#endif
        }

        private static void SetupSwiftProperties(string projPath)
        {
#if UNITY_IOS
            PBXProject proj = new PBXProject();
            proj.ReadFromString(File.ReadAllText(projPath));

            string targetGuid = proj.GetUnityMainTargetGuid();

            proj.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");
            proj.AddBuildProperty(targetGuid, "LD_RUNPATH_SEARCH_PATHS", "@executable_path/Frameworks $(PROJECT_DIR)/lib/$(CONFIGURATION) $(inherited)");
            proj.AddBuildProperty(targetGuid, "FRAMERWORK_SEARCH_PATHS", "$(inherited) $(PROJECT_DIR) $(PROJECT_DIR)/Frameworks");
            proj.AddBuildProperty(targetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");
            proj.AddBuildProperty(targetGuid, "DYLIB_INSTALL_NAME_BASE", "@rpath");
            proj.AddBuildProperty(targetGuid, "LD_DYLIB_INSTALL_NAME", "@executable_path/../Frameworks/$(EXECUTABLE_PATH)");
            proj.AddBuildProperty(targetGuid, "DEFINES_MODULE", "YES");
            proj.AddBuildProperty(targetGuid, "SWIFT_VERSION", "5.0");
            proj.AddBuildProperty(targetGuid, "COREML_CODEGEN_LANGUAGE", "Swift");

            proj.WriteToFile(projPath);
#endif
        }

        private static void SetupFrameworkEmbed(string projPath)
        {
#if UNITY_IOS
            PBXProject proj = new PBXProject();
            proj.ReadFromString(File.ReadAllText(projPath));

            string targetGuid = proj.GetUnityMainTargetGuid();

            string frameworkPath = "uSDKPlugin/Plugins/iOS";
            string frameworkFilename = "uSDK.framework";
            string framework = Path.Combine(frameworkPath, frameworkFilename);
            string fileGuid = proj.AddFile(framework, "Frameworks/" + framework, PBXSourceTree.Sdk);
            PBXProjectExtensions.AddFileToEmbedFrameworks(proj, targetGuid, fileGuid);

            proj.WriteToFile(projPath);
#endif
        }

        private static void SetupInfoPlist(string path, USDKConfig uSDKConfig)
        {
#if UNITY_IOS
            var plistPath = Path.Combine(path, "Info.plist");
            var plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

            PlistElementDict rootDict = plist.root;

            PlistElement urlSchemesArrayElement = rootDict["CFBundleURLTypes"];
            if (urlSchemesArrayElement == null)
            {
                urlSchemesArrayElement = new PlistElementArray();
                rootDict["CFBundleURLTypes"] = urlSchemesArrayElement;
            }
            PlistElementArray urlSchemesArray = urlSchemesArrayElement.AsArray();
            PlistElementDict mafUrlSchemeDict = urlSchemesArray.AddDict();
            mafUrlSchemeDict["CFBundleURLName"] = new PlistElementString("$(PRODUCT_BUNDLE_IDENTIFIER)");
            PlistElementArray mafUrlSchemes = new PlistElementArray();
            mafUrlSchemes.AddString("$(PRODUCT_BUNDLE_IDENTIFIER)");
            mafUrlSchemeDict["CFBundleURLSchemes"] = mafUrlSchemes;

            PlistElement networkItemsArraylement = rootDict["SKAdNetworkItems"];
            if (networkItemsArraylement == null)
            {
                networkItemsArraylement = new PlistElementArray();
                rootDict["SKAdNetworkItems"] = networkItemsArraylement;
            }
            PlistElementArray networkItemsArray = networkItemsArraylement.AsArray();
            PlistElementDict mafNetworkItemDict = networkItemsArray.AddDict();
            mafNetworkItemDict["SKAdNetworkIdentifier"] = new PlistElementString("cad8qz2s3j.skadnetwork");

            rootDict["NSUserTrackingUsageDescription"] = new PlistElementString("By clicking on ALLOW you will avoid any Advertising that is not personalized for you. Your Advertising ID will be managed in a completely anonymous way. Thanks");

            File.WriteAllText(plistPath, plist.WriteToString());
#endif
        }
    }
}