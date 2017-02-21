<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ServicioBecario.Vistas.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="row">
        <div class="col-md-12" >
            <img src="../images/BecaBecario2.png" id="imgDefault" />
            <asp:Label runat="server" ID="lbletiqueta"></asp:Label>
        </div>

    </div> 

      <asp:Label runat="server" ID="Label1"></asp:Label>
    <script type="text/javascript">
        $(document).ready(function () {
            Ajuste();
        });
        $(window).resize(function () {
            Ajuste();
        });
        function Ajuste()
        {
            var wi = $(window).width();
            if (wi < 500)
            {
                var ancho = wi - 90;
                $("#imgDefault").attr("src", "../images/BecaBecario2Mini.png");
                $("#imgDefault").css({"width": ancho+"px","height":(ancho*0.72)+"px"});
            } else {
                var ancho = wi - 290;
                $("#imgDefault").attr("src", "../images/BecaBecario2.png");
                $("#imgDefault").css({ "width": ancho + "px", "height": (ancho * 0.7) + "px" });
            }
            
        }
    </script>
</asp:Content>
