var dtbl;
$(document).ready(function () {

    loaddata();

}
);


function loaddata() {
    dtbl = $("#mytable").DataTable(
        {
            "ajax": {
                "url": "/Admin/Order/GetData",
                "type": "GET", // Specify GET request
                "datatype": "json" // Specify data format
                
            },
            "columns": [
                { "data": "id" },
                { "data": "name" },
                { "data": "phoneNumber" },
                { "data": "applicationUser.email" },
                { "data": "orderStatus" },
                { "data": "totalPrice" },
                {
                    "data": "id",
                    "render": function (data) {
                        return `
                        
                             <a href="/Admin/Order/Details?orderid=${data}" class="btn btn-warning">Details</a>

                              `
                    }
                }



            ]
        }
    );
}


function DeleteItem(url) {
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
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dtbl.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}

