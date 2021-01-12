/*-----------------------------------------------------------------------------
Copyright (c) 2020 Kai NeSmith

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
-----------------------------------------------------------------------------*/

using System;
using System.IO;
using VibRibbonPakExtractor;

namespace VibRibbonPakExtractor
{
    class Program
    {
        private const string VERSION = "1.1.0";
        private const string USAGE = "Usage: vbpakext <PAK file path>";

        // Debug flag to enable/disable verbose output describing file entries; I should probably add an argument for this at some point but it's currently 1:00 AM and I'm tired :)
        public const bool DEBUG = false;

        static void Main(string[] args)
        {
            Console.WriteLine($"\nVibRibbonPakExtractor v{VERSION} (c) 2020 Kai NeSmith");

            if (args.Length != 1 || !File.Exists(args[0]))
            {
                WriteUsage();
            }

            string fullFilePath = Path.GetFullPath(args[0]);
            Extractor ext = new Extractor(fullFilePath);
            ext.Extract();
        }

        static void WriteUsage()
        {
            Console.WriteLine(USAGE);
            Environment.Exit(0);
        }
    }
}
