<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="Rol_nuevo.aspx.cs" Inherits="ServicioBecario.Vistas.Rol_nuevo" Culture="Auto" UICulture="Auto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="../scripts/footable.bootstrap.min.css" type="text/css" /> 
    <link rel="stylesheet"  href="../scripts/Enginee/css/validationEngine.jquery.css" type="text/css"/> 
    <link rel="stylesheet" href="../scripts/Estiloss.css" type="text/css" />   
    <script type="text/javascript" src="../scripts/Enginee/js/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../scripts/Enginee/js/jquery.validationEngine.js" charset="utf-8"></script>
    <style>
        .ContAll{
            background-color:#eee;
            border:1px solid #eee;
        }
        /*#2d6ca2*/
    </style>
    <script type="text/javascript">
        function Bandera() {
            localStorage["Bandera"] = 1;
        }
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
            validar();
        }

        function formatofecha(e, control) {
            var dato;
            key = e.keyCode || e.which;
            tecla = String.fromCharCode(key).toLowerCase();
            letras = "/0123456789";
            especiales = "8-37-39-46";

            var array = especiales.split("-");


            tecla_especial = false
            //for (var i in especiales) {
            //    if (key == especiales[i]) {
            //        tecla_especial = true;
            //        break;
            //    }
            //}

            for (var i in array) {
                var ar = array[i];
                if (key == array[i]) {
                    tecla_especial = true;
                    break;
                }
            }

            dato = control.value;
            if (dato.length <= 9) {
                if (letras.indexOf(tecla) == -1 && !tecla_especial) {
                    return false;
                }
            }
            else {
                if (key == "8" || key == "37" || key == "39" || key == "46") {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        function paraNomina(e) {
           
            key = e.keyCode || e.which;
            tecla = String.fromCharCode(key).toLowerCase();
            letras = "l0123456789";
            especiales = "8-37-39-46";

            tecla_especial = false
            for (var i in especiales) {
                if (key == especiales[i]) {
                    tecla_especial = true;
                    break;
                }
            }

  
            if (letras.indexOf(tecla) == -1 && !tecla_especial) {
                return false;
            }


        }
       
    </script>
    <div class="row">
            <div class="col-md-12 " >
                 <h1 class="text-center">Servicio Becario</h1>
            </div>
    </div>
    <div class="row" >
        <div class="col-md-12">
            <h4 class="text-center">Roles</h4>                    
        </div>        
    </div>
    <a class="btn btn-primary btn-sm" id="btnA"><em class="glyphicon glyphicon-plus"></em> Agregar</a>
    <a class="btn btn-primary btn-sm" id="btnB"><em class="glyphicon glyphicon-search"></em> Buscar</a>
    <div class="ContAll">
        <div id="ContAg">
    <div class="jumbotron">
        <div class="row ">
            <div class="col-md-1 col-md-offset-2" >
                <label> Nombre</label>                
            </div>
            <div class="col-md-2">
                  <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control validate[required]" placeholder="ADMIN"></asp:TextBox>
            </div>
            <div class="col-md-1">
                <label>Descripción</label>                
            </div>
            <div class="col-md-2">
                  <asp:TextBox ID="txtdescripcion" runat="server" CssClass="form-control validate[required]" placeholder="Administrador"></asp:TextBox>
            </div>
            <div class="col-md-1">
                <asp:Panel ID="pnlguardar" runat="server" Visible="true">
                       <asp:Button ID="btnGuardar" runat="server" Text="Guardar"  CssClass="btn btn-primary" OnClick="btnGuardar_Click" OnClientClick="validar();"/>
                </asp:Panel>                      
            </div>
        </div>
        <br />
        <div class="row">
             <asp:Panel ID="pnlactualizar" runat="server" Visible="false">
                 <div class="col-md-offset-4 col-md-1">
                     <asp:Button ID="btnupdate" runat="server" Text="Modificar"  CssClass="btn btn-primary" OnClick="btnupdate_Click" 
                            onclientclick="confirmar('¿En verdad desea actualizar el rol?');" />
                 </div>
                 <div class="col-md-1">
                     <asp:Button runat="server" OnClientClick="quitarValidar();" CssClass="btn btn-primary" Text="Cancelar" id="btnmodificar" OnClick="btnmodificar_Click"/>
                 </div>                         
            </asp:Panel>   
        </div>    
        
    </div>
    </div>
        <div id="ContBu">
    <asp:Panel ID="pnlMostrarGrid" runat="server" Visible="false">
        <div class="jumbotron">
            <div class="row">
                <div class="col-md-12 text-center">
                    <fieldset>                                                
                        <legend>
                            <h4 class="text-center">Filtros</h4>
                        </legend>
                    </fieldset>
                </div>
            </div>
            <div class="row">
                <div class="col-md-1">
                    <label>Rol</label>                    
                </div>
                <div class="col-md-3">
                    <asp:DropDownList ID="ddlFiltrarRol" CssClass="form-control" runat="server" OnDataBound="ddlFiltrarRol_DataBound" > </asp:DropDownList>
                </div>
                <div class="col-md-1">
                    <label>Descripción</label>                    
                </div>
                <div class="col-md-2">
                    <asp:TextBox ID="txtfiltrarDescripcion" CssClass="form-control" runat="server" placeholder="Administrador"></asp:TextBox>
                </div>
                <div class="col-md-1">
                    <label>Fecha de creación</label>                    
                </div>
                <div class="col-md-2">
                    <asp:TextBox ID="txtfiltarFecha" OnKeyPress="return formatofecha(event,this);"  CssClass="form-control" placeholder="dd/mm/aaaa"  runat ="server"></asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender1" TargetControlID="txtfiltarFecha" runat="server" />
                </div>
                <div class="col-md-1">
                    <asp:Button ID="btnFiltar" runat="server" OnClientClick="quitarValidar();" CssClass="btn btn-primary" Text="Filtrar" OnClick="btnFiltar_Click" />
                </div>
            </div>
        </div>
        <div>
            <div class="row table-condensed ">
                <div class="col-md-12 col-sm-12 col-lg-12 col-xs-12">
                    <asp:Panel ID="pnlxxx" CssClass="hscrollbar" runat="server">
                        <asp:GridView ID="Gvroles" runat="server"  AllowPaging="True" AutoGenerateColumns="false" 
                            BorderStyle="Solid" BorderWidth="0px" CellPadding="4" ForeColor="#333333" GridLines="None" 
                            PageSize="20" OnPageIndexChanging="Gvroles_PageIndexChanging" 
                            OnSelectedIndexChanged="Gvroles_SelectedIndexChanged" 
                            DataKeyNames="id_rol" OnRowDeleting="Gvroles_RowDeleting" OnRowDataBound="Gvroles_RowDataBound"  
                            CssClass="table" Width="100%">

                            <AlternatingRowStyle BackColor="White" ForeColor="#284775"  />
                
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="#3B83C0" Font-Bold="True" ForeColor="White" CssClass="letra" />
                            <HeaderStyle BackColor="#3B83C0" Font-Bold="True" ForeColor="White" CssClass="letra" />
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle BackColor="#3B83C0" ForeColor="White" HorizontalAlign="Center" CssClass="letra" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                            <Columns>                    
                                <asp:BoundField ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" DataField="id_rol" HeaderText="id_rol" SortExpression="id_rol" Visible="false" />
                                <asp:BoundField ItemStyle-CssClass="text-left" HeaderStyle-CssClass="text-left" DataField="Nombre" HeaderText="Rol" Visible="true" />
                                <asp:BoundField ItemStyle-CssClass="text-left" HeaderStyle-CssClass="text-left" DataField="Descripción" HeaderText="Descripción" Visible="true" />
                                <asp:BoundField ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" DataField="Fecha_creación" HeaderText="Fecha de creación" Visible="true" />
                                <asp:CommandField ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" ButtonType="Image"  HeaderText="Modificar" SelectImageUrl="~/images/update.PNG" ShowSelectButton="True" />
                                <asp:TemplateField HeaderText="Eliminar" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" >
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imbEliminar"   CommandArgument='<%# Eval("id_rol") %>'  runat="server" ImageUrl="~/images/EliminarR.png" OnClick="imbEliminar_click" />
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
                        </asp:GridView>
                    </asp:Panel>                
                    
                </div>
            </div>
        </div>
    </asp:Panel>
    </div>
        </div>



   <asp:HiddenField ID="hdfid_rol" runat="server" />
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
                                    <button type="button" id="cancel" class="btn" style="background-color:#1D4860;color:#F6F6F6" data-dismiss="modal">Cerrar</button>        
                                  </div>
                                </div><!-- /.modal-content -->
                              </div><!-- /.modal-dialog -->
            
        </asp:Panel>
 <%--   Fin de la face del modal--%>



</asp:Content>
