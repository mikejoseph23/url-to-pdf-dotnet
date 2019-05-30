const puppeteer = require('puppeteer');

async function printPDF () {
  const browser = await puppeteer.launch({ 
	headless: true
	// executablePath: './node_modules/puppeteer/.local-chromium/win64-662092/chrome-win/chrome.exe' 
  });
  
  const page = await browser.newPage();
  await page.goto(
	process.argv[2],
	{ waitUntil: 'networkidle0' }
  );
  const pdf = await page.pdf({
	format: 'A4',
	path: process.argv[3],
	printBackground: true
  });

  await browser.close();
  return pdf;
}

printPDF().then(() => console.log('Done!'));