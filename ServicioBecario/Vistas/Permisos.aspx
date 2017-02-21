<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="Permisos.aspx.cs" Inherits="ServicioBecario.Vistas.Permisos" %>
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
        function validar()
        {
            //Esto es para validar campos
            jQuery("form").validationEngine();
        }
        function quitarValidar()
        {
            //Esto es para omitir la validación
            jQuery("form").validationEngine('detach');
        }
        function confirmar(rs) {

            var r = confirm(rs);
            $('#<%=hdfDesion.ClientID%>').val(r);
        }
    </script>
    <div class="row" >
        <div class="col-md-12 text-center">
            <h1><label>Permisos</label></h1>
            <hr />
        </div>
    </div>
    <div class="jumbotron">
        <div class="row">
            <div class="col-md-1 col-md-offset-2" >
                <label>Nombre</label>
               <%-- <asp:Label ID="Label1" runat="server" Text="Nombre"></asp:Label>--%>
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control validate[required]"  ></asp:TextBox>
            </div>
            <div class="col-md-1">
                    <label>Descripción</label>
                  <%--<asp:Label ID="Lbel" runat="server" Text="Descripción"></asp:Label>--%>
            </div>      
            <div class="col-md-2">
                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control validate[required]"></asp:TextBox>
            </div>  
            <div class="col-md-1">
                <asp:Panel ID="PnlGuardar" runat="server" Visible="true">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass=" btn btn-primary" OnClientClick="validar();" OnClick="btnGuardar_Click"/>
                </asp:Panel>
                <asp:Panel ID="pnlupdate" runat="server" Visible="false">
                    <asp:Button CssClass=" btn btn-primary" ID="btnActualizar" runat="server" Text="Actualizar" OnClick="btnActualizar_Click" />
                </asp:Panel>
            </div>
        </div>
        <br />
        <div class="row">          
            <div class="col-md-2 col-md-offset-4" >
                <asp:CheckBox ID="chkverPermisos"  CssClass="checkbox checkbox-inline" Text="Ver permisos" runat="server" OnClientClick="quitarValidar();" OnCheckedChanged="chkverPermisos_CheckedChanged" AutoPostBack="true" />                
            </div>            
        </div>
    </div>
    <asp:Panel ID="pnlPermisos" runat="server" Visible="false" Height="100%">
        <div class ="jumbotron">
            <div class="row">
                <div class="col-md-12">
                    <fieldset>
                        <legend class="text-center">
                            <h4><label>Filtros</label></h4>
                        </legend>
                    </fieldset>
                </div>
            </div>
            <div class="row">
                <div class ="col-md-1 col-md-offset-1">
                    <label>Permisos</label>
                    <%--<asp:Label ID="Label2" runat="server" Text="Permisos"></asp:Label>--%>
                </div>
                <div class="col-md-3">
                    <asp:DropDownList ID="ddlFiltrarPermisos" CssClass="form-control" runat="server" OnDataBound="ddlFiltrarPermisos_DataBound"></asp:DropDownList>
                </div>
                <div class="col-md-1">
                    <label>Descripción</label>
                    <%--<asp:Label ID="Label4"   runat="server" Text="Descripción"></asp:Label>--%>
                </div>
                <div class="col-md-2">
                    <asp:TextBox ID="txtfiltrarDescripcion" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-md-1">
                    <asp:Button ID="btnFiltrar" runat="server" CssClass="btn btn-primary" Text="Filtrar" OnClick="btnFiltrar_Click" />
                </div>
            </div>
        </div>
        <div>
            <div class="row table-condensed">
                <div class="col-md-12 col-xs-12 col-lg-12 col-sm-12" >
                    <asp:Panel ID="pnlxx" runat="server" CssClass="hscrollbar">
                          <asp:GridView ID="GvPermisos" CssClass="table" runat="server" CellPadding="4" 
                                DataKeyNames="id_permiso" ForeColor="#333333" GridLines="None" 
                                AutoGenerateColumns="False" 
                                 AllowPaging="True" PageSize="10" OnSelectedIndexChanged="GvPermisos_SelectedIndexChanged" 
                                OnPageIndexChanging="GvPermisos_PageIndexChanging" OnRowDataBound="GvPermisos_RowDataBound" 
                                OnRowDeleting="GvPermisos_RowDeleting" Width="100%" >
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" CssClass="letra"/>
                                <Columns>
                                    <asp:BoundField DataField="id_permiso" HeaderText="id_permiso" SortExpression="id_permiso" Visible="false" />
                                    <asp:BoundField DataField="Permiso" HeaderText="Permiso" Visible="true" />
                                    <asp:BoundField DataField="Descripción" HeaderText="Descripción" Visible="true" />                                                    
                                    <asp:CommandField ButtonType="Image" SelectImageUrl="~/images/update.PNG" ShowSelectButton="True" HeaderText="Editar" />
                                    <asp:TemplateField HeaderText="Eliminar">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imbEliminar"   CommandArgument='<%# Eval("id_permiso") %>'  runat="server" ImageUrl="~/images/EliminarR.png" OnClick="imbEliminar_click" />
                                            <cc1:ConfirmButtonExtender ID="cbe" runat="server" DisplayModalPopupID="mpe" TargetControlID="imbEliminar">
                                             </cc1:ConfirmButtonExtender>
                                            <cc1:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlPopup" TargetControlID="imbEliminar"
                                            OkControlID="btnYes" CancelControlID="btnNo" BackgroundCssClass="modalBackground1">
                                            </cc1:ModalPopupExtender>
                                            <asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style="display: none">
                                                <div class="header">
                                                    Confirmación
                                                </div>
                                                <div class="body">
                                                    <asp:Label ID="Label12" runat="server" Text="¿Desea eliminar el registro?"></asp:Label>                            
                                                </div>
                                                <div class="footer" align="right">
                                                    <asp:Button ID="btnYes" runat="server" Text="Si" CssClass="yes" />
                                                    <asp:Button ID="btnNo" runat="server" Text="No" CssClass="no" />
                                                </div>
                                            </asp:Panel>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle BackColor="#999999"  CssClass="letra"/>
                        <FooterStyle BackColor="#3B83C0" Font-Bold="True" ForeColor="White" CssClass="letra"/>
                        <HeaderStyle BackColor="#3B83C0" Font-Bold="True" ForeColor="White" CssClass="letra"/>
                        <PagerStyle BackColor="#3B83C0" ForeColor="White" HorizontalAlign="Center" CssClass="letra" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="letra" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                    </asp:Panel>
                   <br />
                   <br />
                </div>
            </div>
        </div>
   </asp:Panel>
     <asp:HiddenField ID="hdfid_permiso" runat="server" />
     <asp:HiddenField ID="hdfDesion" runat="server" />

    
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
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
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
                                    <button type="button" id="cancel" class="btn" style="background-color:#1D4860;color:#F6F6F6" data-dismiss="modal">Close</button>        
                                  </div>
                                </div><!-- /.modal-content -->
                              </div><!-- /.modal-dialog -->
            
        </asp:Panel>
 <%--   Fin de la face del modal--%>

</asp:Content>
