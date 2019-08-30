//
// Overmodded (SANDBOX) Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using Overmodded.Mods.API.Core.Gameplay;
using Overmodded.Mods.API.Core.Gameplay.Player;
using Overmodded.Mods.API.Core.Gameplay.Teams;
using Overmodded.Mods.API.Core.Networking;

namespace Overmodded.Gamemode.FFA.Objects
{
    /// <inheritdoc />
    internal class FFAPlayerBehaviour : MPlayerBehaviour
    {
        private byte MessageIndex_OnRespawnRequest;

        private void RegisterNetworkMessages()
        {
            // here we can register network messages!
            MessageIndex_OnRespawnRequest = RegisterNetworkMessage(OnRespawnRequest);
        }

        private void OnTeamUpdated(MTeamInstance teamInstance)
        {
            if (teamInstance == null)
                return;

            Operations.Chat.SendServerMessageAll($"{parent.playerName} joins team `{teamInstance.TeamName}`");

            // update perspective
            FFAGamemode.Instance.parent.BroadcastForcedCVarChange(parent, "game.tpp", false);
            
            // TEMP
            if (teamInstance.IsDefault)
            {
               parent.SetSpectateMode(true);
            }
            else if (parent.isEntityAlive)
            {
                parent.SetSpectateMode(false);
            }
        }

        private void ClientOnEntityKilled(MDamageInfo damageInfo)
        {
            Log($"Entity killed owner? -> {IsOwner}");
            if (!IsOwner) return;
            if (FFAModBehaviour.RespawnPanel == null)
            {
                LogError("No Respawn Panel!");
                return;
            }

            Log("Set respawn true");
            FFAModBehaviour.RespawnPanel.SetActive(true);
        }

        private void ClientOnEntityRespawned()
        {
            if (!IsOwner) return;
            if (FFAModBehaviour.RespawnPanel == null)
            {
                LogError("No Respawn Panel!");
                return;
            }

            Log("Set respawn false");
            FFAModBehaviour.RespawnPanel.SetActive(false);
        }

        private void ServerOnEntityKilled(MDamageResult damageResult)
        {
            Operations.Chat.SendServerMessageAll($"{parent.playerName} died.");

            parent.SetSpectateMode(true);
        }

        private void ServerOnEntityRespawned()
        {
            Operations.Chat.SendServerMessageAll($"{parent.playerName} respawned.");

            parent.SetSpectateMode(false);
        }

        /// <summary>
        ///     Sends a respawn request to server.
        /// </summary>
        internal void RespawnRequest()
        {
            Log("Sending Respawn Request");
            SendClientMessage(MessageIndex_OnRespawnRequest).ApplyAndSend();
        }

        private void OnRespawnRequest(MNetworkIncomingMessage incomingMessage)
        {
            Log("Respawn request Received.");
            parent.RestartCharacter(true);
        }
    }
}
