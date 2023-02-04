
function myDropdownUser() {
    document.getElementById("myDropdown-user").classList.toggle("show");
}

window.onclick = function (event) {
    if (!event.target.matches('.drop-user-btn')) {
        var dropdowns = document.getElementsByClassName("dropdown-user-content");
        var i;
        for (i = 0; i < dropdowns.length; i++) {
            var openDropdown = dropdowns[i];
            if (openDropdown.classList.contains('show')) {
                openDropdown.classList.remove('show');
            }
        }
    }
}