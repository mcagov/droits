export const  allWmContainImages = (obj) => {
  for (let key in obj) {
    if(typeof obj[key].image != "string"){
      return false
    }
  }

  return true;
}
  
