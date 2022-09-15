This tool is outdated!
======================
**Please use [VibRipper](https://github.com/resistiv/VibRipper) or [KNFE](https://github.com/resistiv/KNFE) instead!**

Vib-Ribbon PAK Extractor
========================
A simple command-line application for extracting files that are contained in PAK (.PAK) files found within the PS1 title Vib-Ribbon / ビブリボン. This tool works with both the Japanese & European versions of the game, and is untested on the PSN version.
This project is intended as a preliminary dive into reverse engineering binary file formats and using the resulting information to create a tangible product, and is to be presented as part of a senior project.

PAK Format
----------
PAK files store a set number of files that are utilized within the game in an uncompressed format. The file layout is as follows:
```
// Table of Contents
    uint32 {4}          File count
    // For each file
        uint32 {4}      File index

// File Body
    // For each file
        char {x}        Variable-length, null-terminated file name (padded to nearest 4 bytes)
        uint32 {4}      File length
        byte {x}        File data
```
Additionally, the file names include nested directories, which this program handles & replicates natively.

License
-------
This software and source code are offered under the MIT License.
For more information, [click here](https://opensource.org/licenses/MIT).
