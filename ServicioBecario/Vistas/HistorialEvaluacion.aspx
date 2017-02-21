<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="HistorialEvaluacion.aspx.cs" Inherits="ServicioBecario.Vistas.HistorialEvaluacion" Culture="Auto" UICulture="Auto" %>
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
        function validarDatos()
        {
            jQuery("form").validationEngine();
            var nomina = $('#<%=txtNomina.ClientID%>').val();
            var matricula = $('#<%=txtmatricula.ClientID%>').val();
            $('#<%=hdfDecide.ClientID%>').val(false);
            var expresionNomina = /^L{0,1}[0-9]+$/;
            var expresionMatricula = /^A{0,1}[0-9]+$/;

            if(nomina =="" && matricula=="")
            {
                $('#<%=hdfDecide.ClientID%>').val(true);
            }
            if(nomina !="" && matricula=="" && expresionNomina.test(nomina) )
            {
                $('#<%=hdfDecide.ClientID%>').val(true);
            }
            if(nomina=="" && matricula !="" && expresionMatricula.test(matricula))
            {
                $('#<%=hdfDecide.ClientID%>').val(true);
            }
            if(nomina!="" && matricula !="" && expresionMatricula.test(matricula) && expresionNomina.test(nomina))
            {
                $('#<%=hdfDecide.ClientID%>').val(true);
            }

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

            tecla_especial = false
            for (var i in especiales) {
                if (key == especiales[i]) {
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
    </script>
     <div class="row">
           <div class="col-md-12">
                <h1 class="text-center">Servicio Becario</h1>
           </div>
       </div>
     <div class="row">
        <div class="col-md-12 text-center">
            <h4>Historial de calificaciones</h4>
        </div>
    </div>
    <div class="jumbotron">
        <div class="row">
            <div class="col-md-12 text-center">
                <fieldset>
                    <legend class="text-center">
                        <h4>Filtros</h4>
                    </legend>
                </fieldset>
            </div>
        </div>
        <div class="row">
            <div class=" col-md-offset-3 col-md-1">
                <label>Periodo</label>
            </div>
            <div class="col-md-3">
                <asp:DropDownList CssClass="form-control" ID="ddlPeriodo" runat="server" OnDataBound="ddlPeriodo_DataBound"></asp:DropDownList>
            </div>
            <div class="col-md-1">
                <label>Nómina</label>
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtNomina"   AutoPostBack="true" placeholder="L00000000"    OnKeyPress = "return paraNomina(event,this);" CssClass="form-control validate[validate,custom[nomina]]" runat="server" Text="" OnTextChanged="txtNomina_TextChanged"></asp:TextBox>
            </div>
            </div>
        <div class="row">
            <div class="col-md-offset-3 col-md-1">
                <label>Matrícula</label>
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtmatricula"  AutoPostBack="true" placeholder="A00000000" OnKeyPress="return paraMatriculas(event,this);"  CssClass="form-control validate[validate,custom[matricula]]" runat="server" Text="" OnTextChanged="txtmatricula_TextChanged"></asp:TextBox>
            </div>
            <div class="col-md-offset-1 col-md-1">
                <label>Fecha </label>
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtfecha" CssClass="form-control" placeholder="dd/mm/aaaa"  OnKeyPress = "return formatofecha(event,this);" runat="server"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtfecha" />
            </div>            
        </div>
        <br />
        <div class="row">
            <div class="col-md-offset-6 col-md-1">
                <asp:Button ID="btnFiltrar" runat="server" CssClass="btn btn-primary" Text="Filtrar" OnClick="btnFiltrar_Click"   OnClientClick="validarDatos();" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">   
            <asp:Panel ID="pnlVisible" runat="server">
                <asp:UpdatePanel ID="UpdatePanelGrid" runat="server" >
                    <ContentTemplate>
                        <asp:HiddenField ID="hdfDecide" runat="server" />
                        <asp:GridView ID="gvdastos" runat="server" CssClass="table" Width="100%" AutoGenerateColumns="false" CellPadding="4" ForeColor="#333333" GridLines="None" OnPageIndexChanging="gvdastos_PageIndexChanging" PageIndex="10" AllowPaging="true">
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField DataField="Modifico" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" HeaderText="Modifico" SortExpression="Modifico"></asp:BoundField>
                                <asp:BoundField DataField="Periodo"  HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center"  HeaderText="Periodo" SortExpression="Periodo"></asp:BoundField>
                                <asp:BoundField DataField="Matricula" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" HeaderText="Matrícula" SortExpression="Matricula"></asp:BoundField>
                                <asp:BoundField DataField="EvaluacionAnterior" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" HeaderText="Evaluacion anterior" SortExpression="EvaluacionAnterior"></asp:BoundField>
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" SortExpression="Fecha"></asp:BoundField>
                                <asp:BoundField DataField="Calificacion_nueva" HeaderText="Calificacion_nueva" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" SortExpression="Calificacion_nueva"></asp:BoundField>
                            </Columns>
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
                    </ContentTemplate>
                    <Triggers>
                          <asp:AsyncPostBackTrigger  ControlID="btnFiltrar" />    
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
