"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/DetailLesson").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

//Tạo ra khung html để đưa lên 
connection.on("ReceiveMessage", function (user, message, avatar) {

    

    var html = '';
    html += '<div class="be-comment"> ';
    html += '<div class="be-img-comment"> ';
    html += '<a href="blog-detail-2.html"> ';
    html += '<img src="'+ avatar +'" alt="" class="be-ava-comment"> ';
    html += '</a> ';
    html += '</div> ';
    html += '<div class="be-comment-content"> ';
    html += '<span class="be-comment-name"> ';
    html += '<h5>' + user+ '</h5> ';
    html += '</span> ';
    html += '<p class="be-comment-text"> ';
    html += ' ' + message;
    html += '  </p> ';
    html += '  </div> ';
    html += '  </div> ';
    var cmt = document.createElement("LI");
    cmt.innerHTML = html
    document.getElementById("CommentPlace").appendChild(cmt);
    document.getElementById("messageInput").value = "";

});

//kết nối
connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

//Xu li click
document.getElementById("sendButton").addEventListener("click", function (event) {
  
    var avatar = document.getElementById("avatarInput").value;
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    var idUser = document.getElementById("idInput").value;
    var idLesson = document.getElementById("idLessonInput").value;

   // Dua du lieu len api de luu vao csdl
    var data = { "Title": "Đây là một comment", "Content": message, "IdLesson": idLesson, "IdUser": idUser };
    var json = JSON.stringify(data);
    $.ajax({
        method: 'POST',
        url: '../../API/CommemtLessonUserAPI/PostCommemtLessonModel',
        contentType: 'application/json',
        accepts: 'application/json',
        data: json,
    })
    connection.invoke("SendMessage", user, message, avatar).catch(function (err) {
        return console.error(err.toString());
    });

    

    event.preventDefault();
});

