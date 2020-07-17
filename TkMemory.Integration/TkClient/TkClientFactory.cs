﻿// This file is part of TkMemory.

// TkMemory is free software. You can redistribute it and/or modify it
// under the terms of the GNU General Public License as published by the
// Free Software Foundation, either version 3 of the License or (at your
// option) any later version.

// TkMemory is distributed in the hope that it will be useful but WITHOUT
// ANY WARRANTY, without even the implied warranty of MERCHANTABILITY or
// FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License
// for more details.

// You should have received a copy of the GNU General Public License
// along with TkMemory. If not, please refer to:
// https://www.gnu.org/licenses/gpl-3.0.en.html

using AutoHotkey.Interop.ClassMemory;
using Serilog;
using System;
using System.Linq;
using TkMemory.Integration.TkClient.Infrastructure;
using TkMemory.Integration.TkClient.Properties.Self;

// ReSharper disable UnusedMember.Global

namespace TkMemory.Integration.TkClient
{
    /// <summary>
    /// A factory for building a path-specific TkClient object using the application
    /// memory of a generic client.
    /// </summary>
    public class TkClientFactory
    {
        #region Fields

        private static readonly StringComparer ComparisonRules = StringComparer.OrdinalIgnoreCase;
        protected readonly ClassMemory ClassMemory;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Instantiates a factory for the specified application.
        /// </summary>
        /// <param name="processId">The process ID of the application.</param>
        public TkClientFactory(int processId)
        {
            ClassMemory = new ClassMemory(processId);
            TkSelf = new TkSelf(ClassMemory);
        }

        /// <summary>
        /// Instantiates a factory for the specified application.
        /// </summary>
        /// <param name="processName">The process name of the application.</param>

        public TkClientFactory(string processName)
        {
            ClassMemory = new ClassMemory(processName);
            TkSelf = new TkSelf(ClassMemory);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets data about the player currently logged into the client.
        /// </summary>
        public TkSelf TkSelf { get; }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Builds a Mage-specific client.
        /// </summary>
        /// <returns>The Mage client that was built.</returns>
        public MageClient BuildMage()
        {
            return new MageClient(ClassMemory);
        }

        /// <summary>
        /// Builds a Poet-specific client.
        /// </summary>
        /// <returns>The Poet client that was built.</returns>

        public PoetClient BuildPoet()
        {
            return new PoetClient(ClassMemory);
        }

        /// <summary>
        /// Builds a Rogue-specific client.
        /// </summary>
        /// <returns>The Rogue client that was built.</returns>

        public RogueClient BuildRogue()
        {
            return new RogueClient(ClassMemory);
        }

        /// <summary>
        /// Builds a Warrior-specific client.
        /// </summary>
        /// <returns>The Warrior client that was built.</returns>

        public WarriorClient BuildWarrior()
        {
            return new WarriorClient(ClassMemory);
        }

        /// <summary>
        /// Gets the base path of the player currently logged into the client. This will first
        /// look at the path of the player from application member and try to correlate it to
        /// a base path. If the player's path is not a recognized value, this will look at all
        /// key spells known to the player and choose whichever base path has the most matching
        /// key spells.
        /// </summary>
        /// <returns>The base path of the player currently logged into the client.</returns>
        public TkClient.BasePath GetBasePath()
        {
            var path = TkSelf.Path;

            if (string.IsNullOrWhiteSpace(path))
            {
                // This covers cases in which a client is open but not logged in.
                return TkClient.BasePath.None;
            }

            if (Constants.Subpath.Mage.Contains(path, ComparisonRules))
            {
                return TkClient.BasePath.Mage;
            }

            if (Constants.Subpath.Poet.Contains(path, ComparisonRules))
            {
                return TkClient.BasePath.Poet;
            }

            if (Constants.Subpath.Rogue.Contains(path, ComparisonRules))
            {
                return TkClient.BasePath.Rogue;
            }

            if (Constants.Subpath.Warrior.Contains(path, ComparisonRules))
            {
                return TkClient.BasePath.Warrior;
            }

            Log.Warning($"Could not determine base path for subpath \"{path}\".");
            return TkClient.BasePath.None;
        }

        #endregion Public Methods

        #region Private Methods

        [Obsolete("All existing subpaths are now mapped to their corresponding base paths, so this method should never be needed.")]
        private TkClient.BasePath GetBasePathByKeySpellCount()
        {
            Log.Warning($"Subpath \"{TkSelf.Path}\" is not currently associated with a base path. Base path will therefore be determined from your spell inventory. You may see a few lines of unusual log activity during this process, but it can be ignored. Please send the developer your base path and subpath to have this corrected in a future release.");

            var warrior = BuildWarrior();
            var rogue = BuildRogue();
            var mage = BuildMage();
            var poet = BuildPoet();

            var warriorKeySpells = warrior.Spells.KeySpells;
            var rogueKeySpells = rogue.Spells.KeySpells;
            var mageKeySpells = mage.Spells.KeySpells;
            var poetKeySpells = poet.Spells.KeySpells;

            var matches = new[]
            {
                warriorKeySpells.GetType().GetProperties().Count(property => property.GetValue(warriorKeySpells, null) != null),
                rogueKeySpells.GetType().GetProperties().Count(property => property.GetValue(rogueKeySpells, null) != null),
                mageKeySpells.GetType().GetProperties().Count(property => property.GetValue(mageKeySpells, null) != null),
                poetKeySpells.GetType().GetProperties().Count(property => property.GetValue(poetKeySpells, null) != null)
            };

            return (TkClient.BasePath)matches.ToList().IndexOf(matches.Max());
        }

        #endregion Private Methods
    }
}
