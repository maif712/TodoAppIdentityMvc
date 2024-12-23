

const mainForm = document.querySelector(".account-form")
const requiredInputes = document.querySelectorAll(".form-input-needs-validation")
if (requiredInputes) {
    requiredInputes.forEach(input => {
        input.addEventListener("input", () => {
            if (input.value) {
                input.classList.remove("error")
            }
        })
    });
}

if (mainForm) {
    mainForm.addEventListener("submit", (e) => {
        requiredInputes.forEach(input => {
            if (!input.value) {
                input.classList.add("error")
                e.preventDefault()
            }
            else {
                input.classList.remove("error")
            }
        })
    })
}