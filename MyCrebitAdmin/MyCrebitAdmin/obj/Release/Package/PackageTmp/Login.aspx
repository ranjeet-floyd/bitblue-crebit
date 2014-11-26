<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CrebitAdminPanelNew.Login" Trace="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <meta name="description" content=""/>
    <meta name="author" content=""/>
    <link rel="icon" href="../../favicon.ico"/>

    <title>Login</title>

    <!-- Bootstrap core CSS -->
    <link href="bootstrap.min.css" rel="stylesheet"/>
    <link href="signin.css" rel="stylesheet" type="text/css" />
    <!-- Custom styles for this template -->
    <script src="Scripts/cookies.js" type="text/javascript" ></script>
    
</head>
<body>
      
     <form class="form-signin"  runat="server">
     <div class="form-control">
      <h2 class="form-signin-heading">Please sign in </h2>

        <asp:TextBox ID="UserId" runat="server"  class="form-control" placeholder="UserId" ></asp:TextBox>
       
        <asp:TextBox ID="Password" runat="server" class="form-control" placeholder="Password"  ></asp:TextBox>
       
       <asp:Label class="checkbox" runat="server" id="Label2"><%--<asp:CheckBox ID="chkRememberMe" runat="server" />Remember me--%> </asp:Label>
       <asp:Button ID="Button1" runat="server" onclick="Button1_Click" class="btn btn-lg btn-primary btn-block"  Text="Sign in" />
       <asp:Label ID="Label3" runat="server" class="checkbox" ></asp:Label>
             
     

    </div> <!-- /container -->
     </form>
    
    </body>
</html>
