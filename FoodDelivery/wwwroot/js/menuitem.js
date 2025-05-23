﻿var dataTable;

$(document).ready(function () {
    loadList();
});

function loadList() {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/api/menuitem",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { data: "name", width: "40%" },
            { data: "price", render: $.fn.dataTable.render.number(',', '.', 2, "$"), width: "15%" },
            { data: "categoryName", width: "15%" },
            {
                data: "foodTypeNames", width: "20%",
                render: function (data, type, row) {
                    return Array.isArray(data) ? data.join(", ") : "";
                }
            },
            {
                data: "id", width: "30%",
                "render": function (data) {
                    return `<div class="text-center">
                            <a href="/Admin/MenuItems/Upsert?id=${data}"
                            class ="btn btn-success text-white style="cursor:pointer; width=100px;"> <i class="far fa-edit"></i>Edit</a>
                           
                            <a onClick=Delete('/api/menuitem/'+${data})
                            class ="btn btn-danger text-white style="cursor:pointer; width=100px;"> <i class="far fa-trash-alt"></i>Delete</a>
                            <br>
                             <a href="/Admin/MenuItems/ManageFoodTypes?id=${data}" 
                             class="btn btn-warning style="cursor:pointer;"><i class="far fa-hamburger"></i>Manage Food Types</a>
                    </div>`;
                }
            }
        ],
        "language": {
            "emptyTable": "no data found."
        },
        "width": "100%"
    });
};

function Delete(url) {
    swal({
        title: "Are you sure you want to delete?",
        text: "You will not be able to restore this data!",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}