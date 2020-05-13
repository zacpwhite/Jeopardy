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
                var $form = $modalPlaceholder.find('form');
                $.validator.unobtrusive.parse($form);
            });
        });

        $modalPlaceholder.on('click', '[data-save="modal"]', function(event){
            event.preventDefault();
            var $form = $(this).parents('.modal').find('form');
            var actionUrl = $form.attr('action');
            var formData = $form.serialize();
            $.post(actionUrl, formData).done(function(response){
                if (response.indexOf('<div') > -1) {
                    var responseHtml = $('.modal-body', response);
                    $modalPlaceholder.find('.modal-body').replaceWith(responseHtml); 
                } else {
                    $modalPlaceholder.find('.modal').modal('hide');
                    window.location.href = response;
                }
            });
        });
    });
}(jQuery));