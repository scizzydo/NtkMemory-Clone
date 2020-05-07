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

using NUnit.Framework;
using System;
using System.Configuration;
using TkMemory.Integration.TkClient;

namespace TkMemory.Tests.Integration
{
    [TestFixture]
    internal class SendTests
    {
        [Explicit]
        [TestCase("{Esc}{v}{Home}{Esc}")]
        public void SendKeystrokes(string keystrokes)
        {
            var clients = new ActiveClients(ConfigurationManager.AppSettings["ProcessName"]);
            var tkMemory = clients.GetPoet();
            tkMemory.Send(keystrokes);
            Console.WriteLine($"Sent keystrokes: \"{keystrokes}\"");
        }
    }
}
