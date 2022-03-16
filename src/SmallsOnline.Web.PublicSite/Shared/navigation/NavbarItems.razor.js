export const setActiveClass = (elementId) => {
    let elem = document.getElementById(elementId);

    elem.classList.add("active");
};

export const removeActiveClass = (elementId) => {
    let elem = document.getElementById(elementId);

    elem.classList.remove("active");
};