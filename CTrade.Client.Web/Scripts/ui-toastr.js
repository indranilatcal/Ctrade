var UIToastr = function () {

    return {
        init: function () {


            toastr.options = {
                closeButton: true,
                debug: false,
                positionClass:  'toast-top-right',
                onclick: null,
                showDuration: 1000,
                showEasing: 'swing',
                showMethod: 'fadeIn',
                hideMethod: 'fadeOut',
                hideDuration: 1000,
                hideEasing: 'linear'

            };

        }

    };

}();