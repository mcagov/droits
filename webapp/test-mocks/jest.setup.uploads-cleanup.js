const fs = require('fs');

afterAll(() => {
  // Clear the contents of uploads/ but leave the directory in place
  if (fs.existsSync('uploads')) {
    for (const entry of fs.readdirSync('uploads')) {
      fs.rmSync(`uploads/${entry}`, { recursive: true, force: true });
    }
  }
});
