# URL to PDF Converter (.NET 9)

A modern .NET 9 implementation of a URL to PDF converter using PuppeteerSharp.

## Architecture

The solution is split into two projects:

1. **UrlToPdf.Core** - Class library containing the core PDF generation logic
2. **UrlToPdf.Console** - Console application providing the CLI interface

## Prerequisites

- .NET 9 SDK
- Chrome/Chromium browser (automatically downloaded on first run)

## Building

```bash
cd src-net9
dotnet build
```

## Running

```bash
# Basic usage
dotnet run --project UrlToPdf.Console -- -u "https://example.com" -o "output.pdf"

# With options
dotnet run --project UrlToPdf.Console -- -u "https://example.com" -o "output.pdf" -l -b

# Or after building
cd UrlToPdf.Console/bin/Debug/net9.0
./UrlToPdf.Console -u "https://example.com" -o "output.pdf"
```

## Command Line Options

| Option | Short | Long | Description | Default |
|--------|-------|------|-------------|---------|
| URL | -u | --url | The URL to convert to PDF | Required |
| Output | -o | --output | Path to save the PDF file | Required |
| Wait Selector | -w | --wait-for-selector | CSS selector to wait for before rendering | None |
| Landscape | -l | --landscape | Output PDF in landscape mode | false |
| Background | -b | --print-background | Print background graphics | true |
| Headless | | --headless | Run browser in headless mode | true |

## Examples

```bash
# Convert a webpage to PDF
dotnet run --project UrlToPdf.Console -- -u "https://github.com" -o "github.pdf"

# Wait for specific content to load
dotnet run --project UrlToPdf.Console -- -u "https://example.com" -o "output.pdf" -w ".main-content"

# Generate landscape PDF with background
dotnet run --project UrlToPdf.Console -- -u "https://example.com" -o "output.pdf" -l -b

# Run in non-headless mode for debugging
dotnet run --project UrlToPdf.Console -- -u "https://example.com" -o "output.pdf" --headless false
```

## Publishing

```bash
# Self-contained executable for Windows
dotnet publish UrlToPdf.Console -c Release -r win-x64 --self-contained

# Self-contained executable for Linux
dotnet publish UrlToPdf.Console -c Release -r linux-x64 --self-contained

# Self-contained executable for macOS
dotnet publish UrlToPdf.Console -c Release -r osx-x64 --self-contained
```

## Migration Notes

This is a migrated version from .NET Framework 4.7.2 to .NET 9 with the following improvements:

- Cross-platform support (Windows, Linux, macOS)
- Modern .NET 9 runtime with better performance
- Separated core logic into a reusable library
- Updated to latest PuppeteerSharp (v20.0.5)
- Fixed duplicate command-line option issue (changed save-path short option from -s to -o)
- Added ASCII art logo
- Improved error handling and disposal patterns