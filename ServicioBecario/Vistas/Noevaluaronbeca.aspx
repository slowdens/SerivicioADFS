<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="Noevaluaronbeca.aspx.cs" Inherits="ServicioBecario.Vistas.Noevaluaronbeca" %>
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
           <div class="col-md-12">
                <h1 class="text-center">Servicio Becario</h1>
           </div>
       </div>  
    <div class="row">
        <div class="col-md-12 text-center">
             <h4>No han evaluado solicitantes</h4>            
        </div>
    </div>
    <div class="jumbotron">
        <div class="row">
            <div class="col-md-12 text-center">
                <fieldset>
                    <legend class="text-center">
                        <h4>Filtro</h4>
                    </legend>
                </fieldset>
            </div>
        </div>
        <div class="row">
            <div class="col-md-1">
                <label>Periodo</label>
            </div>
            <div class="col-md-3">
                <asp:DropDownList ID="ddlFiltrarPeriodo" runat="server"  CssClass="form-control" OnDataBound="ddlFiltrarPeriodo_DataBound" ></asp:DropDownList>
                
            </div>
            <div class="col-md-1">
                <label> Nivel</label>
            </div>
            <div class="col-md-3">
                <asp:DropDownList ID="ddlNivel"  runat="server"  CssClass="form-control" OnDataBound="ddlNivel_DataBound" ></asp:DropDownList>
            </div>
            <div  class="col-md-1">
                <label>Campus</label>
            </div>
            <div class="col-md-3">
                

                 <table><tr><td>
                <img src="../images/icono-question.png" width="30" height="30" class="imgHelp" /><div class="Help">Si requieres un reporte multicampus solicítalo a tu DBA</div>
                </td><td>
                   <asp:DropDownList  ID="ddlFiltrarCampus" runat="server" CssClass="form-control validate[required]" OnDataBound="ddlFiltrarCampus_DataBound" ></asp:DropDownList>
                <asp:Label runat="server" ID="lblCampus"></asp:Label></td></tr>
                </table>
                <asp:HiddenField runat="server" ID="hdfMostrarId"/>
                <asp:HiddenField runat="server" ID="hdfActivarRol"/>
            </div>            
        </div>
        <br />
        <div class="row">
            <div class="col-md-offset-5 col-md-1">
                <asp:Button  ID="btnFiltrar" runat="server" CssClass="btn btn-primary" Text="Filtrar" OnClick="btnFiltrar_Click" OnClientClick="validar();"/>
            </div>
            <div class="col-md-1">
                <asp:ImageButton runat="server" ID="btnimgDescargar" ImageUrl="~/images/Excel.jpg" ToolTip="Descargar" OnClick="btnimgDescargar_Click" />
            </div>
        </div>
    </div>
    <div class="row table-condensed">
        <div class="col-md-12 col-lg-12 col-xs-12 col-sm-12" >
            <asp:Panel ID="pnlGrid" runat="server" CssClass="hscrollbar">
                
                
                        <asp:GridView ID="gvDatos" CssClass="table" AllowPaging="true" PageSize="20"  runat="server" AutoGenerateColumns="false" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None" OnPageIndexChanging="gvDatos_PageIndexChanging">
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField DataField="matricula" HeaderText="matricula" SortExpression="matricula" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                <asp:BoundField DataField="nombrecompleto" HeaderText="Nombre completo" SortExpression="nombrecompleto" ItemStyle-CssClass="text-left" HeaderStyle-CssClass="text-left" ></asp:BoundField>
                                <asp:BoundField DataField="periodo" HeaderText="Periodo" SortExpression="Periodo" ItemStyle-CssClass="text-left" HeaderStyle-CssClass="text-left" ></asp:BoundField>
                                <asp:BoundField DataField="nivel" HeaderText="Nivel" SortExpression="Nivel" ItemStyle-CssClass="text-left" HeaderStyle-CssClass="text-left" ></asp:BoundField>
                                
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

       
</asp:Content>
