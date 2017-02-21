<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="ExcepcionNomina.aspx.cs" Inherits="ServicioBecario.Vistas.ExcepcionNomina" Culture="Auto" UICulture="Auto" %>
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
        function soloNumeros(e) {
            key = e.keyCode || e.which;
            tecla = String.fromCharCode(key).toLowerCase();
            letras = "0123456789";
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


            if (letras.indexOf(tecla) == -1 && !tecla_especial) {
                return false;
            }
            else
            {
                return true;
            }
        }
        function paraNomina(e, control) {
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
            if (dato.length<=8)
            {
                if (letras.indexOf(tecla) == -1 && !tecla_especial) {
                    return false;
                }
            }
            else
            {

                if (key == "8" || key == "37" || key == "39" || key == "46") {

                    return true;
                }
                else {
                    return false;
                }
            }
            
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
            if (dato.length <= 9)
            {
                if (letras.indexOf(tecla) == -1 && !tecla_especial) {
                    return false;
                }
            }
            else
            {
                if (key == "8" || key == "37" || key == "39" || key == "46") {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
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
        }
        function openModal()
        {
            $('#myModal').modal('show');
        }
    </script>
    <div class="row">
           <div class="col-md-12">
                <h1 class="text-center">Servicio Becario</h1>
           </div>
    </div>
    <div class="row">
        <div class="col-md-12 text-center">
            <h4>Excepción de nómina</h4> 
        </div>    
    </div>
    <a class="btn btn-primary btn-sm" id="btnA"><em class="glyphicon glyphicon-plus"></em> Agregar</a>
    <a class="btn btn-primary btn-sm" id="btnB"><em class="glyphicon glyphicon-search"></em> Buscar</a>
    <div class="ContAll">
    <div id="ContAg">
    <div class="jumbotron">
        <div class="row">
            <div class="col-md-1">
                <label>Nómina</label>
                <%--<asp:Label ID="Label1" runat="server" Text="Nomina"></asp:Label>--%>
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtNomina" OnKeyPress = "return paraNomina(event,this);" placeholder="L00000000" CssClass="form-control validate[required]" runat="server" OnTextChanged="txtNomina_TextChanged" AutoPostBack="True"></asp:TextBox>
            </div>
            <div class="col-md-1">
                <label>Nombre</label>
                <%--<asp:Label ID="Label2" runat="server"  CssClass="control-label"  Text="Nombre"></asp:Label>--%>
            </div>
            <div class="col-md-3">
                <asp:Label ID="lblNombre" runat="server" Text=""></asp:Label>
            </div>
            <div class="col-md-2">
                <label>Cantidad de becarios</label>
                <%--<asp:Label ID="Label3" runat="server" Text="Cantidad de becarios"></asp:Label>--%>
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtNumeroBecarios" placeholder="0" CssClass="form-control validate[required]" OnKeyPress = "return soloNumeros(event);" runat="server"></asp:TextBox>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-md-1">
                <label>Justificación</label>
                <%--<asp:Label ID="Label4" runat="server" Text="Justificación:"></asp:Label>--%>
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtJustificacion" runat="server" placeholder="Descripción" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
            </div>
            <div class="col-md-2">
                <label>Fecha inicio</label>

                <%--<asp:Label ID="Label5" runat="server" Text="Fecha inicio"></asp:Label>--%>
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtFechaInicio" runat="server"  placeholder="dd/mm/aaaa"  OnKeyPress = "return formatofecha(event,this);"  CssClass="form-control validate[required]" ></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFechaInicio" />
            </div>
            <div class="col-md-2">
                <label>Fecha fin </label>
                <%--<asp:Label ID="Label6" runat="server" Text="Fecha fin"></asp:Label>--%>
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtFechaFin" runat="server"  placeholder="dd/mm/aaaa" OnKeyPress = "return formatofecha(event,this);"  CssClass="form-control validate[required]"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtFechaFin" />
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-md-1">
                <label>Periodo</label>
               <%-- <asp:Label ID="Label7" runat="server" Text="Periodo"></asp:Label>--%>
            </div>
            <div class="col-md-3">
                <asp:DropDownList ID="ddlPeriodo" CssClass="form-control validate[required]" runat="server" OnDataBound="ddlPeriodo_DataBound"></asp:DropDownList>
            </div>
            <div class="col-md-offset-2 col-md-1">
                <asp:Button ID="btnGuardar" CssClass="btn btn-primary" runat="server" Text="Agregar" OnClick="btnGuardar_Click" OnClientClick="validar();" Visible="true" />
                <asp:Button ID="btnUpdate" CssClass="btn btn-primary" runat="server" Text="Actualizar" OnClientClick="validar();" Visible="false" OnClick="btnUpdate_Click" />
                
            </div>
            <div class="col-md-1">
                <asp:Button  ID="btncancelar" Visible="false" CssClass="btn btn-primary" runat="server" Text="Cancelar" OnClientClick="quitarValidar();" OnClick="btncancelar_Click"/>
            </div>
            
        </div>
    </div>
        </div>
        <div id="ContBu">   
    <asp:Panel ID="PnlExcepcionNominas" runat="server" Visible="false">
    <div class="jumbotron">
        <div class="row">
             <div class="col-md-12 text-center" >                 
                        <%--<asp:Label ID="Label15" runat="server" Text="Filtrar"></asp:Label>                  --%>
                    <fieldset class="text-center">
                        <legend class="text-center">                            
                            <h4>Filtros</h4>
                        </legend>
                    </fieldset>
             </div>
        </div>
        <div class ="row">
            <div class="col-md-2">
                <label>Nómina</label>
                <%--<asp:Label ID="Label9" runat="server" Text="Nomina"></asp:Label>--%>
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtfiltrarNomina" placeholder="L00000000" OnKeyPress = "return paraNomina(event,this);"  CssClass="form-control" runat="server" Text="" OnTextChanged="txtfiltrarNomina_TextChanged" AutoPostBack="true" ></asp:TextBox>
            </div> 
            <div class="col-md-2">
                <label>Fecha inicio</label>
                <%--<asp:Label ID="Label11" runat="server" Text="Fecha inicio"></asp:Label>--%>
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtfiltraFechaInicio"  placeholder="dd/mm/aaaa" OnKeyPress = "return formatofecha(event,this);"  CssClass="form-control" runat="server"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtfiltraFechaInicio" />
            </div>          
            <div class="col-md-1">
                <label>Fecha fin</label>
                <%--<asp:Label ID="Label12" runat="server" Text="Fecha fin"></asp:Label>--%>
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtfiltrarFechaFin"  placeholder="dd/mm/aaaa" CssClass="form-control" OnKeyPress = "return formatofecha(event,this);" runat="server"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtfiltrarFechaFin" />
            </div>     
            
        </div>
        <br />
        <div class="row">
            <div class="col-md-2">
                <label>Cantidad de becarios</label>
                <%--<asp:Label ID="Label10" runat="server" Text="Cantidad de becarios"></asp:Label>--%>
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtfiltrarCantidaddeBecarios" placeholder="0" OnKeyPress = "return soloNumeros(event);" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
            
            <div class="col-md-2" >
                <label>Periodo</label>
                
            </div>
            <div class="col-md-3">
                <asp:DropDownList ID="ddlFiltrarPeriodo" CssClass="form-control" runat="server" OnDataBound="ddlFiltrarPeriodo_DataBound"></asp:DropDownList>
            </div>
            
            <div class="col-md-1">
                <asp:Button ID="btnFiltrar" runat="server"  CssClass="btn btn-primary" Text="Filtrar" OnClientClick="quitarValidar();" OnClick="btnFiltrar_Click" />
            </div>     
        </div>
    </div>
    <div class="row table-condensed">
        <div class="col-md-12 col-sm-12 col-lg-12 col-xs-12 ">
            <asp:Panel ID="pnlxxx" runat="server" CssClass="hscrollbar">
                <asp:GridView ID="GVExcepcionNominas" runat="server"  CssClass="table"  CellPadding ="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" DataKeyNames="id_nomina_especifico,Periodo" OnSelectedIndexChanged="GVExcepcionNominas_SelectedIndexChanged" OnRowDataBound="GVExcepcionNominas_RowDataBound" AllowPaging="True" OnPageIndexChanging="GVExcepcionNominas_PageIndexChanging" OnRowDeleting="GVExcepcionNominas_RowDeleting" PageSize="20">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>  
                        <asp:BoundField DataField="id_nomina_especifico" HeaderText="id_nomina_especifico" SortExpression="id_nomina_especifico" Visible="false" />                        
                        <asp:BoundField DataField="Nomina"   HeaderStyle-Width="20px" HeaderText="Nómina"  Visible="true" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"/>
                        <asp:BoundField DataField="Cantidad de becarios" HeaderStyle-VerticalAlign="Bottom"   HeaderStyle-Width="150px" HeaderText="Cantidad de becarios" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"  Visible="true" />                    
                        <asp:BoundField DataField="Fecha_inicio" HeaderText="Fecha inicio" HeaderStyle-Width="100px" Visible="true" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"  />
                        <asp:BoundField DataField="Fecha_fin" HeaderText="Fecha fin" Visible="true" HeaderStyle-Width="100px" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"/>
                        <asp:BoundField DataField="Justificacion" HeaderText="Justificacion" Visible="true" HeaderStyle-Width="200px" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                        <asp:BoundField DataField="Registro" HeaderText="Registro" Visible="true" HeaderStyle-Width="100px" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"  />
                        <asp:BoundField DataField="Fecha_registro" HeaderText="Fecha_registro" Visible="true" HeaderStyle-Width="150px" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"  />
                        <asp:BoundField DataField="Periodo" HeaderText="Periodo" Visible="true" HeaderStyle-Width="150px"  ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                        <asp:BoundField DataField="Periodo" HeaderText="Periodo" Visible="false" HeaderStyle-Width="150px" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"   />                                                
                        <asp:CommandField SelectImageUrl="~/images/update.png"  HeaderText="Modificar" ShowSelectButton="True" ButtonType="Image" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"  ItemStyle-HorizontalAlign="Center"/>
                        <asp:TemplateField HeaderText="Eliminar" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="imbEliminar"   CommandArgument='<%# Eval("id_nomina_especifico") %>'  runat="server" ImageUrl="~/images/EliminarR.png" OnClick="imbEliminar_click" />
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
        <asp:HiddenField ID="hdfid" runat="server" />
        <asp:HiddenField ID="hdfDesion" runat="server" />
    </div>
    </asp:Panel>
    </div></div>
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
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
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
