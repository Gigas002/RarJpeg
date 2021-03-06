# RarJpeg

Very small and simple project on **WPF** for creating `*.%archiveExtension%.%imageExtension%` files. Solution is build in **VS2019 (16.4.5)**, **.NET Core 3.1**, targeting **Windows** systems. Project can also be built in **VSCode** (**1.42.1**+) with [omnisharp-vscode](https://github.com/OmniSharp/omnisharp-vscode) extension (**1.21.11**+).

I know, that GUI is kind of useless, since you can do it with one line in console: `copy /b image1.jpg+something.rar image2.jpg`, but still I wanted create it. Why not?

[![Build status](https://ci.appveyor.com/api/projects/status/7wvh1l5kv3bi9iae/branch/master?svg=true)](https://ci.appveyor.com/project/Gigas002/rarjpeg) [![Actions Status](https://github.com/Gigas002/RarJpeg/workflows/.NET%20Core%20CI/badge.svg)](https://github.com/Gigas002/RarJpeg/actions)

## Current version

You can get stable pre-built binaries here: [![Release](https://img.shields.io/github/release/Gigas002/RarJpeg.svg)](https://github.com/Gigas002/RarJpeg/releases/latest). This project supports [SemVer 2.0.0](https://semver.org/) (template is `{MAJOR}.{MINOR}.{PATCH}.{BUILD}`).

Information about changes since previous releases can be found in [changelog](https://github.com/Gigas002/RarJpeg/blob/master/CHANGELOG.md).

Previous versions can be found on [releases](https://github.com/Gigas002/RarJpeg/releases) and [branches](https://github.com/Gigas002/RarJpeg/branches) pages.

## Requirements

- Windows 7 and newer;

## Usage

Just select **any image as container** and **any archive** as… well, archive. Then, select the **output file’s path** (*don’t write any extensions here please*) and press **Start**.

![Main page](Screenshots/MainPage.png)

## Dependencies

- [Caliburn.Micro](<https://www.nuget.org/packages/Caliburn.Micro>) – 4.0.105-alpha;
- [MaterialDesignColors](<https://www.nuget.org/packages/MaterialDesignColors>) – 1.2.2;
- [MaterialDesignThemes](<https://www.nuget.org/packages/MaterialDesignThemes>) – 3.0.1;
- [MaterialDesignExtensions](<https://www.nuget.org/packages/MaterialDesignExtensions/>) – 3.0.0;
- [SharpZipLib](<https://www.nuget.org/packages/SharpZipLib/>) – 1.2.0;

## Localization

There’s really not much to localize, but feel free to participate! Currently, only **English** and **Russian** are supported. All the localizable strings are located into `Localization/Strings.rexs`.

## Contributing

Even though there’s nothing to do, feel free to contribute, make forks, change some code, add [issues](https://github.com/Gigas002/RarJpeg/issues), etc.
