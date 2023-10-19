var dataTable;

$(document).ready(function () {
    loadDataTables();
});

function loadDataTables() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/product/getall' },
        "columns": [
            { data: 'title', "width": "25%" },
            { data: 'isbn', "width": "15%" },
            { data: 'listPrice', "width": "10%" },
            { data: 'author', "width": "15%" },
            { data: 'category.name', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                    <a href="/product/upsert?id=${data}" class="btn btn-primary mx-2"> Edit </a>
                    <a onClick=Delete("/product/delete?id=${data}") class="btn btn-primary mx-2"> Delete </a>
                    </div>`
                },
                "width": "10%"
            }
        ]
    })
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire(
                $.ajax({
                    url: url,
                    type: 'DELETE',
                    success: function (data) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                })
            )
        }
    })
}



