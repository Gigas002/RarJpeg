# RarJpeg

Very small and simple project on **WPF** for creating `*.%archiveExtension%.%imageExtension%` files. Solution is build in **VS2019 (16.1.3)**, **.NET Framework 4.8**, targeting **Windows x64** systems.

I know, that GUI is kind of useless, since you can do it with one line in console: `copy /b image1.jpg+something.rar image2.jpg`, but still I wanted create it. Why not?

[![Build status](https://ci.appveyor.com/api/projects/status/7wvh1l5kv3bi9iae?svg=true)](https://ci.appveyor.com/project/Gigas002/rarjpeg)

## Current version

You can get stable pre-built binaries here: [![Release](https://img.shields.io/github/release/Gigas002/RarJpeg.svg)](https://github.com/Gigas002/RarJpeg/releases/latest). This project supports [SemVer 2.0.0](https://semver.org/) (template is `{MAJOR}.{MINOR}.{PATCH}.{BUILD}`).

Information about changes since previous releases can be found in [changelog](https://github.com/Gigas002/RarJpeg/blob/master/CHANGELOG.md).

Previous versions can be found on [releases](https://github.com/Gigas002/RarJpeg/releases) and [branches](https://github.com/Gigas002/RarJpeg/branches) pages.

## Requirements

- Windows 7 x64 and newer;
- [.NET Framework 4.8](<https://dotnet.microsoft.com/download/dotnet-framework/net48>);

## Usage

Just select any image as container and any archive as… well, archive. Then, select the output file’s path (do not write any extensions here please) and press **Start**.

![Main page](Screenshots/MainPage.png)

## Dependencies

- [Caliburn.Micro](<https://www.nuget.org/packages/Caliburn.Micro>) – 3.2.0;
- [ControlzEx](<https://www.nuget.org/packages/ControlzEx>) – 3.0.2.4 (required by `MaterialDesignThemes`);
- [MahApps.Metro](<https://www.nuget.org/packages/MahApps.Metro>) – 1.6.5 (required by `MaterialDesignThemes`);
- [MaterialDesignColors](<https://www.nuget.org/packages/MaterialDesignColors>) – 1.1.3 (required by `MaterialDesignThemes` and `MaterialDesignExtensions`);
- [MaterialDesignThemes](<https://www.nuget.org/packages/MaterialDesignThemes>) – 2.5.1;
- [MaterialDesignExtensions](<https://www.nuget.org/packages/MaterialDesignExtensions/>) – 2.6.0;
- [SharpZipLib](<https://www.nuget.org/packages/SharpZipLib/>) – 1.1.0;
- [System.Threading.Tasks.Extensions](<https://www.nuget.org/packages/System.Threading.Tasks.Extensions>) – 4.5.2;
- [System.Runtime.CompilerServices.Unsafe](<https://www.nuget.org/packages/System.Runtime.CompilerServices.Unsafe>) – 4.5.2 (required by `System.Threading.Tasks.Extensions`);

## Localization

There’s really not much to localize, but feel free to participate. Currently, only `EN-US` and `RU-RU` are supported. All the localizable strings are located into `Strings.rexs`.

## Contributing

Even though there’s nothing to do, feel free to contribute, make forks, change some code, add [issues](https://github.com/Gigas002/RarJpeg/issues), etc.
