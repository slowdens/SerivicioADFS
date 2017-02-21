<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="ReporteTotales.aspx.cs" Inherits="ServicioBecario.Vistas.ReporteTotales" Culture="Auto" UICulture="Auto" %>
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
    <script src="../scripts/PlantillaZul/Script/datatables.min.js"></script>
    <script type="text/javascript">
        function Bandera() {
            localStorage["Bandera"] = 1;
        }
        function validar() {
            jQuery("form").validationEngine();
        }
        function quitarValidar() {
            jQuery("form").validationEngine('detach');
        }
        function confirmar(rs) {}
        function formatofecha(e, control) {
            var dato;
            key = e.keyCode || e.which;
            tecla = String.fromCharCode(key).toLowerCase();
            letras = "/0123456789";
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
            if (dato.length <= 9)
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
        function Prueba(Id,Campus,Periodo)
        {
            $("#TitleCampus").html(Campus);
            $.ajax({
                async: true,
                type: "POST",
                dataType: "json",
                data: "{Campus:'" + Id + "',Periodo:'" + Periodo + "'}",
                contentType: "application/json; charset=utf-8",
                url: "ReporteTotales.aspx/CampusDetalle",
                beforeSend: function (obj) {
                    $("#Cargando").css({ "display": "block" });
                    $("#BodyCampus").html("");
                },
                success: function (dd) {
                    $("#Cargando").css({ "display": "none" });

                    $("#BodyCampus").html(dd.d);
                    $("#BodyCampus").animate({ "opacity": "1" });
                    $("#dtTable").DataTable();
                    $("#dtTable2").DataTable();
                    
                },
                error: function (msg) {
                    $("#BodyCampus").html("<div class='alert alert-danger text-justify'><b>Error:</b>"+msg.error+"</div>");
                    $("#Cargando").css({ "display": "none" });

                },
                statusCode: {
                    404: function () {
                    $("#BodyCampus").html("<div class='alert alert-danger text-justify'><b>Error: </b> No se encontro el recurso</div>");
                    },
                    500: function () {
                    $("#BodyCampus").html("<div class='alert alert-danger text-justify'><b>Error: </b> Tiempo de espera se agotó (Demasiados resultados)</div>");
                    }
                }
                
            });
         
        }
    </script>

    <div class="row">
            <div class="col-md-12 " >
                 <h1 class="text-center">Servicio Becario</h1>
            </div>
        </div>
    <div class="row">
          <div class="col-md-12 text-center">
              <h4>Reporte General</h4>              
              
          </div>
   </div>
   <div class="row">
       <div class="col-md-12"></div>
   </div>
    <div class="jumbotron">
        <div class="row"><div class="col-md-12"><br /></div></div>
        <div class="row">
            <div class="col-md-4">
                <asp:DropDownList ID="ListaPeriodos" runat="server" OnDataBound="ListaPeriodos_DataBound" CssClass="form-control"></asp:DropDownList>
            </div>
            <div class="col-md-1">
                <asp:Button ID="Filtrar" runat="server" Text="Filtrar" CssClass="btn btn-primary" OnClick="Filtrar_Click" />
            </div>
            <div class="col-md-1">
            <asp:ImageButton runat="server" ID="btnimgnDescargar" ToolTip="Descargar" ImageUrl="~/images/Excel.jpg" OnClick="btnimgnDescargar_Click"/></div>
        </div>
        <div class="row"><div class="col-md-12"><br /><br /></div></div>
    
        <div class="row"><div class="col-md-12"><br />
            <asp:Label ID="Totales" runat="server" Text=""></asp:Label>
            <br /></div></div>




    <div class="row table-condensed">
        <div class="col-md-12 col-sm-12 col-xs-12 col-lg-12" >
            <asp:Panel ID="pnldatos" runat="server" CssClass="hscrollbar">
                
                        <asp:GridView ID="gvDatos" AllowPaging="True" AutoGenerateColumns="false" CssClass="table" PageSize="28" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" OnPageIndexChanging="gvDatos_PageIndexChanging">
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField DataField="Campus" HeaderText="Campus" SortExpression="Campus" ItemStyle-CssClass="text-left" HeaderStyle-CssClass="text-left" ></asp:BoundField>
                                <asp:BoundField DataField="click" HtmlEncode="false" HeaderText="Ver" SortExpression="Evento" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"  ></asp:BoundField>
                                
                                
                                
                                
                                 <asp:BoundField DataField="NoAsignado" HeaderText="No asignado" SortExpression="No asignado" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" ></asp:BoundField>
                                <asp:BoundField DataField="Asignado" HeaderText="Asignados" SortExpression="Asignados" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" ></asp:BoundField>
                                <asp:BoundField DataField="ReAsignado" HeaderText="Reasignados" SortExpression="Reasignados" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                <asp:BoundField DataField="DesAsignado" HeaderText="Desasignados" SortExpression="Desasignados" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" ></asp:BoundField>
                                <asp:BoundField DataField="BecariosTotal" HeaderText="Total de Becarios" SortExpression="Total" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                <asp:BoundField DataField="Pendiente" HeaderText="Pendiente" SortExpression="Pendiente" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                <asp:BoundField DataField="Aprobada" HeaderText="Aprobada" SortExpression="Aprobada" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" ></asp:BoundField>
                                <asp:BoundField DataField="Asignada" HeaderText="Asignada" SortExpression="Asignada" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" ></asp:BoundField>
                                <asp:BoundField DataField="Rechazada" HeaderText="Rechazada" SortExpression="Rechazada" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"></asp:BoundField>
                                <asp:BoundField DataField="SolicitantesTotal" HeaderText="Total de solicitantes" SortExpression="Total" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" ></asp:BoundField>
                                






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
    <!-- Modal -->
  <div class="modal fade" id="myModal" role="dialog">
    <div class="modal-dialog"  style="width:70%">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          <h4 class="modal-title"><span id="TitleCampus"></span></h4>
        </div>
        <div class="modal-body">
          
              <div style="width:100%" class="text-center"><center><img src="../images/loading.gif" id="Cargando" style="display:none"/></center></div>
              <p><span id="BodyCampus" style="opacity:0"></span></p>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
        </div>
      </div>
      
    </div>
  </div>
    </asp:Content>