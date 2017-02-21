<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="Reglamento.aspx.cs" Inherits="ServicioBecario.Vistas.Reglamento" %>
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
        function validar()
        {
            //Esto es para validar campos
            jQuery("form").validationEngine();
        }
        function quitarValidar()
        {
            //Esto es para omitir la validación
            jQuery("form").validationEngine('detach');
        }
       
    </script>
    <div class="row">
        <div class="col-md-12">           
           <h1 class="text-center">Servicio Becario</h1>           
        </div>        
    </div>
    <div class="row">
        <div class="col-md-12 text-center">
            <h4>Liga del reglamento del servico becario</h4>
        </div>
    </div>
    <asp:Panel ID="pnlmodificar" runat="server" Visible="false">
        <div class="jumbotron">
            <div class="row">
                <div class="col-md-offset-2 col-md-2 ">
                    <label>Url del reglamento</label>
                </div>
                <div class="col-md-6">
                    <asp:TextBox ID="txturl" Text="" CssClass="form-control validate[required]" runat="server"></asp:TextBox>
                </div>
                <div>
                    <asp:Button ID="btnModificar" OnClientClick="validar();" runat="server" CssClass="btn btn-primary" Text="Modificar"  OnClick="btnModificar_Click" />
                </div>
            </div>            
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlGrid" Visible="true">
        <div class="row table-condensed">
            <div class="col-md-12 col-sm-12 col-lg-12 col-xs-12">
                <asp:Panel ID="pnlxxx" runat="server" CssClass="hscrollbar">
                    <asp:GridView ID="Gvdatos" runat="server"  CssClass="table" OnSelectedIndexChanged="Gvdatos_SelectedIndexChanged" AutoGenerateColumns="false" Width="100%"  CellPadding="4" ForeColor="#333333" GridLines="None">

                    <AlternatingRowStyle BackColor="White" ForeColor="#284775"></AlternatingRowStyle>
                    <Columns>
                        <asp:BoundField DataField="link" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="Url del reglamento" SortExpression="link"></asp:BoundField>
                        <asp:CommandField ButtonType="Image" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" SelectImageUrl="~/images/update.PNG" ShowSelectButton="True" HeaderText="Modificar" />
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
    </asp:Panel>
        
        
    
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


</asp:Content>
