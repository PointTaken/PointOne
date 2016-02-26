<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BeerAdvocateTest.aspx.cs" Inherits="PT.PointOne.WebAPI.BeerAdvocateTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="Scripts/jquery-2.2.0.min.js"></script>
    <script src="Scripts/jquery.signalR-2.2.0.min.js"></script>
    <script src="/signalr/hubs"></script>
    <title>SignalR-test</title>
    <script>
        var chat = $.connection.chat;
        chat.name = "Test";
        chat.send = function (message) {
            $("#messages").append(message); 
        }

        $("#send").click(function () {
            chat.distribute($("#test").val());
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h3>Messages: </h3>
        <div id="messages"></div>
        <input type="text" id="test" />
        <input type="button" id="send" />
    </div>
    </form>
</body>
</html>
