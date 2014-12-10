var controls = (function ($) {
    var DateRange = function (elementToWrap) {
        var dateFormat = 'MM/DD/YYYY';
        if (!jQuery().daterangepicker)
            return;
        $(elementToWrap).daterangepicker({
            format: dateFormat
        });
        var picker = $(elementToWrap).data('daterangepicker');
        var inputElement = elementToWrap + ' input';

        function setInput() {
            $(inputElement).val(picker.startDate.format(dateFormat) + ' - ' + picker.endDate.format(dateFormat));
        }

        $(elementToWrap).on('apply.daterangepicker', function (event, picker) {
            setInput();
        });

        this.startDate = function () {
            return picker.startDate;
        };
        this.setDates = function (start, end) {
            picker.setStartDate(moment(start));
            picker.setEndDate(moment(end));
            setInput();
        };
        this.endDate = function () {
            return picker.endDate;
        };
        this.setEndDate = function (end) {
            picker.setEndDate(moment(end));
        };
        this.disable = function (disabled) {
            $(elementToWrap).find('input,button').prop('disabled', disabled);
        };
        this.clear = function () {
            $(inputElement).val('');
            var now = moment();
            picker.setStartDate(now);
            picker.setEndDate(now);
        };
    };

    var Editor = function (textAreaToWrap) {
        if (!jQuery().wysihtml5)
            return;

        $(textAreaToWrap).wysihtml5({
            "stylesheets": ["/Content/Styles/plugins/wysiwyg-color.css"],
            "emphasis": true,
            "color": true
        });

        var editor = $(textAreaToWrap).data("wysihtml5").editor;

        this.setValue = function (text) {
            editor.setValue(text);
        };
        this.clear = function () {
            editor.clear();
        };
        this.val = function () {
            return $(textAreaToWrap).val();
        };
    };

    return {
        DateRange: DateRange,
        Editor: Editor
    };
})(jQuery);