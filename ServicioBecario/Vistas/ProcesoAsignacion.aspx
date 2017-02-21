<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="ProcesoAsignacion.aspx.cs" Inherits="ServicioBecario.Vistas.ProcesoAsignacion" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <%--<script type="text/javascript" src="../scripts/Enginee/js/jquery-1.8.2.min.js"  charset="utf-8"></script>--%>
    <link rel="stylesheet" href="../scripts/footable.bootstrap.min.css" type="text/css" /> 
    <link rel="stylesheet"  href="../scripts/Enginee/css/validationEngine.jquery.css" type="text/css"/> 
    <link rel="stylesheet" href="../scripts/Estiloss.css" type="text/css" />  
     
    <script type="text/javascript" src="../scripts/Enginee/js/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../scripts/Enginee/js/jquery.validationEngine.js" charset="utf-8"></script>
    
    <script src="../scripts/jquery/ui/1.11.4/jquery-ui.js"></script>

    <link href="../scripts/DragAndDrog.css" rel="stylesheet" />
    
      <style type="text/css">
          .colores
          {
               height:45px;
               width: 100%;
               background: #3B83C0;
               font-size: 35px;
               text-align: center;
               font: bold  ;
               color: #FFFFFF;
   
  
            }
        </style>


    <script>

        $(function () {
            $("#sortable1,#sortable2").sortable({
                connectWith: "#sortable1,#sortable2",
                revert:true,
                update: function () {
                    var id_campus = $('#<%= hdf_id_campus.ClientID %>').val();
                    var ordenElementos = $("#sortable1").sortable("toArray").toString();
                    var ordenElementoss = $("#sortable2").sortable("toArray").toString();
                    
                    var arrElm = ordenElementoss.split(",");
                    actDomPrioridad(arrElm);

                    var obj = "{element_s1:'" + ordenElementos + "',element_s2:'" + ordenElementoss + "',id_campus:'" + id_campus + "' }";                

                    $.ajax({
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        data: obj,
                        url: "ProcesoAsignacion.aspx/guardaCambios",
                        success: function (dd) {
                            var dato = dd.d
                            console.log(dato);
                        }
                    });                   
                },
                placeholder: "elementliquitar"

            });
        });


        function actDomPrioridad(arreglo) {
                     
            for (x = 1; x <= 4; x++) {

                document.getElementById("img-ord" + x).style.display = "none";
            }
            for (x = 0; x <= arreglo.length - 1; x++) {
                if (arreglo[0] == null || arreglo[0] == '')
                {
                    document.getElementById("img-ord1").style.display = "none";
                }
                else
                {
                    document.getElementById("img-ord" + (x + 1)).style.display = "block";
                }
                
            }
        }
        function iniciarElementos(elem) {
            document.getElementById("img-ord" + elem).style.display = "block";
        }


        function soloNumeros(e) {
            key = e.keyCode || e.which;
            tecla = String.fromCharCode(key).toLowerCase();
            letras = "0123456789";
            especiales = "8-37-39-46";

            tecla_especial = false
            for (var i in especiales) {
                if (key == especiales[i]) {
                    tecla_especial = true;
                    break;
                }
            }

            if (letras.indexOf(tecla) == -1 && !tecla_especial) {
                return false;
            }
        }
        function validar() {
            //Esto es para validar campos
            jQuery("form").validationEngine();
        }
        function quitarValidar() {
            //Esto es para omitir la validación
            jQuery("form").validationEngine('detach');
        }
        function confirmar(rs) {

            var r = confirm(rs);
           $('#<%=hdfDesion.ClientID%>').val(r);
        }
    </script>
    <div class="row">
            <div class="col-md-12 " >
                 <h1 class="text-center">Servicio Becario</h1>
            </div>
        </div>
    <div class="row">
        <div class="col-md-12 text-center">
            <h4>Configuración de proceso de asignación</h4>            
        </div>        
    </div>  

        <asp:HiddenField ID="hdfUsuario" runat="server" />
        <asp:HiddenField ID="hdfActivarRol" runat="server" />
        <asp:Panel ID="pnlSeleccionaCampus" runat="server" Visible="true">
            <div class="jumbotron">        
                <div class="row">
                    <div class="col-md-1 col-md-offset-1">
                        <label>Campus</label>
                      
                    </div>                    
                    <div class="col-md-3">
                        <asp:DropDownList ID="ddlCampus"  CssClass="form-control validate[required]" runat="server" OnDataBound="ddlCampus_DataBound" Visible="true"></asp:DropDownList>
                        <asp:Label ID="lblCampus" runat="server" Text="Guadalajara" Visible="false"></asp:Label>
                        <asp:HiddenField ID="hdfIdCampus" runat="server"  />                       
                   <br /><br /><br /><br /> </div>
                    <div class="col-md-2">
                        <label>Periodo</label>
                   <br /><br /><br /><br /> </div>
                    <div class="col-md-3">
                        <asp:DropDownList runat="server" ID="ddlPeriodo" CssClass="form-control validate[required]" OnDataBound="ddlPeriodo_DataBound" ></asp:DropDownList>
                   <br /><br /><br /><br /> </div>                               
                                       
                    <div class="col-md-1">
                        <asp:Button ID="btnConfigurar" runat="server" CssClass="btn btn-primary" Text="Configurar" OnClick="btnConfigurar_Click"  OnClientClick="validar();"/>
                  <br /><br /><br /><br />  </div>                    
                </div>                       
            </div>
        </asp:Panel>        
  
   
    <asp:Panel ID="pnlGeneral" CssClass="table-condensed " runat="server" Visible="false">
        <div class="jumbotron">
            <div class="row  hscrollbar">
                <div class="col-md-10 ">
                    <div class="row">
                        <div class="col-md-offset-2 col-md-11 col-sm-11 col-xs-11 col-lg-11">
                            <%--Aqui es donde se genera el drop and drag--%>
                            <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnGuarConfiguracion" Width="100%" Text="Correr" CssClass="btn btn-primary" runat="server" OnClick="btnGuarConfiguracion_Click" />
                        </div>
                    </div>
                    <div class="row">
                        <div class=" col-md-12">
                            <asp:Button ID="btprueba" Width="100%" CssClass="btn btn-primary" runat="server" Text="Provar" OnClick="btprueba_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>   
                  
    </asp:Panel>
    <asp:HiddenField ID="hdfTipoEjecucion" runat="server" />
    <asp:Label runat ="server" ID="lblMensajito" ></asp:Label>
    <asp:Panel runat="server" ID="pnlGridview" Visible="false">
        <div class="row table-condensed">
            <div class="col-md-11 col-sm-11 col-lg-11 col-xs-11">
                <asp:Panel ID="pnlgrid" CssClass="hscrollbar" runat="server">
                    <asp:GridView ID="gvCorreAsignaciones" AllowPaging="true" OnPageIndexChanging="gvCorreAsignaciones_PageIndexChanging"  AutoGenerateColumns="false" CssClass="table" runat="server" PageSize="20" CellPadding="4" ForeColor="#333333" GridLines="None">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775"></AlternatingRowStyle>
                        <Columns>
                            <asp:BoundField  DataField="Matricula" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="Matrícula" SortExpression="Matricula"></asp:BoundField>
                            <asp:BoundField DataField="NombreBecario" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="Nombre becario" SortExpression="NombreBecario"></asp:BoundField>
                            <asp:BoundField DataField="TipoAsignacion" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="Tipo asignación" SortExpression="TipoAsignacion"></asp:BoundField>
                            <asp:BoundField DataField="Nomina" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="Nomina" SortExpression="Nomina"></asp:BoundField>
                            <asp:BoundField DataField="NombreNomina" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="Nombre responsable" SortExpression="NombreNomina"></asp:BoundField>
                            <asp:BoundField DataField="Periodo" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"  HeaderText="Periodo" SortExpression="Periodo"></asp:BoundField>
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
            <div class="col-md-1">
                <asp:ImageButton runat="server" ID="imgbtnDescargar" ToolTip="Descargar" ImageUrl="~/images/Excel.jpg" OnClick="imgbtnDescargar_Click" />
            </div>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlSinAsignar">
        <asp:Label ID="lbltitiloGrid" runat="server" Text=""></asp:Label>
        <div class="row table-condensed">
           <div class="col-md-11 col-sm-11 col-lg-11 col-xs-11">
                <asp:Panel ID="pnlgridSinAsignar" CssClass="hscrollbar" runat="server">
                    <asp:GridView  ID="gvSinasignar" OnPageIndexChanging="gvSinasignar_PageIndexChanging" CssClass="table" runat="server" Width="100%" PageSize="10" AllowPaging="true" AutoGenerateColumns="false" CellPadding="4" ForeColor="#333333" GridLines="None">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775"></AlternatingRowStyle>

                        <Columns>
                            <asp:BoundField DataField="Nomina" HeaderText="Nomina" SortExpression="Nomina"   HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center"></asp:BoundField>
                            <asp:BoundField DataField="Nombre" HeaderText="Tipo de solicitud" SortExpression="Nombre"  HeaderStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center"></asp:BoundField>
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
    </asp:Panel>
    

  


     <!-- Bootstrap Modal Dialog -->
