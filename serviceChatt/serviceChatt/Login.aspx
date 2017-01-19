<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="serviceChatt.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width:700px; margin: 50px auto; font-family:Arial">
        <table style="width:700px; margin: 10px auto; text-align:center;">
            <tr>
                <td colspan="4" style="height: 100px">
                    <asp:Label ID="Label1" runat="server" Text="ServiceChat" Font-Bold="True" Font-Size="X-Large"></asp:Label>
                </td>
                
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="Label2" runat="server" Text="Registracija" Font-Bold="True" Font-Size="Large"></asp:Label>
                    <br />
                    <asp:Label ID="Label4" runat="server" Font-Size="Smaller" Text="Nov uporabnik" ForeColor="#666666"></asp:Label>
                </td>
                <td style="width:350px" colspan="2">
                    <asp:Label ID="Label3" runat="server" Text="Prijava" Font-Bold="True" Font-Size="Large"></asp:Label>
                    <br />
                    <asp:Label ID="Label5" runat="server" Font-Size="Smaller" Text="Obstoječ uporabnik" ForeColor="#666666"></asp:Label>
                </td>
            </tr>
            <tr style="text-align:right;">
                <td class="auto-style2">
                        <br />
                    <asp:Label ID="Label6" runat="server" Text="Ime:"></asp:Label>
                        <br /><br />
                    <asp:Label ID="Label7" runat="server" Text="Uporabniško ime:"></asp:Label>
                        <br /><br />
                    <asp:Label ID="Label8" runat="server" Text="Geslo:"></asp:Label>
                        <br /><br />
                    <asp:Label ID="Label9" runat="server" Text="Geslo:"></asp:Label>
                        <br />
                        <br />
                        <br />
                </td>
                <td class="auto-style2">
                        <br />
                    <asp:TextBox ID="ime" runat="server" Width="150px"></asp:TextBox>
                        <br /><br />
                    <asp:TextBox ID="uporabniskoI" runat="server" Width="150px"></asp:TextBox>
                        <br /><br />
                    <asp:TextBox ID="geslo1" runat="server" Width="150px" TextMode="Password"></asp:TextBox>
                        <br /><br />
                    <asp:TextBox ID="geslo2" runat="server" Width="150px" TextMode="Password"></asp:TextBox>
                                               
                        <br />
                        <br />
                        <asp:Label ID="napacnoG" runat="server" Font-Size="X-Small" ForeColor="Red" Text="Vnesli ste napačne podatke!" Visible="False"></asp:Label>
                                               
                        <br />
                </td>
                <td class="auto-style2" style="vertical-align:top">
                        <br />
                    <asp:Label ID="Label10" runat="server" Text="Uporabniško ime:"></asp:Label>
                        <br /><br />
                    <asp:Label ID="Label11" runat="server" Text="Geslo:"></asp:Label>
                        <br />
                    
                </td>
                <td class="auto-style2" style="vertical-align:top">
                        <br />
                    <asp:TextBox ID="Username" runat="server" Width="150px"></asp:TextBox>
                        <br /><br />
                    <asp:TextBox ID="Password" runat="server" Width="150px" TextMode="Password"></asp:TextBox>
                        <br />
                                            
                        <asp:Label ID="napacno" runat="server" Font-Size="X-Small" ForeColor="Red" Text="Vnesli ste napačne prijavne podatke" Visible="False"></asp:Label>
                                            
                        <br />
                    <asp:Button ID="loginBtn" runat="server" Text="Prijava" Width="154px" style="margin-top:5px" OnClick="loginBtn_Click" />
                                            
                        <br />
                    <asp:Button ID="loginAdmin" runat="server" Text="Prijava kot admin" Width="154px" style="margin-right:4px; margin-top:14px" OnClick="loginAdmin_Click" />
                                            
                </td>
                               
            </tr>
            <tr>
                <td colspan="2" style="text-align:right">
                        <asp:Label ID="registracija" runat="server" Font-Size="X-Small" ForeColor="#009933" Visible="False" Text="Vaš uporabniški račun je bil uspešno ustvarjen!"></asp:Label>
                        <br />
                    <asp:Button ID="regBtn" runat="server" Text="Registracija" Width="154px" style="margin-right:4px;" OnClick="regBtn_Click"/>
                </td>
                <td colspan="2" style="text-align:right">
                        <br />
                </td>
            </tr>
        </table>



    </div>
    </form>
</body>
</html>
