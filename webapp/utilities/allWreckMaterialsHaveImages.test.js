import fs from 'fs';
import { allWreckMaterialsHaveImages } from './allWreckMaterialsHaveImages';

jest.mock('fs', () => {
  const actual = jest.requireActual('fs');
  return {
    ...actual,
    promises: {
      ...actual.promises,
      access: jest.fn(),
    },
  };
});

beforeEach(() => {
  jest.clearAllMocks();
  jest.spyOn(console, 'warn').mockImplementation(() => {});
  fs.promises.access.mockResolvedValue(undefined);
});

afterEach(() => {
  console.warn.mockRestore();
});

describe('allWreckMaterialsHaveImages', () => {
  describe('image property type check', () => {
    it('returns true when all items have a string image', async () => {
      const obj = {
        a: { image: 'photo1.jpg' },
        b: { image: 'photo2.png' },
      };
      await expect(allWreckMaterialsHaveImages(obj)).resolves.toBe(true);
    });

    it('returns false when any item has no image property', async () => {
      const obj = {
        a: { image: 'photo1.jpg' },
        b: { name: 'no image here' },
      };
      await expect(allWreckMaterialsHaveImages(obj)).resolves.toBe(false);
    });

    it('returns false when image is null', async () => {
      const obj = { a: { image: null } };
      await expect(allWreckMaterialsHaveImages(obj)).resolves.toBe(false);
    });

    it('returns false when image is a number', async () => {
      const obj = { a: { image: 42 } };
      await expect(allWreckMaterialsHaveImages(obj)).resolves.toBe(false);
    });

    it('returns false when image is an object', async () => {
      const obj = { a: { image: { url: 'photo.jpg' } } };
      await expect(allWreckMaterialsHaveImages(obj)).resolves.toBe(false);
    });

    it('returns true for an empty object', async () => {
      await expect(allWreckMaterialsHaveImages({})).resolves.toBe(true);
    });

    it('does not touch the filesystem when an image is not a string', async () => {
      const obj = { a: { image: null } };
      await allWreckMaterialsHaveImages(obj);
      expect(fs.promises.access).not.toHaveBeenCalled();
    });
  });

  describe('file existence check', () => {
    it('returns true when every image file exists on disk', async () => {
      const obj = {
        a: { image: 'photo1.jpg' },
        b: { image: 'photo2.png' },
      };
      await expect(allWreckMaterialsHaveImages(obj)).resolves.toBe(true);
      expect(fs.promises.access).toHaveBeenCalledTimes(2);
    });

    it('resolves each image path against the uploads directory', async () => {
      const obj = { a: { image: 'photo1.jpg' } };
      await allWreckMaterialsHaveImages(obj);
      const [calledPath] = fs.promises.access.mock.calls[0];
      expect(calledPath).toMatch(/uploads[\\/]photo1\.jpg$/);
    });

    it('returns false when an image file is missing from disk', async () => {
      fs.promises.access.mockRejectedValueOnce(
        Object.assign(new Error('ENOENT'), { code: 'ENOENT' }),
      );
      const obj = { a: { image: 'missing.jpg' } };
      await expect(allWreckMaterialsHaveImages(obj)).resolves.toBe(false);
    });

    it('returns false on the first missing file and stops checking', async () => {
      fs.promises.access
        .mockResolvedValueOnce(undefined)
        .mockRejectedValueOnce(
          Object.assign(new Error('ENOENT'), { code: 'ENOENT' }),
        );
      const obj = {
        a: { image: 'exists.jpg' },
        b: { image: 'missing.jpg' },
        c: { image: 'never-checked.jpg' },
      };
      await expect(allWreckMaterialsHaveImages(obj)).resolves.toBe(false);
      expect(fs.promises.access).toHaveBeenCalledTimes(2);
    });
  });
});
