<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="CambioSolicitanteDdA.aspx.cs" Inherits="ServicioBecario.Vistas.CambioSolicitanteDdA" %>
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
            //Esto es para validar campos
            jQuery("form").validationEngine();
        }


        function confirmar() {
            var opt = confirm("Desea asignar al becario");
            $('#<%=hdfDecide.ClientID%>').val(opt);

        }
    </script>
    <div class="row">
        <div class="col-md-12 text-center">
                 <h1 class="text-center">Servicio Becario</h1>

        </div>
    </div>

    <div class="row">
        <div class="col-md-12 text-center">
            <h4>Cambio de Solicitante de becarios</h4>
        </div>
    </div>
    <div class="jumbotron">
        <div class="row">
            <div class="col-md-12">
                <fieldset>
                    <legend class="text-center">
                        <h4> Datos de becario</h4>
                    </legend>
                </fieldset>
            </div>
        </div>
       
        <div class="row">
            <div class="col-md-1">
                <label>
                    Periodo
                </label>
            </div>
            <div class="col-md-2">
                <asp:DropDownList runat="server" ID="ddlPeriodo" CssClass="form-control validate[required] " OnDataBound="ddlPeriodo_DataBound">
                </asp:DropDownList>
            </div>
            <div class="col-md-1">
                <label>Matrícula</label>
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtMatricula" runat="server" CssClass="form-control validate[required]" OnTextChanged="txtMatricula_TextChanged" AutoPostBack="true" ></asp:TextBox>
            </div>
            <div class="col-md-1">
                <label>Nombre </label>
            </div>
            <div class="col-md-3">
                <asp:Label ID="lblNombre" runat="server"></asp:Label>
            </div>
            <div class="col-md-1">
                <asp:Button runat="server" ID="btnVerasignacion" CssClass="btn btn-primary" Text="Ver asignacion" OnClientClick="validar();"  OnClick="btnVerasignacion_Click" />
            </div>
        </div>
        
    </div>
    <asp:Panel ID="pnlAsignacion" runat="server" Visible="false">
        <div class="row">
            <div class="col-md-12">
               <fieldset >
                   <legend class="text-center" >
                       <h4><label>Datos de nómina responsable</label></h4>
                   </legend>
               </fieldset>
            </div>
        </div>
        <div class="row">
            <%--DATOS DE LAS ASIGNACIONES--%>
            <div class="col-md-1">
                <label>
                    Nómina
                </label>
            </div>
            <div class="col-md-2">
                <asp:Label runat="server" ID="lblNomina" Text=""></asp:Label>
                
            </div>
            
             <div class="col-md-1">
                 <label>Nombre</label>
             </div>
            <div class="col-md-3">
                <asp:label runat="server" ID="lblNombreSolicitante" ></asp:label>
            </div>
            <div class="col-md-1">
                <label>Puesto</label>
            </div>
           <div class="col-md-2">
               <asp:Label runat="server" ID="lblPuesto" ></asp:Label>
           </div>
        </div>
        <div class="row">
            <div class="col-md-1">
                <label>
                    Campus
                </label>
            </div>
            <div class="col-md-2">
                <asp:Label runat="server" ID="lblCampus"></asp:Label>
            </div>
            <div class="col-md-1">
                <label>
                    Ubicación
                </label>
            </div>
            <div class="col-md-2">
                <asp:Label runat="server" ID="lblubucacion"></asp:Label>
            </div>
            <div class="col-md-1">
                <label>
                    Extención
                </label>
            </div>
            <div class="col-md-1">
                <asp:Label runat="server" ID="lblExtencion"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <fieldset>
                    <legend class="text-center">
                        <h4>
                            <label>
                                Motivos de desagnación
                            </label>
                        </h4>
                    </legend>
                </fieldset>
            </div>
        </div>
        <div class="row">
            <div class="col-md-1">
                <label>
                    Justificación
                </label>
            </div>
            <div class="col-md-9">
               <asp:TextBox ID="txtJustificacion" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-md-2">
                <asp:Button ID="btnDesAsignacion" runat="server" CssClass="btn btn-primary" Text="Des asignación" OnClick="btnDesAsignacion_Click"/>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlGrid">
        <div class="row table-condensed">
            <div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">
                <asp:Panel runat="server" ID="pnlxxx" CssClass="hscrollbar">
                    <asp:GridView ID="gvDatos" AllowPaging="True" PageSize="10" runat="server" AutoGenerateColumns="false" CssClass="table" CellPadding="4" ForeColor="#333333" GridLines="None" OnPageIndexChanging="gvDatos_PageIndexChanging">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775"></AlternatingRowStyle>
                        <Columns>
                            <asp:BoundField DataField="Periodo" HeaderText="Periodo" SortExpression="Periodo" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"></asp:BoundField>
                            <asp:BoundField DataField="Nomina" HeaderText="Nómina" SortExpression="Nomina" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" ></asp:BoundField>
                            <asp:BoundField DataField="NombreSolicitante" HeaderText="NombreSolicitante" SortExpression="NombreSolicitante" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" ></asp:BoundField>
                            <asp:BoundField DataField="Solicitud" HeaderText="Solicitud" SortExpression="Solicitud" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" ></asp:BoundField>
                            <asp:TemplateField HeaderText="Desasociar">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkselecioname" OnClick="confirmar();" runat="server" ToolTip='<%#Eval("id_MiSolicitud")%>' OnCheckedChanged="chkselecioname_CheckedChanged" AutoPostBack="true" />
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
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE"></SortedDescendingHeaderStyle>                    </asp:GridView>
                </asp:Panel>
            </div>
        </div>
    </asp:Panel>

    <asp:HiddenField  runat="server" ID="hdfDecide"/>


        
    
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









</asp:Content>
