var TableEditable = function () {

    var handleTable = function () {

        function restoreRow(oTable, nRow) {

            var aData = oTable.fnGetData(nRow);
            if (nRow.className != 'group') {
                oTable.fnUpdate(aData["serialnum"], nRow, 3, false);
                oTable.fnUpdate(aData["tracking"], nRow, 4, false);
                oTable.fnUpdate(aData["shipdate"], nRow, 5, false);
                oTable.fnUpdate(aData["shipmethod"], nRow, 6, false);
            }
            else {
                nRow.cells[1].innerHTML = '';
                nRow.cells[2].innerHTML = '';
                nRow.cells[3].innerHTML = '';
                nRow.cells[4].innerHTML = '<a class="edit" href="javascript:;">Edit </a>';

                //var salesOrder = nRow.cells[0].innerHTML;
                //var table = $(oTable).dataTable().api();
                //for (i = 0; i < table.rows().data().length; i++) {
                //    var d = table.row(i).data();
                //    if (d.so == salesOrder) {
                //        var nRow = oTable.fnGetNodes(i);
                //        if (d.serialreq && (d.serialnum == null || d.serialnum == '')) {
                //            nRow.cells[2].innerHTML = '';
                //        }
                //        nRow.cells[6].innerHTML = '<a class="edit" href="javascript:;">Edit </a>';
                //    }

                //}

                //var salesOrder = nRow.cells[0].innerHTML;
                //for (i = 0; i < oTable.fnSettings().fnRecordsTotal(); i++)
                //{
                //    var nRow = oTable.fnGetNodes(i);
                //    if (nRow.cells[0].innerText == salesOrder)
                //        {

                //            if (d.serialreq && (d.serialnum == null || d.serialnum == '')) {
                //                nRow.cells[2].innerHTML = '';
                //            }
                //            nRow.cells[6].innerHTML = '<a class="edit" href="javascript:;">Edit </a>';
                //        }
                //}


            }

        }

        function editRow(oTable, nRow) {
            var aData = oTable.fnGetData(nRow);
            if (nRow.className == 'group') {

                nRow.cells[1].innerHTML = '<input type="text" class="form-control input-small" value="">';
                nRow.cells[2].innerHTML = '<div class="input-group input-group-sm date date-picker margin-bottom-5" data-date-format="dd/mm/yyyy"><input type="text" class="form-control form-filter" readonly name="ship_date" style="width: 100px;" value=""><span class="input-group-btn"><button class="btn btn-default" type="button"><i class="fa fa-calendar"></i></button></span></div>';

                var innerHTML = '<select class="form-control form-filter input-sm"><option value="">Select...</option>';
                innerHTML += '<option value="UPS">UPS</option>';
                innerHTML += '<option value="INSTORE PICKUP">INSTORE PICKUP</option>';
                innerHTML += '</select>';

                nRow.cells[3].innerHTML = innerHTML;
                //  nRow.cells[6].innerHTML = '<select class="form-control form-filter input-sm"><option value="">Select...</option><option value="UPS">UPS</option><option value="FED EX">FED EX</option></select>';
                nRow.cells[4].innerHTML = '<a class="edit" href="">Save</a>&nbsp;&nbsp;&nbsp;<a class="cancel" href="">Cancel</a>';

                // find rows belonging to sales order and show inputs on serial number if required for item
                //var salesOrder = nRow.cells[0].innerHTML;
                //var table = $(oTable).dataTable().api();
                //for (i = 0; i < table.rows().data().length; i++)
                //{
                //    var d = table.row(i).data();
                //    if (d.so == salesOrder)
                //    {
                //        var nRow = oTable.fnGetNodes(i);
                //        if (d.serialreq && (d.serialnum == null || d.serialnum == ''))
                //        {
                //            nRow.cells[2].innerHTML = '<input type="text" class="form-control input-small" value="">';                          
                //        }
                //            nRow.cells[6].innerHTML = '';
                //    }

                //}
            }

            else {

                if (aData["serialreq"]) {
                    nRow.cells[2].innerHTML = '<input type="text" class="form-control input-small" value="' + aData["serialnum"] + '">';
                }
                nRow.cells[3].innerHTML = '<input type="text" class="form-control input-small" value="' + aData["tracking"] + '">';

                nRow.cells[4].innerHTML = '<div class="input-group input-group-sm date date-picker margin-bottom-5" data-date-format="dd/mm/yyyy"><input type="text" class="form-control form-filter" readonly name="ship_date" style="width: 100px;" value="' + aData["shipdate"] + '"><span class="input-group-btn"><button class="btn btn-default" type="button"><i class="fa fa-calendar"></i></button></span></div>';

                // $('#table_editable tr:nth-child(' + 3 + ') td:nth-child(6)').html('<div class="input-group input-group-sm date date-picker margin-bottom-5" data-date-format="dd/mm/yyyy"><input type="text" class="form-control form-filter" readonly name="ship_date" style="width: 100px;"><span class="input-group-btn"><button class="btn btn-default" type="button"><i class="fa fa-calendar"></i></button></span></div>');
                var innerHTML = '<select class="form-control form-filter input-sm"><option value="">Select...</option>';
                innerHTML += aData["shipmethod"] == 'UPS' ? '<option value="UPS" selected>UPS</option>' : '<option value="UPS">UPS</option>';
                innerHTML += aData["shipmethod"] == 'INSTORE PICKUP' ? '<option value="INSTORE PICKUP" selected>INSTORE PICKUP</option>' : '<option value="INSTORE PICKUP">INSTORE PICKUP</option>';
                innerHTML += '</select>';

                nRow.cells[5].innerHTML = innerHTML;
                //  nRow.cells[6].innerHTML = '<select class="form-control form-filter input-sm"><option value="">Select...</option><option value="UPS">UPS</option><option value="FED EX">FED EX</option></select>';
                nRow.cells[6].innerHTML = '<a class="edit" href="">Save</a>&nbsp;&nbsp;&nbsp;<a class="cancel" href="">Cancel</a>';


            }

            $('.date-picker').datepicker({
                autoclose: true,
                format: 'mm/dd/yyyy'
            });


        }

        function saveRow(oTable, nRow) {

            var salesOrder = nRow.cells[0].innerHTML;

            //var jqSelect = $('select', nRow);
            //var jqSelectValue = jqSelect[0].value;

            //var jqInputs = $('input', nRow);
            //var reqSerialRows = [];

            var inputs = $('select, input', nRow)

            var table = $('#table_editable').dataTable().api();
            if (nRow.className == 'group') {


                //var total = oTable.fnSettings().fnRecordsTotal();

                for (i = 0; i < table.rows().data().length; i++) {
                    // var row = oTable.fnGetData(i);
                    var d = table.row(i).data();
                    if (d.so == salesOrder) {

                        if (d.tracking == null || d.tracking == '') {
                            d.tracking = inputs[0].value;
                        }
                        if (d.shipdate == null || d.shipdate == '') {
                            d.shipdate = inputs[1].value;
                        }
                        if (d.shipmethod == null || d.shipmethod == '') {
                            d.shipmethod = inputs[2].value;
                        }

                        //d.shipdate = jqInputs[1].value;
                        //d.shipmethod = jqSelectValue;
                        //if (row["so"] == salesOrder)
                        table.row(i).data(d);

                        //if (d.serialreq && (d.serialnum == null || d.serialnum == '')) {
                        //    table.row(i).cells[2].innerHTML = '<input type="text" class="form-control input-small" value="">';

                        //}
                    }
                    //{
                    //    if (row["tracking"] == '')
                    //    {
                    //        // logic for formatting tracking url
                    //        oTable.fnUpdate(jqInputs[0].value, row, 4, false);
                    //    }
                    //    if (row["shipdate"] == '') {
                    //        // logic for formatting tracking url
                    //        oTable.fnUpdate(jqInputs[1].value, row, 5, false);
                    //    }
                    //    if (row["shipmethod"] == '') {
                    //        // logic for formatting tracking url
                    //        oTable.fnUpdate(jqSelectValue, row, 6, false);
                    //    }

                    //}

                    //if (d.serialreq && (d.serialnum == null || d.serialnum == '')) {
                    //    var nRow = oTable.fnGetNodes(i);
                    //    nRow.cells[2].innerHTML = '<input type="text" class="form-control input-small" value="">';
                    //    nRow.cells[6].innerHTML = '<a class="edit" href="">Save</a>&nbsp;&nbsp;&nbsp;<a class="cancel" href="">Cancel</a>';
                    //    if (nEditing == null)
                    //    {
                    //        nEditing = nRow;
                    //    }
                    //}

                }
                table.draw();



                var salesOrder = nRow.cells[0].innerHTML;
                var table = $(oTable).dataTable().api();
                for (i = 0; i < table.rows().data().length; i++) {
                    var d = table.row(i).data();
                    if (d.so == salesOrder) {
                        var nRow = oTable.fnGetNodes(i);

                        if (d.serialreq && (d.serialnum == null || d.serialnum == '')) {
                            nRow.cells[2].innerHTML = '<input type="text" class="form-control input-small" value="">';
                            nRow.cells[6].innerHTML = '<a class="edit" href="">Save</a>&nbsp;&nbsp;&nbsp;<a class="cancel" href="">Cancel</a>';
                        }

                    }

                }


                //oTable.fnDraw();
                // change serial number(s) to editable fields if required
                //for (i = 0; i < reqSerialRows.length; i++)
                //{

                // table.row(0).cells[2].innerHTML = '<input type="text" class="form-control input-small" value="">';


                //    table.row(reqSerialRows[i]).cells[2].innerHTML = '<input type="text" class="form-control input-small" value="">';
                //}
            }
            else {

                var r = table.rows(nRow).data();
                if (r[0].serialreq) {
                    r[0].serialnum = inputs[0].value;

                    if (inputs.length > 1) {
                        r[0].tracking = inputs[1].value;
                        r[0].shipdate = inputs[2].value;
                        r[0].shipmethod = inputs[3].value;
                    }
                    table.rows(nRow).data(r[0]);

                    //oTable.fnUpdate(inputs[0].value, nRow, 3, false);
                    //oTable.fnUpdate(inputs[1].value, nRow, 4, false);
                    //oTable.fnUpdate(inputs[2].value, nRow, 5, false);
                    //oTable.fnUpdate(inputs[3].value, nRow, 6, false);
                }
                else {
                    r[0].tracking = inputs[0].value;
                    r[0].shipdate = inputs[1].value;
                    r[0].shipmethod = inputs[2].value;

                    //oTable.fnUpdate(inputs[0].value, nRow, 4, false);
                    //oTable.fnUpdate(inputs[1].value, nRow, 5, false);
                    //oTable.fnUpdate(inputs[2].value, nRow, 6, false);
                }

                table.draw();

                oTable.fnUpdate('<a class="edit" href="">Edit</a>', nRow, 7, false);
                oTable.fnDraw();
            }


            //.dataTable().fnUpdate('Zebra' , $('tr#3692')[0], 1 )
            DisplayConfirmation('success', 'Confirmation', 'Shipping information has been updated');
        }

        function DisplayConfirmation(type, title, msg) {

            // toaster notification
            switch (type) {
                case 'success':
                    toastr.success(msg, title);
                    break;
                case 'warning':
                    toastr.warning(msg, title);
                    break;
                case 'error':
                    toastr.error(msg, title);
                    break;
                case 'info':
                    toastr.info(msg, title);
                    break;
                default:
                    break;
            }


        }

        function cancelEditRow(oTable, nRow) {
            var jqInputs = $('input', nRow);
            oTable.fnUpdate(jqInputs[0].value, nRow, 3, false);
            oTable.fnUpdate(jqInputs[1].value, nRow, 4, false);
            oTable.fnUpdate(jqInputs[2].value, nRow, 5, false);
            oTable.fnUpdate(jqInputs[3].value, nRow, 6, false);
            oTable.fnUpdate('<a class="edit" href="">Edit</a>', nRow, 7, false);
            oTable.fnDraw();
        }

        var table = $('#table_editable');

        var oTable = table.dataTable({


            data: data,
            columns:
            [
               { data: 'so' },
               { data: 'po' },
               { data: 'desc' },
               { data: 'serialnum' },
               { data: 'tracking' },
               { data: 'shipdate' },
               { data: 'shipmethod' },

            ],

            "bLengthChange": false,

            // set the initial value
            "pageLength": 5,



            //"language": {   
            //    "lengthMenu": " _MENU_ records"
            //},
            "columnDefs": [
                { // set default column settings
                    'orderable': true,
                    'targets': [1]

                },
                {
                    'targets': [7],
                    "defaultContent": "<a class='edit' href='javascript:;'>Edit </a>"
                }
            ,
            {
                "searchable": true,
                "targets": [1]
            }
            , { "visible": false, "targets": [0] }
            ],

            "drawCallback": function (settings) {

                var api = this.api();
                var rows = api.rows({ page: 'current' }).nodes();

                var last = null;

                api.column(0, { page: 'current' }).data().each(function (group, i) {
                    if (last !== group) {
                        $(rows).eq(i).before(
                           '<tr class="group"><td colspan="3">' + group + '</td><td></td><td></td><td></td>  <td><a class="edit" href="javascript:;">Edit </a></td></tr>'


                        )

                        last = group;
                    }
                });
            },



            "order": [
                [0, "asc"]
            ] // set first column as a default sort ascending
        });


        // Order by the grouping
        //$('#table_editable tbody').on('click', 'tr.group', function () {
        //    var currentOrder = oTable.order()[0];  // object doesnt support property or method 'order'
        //    if (currentOrder[0] === 0 && currentOrder[1] === 'asc') {
        //        oTable.order([0, 'desc']).draw();
        //    }
        //    else {
        //        oTable.order([0, 'asc']).draw();
        //    }
        //});

        var tableWrapper = $("#table_editable_wrapper");

        tableWrapper.find(".dataTables_length select").select2({
            showSearchInput: false //hide search box with special css class
        }); // initialize select2 dropdown

        var nEditing = null;
        var nNew = false;
        var nCurrentEdit = null;


        $('#table_editable_new').click(function (e) {
            e.preventDefault();

            if (nNew && nEditing) {
                if (confirm("Previose row not saved. Do you want to save it ?")) {
                    saveRow(oTable, nEditing); // save
                    $(nEditing).find("td:first").html("Untitled");
                    nEditing = null;
                    nNew = false;

                } else {
                    oTable.fnDeleteRow(nEditing); // cancel
                    nEditing = null;
                    nNew = false;

                    return;
                }
            }

            var aiNew = oTable.fnAddData(['', '', '', '', '', '']);
            var nRow = oTable.fnGetNodes(aiNew[0]);
            editRow(oTable, nRow);
            nEditing = nRow;
            nNew = true;
        });

        table.on('click', '.delete', function (e) {
            e.preventDefault();

            if (confirm("Are you sure to delete this row ?") == false) {
                return;
            }

            var nRow = $(this).parents('tr')[0];
            oTable.fnDeleteRow(nRow);
            alert("This is a demo. No data will not be deleted");
        });

        table.on('click', '.cancel', function (e) {
            e.preventDefault();

            if (nNew) {
                oTable.fnDeleteRow(nEditing);
                nNew = false;
            } else {

                if (nEditing == null) { nEditing = $(this).parents('tr')[0]; }
                restoreRow(oTable, nEditing);
                nEditing = null;
            }
        });

        table.on('click', '.edit', function (e) {
            e.preventDefault();

            /* Get the row as a parent of the link that was clicked on */
            var nRow = $(this).parents('tr')[0];

            if (nEditing !== null && nEditing != nRow) {
                nCurrentEdit = nRow;
                /* Currently editing - but not this row - restore the old before continuing to edit mode */
                $('#confirmation')
                .find('.modal-body').html('The changes you are making have not been saved. Do you wish to continue?').end()
                .find('.btn-info').show().end()
                .find('.btn-default').show().end()
                //.find('.btn-info').on('click.callback')
                .modal({ backdrop: 'static', keyboard: false })
                .on('click', '[data-value]', function (e) {
                    if ($(this).data('value')) {
                        restoreRow(oTable, nEditing);
                        editRow(oTable, nCurrentEdit);
                        nEditing = nCurrentEdit;
                    } else {
                        return;
                    }
                });



            }
                //else if (nEditing == nRow && this.innerHTML == "Save")
            else if (this.innerHTML == "Save") {
                /* Editing this row and want to save it */
                if (nEditing == null) { nEditing = $(this).parents('tr')[0]; }

                //************
                var inputs = $('select, input', nRow);
                if (nRow.className == 'group') {
                    if (inputs[2].value == 'INSTORE PICKUP' && (inputs[0].value == '' || inputs[1].value == '')) {
                        $('#confirmation')
                            .modal({ backdrop: 'static', keyboard: false })
                             .find('.modal-body').html('You must indicate the individual who picked up the order under Tracking and the Date Shipped when selecting INSTORE PICKUP.').end()
                             .find('.btn-default').hide().end()
                                .find('.btn-info').show().end()
                        return;

                        //.on('click', '[data-value]', function (e) {
                        //    if ($(this).data('value')) {
                        //        saveRow(oTable, nEditing);
                        //        nEditing = null;
                        //        alert("This is a demo. No data will not be updated");

                        //    } else {
                        //        return;
                        //    }
                        //});
                    }
                    // validate serial number(s) indicated if required
                    //for (i = 0; i < oTable.fnSettings().fnRecordsTotal() ; i++)
                    //{

                    //}
                    //var salesOrder = nRow.cells[0].innerHTML;
                    //var table = $(oTable).dataTable().api();
                    //for (i = 0; i < table.rows().data().length; i++) {
                    //var d = table.row(i).data();
                    //var nRow = oTable.fnGetNodes(i);
                    //var inputs = $('input', nRow);
                    //if (d.so == salesOrder && chas[0].value == '')
                    //if (d.so == salesOrder && d.serialnum == '')
                    //{
                    //if (inputs[0].value == '')
                    //{
                    //if (nRow.cells[0] == '')
                    //{
                    // warning message
                    $('#confirmation')
    .find('.modal-body').html('You must provide a serial number where indicated before the order will be moved to ship status.').end()
    .find('.btn-default').hide().end()
    .modal({ backdrop: 'static', keyboard: false })
    .one('click', '[data-value]', function (e) {
        if ($(this).data('value')) {
            saveRow(oTable, nEditing);
            nEditing = null;
            // return;
            //} else {
            //    return;
        }
    });


                    // .find('.btn-default').off('click.callback')
                    //return;
                    //  break;



                    //}
                    //}

                    //}

                    //}

                }  // if not group
                else {

                    if (inputs.length == 4) {
                        if (inputs[3].value == 'INSTORE PICKUP' && (inputs[1].value == '' || inputs[2].value == '')) {
                            $('#confirmation')
                                .modal({ backdrop: 'static', keyboard: false })
                                 .find('.modal-body').html('You must indicate the individual who picked up the order under Tracking and the Date Shipped when selecting INSTORE PICKUP.').end()
                                .find('.btn-default').hide().end()
                                .find('.btn-info').show().end()
                                .find('.btn-info').off('click.callback')
                            return;
                        }



                    }
                    if (inputs.length == 3) {
                        if (inputs[2].value == 'INSTORE PICKUP' && (inputs[0].value == '' || inputs[1].value == '')) {
                            $('#confirmation')
                                .modal({ backdrop: 'static', keyboard: false })
                                 .find('.modal-body').html('You must indicate the individual who picked up the order under Tracking and the Date Shipped when selecting INSTORE PICKUP.').end()
                                .find('.btn-info').hide().end()
                                .find('.btn-info').off('click.callback')
                            return;
                        }


                    }

                    if (inputs.length == 1) {
                        if (inputs[0].value == '') {
                            $('#confirmation')
                                .modal({ backdrop: 'static', keyboard: false })
                                 .find('.modal-body').html('You must provide a serial number before saving changes.').end()
                                .find('.btn-default').hide().end()
                                .find('.btn-info').show().end()
                                .find('.btn-info').off('click.callback')
                            return;
                        }


                    }





                    saveRow(oTable, nEditing);
                    nEditing = null;
                }
                //}
                // **********
                //saveRow(oTable, nEditing);
                //nEditing = null;
                // alert("This is a demo. No data will not be updated");
            } else {
                /* No edit in progress - let's start one */
                editRow(oTable, nRow);
                nEditing = nRow;
            }
        });
    }

    return {

        //main function to initiate the module
        init: function () {
            handleTable();
        }

    };

}();

