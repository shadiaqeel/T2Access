


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
        "ordering": false,
        bFilter: true,
        bInfo: false,
        bAutoWidth: true,
        bSortable: true,
        bPaginate: false,
        sScrollY: "50vh",
        sScrollX: "90%",
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
                data: null,
                defaultContent: '',
                orderable: false,
                width: "1%",
                className: 'select-checkbox',
                checkboxes: { selectRow: true },

            },
            { data: "NameAr", name: "NameAr", autoWidth: true },
            { data: "NameEn", name: "NameEn", autoWidth: true }
        ],
        select: {
            style: 'multi',
            selector: 'tds'

        },
        preDrawCallback: function () {

            $('div.dataTables_filter input').addClass("form-control form-control-sm");
            $('div.dataTables_filter label').addClass("pull-away");

        },
        language:
        {
            "search": search,
            "zeroRecords": zeroRecords
        }
        

    });

    $('#tbodyPartial').on('click', 'tr', function () {
        $(this).toggleClass('selected');
    });


});





