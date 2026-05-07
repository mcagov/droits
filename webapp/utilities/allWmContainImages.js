export const  allWmContainImages = (obj) => {
  let stack  = [];

  for (let key in obj) {
    if(typeof obj[key].image == "string"){
      stack.push(true);
    } else {
      stack.push(false);
    }
  }

  return stack.every((image) => image === true);
}
  
