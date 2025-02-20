document.querySelectorAll('.tab-button').forEach(button => {
    button.addEventListener('click', function () {
        document.querySelectorAll('.tab-button').forEach(btn => {
            btn.classList.remove('bg-blue-600', 'text-white', 'border-blue-600');
            btn.classList.add('bg-gray-200', 'text-gray-800', 'border-gray-300');
        });
        this.classList.add('bg-blue-600', 'text-white', 'border-blue-600');
        this.classList.remove('bg-gray-200', 'text-gray-800', 'border-gray-300');

        document.querySelectorAll('.tab-section').forEach(section => section.classList.add('hidden'));
        document.querySelector(`.tab-section[data-tab="${this.dataset.tab}"]`).classList.remove('hidden');
    });
});

const percentInputs = document.querySelectorAll('.percent-input');
const progressBars = document.querySelectorAll('.progress-bar');

percentInputs.forEach((input, index) => {
    input.addEventListener('input', () => {
        let value = parseInt(input.value);

        if (value < 0) value = 0;
        if (value > 100) value = 100;

        progressBars[index].style.width = value + '%';
    });
});
