<%@ Page Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="MiAsignacion.aspx.cs" Inherits="ServicioBecario.Vistas.MiAsignacion" %>
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
    <table id ="Asignacion" runat="server" class="table-responsive table" visible="true">
        <tr>
            <td>
        <div>
            <div class="row "><div class="col-md-12">&nbsp;</div></div>
            <div class="row "><div class="col-md-12">&nbsp;</div></div>
            <div class="row">
                <div class="col-md-12">
                    <h1 class="text-center text-capitalize">Mi Asignación</h1>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12 text-center">
                    <asp:Label ID="Msg" runat="server"></asp:Label>
                </div>
            </div>
            <div class="row "><div class="col-md-12">&nbsp;</div></div>
            <div class="row">
                <div class="col-md-12">
                    <h4 class="text-center bg-info">Datos del becario</h4>
                </div>
            </div>
            <fieldset>
                <div class="row bg-success">
                    <div class="col-md-1  col-md-offset-5">
                        <label class="control-label"><em>Matricula:</em></label>
                    </div>
                    <div class="col-md-4">
                        <asp:Label ID="LblMatricula" runat="server" CssClass="control-label" Text="LblMatricula"></asp:Label>
                        <asp:Label ID="LblIdAlumno" runat="server" CssClass="control-label" Text="" Visible="false"></asp:Label>
                    </div>
                </div>
                <div class="row bg-success">
                    <div class="col-md-1 col-md-offset-5">
                        <label class="control-label"><em>Nombre:</em></label>
                    </div>
                    <div class="col-md-4">
                        <asp:Label ID="LblNombre" runat="server" CssClass="control-label" Text="LblNombre"></asp:Label>
                    </div>
                </div>
                <div class="row bg-success">
                    <div class="col-md-1 col-md-offset-5">
                        <label class="control-label"><em>Proyecto:</em></label>
                    </div>
                    <div class="col-md-4">
                        <asp:Label ID="LblProyecto" runat="server" CssClass="control-label" Text="LblProyecto"></asp:Label>
                    </div>
                </div>
                <div class="row bg-success">
                    <div class="col-md-1 col-md-offset-5">
                        <label class="control-label"><em>Solicitud:</em></label>
                    </div>
                    <div class="col-md-4">
                        <asp:LinkButton ID="LBSolicitud" runat="server" OnClick="LBSolicitud_Click">LinkButton</asp:LinkButton>
                    </div>
                </div>
            </fieldset>
        </div>
            </td>
        </tr>
    </table>
    <table id ="MiSolicitud" runat="server" class="table-responsive table" visible="false">
        <tr>
            <td>
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12">
                    <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                </div>
            </div>
            <fieldset>
                <div class="row "><div class="col-md-12">&nbsp;</div></div>
                <div class="row">
                    <div class="col-md-12">
                        <h1 class="text-center">Detalle de la asignación</h1>
                    </div>
                </div>
                <div class="row"><div class="col-md-12">&nbsp;</div></div>
                <div class="row">
                    <div class="col-md-12">
                        <h4 class="text-center bg-info">Datos del solicitante</h4>
                    </div>
                </div>
                <div class="row bg-success">
                    <div class="col-md-5 col-md-offset-1">
                        <label class="control-label"><em>Favor de presentarte para la ejecución de tu Servicio Becario en:</em></label>
                    </div>
                    <div class="col-md-5">
                        <asp:Label ID="LblCampus" runat="server" CssClass="control-label" Text="LblCampus"></asp:Label>
                    </div>
                </div>
                <div class="row bg-success">
                    <div class="col-md-1 col-md-offset-1">
                        <label class="control-label"><em>Nómina:</em></label>
                    </div>
                    <div class="col-md-1">
                        <asp:Label ID="LblNomina" runat="server" CssClass="control-label" Text="LblNomina"></asp:Label>
                    </div>
                    <div class="col-md-1">
                        <label class="control-label"><em>Nombre:</em></label>
                    </div>
                    <div class="col-md-5">
                        <asp:Label ID="LblNombreDetalle" runat="server" CssClass="control-label" Text="LblNombre"></asp:Label>
                    </div>
                </div>
                <div class="row bg-success">
                    <div class="col-md-1 col-md-offset-1">
                        <label class="control-label"><em>Puesto:</em></label>
                    </div>
                    <div class="col-md-5">
                        <asp:Label ID="LblPuesto" runat="server" CssClass="control-label" Text="LblPuesto"></asp:Label>
                    </div>
                </div>
                <div class="row bg-success">
                    <div class="col-md-2 col-md-offset-1">
                        <label class="control-label"><em>Periodo Académico:</em></label>
                    </div>
                    <div class="col-md-1">
                        <asp:Label ID="LblPeriodo" runat="server" CssClass="control-label" Text="LblPeriodo"></asp:Label>
                    </div>
                    <div class="col-md-1">
                        <label class="control-label"><em>Inicio:</em></label>
                    </div>
                    <div class="col-md-1">
                        <asp:Label ID="LblFechaInicio" runat="server" CssClass="control-label" Text="LblFechaInicio"></asp:Label>
                    </div>
                    <div class="col-md-1">
                        <label class="control-label"><em>Fin:</em></label>
                    </div>
                    <div class="col-md-1">
                        <asp:Label ID="LblFechaFin" runat="server" CssClass="control-label" Text="LblFechaFin"></asp:Label>
                    </div>
                </div>
                <div class="row bg-success">
                    <div class="col-md-1 col-md-offset-1">
                        <label class="control-label"><em>Ubicación:</em></label>
                    </div>
                    <div class="col-md-5">
                        <asp:Label ID="LblUbicacionFisica" runat="server" CssClass="control-label" Text="LblUbicacionFisica"></asp:Label>
                    </div>
                </div>
                <div class="row bg-success">
                    <div class="col-md-1 col-md-offset-1">
                        <label class="control-label"><em>Extensión:</em></label>
                    </div>
                    <div class="col-md-1">
                        <asp:Label ID="LblExtension" runat="server" CssClass="control-label" Text="LblExtension"></asp:Label>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label"><em>Correo electrónico:</em></label>
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="LblEmail" runat="server" CssClass="control-label" Text="LblEmail"></asp:Label>
                    </div>
                </div>
                <div class="row bg-success">
                    <div class="col-md-2 col-md-offset-1">
                        <label class="control-label"><em>Ubicación Alterna:</em></label>
                    </div>
                    <div class="col-md-5">
                        <asp:Label ID="LblUbicacionAlterna" runat="server" CssClass="control-label" Text="LblUbicacionAlterna"></asp:Label>
                    </div>
                    <div class="col-md-1">
                        <label class="control-label" ><em></em></label>
                    </div>
                    <div class="col-md-1">
                        <asp:Label ID="LblTelefono" runat="server" CssClass="control-label" Text="LblTelefono" Visible="false"></asp:Label>
                    </div>
                </div>
            </fieldset>
            <fieldset>
                <div class="row "><div class="col-md-12">&nbsp;</div></div>
                <div class="row">
                    <div class="col-md-12">
                        <h4 class="text-center bg-info">Descripción del proyecto</h4>
                    </div>
                </div>
                <div class="row bg-success">
                    <div class="col-md-1 col-md-offset-1">
                        <label class="control-label"><em>Nombre:</em></label>
                    </div>
                    <div class="col-md-10">
                        <asp:Label ID="ProyectoAsignado" runat="server" CssClass="control-label" Text="LblProyecto"></asp:Label>
                    </div>
                </div>
                <div class="row bg-success">
                    <div class="col-md-1 col-md-offset-1">
                        <label class="control-label"><em>Objetivo:</em></label>
                    </div>
                    <div class="col-md-10">
                        <asp:Label ID="LblObjetivo" runat="server" CssClass="control-label" Text="LblObjetivo"></asp:Label>
                    </div>
                </div>
                <div class="row bg-success">
                    <div class="col-md-1 col-md-offset-1">
                        <label class="control-label"><em>Justificación:</em></label>
                    </div>
                    <div class="col-md-10">
                        <asp:Label ID="LblJustificacion" runat="server" CssClass="control-label" Text="LblJustificacion"></asp:Label>
                    </div>
                </div>
            </fieldset>
            <fieldset>
                <div class="row "><div class="col-md-12">&nbsp;</div></div>
                <div class="row">
                    <div class="col-md-12">
                        <h4 class="text-center bg-info">Mis Actividades</h4>
                    </div>
                </div>
                <div class="row bg-success">
                    <div class="col-md-10 col-md-offset-1">
                        <asp:Label ID="LblActividades" runat="server" CssClass="control-label" Text="LblActividades"></asp:Label>
                    </div>
                </div>
            </fieldset>
            <div class="row "><div class="col-md-12">&nbsp;</div></div>
            <div class="row "><div class="col-md-12">&nbsp;</div></div>
            <div class="col-md-12 text-center center-block">
                <asp:Button ID="BtnCerrar" runat="server" Text="Cerrar" OnClick="BtnCerrar_Click" CssClass="btn btn-primary"/>
            </div>
        </div>
            </td>
        </tr>
    </table>

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