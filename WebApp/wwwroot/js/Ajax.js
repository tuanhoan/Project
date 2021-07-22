// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    $("#loaderbody").addClass('hide');

    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('hide');
    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('hide');
    });
});


showInPopup = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form-modal .modal-body').html(res);
            $('#form-modal .modal-title').html(title);
            $('#form-modal').modal('show');
        }
    })
}

function callAPIUser(url,urledit,urldetail,urldelete) {
    $.ajax({
        type: 'GET',
        url: url,
        dataType: 'json',
        success: function (res) {
            console.log(res);
            $.each(res, function (index, item) {
                if ($('#userTable tbody').length == 0) {
                    $('#userTable').append("<tbody ></tbody >");
                }
                $('#userTable tbody').append(
                    "<tr>" +
                    "<td>" + item.accountName + "</td>" +
                    '<td><img src="../Img/User/' + item.img +'" alt="Alternate Text" </td>' +
                    "<td>" + item.roles.name + "</td>" +
                    '<td> <a href ="' + urledit + '/' + item.id + '" class="btn btn-warning text-white" >Edit</a>|' +
                    '<a href ="' + urldetail + '/' + item.id + '" class="btn btn-info" >Detail</a>|' +
                    '<form action="' + urldelete + '/' + item.id + '" class="d-inline"> ' +
                    '<input type = "hidden" value="' + item.id + '" />' +
                        ' <input type="submit" value="Xoá" class="btn btn-danger " />' + 
                       '</form ></td>' +
                    "</tr>"
                );
            });
        }
    })
}



jQueryAjaxPost = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#view-all').html(res.html)
                    $('#form-modal .modal-body').html('');
                    $('#form-modal .modal-title').html('');
                    $('#form-modal').modal('hide');
                    $('#AddOrEdit-modal').modal('show');
                }
                else
                    $('#form-modal .modal-body').html(res.html);
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

jQueryAjaxDelete = form => {
    if (confirm('Bạn có thật sự muốn xoá ?')) {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    $('#view-all').html(res.html);
                    $('#AddOrEdit-modal').modal('show');
                },
                error: function (err) {
                    console.log(err)
                }
            })
        } catch (ex) {
            console.log(ex)
        }
    }

    //prevent default form submit event
    return false;
}

function readURL(input, idImg) {
		if (input.files && input.files[0]) {
			var reader = new FileReader();
			reader.onload = function (e) {
    $(idImg).attr("src", e.target.result);
			}
			reader.readAsDataURL(input.files[0]);
        }
}
