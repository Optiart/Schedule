function activateNavLink(id) {
    $(".nav-link").removeClass('active');
    $("#" + id).addClass('active');
}