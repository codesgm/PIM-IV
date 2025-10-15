// FAQ Site JavaScript
document.addEventListener('DOMContentLoaded', function() {
    // Auto-focus no campo de busca
    const searchInput = document.querySelector('input[name="busca"]');
    if (searchInput && !searchInput.value) {
        searchInput.focus();
    }
    
    // Smooth scroll para accordion items
    const accordionButtons = document.querySelectorAll('.accordion-button');
    accordionButtons.forEach(button => {
        button.addEventListener('click', function() {
            setTimeout(() => {
                this.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
            }, 300);
        });
    });
});
