
let btnClicked = document.querySelector("#mobile-button");
let m_menu = document.querySelector(".mobile-menu")

window.onload = function () {
    const mobileMenu = document.querySelector(".mobile-menu");
    m_menu.classList.add('invisible')
}

btnClicked.addEventListener('click', () => {
    if (m_menu.classList.contains('invisible')) {
        m_menu.classList.remove('invisible')
        m_menu.classList.add('visible')
        btnClicked.innerHTML = '<i class="fa-solid fa-xmark"></i>'
    } else {
        m_menu.classList.add('invisible')
        m_menu.classList.remove('visible')
        btnClicked.innerHTML = '<i class="fa-solid fa-bars-staggered"></i>'
    }
})

const checkScreenSize = () => {
    
    if (window.innerWidth >= 1400) {
        m_menu.classList.add('invisible')
        m_menu.classList.remove('visible')
        btnClicked.innerHTML = '<i class="fa-solid fa-bars-staggered"></i>'
  
    } 
}
window.addEventListener('resize', checkScreenSize);
checkScreenSize();