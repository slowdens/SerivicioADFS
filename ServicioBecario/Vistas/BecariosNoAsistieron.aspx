<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="BecariosNoAsistieron.aspx.cs" Inherits="ServicioBecario.Vistas.BecariosNoAsistieron" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link rel="stylesheet" href="../scripts/footable.bootstrap.min.css" type="text/css" /> 
    <link rel="stylesheet"  href="../scripts/Enginee/css/validationEngine.jquery.css" type="text/css"/> 
    <link rel="stylesheet" href="../scripts/Estiloss.css" type="text/css" />   
    <%--<script type="text/javascript" src="../scripts/Enginee/js/jquery-1.8.2.min.js"  charset="utf-8"></script>--%>
    <script type="text/javascript" src="../scripts/Enginee/js/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../scripts/Enginee/js/jquery.validationEngine.js" charset="utf-8"></script>
     
    <script>
        function confirmar() {
            var opt = confirm("Desea des asignar al becario");
            $('#<%=hdfDecide.ClientID%>').val(opt);

        }

        function paraNomina(e,control) {
            key = e.keyCode || e.which;
            tecla = String.fromCharCode(key).toLowerCase();
            letras = "l0123456789";
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

   
    <div class="row">
           <div class="col-md-12">
                <h1 class="text-center">Servicio Becario</h1>
           </div>
    </div>
    <div class="row">
        <asp:HiddenField runat="server"  ID="hdfRol"/>
        <div class="col-md-12 text-center">
            <h4>Becarios  que no asistieron</h4>      
        </div>
    </div>
    <div class="jumbotron">
        <div class="row">
            <div class="col-md-12">
                <fieldset>
                    <legend class="text-center">
                        <h4>Filtros</h4>
                    </legend>
                </fieldset>
            </div>
        </div>
        <div class="row">
            <div class="col-md-1">
                <label>Periodo</label>
            </div>
            <div class="col-md-3">
                <asp:DropDownList CssClass="form-control" ID="ddlPeriodo" runat="server" OnDataBound="ddlPeriodo_DataBound" ></asp:DropDownList>
            </div>
            <div class="col-md-1">
                <label>Campus</label>
            </div>
            <div class="col-md-2">
                <asp:Panel ID="pnlDdlis" runat="server" Visible="false" >
                    <asp:DropDownList CssClass="form-control" ID="ddlCampus" runat="server" OnDataBound="ddlCampus_DataBound"  ></asp:DropDownList>
                </asp:Panel>
                <asp:Panel ID="pnlconjunto" runat="server" Visible="false" >
                    <asp:Label runat="server" ID="lblcampus"></asp:Label>
                    <asp:HiddenField ID="hdfId_campus" runat="server"/>
                </asp:Panel>
            </div>
            <div class="col-md-1">
                <label>Nómina</label>
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtNomina" AutoPostBack="true" placeholder="L00000000" OnKeyPress = "return paraNomina(event,this);" runat="server" CssClass="form-control" OnTextChanged="txtNomina_TextChanged"></asp:TextBox>
            </div>
            <div class="col-md-1">
                <asp:Button ID="btnFiltrar"   runat="server" CssClass="btn btn-primary" Text="Filtrar" OnClick="btnFiltrar_Click"/>
            <br /><br /><br /><br /></div>
        </div>
    </div>
    <div class="row table-condensed">
        <div class="col-md-12 col-sm-12 col-lg-12 col-xs-12">
            <asp:Panel runat="server" ID="pnlGrid" CssClass="hscrollbar">
                <asp:GridView ID="gvDatos" AutoGenerateColumns="false" runat="server" AllowPaging="true" PageSize="10" CssClass="table" CellPadding="4" ForeColor="#333333" GridLines="None" OnPageIndexChanging="gvDatos_PageIndexChanging">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775"></AlternatingRowStyle>
                    <Columns>
                        <asp:BoundField ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" DataField="Nomina" HeaderText="Nómina" SortExpression="Nomina" />
                        <asp:BoundField ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" DataField="NombreSolicitante" HeaderText="Solicitante" SortExpression="NombreSolicitante"></asp:BoundField>
                        <asp:BoundField ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" DataField="Matricula" HeaderText="Matrícula" SortExpression="Matricula"></asp:BoundField>
                        <asp:BoundField ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" DataField="NombreBecario" HeaderText="Becario" SortExpression="NombreBecario"></asp:BoundField>
                        <asp:BoundField ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" DataField="Nombre" HeaderText="Periodo" SortExpression="Nombre"></asp:BoundField>
                        <asp:TemplateField HeaderText="Des asignar">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkselecioname" OnClick="confirmar();" runat="server" ToolTip='<%#Eval("id_consecutivo")%>' OnCheckedChanged="chkselecioname_CheckedChanged" AutoPostBack="true"/>
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
                <asp:HiddenField ID="hdfDecide" runat="server" />
            </asp:Panel>
        </div>
    </div>


    
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