<div class="modal fade" id="myModal" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
    <div class="modal-dialog">
        <asp:UpdatePanel ID="upModal" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-content" style="background-color:#E6E6E6">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title familia-letra" >                            
                            <asp:Label ID="lblModalTitle" runat="server" Text=""></asp:Label></h4>
                    </div>
                    <div class="modal-body letra" >
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Alertaz.png"  />
                        <asp:Label ID="lblModalBody" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="modal-footer">
                        <button class="btn btn-primary " style="background-color:#1D4860;color:#F6F6F6" data-dismiss="modal" aria-hidden="true">Cerrar</button>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
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
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                    <h4 class="modal-title">
                                        <asp:Label ID="lblCabeza" Font-Names="open sans" Font-Size="13px"  runat="server" Text="Este es el titulo"></asp:Label></h4>
                                  </div>
                                  <div class="modal-body">
                                        <div class="col-md-1">
                                            <asp:Image ID="Image2" runat="server" ImageUrl="~/images/Alertaz.png" />
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

    <asp:HiddenField ID="hdfDesion" runat="server" />
    <asp:HiddenField ID="hdf_id_proceso_asignacion_campus" runat="server" />
    <asp:HiddenField ID="hdf_id_precoso_asignacion" runat="server" />
    <asp:HiddenField ID="hdf_id_campus" runat="server" />
    

</asp:Content>
