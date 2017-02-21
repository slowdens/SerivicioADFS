<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="Revaluacion.aspx.cs" Inherits="ServicioBecario.Vistas.Revaluacion" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <link rel="stylesheet" href="../scripts/footable.bootstrap.min.css" type="text/css" /> 
    <link rel="stylesheet"  href="../scripts/Enginee/css/validationEngine.jquery.css" type="text/css"/> 
    <link rel="stylesheet" href="../scripts/Estiloss.css" type="text/css" />   
    <%--<script type="text/javascript" src="../scripts/Enginee/js/jquery-1.8.2.min.js"  charset="utf-8"></script>--%>
    <script type="text/javascript" src="../scripts/Enginee/js/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../scripts/Enginee/js/jquery.validationEngine.js" charset="utf-8"></script>
    <script>
        function validar() {
            //Esto es para validar campos
            jQuery("form").validationEngine();
        }
        function progreso(x) {
            //var id = { id: x };
            $.ajax({
                success: function () {
                    document.getElementById("bar1").style.width = x + "%";
                    $("#lblDatoProgreso").text("");
                    $("#lblDatoProgreso").text("Valor: " + x + "%");
                }
            });

        }
    </script>
     <div class="row">
           <div class="col-md-12">
                <h1 class="text-center">Servicio Becario</h1>
           </div>
       </div>

    <div class="row">
        <div class="col-md-12 text-center">
            <h4>Envio de correo de recordar evaluación </h4>
            
        </div>
    </div>
    <div class="jumbotron">
        <div class="row">
            <div class="col-md-1">
                 <label>Periodo</label>               
            </div>
            <div class="col-md-3">
                <asp:DropDownList runat="server" ID="ddlperiodo" CssClass="form-control validate[required]" OnDataBound="ddlperiodo_DataBound"></asp:DropDownList>
            </div>
            <div class="col-md-1">
                <label>Campus</label>
            </div>
            <div class="col-md-3">
                <asp:DropDownList ID="ddlCampus" runat="server" CssClass="form-control validate[required]" OnDataBound="ddlCampus_DataBound"></asp:DropDownList>
                <asp:Label runat="server" ID="lblCampus" Visible="false"></asp:Label>
                <asp:HiddenField id="hdfActivarRol"  runat="server" />
                <asp:HiddenField id="hdfidCampus" runat="server" />
            </div>
            <div class="col-md-1">
                <asp:Button runat="server" ID="btnMandar" CssClass="btn btn-primary" OnClientClick="validar();" Text="Enviar" OnClick="btnMandar_Click" />
           <br /><br /><br /><br /><br /><br /> </div>
        </div>     
        
    </div>
    <div class="row">
        <div class=" col-sm-offset-2 col-xs-offset-2 col-lg-offset-2 col-md-offset-2 col-md-5 ">
            <asp:Panel ID="pnlprogres" runat="server" Visible="true">                
                <div class="progress progress-striped">
                    <div class="progress-bar progress-bar-success" role="progressbar"
                        aria-valuenow="60" aria-valuemin="0" aria-valuemax="100"
                        id="bar1" >
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
    <div class="row">
        <div class=" col-sm-offset-2 col-xs-offset-2 col-lg-offset-2 col-md-offset-2 col-md-2">
            <label id="lblDatoProgreso"  ></label>  
           <asp:Label ID="lblMensaje" runat="server"></asp:Label>  
        </div>
    </div>

    
    <%--Se tiene que copear este face para junto un metodo masa para poderse ejecuatar--%>
    <asp:Label ID="Label14" runat="server" Text=""></asp:Label>
        <!-- ModalPopupExtender -->
        <cc1:ModalPopupExtender ID="mp1" runat="server" PopupControlID="Panel1" TargetControlID="Label14"
            CancelControlID="cancel" BackgroundCssClass="modalBackground1">
        </cc1:ModalPopupExtender>
        <asp:Panel ID="Panel1" runat="server"  align="center" style = "display:none">
             <div class="modal-dialog">
                                <div class="modal-content" style="background-color:#E6E6E6">
                                  <div class="modal-header">
                                    <!--<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>-->
                                    <h4 class="modal-title">
                                        <asp:Label ID="lblCabeza" Font-Names="open sans" Font-Size="13px"  runat="server" Text="Este es el titulo"></asp:Label></h4>
                                  </div>
                                  <div class="modal-body">
                                        <div class="col-md-1">
                                            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Alertaz.png" />
                                        </div>
                                        <div class="col-md-11">
                                            <asp:Label ID="lblcuerpo" runat="server" Font-Names="open sans" Font-Size="13px" Text=""></asp:Label>&hellip;
                                        </div>                                  
                                  </div>
                                  <div class="modal-footer">
                                    <button type="button" id="cancel" class="btn" style="background-color:#1D4860;color:#F6F6F6" data-dismiss="modal">Cerrar</button>        
                                  </div>
                                </div><!-- /.modal-content -->
                              </div><!-- /.modal-dialog -->
            
        </asp:Panel>
 <%--   Fin de la face del modal--%>


</asp:Content>
