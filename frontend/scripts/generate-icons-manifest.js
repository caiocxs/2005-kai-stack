const fs = require('fs');
const path = require('path');

const iconsDir = path.join(__dirname, '../assets/icon');
const outputFile = path.join(__dirname, '../assets/icons-manifest.json');

const files = fs.readdirSync(iconsDir).filter((f) => f.endsWith('.png'));

fs.writeFileSync(outputFile, JSON.stringify(files, null, 2));
console.log(`Gerado manifest com ${files.length} ícones.`);
