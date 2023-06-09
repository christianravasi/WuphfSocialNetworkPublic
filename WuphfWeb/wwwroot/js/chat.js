"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

// Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

var username1 = document.getElementById("username1input").value;

// Receive message
connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");

    var today = new Date();
    var time = "";
    if (today.getMinutes() < 10) {
        time = today.getHours() + ":0" + today.getMinutes();
    }
    else {
        time = today.getHours() + ":" + today.getMinutes();
    }
    if (username1 == user) {
        var encodedMsg = `<div class="d-flex flex-row justify-content-end pt-1"><div><div class="small p-2 me-3 mb-1 text-white rounded-3 mess-2" style="display: flex;">
                                                <p class="pe-3" style="margin-bottom: 0px;">` + msg + `</p>
                                                <div class="col"></div>
                                                <div class="small text-white ora">` + time + `</div>
                                            </div></div></div>`;

        var element = document.createElement("div");
        element.innerHTML = encodedMsg;
        document.getElementById("messages").appendChild(element);
    }
    else {
        var encodedMsg = `<div class="d-flex flex-row justify-content-start"><div><div class="small p-2 ms-3 mb-1 rounded-3 mess-1" style="display: flex;">
                                                <p class="pe-3" style="margin-bottom: 0px;">` + msg + `</p>
                                                <div class="col"></div>
                                                <div class="small text-muted ora">` + time + `</div>
                                            </div></div></div>`;

        var element = document.createElement("div");
        element.innerHTML = encodedMsg;
        document.getElementById("messages").appendChild(element);
    }
    document.getElementById("messages").scrollTop = document.getElementById("messages").scrollHeight;
});

// Chat hub connection established
connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

// Send button click event
document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;
    var receiver = document.getElementById("receiverInput").value;
    var idChat = document.getElementById("idChat").value;
    var token = document.getElementById("token").value;

    if (message.trim() !== "") {
        if (receiver != "") {
            connection.invoke("SendMessageToGroup", receiver, message, idChat, token).catch(function (err) {
                return console.error(err.toString());
            });
        }
        else {
            connection.invoke("SendMessage", message).catch(function (err) {
                return console.error(err.toString());
            });
        }
    }

    document.getElementById("messageInput").value = "";
    event.preventDefault();
});

// Message input field key up event
document.getElementById("messageInput").addEventListener("keyup", function (event) {
    if (event.keyCode === 13) {
        document.getElementById("sendButton").click();
    }
    event.preventDefault();
});
