function Dropdown() {
    $(document).ready(function () {
        $('.dropdown-menu a.dropdown-toggle').mouseover('click', function (e) {
            var $el = $(this);
            $el.toggleClass('active-dropdown');
            var $parent = $(this).offsetParent(".dropdown-menu");
            if (!$(this).next().hasClass('show')) {
                $(this).parents('.dropdown-menu').first().find('.show').removeClass("show");
            }
            var $subMenu = $(this).next(".dropdown-menu");
            $subMenu.toggleClass('show');

            $(this).parent("li").toggleClass('show');

            $(this).parents('li.nav-item.dropdown.show').on('hidden.bs.dropdown', function (e) {
                $('.dropdown-menu .show').removeClass("show");
                $el.removeClass('active-dropdown');
            });

            if (!$parent.parent().hasClass('navbar-nav')) {
                $el.next().css({ "top": $el[0].offsetTop, "right": $parent.outerWidth(), "width": "250px", "text-align": "right" });
            }

            return false;
        });
    });
}