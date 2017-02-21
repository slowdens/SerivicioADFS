<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="General.aspx.cs" Inherits="ServicioBecario.Vistas.General" %>
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
        <div class="col-md-12 text-center">
            <h1>Servicio Becario</h1>

        </div>
    </div>
    <div class="row">
        <div class="col-md-12 text-center">
            <h4>Reportes de becarios</h4>

        </div>
    </div>
    <div class="jumbotron">
       
                <div class="row">
                    <div class="col-md-1">
                        <label>Periodo</label>
                    </div>
                    <div class="col-md-3">
                        <asp:DropDownList ID="ddlperiodo" runat="server" CssClass="form-control" OnDataBound="ddlperiodo_DataBound"></asp:DropDownList>
                    </div>
                    <div class="col-md-1">
                        <label>
                            Nivel            
                        </label>
                    </div>
                    <div class="col-md-2">
                        <asp:DropDownList runat="server" ID="ddlNivel" CssClass="form-control" OnDataBound="ddlNivel_DataBound" OnSelectedIndexChanged="ddlNivel_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </div>
                    
                    <div class="col-md-1">
                        <label>Programa academico</label>
                    </div>
                    <div class="col-md-4">
                        <asp:DropDownList runat="server" ID="ddlPAcademico" CssClass="form-control" OnDataBound="ddlPAcademico_DataBound"></asp:DropDownList>
                    </div>
               </div> 
                <div class="row">
                    <div class="col-md-1">
                        <label>Campus</label>
                    </div>
                    <div class="col-md-3">
                        
                        <table><tr><td>
                     <asp:DropDownList runat="server" ID="ddlCampus" CssClass="form-control validate[required]" OnDataBound="ddlCampus_DataBound"></asp:DropDownList>
                             <asp:Label runat="server" ID="lblCampus"></asp:Label>
                                   </td><td>
                <img src="../images/icono-question.png" width="30" height="30" class="imgHelp" /><div class="Help">Si requieres un reporte multicampus solicítalo a tu DBA</div>
                </td></tr>
                </table>
                          
                            <asp:HiddenField runat="server" ID="hdfMostrarId"/>
                            <asp:HiddenField runat="server" ID="hdfActivarRol"/>

                    </div>                  
                    <asp:Panel runat="server" ID="pnlEvaluados" Visible="true">
                        <div class="col-md-1">
                        <label>Evaluados</label>
                    </div>
                    <div class="col-md-3">
                        <asp:DropDownList runat="server" ID="ddlevaluados" CssClass="form-control" AutoPostBack="true" OnDataBound="ddlevaluados_DataBound" OnSelectedIndexChanged="ddlevaluados_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                    </asp:Panel>
                    
                    <div class="col-md-3">
                        <asp:CheckBox runat="server" AutoPostBack="true" Text="No han sido evaluados" ID="chkNoEvaluados" OnCheckedChanged="chkNoEvaluados_CheckedChanged" />
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class=" col-md-offset-5 col-md-1">
                        <asp:Button ID="btnfiltrar" runat="server" CssClass="btn btn-primary" text="Filtrar" OnClick="btnfiltrar_Click" OnClientClick="validar();"/>
                    
                        
                        
                         </div>
                    <div class="col-md-1">
                        <asp:ImageButton runat="server" ID="btnimgnDescargar" ToolTip="Descargar" ImageUrl="~/images/Excel.jpg" OnClick="Unnamed1_Click" />
                    </div>
                </div>
           
        
    </div>
    <div class="row table-condensed">
        <div class="col-md-12 col-sm-12 col-xs-12 col-lg-12" >
            <asp:Panel ID="pnldatos" runat="server" CssClass="hscrollbar">
                
                        <asp:GridView ID="gvDatos" AllowPaging="True" AutoGenerateColumns="false" CssClass="table" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" OnPageIndexChanging="gvDatos_PageIndexChanging">
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField DataField="Matricula" HeaderText="Matrícula" SortExpression="Matricula" ItemStyle-CssClass="text-left" HeaderStyle-CssClass="text-left"></asp:BoundField>
                                <asp:BoundField DataField="Alumno" HeaderText="Alumno becario" SortExpression="Alumno" ItemStyle-CssClass="text-left" HeaderStyle-CssClass="text-left" ></asp:BoundField>
                                <asp:BoundField DataField="Periodo" HeaderText="Periodo" SortExpression="Periodo" ItemStyle-CssClass="text-left" HeaderStyle-CssClass="text-left" ></asp:BoundField>
                                <asp:BoundField DataField="Programa" HeaderText="Programa academico" SortExpression="Programa" ItemStyle-CssClass="text-left" HeaderStyle-CssClass="text-left"></asp:BoundField>
                                <asp:BoundField DataField="Proyecto" HeaderText="Proyecto" SortExpression="Proyecto" ItemStyle-CssClass="text-left" HeaderStyle-CssClass="text-left" ></asp:BoundField>
                                <asp:BoundField DataField="Evaluacion" HeaderText="Evaluación" SortExpression="Evaluacion" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"></asp:BoundField>
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
                                    <button type="button" id="cancel" class="btn" style="background-color:#1D4860;color:#F6F6F6" data-dismiss="modal">Cerrar</button>        
                                  </div>
                                </div><!-- /.modal-content -->
                              </div><!-- /.modal-dialog -->
            
        </asp:Panel>
 <%--   Fin de la face del modal--%>

        </ContentTemplate>
    </asp:UpdatePanel>

   
</asp:Content>
