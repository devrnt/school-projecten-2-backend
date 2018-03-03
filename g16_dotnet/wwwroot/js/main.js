window.onload = () => {
    let spans = document.querySelectorAll('.close');
    console.log(spans);
    spans.forEach(span => span.addEventListener('click', event => {
        closeAlert(event.target.parentNode);
    }));

    function closeAlert(node) {
        node.style.display = 'none';
    }

}