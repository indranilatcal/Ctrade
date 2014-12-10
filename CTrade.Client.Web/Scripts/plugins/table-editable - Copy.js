var TableEditable = function () {

    var handleTable = function () {

        function restoreRow(oTable, nRow) {
            var aData = oTable.fnGetData(nRow);
            var jqTds = $('>td', nRow);

            for (var i = 0, iLen = jqTds.length; i < iLen; i++) {
                oTable.fnUpdate(aData[i], nRow, i, false);
            }

            oTable.fnDraw();
        }

        function editRow(oTable, nRow) {
            var aData = oTable.fnGetData(nRow);
            //var jqTds = $('>td', nRow);
            nRow.cells[3].innerHTML = '<input type="text" class="form-control input-small" value="' + aData[3] + '">';
            nRow.cells[4].innerHTML = '<input type="text" class="form-control input-small" value="' + aData[4] + '">';

            nRow.cells[5].innerHTML = '<div class="input-group input-group-sm date date-picker margin-bottom-5" data-date-format="dd/mm/yyyy"><input type="text" class="form-control form-filter" readonly name="ship_date" style="width: 100px;" value="' + aData[5] + '"><span class="input-group-btn"><button class="btn btn-default" type="button"><i class="fa fa-calendar"></i></button></span></div>';

           // $('#sample_editable_1 tr:nth-child(' + 3 + ') td:nth-child(6)').html('<div class="input-group input-group-sm date date-picker margin-bottom-5" data-date-format="dd/mm/yyyy"><input type="text" class="form-control form-filter" readonly name="ship_date" style="width: 100px;"><span class="input-group-btn"><button class="btn btn-default" type="button"><i class="fa fa-calendar"></i></button></span></div>');
            var innerHTML = '<select class="form-control form-filter input-sm"><option value="">Select...</option>';
            innerHTML += aData[6] == 'FED EX' ? '<option value="FED EX" selected>FED EX</option>' : '<option value="FED EX">FED EX</option>';
            innerHTML += aData[6] == 'UPS' ? '<option value="UPS" selected>UPS</option>' : '<option value="UPS">UPS</option>';
            innerHTML += '</select>';

            nRow.cells[6].innerHTML = innerHTML;
          //  nRow.cells[6].innerHTML = '<select class="form-control form-filter input-sm"><option value="">Select...</option><option value="UPS">UPS</option><option value="FED EX">FED EX</option></select>';
            nRow.cells[7].innerHTML = '<a class="edit" href="">Save</a>&nbsp;&nbsp;&nbsp;<a class="cancel" href="">Cancel</a>';


            $('.date-picker').datepicker({
                autoclose: true,
                format: 'mm/dd/yyyy'
            });

            
        }

        function saveRow(oTable, nRow) {
            var jqSelect = $('select', nRow);
            var jqSelectValue = jqSelect[0].value;

            var jqInputs = $('input', nRow);
            oTable.fnUpdate(jqInputs[0].value, nRow, 3, false);
            oTable.fnUpdate(jqInputs[1].value, nRow, 4, false);
            oTable.fnUpdate(jqInputs[2].value, nRow, 5, false);
            oTable.fnUpdate(jqSelectValue, nRow, 6, false);
            oTable.fnUpdate('<a class="edit" href="">Edit</a>', nRow, 7, false);
            oTable.fnDraw();
     
            DisplayConfirmation('success', 'Confirmation', 'Shipping information has been updated');
        }

        function DisplayConfirmation(type, title, msg)
        {
            // toaster notification
            switch (type)
            {
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
            
            //toastr.clear();
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

       


        var table = $('#sample_editable_1');

        var oTable = table.dataTable({
           

            "bLengthChange": false,

            // set the initial value
            "pageLength": 15,

            //"language": {   
            //    "lengthMenu": " _MENU_ records"
            //},
            "columnDefs": [{ // set default column settings
                'orderable': true,
                'targets': [0]
            }, {
                "searchable": true,
                "targets": [0]
            }],
            "order": [
                [0, "asc"]
            ] // set first column as a default sort by asc
        });

        var tableWrapper = $("#sample_editable_1_wrapper");

        tableWrapper.find(".dataTables_length select").select2({
            showSearchInput: false //hide search box with special css class
        }); // initialize select2 dropdown

        var nEditing = null;
        var nNew = false;

        $('#sample_editable_1_new').click(function (e) {
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
                DisplayConfirmation('warning', 'Cancelled', 'Shipping information has not been updated');
                restoreRow(oTable, nEditing);
                nEditing = null;
            }
        });

        table.on('click', '.edit', function (e) {
            e.preventDefault();

            /* Get the row as a parent of the link that was clicked on */
            var nRow = $(this).parents('tr')[0];

            if (nEditing !== null && nEditing != nRow) {
                /* Currently editing - but not this row - restore the old before continuing to edit mode */
                restoreRow(oTable, nEditing);
                editRow(oTable, nRow);
                nEditing = nRow;
            } else if (nEditing == nRow && this.innerHTML == "Save") {
                /* Editing this row and want to save it */
                saveRow(oTable, nEditing);
                nEditing = null;
                alert("This is a demo. No data will not be updated");
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