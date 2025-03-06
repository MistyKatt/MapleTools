document.addEventListener('DOMContentLoaded', function () {
    // Initialize all collapses as open
    $('.collapse').collapse('show');
    $('#toggleCollapse').text("hide all");
});

// Handle toggle rotation
$('[data-toggle="collapse"]').on('click', function () {
    $(this).toggleClass('collapsed');
});

// Remove accordion behavior
$('.collapse').on('show.bs.collapse', function () {
    $(this).parent().find('.collapse').collapse('hide');
});

$('#toggleCollapse').click(function () {
    if ($(this).hasClass('expanded')) {
        $(this).removeClass('expanded');
        $(this).text("show all");
        $('.collapse').collapse('hide');
        $('[data-toggle="collapse"]').addClass('collapsed');
    }
    else {
        $(this).addClass('expanded');
        $(this).text("hide all");
        $('.collapse').collapse('show');
        $('[data-toggle="collapse"]').removeClass('collapsed');
    }
});

