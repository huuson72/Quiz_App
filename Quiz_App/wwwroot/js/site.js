// Quiz App JavaScript

// Initialize the application when the document is ready
document.addEventListener('DOMContentLoaded', function () {
    console.log('Quiz App initialized');
    
    // Enable tooltips if they exist
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
    
    // Make list group items clickable for radio buttons
    makeListGroupItemsClickable();
    
    // Add visual indication to selected answers
    highlightSelectedAnswers();
});

// Function to make entire list group item clickable for radio buttons
function makeListGroupItemsClickable() {
    var labels = document.querySelectorAll('.list-group-item');
    
    labels.forEach(function (label) {
        label.addEventListener('click', function (e) {
            // Don't process clicks on the radio button itself
            if (e.target.type !== 'radio') {
                var radio = this.querySelector('input[type="radio"]');
                if (radio) {
                    radio.checked = true;
                }
            }
        });
    });
}

// Function to highlight the selected answer
function highlightSelectedAnswers() {
    var radioButtons = document.querySelectorAll('.list-group-item input[type="radio"]');
    
    radioButtons.forEach(function (radio) {
        radio.addEventListener('change', function () {
            // Remove highlight from all list items
            var listItems = document.querySelectorAll('.list-group-item');
            listItems.forEach(function (item) {
                item.classList.remove('active');
            });
            
            // Add highlight to selected item
            if (this.checked) {
                this.closest('.list-group-item').classList.add('active');
            }
        });
    });
} 