# url-to-pdf-dotnet
Converts a Web Page to a PDF File - .NET C# Library made using NodeJs and Puppeteer

## About UrlToPdf-DotNet
A very common requirement for many of my clients is to provide printable web pages in the form of a PDF document. Why not just instruct the user to use their browser's print option? There are a few reasons:

  1. By default, browsers print ugly URL headers and footers on their printed output. The user needs to know how to turn this option off in order to get printed output without these headers, which lessens the usability of your application for novice users.
  2. Sometimes the PDF needs to be automatically generated in order to archive, send, or further process the output.

Over the years, I have used many commercially available components that I've used for converting rendered website content to a PDF file. These libraries often cost a pretty penny (hundreds of dollars per license) and I'm not naming names, but almost all of them have left me with a bad taste for both having spent a fortune on the components and support subscriptions all while wrangling with technical difficulties and issues. I've been keeping my eye out for alternatives and have come up with my own component using Puppeteer, which is a Node-based automated Chromium project.

This project is free to anyone who finds it useful. Hopefully it will save someone lots of money and headaches!

## Getting Started

1. Install Prerequisites
  - NodeJs
  - Yarn (optional) - You can use NPM instead if you'd rather.
2. Clone this repository.
3. Edit App.config to reflect local file paths.
4. To be continued...

## Roadmap / Desired features
- Read the converted PDF into a MemoryStream eliminating the need for the filesystem for output.
