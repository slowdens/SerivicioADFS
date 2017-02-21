<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="CampusIndicador.aspx.cs" Inherits="ServicioBecario.Vistas.CampusIndicador" %>
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
    <div class="row">
        <div class="col-md-12 text-center">
               <h1 class="text-center">Servicio Becario</h1>        
        </div>        
    </div>
      <div class="row">
        <div class="col-md-12 text-center">
            <h4>Campus indicador</h4>            
        </div>
   </div>
    <div class="row table-condensed">
            <div class="col-md-12 col-sm-12 col-lg-12 col-xs-12">
                <asp:Panel ID="pnlCampus" runat="server" CssClass="hscrollbar">
                    <asp:GridView ID="gvCampus" CssClass="table" Width="100%" AllowPaging="true" DataKeyNames="id_campus" 
                        AutoGenerateColumns="false" runat="server" 
                        PageSize="20" CellPadding="4" ForeColor="#333333" GridLines="None" 
                        OnPageIndexChanging="gvCampus_PageIndexChanging"  OnSelectedIndexChanged="gvCampus_SelectedIndexChanged">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775"></AlternatingRowStyle>
                        <Columns>
                            <asp:BoundField DataField="id_campus" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" HeaderText="id_rol" SortExpression="id_rol" Visible="false" />
                            <asp:BoundField DataField="Campus" HeaderText="Campus" SortExpression="Campus" HeaderStyle-CssClass="text-left" ItemStyle-CssClass="text-left" ></asp:BoundField>
                            <asp:BoundField DataField="Cuenta" HeaderText="Cuenta con alumnos" SortExpression="Cuenta" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" ></asp:BoundField>
                            <asp:BoundField DataField="Asignador" HeaderText="Campus asignador" SortExpression="Asignador" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" ></asp:BoundField>
                            <asp:CommandField ButtonType="Image"  HeaderText="Modificar" SelectImageUrl="~/images/update.PNG" ShowSelectButton="True" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" />
                        </Columns>
                        <EditRowStyle BackColor="#999999"></EditRowStyle>
                        <FooterStyle   BackColor="#3B83C0" Font-Bold="True" ForeColor="White"></FooterStyle>
                        <HeaderStyle  BackColor="#3B83C0" Font-Bold="True" ForeColor="White"></HeaderStyle>
                        <PagerStyle  HorizontalAlign="Center" BackColor="#3B83C0" ForeColor="White"></PagerStyle>
                        <RowStyle  BackColor="#F7F6F3" ForeColor="#333333"></RowStyle>
                        <SelectedRowStyle  BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333"></SelectedRowStyle>
                        <SortedAscendingCellStyle  BackColor="#E9E7E2"></SortedAscendingCellStyle>
                        <SortedAscendingHeaderStyle  BackColor="#506C8C"></SortedAscendingHeaderStyle>
                        <SortedDescendingCellStyle BackColor="#FFFDF8"></SortedDescendingCellStyle>
                        <SortedDescendingHeaderStyle   BackColor="#6F8DAE"></SortedDescendingHeaderStyle>
                    </asp:GridView>
                </asp:Panel>
                               
            </div>
        </div>
        <asp:Panel runat="server" ID="pnlModificar" Visible="false">
            <div class="jumbotron">
                <div class="row">
                    <div class="col-md-offset-5 col-md-1">
                        <label>Campus</label>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblCampus" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-2">
                        <label>Cuenta con alumnos</label>
                    </div>
                    <div class="col-md-2">
                        <asp:DropDownList runat="server" ID="ddlCuenta" CssClass="form-control validate[required]" AutoPostBack="true" OnSelectedIndexChanged="ddlCuenta_SelectedIndexChanged">
                            <asp:ListItem Value="">-- Seleccione --</asp:ListItem>
                            <asp:ListItem Value="Si">Si</asp:ListItem>
                            <asp:ListItem Value="No">No</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <asp:Panel runat="server" ID="pnlGuardarCampus" Visible="false">
                        <div class="col-md-3">
                            <label>Campus que provee alumnos</label>
                        </div>
                        <div class="col-md-2">
                            <asp:DropDownList runat="server" CssClass="form-control" ID="ddlCampus"></asp:DropDownList>
                        </div>
                        <div class="col-md-1">
                            <asp:Button runat="server" ID="btnAsignar" Text="Asignar" CssClass="btn btn-primary" OnClick="btnAsignar_Click" />
                        </div>
                        <div class="col-md-1">
                            <asp:Button runat="server" ID="btnCancelar" Text="Cancelar" CssClass="btn btn-primary" OnClick="btnCancelar_Click" />
                        </div>
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
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
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


    <asp:HiddenField ID="hdf_id_campus" runat="server" />


</asp:Content>
