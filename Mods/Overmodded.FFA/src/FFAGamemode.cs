//
// Overmodded (SANDBOX) Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using Overmodded.Gamemode.FFA.Objects;
using Overmodded.Mods;
using Overmodded.Mods.API.Core.Gameplay;
using Overmodded.Mods.API.Core.Gameplay.Gamemode;
using Overmodded.Mods.API.Core.Gameplay.Player;
using Overmodded.Mods.API.Core.Gameplay.Teams;
using System.Collections.Generic;

namespace Overmodded.Gamemode.FFA
{
    /// <inheritdoc />
    internal class FFAGamemode : GamemodeBehaviour
    {
        public FFAGamemode(ModInstance parent) : base(parent)
        {
            Instance = this;
        }

        private void OnGameWorldLoaded()
        {
            // apply cVars
            parent.EditLocalCVar("game.allow_tpp", false);

            // register team
            FFATeam = Operations.Teams.RegisterTeam(new MTeamConfiguration
            {
                TeamName = "ffa",
                TeamLocaleName = "FFA",
                NeedPlayersToStart = 1,

                IsDefault = false
            });

            // and spectate...
            Operations.Teams.RegisterTeam(new MTeamConfiguration
            {
                TeamName = "spectators",
                TeamLocaleName = "SPECTATORS",
                NeedPlayersToStart = 0,

                IsDefault = true
            });
        }

        private void OnGameWorldUnloaded()
        {
            if (FFAModBehaviour.RespawnPanel != null)
                FFAModBehaviour.RespawnPanel.SetActive(false, true);
        }

        private void OnEntityReceived(MEntity entity, bool wasNatural)
        {
            if (entity.IsPlayer())
            {
                // A player entity has been received!
                // Register behaviour to control transformation in to props!
                var player = (FFAPlayerBehaviour) entity.RegisterBehaviour(parent, typeof(FFAPlayerBehaviour).FullName);
                // player.parent.SetServerTeam(FFATeam);

                AllPlayers.Add(player);
            }
        }

        private void OnEntityLost(MEntity entity, bool wasNatural)
        {
            if (entity.IsPlayer())
            {
                var player = (FFAPlayerBehaviour)entity.GetBehaviour(typeof(FFAPlayerBehaviour).FullName);
                AllPlayers.Remove(player);
            }
        }

        private void OnLocalPlayerReceived(MPlayerEntity player)
        {
            // Here we are receiving reference to a local player 
            // Useful for sending player's input to server.
            LocalPlayer = player;
        }

        public static MPlayerEntity LocalPlayer { get; private set; }
        public static MTeamInstance FFATeam { get; private set; }

        public static FFAGamemode Instance { get; private set; }

        public static List<FFAPlayerBehaviour> AllPlayers { get; } = new List<FFAPlayerBehaviour>();
    }
}
