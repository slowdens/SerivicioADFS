<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="Usuario_rol_nuevo.aspx.cs" Inherits="ServicioBecario.Vistas.Usario_rol_nuevo" %>
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
        function Bandera() {
            localStorage["Bandera"] = 1;
        }
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
        function validaNominaRol()
        {
            //Traemos objetos
            var TxtNomina = $("#<%=txtNomina.ClientID%>");
            var ddlRoles = $("#<%=DdlListaRoles.ClientID%>");
            var aNomina = $("#<%=txtAgragerNomina.ClientID%>");
            
            //Agregamos clase de validacion
            TxtNomina.addClass("validate[required]");
            ddlRoles.addClass("validate[required]");

            //quitamos la clase
            aNomina.removeClass("validate[required]").addClass("");


            //Se hace la validacion
            jQuery("form").validationEngine();
        }
        function validarSoloFormalarioNominaNueva()
        {
                       
            var aNomina = $("#<%=txtAgragerNomina.ClientID%>");
            var TxtNomina = $("#<%=txtNomina.ClientID%>");
            var ddlRoles = $("#<%=DdlListaRoles.ClientID%>");


            //Agregamos la validacion solo al anominia
            aNomina.addClass("validate[required]");

            //Quitamos validacion
            TxtNomina.removeClass("validate[required]").addClass("");
            ddlRoles.removeClass("validate[required]").addClass("");

            //Se hace la validacion
            jQuery("form").validationEngine();
        }
        function soloLetras(e, control) {
            var dato;
            key = e.keyCode || e.which;
            tecla = String.fromCharCode(key).toLowerCase();
            letras = "l0123456789";
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
            if (dato.length <= 8)
            {
                if (letras.indexOf(tecla) == -1 && !tecla_especial) {
                    return false;
                }
            }
            else
            {
                if (key == "8" || key == "37" || key == "39" || key == "46")
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            
        }
     
    </script>
        <div class="row">
            <div class="col-md-12 " >
                 <h1 class="text-center">Servicio Becario</h1>
            </div>
        </div>
    <div class="row" >
        <div class="col-md-12 text-center">
            <h4 class="text-center">Usuario rol</h4>     
            
        </div>        
    </div>
    <a class="btn btn-primary btn-sm" id="btnA"><em class="glyphicon glyphicon-plus"></em> Agregar</a>
    <a class="btn btn-primary btn-sm" id="btnB"><em class="glyphicon glyphicon-search"></em> Buscar</a>
    <div class="ContAll">
    <div id="ContAg">
    <asp:Panel ID="pnlOculto" runat="server" Visible="false">
        <div class="jumbotron">

            <div class="row">
                <div class=" col-md-offset-5 col-md-4">
                    <asp:CheckBox ID="chkAgregar" runat="server" CssClass="checkbox" Text="Agregar nómina no registrada" AutoPostBack="true" OnCheckedChanged="chkAgregar_CheckedChanged" />
                    <cc1:ToggleButtonExtender ID="ToggleButtonExtender1"
                        runat="server"
                        ImageHeight="35"
                        ImageWidth="35"
                        TargetControlID="chkAgregar"
                        CheckedImageUrl="~/images/AgregarAzul.png"
                        UncheckedImageUrl="~/images/AgregarVerde.png"
                        UncheckedImageOverAlternateText="Agregar usuarios no registrados" />
                </div>
            </div>


            <br />
            <asp:Panel ID="pnlAgregarNuevo" Visible="false" runat="server">

                <div class="row">
                    <div class="col-md-offset-4 col-md-1">
                        <label>Nómina</label>
                     
                    </div>
                    <div class="col-md-3">
                        <asp:TextBox ID="txtAgragerNomina" placeholder="L00000000" CssClass="form-control " runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-1">
                        <asp:Button ID="btnAgregarEmpleado" CssClass="btn btn-default" runat="server" Text="Guardar" OnClientClick="validarSoloFormalarioNominaNueva();" OnClick="btnAgregarEmpleado_Click" />
                    </div>
                </div>

            </asp:Panel>

        </div>
    </asp:Panel>
    <div class="jumbotron">
        <div class="row ">
                <div class ="col-md-12 text-center ">
                        <fieldset>                         
                            
                            <legend class="text-center">
                                 <h4>Asignar nómina a un rol</h4>
                            </legend>
                        </fieldset>                      
                </div>
            </div>
        <div class="row">
            <div class="col-md-1">
                <label>Nómina</label>
            
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtNomina" runat="server" placeholder="L00000000" CssClass="form-control" OnTextChanged="txtNomina_TextChanged" OnKeyPress = "return soloLetras(event,this);" AutoPostBack="True"></asp:TextBox>
            </div>
            <div class="col-md-2">
                <label>Nombre </label>
            
            </div>
            <div class="col-md-3">
                <asp:Label ID="lblNombre" runat="server" Text=""></asp:Label>
            </div>
            <div class="col-md-1">
                <label>Rol</label>
            
            </div>
            <div class="col-md-3">
                     <asp:DropDownList ID="DdlListaRoles" runat="server" CssClass="form-control " OnDataBound="DdlListaRoles_DataBound">
                         <asp:ListItem Text="Please Select" Value="" />
                     </asp:DropDownList>
            </div>
        </div>
        <div class="row">
            <div class="col-md-offset-4 col-md-1 ">
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass=" btn btn-primary" OnClick="btnGuardar_Click" OnClientClick="validaNominaRol();"/>
                <asp:Button ID="btnEditar" runat="server" Text="Modificar" CssClass="btn btn-primary " Visible="false" OnClick="btnEditar_Click" />
                
            </div>
            <div class="col-md-1">
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-primary " Visible="false" OnClick="btnCancelar_Click"/>                
            </div>
        </div>
        
    </div>   
        </div>
        <div id="ContBu"> 
    <asp:Panel ID="pnlRolesUsuarios" runat="server" Visible="false">
        <div class="jumbotron">
            <div class="row">
                <div class ="col-md-12 text-center ">
                    <fieldset>
                        <legend class="text-center">
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
                    <asp:DropDownList ID="ddlfiltrarRol" runat="server" CssClass="form-control"   OnDataBound="ddlfiltrarRol_DataBound"  ></asp:DropDownList>
                    
                </div>
                <div class="col-md-1">
                    <label>Nómina</label>
                    
                </div>
                <div class="col-md-2">
                    <asp:TextBox ID="txtFiltraNomimina" CssClass="form-control" placeholder="L00000000" OnKeyPress = "return soloLetras(event,this);" runat="server" AutoPostBack="true" OnTextChanged="txtFiltraNomimina_TextChanged"></asp:TextBox>
                </div>
                <div class="col-md-1">
                    <label>Campus</label>

                </div>
                <div class="col-md-3">
                    <asp:DropDownList CssClass="form-control" ID="ddlCampus"    runat="server" OnDataBound="ddlCampus_DataBound"></asp:DropDownList>
                </div>
                <div class="col-md-1">
                    <asp:Button ID="btnFiltar" runat="server" Text="Filtrar"  CssClass="btn btn-primary" OnClientClick="quitarValidar();" OnClick="btnFiltar_Click" />
                </div>
            </div>
        </div>
        <div >
            <div class="row table-condensed">
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <asp:Panel ID="pnlxx" runat="server" CssClass="hscrollbar">
                        <asp:GridView ID="GvRolesUsuarios"  CssClass="table" runat="server" CellPadding="4"
                            DataKeyNames="Nomina" ForeColor="#333333" GridLines="None" Width="100%"
                            AutoGenerateColumns="false" OnRowDataBound="GvRolesUsuarios_RowDataBound"
                            OnRowDeleting="GvRolesUsuarios_RowDeleting" AllowPaging="True" PageSize="20" OnPageIndexChanging="GvRolesUsuarios_PageIndexChanging" OnSelectedIndexChanged="GvRolesUsuarios_SelectedIndexChanged">
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" DataField="Nomina" HeaderText="id_usuario" SortExpression="Nomina" Visible="false" />
                                <asp:BoundField ItemStyle-CssClass="text-left" HeaderStyle-CssClass="text-left" DataField="Rol" HeaderText="Rol" Visible="true" />
                                <asp:BoundField ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" DataField="Nomina" HeaderText="Nómina" Visible="true" />
                                <asp:BoundField ItemStyle-CssClass="text-left" HeaderStyle-CssClass="text-left" DataField="Usuario" HeaderText="Usuario" Visible="true" />
                                <asp:BoundField ItemStyle-CssClass="text-left" HeaderStyle-CssClass="text-left" DataField="Campus" HeaderText="Campus" Visible="true" />
                                <asp:CommandField HeaderStyle-CssClass="text-center" ButtonType="Image" SelectImageUrl="~/images/update.PNG" ShowSelectButton="True" HeaderText="Modificar" ItemStyle-HorizontalAlign="Center" />
                                <asp:TemplateField HeaderText="Eliminar"  ItemStyle-HorizontalAlign="center" HeaderStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imbEliminar" CommandArgument='<%# Eval("Nomina") %>' runat="server" ImageUrl="~/images/EliminarR.png" OnClick="imbEliminar_click" />
                                        <cc1:ConfirmButtonExtender ID="cbe" runat="server" DisplayModalPopupID="mpe" TargetControlID="imbEliminar"></cc1:ConfirmButtonExtender>
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
                     <br />
                    <br />      
                </div>
            </div>
        </div>
    </asp:Panel>
            </div>
        </div>
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
