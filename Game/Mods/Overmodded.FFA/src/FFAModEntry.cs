//
// Overmodded (SANDBOX) Source
//
// Copyright (c) 2019 ADAM MAJCHEREK ALL RIGHTS RESERVED
//

using Overmodded.Mods.API;

namespace Overmodded.Gamemode.FFA
{
    /// <inheritdoc />
    public class FFAModEntry : ModEntry
    {
        /// <inheritdoc />
        protected override void OnLoad()
        {
            RegisterBehaviour(new FFAModBehaviour());
        }
    }
}
