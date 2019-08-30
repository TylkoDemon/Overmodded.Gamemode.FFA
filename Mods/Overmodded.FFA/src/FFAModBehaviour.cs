//
// Overmodded (SANDBOX) Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using Overmodded.Gamemode.FFA.Objects;
using Overmodded.Mods.API;
using Overmodded.Mods.API.Core.JEM;
using Overmodded.Mods.API.Core.UI;

namespace Overmodded.Gamemode.FFA
{
    /// <inheritdoc />
    internal class FFAModBehaviour : ModBehaviour
    {
        private void OnModLoaded()
        {
            var gamemode = Operations.Gamemode.CreateNewGamemodeConfiguration();
            gamemode.GamemodeName = "ffa";
            gamemode.GamemodeBehaviour = new FFAGamemode(Parent);
            Operations.Gamemode.RegisterGamemode(gamemode);
        }

        private void OnSLUIReloaded()
        {
            var panel = Operations.SLUI.SLUIRoot.Find("Panel");
            if (panel == null)
            {
                LogError("SLUI error. Panel not found.");
                return;
            }

            RespawnPanel = panel.CollectComponent<MJEMInterfaceFadeAnimation>();
            if (RespawnPanel == null)
            {
                LogError("SLUI Error. No JEMInterfaceFadeAnimation on RespawnPanel object.");
                return;
            }

            var buttonObj = panel.Find("Header/Button");
            var button = buttonObj.CollectComponent<MButton>();
            button.RegisterOnClick(new MSmartMethod(this, "OnRespawnClick"));       
            
            Log("SLUI Reloaded!");
        }

        private void OnRespawnClick()
        {
            var player = (FFAPlayerBehaviour) FFAGamemode.LocalPlayer.GetBehaviour(typeof(FFAPlayerBehaviour).FullName);
            player.RespawnRequest();
        }

        /// <summary>
        ///     Respawn panel reference.
        /// </summary>
        internal static MJEMInterfaceFadeAnimation RespawnPanel { get; private set; }
    }
}
