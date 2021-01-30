// Listen for click events on buttons matching the AJAX modal toggle attribute
$(function () {
    var modalTarget = $('#modal-target')

    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            modalTarget.html(data);
            modalTarget.find('.modal').modal('show');
        });
    });

    // Listen for the save event on the form
    modalTarget.on('click', '[data-save="modal"]', function (event) {
        event.preventDefault();

        // Find the form data and prepare for AJAX post
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var payload = form.serialize();
        var currentUrl = window.location.pathname;
        var clientId = currentUrl.slice(currentUrl.lastIndexOf('/') + 1);
        actionUrl = actionUrl + "?clientId=" + clientId;

        $.post(actionUrl, payload).done(function (data) {
            // Replace the modal content with the response data, to persist validation errors until the form is valid
            var modalBody = $('.modal-body', data);
            modalTarget.find('.modal-body').replaceWith(modalBody);

            var isValid = modalBody.find('[name="is-valid"]').val() == 'True';
            if (isValid) {
                modalTarget.find('.modal').modal('hide');
            }
        });
    });
});