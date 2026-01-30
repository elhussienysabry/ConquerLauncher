# How to Use Server Update Scripts

This guide explains how to use the PowerShell scripts to manage your server files and updates.

## Files Overview

- **Generate-FilesJson.ps1** - Automatically generates `files.json` from your files
- **Quick-Update.ps1** - Quick update workflow (version + files.json)
- **files.json** - Example file list structure
- **version.txt** - Example version file
- **news.html** - Example news/announcements page

## Prerequisites

- Windows PowerShell (comes with Windows)
- Your game files ready in the `files` folder

## Using Generate-FilesJson.ps1

This script scans your `files` folder and creates a `files.json` automatically.

### Basic Usage

```powershell
.\Generate-FilesJson.ps1
```

### With Base URL

```powershell
.\Generate-FilesJson.ps1 -BaseURL "https://yourserver.com/files"
```

### What It Does

1. Scans the `files` folder recursively
2. Creates entries for each file
3. Generates relative paths automatically
4. Preserves existing download URLs (if files.json exists)
5. Saves the result to `files.json`

### Example Output

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

## Using Quick-Update.ps1

This script helps you quickly prepare a new update.

### Usage

```powershell
.\Quick-Update.ps1 -Version "1.0.1"
```

### With Base URL

```powershell
.\Quick-Update.ps1 -Version "1.0.1" -BaseURL "https://yourserver.com/files"
```

### What It Does

1. Updates `version.txt` with the new version
2. Regenerates `files.json` from the `files` folder
3. Shows a summary of files

### Workflow Example

1. Add/update files in the `files` folder
2. Run the quick update script:
   ```powershell
   .\Quick-Update.ps1 -Version "1.0.1"
   ```
3. Review the generated files
4. Upload to your server:
   - `version.txt`
   - `files.json`
   - All files in the `files` folder

## File Structure

Your ServerFiles folder should look like this:

```
ServerFiles/
├── version.txt          # Current version (e.g., 1.0.0)
├── files.json          # File list for updates
├── news.html           # Optional news page
└── files/              # Your game files
    ├── Clash.exe
    ├── data/
    │   └── data.dat
    └── ...
```

## files.json Format

Each file entry requires three fields:

```json
{
  "FileName": "file.ext",      // Name of the file
  "RelativePath": "folder",     // Folder path (empty string for root)
  "DownloadURL": "https://..."  // Full download URL
}
```

### Examples

**Root level file:**
```json
{
  "FileName": "Clash.exe",
  "RelativePath": "",
  "DownloadURL": "https://yourserver.com/files/Clash.exe"
}
```

**File in subfolder:**
```json
{
  "FileName": "data.dat",
  "RelativePath": "data",
  "DownloadURL": "https://yourserver.com/files/data/data.dat"
}
```

**File in nested folders:**
```json
{
  "FileName": "config.ini",
  "RelativePath": "data/config",
  "DownloadURL": "https://yourserver.com/files/data/config/config.ini"
}
```

## Common Tasks

### First Time Setup

1. Create the `files` folder
2. Add your game files to it
3. Run the generator:
   ```powershell
   .\Generate-FilesJson.ps1 -BaseURL "https://yourserver.com/files"
   ```
4. Set the initial version:
   ```powershell
   "1.0.0" | Out-File version.txt -Encoding UTF8
   ```
5. Upload everything to your server

### Publishing an Update

1. Update/add files in the `files` folder
2. Run the quick update:
   ```powershell
   .\Quick-Update.ps1 -Version "1.0.1" -BaseURL "https://yourserver.com/files"
   ```
3. Upload the updated files to your server
4. Players will automatically receive the update

### Manual files.json Editing

If you need to manually edit `files.json`:

1. Keep the JSON format valid
2. Ensure all three fields are present
3. Use forward slashes (/) in paths
4. Don't include leading slashes in RelativePath
5. Make sure DownloadURLs are accessible

## Tips

- Always test updates on a local launcher first
- Keep backups of your `files.json`
- Use consistent version numbering (e.g., 1.0.0, 1.0.1, 1.1.0)
- The launcher compares file sizes, not hashes
- Large files will show download progress to players

## Troubleshooting

### Script Won't Run

If you get an execution policy error:
```powershell
Set-ExecutionPolicy -Scope CurrentUser -ExecutionPolicy RemoteSigned
```

### Files Not Detected

- Make sure files are in the `files` subfolder
- Check that the folder isn't empty
- Verify you're running the script from ServerFiles_Examples

### Invalid JSON

- Use the scripts instead of manual editing when possible
- Validate JSON at https://jsonlint.com
- Check for missing commas or brackets

## Need Help?

- Check the main [README.md](../README.md)
- Join the Discord community
- Open an issue on GitHub
