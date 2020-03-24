



var dataTableConfig = {
    bFilter: true,
    bInfo: true,
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


var selectGateTable;

$(document).ready(function () {
     selectGateTable = $("#GTable").DataTable(dataTableConfig);

});



