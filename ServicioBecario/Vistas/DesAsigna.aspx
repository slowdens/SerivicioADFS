<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="DesAsigna.aspx.cs" Inherits="ServicioBecario.Vistas.DesAsigna" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
            <div class="col-md-12 " >
                 <h1 class="text-center">Servicio Becario</h1>
            </div>
    </div>
    <div class="row">
        <div class="col-md-12 text-center ">
            <h4>Desasociar</h4>            
        </div>
    </div>
  <div class="row">
      <div class="col-md-12">
          <fieldset>
            
              <legend>  <h4 class="text-center">Datos del alumno becario</h4></legend>
              <div class="row">
                  <div class="col-md-1" >
                     <label>Matrícula</label>
                  </div>
                  <div class="col-md-3">
                      <asp:Label ID="lblMatricula" runat="server" Text=""></asp:Label>
                  </div>
                  <div class="col-md-1">
                      <label>Nombre: </label>
                  </div>
                  <div class="col-md-3">
                      <asp:Label ID="lblNombreBecario" runat="server" Text=""></asp:Label>
                  </div>
                  <div class="col-md-1">
                      <label> Periodo </label>                      
                  </div>
                  <div class="col-md-3">
                      <asp:Label ID="lblPeriodo" runat="server" Text=""></asp:Label>
                  </div>
              </div>
              <div class="row">
                  <div class="col-md-1">
                      <label>Proyecto</label>
                  </div>
                  <div class="col-md-3">
                      <asp:Label ID="lblProyecto" runat="server" Text=""></asp:Label>
                  </div>
                  <div class="col-md-1">
                      <label>Nivel academico</label>
                  </div>
                  <div class="col-md-3">
                      <asp:Label ID="lblNivelEstudios" runat="server" Text=""></asp:Label>
                  </div>
                  <div class="col-md-1">
                      <label>Campus</label>
                  </div>
                  <div class="col-md-2">
                      <asp:Label ID="lblCampus" runat="server" Text=""></asp:Label>
                  </div>
              </div>
          </fieldset>
      </div>
  </div>  
  <div class="row">
      <div class="col-md-12">
          <fieldset>
              <h4 class="text-center"> Datos Responsable  </h4>
              <legend></legend>
              <div class="row">
                  <div class="col-md-1">
                      <label>Nómina</label>
                  </div>
                  <div class="col-md-1">
                      <asp:Label ID="lblNomina" runat="server" Text=""></asp:Label>
                  </div>
                  <div class="col-md-1">
                      <label>Nombre</label>
                  </div>
                  <div class="col-md-3">
                      <asp:Label ID="lblNombreSb" runat="server" Text=""></asp:Label>
                  </div>
                   <div class="col-md-1">
                       <label>Puesto</label>
                  </div>
                   <div class="col-md-3">
                       <asp:Label ID="lblPuesto" runat="server" Text=""></asp:Label>
                  </div>
              </div>
              <div class="row">
                   <div class="col-md-2">
                       <label>Departamento</label>
                  </div>
                   <div class="col-md-2">
                       <asp:Label ID="lblDepartamento" runat="server" Text=""></asp:Label>
                  </div>
                   <div class="col-md-3">
                       <label>Ubicación fisica</label>
                  </div>
                   <div class="col-md-3">
                       <asp:Label ID="lblUbicacionFisica" runat="server" Text=""></asp:Label>
                  </div>
              </div>
          </fieldset>
      </div>
  </div>
   <div class="jumbotron">
        <div class="row">
            <div class="col-md-12">
                <fieldset>
                   
                    <legend> <h4 class="text-center">Quitar asignación</h4></legend>
                    <div class="row">
                        <div class="col-md-1">
                            <label>Justificación</label>
                        </div>
                        <div class="col-md-11">
                            <asp:TextBox ID="txtJustificacion" CssClass="form-control" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                    <br />

                    <div class="row">
                        <div class="col-md-1 col-md-offset-5 ">
                            <asp:Button ID="btnDesAsignar" CssClass="btn btn-primary" runat="server" Text="Desasociar" OnClick="btnDesAsignar_Click" />
                        </div>
                    </div>
                </fieldset>
            </div>
         </div>
   </div>
  















     <%--Se tiene que copear este face para junto un metodo masa para poderse ejecuatar--%>
    <asp:Label ID="Label14" runat="server" Text=""></asp:Label>
        <!-- ModalPopupExtender -->
        <cc1:ModalPopupExtender ID="mp1" runat="server" PopupControlID="Panel1" TargetControlID="Label14"
            CancelControlID="cancel" BackgroundCssClass="modalBackground">
        </cc1:ModalPopupExtender>
        <asp:Panel ID="Panel1" runat="server"  align="center" style = "display:none">
             <div class="modal-dialog">
                                <div class="modal-content" style="background-color:#E6E6E6">
                                  <div class="modal-header">
                                    <%--<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>--%>
                                    <h4 class="modal-title">
                                        <asp:Label ID="lblCabeza" runat="server" Text="Este es el titulo"></asp:Label></h4>
                                  </div>
                                  <div class="modal-body">
                                        <div class="col-md-1">
                                            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Alertaz.png" />
                                        </div>
                                        <div class="col-md-11">
                                            <asp:Label ID="lblcuerpo" runat="server" Text=""></asp:Label>&hellip;
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
