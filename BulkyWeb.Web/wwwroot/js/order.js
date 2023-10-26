var dataTable;

$(document).ready(function () {
    var url = window.location.search
    if (url.includes("inprocess")) {
        loadDataTables("inprocess");
    } else if (url.includes("approved")) {
        loadDataTables("approved");
    } else if (url.includes("completed")) {
        loadDataTables("completed");
    } else if (url.includes("pending")) {
        loadDataTables("pending");
    } else {
        loadDataTables("all");
    }
});

function loadDataTables(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/order/getall?status=' + status},
        "columns": [
            { data: 'id', "width": "25%" },
            { data: 'name', "width": "15%" },
            { data: 'phoneNumber', "width": "10%" },
            { data: 'applicationUser.email', "width": "15%" },
            { data: 'orderStatus', "width": "10%" },
            { data: 'orderTotal', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                    <a href="/order/details?orderId=${data}" class="btn btn-primary mx-2"> Edit </a>
                    </div>`
                },
                "width": "10%"
            }
        ]
    })
}



