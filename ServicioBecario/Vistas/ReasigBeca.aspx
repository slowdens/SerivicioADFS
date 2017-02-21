<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="ReasigBeca.aspx.cs" Inherits="ServicioBecario.Vistas.ReasigBeca" %>
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
       function validar() {
           jQuery("form").validationEngine();
       }
       $(document).ready(function () {
           $(".imgHelp").mouseover(function () {
               $(".Help").css({ "display": "block" });
           });
           $(".imgHelp").mouseout(function () {
               $(".Help").css({ "display": "none" });
           });
       });
    </script>
    
     <div class="row">
        <div class="col-md-12 text-center" >
            <h1>Servicio Becario</h1>
           
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 text-center" >
            <h4>Reportes de becarios reasignados</h4>
           
        </div>
    </div>
    <div class="jumbotron">
        <div class="row">
            <div class="col-md-12 text-center">
                <fieldset >
                    <legend class="text-center"> <h4>Filtros</h4> </legend>
                </fieldset>
            </div>
        </div>
        <div class="row">
            <div class="col-md-1">
                <label>Periodo</label>
            </div>
            <div class="col-md-3">
                <asp:DropDownList ID="ddlPeriodo" CssClass="form-control" runat="server" OnDataBound="ddlPeriodo_DataBound"></asp:DropDownList>
            </div>
            <div class="col-md-1">
                <label>Nivel</label>
            </div>
            <div class="col-md-3">
                <asp:DropDownList ID="ddlNivel" CssClass="form-control" runat="server" OnDataBound="ddlNivel_DataBound"></asp:DropDownList>
            </div>
            <div class="col-md-1">
                <label>Campus</label>
            </div>
            <div class="col-md-3">
                

                 <table><tr><td>
                <img src="../images/icono-question.png" width="30" height="30" class="imgHelp" /><div class="Help">Si requieres un reporte multicampus solicítalo a tu DBA</div>
                </td><td>
                  <asp:DropDownList  ID="ddlCampus" runat="server" CssClass="form-control validate[required]" OnDataBound="ddlCampus_DataBound"></asp:DropDownList>
                <asp:Label runat="server" ID="lblCampus"></asp:Label></td></tr>
                </table>
                <asp:HiddenField runat="server" ID="hdfMostrarId"/>
                <asp:HiddenField runat="server" ID="hdfActivarRol"/>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-md-offset-4 col-md-1">
                <asp:Button runat="server" ID="btnFiltrar" CssClass="btn btn-primary" Text="Filtro" OnClick="btnFiltrar_Click" OnClientClick="validar();"/>
            </div>
            <div class="col-md-1">
                <asp:ImageButton  runat="server" ID="btnImageDescargar"  ImageUrl="~/images/Excel.jpg" OnClick="btnImageDescargar_Click" />
            </div>
        </div>
    </div>
    <div class="row table-condensed">
        <div class="col-md-12 col-sm-12 col-lg-12 col-xs-12">
            <asp:Panel ID="pnlGrid" runat="server" CssClass="hscrollbar">
                <asp:UpdatePanel ID="updatePanelGrid" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvDatos" AllowPaging="true" PageSize="10" runat="server" CssClass="table" AutoGenerateColumns="False"  CellPadding="4" ForeColor="#333333" GridLines="None" OnPageIndexChanging="gvDatos_PageIndexChanging">
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="Matricula" HeaderText="Matricula" SortExpression="Matrícula" ItemStyle-CssClass="text-left"  HeaderStyle-CssClass="text-left"></asp:BoundField>
                                <asp:BoundField DataField="Becario" HeaderText="Becario" SortExpression="Becario" ItemStyle-CssClass="text-left"  HeaderStyle-CssClass="text-left" ></asp:BoundField>
                                <asp:BoundField DataField="Deasocio" HeaderText="Deasocio" SortExpression="Deasocio" ItemStyle-CssClass="text-left"  HeaderStyle-CssClass="text-left" ></asp:BoundField>
                                <asp:BoundField DataField="Periodo" HeaderText="Periodo" SortExpression="Periodo" ItemStyle-CssClass="text-left"  HeaderStyle-CssClass="text-left" ></asp:BoundField>
                                <asp:BoundField DataField="Campus" HeaderText="Campus" SortExpression="Campus" ItemStyle-CssClass="text-left"  HeaderStyle-CssClass="text-left" ></asp:BoundField>
                                <asp:BoundField DataField="Nivel" HeaderText="Nivel académico" SortExpression="Nivel_academico" ItemStyle-CssClass="text-left"  HeaderStyle-CssClass="text-left" ></asp:BoundField>
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
                                    <button type="button" id="cancel" class="btn" style="background-color:#1D4860;color:#F6F6F6" data-dismiss="modal">Close</button>        
                                  </div>
                                </div><!-- /.modal-content -->
                              </div><!-- /.modal-dialog -->
            
        </asp:Panel>
 <%--   Fin de la face del modal--%>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
