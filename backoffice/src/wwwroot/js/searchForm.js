import Choices from "choices.js";


export function initializeSearchForm() {

    
    const droitSearchFormElements = document.querySelectorAll('.js-droit-search');
    if (droitSearchFormElements.length > 0) {
        initializeDroitSearchForm();
    }
    
    const salvorSearchFormElements = document.querySelectorAll('.js-salvor-search');
    if (salvorSearchFormElements.length > 0) {
        initializeSalvorSearchForm();
    }
   
}




function initializeDroitSearchForm(){
     new Choices('.js-search-droit-status', {
        removeItems: true,
        removeItemButton: true,
        placeholder: true,
        placeholderValue: 'Select status',
    });

    new Choices('.js-select-assigned-user', {
        allowHTML: false,
        searchEnabled: true,
        itemSelectText: 'Select',
    });
    
    const droitSearchFormButton = document.querySelector('.js-toggle-droit-search');
    

    droitSearchFormButton.addEventListener('click', function (){
        console.log("droit search button clicked")
        const droitSearchForm = document.querySelector('.js-droit-search');
        droitSearchForm.classList.toggle('d-none');
    });
    

}


function initializeSalvorSearchForm(){
    
    const salvorSearchFormButton = document.querySelector('.js-toggle-salvor-search');
    
    
    salvorSearchFormButton.addEventListener('click', function (){
        console.log("salvor search button clicked")
        const salvorSearchForm = document.querySelector('.js-salvor-search');
        salvorSearchForm.classList.toggle('d-none');
    });
    


}