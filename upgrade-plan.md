# .NET Framework 4.7.2 to .NET 9 Migration Plan

## Table of Contents
- [Overview](#overview)
- [Current State Analysis](#current-state-analysis)
  - [Project Structure](#project-structure)
  - [Current Dependencies](#current-dependencies)
- [Migration Steps](#migration-steps)
  - [Phase 1: Project File Migration](#phase-1-project-file-migration)
  - [Phase 2: Package Updates](#phase-2-package-updates)
  - [Phase 3: Code Changes](#phase-3-code-changes)
  - [Phase 4: Build System Updates](#phase-4-build-system-updates)
  - [Phase 5: Testing Strategy](#phase-5-testing-strategy)
- [Benefits of Migration](#benefits-of-migration)
- [Risk Assessment](#risk-assessment)
- [Rollback Plan](#rollback-plan)
- [Success Criteria](#success-criteria)
- [Timeline Estimate](#timeline-estimate)
- [Next Steps](#next-steps)

## Overview
This document outlines the migration strategy for upgrading the UrlToPdf console application from .NET Framework 4.7.2 to .NET 9.

## Current State Analysis

### Project Structure
- **Framework**: .NET Framework 4.7.2
- **Project Type**: Console Application
- **Main Project**: UrlToPdf.Console
- **Build System**: MSBuild with traditional .csproj format
- **Target Architecture**: Will be split into:
  - UrlToPdf.Core (Class Library) - Core PDF generation logic (PDF only for now)
  - UrlToPdf.Console (Console App) - CLI interface consuming the library

### Current Dependencies
1. **PuppeteerSharp** 2.0.4 → Need to upgrade to latest version (13.x+)
2. **CommandLineParser** 2.5.0 → Should work with .NET 9
3. **Newtonsoft.Json** 12.0.2 → Consider migrating to System.Text.Json

## Migration Steps

### Phase 1: Project File Migration ✅ COMPLETED
- [x] Create new `src-net9` folder to preserve original .NET Framework version
- [x] Create new UrlToPdf.Core class library project (.NET 9) in `src-net9`
- [x] Create new UrlToPdf.Console project with SDK-style format in `src-net9`
- [x] Target .NET 9 (`<TargetFramework>net9.0</TargetFramework>`) for both projects
- [x] Move PDF generation logic to UrlToPdf.Core library
- [x] Keep CLI parsing logic in UrlToPdf.Console
- [x] Remove unnecessary assembly references
- [x] Update output paths and build configurations

### Phase 2: Package Updates ✅ COMPLETED
- [x] **PuppeteerSharp**: Upgrade to latest version (20.0.5)
   - Updated to v20.0.5 with full .NET 9 support
   - Fixed API changes (IgnoreHTTPSErrors → --ignore-certificate-errors arg)
   
- [x] **CommandLineParser**: Update to latest version (2.9.1)
   - Successfully upgraded and working with .NET 9
   
- [x] **Newtonsoft.Json**: Removed dependency
   - Not needed in current implementation
   - Can use System.Text.Json if JSON handling is needed later

### Phase 3: Code Changes ✅ COMPLETED

#### Required Changes
- [x] **Add ASCII art logo**: Added URL to PDF ASCII art logo at the beginning of console app
- [x] **Namespace updates**: Updated all namespaces to modern conventions
- [x] **Async patterns**: All async methods follow latest conventions with proper async/await
- [x] **File I/O**: Using modern file operations via PuppeteerSharp
- [x] **Browser fetcher**: Updated to use new BrowserFetcher API

#### Potential Issues Fixed
- [x] **LaunchOptions**: Headless property now configurable via command line (default: true)
- [x] **Command-line parsing**: Fixed duplicate short option - changed save-path to use '-o' for output
- [x] **Browser disposal**: Added proper IDisposable pattern for PdfGenerator class

### Phase 4: Build System Updates

- [ ] Update solution file if needed
- [ ] Create new build scripts for .NET CLI:
   ```bash
   dotnet build
   dotnet run
   dotnet publish
   ```
- [ ] Update CI/CD pipelines if applicable

### Phase 5: Testing Strategy

- [ ] **Unit Tests**: Create if not present
- [ ] **Integration Tests**: Test PDF generation with various URLs
- [ ] **Command-line Tests**: Verify all CLI options work correctly
- [ ] **Performance Tests**: Compare performance between old and new versions

## Benefits of Migration

1. **Cross-platform support**: Run on Linux, macOS, and Windows
2. **Performance improvements**: Better runtime performance and smaller memory footprint
3. **Modern C# features**: Access to latest C# language features
4. **Better containerization**: Easier Docker support
5. **Long-term support**: .NET 9 is the latest version with ongoing support
6. **Improved security**: Latest security patches and updates

## Risk Assessment

### Low Risk
- CommandLineParser migration
- Basic console functionality

### Medium Risk
- PuppeteerSharp upgrade (API changes)
- File I/O operations

### High Risk
- None identified

## Rollback Plan

- [ ] Keep original .NET Framework project in a separate branch
- [ ] Maintain both versions temporarily during transition
- [ ] Document any breaking changes for users

## Success Criteria

1. ✅ Application builds successfully on .NET 9
2. ✅ All command-line options work as before
3. ✅ PDF generation produces identical output
4. ✅ Performance is equal or better
5. ✅ Application runs on Windows, Linux, and macOS

## Timeline Estimate

- Phase 1: 30 minutes
- Phase 2: 1 hour
- Phase 3: 1-2 hours
- Phase 4: 30 minutes
- Phase 5: 1-2 hours

**Total estimated time**: 4-6 hours

## Next Steps

- [ ] Create a new branch for migration
- [ ] Start with Phase 1: Project file migration
- [ ] Incrementally apply changes and test after each phase
- [ ] Document any breaking changes or new requirements