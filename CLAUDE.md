# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a .NET Framework 4.7.2 console application that converts web pages to PDF files using PuppeteerSharp. The project provides both a C# implementation and a simpler Node.js alternative.

## Build and Development Commands

### Building the Project

```bash
# Build the solution using MSBuild (Visual Studio Developer Command Prompt)
msbuild src\UrlToPdf.sln /p:Configuration=Debug

# Or for release build
msbuild src\UrlToPdf.sln /p:Configuration=Release

# Restore NuGet packages if needed
nuget restore src\UrlToPdf.sln
```

### Running the Application

```bash
# Run the console application with required parameters
src\UrlToPdf.Console\bin\Debug\UrlToPdf.Console.exe -u "https://example.com" -s "output.pdf"

# Optional parameters:
# --wait-for-selector: Wait for CSS selector before rendering
# --landscape: Output PDF in landscape mode  
# --print-background: Print background (default: true)
```

## Architecture

### Core Components

1. **UrlToPdf.Console** - Main console application project
   - `Program.cs`: Entry point with command-line parsing using CommandLineParser library
   - Uses PuppeteerSharp to control headless Chrome for PDF generation
   - Async/await pattern for browser automation

2. **Key Dependencies**
   - PuppeteerSharp 2.0.4 - .NET port of Puppeteer for browser automation
   - CommandLineParser 2.5.0 - Command-line argument parsing
   - Newtonsoft.Json 12.0.2 - JSON serialization

3. **NodeJs Alternative**
   - Simple Node.js script using original Puppeteer library
   - Located in `src/NodeJs/urlToPdf.js`

### PDF Generation Flow

1. Parse command-line arguments to get URL and output path
2. Download Chromium browser if not present (via BrowserFetcher)
3. Launch headless Chrome instance with HTTPS error ignoring
4. Navigate to specified URL
5. Optionally wait for specific CSS selector to load
6. Emulate print media type for proper styling
7. Generate PDF with specified options (landscape, background printing)
8. Save to specified file path

### Known Limitations

- Currently saves PDF to filesystem only (no MemoryStream support yet)
- Headless mode is set to false in current code (line 49 of Program.cs)
