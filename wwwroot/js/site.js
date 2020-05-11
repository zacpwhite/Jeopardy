// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

(function($){
    $(document).ready(function(e){
        var $modalLink = $('[data-toggle="modal"]');
        var $modalPlaceholder = $("#modal-placeholder");

        $modalLink.click(function(event){
            event.preventDefault();
            var url = $(this).data('url');
            $.get(url).done(function (data) {
                $modalPlaceholder.html(data);
                $modalPlaceholder.find('.modal').modal('show');
            });
        });

        $modalPlaceholder.on('click', '[data-save="modal"]', function(event){
            event.preventDefault();
            var $form = $(this).parents('.modal').find('form');
            var actionUrl = $form.attr('action');
            var formData = $form.serialize();
            $.post(actionUrl, formData).done(function(response){
                var responseHtml = $('.modal-body', response);
                var isValid = responseHtml.find('[name="IsValid"]').val() == 'True';
                if (isValid) {
                    $modalPlaceholder.find('.modal').modal('hide');
                } else {
                    $modalPlaceholder.find('.modal-body').replaceWith(responseHtml);
                }
            });
        });
    });
}(jQuery));