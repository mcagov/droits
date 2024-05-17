export const  allWmContainImages = (obj) => {
  let stack  = [];

  for (let key in obj) {
    if ('image' in obj[key]) {
      stack.push(true);
    } else {
      stack.push(false);
    }
  }

  return stack.every((image) => image === true);
}
  
