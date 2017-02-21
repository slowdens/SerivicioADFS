<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HttpErrorPage.aspx.cs" Inherits="ServicioBecario.HttpErrorPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Generic Error Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>

    <h2>Http Error Page</h2>    
    <asp:Panel ID="InnerErrorPanel" runat="server" Visible="false">
        <asp:Label ID="innerMessage" runat="server" 
            Font-Bold="true" Font-Size="Large" /><br />
        <pre>
<asp:Label ID="innerTrace" runat="server" />
        </pre>
    </asp:Panel>
    Error Message:<br />
    <asp:Label ID="exMessage" runat="server" 
        Font-Bold="true" Font-Size="Large" />
    <pre>
<asp:Label ID="exTrace" runat="server" visible="false" />
    </pre>

    </div>
    </form>
</body>
</html>

