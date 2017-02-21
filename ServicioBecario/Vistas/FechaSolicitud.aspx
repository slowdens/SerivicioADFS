<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="FechaSolicitud.aspx.cs" Inherits="ServicioBecario.Vistas.FechaSolicitud" Culture="Auto" UICulture="Auto" %>
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
        function Bandera() {
            localStorage["Bandera"] = 1;
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

           <%-- var r = confirm(rs);
            //$('#<%=hdfDesion.ClientID%>').val(r);--%>
        }

        function formatofecha(e, control) {
            var dato;
            key = e.keyCode || e.which;
            tecla = String.fromCharCode(key).toLowerCase();
            letras = "/0123456789";
            especiales = "8-37-39-46";

            var array = especiales.split("-");

            tecla_especial = false
            //for (var i in especiales) {
            //    if (key == especiales[i]) {
            //        tecla_especial = true;
            //        break;
            //    }
            //}




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

    </script>
       <div class="row">
           <div class="col-md-12">
                <h1 class="text-center">Servicio Becario</h1>
           </div>
       </div>
      <div class="row">
          <div class="col-md-12 text-center">
              <h4 class="text-center">Fecha de solicitud de becarios</h4>
              
          </div>   
    </div>
    <a class="btn btn-primary btn-sm" id="btnA"><em class="glyphicon glyphicon-plus"></em> Agregar</a>
    <a class="btn btn-primary btn-sm" id="btnB"><em class="glyphicon glyphicon-search"></em> Buscar</a>
    <div class="ContAll">
    <div id="ContAg">
    <asp:Panel ID="pnlMostrarCampus" Visible="false" runat="server">
        <div class="jumbotron">        
                <div class="row">
                    <div class="col-md-1 col-md-offset-4">
                        <label>Campus</label>
                        <%--<asp:Label ID="Label1" runat="server" Text="Campus"></asp:Label>--%>
                    </div>
                    <div class="col-md-3">
                        <%--<asp:DropDownList ID="ddlCampus" CssClass="form-control " runat="server" OnDataBound="ddlCampus_DataBound" OnSelectedIndexChanged="ddlCampus_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>--%>
                    </div>
                </div>      
        </div>
    </asp:Panel>
    
        <div class="jumbotron">
            <div class="row">
                <div class="col-md-1 col-md-offset-4">
                    <label>Campus</label>
                   
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlCampus" CssClass="form-control " runat="server" OnDataBound="ddlCampus_DataBound" OnSelectedIndexChanged="ddlCampus_SelectedIndexChanged" AutoPostBack="true" Visible="true"></asp:DropDownList>
                    <asp:Label ID="lblCampus" runat="server" Text=""></asp:Label>
                    <asp:HiddenField ID="hdfidCampus" runat="server" />
                </div>
            </div>
            <br />
            <div class="row"  role="form" data-toggle="validator" >                
                    <div class="col-md-1">
                        <label>Periodo</label>
                        <%--<asp:Label ID="Label3" CssClass="control-label" runat="server" Text="Periodo" ></asp:Label>--%>
                    </div>
                    <div class="col-md-3">
                        <asp:DropDownList CssClass="form-control validate[required]" ID="ddlPeriodo" runat="server" OnDataBound="ddlPeriodo_DataBound"></asp:DropDownList>
                    </div>
                    <div class="col-md-1">
                        <label>Fecha inicio</label>
                        <%--<asp:Label ID="Label4" CssClass="control-label"  For="txtFechaInicio" runat="server" Text="Fecha inicio" ></asp:Label>--%>
                    </div>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtFechaInicio" OnKeyPress = "return formatofecha(event,this);"  placeholder="dd/mm/aaaa" runat="server" CssClass="form-control validate[required]" ></asp:TextBox>
                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFechaInicio" />                                            
                    </div>
                    <div class="col-md-1">
                        <label>Fecha fin</label>
                        <%--<asp:Label ID="Label5" runat="server" Text="Fecha fin"></asp:Label>--%>
                    </div>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtFechaFin" runat="server" OnKeyPress = "return formatofecha(event,this);"  placeholder="dd/mm/aaaa" CssClass="form-control validate[required]" ></asp:TextBox>
                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtFechaFin" />
                    </div>
                    <div class="col-md-1">
                        
                        
                    </div>
                    <div class="col-md-1">
                        
                    </div>
                
             </div>
            <div class="row">
                <div class="col-md-offset-5 col-md-1">
                    <asp:Button ID="btnGuardar" CssClass="btn btn-primary" runat="server"  OnClientClick="validar();" Text="Guardar" OnClick="btnGuardar_Click" />
                        <asp:Button ID="btnActualizar" CssClass="btn btn-primary" runat="server"  OnClientClick="validar();" Text="Actualizar" visible="false" OnClick="btnActualizar_Click" />
                </div>
                <div class="col-md-1">
                    <asp:Button ID="btncancelar" OnClientClick="quitarValidar();" Text="Cancelar" runat="server" CssClass="btn btn-primary" Visible="false" OnClick="btncancelar_Click" />
                </div>
            </div>
            
        </div>
        </div>
        <div id="ContBu"> 
        <%--Empieza zona de filtrado--%>
        <asp:Panel ID="pnlBusquedaGrid" runat="server" Visible="false">
        <div class="jumbotron">
            <div class="row">
                <div class="col-md-12 text-center"> 
                    <fieldset>
                        <legend class="text-center">
                            <h4>Filtros</h4>
                        </legend>
                    </fieldset>                                      
                </div>                
            </div>
            <div class ="row">
                <div class="col-md-1">
                    <label>Periodo</label>
                    <%--<asp:Label ID="Label7" runat="server" Text="Periodo"></asp:Label>--%>
                </div>
                <div class="col-md-3">
                    <asp:DropDownList ID="ddlFiltrarPeriodo"  CssClass="form-control" runat="server" OnDataBound="ddlFiltrarPeriodo_DataBound"></asp:DropDownList>
                </div>
                <div class="col-md-1">
                    <label>Fecha inicio</label>
                    <%--<asp:Label ID="Label9" runat="server" Text="Fecha inicio"></asp:Label>--%>
                </div>                
                <div class="col-md-2">
                    <asp:TextBox ID="txtFiltrarFechaInicio" OnKeyPress = "return formatofecha(event,this);"  placeholder="dd/mm/aaaa" runat="server" CssClass="form-control" ></asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtFiltrarFechaInicio" />
                </div>
                <div class="col-md-1">
                    <label>Fecha fin </label>
                    <%--<asp:Label ID="Label6" runat="server" Text="Fecha fin"></asp:Label>--%>
                </div>
                <div class="col-md-2">
                    <asp:TextBox ID="txtFiltrarFechaFin" runat="server" OnKeyPress = "return formatofecha(event,this);"  placeholder="dd/mm/aaaa" CssClass="form-control" ></asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtFiltrarFechaFin" />
                </div>
                </div>
                <div class="row">
                <asp:Panel ID="PnlFiltrarMostrarCampus" runat="server">
                    <div class="col-md-1">
                        <label>Campus</label>
                        <%--<asp:Label ID="Label10" runat="server" Text="Campus"></asp:Label>--%>
                    </div>
                    <div class="col-md-3">
                        <asp:DropDownList ID="ddlFiltraCampus" runat="server" CssClass="form-control" OnDataBound="ddlFiltraCampus_DataBound"></asp:DropDownList>
                    </div>

                </asp:Panel>                
              </div>
            <div class="row">
                <div class="col-md-1 col-md-offset-5">
                    <asp:Button ID="btnFiltrar" OnClientClick="quitarValidar();" runat="server" Text="Filtrar" CssClass="btn btn-primary" OnClick="btnFiltrar_Click"/>
                </div>
            </div>
            <%--Termina zona de filtrado--%>
        </div>
          
            <div class="row table-condensed">
                <div class="col-md-12">
                    <asp:Panel ID="pnlxx" runat="server" CssClass="hscrollbar">
                        <asp:GridView ID="GVMostarFechas" AllowPaging="true" PageSize="20" CssClass="table"  DataKeyNames="id_fecha_solicitud" Width="100%" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false" GridLines="None" OnPageIndexChanging="GVMostarFechas_PageIndexChanging" OnSelectedIndexChanged="GVMostarFechas_SelectedIndexChanged"  >
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775"  />
                        <Columns>
                            <asp:BoundField DataField="id_fecha_solicitud" HeaderText="id_fecha_solicitud" SortExpression="id_fecha_solicitud" Visible="false" />                        
                           <asp:BoundField DataField="Campus"    HeaderText="Campus" HeaderStyle-HorizontalAlign="Center" Visible="true" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-left"  ItemStyle-CssClass="text-left"/> 
                             <asp:BoundField DataField="Fecha_inicio" FooterStyle-VerticalAlign="Middle"   HeaderText="Fecha inicio" HeaderStyle-CssClass="text-center"  ItemStyle-CssClass="text-center" HeaderStyle-HorizontalAlign="Center" Visible="true" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Fecha_fin"    HeaderText="Fecha fin" HeaderStyle-HorizontalAlign="Center" Visible="true" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center"  ItemStyle-CssClass="text-center"/>
                            <asp:BoundField DataField="Periodo"    HeaderText="Periodo" HeaderStyle-HorizontalAlign="Center" Visible="true" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center"  ItemStyle-CssClass="text-center"/>
                            <asp:CommandField ItemStyle-HorizontalAlign="Center"  SelectImageUrl="~/images/update.png" ShowSelectButton="True" ButtonType="Image" HeaderText="Modificar"  HeaderStyle-CssClass="text-center" />
                        </Columns>
                        
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#3B83C0" Font-Bold="True" ForeColor="White" CssClass="letra" />
                        <HeaderStyle BackColor="#3B83C0" Font-Bold="True" ForeColor="White" CssClass="letra" />
                        <PagerStyle BackColor="#3B83C0" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                    </asp:Panel>
                    
                </div>
            </div>
       </asp:Panel>
        
        </div></div>
    <%--Se tiene que copear este face para junto un metodo masa para poderse ejecuatar--%>
    <asp:Label ID="Label14" runat="server" Text=""></asp:Label>
        <!-- ModalPopupExtender -->
        <cc1:ModalPopupExtender ID="mp1" runat="server" PopupControlID="Panel1" TargetControlID="Label14"
            CancelControlID="cancel" BackgroundCssClass="modalBackground">
        </cc1:ModalPopupExtender>
        <asp:Panel ID="Panel1" runat="server"  align="center" style = "display:none">
             <div class="modal-dialog">
                                <div class="modal-content" style="background-color:#E6E6E6">
                                  <div class="modal-header">                                   
                                    <h4 class="modal-title">
                                        <asp:Label ID="lblCabeza" runat="server" Text="Este es el titulo"></asp:Label>
                                    </h4>
                                  </div>
                                  <div class="modal-body">
                                        <div class="col-md-1">
                                            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Alertaz.png" />
                                        </div>
                                        <div class="col-md-11">
                                            <asp:Label ID="lblcuerpo" runat="server" Text=""></asp:Label>&hellip;
                                        </div>                                  
                                  </div>
                                  <div class="modal-footer">
                                    <button type="button" id="cancel" class="btn" style="background-color:#1D4860;color:#F6F6F6" data-dismiss="modal">Cerrar</button>        
                                  </div>
                                </div><!-- /.modal-content -->
                              </div><!-- /.modal-dialog -->
            
        </asp:Panel>
 <%--   Fin de la face del modal--%>
        <asp:HiddenField ID="hdfMostrarId" runat="server" />
        <asp:HiddenField ID="hdfActivarRol" runat="server" /> <%--este compomente es para saber si esta habilitado el rol que muestra el campus--%>

    
    
</asp:Content>
