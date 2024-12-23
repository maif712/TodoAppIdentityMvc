

(function () {
    // Retrieve the theme from localStorage
    const theme = JSON.parse(localStorage.getItem('theme')) || 'light'; // Default to light mode

    // Add the theme class to the <html> tag
    document.documentElement.classList.add(theme);
})();