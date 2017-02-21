<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="SeguimientoSolicitudes.aspx.cs" Inherits="ServicioBecario.Vistas.SeguimientoSolicitudes" %>
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
        $(document).ready(function () {
            $(".imgHelp").mouseover(function () {
                $(".Help").css({ "display": "block" });
            });
            $(".imgHelp").mouseout(function () {
                $(".Help").css({ "display": "none" });
            });
        });
        function confirmar()
        {
            var opt = confirm("Desea cancelar la solicitud");
            $('#<%=hdfDecide.ClientID%>').val(opt);
            
        }

        function validar() {
            jQuery("form").validationEngine();
        }
        function paraNomina(e, control) {
            var dato;
            key = e.keyCode || e.which;
            tecla = String.fromCharCode(key).toLowerCase();
            letras = "l0123456789";
            especiales = "8-37-39-46";

            var array = especiales.split("-");

            tecla_especial = false
            ////for (var i in especiales) {
            ////    if (key == especiales[i]) {
            ////        tecla_especial = true;
            ////        break;
            ////    }
            ////}
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
                if (key == "8" || key == "37" || key == "39" || key == "46") {
                    return true;
                }
                else {
                    return false;
                }
            }
            
        }
        function paraMatriculas(e) {
            key = e.keyCode || e.which;
            tecla = String.fromCharCode(key).toLowerCase();
            letras = "a0123456789";
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
           <div class="col-md-12">
                <h1 class="text-center">Servicio Becario</h1>
           </div>
       </div>
    <div class="row" > 
        <div class="col-md-12 text-center">
            <h4> Seguimiento de solicitud  </h4>            
        </div>
    </div>
    <div class="jumbotron">
        <div class="row">
            <div class="col-md-1 col-xs-3    ">
                <label>Periodo</label>
            </div>
            <div class="col-md-3 col-xs-9">
                <asp:DropDownList ID="ddlPeriodo" CssClass="form-control "  runat="server" OnDataBound="ddlPeriodo_DataBound"></asp:DropDownList>
            </div>
            <div class="col-md-1 col-xs-3 ">
                <label>Campus</label>
            </div>
            <div class="col-md-3 col-xs-9">
               
                <table><tr><td>
                     <asp:DropDownList ID="ddlCampus" CssClass="form-control validate[required]" runat="server" OnDataBound="ddlCampus_DataBound"></asp:DropDownList><asp:Label runat="server" ID="lblCampus"></asp:Label></td><td>
                <img src="../images/icono-question.png" width="30" height="30" class="imgHelp" /><div class="Help">Si requieres un reporte multicampus solicítalo a tu DBA</div>
                </td></tr>
                </table> 
                
                <asp:HiddenField runat="server" ID="hdfMostrarId"/>
                  <asp:HiddenField runat="server" ID="hdfActivarRol"/>
            </div>
            <div class="col-md-1 col-xs-3">
                <label>Nómina</label>
            </div>
            <div class="col-md-2  col-xs-9">
                <asp:TextBox ID="txtNomina" runat="server"  AutoPostBack="true" placeholder="L00000000" OnKeyPress = "return paraNomina(event,this);"    CssClass="form-control validate[validate,custom[nomina]] " OnTextChanged="txtNomina_TextChanged" ></asp:TextBox>
            </div>              
        </div>  
        <br />      
        <div class="row">
            <div class=" col-md-1 col-xs-3  ">
                <label>Tipo de solicitud</label>
            </div>
            <div class="col-md-3 col-xs-9">
                <asp:DropDownList ID="ddlTipoSolicitud" CssClass ="form-control"       runat="server" OnDataBound="ddlTipoSolicitud_DataBound" ></asp:DropDownList>
            </div>
            <div class="col-md-1 col-xs-4">
                <asp:CheckBox ID="chkAprovado" CssClass=" checkbox" Text="Aprobado" runat="server" />
            </div>
            <div class="col-md-1 col-xs-3 ">
                <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" CssClass="btn btn-primary" OnClick="btnFiltrar_Click"  OnClientClick="validar();" />
            </div>
            <div class="col-md-1">
                <asp:ImageButton ID="IbtnExportar"  AlternateText="Excel" ToolTip="Descargar a excel" runat="server" ImageUrl="~/images/Excel.jpg" OnClick="IbtnExportar_Click"/>
            </div>
        </div>
    </div>
    <div class="row table-condensed">
        <div class="col-md-12 col-sm-12 col-xs-12">
            <asp:Panel ID="PnlpmostrarDatos" runat="server" CssClass="hscrollbar">
                <asp:GridView ID="GvMostrarDatos" AllowPaging="True" PageSize="5" runat="server" CssClass="table"  AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" OnPageIndexChanging="GvMostrarDatos_PageIndexChanging" OnRowCreated="GvMostrarDatos_RowCreated">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="Periodo" HeaderText="Periodo" SortExpression="Periodo" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" />
                        <asp:BoundField DataField="Campus" HeaderText="Campus" SortExpression="Campus" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" />
                        <asp:BoundField DataField="Solicitud" HeaderText="Solicitud" SortExpression="Solicitud"  HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center"/>
                        <asp:TemplateField HeaderText="Nombre solicitante"  ControlStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButtonDetalles" runat="server" AlternateText='<%# Eval("Nombre Solicitante") %>' Style="cursor: pointer;"  />
                                <cc1:PopupControlExtender ID="PopupDetalles" runat="server"
                                            DynamicServiceMethod="GetDynamicContent"
                                            DynamicContextKey='<%#Eval("Nomina")%>'
                                            TargetControlID="ImageButtonDetalles"
                                            Position="Right"
                                            DynamicControlID="detallesgridPanel"
                                            PopupControlID="detallesgridPanel">
                                </cc1:PopupControlExtender>                                 
                            </ItemTemplate>
                            <ControlStyle CssClass="text-center" />
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Right"  />
                        </asp:TemplateField>                        
                        <asp:BoundField DataField="Ubicacion fisica" HeaderText="Ubicacion fisica" SortExpression="Ubicacion fisica" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center"/>
                        <asp:BoundField DataField="Ubicacion alterna" HeaderText="Ubicacion alterna" SortExpression="Ubicacion alterna" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" />
                        <asp:BoundField DataField="Proyecto" HeaderText="Proyecto" SortExpression="Proyecto" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" />
                        <asp:BoundField DataField="Solicitud estatus" HeaderText="Solicitud estatus" SortExpression="Solicitud estatus" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center"/>
                        <asp:TemplateField HeaderText="Desaprovar" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:CheckBox   ID ="chkAprovar" OnClick="confirmar();"  Enabled='<%# Convert.ToBoolean( Eval("Decide"))%>' runat="server" Checked='<%# Convert.ToBoolean( Eval("Decide"))%>'  ToolTip='<%#Eval("id_MiSolicitud")+"!"+Eval("Periodo")+"!"+Eval("Solicitud")%>' AutoPostBack="true"  OnCheckedChanged="chkAprovar_CheckedChanged"     />                                
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField >
                            <ItemTemplate>
                                <asp:Button ID="btnMandar" runat="server" Text="Ver" CssClass="btn btn-primary"  CommandName='<%#Eval("id_MiSolicitud")+"!"+Eval("Solicitud") %>' OnClick="btnLlevarRegistro_Click"  />
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
                <asp:Panel ID="detallesgridPanel" runat="server"  Style="display: none; background-color: #5099F9; font-size: 11px; border: solid 1px Black;">
                </asp:Panel>
            </asp:Panel>
            
        </div>
    </div>


    

    <br />
    <br />





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
    
    <asp:HiddenField ID="hdfDecide" runat="server" />

     <script type="text/javascript">
         $(document).ready(function () {
             $("#ContentPlaceHolder1_detallesgridPanel").css({
                 "background-color": "#FFF",
                 "border": "1px solid #eee",
                 "box-shadow": "2px 2px 2px #eee",
                 "-webkit-box-shadow": " 2px 2px 5px #eee",
                 "-moz-box-shadow": "2px 2px 5px #eee"
             });
         });

    </script>
</asp:Content>
