﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="Asignados.aspx.cs" Inherits="ServicioBecario.Vistas.Asignados" %>
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
        <div class="col-md-12 text-center">
            <h1>Servicio Becario</h1>            
        </div>        
    </div>
    <div class="row">
        <div class="col-md-12 text-center " >
            <h4>Becarios asignados </h4>    
        </div>
    </div>
    <asp:Panel runat="server" ID="pnlmostrarGrid">
        <div class="row table-condensed">
            <div class="col-md-12 col-lg-12 col-sm-12 col-xs-12">
                <asp:Panel ID="pnlgrs" runat="server" CssClass=" hscrollbar">
                     <asp:GridView ID="GvDatos" AutoGenerateColumns="false" OnRowCreated="GvDatos_RowCreated" CssClass="table" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775"></AlternatingRowStyle>
                    <Columns>
                        <asp:BoundField DataField="Matricula" HeaderText="Matrícula" SortExpression="Matricula" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center"></asp:BoundField>
                        <asp:BoundField DataField="NombreAlumnos" HeaderText="Nombre" SortExpression="NombreAlumnos" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center"></asp:BoundField>
                        <asp:BoundField DataField="carrera" HeaderText="Carrera" SortExpression="Carrera" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center"></asp:BoundField>                        
                        <asp:BoundField DataField="Proyecto" HeaderText="Proyecto" SortExpression="Proyecto" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center"></asp:BoundField>
                        <asp:BoundField DataField="Inicio" HeaderText="Inicio" SortExpression="Inicio" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center"></asp:BoundField>
                        <asp:BoundField DataField="Fin" HeaderText="Fin" SortExpression="Fin" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center"></asp:BoundField>
                        <asp:TemplateField HeaderText="Ubicaciones" ControlStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButtonDetalles" Width="180px" CssClass="text-center" runat="server" AlternateText='ubicaciones' Style="cursor: pointer;" />
                                        <cc1:PopupControlExtender ID="PopupDetalles" runat="server"
                                            DynamicServiceMethod="GetDynamicContent"
                                            DynamicContextKey='<%#Eval("id_Misolicitud")%>'
                                            TargetControlID="ImageButtonDetalles"
                                            Position="Right"
                                            DynamicControlID="detallesgridPanel"
                                            PopupControlID="detallesgridPanel">
                                        </cc1:PopupControlExtender>
                                    </ItemTemplate>
                                    <ControlStyle CssClass="text-center" />
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center"  />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <%--<asp:Button ID="btnLlevarRegistro" CommandName='<%#Eval("Periodo") + "!" +Eval("Matricula")%>' CssClass="btn btn-primary" runat="server" Text="Ver" OnClick="btnLlevarRegistro_Click" />--%>
                                <asp:ImageButton ID="btnmandar"  runat="server" Height="45px" Width="45px" ImageUrl="~/images/00 banco de íconos-94-32.png" onclick="btnLlevarRegistro_Click"/>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                    <EditRowStyle BackColor="#999999"></EditRowStyle>
                    <FooterStyle BackColor="#3B83C0" Font-Bold="True" ForeColor="White"></FooterStyle>
                    <HeaderStyle BackColor="#3B83C0" Font-Bold="True" ForeColor="White"></HeaderStyle>
                    <PagerStyle HorizontalAlign="Center" BackColor="#3B83C0" ForeColor="White"></PagerStyle>
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333"></RowStyle>
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333"></SelectedRowStyle>
                    <SortedAscendingCellStyle BackColor="#E9E7E2"></SortedAscendingCellStyle>
                    <SortedAscendingHeaderStyle BackColor="#506C8C"></SortedAscendingHeaderStyle>
                    <SortedDescendingCellStyle BackColor="#FFFDF8"></SortedDescendingCellStyle>
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE"></SortedDescendingHeaderStyle>
                </asp:GridView>
                </asp:Panel>
               <asp:Panel ID="detallesgridPanel" runat="server"
                            Style="display: none; background-color: #5099F9; font-size: 11px; border: solid 1px Black;">
               </asp:Panel>
                <br /><br /><br /><br /><br /><br />
            </div>
        </div>
    </asp:Panel>
    


    
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
