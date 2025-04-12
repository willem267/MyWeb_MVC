﻿$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            url: '/admin/user/getall',
            dataSrc: 'data' // Specify the data source property from JSON response
        },
        "columns": [
            { data: 'name', "width": '25%' },
            { data: 'email', "width": '15%' },
            { data: 'phoneNumber', "width": '10%' },
            { data: 'company.name', "width": '10%' },
            { data: 'role', "width": '15%' },
            {
                data: { id:'id', lockoutEnd: "lockoutEnd"},
                render: function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > today) {
                        return`
                        <div class="text-center">
                            <a onclick=LockUnLock('${data.id}') class="btn btn-success text-white" style="cursor:pointer; width:100px;">
                            <i class="bi bi-unlock-fill"></i> Unlock
                            </a>
                             <a href="/admin/user/RoleManagement?userId=${data.id}" class="btn btn-success text-white" style="cursor:pointer; width:150px;">
                            <i class="bi bi-pencil-square"></i> Permission
                            </a>
                        </div>`
                    }
                    else {
                        return`
                        <div class="text-center">
                            <a onclick=LockUnLock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer; width:100px;">
                            <i class="bi bi-unlock-fill"></i> Lock
                            </a>
                             <a href="/admin/user/RoleManagement?userId=${data.id}" class="btn btn-success text-white" style="cursor:pointer; width:150px;">
                            <i class="bi bi-pencil-square"></i> Permission
                            </a>
                        </div>`
                    }
                },
                
                "width": '25%'
            }
        ]
    });
}
function LockUnLock(id) {
    $.ajax({
        type: "POST",
        url: '/Admin/User/LockUnLock',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            }
            else {
                toastr.error(data.message);
            }
        },
         


    })
}