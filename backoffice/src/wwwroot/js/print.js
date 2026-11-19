export function initializePrint() {
    document.querySelector('.js-print-btn')
        ?.addEventListener('click', () => window.print());

    document.querySelector('#include-notes-checkbox')
        ?.addEventListener('change', e =>
            document.body.classList.toggle('print-include-notes', e.target.checked));
}
