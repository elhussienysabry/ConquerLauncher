# Server Files Examples

This folder contains example files and helper scripts for managing your game server updates.

## Contents

- **Generate-FilesJson.ps1** - PowerShell script to automatically generate `files.json`
- **Quick-Update.ps1** - PowerShell script for quick version updates
- **HOW_TO_USE_SCRIPTS.md** - Detailed guide on using the scripts
- **files.json** - Example file list
- **version.txt** - Example version file
- **news.html** - Example news/announcements page

## Quick Start

### 1. Setup Your Files

Create a `files` folder and add your game files:

```
ServerFiles_Examples/
└── files/
    ├── Clash.exe
    ├── data/
    │   └── data.dat
    └── ...
```

### 2. Generate files.json

```powershell
.\Generate-FilesJson.ps1 -BaseURL "https://yourserver.com/files"
```

### 3. Set Version

```powershell
"1.0.0" | Out-File version.txt -Encoding UTF8
```

### 4. Upload to Server

Upload these files to your web server:
- `version.txt`
- `files.json`
- All files from the `files` folder

## Update Workflow

When you want to release an update:

1. **Update files** - Add/modify files in the `files` folder

2. **Run quick update**:
   ```powershell
   .\Quick-Update.ps1 -Version "1.0.1" -BaseURL "https://yourserver.com/files"
   ```

3. **Upload to server** - Upload the updated files

4. **Done!** - Players will automatically get the update

## File Format

The `files.json` format is simple:

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
    "DownloadURL": "https://yourserver.com/files/data/data.dat"
  }
]
```

- **FileName** - Name of the file
- **RelativePath** - Folder path (empty string for root)
- **DownloadURL** - Full URL to download the file

## Scripts

### Generate-FilesJson.ps1

Automatically scans your `files` folder and creates `files.json`:

```powershell
# Basic usage
.\Generate-FilesJson.ps1

# With base URL
.\Generate-FilesJson.ps1 -BaseURL "https://yourserver.com/files"
```

**Features:**
- Recursive folder scanning
- Automatic path generation
- Preserves existing URLs
- Creates proper JSON structure

### Quick-Update.ps1

Updates version and regenerates files.json in one command:

```powershell
.\Quick-Update.ps1 -Version "1.0.1" -BaseURL "https://yourserver.com/files"
```

**What it does:**
- Updates `version.txt`
- Regenerates `files.json`
- Shows file summary

## Tips

✅ **DO:**
- Test updates locally first
- Use consistent version numbering (1.0.0, 1.0.1, 1.1.0)
- Keep backups of your files.json
- Verify URLs are accessible

❌ **DON'T:**
- Edit files.json manually (use scripts instead)
- Forget to update version.txt
- Upload incomplete updates

## Need Help?

See [HOW_TO_USE_SCRIPTS.md](HOW_TO_USE_SCRIPTS.md) for detailed instructions.

For more information, check the main [README.md](../README.md)
