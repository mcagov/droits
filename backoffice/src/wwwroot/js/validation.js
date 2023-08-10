export function validateFormTab(navLink) {
    if(!navLink){ return; }
    const target = document.querySelector(navLink.getAttribute('data-bs-target'));
    if (target && target.querySelectorAll(".input-validation-error").length > 0) {
        navLink.classList.add("text-danger");
    }
}
