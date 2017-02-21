﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="BecarioExcento.aspx.cs" Inherits="ServicioBecario.Vistas.BecarioExcento" Culture="Auto" UICulture="Auto" %>
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
            else {
                if (key == "8" || key == "37" || key == "39" ||key == "46")
                {
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
       
        function paraMatriculas(e, control) {
            var dato;
            key = e.keyCode || e.which;
            tecla = String.fromCharCode(key).toLowerCase();
            letras = "a0123456789";
            especiales = "8-37-39-46";
            var array = especiales.split("-");
            tecla_especial = false
                      

            for (var i in array) {
                var ar = array[i];
                if (key == array[i]) {
                    tecla_especial = true;
                    break;
                }
            }



            dato = control.value;
            if (dato.length <= 8) {
                if (letras.indexOf(tecla) == -1 && !tecla_especial) {
                    return false;
                    //No permite toner el numero
                }
            }
            else {
                if (key == "8" || key == "37" || key == "39" || key == "46") {
                    return true
                }
                else {
                    return false;
                }

            }
        }









    </script>
    <div class="row">        
        <div class="col-md-12 text-center">
            <h1>Servicio Becario</h1>            
        </div>        
    </div>
    <div class="row">        
        <div class="col-md-12 text-center">
            <h4>Excepción de becarios</h4>
            
        </div>        
    </div>
    <a class="btn btn-primary btn-sm" id="btnA"><em class="glyphicon glyphicon-plus"></em> Agregar</a>
    <a class="btn btn-primary btn-sm" id="btnB"><em class="glyphicon glyphicon-search"></em> Buscar</a>
    <div class="ContAll">
    <div id="ContAg">
    <div class="jumbotron">
        <div class="row">
            <div class="col-md-1 ">
                <label>
                    Matrícula
                </label>
                
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtMatricula" placeholder="A00000000"   OnKeyPress="return paraMatriculas(event,this);" CssClass="form-control validate[required]" runat="server" AutoPostBack="true" OnTextChanged="txtMatricula_TextChanged"></asp:TextBox>
            </div>
            <div class="col-md-1">
                <label>
                    Nombre:
                </label>
                
            </div>
            <div class="col-md-3">
                <asp:Label ID="lblNombre" runat="server" Text=""></asp:Label>
            </div>
            <div class="col-md-1">
                <label>Periodo</label>                
            </div>
            <div class="col-md-3">
                <asp:DropDownList ID="ddlPeriodo" CssClass="form-control validate[required]" runat="server" OnDataBound="ddlPeriodo_DataBound" ></asp:DropDownList>
            </div>            
        </div>
        <br />
        <div class="row">
            <div class="col-md-2">
                <label>Fecha inicio</label>               
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtFechaInico" CssClass="form-control validate[required]"  placeholder="dd/mm/aaaa" runat="server" OnKeyPress = "return formatofecha(event,this);"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFechaInico" />
            </div>
            <div class="col-md-1">
                <label>Fecha fin</label>
               
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtFechaFin" CssClass="form-control validate[required]"  placeholder="dd/mm/aaaa" OnKeyPress = "return formatofecha(event,this);" runat="server"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtFechaFin" />
            </div>
            <div class="col-md-1">               
                <label>Justificación</label>
            </div>
            <div class="col-md-3">
                <asp:TextBox ID="txtJustificacion" CssClass="form-control validate[required]" placeholder="Descripción" runat="server" TextMode="MultiLine"></asp:TextBox>
            </div>
            
        </div>
        <div class="row">
            <div class="col-md-1 col-md-offset-4">                
                <asp:Button ID="btnGuardar" CssClass="btn btn-primary" OnClientClick="validar();"  runat="server" Text="Agregar" OnClick="btnGuardar_Click" />
                <asp:Button ID="btnActualizar" OnClientClick="validar();" Visible="false" runat="server" CssClass="btn btn-primary" Text="Modificar" OnClick="btnActualizar_Click" />     
            </div>
            <div class="col-md-1">
                <asp:Button ID="btnCancelar" OnClientClick="quitarValidar();" Visible="false" runat="server" CssClass="btn btn-primary" Text="Cancelar" OnClick="btnCancelar_Click" />     
            </div>
           
        </div>
        
    </div>
        </div>
        <div id="ContBu"> 
    <asp:Panel ID="pnlMostrar" runat="server" Visible="false">
    <div class="jumbotron">
        <div class="row">
            <div class="col-md-12">
                <fieldset>
                    <legend class="text-center">
                        <h4>Filtros </h4>
                    </legend>
                </fieldset>
            </div>
        </div>
        <div class="row">
            <div class="col-md-offset-3 col-md-1">
                <label>Matrícula</label>
                
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtFiltrarMatricula"  placeholder="A00000000" OnKeyPress="return paraMatriculas(event,this);"   CssClass="form-control" runat="server"></asp:TextBox>
            </div>
            <div class="col-md-1">
                <label>Periodo </label>
          </div>      
     
      

        
            <div class="col-md-3">
                <asp:DropDownList ID="ddlFiltrarPeriodo" CssClass="form-control" runat="server" OnDataBound="ddlFiltrarPeriodo_DataBound"></asp:DropDownList>
            </div>
          </div>
          <div class="row">
            <div class=" col-md-offset-3 col-md-1">
                <label>Inicio</label>
                
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtFiltarFechaIncio" OnKeyPress = "return formatofecha(event,this);"  placeholder="dd/mm/aaaa"  CssClass="form-control" runat="server"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtFiltarFechaIncio" />
            </div>
            <div class="col-md-1">
                <label>Fin </label>
                
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtFiltrarFechaFin" OnKeyPress = "return formatofecha(event,this);"  placeholder="dd/mm/aaaa" CssClass="form-control" runat="server"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtFiltrarFechaFin" />
            </div>
            
        </div>
        <br />
        <div class="row">
            <div class="col-md-1 col-md-offset-6">
                <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" OnClientClick="quitarValidar();" CssClass="btn btn-primary" OnClick="btnFiltrar_Click"/>
            </div>
        </div>
    </div>
    <div class="row table-condensed">
        <div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">
            <asp:Panel ID="pnlxx" runat="server" CssClass="hscrollbar">
                <asp:GridView ID="GVMostrar"   DataKeyNames="id_excepcion_becario"  runat="server" AutoGenerateColumns="false" 
                CellPadding="4" Width="100%" ForeColor="#333333" BorderStyle="Solid" BorderWidth="0px" PageSize="10"
                GridLines="None" OnSelectedIndexChanged="GVMostrar_SelectedIndexChanged"
                OnRowDataBound="GVMostrar_RowDataBound" OnRowDeleting="GVMostrar_RowDeleting" CssClass="table">
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:BoundField DataField="id_excepcion_becario" HeaderText="id_excepcion_becario" SortExpression="id_excepcion_becario" Visible="false" />

                    <asp:BoundField DataField="matricula" HeaderText="Matrícula" Visible="true" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />  
                    <asp:BoundField DataField="Nombre" HeaderText="Periodo" Visible="true" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />  
                    <asp:BoundField DataField="fecha_inicio" HeaderText="Fecha inicio" Visible="true" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" /> 
                    <asp:BoundField DataField="Fecha_fin" HeaderText="Fecha fin" Visible="true" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" /> 
                    <asp:BoundField DataField="justificacion" HeaderText="Justificación" Visible="true" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" /> 
                    <asp:BoundField DataField="Usuario_que_registro" HeaderText="Quien registro" Visible="true" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" /> 
                    <asp:BoundField DataField="calificacion" HeaderText="Calificación" Visible="true" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" /> 
                    <asp:CommandField ButtonType="Image" SelectImageUrl="~/images/update.PNG" HeaderText="Modificar" ShowSelectButton="True"  ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center"/>
                    <asp:TemplateField HeaderText="Eliminar" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="imbEliminar"   CommandArgument='<%# Eval("id_excepcion_becario") %>'  runat="server" ImageUrl="~/images/EliminarR.png" OnClick="imbEliminar_click" />
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
                <HeaderStyle BackColor="#3B83C0" Font-Bold="True" ForeColor="White" CssClass="letra" />
                <PagerStyle BackColor="#3B83C0" ForeColor="White" HorizontalAlign="Center" CssClass="letra" />
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
    </asp:Panel>
</div></div>
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
    <asp:HiddenField ID="hdfDesion" runat="server" />
    <asp:HiddenField ID="hd_id" runat="server" />

</asp:Content>
