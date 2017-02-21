<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="CalificacionSolicitante.aspx.cs" Inherits="ServicioBecario.Vistas.CalificacionSolicitante" %>
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
            jQuery("form").validationEngine();
            $('#<%=hdfvalidar.ClientID%>').val(false);
            var puntaje = $('#<%=txtcalificacion.ClientID%>').val();
            var nomina = $('#<%=txtNomina.ClientID%>').val();
            var expresionNomina = /^L{0,1}[0-9]+$/;
            var expresionPuntaje = /^[0-9]*.{0,1}[0-9]+$/;
            if(nomina=="" && puntaje=="")
            {
                $('#<%=hdfvalidar.ClientID%>').val(true);
            }
            if(nomina!="" && puntaje=="" && expresionNomina.test(nomina))
            {
                $('#<%=hdfvalidar.ClientID%>').val(true);
            }
            if(nomina=="" && puntaje !="" && expresionPuntaje.test(puntaje))
            {
                $('#<%=hdfvalidar.ClientID%>').val(true);
            }
            if(nomina!="" && puntaje!="" && expresionNomina.test(nomina) && expresionPuntaje.test(puntaje))
            {
                $('#<%=hdfvalidar.ClientID%>').val(true);
            }            
        }
        function paraCalificaciones(e)
        {
            key = e.keyCode || e.which;
            tecla = String.fromCharCode(key).toLowerCase();
            letras = ".0123456789";
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
           <div class="col-md-12">
                <h1 class="text-center">Servicio Becario</h1>
           </div>
       </div>
    <div class="row">
        <div class="col-md-12 text-center">
            <h4>Calificación de solicitante de becarios</h4>
        </div>
    </div>
    <div class="jumbotron">
        <div class="row">
            <div class="col-md-12">
                <fieldset>
                    <legend class="text-center">
                        <h4> Filtros</h4>
                    </legend>
                </fieldset>
            </div>
        </div>
        <div class="row">
            <div class="col-md-1">
                <label>Nómina</label>
            </div>
            <div class="col-md-2">
                <asp:TextBox CssClass="form-control validate[validate,custom[nomina]]" placeholder="L00000000" runat="server" ID="txtNomina" OnKeyPress = "return paraNomina(event);" OnTextChanged="txtNomina_TextChanged" AutoPostBack="true"></asp:TextBox>
            </div>
            <div class="col-md-2">
                <asp:Label runat="server" ID="lblNombreSolicitante" Text=""></asp:Label>
            </div>
            <div class="col-md-1">
                <label>
                    Calificación
                </label>
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtcalificacion" placeholder="0"   CssClass="form-control  validate[validate,custom[nomina]]  " OnKeyPress = "return paraCalificaciones(event);" runat="server" Text=""></asp:TextBox>
            </div>
            <div class="col-md-1">
                <label>Periodo</label>
            </div>
            <div class="col-md-2">
                <asp:DropDownList ID="ddlPeriodo" CssClass="form-control" runat="server" OnDataBound="ddlPeriodo_DataBound"></asp:DropDownList>
            </div>
            <div class="col-md-1">
                <asp:Button ID="btnFiltrar" runat="server" CssClass="btn btn-primary" Text="Filtrar" OnClientClick="validar();" OnClick="btnFiltrar_Click" />
            </div>
        </div>
    </div>
    <div class="row table-condensed ">
        <div class="col-md-12 col-lg-12 col-xs-12 col-sm-12 ">
            <asp:Panel ID="Panel2" runat="server" CssClass="hscrollbar">
                <asp:UpdatePanel ID="updateGrid" runat="server">
                    <ContentTemplate>
                        <asp:HiddenField ID="hdfvalidar" runat="server" />
                        <asp:GridView ID="GvDatos" CssClass="table" AllowPaging="True" PageSize="10" Width="100%" AutoGenerateColumns="false" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" OnPageIndexChanging="GvDatos_PageIndexChanging">
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField DataField="Nomina" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" HeaderText="Nómina" SortExpression="Nomina"></asp:BoundField>
                                <asp:BoundField DataField="Nombre" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" HeaderText="Nombre" SortExpression="Nombre"></asp:BoundField>
                                <asp:BoundField DataField="Empleado_puntuaje" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" HeaderText="Puntuaje" SortExpression="Empleado_puntuaje"></asp:BoundField>
                                <asp:BoundField DataField="Periodo" HeaderText="Periodo" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" SortExpression="Periodo"></asp:BoundField>
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
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnFiltrar" />
                    </Triggers>
                </asp:UpdatePanel>
            </asp:Panel>
            
        </div>
    </div>



    <asp:UpdatePanel ID="UpdateModal" runat="server">
        <ContentTemplate>
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
            </ContentTemplate>
    </asp:UpdatePanel>



    
</asp:Content>
