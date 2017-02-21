<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="BecarioPorPuesto.aspx.cs" Inherits="ServicioBecario.Vistas.BecarioPorPuesto" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        body {
	        font-size:13px;
	        font-family: 'open sans';
        }	
    </style>
    <link rel="stylesheet" href="../scripts/footable.bootstrap.min.css" type="text/css" /> 
    <link rel="stylesheet"  href="../scripts/Enginee/css/validationEngine.jquery.css" type="text/css"/> 
    <link rel="stylesheet" href="../scripts/Estiloss.css" type="text/css" />   
    <%--<script type="text/javascript" src="../scripts/Enginee/js/jquery-1.8.2.min.js"  charset="utf-8"></script>--%>
    <script type="text/javascript" src="../scripts/Enginee/js/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../scripts/Enginee/js/jquery.validationEngine.js" charset="utf-8"></script>
    <script>
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
        function soloNumeros(e)
        {
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
        function confirmar()
        {
            validar();
            <%-- var r = confirm();
            $('#<%=hdfDesion.ClientID%>').val(r);--%>
            var valor = $('#<%=hdf_idCampus.ClientID%>').val();
            if(valor==3)
            {
                var r = confirm('Esta información se actualizara para todo los campus \\n esta seguro de hacerlo');
                $('#<%=hdfDesion.ClientID%>').val(r)
            }
            else
            {
                var r = confirm('Esta seguro que desea actualizar los datos');
                $('#<%=hdfDesion.ClientID%>').val(r)
            }
        }
    </script>

    <div class="row ">
        <div class="col-md-12 text-center">        
           
           <h1 class="text-center">Servicio Becario</h1>
            
        </div>        

    </div>
    <div class="row ">
        <div class="col-md-12 text-center">        
           <%--<asp:Label ID="Label8" runat="server" ForeColor="#0000cc" Font-Names="open sans" Font-Size="15px" Text="Becarios por puesto"></asp:Label>--%>
            <h4>  Becarios por puesto</h4>
            
        </div>        

    </div>
    <a class="btn btn-primary btn-sm" id="btnA"><em class="glyphicon glyphicon-plus"></em> Agregar</a>
    <a class="btn btn-primary btn-sm" id="btnB"><em class="glyphicon glyphicon-search"></em> Buscar</a>
    <div class="ContAll">
    <div id="ContAg">
    <asp:Panel ID="pnlCampus" runat="server" Visible="false">
    <div class="jumbotron">
        <div class="row">
            <div class="col-md-offset-4 col-md-1 ">
                <label>Campus</label>
            </div>
            <div class="col-md-2">
                <%--<asp:DropDownList ID="ddlCampus" CssClass="form-control validate[required] " runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCampus_SelectedIndexChanged" OnDataBound="ddlCampus_DataBound"></asp:DropDownList>--%>
            </div>
        </div>
    </div>
    </asp:Panel>
        
    <div class="jumbotron">
        <div class="row">
            <div class="col-md-1">
                <label>Campus</label>
            </div>
            <div class="col-md-2">
                <asp:Label ID="lblCampus"  runat="server" Text=""></asp:Label>
                <asp:DropDownList ID="ddlCampus" CssClass="form-control validate[required] " runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCampus_SelectedIndexChanged" OnDataBound="ddlCampus_DataBound"></asp:DropDownList>
            </div>
            <div class="col-md-1">
                <label>Puesto</label>
            </div>
            <div class="col-md-3">
                <asp:DropDownList ID="ddlPuestos" CssClass="form-control validate[required]" runat="server" OnDataBound="ddlPuestos_DataBound"></asp:DropDownList>
            </div>
            <div class="col-md-2">
                <label>Cantidad becarios</label>
            </div>
            <div class="col-md-1">
                <asp:TextBox ID="txtCantidadBecarios" CssClass="form-control validate[required] " OnKeyPress = "return  soloNumeros(event);" runat="server" placeholder="0"></asp:TextBox>
            </div>  
              <div class="col-md-1">
                <asp:Button ID="btnGuardar" CssClass="btn btn-primary" runat="server" Text="Guardar" OnClientClick="validar();" OnClick="btnGuardar_Click" />
            </div>          
        </div>
        <div class="row">
          
            <div class="col-md-1 col-md-offset-4 ">
                <asp:Button ID="btnActualizar" CssClass="btn btn-primary" runat="server" Text="Modificar" visible="false" OnClick="btnActualizar_Click" OnClientClick="confirmar();" />
             </div>
            <div class="col-md-1">
                <asp:Button ID="btnCancelar" CssClass="btn btn-primary" runat="server" Text="Cancelar"  visible="false" OnClick="btnCancelar_Click" OnClientClick="quitarValidar();" />
            </div>
        </div>
        
    </div>
           </div>
        <div id="ContBu">   
    <asp:Panel ID="pnlMostrarGrid" Visible="false" runat="server">
        <div class="jumbotron">
            <div class="row">
                <div class="col-md-12 text-center">
                    <fieldset>
                        <legend class="text-center">
                            <h4>  Filtros</h4>
                        </legend>
                    </fieldset>
                  
                </div>
            </div>
            <div class="row">
                <div class="col-md-1">
                    <label>Puesto</label>
                </div>
                <div class="col-md-3">
                    <asp:DropDownList ID="ddlFitrarPuesto" CssClass="form-control" runat="server" OnDataBound="ddlFitrarPuesto_DataBound"></asp:DropDownList>
                </div>
                <div class="col-md-1">
                    <label>Cantidad becarios</label>
                </div>
                <div class="col-md-2">
                    <asp:TextBox ID="txtFiltraCantidadBecarios" CssClass="form-control" OnKeyPress = "return  soloNumeros(event);" runat="server" placeholder="0"></asp:TextBox>
                </div>
                <asp:Panel ID="pnlFitrarCampus" Visible="false" runat="server">
                    <div class="col-md-1">
                        <label>Campus</label>
                    </div>
                    <div class="col-md-2">
                        <asp:DropDownList ID="ddlFiltrarCampus" CssClass="form-control" runat="server" OnDataBound="ddlFiltrarCampus_DataBound"></asp:DropDownList>
                    </div>
                </asp:Panel>
                <div class="col-md-1">
                    <asp:Button ID="btnFiltrar" OnClientClick="quitarValidar();"  CssClass="btn btn-primary" runat="server" Text="Filtrar" OnClick="btnFiltrar_Click" />
                </div>
            </div>
        </div>
   
    
    <div class="row table-condensed">
        <div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">
            <asp:Panel ID="pnlxx" runat="server" CssClass="hscrollbar">
                <asp:GridView ID="GvMostrar" CssClass="table" Width="100%" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="false" OnPageIndexChanging="GvMostrar_PageIndexChanging" AllowPaging="true" PageSize="20" OnSelectedIndexChanged="GvMostrar_SelectedIndexChanged" >
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:BoundField DataField="Nombre_puesto" ItemStyle-CssClass="text-center"  HeaderStyle-CssClass="text-center" HeaderText="Puesto" Visible="true" />
                    <asp:BoundField DataField="Cantidad_becarios" ItemStyle-CssClass="text-center"  HeaderStyle-CssClass="text-center" HeaderText="Cantidad de becarios" Visible="true" />
                    <asp:BoundField DataField="Campus" ItemStyle-CssClass="text-left"  HeaderStyle-CssClass="text-left" HeaderText="Campus" Visible="true" />
                    <asp:CommandField SelectImageUrl="~/images/update.png" ShowSelectButton="True" ButtonType="Image"  HeaderText="Modificar" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" />
                </Columns>
                <EditRowStyle BackColor="#999999" />
                <FooterStyle BackColor="#3B83C0"  Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#3B83C0" Font-Bold="True" ForeColor="White" />
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
    <asp:HiddenField ID="hdf_idCampus" runat="server" />
    <asp:HiddenField ID="hdf_rol" runat="server" />
    <asp:HiddenField ID="hdfDesion" runat="server" />
           

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
