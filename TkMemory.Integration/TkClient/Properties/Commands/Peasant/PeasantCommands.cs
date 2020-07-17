﻿using System.Threading.Tasks;
using TkMemory.Domain.Spells;

// ReSharper disable UnusedMember.Global

namespace TkMemory.Integration.TkClient.Properties.Commands.Peasant
{
    /// <summary>
    /// Base class for everything related to commands from the player to perform an action.
    /// Some properties/methods related to commands live in TkActivity as they are involve
    /// activity beyond the control of the player. Furthermore, commands are class-specific
    /// while command activity is not. But those properties/methods are cross-listed here for
    /// convenience due to the subject matter overlap between commands and TkActivity.
    /// </summary>
    public abstract class PeasantCommands
    {
        #region Fields

        protected readonly TkClient Self;
        private readonly KeySpell _gatewaySpell;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Assigns spells and items from the Mage's spell and item inventories.
        /// </summary>
        /// <param name="self">The game client data for the Mage.</param>
        protected PeasantCommands(MageClient self)
        {
            Self = self;

            _gatewaySpell = self.Spells.KeySpells.Gateway;

            Items = new PeasantItemCommands(self);
            Movement = new PeasantMovementCommands(self);
        }

        /// <summary>
        /// Assigns spells and items from the Poet's spell and item inventories.
        /// </summary>
        /// <param name="self">The game client data for the Poet.</param>
        protected PeasantCommands(PoetClient self)
        {
            Self = self;
            _gatewaySpell = self.Spells.KeySpells.Gateway;

            Items = new PeasantItemCommands(self);
            Movement = new PeasantMovementCommands(self);
        }

        /// <summary>
        /// Assigns spells and items from the Rogue's spell and item inventories.
        /// </summary>
        /// <param name="self">The game client data for the Rogue.</param>
        protected PeasantCommands(RogueClient self)
        {
            Self = self;
            _gatewaySpell = self.Spells.KeySpells.Gateway;

            Items = new PeasantItemCommands(self);
            Movement = new PeasantMovementCommands(self);
        }

        /// <summary>
        /// /// Assigns spells and items from the Warrior's spell and item inventories.
        /// </summary>
        /// <param name="self">The game client data for the Warrior.</param>
        protected PeasantCommands(WarriorClient self)
        {
            Self = self;
            _gatewaySpell = self.Spells.KeySpells.Gateway;

            Items = new PeasantItemCommands(self);
            Movement = new PeasantMovementCommands(self);
        }

        #endregion Constructors

        #region Enums

        /// <summary>
        /// The destinations to which one may teleport using the Gateway spell.
        /// </summary>
        public enum Gate { North, East, South, West }

        #endregion Enums

        #region Properties

        /// <summary>
        /// The default number of milliseconds to wait in between commands.
        /// </summary>
        public int DefaultCommandCooldown
        {
            get => Self.Activity.DefaultCommandCooldown;
            set => Self.Activity.DefaultCommandCooldown = value;
        }

        /// <summary>
        /// Commands for using items.
        /// </summary>
        public PeasantItemCommands Items { get; }

        /// <summary>
        /// Commands for moving the player.
        /// </summary>
        public PeasantMovementCommands Movement { get; }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Teleports the caster to a specified destination using the Gateway spell.
        /// </summary>
        /// <param name="destinationGate">The destination to which to teleport.</param>
        /// <returns>True if the spell was cast; false otherwise.</returns>
        public async Task<bool> Gateway(Gate destinationGate)
        {
            return await SpellCommands.CastSpell(Self, _gatewaySpell, true, secondaryInput: destinationGate.ToString());
        }

        /// <summary>
        /// Resets that timestamp of the most recently performed command to be the current time.
        /// This method should usually be called any time a command is performed, although there
        /// are some exceptions when no cooldown is required between commands (e.g. using a mana
        /// restoration before casting a spell).
        /// </summary>
        public void ResetCommandCooldown()
        {
            Self.Activity.ResetCommandCooldown();
        }

        /// <summary>
        /// Delays any further action until the number of milliseconds since the previous command
        /// is greater than the number of milliseconds currently assigned to the DefaultCommandCooldown
        /// property.
        /// </summary>
        public async Task WaitForCommandCooldown()
        {
            await Self.Activity.WaitForCommandCooldown();
        }

        /// <summary>
        /// Delays any further melee commands until the number of milliseconds since the previous command
        /// is greater than of the number of milliseconds set for the cooldown on melee commands.
        /// </summary>
        public async Task WaitForMeleeCooldown()
        {
            await Self.Activity.WaitForMeleeCooldown();
        }

        #endregion Public Methods
    }
}
