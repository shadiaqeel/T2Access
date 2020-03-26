


var selectGateTable;
ajaxUrl;
$(document).ready(function () {
    selectGateTable = $("#GTable").DataTable({
        serverSide: true,
        ajax: function (data, callback, settings) {

            $.get(ajaxUrl, data, function (result) {


                callback({
                    recordsTotal: result.recordsTotal,
                    recordsFiltered: result.recordsFiltered,
                    draw: result.draw,
                    data: result.data
                });
               // $("#tbodyPartial").html(result.view);


            });

        },
        deferRender: true,
        ordering: true,
        bFilter: true,
        bInfo: false,
        bAutoWidth: true,
        bSortable: true,
        paging: false,
        scrollY: "300px",
        scrollCollapse: true,
        multiSelect: false,
        aaSorting: false,
        scroller: {
            loadingIndicator: true
        }, createdRow: function (row, data, dataIndex) {

            if (data["Checked"]) {
                $(row).addClass('selected');
            }
        },
        columns: [
            {
                data:null,
                defaultContent:'',
                orderable: false,
                width: "1%",
                className: 'select-checkbox',
                checkboxes: { selectRow: true },

            },
            { data: "NameAr", name: "NameAr" },
            { data: "NameEn", name: "NameEn" }
        ],
        select: {
            style: 'multi',
            selector: 'td:first-child'

        }

    });

    $('#tbodyPartial').on('click', 'tr', function () {
        $(this).toggleClass('selected');
    });

    console.log(selectGateTable);

});





