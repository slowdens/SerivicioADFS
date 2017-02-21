<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="Permiso2.aspx.cs" Inherits="ServicioBecario.Vistas.Permiso" %>
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


    <style type="text/css">
       .table th{
            text-align: center;
            background-color: #3B83C0;
            color: #ffffff;
            border-color: #5d7b9d;
        }
        .table td {
            text-align: center;
        }

        .rellenar{
            background-color:red;
        }
        input[type=checkbox].css-checkbox {
            display: none;
        }

            input[type=checkbox].css-checkbox + label.css-label {
                padding-left: 20px;
                height: 15px;
                display: inline-block;
                line-height: 15px;
                background-repeat: no-repeat;
                background-position: 0 0;
                font-size: 15px;
                vertical-align: middle;
                cursor: pointer;
                font-weight:normal;
            }

            input[type=checkbox].css-checkbox:checked + label.css-label {
                background-position: 0 -15px;
            }

        .css-label {
            /*background-image: url(http://html-generator.weebly.com/files/theme/checkboxd1.png);*/
            background-image: url("../images/checkboxd1.png");
        }
    </style>
  

    <script type="text/javascript">
        /***/


        /**************************************************************************************************************************************************/
        //inicalizamos la tabla 

        $(document).ready(function () {
            llenarGrid();
        });

        
       

        /**************************************************************************************************************************************************************************/
        //unavez guardado los permisos los pinta en la tabla
        function llenarGrid() {
            var dato = "asda";
            $.ajax({
                url: 'Permiso.aspx/inicio',
                type: 'post',
                contentType: "application/json; charset=utf-8",
                data: '{nomina:"' + dato + '"}',
                datatype: 'json',
                success: function (dd) {
                    var dataset = dd.d

                    var ds = jQuery.parseJSON(dataset);
                    tablita = $("#example").DataTable({
                        data: ds,
                        "language": {
                            "url": "http://cdn.datatables.net/plug-ins/1.10.11/i18n/Spanish.json"
                        },
                        "bSort": false,
                        responsive: true,
                        columns: [
                            { title: "Nombre" },
                            { title: "Pantalla" },
                            { title: "Inicio" },
                            { title: "Fin" },
                            { title: "Nómina" },
                            { title: "NombreCompleto" },
                            {

                                "width": "20%",
                                "render": function (datas, type, full, meta) {
                                    var buttonID = "rollover_" + full.id;
                                    //deplega el boton eliminar
                                    return "<a id=" + buttonID + " class=' rolloverBtn' role='button'    onclick='eliminar(\"" + ds[datas][7] + "\"," + datas + ",\"" + full[datas] + "\");' ><img src='../images/EliminarR.png' width='30px' height='30px'   /></a>";
                                }
                            }

                        ]
                    });
                }
            });
        }
        /*******************************************************************************************************************************************************************/
        function eliminar(id, contador, idnose) {

            /*Agregamos la seleccion a la tabla */
            var table = $('#example').DataTable();
            //var row = table.row(contador).node();

            var regu = table.row(contador).data();



            var ross = getRow('example', id);

            $(ross).addClass('selected');


       


            /*Preguntamos si se va a eliminar*/
            var decide = confirm("Esta seguro que deceas quitar el permiso");
            if (decide == true) {
                ajaxEliminar(id, contador, ross);
            }
            $(ross).removeClass('selected');

        }

        /*******************************************************************************************************************************************************************/
        //con esta funcion detecta la row apartir del id.
        function getRow(table_id, id) {
            var oTable = $('#' + table_id).dataTable(),
                data = oTable.fnGetData(),
                row, i, l = data.length;

            for (i = 0; i < l; i++) {
                row = data[i];
                var strinf = row[7];
                // columns to search are hard-coded, but you could easily pass this in as well
                if (row[7] == id) {
                    return $('#' + table_id + ' tr').eq((i + 1));
                }
            }
            return false;
        }


        /*****************************************************************************************************************************************************************/
        //esta funcion selecciona los registross a eliminar
        function ajaxEliminar(id, contador, ross) {
            // contador = contador - 1;
            var nomina_texbox = $("#txtnomina").val();
            var nomina_grid;
            var table = $('#example').DataTable();


            var datosTabla = table.row(ross).data();
            var Menu = "asda";//datosTabla[0];
            var Pantalla = "adsa";//datosTabla[1];
            var nomina = datosTabla[4];

            nomina_grid = nomina;


            $.ajax({
                url: 'Permiso.aspx/delete',
                type: 'post',
                contentType: "application/json; charset=utf-8",
                data: '{menu:"' + Menu + '", pantalla:"' + Pantalla + '",nomina:"' + nomina + '",id:"' + id + '"}',
                datatype: 'json',
                success: function (datos) {
                    //lo que se decea hacer
                    //alert(datos.d);
                    if (datos.d == "Eliminado") {
                        if (nomina_texbox == nomina_grid) {
                            //Limpiamos los componentes del div
                            $("#div-componentes").empty();
                            //agregamos nuevamente los componentes  al div
                            agregarComponenetes();
                        }

                    }
                }
            });
            //Elinamos el registro correctamente            
            table.row('.selected').remove().draw(false);

        }

        /**************************************************************************************************************************************************/
        //escribe solamente l y numeros para escribir solamente nominas
        function paraNomina(e) {
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
        /**************************************************************************************************************************************************/
        //esta funcion se desplega cuando termina de escribir una nomina
        function agregarComponenetes(tx) {
            /*Limpiamos el componente del mensaje*/
            LimpiarMensaje();

            //funcion para validar que se correcta la nomina
            var dato = $("#txtnomina").val();
            dato = dato.toUpperCase();
            if (dato.indexOf("L") != -1) {
                $("#txtnomina").val(dato);
            }
            else {
                if (dato != "") {
                    dato = "L" + dato;
                    $("#txtnomina").val(dato);
                }
            }
            /*Verifica que la nomina no esta vacia*/
            if (dato != "") {
                var expNominas = new RegExp("^L{0,1}[0-9]+$");
                if (expNominas.test(dato)) {
                    var salida = tomarRoles(dato);
                }
                else {
                    MensajeRojo("!No es un formato valido de nómina!");
                    $("#div-componentes").empty();
                }

            }
            else {
                MensajeRojo("No puede estar la nómina vacia!");
            }
        }
 /********************************************************************************************************************************************************************/
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
            if (dato.length <= 9) {
                if (letras.indexOf(tecla) == -1 && !tecla_especial) {
                    return false;
                }
            }
            else {
                if (key == "8" || key == "37" || key == "39" || key == "46")
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            
        }


/***********************************************************************************************************************************************************************/

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
                    $("#div-componentes").html(dato);
                }
            });

        }

        /**************************************************************************************************************************************************/
        function paraNomina(e, control) {
            var dato;
            key = e.keyCode || e.which;
            tecla = String.fromCharCode(key).toLowerCase();
            letras = "l0123456789";
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
            if (dato.length <= 8)
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
                else
                {
                    return false;
                }

                
            }
            
        }

        /**************************************************************************************************************************************************/

        function tomarRoles(dato) {
            var s = "";
            $.ajax({
                async: true,
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: '{nomina:"' + dato + '"}',
                url: "Permiso.aspx/tomarRol",
                success: function (dd) {
                    var datos = dd.d
                    var s = datos;
                    s = jQuery.parseJSON(datos);
                    if (s[0].Mensaje == "Ok") {
                        var nombre = s[0].NombreComleto;
                        var rol = s[0].Rol;
                        $("#lblNombre").text(nombre);//val(nombre);
                        $("#lblnombreRol").text(rol);//.val(rol);                                    
                        //Si la nomina esta registrada podra agregar los componentes
                        pintarComponentePanel(dato);
                    }
                    else {
                        MensajeRojo(s[0].Mensaje);
                        //MensajeRojo("La nómina " + dato + " no se encuentra registrada en el sistema");
                        //Limpiamos la tabla 
                        $("#div-componentes").empty();
                    }
                }
            });

        }
        /**************************************************************************************************************************************************/
        //Tiene la funcionalidad de agragregar los permisos a la nomina y se dispara la funcion cuando le damos click al botton de agregar
        function agregar() {
            LimpiarMensaje();
            jQuery("form").validationEngine();

            var nomina = $("#txtnomina").val();
            nomina = nomina.toUpperCase();

            var fin = $("#<%=txtFechafin.ClientID%>").val();
            var inicio = $("#<%=txtFechaIncio.ClientID%>").val();

            //Regex para nomina y para fechas
            var fechas = new RegExp("^[0-3]{1}[0-9]{1}/[0-3]{1}[0-9]{1}/[0-9]{1}[0-9]{1}[0-9]{1}[0-9]{1}$");
            var expNominas = new RegExp("^L{0,1}[0-9]+$");


            if (nomina != "" && nomina != null) {
                if (expNominas.test(nomina)) {
                    if (inicio != "" && inicio != null) {
                        if (fechas.test(inicio)) {
                            if (fin != "" && fin != null) {
                                if (fechas.test(fin)) {
                                    //guardarServer(nomina, inicio, fin)
                                    if(fechasTomadadas(inicio, fin)==true)
                                    {
                                        guardarServer(nomina, inicio, fin)
                                    }
                                    else
                                    {
                                        MensajeRojo("La fecha inicio es mayor que la fecha fin");
                                        $("#<%=txtFechaIncio.ClientID%>").focus();
                                    }
                                }
                                else {
                                    MensajeRojo("El formato no es el correcto en las fechas");
                                    $("#<%=txtFechafin.ClientID%>").focus();
                                }
                            }
                            else {
                                MensajeRojo("No hay fechas en componente de fecha fin");
                                $("#<%=txtFechafin.ClientID%>").focus();
                            }
                        }
                        else {
                            MensajeRojo("El formato de fecha no es valído");
                            $("#<%=txtFechaIncio.ClientID%>").focus();
                        }
                    }
                    else {
                        MensajeRojo("No hay datos en campo de fecha inicio");
                        $("#<%=txtFechaIncio.ClientID%>").focus();
                    }
                }
                else {
                    MensajeRojo("El formato de la nómina no es el correcto");
                    $("#txtnomina").focus();
                }
            }
            else {
                MensajeRojo("Es necesario  escribir una nómina");
                $("#txtnomina").focus();
            }



        }
        /**********************************************************************************************************************************************************************/
        function fechasTomadadas(inicio,fin)
        {
            var bandera = false;
            var splitInicio = inicio.split("/");
            var splitFin = fin.split("/");
            var fechainicio = new Date(inicio);
            var fechaFin = new Date(fin);
            if (fechainicio <= fechaFin)
            {
                bandera = true;
            }
            
            

            return bandera;
        }



        /***************************************************************************************************************************************************/
        //Este funcion es para guardar los permisos en la aplicación
        function guardarServer(usuario, inicio, fin) {
            var strcadena = "";
            $("input:checkbox:checked").each(function () {
                strcadena = strcadena + $(this).val() + "!";
            });

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
                    //agregamos nuevamente los componentes  al div
                    agregarComponenetes();
                    table = $('#example').DataTable();
                    //destruimos la tabla 
                    table.destroy();
                    //llenamos la tabla 
                    llenarGrid();

                }
            });
        }

        /**********************************************();****************************************************************************************************/
        function MensajeRojo(valor) {
            $("#error").css({ display: "block" });
            $("#error").addClass("rellenar text-center");
            $("#error").text(valor);
        }
        /**************************************************************************************************************************************************/
        function LimpiarMensaje() {
            $("#error").css({ display: "none" });

        }
        /**************************************************************************************************************************************************/
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
    </div>
    <div class="row">
        <div class="col-md-12">
            <div id="error" style="display:none">
                  
            </div>
        </div>
    </div>
    <div class="jumbotron"> 
        <div class="row">
            <div class="col-md-1">
                <label>Nómina</label>
            </div>
            <div class="col-md-2">
                <input id="txtnomina"  class="form-control validate[required]" type="text" onchange="javascript:agregarComponenetes();"  onkeypress = "return paraNomina(event,this);"/>
            </div>
            <div class="col-md-1">
                <label >Nombre</label>
            </div>
            <div class="col-md-3">
                <label id="lblNombre"></label>
            </div>
            <div class="col-md-1">
                <label>Rol</label>
            </div>
            <div class="col-md-2">
                <label id="lblnombreRol"></label>
            </div>
        </div>
        <br />

        <div class="row">
            <div class="col-md-2">
                <label>Fecha inicio</label>
            </div>

            <div class="col-md-2">
                <asp:TextBox ID="txtFechaIncio" runat="server" OnKeyPress = "return formatofecha(event,this);" CssClass="form-control validate[required]" Width="100%" ></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFechaIncio"   ClientIDMode="Predictable" EnabledOnClient="true" />
            </div>
            <div class="col-md-2">
                <label>Fecha fin</label>
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtFechafin" runat="server" OnKeyPress = "return formatofecha(event,this);" CssClass="form-control validate[required]"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtFechafin" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div id="div-componentes">
                    <%--Esta parte es donde dinamicamente los componentes checkbox se cargan!!--%>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-1 col-md-offset-5">
                <input id="btnAgregar" type="button" onclick="agregar();" value="Agregar"class="btn btn-primary" />
               
            </div>
        </div>
    </div>
    <div class="row ">
        <div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">
            <%--Tabla con la que cargo todos los registros--%>          
                
              <table id="example"  class="table table-striped  dt-responsive nowrap"  width="100%"></table>                        
        </div>
    </div>
 
    
</asp:Content>
