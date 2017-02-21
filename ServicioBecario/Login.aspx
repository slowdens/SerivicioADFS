<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ServicioBecario.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <script src="scripts/boostrapt/js/jquery-1.11.1.min.js"></script>
    <script src="scripts/boostrapt/js/jquery-1.11.1.js"></script>
    <script src="scripts/PlantillaZul/Script/jquery.browser.min.js"></script>
    <link href="scripts/boostrapt/css/Propias.css" rel="stylesheet" />
    <link href="scripts/boostrapt/css/bootstrap.min.css" rel="stylesheet" />
    <link href="scripts/boostrapt/css/bootstrap-theme.min.css" rel="stylesheet" />
    <link href="scripts/boostrapt/css/estilos.css" rel="stylesheet" />
    <link href="scripts/login.css" rel="stylesheet" />

    <link href="http://fonts.googleapis.com/css?family=Open+Sans:400,300,600,700" rel="stylesheet" type="text/css" />
    <link href="scripts/boostrapt/css/simple-sidebar.css" rel="stylesheet" />
    <script src="scripts/boostrapt/js/bootstrap.min.js"></script>
    <link href="scripts/PlantillaZul/CSS/MiPlantilla.css" rel="stylesheet" />
      <style type="text/css">
          div.caja {
              height: 700px;
              width: 100%;
              padding: 300px;
              margin:1px 1px 1px 1px;
              /*background:Red;*/
         }
          div.cajita{
              height: 200px;
              width: 100%;
              margin:20px;
              /*padding: 10px;*/
              margin:1px 1px 1px 1px;
              /*background:Blue;*/
          }
  </style>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    <div>
        <asp:HiddenField ID="Hdfusuario" runat="server" />
        <nav class="Navegacion">
            <br />
            <br />
           
            <img id="LogoT" src="scripts/PlantillaZul/Imagenes/Logotipo.png" />
            <br />
            <br />
            <br />
            <div id="dos">
            </div>

        </nav>
        <div id="RetContenido">
            <section class="ContenidoArticle" style="opacity:1">
                <article>
                        
                    <div class="caja">
                        <div class="cajita">
                              <div class="login">
                           <div class="row">
                               <div class="col-md-2">
                                  
                               </div>
                               <div class="col-md-10">
                                   <asp:TextBox runat="server" placeholder="Nómina"  ID="txtNomina"></asp:TextBox>
                               </div>
                           </div>
                           <div class="row" >
                               <div class="col-md-2">
                                  
                               </div>
                               <div class="col-md-10">
                                   <asp:TextBox runat="server" TextMode="Password" placeholder="Contraseña"  ID="txtPassword"></asp:TextBox>
                               </div>
                           </div>
                            <div class="row">
                                <div class="col-md-offset-2 col-md-10" >
                                    <asp:Button CssClass="btn btn-primary btn-lg" runat="server" Text="Entrar" ID="btnAgregar" OnClick="btnAgregar_Click" />
                                </div>
                            </div>

                                  </div>
                        </div>
                    </div>
                </article>
            </section>
            <aside>
            </aside>
        </div>  
         <!--Fin uno-->
        
    </div>
    </form>

    <script src="../scripts/PlantillaZul/Script/PlantillaMenu.js"></script> 
    <script type="text/javascript">-
        $(window).resize(function () {
            
        });

        $(document).ready(function () {
     
            $("#RetContenido").fadeIn(1000);
            console.log("Hola");
            var al = $(document).height();
            var anc = $(document).width();
            $(".Navegacion").css({ "height": al });
            $(".ContenidoFooter").css({ "top": al, "width": anc });




          
        });
    </script>

</body>
</html>
