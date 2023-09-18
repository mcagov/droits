function initializeSearchBar() {
    const searchInput = document.getElementById('searchInput');
    const searchResults = document.getElementById('searchResults');

    searchInput.addEventListener('input', function () {
        const query = this.value;
        fetchSearchResults(query, searchResults);
    });

    document.addEventListener('click', (e) => {
        if (!searchResults.contains(e.target) && e.target !== searchInput) {
            searchResults.classList.remove('show');
        }
    });
}

function createElement(tag, classes = [], textContent = '') {
    const element = document.createElement(tag);
    element.classList.add(...classes);
    element.textContent = textContent;
    return element;
}

function createSearchResultContainer(item, query) {
    const { reference, wreckId, wreckName, salvorId, salvorName, assignedTo, id, lastModified, created } = item;

    const resultContainer = createElement('div', ['container', 'search-result', 'p-2', 'border-bottom']);
    resultContainer.setAttribute('data-href', `/Droit/View/${id}`);

    const row = createElement('div', ['row', 'd-flex', 'flex-column', 'flex-lg-row']);

    const leftColumn = createElement('div', ['col-12', 'col-xl-5']);
    const rightColumn = createElement('div', ['col-12', 'col-xl-7']);

    const referenceElement = createElement('strong', ['text-primary'], 'Reference: ');
    const createdElement = createElement('strong', ['text-primary'], 'Created: ');
    const lastModifiedElement = createElement('strong', ['text-primary'], 'Modified: ');

    leftColumn.appendChild(referenceElement);

    const highlightedReference = highlightMatch(reference, query);
    highlightedReference.forEach(element => {
        leftColumn.appendChild(element);
    });

    leftColumn.appendChild(document.createElement('br'));

    leftColumn.appendChild(createdElement);
    leftColumn.appendChild(document.createTextNode(created));
    leftColumn.appendChild(document.createElement('br'));

    leftColumn.appendChild(lastModifiedElement);
    leftColumn.appendChild(document.createTextNode(lastModified));

    // Right Column Content
    const wreckNameElement = createElement('strong', ['text-primary'], 'Wreck Name: ');
    const salvorNameElement = createElement('strong', ['text-primary'], 'Salvor Name: ');
    const assignedToElement = createElement('strong', ['text-primary'], 'Assigned To: ');

    rightColumn.appendChild(wreckNameElement);

    const highlightedWreckName = highlightMatch(wreckName, query);
    highlightedWreckName.forEach(element => {
        rightColumn.appendChild(element);
    });

    rightColumn.appendChild(document.createElement('br'));

    rightColumn.appendChild(salvorNameElement);

    const highlightedSalvorName = highlightMatch(salvorName, query);
    highlightedSalvorName.forEach(element => {
        rightColumn.appendChild(element);
    });

    rightColumn.appendChild(document.createElement('br'));

    rightColumn.appendChild(assignedToElement);
    
    const highlightedAssignedTo = highlightMatch(assignedTo, query);
    highlightedAssignedTo.forEach(element => {
        rightColumn.appendChild(element);
    });



    row.appendChild(leftColumn);
    row.appendChild(rightColumn);

    resultContainer.appendChild(row);

    return resultContainer;
}

function highlightMatch(text, query) {
    const queryWords = query.split(' ').filter(word => word.length > 0);
    if (queryWords.length === 0) {
        return [document.createTextNode(text)];
    }

    const regex = new RegExp(`(${queryWords.join('|')})`, 'gi');
    const parts = text.split(regex);

    return parts.map((part, index) => {
        if (regex.test(part)) {
            return createElement('span', ['text-secondary'], part);
        } else {
            return document.createTextNode(part);
        }
    });
}

function fetchSearchResults(query, searchResults) {
    if (typeof query !== 'string' || query.length < 3) {
        searchResults.classList.remove('show');
        return;
    }

    query = query.trim().toLowerCase();

    fetch(`/Search/SearchDroits?query=${query}`)
        .then((response) => response.json())
        .then((data) => {
            showSearchResults(data, query, searchResults);
        })
        .catch((error) => console.error('Error fetching search results:', error));
}

function showSearchResults(data, query, searchResults) {
    const resultsContainer = document.createElement('div');
    resultsContainer.classList.add("search-bar-results-container")

    data.forEach((item, index) => {
        const resultContainer = createSearchResultContainer(item, query);
        resultsContainer.appendChild(resultContainer);
    });

    searchResults.innerHTML = '';
    searchResults.appendChild(resultsContainer);

    if (data.length > 0) {
        searchResults.classList.add('show');
    } else {
        searchResults.classList.remove('show');
    }

    const resultRows = document.querySelectorAll('.search-result');
    resultRows.forEach((row) => {
        row.addEventListener('mouseenter', () => {
            row.classList.add('bg-light');
            row.classList.add('cursor-pointer');
        });

        row.addEventListener('mouseleave', () => {
            row.classList.remove('bg-light');
            row.classList.remove('cursor-pointer');
        });

        row.addEventListener('click', () => {
            const href = row.getAttribute('data-href');
            if (href) {
                window.location.href = href;
            }
        });
    });
}

export { initializeSearchBar };
