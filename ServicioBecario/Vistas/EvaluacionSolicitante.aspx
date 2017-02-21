<%@ Page Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="EvaluacionSolicitante.aspx.cs" Inherits="ServicioBecario.Vistas.EvaluacionSolicitante" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <!-- JQuery UI-->    
    <link rel="Stylesheet" href="../scripts/jquery/jquery-ui.css" />
    <link href="../scripts/footable.bootstrap.min.css" rel="stylesheet" />
    <script src="../scripts/jquery/jquery-ui.js" ></script>
 
    <!-- Validate CSS -->
    <link rel="stylesheet" type="text/css" href="../scripts/Enginee/css/validationEngine.jquery.css"/>

    <!-- Validation Engine -->    
    <script type="text/javascript" src="../scripts/Enginee/js/jquery.validationEngine-es.js"></script>    
    <script type="text/javascript" src="../scripts/Enginee/js/jquery.validationEngine.js"></script>

        <div class="container">
            <div class="row">
                <div class=" col-md-12 text-center">
                    <h1 class="text-center">Evaluación de Servicio Becario</h1>
                </div>
            </div>
            <div class="jumbotron">
                <div class="form-group table-responsive">
                    <div class="row" >
                        <div class="col-md-12" >
                            <h5 class="text-center"><p><strong>Instrucciones</strong></p></h5>
                        </div>
                        <div class="col-md-12 text-center" >
                            Favor de contestar la siguiente encuesta relacionada con tu Servicio Becario <strong>Tu información es confidencial</strong></h5>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row "><div class="col-md-12">&nbsp;</div></div>
            <div class="row">
                <div class=" col-md-12">
                    <asp:Panel ID="PanelFormulario" runat="server">            </asp:Panel>
                </div>
            </div>
            <div class="row "><div class="col-md-12">&nbsp;</div></div>
            <div class="row">
                <div class=" col-md-12 text-center">
                    <asp:Label ID="LblComentarios" runat="server" Text="Comentarios" Visible="false"></asp:Label>
                </div>
            </div>
            <div class="row "><div class="col-md-12">&nbsp;</div></div>
            <div class="row">
                <div class=" col-md-12 text-center">
                    <asp:TextBox ID="TxtComentarios" runat="server" CssClass=" form-control"  Visible="false"></asp:TextBox>
                </div>
            </div>
            <div class="row "><div class="col-md-12">&nbsp;</div></div>
            <div class="row">
                <div class=" col-md-12 text-center">
                    <asp:Button ID="BtnEnviar" runat="server" Text="Enviar" OnClick="BtnEnviar_Click" CssClass="btn-primary" Visible="false"/>
                </div>
            </div>
            <div class="row "><div class="col-md-12">&nbsp;</div></div>
            <div class="row">
                <div class=" col-md-12 text-center">
                    <asp:Label ID="Msg" runat="server"></asp:Label><br />
                </div>
            </div>
        </div>
    <%--Se tiene que copiar este face para junto un metodo masa para poderse ejecuatar--%>
    <asp:Label ID="Label14" runat="server" Text=""></asp:Label>
    <!-- ModalPopupExtender -->
    <cc1:ModalPopupExtender ID="mp1" runat="server" PopupControlID="Panel1" TargetControlID="Label14"
        CancelControlID="cancel" BackgroundCssClass="modalBackground1">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="Panel1" runat="server"  align="center" style = "display:none">
        <div class="modal-dialog">
            <div class="modal-content" style="background-color:#E6E6E6">
                <div class="modal-header">
                    <%--<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>--%>
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