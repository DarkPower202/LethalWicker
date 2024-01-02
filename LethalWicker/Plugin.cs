using BepInEx;
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using ModelReplacement;
using BepInEx.Configuration;
using System;

//using System.Numerics;
namespace LethalWicker
{
    [BepInPlugin("Froze.LethalWicker", "LethalWicker", "1.0.0")]
    [BepInDependency("meow.ModelReplacementAPI", BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
        public static ConfigFile config;

        // Universal config options 
        public static ConfigEntry<bool> enableWickerForAllSuits { get; private set; }
        public static ConfigEntry<bool> enableWickerAsDefault { get; private set; }
        public static ConfigEntry<string> suitNamesToEnableWicker { get; private set; }

        // Wicker model specific config options
        /*
        public static ConfigEntry<float> UpdateRate { get; private set; }
        public static ConfigEntry<float> distanceDisablePhysics { get; private set; }
        public static ConfigEntry<bool> disablePhysicsAtRange { get; private set; }
        */

        private static void InitConfig()
        {
            enableWickerForAllSuits = config.Bind<bool>("Suits to Replace Settings", "Enable LethalWicker for all Suits", false, "Enable to replace every suit with LethalWicker. Set to false to specify suits");
            enableWickerAsDefault = config.Bind<bool>("Suits to Replace Settings", "Enable LethalWicker as default", false, "Enable to replace every suit that hasn't been otherwise registered with LethalWicker.");
            suitNamesToEnableWicker = config.Bind<string>("Suits to Replace Settings", "Suits to enable LethalWicker for", "Orange suit", "Enter a comma separated list of suit names. (Additionally, [Green suit, Pajama suit, Hazard suit] etc)");

            // Dynamic bones support soon
            /*
            UpdateRate = config.Bind<float>("Dynamic Bone Settings", "Update rate", 60, "Refreshes dynamic bones more times per second the higher the number");
            disablePhysicsAtRange = config.Bind<bool>("Dynamic Bone Settings", "Disable physics at range", false, "Enable to disable physics past the specified range");
            distanceDisablePhysics = config.Bind<float>("Dynamic Bone Settings", "Distance to disable physics", 20, "If Disable physics at range is enabled, this is the range after which physics is disabled.");
            */
        }
        private void Awake()
        {
            config = base.Config;
            InitConfig();
            Assets.PopulateAssets();

            // Plugin startup logic
            if (enableWickerForAllSuits.Value)
            {
                ModelReplacementAPI.RegisterModelReplacementOverride(typeof(LethalWicker));
            }
            if (enableWickerAsDefault.Value)
            {
                ModelReplacementAPI.RegisterModelReplacementDefault(typeof(LethalWicker));
            }

            var commaSepList = suitNamesToEnableWicker.Value.Split(',');
            foreach (var item in commaSepList)
            {
                ModelReplacementAPI.RegisterSuitModelReplacement(item, typeof(LethalWicker));
            }

            Harmony harmony = new Harmony("Froze.LethalWicker");
            harmony.PatchAll();
            Logger.LogInfo($"Plugin {"Froze.LethalWicker"} is loaded!");
        }
    }
    public static class Assets
    {
        public static string mainAssetBundleName = "LethalWicker";
        public static AssetBundle MainAssetBundle = null;

        private static string GetAssemblyName() => Assembly.GetExecutingAssembly().GetName().Name;
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
