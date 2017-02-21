<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="Permisos_especial.aspx.cs" Inherits="ServicioBecario.Vistas.Permisos_especial" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet"  href="../scripts/Enginee/css/validationEngine.jquery.css" type="text/css"/>
      
    <%--<script type="text/javascript" src="../scripts/Enginee/js/jquery-1.8.2.min.js"  charset="utf-8"></script>--%>
    <script type="text/javascript" src="../scripts/Enginee/js/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../scripts/Enginee/js/jquery.validationEngine.js" charset="utf-8"></script>  
        
    <script>
        function validar()
        {
            //Esto es para validar campos
            jQuery("form").validationEngine();
            recorrercomp();
        }
        function quitarValidar()
        {
            //Esto es para omitir la validación
            jQuery("form").validationEngine('detach');
        }
        function confirmar(rs) {

            var r = confirm(rs);
            $('#<%=hdfDesion.ClientID%>').val(r);
        }
        function soloLetras(e) {
            key = e.keyCode || e.which;
            tecla = String.fromCharCode(key).toLowerCase();
            letras = "l0123456789";
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


        function txtChanged(mytxt) {
            //document.all.txtFlag.value = 1;
            var dato= $('#<%=txtnomina.ClientID%>').val();
           
            $.ajax({
                async: true,
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: '{nomina:"' + dato + '"}',
                url: "Permisos_especial.aspx/agregarElementos",
                success: function (dd) {
                    var dato = dd.d                   
                    //$find("ModalBehaviour").show();
                    $("#comp").html(dato);
                }
            });
        }
        function recorrercomp()
        {
            var strcadena="";
            $("input:checkbox:checked").each(function () {            
                strcadena = strcadena + $(this).val() + "!";
            });
            var usuario = $('#<%=txtnomina.ClientID%>').val();
            var inicio = $('#<%=txtFechaIncio.ClientID%>').val();
            var fin = $('#<%=txtFechafin.ClientID%>').val();
            
           $.ajax({
                async: false,
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: '{p_encrii:"' + strcadena + '",usiario:"' + usuario + '",inicio:"'+inicio+'",fin:"'+fin+'"}',
                url: "Permisos_especial.aspx/guardar",                        
                success: function (dd) {                                      
                    var dato = dd.d                      
                }
            });         

        }
        
    </script>
      <div class="row">
          <div class="col-md-12 text-center">
              <h1><label>Permisos especiales</label></h1>
              <hr />
          </div>
      </div>
      <div class="row">
        <div class ="col-md-10 form-horizontal jumbotron">
            <div class="row ">
                <div class="col-md-2 col-md-offset-1" >
                    <label>Nómina</label>
                    <%--<asp:Label ID="Label1" runat="server" Text="Nómina" CssClass="control-label"></asp:Label>--%>
                </div>
                <div class="col-md-2">                    
                            <asp:TextBox ID="txtnomina" runat="server" CssClass="form-control validate[required]" OnTextChanged="txtnomina_TextChanged" AutoPostBack="true" OnKeyPress = "return soloLetras(event);" onchange="javascript: txtChanged( this );"  ></asp:TextBox>                    
                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" ServiceMethod="SearchCustomers"
                                MinimumPrefixLength="2"    CompletionInterval="100" EnableCaching="false" CompletionSetCount="10" 
                                TargetControlID="txtnomina" FirstRowSelected="false"  >
                            </cc1:AutoCompleteExtender>
                      
                </div>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                <div class="col-md-3">
                    <asp:Label ID="LblNombreEmpleado" CssClass="control-label" runat="server" Text=""></asp:Label>
                </div>
                <div class="col-md-1">
                    <asp:Label ID="Label7" runat="server" Text="Rol"></asp:Label>
                </div>
                <div>
                    <asp:Label ID="lblRol" runat="server" Text=""></asp:Label>
                </div>
                        </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtnomina" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <br />
            <div class="row"">
                <div class="col-md-2">
                    <label>Fecha inicio</label>
                    <%--<asp:Label ID="Label2" runat="server" Text="Fecha inicio"></asp:Label>--%>
                </div>
                <div class="col-md-2">
                    <asp:TextBox ID="txtFechaIncio" runat="server" CssClass="form-control" Width="100%" ></asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFechaIncio"   ClientIDMode="Predictable" EnabledOnClient="true" />
                </div>
                <div class="col-md-1">                                   
                </div>
                <div class="col-md-2">
                    <label>Fecha fin</label>
                    <%--<asp:Label ID="Label3" runat="server" Text="Fecha fin"></asp:Label>--%>
                </div>
                <div class="col-md-2">
                    <asp:TextBox ID="txtFechafin" runat="server" CssClass="form-control"></asp:TextBox>
                     <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtFechafin" />
                </div>
                <div class="col-md-2">                   
                </div>
            </div>                     
            <div class="row">
                <div class="col-md-12 col-xs-12 col-sm-12"  >                  
                                <asp:Panel ID="Panel1" runat="server"  Height="300px" ScrollBars="Auto">
                                    <div id="comp">
                                        
                                    </div> 
                                        <asp:Table ID="TblComponentes" CssClass="table table-hover col-md-12 col-xs-12 col-sm-12" EnableViewState="false"
                                            runat="server" Height="50px" >                          
                                        </asp:Table>
                                        
                                </asp:Panel>                                            
                </div>                
            </div>
            <div class="row" >
                <div class="col-md-6 col-md-offset-5 col-xs-offset-3 col-sm-offset-3 ">
                    <asp:Button ID="btnGuardar" runat="server" Text="Asignar" CssClass="btn btn-primary" OnClientClick="validar();" OnClick="btnGuardar_Click1"  />
                </div>
                
            </div>      
        </div>
          <!--clse del boton-->
          

          <asp:UpdatePanel ID="UpdatePanel2" runat="server">
              <ContentTemplate>
                  <!--<input id="hdfMostrarPopop" type="hidden" />-->
                  <asp:Label ID="Label4" runat="server" Text=""></asp:Label><!--<asp:HiddenField ID="hdfMostrarPopop" runat="server" />-->
                 <!-- <asp:Button ID="btnpopUp" runat="server" Text="Mostrar Pop" Visible="false" />-->

                  <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" BehaviorID="ModalBehaviour" Enabled="true" PopupControlID="PanelModal" TargetControlID="Label4" BackgroundCssClass="modalBackground" CancelControlID="cancel">
                  </cc1:ModalPopupExtender>
                  <asp:Panel ID="PanelModal" runat="server" style="display:none;background-color:white;width:0px;height:auto">
                        <div class="modal-dialog">
                                <div class="modal-content" style="background-color:#E6E6E6">
                                  <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                    <h4 class="modal-title">
                                        <asp:Label ID="lblCabeza" runat="server" Text=""></asp:Label></h4>
                                  </div>
                                  <div class="modal-body">
                                    <p>
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Alertaz.png" />
                                        <asp:Label ID="lblcuerpo" runat="server" Text=""></asp:Label>&hellip;</p>
                                  </div>
                                  <div class="modal-footer">
                                    <button type="button" id="cancel" class="btn" style="background-color:#1D4860;color:#F6F6F6" data-dismiss="modal">Close</button>        
                                  </div>
                                </div><!-- /.modal-content -->
                              </div><!-- /.modal-dialog -->
                  </asp:Panel>
              </ContentTemplate>
              
          </asp:UpdatePanel>



                  
    </div>
      
    <asp:HiddenField ID="hdfid_usuario" runat="server" />
    <asp:HiddenField ID="hdfDesion" runat="server" />
 
</asp:Content>
