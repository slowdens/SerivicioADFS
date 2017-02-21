<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="CatPeriodo.aspx.cs" Inherits="ServicioBecario.Vistas.CatPeriodo" Culture="Auto" UICulture="Auto" %>
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
        function quitarValidar() {
            //Esto es para omitir la validación
            jQuery("form").validationEngine('detach');
        }

        function validar() {
            //Esto es para validar campos
            jQuery("form").validationEngine();
        }
        function soloNumeros(e, control) {
            var dato;
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



            dato = control.value;
            if (dato.length < 6)
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

    </script>
     <div class="row">
        <div class="col-md-12 text-center">
                 <h1 class="text-center">Servicio Becario</h1>
        </div>
    </div>
    <div class="row">
         <div class="col-md-12 text-center">
              <h4>Periodos</h4>           
         </div>
    </div>
    <a class="btn btn-primary btn-sm" id="btnA"><em class="glyphicon glyphicon-plus"></em> Agregar</a>
    <a class="btn btn-primary btn-sm" id="btnB"><em class="glyphicon glyphicon-search"></em> Buscar</a>
    <div class="ContAll">
    <div id="ContAg">
    <div class="jumbotron">
        <div class="row">
            <div class="col-md-1">
                <label>Código</label>
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtnombre" OnKeyPress = "return soloNumeros(event,this);" runat="server" CssClass="form-control validate[required]" placeholder="201611"></asp:TextBox>
            </div>
            <div class="col-md-1">
                <label>Descripción</label>
            </div>
            <div class="col-md-2">
                <asp:TextBox runat="server" ID="txtDescripcion" CssClass="form-control validate[required]" placeholder="Enero-Mayo 2016"></asp:TextBox>
            </div>
            <div class="col-md-1">
                <label>Fecha inicio</label>
            </div>
            <div class="col-md-2">
                <asp:TextBox CssClass="form-control validate[required]" ID="txtFechaInicio" OnKeyPress = "return formatofecha(event,this);" placeholder="dd/mm/aaaa" runat="server"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFechaInicio" />                                            
            </div>
            <div class="col-md-1">
                <label>Fecha fin</label>
            </div>
            <div class="col-md-2">
                <asp:TextBox CssClass="form-control validate[required]" ID="txtFechaFin" OnKeyPress = "return formatofecha(event,this);"  placeholder="dd/mm/aaaa" runat="server"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtFechaFin" />                                            
            </div>
            
        </div>
        <div class="row">
            <div class="col-md-offset-3 col-md-2">
                <asp:CheckBox runat="server" Checked="true" ID="chkactivo" Text="Activo" CssClass="checkbox" />
            </div>
            <div class="col-md-1">
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar"  CssClass="btn btn-primary" OnClientClick="validar();" OnClick="btnGuardar_Click" />
                <asp:Button ID="btnupdate" runat="server" Text="Modificar"  CssClass="btn btn-primary" OnClientClick="validar();" OnClick="btnupdate_Click" Visible="false" />
                <asp:HiddenField ID="hdfId_periodo" runat="server" />
            </div>
            <div class="col-md-1">
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar"  CssClass="btn btn-primary" OnClientClick="quitarValidar();" OnClick="btnCancelar_Click" Visible="false" />
            </div>
           
        </div>        
    </div>    
       </div>
        <div id="ContBu">   
            <div class="jumbotron">
                 <div class="row">
                <div class="col-md-1">
                    <label>Código</label>
                </div>
                <div class="col-md-2">
                    <asp:TextBox runat="server" OnKeyPress = "return soloNumeros(event,this);" ID="txtFiltrarCodigo" placeholder="201611" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-md-1">
                    <label> Descripción
                    </label>
                </div>
                <div class="col-md-2">
                    <asp:TextBox runat="server" ID="txtFiltrarDescripcion" CssClass="form-control" placeholder="Enero-Marzo 2016"></asp:TextBox>
                </div>
                <div class="col-md-1">
                    <label>Fecha inicio</label>
                </div>
                <div class="col-md-2">
                    <asp:TextBox runat="server" ID="txtFiltrarFechainicio" OnKeyPress = "return formatofecha(event,this);"  placeholder="dd/mm/aaaa" CssClass="form-control"></asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtFiltrarFechainicio" />                                            
                </div>
                <div class="col-md-1">
                    <label>Fecha fin</label>
                </div>
                <div class="col-md-2">
                    <asp:TextBox runat="server" ID="txtFiltrarFechaFin" OnKeyPress = "return formatofecha(event,this);"  placeholder="dd/mm/aaaa" CssClass="form-control"></asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtFiltrarFechaFin" />                                            
                </div>
            </div>
              <div class="row">
                  <div class="col-md-1  ">
                      <label>Estatus</label>
                      <%--<asp:CheckBox ID="Checkactivo" runat="server" CssClass="checkbox" Text="Activo"  />--%>

                     
                  </div>
                  <div class="col-md-2">
                      <asp:DropDownList runat="Server" ID="ddlEstatus" CssClass="form-control" >
                          <asp:ListItem Value="">--Seleccione--</asp:ListItem>
                          <asp:ListItem Value="1">Activo</asp:ListItem>
                          <asp:ListItem Value="0">In activo</asp:ListItem>
                      </asp:DropDownList>
                  </div>
                  <div class="col-md-offset-3 col-md-2">
                      <asp:Button  CssClass="btn btn-primary" ID="btnFiltar" runat="server" Text="Filtrar" OnClick="btnFiltar_Click"/>
                  </div>
              </div>  
            </div>
           
    <div class="row table-condensed">
        <div class="col-md-12 col-sm-12 col-lg-12 col-xs-12">
            <asp:Panel runat="server" ID="pnlGridview" Visible="false" CssClass="hscrollbar">
                <asp:GridView ID="GridView1" AllowPaging="True"  PageSize ="20"  CssClass="table" AutoGenerateColumns="false"   DataKeyNames="Periodo" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnPageIndexChanging="GridView1_PageIndexChanging">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775"></AlternatingRowStyle>
                    <Columns>
                        <asp:BoundField DataField="Periodo" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" HeaderText="Periodo" SortExpression="Periodo"  />                        
                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción" Visible="true" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" />
                        <asp:BoundField DataField="Fechainicio" HeaderText="Fecha de inicio" Visible="true" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" />
                        <asp:BoundField DataField="Fechafin" HeaderText="Fecha fin" Visible="true" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" />
                        <asp:CheckBoxField DataField="Activo" HeaderText="Activo" SortExpression="Activo" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" />
                        
                        
                        
                        
                        
                        <asp:CommandField ButtonType="Image" HeaderText="Modificar" SelectImageUrl="~/images/update.PNG" ShowSelectButton="True" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center"/>
                        <asp:TemplateField HeaderText="Eliminar" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="imbEliminar" CommandArgument='<%# Eval("Periodo") %>' runat="server" ImageUrl="~/images/EliminarR.png" OnClick="imbEliminar_click" />
                                <cc1:ConfirmButtonExtender ID="cbe" runat="server" DisplayModalPopupID="mpe" TargetControlID="imbEliminar"></cc1:ConfirmButtonExtender>
                                <cc1:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlPopup" TargetControlID="imbEliminar"
                                    OkControlID="btnYes" CancelControlID="btnNo" BackgroundCssClass="modalBackground1">
                                </cc1:ModalPopupExtender>
                                <asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style="display: none">
                                    <div class="header">
                                        Confirmación
                                    </div>
                                    <div class="body">
                                        <asp:Label ID="Label12" runat="server" Text="¿Desea eliminar el periodo?"></asp:Label>
                                    </div>
                                    <div class="footer" align="right">
                                        <asp:Button ID="btnYes" runat="server" Text="Si" CssClass="yes" />
                                        <asp:Button ID="btnNo" runat="server" Text="No" CssClass="no" />
                                    </div>
                                </asp:Panel>
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
        </div>
    </div>
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
                                    <%--<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>--%>
                                    <h4 class="modal-title">
                                        <asp:Label ID="lblCabeza" Font-Names="open sans" Font-Size="13px"  runat="server" Text="Este es el titulo"></asp:Label></h4>
                                  </div>
                                  <div class="modal-body">
                                        <div class="col-md-1">
                                            <asp:Image ID="Image2" runat="server" ImageUrl="~/images/Alertaz.png" />
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
