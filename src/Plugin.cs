using System;
using BepInEx;
using UnityEngine;
using SlugBase.Features;
using static SlugBase.Features.FeatureTypes;
using BepInEx.Logging;

namespace Botanist
{
    [BepInPlugin(MOD_ID, "The Botanist", "0.1.0")]
    class Plugin : BaseUnityPlugin
    {
        private const string MOD_ID = "qtpi.botanist";

        internal static ManualLogSource s_logger;

        public void OnEnable()
        {
            Plugin.s_logger = base.Logger;

            try
            {
                On.RainWorld.OnModsInit += Extras.WrapInit(LoadResources);
            }
            catch (Exception ex)
            {
                s_logger.LogError(ex);
            }
        }

        // Load any resources, such as sprites or sounds
        private void LoadResources(RainWorld rainWorld)
        {
        }

    }
}