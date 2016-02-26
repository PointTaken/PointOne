var dropdownToggle = $('.dropdown-toggle');
var dropdown = dropdownToggle.next('.dropdown');
$(function () {
    dropdown.slideUp(0);
    $(document).on('click', function (e) {
        var target = e.target;
        if (!$(target).is('.dropdown-toggle') && !$(target).parents().is('.dropdown-toggle')) {
            if (dropdown.is(':visible')) {
                dropdown.stop(true).slideUp();
            }
        }
    });
    dropdownToggle.on('click', function () {
        dropdown.stop(true).slideToggle();
    });
});