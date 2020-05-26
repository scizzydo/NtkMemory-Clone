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

using System;
using System.Threading.Tasks;
using TkMemory.Application.Demo;

namespace TkMemory.Application
{
    internal class Program
    {
        public static async Task Main()
        {
            try
            {
                //var demo = new MageDemo();
                //var demo = new PoetDemo();
                //var demo = new RogueDemo();
                var demo = new WarriorDemo();
                await demo.AutoHunt();
            }
            catch (Exception ex)
            {
                TkBotFactory.Terminate(ex);
            }
        }
    }
}
