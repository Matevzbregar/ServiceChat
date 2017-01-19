<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Chat.aspx.cs" Inherits="serviceChatt.Chat" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width:582px; margin: 100px auto; height: 293px; border:thin solid gray; ">
        <table style="width:450px; margin: 10px auto">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Prijavljeni ste kot "></asp:Label>
                    <asp:Label ID="CurrentUser" runat="server" Text=" ..."></asp:Label>
                </td>
                <td>
                    <asp:Button ID="Logout" runat="server" Text="Odjava" Width="150px" OnClick="Logout_Click" />
                </td>
            </tr>
            <tr>
                <td rowspan="2">
                    <!-- ReadOnly = true onemogoča uporabnikom spreminjanje poslanih sporočil in prijavljenih uporabnikov -->
                    <!-- TextMode = multiline omogoči večvrstični vnos v textbox -->
                    <asp:TextBox ID="Messages" runat="server" Height="200px" Width="400px" ReadOnly="True" VerticalContentAlignment="Top" TextMode="MultiLine"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="Users" runat="server" Height="175px" Width="144px" ReadOnly="True" VerticalContentAlignment="Top" TextMode="MultiLine" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="Refresh" runat="server" Text="Osveži" Width="150px" OnClick="Refresh_Click" />
                </td>
            </tr>
            <tr>
                <td class="auto-style1">
                    <asp:TextBox ID="Message" runat="server" Width="400px"></asp:TextBox>
                </td>
                <td class="auto-style1">
                    <asp:Button ID="Send" runat="server" Text="Pošlji" Width="150px" OnClick="Send_Click" />
                </td>
            </tr>
        </table>
        </div>
        </form>
</body>
</html>
