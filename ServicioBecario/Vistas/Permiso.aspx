<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="Permiso.aspx.cs" Inherits="ServicioBecario.Vistas.Permiso" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <link href="../scripts/Datatablet/jquery.dataTables.min.css" rel="stylesheet" />
    <%--<link href="../scripts/Table2/bootstrap.min.css" rel="stylesheet" />--%>
    <link href="../scripts/Table2/dataTables.bootstrap.min.css" rel="stylesheet" />
    <link href="../scripts/Table2/responsive.bootstrap.min.css" rel="stylesheet" />

   <%-- <script src="../scripts/Table2/jquery-1.12.0.min.js"></script>--%>
    <script src="../scripts/Table2/jquery.dataTables.min.js"></script>
    <script src="../scripts/Table2/dataTables.bootstrap.min.js"></script>
    <script src="../scripts/Table2/dataTables.responsive.min.js"></script>
    <script src="../scripts/Table2/responsive.bootstrap.min.js"></script>


      <link rel="stylesheet"  href="../scripts/Enginee/css/validationEngine.jquery.css" type="text/css"/> 
     
    <script type="text/javascript" src="../scripts/Enginee/js/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../scripts/Enginee/js/jquery.validationEngine.js" charset="utf-8"></script>
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
        function confirmar(rs) {

           <%-- var r = confirm(rs);
            //$('#<%=hdfDesion.ClientID%>').val(r);--%>
        }
        function VerificarUser()
        {
            var Nomina = $('#<%=AgregarNomina.ClientID%>').val();
           
            $.ajax({
                async: true,
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: '{nomina:"' + Nomina + '"}',
                url: "Permiso.aspx/ElementosAMostrar",
                beforeSend: function (obj) {
                    $("#Cargando").css({ "display": "block" });
                },
                success: function (dd) {
                    var btnAgregar = document.getElementById("btnAgregar");
                    btnAgregar.disabled = true;
                    var dato = dd.d
                    $("#lblnombreRol").html("No se encontró");
                    $("#Cargando").css({ "display": "none" });
                    $("#lblNombre").text("");
                    $("#div-componentes").empty();
                    $("#lblnombreRol").html(dato);
                    
                   

                    var datos = dd.d
                    var s = datos;
                    s = jQuery.parseJSON(datos);
                  
                    if (s[0].Mensaje == "Ok") {
                        btnAgregar.disabled = false;
                        var nombre = s[0].NombreComleto;
                        var rol = s[0].Rol;
                        $("#lblNombre").text(nombre);//val(nombre);
                        $("#lblnombreRol").text(rol);//.val(rol);                                    
                        //Si la nomina esta registrada podra agregar los componentes
                        pintarComponentePanel(Nomina);
                    }
                    else {
                        MensajeRojo(s[0].Mensaje);
                        //MensajeRojo("La nómina " + dato + " no se encuentra registrada en el sistema");
                        //Limpiamos la tabla 
                        $("#lblnombreRol").html("No se encontró");
                        $("#Cargando").css({ "display": "none" });
                        $("#lblNombre").text("");
                        $("#div-componentes").empty();
                        btnAgregar.disabled = true;
                    }





                },
                error: function (msg) {
                    $("#lblnombreRol").html("No se encontró");
                    $("#Cargando").css({ "display": "none" });
                    $("#lblNombre").text("");
                    $("#div-componentes").empty();
                    var btnAgregar = document.getElementById("btnAgregar");
                    btnAgregar.disabled = true;
                }
            });
        }
        function pintarComponentePanel(dato) {
            $.ajax({
                async: true,
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: '{nomina:"' + dato + '"}',
                url: "Permiso.aspx/agregarElementos",
                success: function (dd) {
                    var dato = dd.d
                    //$find("ModalBehaviour").show();
                    $("#div-componentes").empty();
                    $("#div-componentes").html(dato);
                
                    
                }, error: function (e) {
                    alert(e);
                }

            });

        }
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
            {if (letras.indexOf(tecla) == -1 && !tecla_especial) {return false;}
            }
            else
            {if (key == "8" || key == "37" || key == "39" || key == "46") { return true;}
             else { return false;}
            }
            
        }
        $(document).ready(function () {
            $("#<%= AgregarNomina.ClientID %>").keyup(function () {
                if($("#<%= AgregarNomina.ClientID %>").val().length>8)
                VerificarUser();
            });
        });

    </script>
    <div class="row">
            <div class="col-md-12 " >
                 <h1 class="text-center">Servicio Becario</h1>
            </div>
        </div>
    <div class="row">
          <div class="col-md-12 text-center">
              <h4>Permisos especiales</h4>              
              
          </div>
     <a class="btn btn-primary btn-sm" id="btnA"><em class="glyphicon glyphicon-plus"></em> Agregar</a>
    <a class="btn btn-primary btn-sm" id="btnB"><em class="glyphicon glyphicon-search"></em> Buscar</a>
    <div class="ContAll">
        
    <div id="ContAg">
        <div class ="jumbotron">
            <div class="row">
        <div class="col-md-12">
            <div id="error" style="display:none">
                  
            </div>
        </div>
    </div>
            <div class="row">
                <div class="col-md-1"><b>Nómina</b></div>
                <div class="col-md-4">
                    <asp:TextBox ID="AgregarNomina" runat="server" CssClass="form-control validate[required]" placeholder="L00000000" ></asp:TextBox>
                </div>
                <div class="col-md-7">
                    
                    <img src="../images/Cargando.gif" width="20" height="20" style="display:none" id="Cargando" /><label id="lblNombre"></label>&nbsp;&nbsp;&nbsp; <label id="lblnombreRol"></label></div>
               
            </div>
            <div class="row"><div class="col-md-12"></div></div>
            <div class="row">
                <div class="col-md-1"><b>Fecha inicio</b></div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtFechaIncio" runat="server" OnKeyPress = "return formatofecha(event,this);" placeholder="dd/mm/aaaa" CssClass="form-control validate[required]" Width="100%" ></asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFechaIncio"   ClientIDMode="Predictable" EnabledOnClient="true" />
                    </div>
                <div class="col-md-1"><b>Fecha fin</b></div>
                <div class="col-md-4">
                    <asp:TextBox ID="txtFechafin" runat="server" OnKeyPress = "return formatofecha(event,this);"  placeholder="dd/mm/aaaa" CssClass="form-control validate[required]"></asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtFechafin" />
                </div>
            </div>
             <div class="row"><div class="col-md-12"><br />
                 <div id="div-componentes">
                    <%--Esta parte es donde dinamicamente los componentes checkbox se cargan!!--%>
                </div>

                 <br />             </div></div>
            <div class="row">
                <div class="col-md-12 text-center">
                    <input type="button" id="btnAgregar" value="Agregar" onclick="agregar();" class="btn btn-primary" disabled="disabled"/></div>
            
            </div>
             <div class="row"><div class="col-md-12"><br /></div></div>
        </div>
    </div>
   
    <div id="ContBu">
        <div class ="jumbotron">
            <div class="row">
                <div class="col-md-1"><b>Nómina</b></div>
                <div class="col-md-4">
                    <asp:TextBox ID="NominaFiltro" runat="server" CssClass="form-control" placeholder="L00000000"></asp:TextBox>
                </div>
            </div>
            <div class="row"><div class="col-md-12"></div></div>
            <div class="row">
                <div class="col-md-1"><b>Fecha inicio</b></div>
                <div class="col-md-4">
                    <asp:TextBox ID="FechaInicioFiltro" runat="server" OnKeyPress = "return formatofecha(event,this);"  placeholder="dd/mm/aaaa" CssClass="form-control" Width="100%" ></asp:TextBox>
                    <cc1:CalendarExtender ID="FechaInicioFiltro2" runat="server" TargetControlID="FechaInicioFiltro"   ClientIDMode="Predictable" EnabledOnClient="true" />
                    </div>
                <div class="col-md-1"><b>Fecha fin</b></div>
                <div class="col-md-4">
                    <asp:TextBox ID="FechaFinFiltro" runat="server" OnKeyPress = "return formatofecha(event,this);"  placeholder="dd/mm/aaaa" CssClass="form-control"></asp:TextBox>
                    <cc1:CalendarExtender ID="FechaFinFiltro2" runat="server" TargetControlID="FechaFinFiltro" />
                </div>
            </div>
             <div class="row"><div class="col-md-12"><br /></div></div>
            <div class="row">
                <div class="col-md-12 text-center">
                    <asp:Button ID="Filtrar" runat="server" Text="Filtrar" OnClick="Filtrar_Click"  CssClass="btn btn-primary"/></div>
            
            </div>
             <div class="row"><div class="col-md-12"><br /></div></div>
            <div class="row">
                <div class="col-md-12 ">
                <asp:Panel ID="VerFiltro" runat="server" Visible="true">
                    <asp:GridView ID="DatosGrid" runat="server" AutoGenerateColumns="false" PageSize="20" AllowPaging="true" GridLines="None" CellPadding="4" Visible="false" Width="100%" CssClass="table" OnPageIndexChanging="DatosGrid_PageIndexChanging" OnRowDeleting="DatosGrid_RowDeleting">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775"  />
                        <Columns>
                            <asp:BoundField DataField="id_especiales" HeaderText="id_especiales" SortExpression="id_especiales" Visible="false" />                        
                            <asp:BoundField DataField="Menu"    HeaderText="Definición" HeaderStyle-CssClass="text-left"  ItemStyle-CssClass="text-left" HeaderStyle-HorizontalAlign="Center" Visible="true" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Pantalla"    HeaderText="Pantalla" HeaderStyle-HorizontalAlign="Center" Visible="true" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-left"  ItemStyle-CssClass="text-left"/>
                            <asp:BoundField DataField="inicio"    HeaderText="Fecha inicio" HeaderStyle-HorizontalAlign="Center" Visible="true" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center"  ItemStyle-CssClass="text-center"/>
                            <asp:BoundField DataField="fin"    HeaderText="Fecha fin" HeaderStyle-HorizontalAlign="Center" Visible="true" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center"  ItemStyle-CssClass="text-center"/>
                            <asp:BoundField DataField="Nomina"    HeaderText="Nomina" HeaderStyle-HorizontalAlign="Center" Visible="true" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center"  ItemStyle-CssClass="text-center"/>
                            <asp:BoundField DataField="NombreCompleto"    HeaderText="Nombre" HeaderStyle-HorizontalAlign="Center" Visible="true" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-left"  ItemStyle-CssClass="text-left"/>
                             <asp:TemplateField HeaderText="Eliminar" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="imbEliminar"  CommandArgument='<%#Eval("id_especiales")  %>'  runat="server" ImageUrl="~/images/EliminarR.png" OnClick="imbEliminar_Click"/>
                                <cc1:ConfirmButtonExtender ID="cbe" runat="server" DisplayModalPopupID="mpe" TargetControlID="imbEliminar">
                                </cc1:ConfirmButtonExtender>
                                <cc1:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlPopup" TargetControlID="imbEliminar"
                                    OkControlID="btnYes" CancelControlID="btnNo" BackgroundCssClass="modalBackground1">
                                </cc1:ModalPopupExtender>
                                <asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style="display: none">
                                    <div class="header">
                                        Confirmación
                                    </div>
                                    <div class="body">
                                        <asp:Label ID="Label12" runat="server" Text="¿Desea eliminar el registro?"></asp:Label>                            
                                    </div>
                                    <div class="footer" align="right">
                                        <asp:Button ID="btnYes" runat="server" Text="Si" CssClass="yes" />
                                        <asp:Button ID="btnNo" runat="server" Text="No" CssClass="no" />
                                    </div>
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:TemplateField>
                            
                            
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
            </div></div>
        </div>
        </div>
        </div> 
         </div>
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


    <script type="text/javascript">
        function agregar() {
            LimpiarMensaje();
            jQuery("form").validationEngine();

            var nomina = $("#<%=AgregarNomina.ClientID%>").val();
            nomina = nomina.toUpperCase();

            var fin = $("#<%=txtFechafin.ClientID%>").val();
            var inicio = $("#<%=txtFechaIncio.ClientID%>").val();

            //Regex para nomina y para fechas
            var fechas = new RegExp("^[0-3]{1}[0-9]{1}/[0-3]{1}[0-9]{1}/[0-9]{1}[0-9]{1}[0-9]{1}[0-9]{1}$");
            var expNominas = new RegExp("^L{0,1}[0-9]+$");
           //alert(nomina+"  "+ inicio+"    " + fin)

            if (nomina != "" && nomina != null) {
                if (expNominas.test(nomina)) {
                    if (inicio != "" && inicio != null) {
                        if (fechas.test(inicio)) {
                            if (fin != "" && fin != null) {
                                if (fechas.test(fin)) {
                                    //guardarServer(nomina, inicio, fin)
                                    if (fechasTomadadas(inicio, fin) == true) {
                                        guardarServer(nomina, inicio, fin)
                                    }
                                    else {
                                        MensajeRojo("<div class='alert alert-danger text-center'><b>La fecha inicio es mayor que la fecha fin</b></div>");
                                        $("#<%=txtFechaIncio.ClientID%>").focus();
                                    }
                                }
                                else {
                                    MensajeRojo("<div class='alert alert-danger text-center'><b>El formato no es el correcto en las fechas</b></div>");
                                    $("#<%=txtFechafin.ClientID%>").focus();
                                }
                            }
                            else {
                                MensajeRojo("<div class='alert alert-danger text-center'><b>No hay fechas en componente de fecha fin</b></div>");
                                $("#<%=txtFechafin.ClientID%>").focus();
                            }
                        }
                        else {
                            MensajeRojo("<div class='alert alert-danger text-center'><b>El formato de fecha no es valido</b></div>");
                            $("#<%=txtFechaIncio.ClientID%>").focus();
                        }
                    }
                    else {
                        MensajeRojo("<div class='alert alert-danger text-center'><b>No hay datos en campo de fecha inicio</b></div>");
                        $("#<%=txtFechaIncio.ClientID%>").focus();
                    }
                }
                else {
                    MensajeRojo("<div class='alert alert-danger text-center'><b>El formato de la nómina no es el correcto</b></div>");
                    $("#txtnomina").focus();
                }
            }
            else {
                MensajeRojo("<div class='alert alert-danger text-center'><b>Es necesario  escribir una nómina</b></div>");
                $("#txtnomina").focus();
            }



        }
        function fechasTomadadas(inicio, fin) {
            var bandera = false;
            var splitInicio = inicio.split("/");
            var splitFin = fin.split("/");
            var fechainicio = new Date(splitInicio[2], splitInicio[1], splitInicio[0]);
            var fechaFin = new Date(splitFin[2], splitFin[1], splitFin[0]);
            if (fechainicio <= fechaFin) {
                bandera = true;
            }
            return bandera;
        }
        function guardarServer(usuario, inicio, fin) {
            var strcadena = "";
            $("input:checkbox:checked").each(function () {
                strcadena = strcadena + $(this).val() + "!";
            });
            //alert(usuario+"  "+inicio+"   "+fin+"    " +strcadena);
            var t = $('#example').DataTable();
            var table;
            $.ajax({
                async: false,
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: '{p_encrii:"' + strcadena + '",usiario:"' + usuario + '",inicio:"' + inicio + '",fin:"' + fin + '"}',
                url: "Permiso.aspx/guardar",
                success: function (dd) {
                    var dato = dd.d
                    //Limpiamos la tabla 
                    $("#div-componentes").empty();
                    MensajeRojo("<div class='alert alert-success text-center'><b>Registro Correcto</b></div>");
                    $("#<%=AgregarNomina.ClientID%>").focus();

                }
            });
        }
        function MensajeRojo(valor) {
            $("#error").css({ display: "block" });
            $("#error").addClass("rellenar text-center");
            $("#error").html(valor);
        }
        /**************************************************************************************************************************************************/
        function LimpiarMensaje() {
            $("#error").css({ display: "none" });

        }
    </script>
    </asp:Content>