﻿$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            url: '/admin/product/getall',
            dataSrc: 'data' // Specify the data source property from JSON response
        },
        "columns": [
            { data: 'title', "width": '25%' },
            { data: 'isbn', "width": '15%' },
            { data: 'listPrice', "width": '10%' },
            { data: 'author', "width": '20%' },
            { data: 'category.name', "width": '15%' },
            {
                data: 'id',
                render: function (data) {
                    return `<div class="w-75 btn-group" role="group" >
                    <a href="/admin/product/upsert?id=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i> Edit </a>
                     <a onClick=Delete("/admin/product/delete/${data}") class="btn btn-danger mx-2"><i class="bi bi-trash-fill"></i> Delete </a>
                    </div>`
                },
                "width": '15%'
            }
        ]
    });
}
function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                },
                error: function (error) {
                    toastr.error("Error deleting record");
                }
            });

        }
    });
}