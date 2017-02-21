<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="RegistroSafe.aspx.cs" Inherits="ServicioBecario.Vistas.RegistroSafe" %>
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
        function paraMatriculas(e,control) {
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
            if (dato.length <= 8)
            {
                if (letras.indexOf(tecla) == -1 && !tecla_especial) {
                    return false;
                    //No permite toner el numero
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

    </script>

    <asp:Label runat="server" ID="Label1"></asp:Label>
    <div class="row">
           <div class="col-md-12">
                <h1 class="text-center">Servicio Becario</h1>
           </div>
       </div>
    <div class="row">
        <div class="col-md-12 text-center">
            <h4>Mandar calificación a banner safe</h4>              
        </div>
    </div>
    <a class="btn btn-primary btn-sm" id="btnA"><em class="glyphicon glyphicon-plus"></em> Agregar</a>
    <a class="btn btn-primary btn-sm" id="btnB"><em class="glyphicon glyphicon-search"></em> Buscar</a>
    <div class="ContAll">
    <div id="ContAg">
    <div class="jumbotron">
        <div class="row">
            <div class="col-md-1">
               <label>Periodo</label>
            </div>
            <div class="col-md-3">
                <asp:DropDownList runat="server" ID="ddlperiodo" CssClass="form-control validate[required]" OnDataBound="ddlperiodo_DataBound" ></asp:DropDownList>
            </div>
            <div class="col-md-1">
                <label>Campus</label>
            </div>
            <div class="col-md-3">
                <asp:DropDownList ID="ddlCampus" runat="server" CssClass="form-control validate[required]" OnDataBound="ddlCampus_DataBound"></asp:DropDownList>
                
                <asp:Label runat="server" ID="lblCampus"></asp:Label>
                <asp:HiddenField runat="server" ID="hdfMostrarId"/>
                  <asp:HiddenField runat="server" ID="hdfActivarRol"/>
            </div>
            <div class="col-md-1">
                <asp:Button OnClientClick="validar();" CssClass="btn btn-primary"  runat="server" ID="btnMandar" Text="Registrar" OnClick="btnMandar_Click"/>
            </div>
            
        </div>
        <div class="row">
            <div class="col-md-12"><asp:Label runat="server" ID="lblimprimir"></asp:Label></div>
        </div>
    </div>
        </div>
        <div id="ContBu"> 

    <asp:Panel runat="server" ID="pnlgrid" >
        <div class="jumbotron">
            <div class="row">
                <div class="col-md-1">
                    <label>Periodo</label>
                </div>
                <div class="col-md-3">
                    <asp:DropDownList runat="server" ID="PeriodoFiltro" CssClass="form-control" OnDataBound="FiltroPeriodo_DataBound"></asp:DropDownList>
                </div>
                <div class="col-md-1">
                    <label>Campus</label>
                </div>
                <div class="col-md-3">
                    <asp:DropDownList ID="CampusFiltro" runat="server" CssClass="form-control" OnDataBound="FiltroCampus_DataBound"></asp:DropDownList>
                    <asp:Label ID="lblfiltrarCampus" runat="server"></asp:Label>
                </div>

                <div class="col-md-1">
                    <label>Matrícula</label>
                </div>
                <div class="col-md-2">
                    <asp:TextBox runat="server" ID="MatriculaFiltro" placeholder="A00000000" OnKeyPress="return paraMatriculas(event,this);" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-1">
                    <label>Calificación</label>
                </div>
                <div class="col-md-3">
                    <asp:DropDownList ID="CalificacionFiltro" runat="server" CssClass="form-control" OnDataBound="CalificacionFiltro_DataBound"></asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="col-md-1 col-md-offset-6"  >
                    <asp:Button  CssClass="btn btn-primary"  runat="server" ID="Filtro" Text="Filtrar" OnClick="Filtro_Click"/>
                </div>
            </div>
    </div>

        <div class="row table-condensed">
        <div class="col-md-12 col-lg-12 col-xs-12 col-sm-12">
            <asp:Panel ID="pnlgridMuestra" runat="server" CssClass="hscrollbar">
                <asp:GridView ID="Gvdatos" AutoGenerateColumns="false" runat="server" PageSize="15" AllowPaging="true" OnPageIndexChanging="Gvdatos_PageIndexChanging" CssClass="table" CellPadding="4" ForeColor="#333333" GridLines="None">
                 <Columns>
                    <asp:BoundField DataField="Periodo" HeaderText="Periodo" SortExpression="Periodo"></asp:BoundField>
                    <asp:BoundField DataField="Matricula" HeaderText="Matrícula" SortExpression="Matricula"></asp:BoundField>
                    <asp:BoundField DataField="Calificion" HeaderText="Calificación" SortExpression="Calificion"></asp:BoundField>
                     <asp:BoundField DataField="Nomina_envio_banner" HeaderText="Envío" SortExpression="Calificion"></asp:BoundField>
                     <asp:BoundField DataField="Fecha_envio" HeaderText="Fecha de envio" SortExpression="Fecha_envio"></asp:BoundField>
                     <asp:BoundField DataField="Folio" HeaderText="Folio" SortExpression="Folio"></asp:BoundField>
                     <asp:BoundField DataField="estatusEnvio" HeaderText="Estatus envío" SortExpression="Folio"></asp:BoundField>
                     
                </Columns>
                <AlternatingRowStyle BackColor="White" ForeColor="#284775"></AlternatingRowStyle>               
                <EditRowStyle BackColor="#999999"></EditRowStyle>
                <FooterStyle BackColor="#3B83C0" Font-Bold="True" ForeColor="White"></FooterStyle>
                <HeaderStyle BackColor="#3B83C0" Font-Bold="True" ForeColor="White"></HeaderStyle>
                <PagerStyle HorizontalAlign="Center" BackColor="#284775" ForeColor="White"></PagerStyle>
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

    <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>


</asp:Content>
