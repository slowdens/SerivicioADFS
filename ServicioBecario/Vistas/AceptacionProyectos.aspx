<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="AceptacionProyectos.aspx.cs" Inherits="ServicioBecario.Vistas.AceptacionProyectos" %>
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
        function confirmacion()
        {
            var decide = confirm("¿Estas seguro en autorizar el proyecto?");
            $('#<%=hdfDesion.ClientID%>').val(decide);

        }


    </script>
    <div class="row">
           <div class="col-md-12">
                <h1 class="text-center">Servicio Becario</h1>
           </div>
       </div>
    <div class="row">
        <div class="col-md-12 text-center">
            <h4>Aceptación por proyectos</h4>
        </div>
     </div>
    <div class="jumbotron">
        <div class="row">
            <asp:HiddenField ID="hdf_id_campus" runat="server" />
            <asp:HiddenField ID="hdfRol" runat="server" />
            <div class="col-md-1">
                <label>Campus</label>
            </div>
            <div class="col-md-2">
                <asp:Panel ID="pnllabel" runat="server" Visible="false">
                    <asp:Label runat="server" ID="lblCampus"></asp:Label>
                </asp:Panel>
                <asp:Panel ID="pnlDrodownlis" runat="server" Visible="false">
                    <asp:DropDownList runat="server" ID="ddlCampus" CssClass="form-control" OnDataBound="ddlCampus_DataBound"></asp:DropDownList>
                </asp:Panel>
            </div>
            <div class="col-md-1">
                <label>Periodo</label>
            </div>
            <div class="col-md-3">
                <asp:DropDownList runat="server" ID="ddlPeriodo" CssClass="form-control" OnDataBound="ddlPeriodo_DataBound"></asp:DropDownList>
            </div>
            <div class="col-md-2">
                <asp:Button runat="server" ID="btnFiltrar" Text="Filtrar" CssClass="btn btn-primary" OnClick="btnFiltrar_Click" />
            <br /><br /><br /><br /><br /><br /></div>
        </div>
    </div>
    <div class="row table-condensed">
        <div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">
            <asp:UpdatePanel ID="updateGrid" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="Panel2" runat="server" CssClass="hscrollbar">
                        <asp:HiddenField ID="hdfDesion" runat="server"  />

                        <asp:GridView ID="gviDatos" 
                            CssClass="table" runat="server" Width="100%" AutoGenerateColumns="false" CellPadding="4" OnPageIndexChanging="gviDatos_PageIndexChanging" AllowPaging="true" PageSize="20"    ForeColor="#333333" GridLines="None">
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField Visible="false" DataField="id_proyecto" HeaderText="id_proyecto" SortExpression="id_proyecto" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" ></asp:BoundField>
                                <asp:BoundField DataField="Proyecto" HeaderText="Proyecto" SortExpression="Proyecto" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                <asp:BoundField DataField="Propuso" HeaderText="Propuso" SortExpression="Propuso" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                <asp:BoundField DataField="Periodo" HeaderText="Periodo" SortExpression="Periodo" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                <asp:BoundField DataField="Campus" HeaderText="Campus" SortExpression="Campus" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" ></asp:BoundField>
                                <asp:TemplateField HeaderText="Aprobar" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkaprovar" OnClick="confirmacion();" runat="server" Checked='<%# Convert.ToBoolean( Eval("Dato"))%>' Enabled='<%# Convert.ToBoolean( Eval("desactivado"))%>' OnCheckedChanged="chkaprovar_CheckedChanged" AutoPostBack="true" ToolTip='<%#Eval("id_proyecto")%>' />
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
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger  ControlID="btnFiltrar" />
                </Triggers>
            </asp:UpdatePanel>
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
