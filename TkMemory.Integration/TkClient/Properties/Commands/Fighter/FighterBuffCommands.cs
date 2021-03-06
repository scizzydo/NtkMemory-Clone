﻿using System.Threading.Tasks;
using TkMemory.Domain.Spells;
using TkMemory.Integration.TkClient.Properties.Status.KeySpells;

namespace TkMemory.Integration.TkClient.Properties.Commands.Fighter
{
    /// <summary>
    /// Commands that are used to cast buffs that are common to both Rogues and Warriors.
    /// </summary>
    public abstract class FighterBuffCommands
    {
        #region Fields

        protected readonly FighterClient Self;

        private readonly KeySpell _enchantSpell;
        private readonly KeySpell _furySpell;
        private readonly BuffStatus _furyStatus;
        private readonly KeySpell _rageSpell;
        private readonly RageStatus _rageStatus;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Assigns buff spells from the Rogue's spell inventory.
        /// </summary>
        /// <param name="self">The game client data for the Rogue.</param>
        protected FighterBuffCommands(RogueClient self)
        {
            Self = self;
            _rageSpell = self.Spells.KeySpells.Rage;
            _rageStatus = self.Status.Rage;

            _enchantSpell = self.Spells.KeySpells.Enchant;
            _furySpell = self.Spells.KeySpells.Fury;
            _furyStatus = self.Status.Fury;
        }

        /// <summary>
        /// Assigns buff spells from the Warrior's spell inventory.
        /// </summary>
        /// <param name="self">The game client data for the Warrior.</param>
        protected FighterBuffCommands(WarriorClient self)
        {
            Self = self;
            _rageSpell = self.Spells.KeySpells.Rage;
            _rageStatus = self.Status.Rage;

            _enchantSpell = self.Spells.KeySpells.Enchant;
            _furySpell = self.Spells.KeySpells.Fury;
            _furyStatus = self.Status.Fury;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Casts an Enchantment buff.
        /// </summary>
        /// <returns>True if the spell was cast; false otherwise.</returns>
        public async Task Enchant()
        {
            await SpellCommands.CastSpell(Self, _enchantSpell);
        }

        /// <summary>
        /// Casts a Fury buff. Does not include Rage/Cunning.
        /// </summary>
        /// <returns>True if the spell was cast; false otherwise.</returns>
        public async Task<bool> Fury()
        {
            if (_rageSpell != null)
            {
                return false;
            }

            return await SpellCommands.CastAetheredSpell(Self, _furySpell, _furyStatus);
        }

        /// <summary>
        /// Casts a Rage/Cunning buff. Does not include Furies.
        /// </summary>
        /// <returns>True if the spell was cast; false otherwise.</returns>
        public async Task<bool> Rage()
        {
            if (_rageSpell == null)
            {
                return false;
            }

            return await SpellCommands.CastAetheredSpell(Self, _rageSpell, _rageStatus);
        }

        #endregion Public Methods
    }
}
