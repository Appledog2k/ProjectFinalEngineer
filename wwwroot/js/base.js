$(document).ready(function () {
  navbarActive();
  openMenu();
});
function openMenu() {
  $("#menu-icon").click(function () {
    $(this).toggleClass("bx-x");
    $(".navbar").toggleClass("open");
  });
}
function navbarActive() {
  $(this).on("click", "ul li", function () {
    $(this).addClass("active").siblings().removeClass("active");
  });
}
