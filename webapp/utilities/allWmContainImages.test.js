import { allWmContainImages } from './allWmContainImages';

describe('allWmContainImages', () => {
  it('returns true when all items have a string image', () => {
    const obj = {
      a: { image: 'photo1.jpg' },
      b: { image: 'photo2.png' },
    };
    expect(allWmContainImages(obj)).toBe(true);
  });

  it('returns false when any item has no image property', () => {
    const obj = {
      a: { image: 'photo1.jpg' },
      b: { name: 'no image here' },
    };
    expect(allWmContainImages(obj)).toBe(false);
  });

  it('returns false when image is not a string (null)', () => {
    const obj = {
      a: { image: null },
    };
    expect(allWmContainImages(obj)).toBe(false);
  });

  it('returns false when image is not a string (number)', () => {
    const obj = {
      a: { image: 42 },
    };
    expect(allWmContainImages(obj)).toBe(false);
  });

  it('returns false when image is not a string (object)', () => {
    const obj = {
      a: { image: { url: 'photo.jpg' } },
    };
    expect(allWmContainImages(obj)).toBe(false);
  });

  it('returns true for an empty object', () => {
    expect(allWmContainImages({})).toBe(true);
  });
});