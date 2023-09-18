import Handlebars from 'handlebars/dist/handlebars.min.js';


const SEARCH_INPUT = document.getElementById('searchInput');
const SEARCH_RESULTS = document.getElementById('searchResults');
const SEARCH_RESULT_TEMPLATE = Handlebars.compile(document.getElementById('search-result-template').innerHTML);
function initializeSearchBar() {
    
    // Event listeners
        SEARCH_INPUT.addEventListener('input', handleSearchInput);
        document.addEventListener('click', handleDocumentClick);
    
    // Register Handlebars helper
        Handlebars.registerHelper('highlightMatch', highlightMatch);
}
function handleSearchInput() {
    const query = SEARCH_INPUT.value.trim().toLowerCase();
    if (query.length < 3) {
        hideSearchResults();
        return;
    }

    fetch(`/Search/SearchDroits?query=${query}`)
        .then(response => response.json())
        .then(data => showSearchResults(data, query))
        .catch(error => console.error('Error fetching search results:', error));
}

function handleDocumentClick(e) {
    if (!SEARCH_RESULTS.contains(e.target) && e.target !== SEARCH_INPUT) {
        hideSearchResults();
    }
}

function highlightMatch(text, query) {
    const queryWords = query.split(' ').filter(word => word.length > 0);
    if (queryWords.length === 0) {
        return new Handlebars.SafeString(text);
    }

    const regex = new RegExp(`(${queryWords.join('|')})`, 'gi');
    const parts = text.split(regex);

    const highlightedParts = parts.map(part =>
        regex.test(part) ? `<span class="text-secondary">${part}</span>` : part
    );

    return new Handlebars.SafeString(highlightedParts.join(''));
}

function showSearchResults(data, query) {
    SEARCH_RESULTS.innerHTML = '';
    data.forEach(item => {
        item.query = query;
        const resultContainerHTML = SEARCH_RESULT_TEMPLATE(item);
        const resultContainer = document.createElement('div')
        resultContainer.innerHTML = resultContainerHTML;
        SEARCH_RESULTS.appendChild(resultContainer.firstElementChild);
    });

    SEARCH_RESULTS.classList.toggle('show', data.length > 0);
}

function hideSearchResults() {
    SEARCH_RESULTS.classList.remove('show');
}

export { initializeSearchBar };
