



var dataTableConfig = {
    bFilter: false,
    bInfo: false,
    bAutoWidth: true,
    bSortable: false,
    scrollY: "400px",
    scrollCollapse: true,
    paging: false,
    multiSelect:true,
    columnDefs: [{
        targets: 0,
        orderable: false,
        width: "1%",
        className: 'select-checkbox',
        checkbox: {
            selectrow: true
        }
    }],
    select: {
        style: 'multi',
        selector: 'td:first-child'
    },
    order: [[1, 'asc']]

}

$(document).ready(function () {
    $("#GTable").DataTable(dataTableConfig);
    console.log($("#GTable").DataTable());

});


