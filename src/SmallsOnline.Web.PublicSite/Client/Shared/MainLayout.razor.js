export const scrollToAnchorId = (anchorId) => {
    let foundElement = document.getElementById(anchorId);

    if (foundElement == null) {
        throw `Could not find an element with the ID named '${anchorId}'.`;
    } else {
        foundElement.scrollIntoView();
    }
}