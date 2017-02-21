<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="Menus_permisos.aspx.cs" Inherits="ServicioBecario.Vistas.Menus_permisos" %>
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
        function quitarValidar() {
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
            <h1><label>Menu permiso</label></h1>
            <hr />
        </div>        
    </div>
    <div class="jumbotron">
        <div class="row">
            <div class="col-md-1">
                <label>Permiso</label>
                <%--<asp:Label ID="Label1" runat="server" Text="Permiso"></asp:Label>--%>
            </div>
            <div class="col-md-3">
                <asp:DropDownList ID="ddlpermiso" CssClass="form-control validate[required] " runat="server" OnDataBound="ddlpermiso_DataBound"  ></asp:DropDownList>
            </div>
            <div class="col-md-2">
                <asp:CheckBox ID="chkMostrarFoltros" CssClass="checkbox-inline checkbox" runat="server" AutoPostBack="true" Text="Mostrar filtros" OnCheckedChanged="chkMostrarFoltros_CheckedChanged" />
            </div>
        </div>
    </div>
    <asp:Panel ID="pnlFiltro1" runat="server" Visible="false">
    <div class="jumbotron">
        <div class="row">
            <div class="col-md-12 text-center" >
                <fieldset>
                    <legend class="text-center">
                        <h4><label>Filtros</label></h4>
                    </legend>
                </fieldset>
                
                
            </div>
        </div>
        <div class="row">
            <div class="col-md-1">
                <label>Menús</label>
                <%--<asp:Label ID="Label2" runat="server" Text="Menus"></asp:Label>--%>
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtFiltraMenus" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
            <div class="col-md-1">
                <label>Pantalla</label>
                <%--<asp:Label ID="Label4" runat="server" Text="Pantalla"></asp:Label>--%>
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtpantala" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
            <div class="col-md-1">
                <label>Padre</label>
                <%--<asp:Label ID="Label5" runat="server" Text="Padre"></asp:Label>--%>
            </div>
             <div class="col-md-3">
                 <asp:DropDownList ID="ddlFiltrarPadre" CssClass="form-control" runat="server" OnDataBound="ddlFiltrarPadre_DataBound" ></asp:DropDownList>
            </div>
            <div class="col-md-1">
                <asp:Button ID="btnFiltrarMenus" runat="server" Text="Filtrar" CssClass="btn btn-primary" OnClick="btnFiltrarMenus_Click" />
            </div>          
        </div>
    </div>
   </asp:Panel>
    <div class="">
        <div class="row table-condensed">
            <div class="col-md-12 col-sm-12 col-lg-12 col-xs-12">
                <asp:Panel ID="pnlxx" CssClass="hscrollbar" runat="server">
                    <asp:GridView ID="GvPermisoMenu" CssClass="table" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" 
                        DataKeyNames="id_menu" AutoGenerateColumns="false" AllowPaging="True"
                          PageSize="5" Width="100%" OnPageIndexChanging="GvPermisoMenu_PageIndexChanging">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" CssClass="letra table table-hover" />
                        <Columns>
                            <asp:BoundField DataField="id_menu" HeaderText="id_menu" SortExpression="id_menu" Visible="false" />
                            <asp:BoundField DataField="Nombre" HeaderText="Menu" Visible="true" />
                            <asp:BoundField DataField="Link" HeaderText="Link" Visible="true" />
                            <asp:BoundField DataField="Padre" HeaderText="Padre" Visible="true" />
                            <asp:TemplateField HeaderText="Agregar">
                                <ItemTemplate >
                                    <asp:CheckBox ID="chkselecioname" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle   BackColor="#3B83C0" Font-Bold="True" ForeColor="White" CssClass="letra"/>
                        <HeaderStyle   BackColor="#3B83C0" Font-Bold="True" ForeColor="White" CssClass="letra " />
                        <PagerStyle BackColor="#3B83C0" ForeColor="White" HorizontalAlign="Center"  CssClass="letra" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="letra" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" CssClass="letra" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" CssClass="letra"/>
                        <SortedAscendingHeaderStyle BackColor="#506C8C" CssClass="letra" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" CssClass="letra"/>
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" CssClass="letra"/>
                    </asp:GridView>
                </asp:Panel>
                
            </div>
            
            
        </div>
        <div class="row">
            <div class="col-md-1">
                <asp:Button ID="btnGuarMenus" runat="server" Text="Guadar" CssClass=" btn btn-primary"  OnClientClick="validar();" OnClick="btnGuarMenus_Click" />
            </div>
        </div>
        
    </div>
    <div class="jumbotron">
        <div class="row">
            <div class="col-md-3 col-md-offset-3"">
                <asp:CheckBox ID="chkVerLigadosPermisosMenus" CssClass="checkbox-inline checkbox" runat="server" Text="Permisos Menus" OnCheckedChanged="chkVerLigadosPermisosMenus_CheckedChanged"  AutoPostBack="true"/>            
            </div>
            <div class="col-md-2">
                <asp:CheckBox ID="chkMostrarFiltro2" runat="server" CssClass="checkbox-inline checkbox" Visible="false" Text="Mostrar filtro" OnCheckedChanged="chkMostrarFiltro2_CheckedChanged" AutoPostBack="true" />
            </div>
        </div>
    </div>
    <asp:Panel ID="pnlLigarRolesPermisos" runat="server">
        <asp:Panel ID="pnlFiltro2" runat="server" Visible="false">
    <div class="jumbotron">
        <div class="row">
            <div class="col-md-12 text-center" >
                <fieldset>
                    <legend>
                        <h4><label>Filtros</label></h4>
                    </legend>
                </fieldset>                
            </div>
        </div>
        <div class="row">
            <div class ="col-md-1">
                <label>Permisos</label>
                <%--<asp:Label ID="Label6" runat="server" Text="Permisos"></asp:Label>--%>
            </div>
            <div class="col-md-3">
                <asp:DropDownList ID="ddlFiltrarPermisos" CssClass="form-control"  runat="server" OnDataBound="ddlFiltrarPermisos_DataBound"></asp:DropDownList>
            </div>
            <div class="col-md-1">
                <label>
                    Menu
                </label>
                <%--<asp:Label ID="Label7" runat="server" Text="Menu"></asp:Label>--%>
            </div>
            <div class="col-md-3">
                <asp:TextBox ID="txtfiltrarMenusegundo" runat="server" CssClass="form-control"></asp:TextBox>
            </div>           
            
        </div> 
        <br />
           
        <div class="row">
            <div class="col-md-1">
                <label>Pantalla</label>
                <%--<asp:Label ID="Label8" runat="server" Text="Pantalla"></asp:Label>--%>
            </div>
            <div class="col-md-3">
                <asp:TextBox ID="txtfiltrarPantalla" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
            <div class="col-md-1">
                <label>Padre</label>
                <%--<asp:Label ID="Label9" runat="server" Text="Padre"></asp:Label>--%>
            </div>
            <div class="col-md-3">
                <asp:DropDownList ID="ddlFiltrarpadresegundo" CssClass="form-control"  runat="server" OnDataBound="ddlFiltrarpadresegundo_DataBound"></asp:DropDownList>
            </div>
            <div class="col-md-1">
                <asp:Button ID="btnFiltrarDos" runat="server" CssClass="btn btn-primary" Text="Filtrar" OnClick="btnFiltrarDos_Click" />
            </div>
        </div>    
    </div>
    </asp:Panel>
    <div class=""> 
        <div class="row table-condensed">
        <div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">
            <asp:Panel ID="pnlxxx" runat="server" CssClass="hscrollbar">
                <asp:GridView ID="GvLigarolesPermisos" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" 
                    AutoGenerateColumns="False" DataKeyNames="id_menu_permiso"
                    AllowPaging="True" PageSize="6" OnPageIndexChanging="GvLigarolesPermisos_PageIndexChanging" 
                    OnRowDataBound="GvLigarolesPermisos_RowDataBound" 
                    OnRowDeleting="GvLigarolesPermisos_RowDeleting"
                     CssClass="table" Width="100%" Height="100px">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" CssClass="letra" />
                    <Columns>
                        <asp:BoundField DataField="id_menu_permiso" HeaderText="id_menu_permiso" SortExpression="id_menu_permiso" visible="false"/>
                        <asp:BoundField DataField="Permiso" HeaderText="Permiso" SortExpression="Permiso" />                    
                        <asp:BoundField DataField="Menu" HeaderText="Menu" SortExpression="Menu" />
                        <asp:BoundField DataField="Link" HeaderText="Link" SortExpression="Link" />
                        <asp:BoundField DataField="Padre" HeaderText="Padre" SortExpression="Padre" />
                        <asp:TemplateField HeaderText="Eliminar">
                            <ItemTemplate>
                                <asp:ImageButton ID="imbEliminar"   CommandArgument='<%# Eval("id_menu_permiso") %>'  runat="server" ImageUrl="~/images/Eliminar.png" OnClick="imbEliminar_click" />
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
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#3B83C0" Font-Bold="True" ForeColor="White" CssClass="letra" />
                    <HeaderStyle BackColor="#3B83C0" Font-Bold="True" ForeColor="White" CssClass="letra"  />
                    <PagerStyle BackColor="#3B83C0" ForeColor="White" HorizontalAlign="Center" CssClass="letra" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="letra" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" CssClass="letra"/>
                    <SortedAscendingCellStyle BackColor="#E9E7E2" CssClass="letra" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C"  CssClass="letra"/>
                    <SortedDescendingCellStyle BackColor="#FFFDF8" CssClass="letra"/>
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" CssClass="letra" />
                </asp:GridView>
            </asp:Panel>
            
        </div>
    </div>
    </div>
    
    </asp:Panel>
    <asp:HiddenField ID="hdfDesion" runat="server" />
    <asp:HiddenField ID="hdfid_permiso" runat="server" />

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
