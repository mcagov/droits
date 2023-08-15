export function initializeMessageBanner() {
    const messageBanner = document.getElementById("messageBanner");
    if (!messageBanner) return;  
    
    setTimeout(function() {
            messageBanner.classList.add("d-none");
    }, 5000);
}
