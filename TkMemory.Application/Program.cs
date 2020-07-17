﻿// This file is part of TkMemory.Application.

// TkMemory.Application is free software. You can redistribute it and/or
// modify it under the terms of the GNU General Public License as published
// by the Free Software Foundation, either version 3 of the License or (at
// your option) any later version.

// TkMemory.Application is distributed in the hope that it will be useful
// but WITHOUT ANY WARRANTY, without even the implied warranty of MERCHANTABILITY
// or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for
// more details.

// You should have received a copy of the GNU General Public License
// along with TkMemory.Application. If not, please refer to:
// https://www.gnu.org/licenses/gpl-3.0.en.html

using System;

namespace TkMemory.Application
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        internal static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.Run(new TkMemoryForm());
        }
    }
}
