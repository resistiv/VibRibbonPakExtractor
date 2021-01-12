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
using System.Text;

namespace VibRibbonPakExtractor
{
    class Extractor
    {
        private readonly string filePath;
        private readonly BinaryReader br;

        public Extractor(string fp)
        {
            filePath = fp;
            br = new BinaryReader(File.OpenRead(filePath));
        }

        public void Extract()
        {
            // Get number of sub-files in this PAK
            int fileCount = br.ReadInt32();

            if (Program.DEBUG) Console.WriteLine($"fileCount: 0x{Convert.ToString(fileCount, 16).ToUpper()};");

            // Set up a table for reading in the table of contents (indices)
            int[] fileIndices = new int[fileCount];

            // Read in table of contents
            for (int i = 0; i < fileCount; i++)
            {
                fileIndices[i] = br.ReadInt32();
            }

            // Set up the output folder before traversing
            string outputDir = $"{Path.GetDirectoryName(filePath)}\\{Path.GetFileName(filePath)}_out\\";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Traverse the archive
            for (int i = 0; i < fileCount; i++)
            {
                // Seek to the current sub-file's index
                br.BaseStream.Seek(fileIndices[i], SeekOrigin.Begin);

                // File names have a teensy bit of complexity in handling, so let's break it down
                // The strings are null-terminated, so we read until we get a null byte
                // The files then pad out the bytes to a multiple of four to ensure that data isn't offset by an odd factor (doesn't always place nice with memory)
                byte currentChar;
                string fileName = "";
                while ((currentChar = br.ReadByte()) != 0)
                {
                    fileName += (char)currentChar;
                }

                // So, once we have the file name, we just need to skip the remaining null bytes to get to our next actual value
                br.BaseStream.Seek(3 - (fileName.Length % 4), SeekOrigin.Current);

                // Get our sub-file length
                int fileLength = br.ReadInt32();

                // If the file is nested in a subdirectory, make sure that it is created before we output
                if (fileName.IndexOf("/") != -1)
                {
                    // Parse out the file subdirectory
                    string subDir = fileName.Substring(0, fileName.LastIndexOf("/") + 1).Replace("/", "\\");

                    if (!Directory.Exists($"{outputDir}{subDir}"))
                    {
                        Directory.CreateDirectory($"{outputDir}{subDir}");
                    }
                }

                // Output file data
                File.WriteAllBytes($"{outputDir}{fileName.Replace("/", "\\")}", br.ReadBytes(fileLength));

                if (Program.DEBUG) Console.WriteLine($"fileOffset: 0x{Convert.ToString(fileIndices[i], 16).ToUpper()}; fileName: {fileName}; fileLength: 0x{Convert.ToString(fileLength, 16).ToUpper()};");
            }

            // Close the BinaryReader to release the file handle
            br.Close();

            Console.WriteLine($"Successfully extracted {fileCount} files");
        }
    }
}
