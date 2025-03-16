using BepInEx;
using RoR2;
using R2API.Utils;

namespace NegativeBossHealthTweak
{
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "LordVGames";
        public const string PluginName = "NegativeBossHealthTweak";
        public const string PluginVersion = "1.0.1";
        public void Awake()
        {
            Log.Init(Logger);
            IL.RoR2.UI.HUDBossHealthBarController.LateUpdate += Main.HUDBossHealthBarController_LateUpdate;
        }
    }
}