var commonUtils = (function ($) {
    var showErrors = function (errorMessages) {
        var summary = $('.validationSummary');
        if (errorMessages && errorMessages.length > 0) {
            summary.html('<ul></ul>');
            var errorMessage = '<ul>';
            for (var i = 0; i < errorMessages.length; i++)
                summary.append('<li>' + errorMessages[i] + '</li>');

            summary.show();
        }
    };

    var showError = function (errorMessage) {
        var summary = $('.validationSummary');
        if (errorMessage){
            summary.html(errorMessage);
            summary.show();
        }
    };

    var clearErrors = function () {
        $('.has-error').removeClass('has-error');
        $('.validationSummary').hide();
    }

    return {
        showErrors: showErrors,
        showError: showError,
        clearErrors: clearErrors
    };
})(jQuery);