<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="School.Login" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <meta charset="utf-8">
    <title>Thakur Vidya Mandir  High School</title>
    <meta name="description" content="mobile first, app, web app, responsive, admin dashboard, flat, flat ui">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
    <link rel="stylesheet" href="/src/css/font.css">
    <link href="/src/css/app.v2.css" rel="stylesheet" />
    <!-- This is what you need -->
    <script src="/src/js/sweet-alert.js"></script>
    <link rel="stylesheet" href="/src/css/sweet-alert.css">
    <!--.......................-->
    <!--[if lt IE 9]> <script src="js/ie/respond.min.js"></script> <script src="js/ie/html5.js"></script> <![endif]-->
    <style>
        @media (min-width: 768px) {
            .login {
                margin-left: 20%;
            }

            .navbar-brand {
                float: none;
                display: block;
                margin: 0;
                text-align: center;
            }
        }

        .login {
            margin-top: 40px;
        }

        .margin-top {
            margin-top: 43px;
        }
    </style>
</head>
<body>
    <!-- header -->
    <header id="header" class="navbar">
        <a class="navbar-brand" href="#">Thakur Vidya Mandir  High School</a>
    </header>
    <!-- / header -->

    <section id="content">
        <section class="main padder">
            <div class="clearfix">
            </div>
            <div class="row">
                <div class="col-sm-6 login ">
                    <section class="panel">
                        <div class="panel-body margin-top form-horizontal">
                            <form runat="server">
                                <div class="form-group">
                                    <label class="col-lg-3 control-label">UserName</label>
                                    <div class="col-lg-8" id="">
                                        <input type="text" id="txtUserName" required runat="server" class="bg-focus form-control parsley-validated" data-required="true" name="txtUserName" placeholder=" User Name..." />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-lg-3 control-label">Password</label>
                                    <div class="col-lg-8">
                                        <input type="password" id="txtPassword" required runat="server" class="bg-focus form-control parsley-validated" data-required="true" name="txtPassword" placeholder=" Password ..." />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-lg-9 col-lg-offset-3">
                                        <asp:Button Text="Login" runat="server" ID="btnLogin" class="btn btn-lg btn-primary " OnClick="btnLogin_Click" OnClientClick="return applyFilter()" />
                                    </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-lg-9 col-lg-offset-3">
                                            <div id="lblMesssage" class=" alert-warning" style="font-size: 16px;" runat="server" ></div>
                                        </div>
                                    </div>
                            </form>
                        </div>
                    </section>
                </div>
            </div>
        </section>
    </section>
    <!-- footer -->
    <footer id="footer">
        <div class="text-center padder clearfix">
            <p>
                <small>© BitBlue 2014 </small>
                <br>
            </p>
        </div>
    </footer>
    <!-- / footer -->

</body>
</html>
