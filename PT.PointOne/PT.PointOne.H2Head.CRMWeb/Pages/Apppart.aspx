<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Apppart.aspx.cs" Inherits="PT.PointOne.H2Head.CRMWeb.Pages.Apppart" %>

<!DOCTYPE html>

<html>
<head>
    <title></title>
    <script type="text/javascript">
        // Set the style of the client web part page to be consistent with the host web.
        (function () {
            'use strict';

            var hostUrl = '';
            if (document.URL.indexOf('?') != -1) {
                var params = document.URL.split('?')[1].split('&');
                for (var i = 0; i < params.length; i++) {
                    var p = decodeURIComponent(params[i]);
                    if (/^SPHostUrl=/i.test(p)) {
                        hostUrl = p.split('=')[1];
                        document.write('<link rel="stylesheet" href="' + hostUrl + '/_layouts/15/defaultcss.ashx" />');
                        break;
                    }
                }
            }
            if (hostUrl == '') {
                document.write('<link rel="stylesheet" href="/_layouts/15/1033/styles/themable/corev15.css" />');
            }
        })();
    </script>
</head>
<body>
    <h1>CRM Stuff</h1>
   Task 1:
Establish connection to CRM: https://aspc2016.crm4.dynamics.com/
    "
Task 2:
Programmatically add all your team members related to your account as contacts
        <asp:Button ID="AddTeamBtn" runat="server"  Text="Add team" OnClick="AddTeamBtn_Click"/>
Task 3:

Write events from your app as cases related to your account

Present your created events in your app/dashboard
    <asp:Button runat="server" ID="WriteEvent" Text="Write event" OnClick="WriteEvent_Click"/>
Task 4:

In case of eval attach (changing of your account name by another team) an alert has to be created and displayed in your app
    <div id="isnamedifferent">

    </div>

</body>
</html>
