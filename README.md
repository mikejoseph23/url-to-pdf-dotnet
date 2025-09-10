# url-to-pdf-dotnet
Converts a Web Page to a PDF File - .NET C# Library made using PuppeteerSharp. http://puppeteersharp.com

## About UrlToPdf-DotNet
A very common requirement for many of my clients is to provide printable web pages in the form of a PDF document. Why not just instruct the user to use their browser's print option? There are a few reasons:

  1. By default, browsers print ugly URL headers and footers on their printed output. The user needs to know how to turn this option off in order to get printed output without these headers, which lessens the usability of your application for novice users.
  2. Sometimes the PDF needs to be automatically generated in order to archive, send, or further process the output.

Over the years, I have used many commercially available components that I've used for converting rendered website content to a PDF file. These libraries often cost a pretty penny (hundreds of dollars per license) and I'm not naming names, but almost all of them have left me with a bad taste for both having spent a fortune on the components and support subscriptions all while wrangling with technical difficulties and issues. I've been keeping my eye out for alternatives and have come up with my own component using Puppeteer, which is a Node-based automated Chromium project.

This project is free to anyone who finds it useful. Hopefully it will save someone lots of money and headaches!

## Project Versions

### .NET 9 Version (New) - Located in `src-net9/`
A modern, cross-platform implementation that runs on Windows, Linux, and macOS.

**Features:**
- ✅ Cross-platform support (Windows, Linux, macOS)
- ✅ .NET 9 with latest performance improvements
- ✅ Separated architecture (Core library + Console app)
- ✅ Comprehensive logging with Serilog (logs saved to `logs/` directory)
- ✅ Modern async/await patterns
- ✅ Improved error handling
- ✅ PuppeteerSharp 20.0.5

**Quick Start:**
```bash
cd src-net9
dotnet run --project UrlToPdf.Console -- -u "https://example.com" -o "output.pdf"

# With verbose logging for debugging
dotnet run --project UrlToPdf.Console -- -u "https://example.com" -o "output.pdf" -v
```

**Command Line Options:**
- `-u, --url` (required): The URL to convert
- `-o, --output` (required): Output PDF file path
- `-w, --wait-for-selector`: Wait for CSS selector before rendering
- `-l, --landscape`: Generate landscape PDF
- `-b, --print-background`: Include background graphics (default: true)
- `--headless`: Run browser in headless mode (default: true)
- `-v, --verbose`: Enable verbose logging

### .NET Framework 4.7.2 Version (Original) - Located in `src/`
The original implementation for Windows environments with .NET Framework.

**Features:**
- Windows-specific implementation
- .NET Framework 4.7.2
- PuppeteerSharp 2.0.4
- Simple console application

**Quick Start:**
```bash
# Build with MSBuild
msbuild src\UrlToPdf.sln /p:Configuration=Release

# Run
src\UrlToPdf.Console\bin\Release\UrlToPdf.Console.exe -u "https://example.com" -s "output.pdf"
```

## Migration Status
✅ **Phase 1-3 Complete**: Successfully migrated core functionality to .NET 9
- Created new architecture with separated Core library and Console app
- Updated all NuGet packages to latest versions
- Added comprehensive logging framework
- Fixed all breaking API changes
- Improved error handling and diagnostics

## Getting Started

### Prerequisites
- For .NET 9 version: [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- For .NET Framework version: Visual Studio 2019+ or MSBuild

### Building from Source

**.NET 9 Version:**
```bash
cd src-net9
dotnet build
dotnet publish -c Release
```

**.NET Framework Version:**
```bash
msbuild src\UrlToPdf.sln /p:Configuration=Release
```

## Troubleshooting

If you encounter issues:
1. Run with verbose logging: add `-v` flag
2. Check the logs in the `logs/` directory (for .NET 9 version)
3. Ensure Chrome/Chromium can be downloaded (first run downloads it automatically)
4. For Linux/macOS, you may need to install additional dependencies for Chromium

## Roadmap / Desired features
- ✅ ~~Migrate to .NET 9 for cross-platform support~~
- ✅ ~~Add comprehensive logging~~
- ✅ ~~Separate core logic into reusable library~~
- ✅ ~~Read the converted PDF into a byte array (implemented in Core library)~~
- ⬜ Add Docker support
- ⬜ Create NuGet package for the Core library
- ⬜ Add unit and integration tests
- ⬜ Add CI/CD pipeline

## License
This project is free to anyone who finds it useful. Hopefully it will save someone lots of money and headaches!