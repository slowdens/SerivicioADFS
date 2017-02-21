<%@ Page Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="ResultadoEvaluacionBecario.aspx.cs" Inherits="SolicitudBecario.ResultadoEvaluacionBecario" %>
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
    <div class="row">
        <div class="col-md-12">
            <h1 class="text-center">Resultado de mi evaluación</h1>
        </div>
    </div>
    <div>
        <asp:Label ID="Msg" runat="server" Text=""></asp:Label>
    </div>
    <fieldset>
        <div class="row bg-info">
            <div class="col-md-12">
                <h4 class="text-center">¿Quién me evaluó?</h4>
            </div>
        </div>
        <div class="row bg-success">
            <div class="col-md-2  col-md-offset-4">
                <label class="control-label"><em>Solicitante</em></label>
            </div>
            <div class="col-md-3">
                <asp:Label ID="LblSolicitante" runat="server" Text="N/D"></asp:Label>
            </div>
        </div>
        <div class="row bg-success">
            <div class="col-md-2  col-md-offset-4">
                <label class="control-label"><em>Nómina</em></label>
            </div>
            <div class="col-md-3">
                <asp:Label ID="LblNomina" runat="server" Text="N/D"></asp:Label>
            </div>
        </div>
        <div class="row bg-success">
            <div class="col-md-2  col-md-offset-4">
                <label class="control-label"><em>Puesto</em></label>
            </div>
            <div class="col-md-3">
                <asp:Label ID="LblPuesto" runat="server" Text="N/D"></asp:Label>
            </div>
        </div>
        <div class="row bg-success">
            <div class="col-md-2  col-md-offset-4">
                <label class="control-label"><em>Departamento:</em></label>
            </div>
            <div class="col-md-3">
                <asp:Label ID="LblDepartamento" runat="server" Text="N/D"></asp:Label>
            </div>
        </div>
        <div class="row bg-success">
            <div class="col-md-2  col-md-offset-4">
                <label class="control-label"><em>Ubicación Física</em></label>
            </div>
            <div class="col-md-3">
                <asp:Label ID="LblUbicacionFisica" runat="server" Text="N/D"></asp:Label>
            </div>
        </div>
        <div class="row bg-success">
            <div class="col-md-2  col-md-offset-4">
                <label class="control-label"><em>Período</em></label>
            </div>
            <div class="col-md-3">
                <asp:Label ID="LblPeriodo" runat="server" Text="N/D"></asp:Label>
            </div>
        </div>
    </fieldset>
    <div class="row "><div class="col-md-12">&nbsp;</div></div>
        <fieldset>
            <div class="row bg-info">
                <div class="col-md-12">
                    <h4 class="text-center">Datos del Becario</h4>
                </div>
            </div>
            <div class="row bg-success">
                <div class="col-md-2  col-md-offset-4">
                    <label class="control-label"><em>Solicitud</em></label>
                </div>
                <div class="col-md-3">
                    <asp:Label ID="LblSolicitud" runat="server" Text="N/D"></asp:Label>
                </div>
            </div>
            <div class="row bg-success">
                <div class="col-md-2  col-md-offset-4">
                    <label class="control-label"><em>Matricula</em></label>
                </div>
                <div class="col-md-3">
                    <asp:Label ID="LblMatricula" runat="server" Text="N/D"></asp:Label>
                </div>
                <div class="col-md-1">
                    <asp:Label ID="LblIdAlumno" runat="server" Text="" Visible="false"></asp:Label>
                </div>
            </div>
            <div class="row bg-success">
                <div class="col-md-2  col-md-offset-4">
                    <label class="control-label"><em>Nombre</em></label>
                </div>
                <div class="col-md-3">
                    <asp:Label ID="LblNombreBecario" runat="server" Text="N/D"></asp:Label>
                </div>
            </div>
            <div class="row bg-success">
                <div class="col-md-2  col-md-offset-4">
                    <label class="control-label"><em>Calificación:</em></label>
                </div>
                <div class="col-md-3">
                    <asp:Label ID="LblCalificacion" runat="server" Text="N/D"></asp:Label>
                </div>
            </div>
        </fieldset>
        <div class="row "><div class="col-md-12">&nbsp;</div></div>
        <div class="row "><div class="col-md-12">&nbsp;</div></div>
        <div class="row text-center">
            <asp:Button ID="BtnCerrar" runat="server" Text="Cerrar" OnClick="BtnCerrar_Click" CssClass="btn-primary" Width="100px" Visible ="false"/>
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
