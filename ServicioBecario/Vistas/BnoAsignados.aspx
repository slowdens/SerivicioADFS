<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="BnoAsignados.aspx.cs" Inherits="ServicioBecario.Vistas.BnoAsignados" %>
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
  
        function quitarValidar() {
            //Esto es para omitir la validación
            jQuery("form").validationEngine('detach');
        }
    </script>
    <div class="row">        
        <div class="col-md-12 text-center">
            <h1>Servicio Becario</h1>            
        </div>        
    </div>
    <div class="row">
        <div class="col-md-12 text-center " >
            <h4>Becarios no asignados </h4>    
        </div>
    </div>
    <div class="jumbotron">
        <div class="row ">
        <div class="col-md-1">
            <label>Campus</label>
        </div>
        <div class="col-md-3">
            

            <table><tr><td>
                     <asp:DropDownList ID="ddlCampus" runat="server" CssClass="form-control validate[required]" OnDataBound="ddlCampus_DataBound" ></asp:DropDownList>
            <asp:Label runat="server" ID="lblCampus"></asp:Label></td><td>
                <img src="../images/icono-question.png" width="30" height="30" class="imgHelp" /><div class="Help">Si requieres un reporte multicampus solicítalo a tu DBA</div>
                </td></tr>
                </table>
                <asp:HiddenField runat="server" ID="hdfMostrarId"/>
                  <asp:HiddenField runat="server" ID="hdfActivarRol"/>
        </div>
        <div class="col-md-1">
            <label>Periodo</label>
        </div>
        <div class="col-md-3">
            <asp:DropDownList ID="ddlPeriodo" runat="server" CssClass="form-control validate[required]" OnDataBound="ddlPeriodo_DataBound" ></asp:DropDownList>
        </div>        
        <div class="col-md-1">
            <asp:Button OnClientClick="validar();"  runat="server" ID="btnInformacion" CssClass="btn btn-primary" Text="Mostrar" OnClick="btnInformacion_Click" />
        </div>
        <div class="col-md-1">
            <asp:ImageButton runat="server"  ID="IbtDescargar" ImageUrl="~/images/Excel.jpg" ToolTip="Descargar reporte" OnClick="IbtDescargar_Click" />
        <br /><br /><br /><br /><br /><br /></div>
    </div>
    </div>
    
    <div class="row table-condensed">
       <div class="col-md-12 col-xs-12 col-lg-12 col-sm-12">
           <asp:Panel runat="server" ID="pnlInformativo" CssClass="hscrollbar"> 

               <asp:GridView ID="gvDatos" OnPageIndexChanging="gvDatos_PageIndexChanging" AllowPaging="true" AutoGenerateColumns="false" PageSize="20" runat="server" CssClass="table" CellPadding="4" ForeColor="#333333" GridLines="None">
               <AlternatingRowStyle BackColor="White" ForeColor="#284775"></AlternatingRowStyle>
                   <Columns>
                       <asp:BoundField DataField="Matricula" HeaderText="Matrícula" SortExpression="Matricula"></asp:BoundField>
                       <asp:BoundField DataField="NombreCompleto" HeaderText="Nombre completo" SortExpression="NombreCompleto"></asp:BoundField>
                       <asp:BoundField DataField="Campus" HeaderText="Campus" SortExpression="Campus"></asp:BoundField>
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
                                    <button type="button" id="cancel" class="btn" style="background-color:#1D4860;color:#F6F6F6" data-dismiss="modal">Close</button>        
                                  </div>
                                </div><!-- /.modal-content -->
                              </div><!-- /.modal-dialog -->
            
        </asp:Panel>
 <%--   Fin de la face del modal--%>




</asp:Content>
