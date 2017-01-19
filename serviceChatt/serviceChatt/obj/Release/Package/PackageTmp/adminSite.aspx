<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="adminSite.aspx.cs" Inherits="serviceChatt.adminSite" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            height: 234px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width:800px; margin: 100px auto; height: 425px; ">
        <table style="width:800px; margin: 10px auto; text-align: center;" border="1">
            <tr>
                <td colspan="4" style="height: 100px">
                    <asp:Label ID="Label1" runat="server" Text="Administratorske strani" Font-Bold="True" Font-Size="X-Large"></asp:Label>
                </td>
                
            </tr>
            <tr>
                <td style="padding-top:7px; padding-bottom:7px;">
                    <asp:Label ID="Label2" runat="server" Text="Izbriši uporabnika" Font-Bold="True" Font-Size="Large"></asp:Label>
                </td>
                <td style="padding-top:7px; padding-bottom:7px;">

                    <asp:Label ID="Label3" runat="server" Text="Upravljaj admin pravice" Font-Bold="True" Font-Size="Large"></asp:Label>
                </td>
                <td style="padding-top:7px; padding-bottom:7px;">
                    <asp:Label ID="Label5" runat="server" Text="Prijavljeni uporabniki" Font-Bold="True" Font-Size="Large"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label8" runat="server" Text="Registrirani uporabniki" Font-Bold="True" Font-Size="Large"></asp:Label>
                
                </td>
            </tr>
            <tr>
                <td style="vertical-align:top; padding-top:7px" class="auto-style1">
                    
                    <asp:Label ID="Label4" runat="server" Text="Uporabniško ime: "></asp:Label>
                    
                    <br />
                    
                    <asp:TextBox ID="imeIzbris" runat="server" style="margin-top:7px; margin-bottom:7px"></asp:TextBox>
                    <br />
                    

                    <asp:Button ID="izbrisi" runat="server" Text="Izbriši" OnClick="izbrisi_Click" />
                    <br />
                    <br />
                    <asp:Label ID="uporabnik" runat="server" Font-Size="Small" ForeColor="#009933" Visible="False" Text="Uporabnik je izbrisan!"></asp:Label>
                </td>
                <td style="vertical-align:top; padding-top:7px" class="auto-style1">
                    <asp:Label ID="Label6" runat="server" Text="Uporabniško ime: "></asp:Label>
                    
                    <br />
                    
                    <asp:TextBox ID="imeUpravljaj" runat="server" style="margin-top:7px; margin-bottom:7px"></asp:TextBox>
                    <br />
                    

                    <asp:Button ID="dodaj" runat="server" Text="Dodaj" width="70px" style="margin-right:2px;" OnClick="dodaj_Click"/>
                    <asp:Button ID="odstrani" runat="server" Text="Odstrani" width="70px" style="margin-left:2px" OnClick="odstrani_Click"/>
                    <br />
                    <br />
                    <asp:Label ID="pravice" runat="server" Font-Size="Small" ForeColor="#009933" Visible="False" Text="Pravice"></asp:Label>
                </td>
                <td class="auto-style1">
                    <asp:TextBox ID="Users" runat="server" Height="175px" Width="144px" ReadOnly="True" VerticalContentAlignment="Top" TextMode="MultiLine" style="margin-top:7px" ></asp:TextBox>
                    <br />
                    <asp:Button ID="Refresh" runat="server" Text="Osveži" Width="150px" style="margin-top:7px; margin-right:4px; margin-bottom:7px" OnClick="Refresh_Click" />
                </td>
                <td>
                    <asp:TextBox ID="registeredUsers" runat="server" Height="210px" Width="144px" ReadOnly="True" VerticalContentAlignment="Top" TextMode="MultiLine" style="margin-top:7px; padding: 7px 0px 0px 7px" ></asp:TextBox>
                </td>
            </tr>
        </table>
        <hr />
        <table style="width:800px; margin: 10px auto; text-align: center;">
            <tr>
                <td colspan="2" style="text-align:left; padding-left:7px">
                    <asp:Label ID="Label7" runat="server" Text="Prijavljeni ste kot "></asp:Label>
                    <asp:Label ID="CurrentUser" runat="server" Text=" ..."></asp:Label>
                </td>
                <td style="width:203px">
                    <asp:Button ID="Logout" runat="server" Text="Odjava" Width="150px" style="margin-right:3px" OnClick="Logout_Click"/>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
