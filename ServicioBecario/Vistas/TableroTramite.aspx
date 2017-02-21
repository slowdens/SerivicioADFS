<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="TableroTramite.aspx.cs" Inherits="ServicioBecario.Vistas.TableroTramite" %>
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
    
    <div class="row">
            <div class="col-md-12 " >
                 <h1 class="text-center">Servicio Becario</h1>
            </div>
    </div>
    <div class="row">
        <div class="col-md-12 text-center">
            <h4>Tablero</h4>            
        </div>
    </div>
    <asp:Button ID="Volver" runat="server" Text="Volver"  OnClick="Volver_Click" CssClass="btn btn-primary btn-sm" />
    <div class="row table-condensed">
        <div class="col-md-12 col-sm-12 col-xs-12 ">
            <div class="table-responsive">
                <asp:Panel ID="Panel2" runat="server" CssClass="hscrollbar">
                    <asp:GridView ID="GvDatosGenerales" runat="server" CssClass="table" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="Periodo" HeaderText="Periodo" SortExpression="Periodo" ItemStyle-HorizontalAlign="Justify" />
                            <asp:BoundField DataField="Nivel academico" HeaderText="Nivel académico" SortExpression="Nivel academico" />
                            <asp:BoundField DataField="Campus Solicitante" HeaderText="Campus Solicitante" SortExpression="Campus Solicitante" />
                            <asp:BoundField DataField="Matricula" HeaderText="Matrícula" SortExpression="Matricula" />
                            <asp:BoundField DataField="Nombre alumno" HeaderText="Nombre alumno" SortExpression="Nombre alumno" />
                            <asp:BoundField DataField="Programa academico" HeaderText="Programa académico" SortExpression="Programa academico" />
                            <asp:BoundField DataField="Periodo cursado" HeaderText="Periodo cursado" SortExpression="Periodo cursado" />
                            <asp:BoundField DataField="Nombre Solicitante" HeaderText="Nombre Solicitante" SortExpression="Nombre Solicitante" />
                            <asp:BoundField DataField="Ubicacion fisica" HeaderText="Ubicacion fisica" SortExpression="Ubicacion fisica" />
                            <asp:BoundField DataField="Ubicacion alterna" HeaderText="Ubicacion alterna" SortExpression="Ubicacion alterna" />
                            
                            <asp:CheckBoxField DataField="Asistencia" HeaderText="Asistencia" SortExpression="Asistencia" />
                            <asp:BoundField DataField="Proyecto" HeaderText="Proyecto" SortExpression="Proyecto" />
                            <asp:BoundField DataField="Evalucacion becario" HeaderText="Evalucación becario" SortExpression="Evalucacion becario" />
                            <asp:BoundField DataField="SB calificacion" HeaderText="SB calificación" SortExpression="SB calificacion" />
                            <asp:BoundField DataField="Correo SB" HeaderText="Correo SB" SortExpression="Correo SB" />
                            <asp:BoundField DataField="Extencion" HeaderText="Extención" SortExpression="Extencion" />
                            <asp:BoundField DataField="Departamento" HeaderText="Departamento" SortExpression="Departamento" />
                            <asp:BoundField DataField="Puesto" HeaderText="Puesto" SortExpression="Puesto" />
                            <asp:BoundField DataField="Campus Becario" HeaderText="Campus Becario" SortExpression="Campus Becario" />
                            <asp:TemplateField HeaderText="Historial">
                                <ItemTemplate >
                                    <asp:Button ID="btnMostrarHistorial"  runat="server"  CommandName='<%#Eval("Periodo") + "!" +Eval("Matricula")%>'  Text="Historial" CssClass="btn btn-primary"  OnClick="btnMostrarHistorial_Click"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Quitar">
                                <ItemTemplate>
                                    <asp:Button ID="btnDesAsignacion" runat="server" Text="Eliminar asignación" CommandName='<%#Eval("Periodo") + "!" +Eval("Matricula")%>'  CssClass="btn btn-primary" OnClick="btnDesAsignacion_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#3B83C0" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#3B83C0" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#3B83C0" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                </asp:Panel>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <!---Datos generales--->
            <asp:Panel ID="pnlHistoriaAsignacion" Visible="false" runat="server">
                <fieldset>                    
                    <legend class="text-center" ><h4>Historia de asignación</h4></legend>
                    <div class="row table-condensed">
                        <div class="col-md-12 col-sm-12 col-xs-12 ">
                            <div class="table-responsive">
                                <asp:Panel ID="Panel3" CssClass="hscrollbar"  runat="server">
                                    <asp:GridView ID="GvHistorialAsignacion" CssClass="table" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None">

                                        
                                        <Columns>
                                            <asp:BoundField DataField="Nomina solicitante anterior" HeaderText="Nomina solicitante anterior" SortExpression="Nomina solicitante anterior" />
                                            <asp:BoundField DataField="Nombre solicitante anterior" HeaderText="Nombre solicitante anterior" SortExpression="Nombre solicitante anterior" />
                                            <asp:BoundField DataField="Departamento solicitante anterior" HeaderText="Departamento solicitante anterior" SortExpression="Departamento solicitante anterior" />
                                            <asp:BoundField DataField="Proyecto" HeaderText="Proyecto" SortExpression="Proyecto" />
                                            <asp:BoundField DataField="Nomina que des-asocio" HeaderText="Nómina para desasociar" SortExpression="Nomina que des-asocio" />
                                            <asp:BoundField DataField="Fecha des-asociacion" HeaderText="Fecha desasociar" SortExpression="Fecha des-asociacion" />
                                            <asp:BoundField DataField="Justificacion" HeaderText="Justificacion" SortExpression="Justificacion" />
                                        </Columns>
                                        <EditRowStyle BackColor="#999999" />
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <FooterStyle BackColor="#3B83C0" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#3B83C0" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#3B83C0" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />

                                    </asp:GridView>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </fieldset>
            </asp:Panel>
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
                                   <%-- <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>--%>
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
