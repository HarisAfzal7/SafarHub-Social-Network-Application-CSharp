
notificationBoxOpened = false;
function getNotifications() {
    var notificationButton = document.getElementById("notification");
    var userName = notificationButton.value;
    $.ajax({
        url: '/FriendTagList/GetAllMyTags',
        method: 'POST',
        data: {
            myUsername: userName
        },
        success: function (data) {
            console.log('AJAX request successful');
            //console.log(data);
            var notifictaionBoxBody = document.getElementById("notification-box");
            const allNotifications = JSON.parse(data);
            //console.log(allNotifications);
            const friendUserNames = allNotifications.FriendUserName;
            //console.log("FriendUserName:");
            if (notificationBoxOpened) {
                notificationBoxOpened = false;
                notifictaionBoxBody.innerHTML = "";

            } else {
                notificationBoxOpened = true;

                for (let i = 0; i < friendUserNames.length; i++) {
                    //console.log("Notification Box Opened: ", notificationBoxOpened)
                    var addNotification = document.createElement("p");
                    addNotification.innerHTML = friendUserNames[i] + " have taged you in his post";
                    //console.log(allNotifications.FriendUserName[i], addNotification.innerHTML);
                    notifictaionBoxBody.appendChild(addNotification);
                }
            }
            //for (const key in allNotifications) {
            //    if (allNotifications.hasOwnProperty(key)) {
            //        const values = allNotifications[key];
            //        console.log(key + ":");
            //        for (let i = 0; i < values.length; i++) {
            //            console.log(values[i]);
            //        }
            //        console.log();
            //    }
            //}
            //for (var i = 0; i < allNotifications.length;i++) {
            //    var addNotification = document.createElement("p");
            //    addNotification.innerHTML = allNotifications.FriendUserName[i];
            //    console.log(allNotifications.FriendUserName[i], addNotification.innerHTML);
            //    notifictaionBox.appendChild(addNotification);
            //}

        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('AJAX request error');
            console.log(textStatus);
        }
    });
}