var data =
    [
    {
        "so": "ACS8IN",
        "po": "8585855",
        "desc": "Surface Pro 3 – 128 GB model",
        "serialnum": "7777-33-223",

        "serialreq": 1,
        "tracking": "",
        "shipdate": "10/1/2014",
        "shipmethod": "UPS"
    },
        {
            "so": "ACS8IN",
            "po": "8585855",
            "desc": "Surface Pro 3 – 256 GB model",
            "serialnum": "",

            "serialreq": 1,
            "tracking": "",
            "shipdate": "",
            "shipmethod": ""
        },
    {
        "so": "ACS8IN",
        "po": "8585855",
        "desc": "Arc Touch Mouse Surface Edition",
        "serialnum": "",
        "serialreq": 0,
        "tracking": "",
        "shipdate": "",
        "shipmethod": ""
    },
        {
            "so": "ACS8IN",
            "po": "8585855",
            "desc": "HDMI adapter",
            "serialnum": "",
            "serialreq": 0,
            "tracking": "",
            "shipdate": "",
            "shipmethod": ""
        },
            {
                "so": "LDS3333",
                "po": "674444",
                "desc": "Surface Pro 3 dock",
                "serialnum": "",
                "serialreq": 0,
                "tracking": "",
                "shipdate": "",
                "shipmethod": ""
            },
                {
                    "so": "LDS3333",
                    "po": "389844",
                    "desc": "Surface Pro 3 – 256 GB model",
                    "serialnum": "",
                    "serialreq": 1,
                    "tracking": "",
                    "shipdate": "",
                    "shipmethod": ""
                },
                {
                    "so": "WSS8884",
                    "po": "674444",
                    "desc": "Type Cover 3 – purple",
                    "serialnum": "",
                    "serialreq": 0,
                    "tracking": "",
                    "shipdate": "",
                    "shipmethod": ""
                },
                   {
                       "so": "WSS8884",
                       "po": "674444",
                       "desc": "Surface Pro 3 – 256 GB model",
                       "serialnum": "",
                       "serialreq": 1,
                       "tracking": "",
                       "shipdate": "",
                       "shipmethod": ""
                   }
    ];


