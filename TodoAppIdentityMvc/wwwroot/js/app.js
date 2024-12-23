document.addEventListener("DOMContentLoaded", () => {
    const btns = document.querySelectorAll(".btn-has-sub-menu")
    btns.forEach(btn => {
        btn.addEventListener("click", (e) => {
            const isExapned = e.target.getAttribute("aria-expanded")
            // const isSubmenuHidden = e.target.nextElementSibling.getAttribute("aria-hidden")
            const submenuBtn = e.target.closest(".btn-has-sub-menu")
            const submenuContainer = submenuBtn.parentElement.querySelector(".aside__sub-menu");

            isExapned == "false" ? btn.setAttribute("aria-expanded", true) : btn.setAttribute("aria-expanded", false)
            submenuContainer.getAttribute("aria-hidden") == "true" ? submenuContainer.setAttribute("aria-hidden", false) : submenuContainer.setAttribute("aria-hidden", true)
        })
    })

    // Setting action 
    const settingBtn = document.querySelector(".aside__setting-btn")
    const themeWrapper = document.querySelector(".theme-wrapper")

    
    // --Close the menu on outside click
    function ResetAriaValues() {
        settingBtn.setAttribute("aria-expanded", false)
    }
    document.addEventListener("click", (e) => {
        if(!themeWrapper.contains(e.target) && !settingBtn.contains(e.target)) {
            ResetAriaValues()
        }
    })

    settingBtn.addEventListener("click", () => {
        const isBtnExpanded = settingBtn.getAttribute("aria-expanded")
        isBtnExpanded == "false" ? settingBtn.setAttribute("aria-expanded", true) : settingBtn.setAttribute("aria-expanded", false)
    })
    
    // Theme actions
    const mainHtml= document.querySelector("html")
    const themeBtns = document.querySelectorAll(".theme-btn")
    // const theme = JSON.parse(localStorage.getItem("theme")) || "light"
    // mainHtml.classList.add(theme)
    
    themeBtns.forEach(btn => {
        btn.addEventListener("click", (e) => {
            const btnValue = e.target.getAttribute("data-label")
            localStorage.setItem("theme", JSON.stringify(btnValue))
            mainHtml.classList.remove("light", "dark")
            mainHtml.classList.add(btnValue)
            ResetAriaValues()
        })
    })
})