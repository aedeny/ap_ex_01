﻿$(document).ready(function() {
    $("ul.nav.navbar-nav").find('a[href="' + location.pathname + '"]')
        .closest("li").addClass("active");
});