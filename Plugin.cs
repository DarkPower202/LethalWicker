using BepInEx;
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using ModelReplacement;
using BepInEx.Configuration;
using System;
using System.Xml.Linq;

namespace LethalWicker
{
    [BepInPlugin("Froze.LethalWicker", "Lethal Wicker", "2.0.1")]
    [BepInDependency("meow.ModelReplacementAPI", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("x753.More_Suits", BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
        public static ConfigFile config;

        public static ConfigEntry<bool> enableModelForAllSuits { get; private set; }
        public static ConfigEntry<bool> enableModelAsDefault { get; private set; }
        public static ConfigEntry<string> suitNamesToEnableModel { get; private set; }

        private static void InitConfig()
        {
            enableModelForAllSuits = config.Bind<bool>("Suits to Replace Settings", "Enable Red Wicker for all Suits", false, "Enable to model replace every suit. Set to false to specify suits");
            enableModelAsDefault = config.Bind<bool>("Suits to Replace Settings", "Enable Red Wicker as default", false, "Enable to model replace every suit that hasn't been otherwise registered.");
            suitNamesToEnableModel = config.Bind<string>("Suits to Replace Settings", "Suits to enable Model for Red Wicker", "SuitNameExample", "Enter a comma separated list of suit names.(Additionally, [Green suit,Pajama suit,Hazard suit])");
        }

        private void Awake()
        {
            config = base.Config;
            InitConfig();
            Assets.PopulateAssets();

            if (enableModelForAllSuits.Value)
            {
                ModelReplacementAPI.RegisterModelReplacementOverride(typeof(MRLETHALWICKERRED));

            }
            if (enableModelAsDefault.Value)
            {
                ModelReplacementAPI.RegisterModelReplacementDefault(typeof(MRLETHALWICKERRED));

            }
            var commaSepList = suitNamesToEnableModel.Value.Split(',');
            foreach (var item in commaSepList)
            {
                ModelReplacementAPI.RegisterSuitModelReplacement(item, typeof(MRLETHALWICKERRED));
            }

            ModelReplacementAPI.RegisterSuitModelReplacement("Red Wicker", typeof(MRLETHALWICKERRED));
            ModelReplacementAPI.RegisterSuitModelReplacement("Blue Wicker", typeof(MRLETHALWICKERBLUE));
            ModelReplacementAPI.RegisterSuitModelReplacement("Green Wicker", typeof(MRLETHALWICKERGREEN));

            Harmony harmony = new Harmony("Froze.LethalWicker");
            harmony.PatchAll();
            Logger.LogInfo($"Plugin {"Froze.LethalWicker"} is loaded!");
        }
    }

    public static class Assets
    {
        // Replace mbundle with the Asset Bundle Name from your unity project 
        public static string mainAssetBundleName = "LethalWicker";
        public static AssetBundle MainAssetBundle = null;

        private static string GetAssemblyName() => Assembly.GetExecutingAssembly().GetName().Name.Replace(" ","_");
        public static void PopulateAssets()
        {
            if (MainAssetBundle == null)
            {
                Console.WriteLine(GetAssemblyName() + "." + mainAssetBundleName);
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(GetAssemblyName() + "." + mainAssetBundleName))
                {
                    MainAssetBundle = AssetBundle.LoadFromStream(assetStream);
                }
            }
        }
    }
}