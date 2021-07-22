function DeleteStudentApi(id) {
    if (confirm('Bạn có muốn xóa học sinh này ?')) {
        $.ajax({
            type: 'DELETE',
            url: '../API/StudentAPI/DeleteStudentModel/' + id,
        })
            .done(function (res) {
                var html = '<tbody id="AllStudent">';
                data = res;
                for (i = 0; i < data.length; i++) {
                    html += ' <tr>';
                    html += ' <td>';
                    html += ' ' + data[i].fullName;
                    html += ' </td>';
                    html += ' <td>';
                    html += ' ' + data[i].email;
                    html += ' </td>';
                    html += ' <td>';
                    html += ' ' + data[i].address;
                    html += ' </td>';
                    html += ' <td>';
                    html += ' ' + data[i].phone;
                    html += ' </td>';
                    html += ' <td>';
                    html += '<a class=\"btn btn-warning\" onclick=\"showInPopup(\'https://localhost:44379/Admin/Student/AddOrEdit/' + data[i].id + '\',\'Chỉnh sửa\')\" > Chỉnh Sửa</a >| ';
                    html += '<button class=\"btn btn-warning\" onclick=\"DeleteStudentApi(' + data[i].id + ')\"> Xóa </button>'
                    html += ' </td>';
                    html += ' </tr>';
                }
                html += ' </tbody>';
                document.getElementById('AllStudent').innerHTML = html;

                
            })
    }
}

//layout
    //var html = '';
    //html += '<div class="be-comment"> ';
    //html += '<div class="be-img-comment"> ';
    //html += '<a href="blog-detail-2.html"> ';
    //html += '<img src="" alt="" class="be-ava-comment"> ';
    //html += '</a> ';
    //html += '</div> ';
    //html += '<div class="be-comment-content"> ';
    //html += '<span class="be-comment-name"> ';
    //html += '<h5>' + user + '</h5> ';
    //html += '</span> ';
    //html += '<p class="be-comment-text"> ';
    //html += ' ' + message;
    //html += '  </p> ';
    //html += '  </div> ';
    //html += '  </div> ';
    //document.getElementById("CommentPlace").appendChild(html);
