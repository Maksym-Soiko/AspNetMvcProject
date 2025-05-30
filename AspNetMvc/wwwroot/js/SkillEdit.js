﻿document.addEventListener('DOMContentLoaded', function () {
    const colorSelect = document.getElementById('colorSelect');
    const colorCircle = document.getElementById('colorCircle');

    if (colorSelect.value) {
        colorCircle.style.backgroundColor = colorSelect.value;
    }

    colorSelect.addEventListener('change', function () {
        const selectedColor = this.value;
        if (selectedColor) {
            colorCircle.style.backgroundColor = selectedColor;
        } else {
            colorCircle.style.backgroundColor = 'transparent';
        }
    });
});

document.addEventListener('DOMContentLoaded', function () {
    const stars = document.querySelectorAll('#star-rating .star');
    const complexityInput = document.querySelector('[name="Complexity"]');

    const complexityMap = {
        1: "Дуже легко",
        2: "Легко",
        3: "Середньо",
        4: "Складно",
        5: "Дуже складно"
    };

    const initialComplexity = complexityInput.value ? Object.keys(complexityMap).find(key => complexityMap[key] === complexityInput.value) : 0;
    if (initialComplexity) {
        stars.forEach(s => s.textContent = '☆');
        for (let i = 0; i < initialComplexity; i++) {
            stars[i].textContent = '★';
        }
    }

    stars.forEach(star => {
        star.addEventListener('click', function () {
            const value = this.getAttribute('data-value');
            stars.forEach(s => s.textContent = '☆');

            for (let i = 0; i < value; i++) {
                stars[i].textContent = '★';
            }

            complexityInput.value = complexityMap[value];
        });
    });
});