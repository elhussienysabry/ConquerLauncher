# Conquer Launcher

A modern, customizable game launcher for Conquer Online with automatic update capabilities.

![License](https://img.shields.io/badge/license-MIT-blue.svg)
![.NET](https://img.shields.io/badge/.NET-Framework%204.7.2-purple.svg)

## Features

- 🚀 **Automatic Updates** - Downloads and installs game updates automatically
- 🎨 **Customizable UI** - Easily customize background and button images
- 🔗 **Social Integration** - Built-in links to Discord, Facebook, and registration
- ⚙️ **Flexible Settings** - Configure server URL, client path, and more
- 📊 **Progress Tracking** - Real-time download progress with speed indicator
- 🔄 **Version Control** - Automatic version checking and update management

## Quick Start

### For Players

1. Download the latest release from the [Releases](../../releases) page
2. Extract the launcher to your game folder
3. Run `ConquerLauncher.exe`
4. Click the Start button to play!

### For Server Owners

1. Set up your server files following the [Server Setup Guide](#server-setup)
2. Configure the launcher settings in `launcher_settings.json`
3. Distribute the launcher to your players

## Configuration

The launcher uses `launcher_settings.json` for configuration:

```json
{
  "ServerURL": "https://yourserver.com/",
  "ClientPath": "Clash.exe",
  "CheckForUpdates": true,
  "BackgroundImage": "data/img/bg.png",
  "ButtonImages": {
    "Start": "data/img/start.png",
    "Settings": "data/img/settings.png",
    "Close": "data/img/close.png",
    "Discord": "data/img/discord.png",
    "Facebook": "data/img/facebook.png",
    "Register": "data/img/register.png"
  },
  "SocialLinks": {
    "Discord": "https://discord.gg/yourserver",
    "Facebook": "https://facebook.com/yourpage",
    "Register": "https://yourserver.com/register"
  },
  "WindowSize": {
    "Width": 800,
    "Height": 600
  }
}
```

## Server Setup

### File Structure

Place your server files in the `ServerFiles` directory:

```
ServerFiles/
├── version.txt          # Current version number (e.g., 1.0.0)
├── files.json          # List of files to update
├── news.html           # News/announcements (optional)
└── files/              # Your game files
    └── Clash.exe
```

### files.json Format

```json
[
  {
    "FileName": "Clash.exe",
    "RelativePath": "",
    "DownloadURL": "https://yourserver.com/files/Clash.exe"
  },
  {
    "FileName": "data.dat",
    "RelativePath": "data",
    "DownloadURL": "https://yourserver.com/files/version.dat"
  }
]
```

### Using Helper Scripts

The `ServerFiles` folder contains PowerShell scripts to help manage updates:

#### Generate files.json

```powershell
cd ServerFile
.\Generate-FilesJson.ps1
```

This will:
- Scan the `files` folder
- Generate `files.json` automatically
- Preserve existing URLs

#### Quick Update

```powershell
.\Quick-Update.ps1 -Version "1.0.1"
```

This will:
- Update version.txt
- Regenerate files.json
- Prepare files for upload

See [ServerFiles_Examples/HOW_TO_USE_SCRIPTS.md](ServerFiles/HOW_TO_USE_SCRIPTS.md) for detailed instructions.

## Customization

### Background Image

Replace `data/img/bg.png` with your custom background (recommended: 800x600px or larger)

### Buttons

Replace the button images in `data/img/` with your designs:
- `start.png` - Start game button
- `settings.png` - Settings button
- `close.png` - Close button
- `discord.png` - Discord link button
- `facebook.png` - Facebook link button
- `register.png` - Register button

### Colors and UI

Edit `Form1.cs` to customize:
- Progress bar colors
- Text colors and fonts
- Window decorations

## Building from Source

### Requirements

- Visual Studio 2019 or later
- .NET Framework 4.7.2
- Newtonsoft.Json (installed via NuGet)

### Build Steps

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/ConquerLauncher.git
   ```

2. Open `ConquerLauncher.sln` in Visual Studio

3. Restore NuGet packages:
   ```
   Tools > NuGet Package Manager > Restore NuGet Packages
   ```

4. Build the solution:
   ```
   Build > Build Solution (Ctrl+Shift+B)
   ```

5. The executable will be in `bin/Debug/net472/` or `bin/Release/net472/`

## How It Works

1. **Version Check** - Compares local `version.dat` with server's `version.txt`
2. **File List** - Downloads `files.json` from server
3. **File Comparison** - Checks which files need updating based on file size
4. **Download** - Downloads only changed files
5. **Launch** - Starts the game client

## Troubleshooting

### Launcher won't start
- Make sure .NET Framework 4.7.2 is installed
- Check that all required DLLs are present (Newtonsoft.Json.dll)

### Updates not working
- Verify `ServerURL` in settings is correct
- Check that `version.txt` and `files.json` are accessible
- Ensure file URLs in `files.json` are valid

### Game won't launch
- Verify `ClientPath` in settings points to the correct executable
- Check that the game files are not corrupted

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Credits

Developed for the Conquer Online private server community.

## Support

- X: [X](https://x.com/el7usienysabry)
- Issues: [GitHub Issues](../../issues)
- Facebook: [Facebook](https://www.facebook.com/elhussienyelnemr)

---

Made by El-Hussieny El-Nemr ❤️ for the Conquer community
