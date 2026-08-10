import path from 'path';
import fs from 'fs';

export const allWreckMaterialsHaveImages = async (obj) => {
  for (let key in obj) {
    if (typeof obj[key].image != 'string') {
      return false;
    }

    try {
      const filePath = path.resolve(__dirname, '../uploads/', obj[key].image);
      await fs.promises.access(filePath, fs.constants.F_OK);
    } catch (error) {
      console.warn('Image file does not exist:', error);
      return false;
    }
  }
  
  return true;
};
  
