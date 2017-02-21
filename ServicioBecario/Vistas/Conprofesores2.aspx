<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="Conprofesores2.aspx.cs" Inherits="ServicioBecario.Vistas.Conprofesores2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../scripts/Datatablet/jquery.dataTables.min.css" rel="stylesheet" />
   
    <link href="../scripts/Table2/dataTables.bootstrap.min.css" rel="stylesheet" />
    <link href="../scripts/Table2/responsive.bootstrap.min.css" rel="stylesheet" />

  
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
        .rellanerverde{
            background-color:green;
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
                /*font-size: 15px;*/
                font-weight: normal;
                vertical-align: middle;
                cursor: pointer;
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
/**************************************************************************************************************************************************************************************************************************************************/
      $(document).ready(function () {
          llenarCampus();
          llenarTipoProfesor();
          //llenarGrid();
          grids();
      });
/**************************************************************************************************************************************************************************************************************************************************/
      function llenarCampus()
      {
          var Menu = "sd";
          $.ajax({
              url: 'Conprofesores.aspx/llenarcampus',
              type: 'post',
              contentType: "application/json; charset=utf-8",
              data: '{menu:"' + Menu + '"}',
              datatype: 'json',
              success: function (datos) {
                  $("#ddlCampus").html(datos.d);
              }
          });
      }
/**************************************************************************************************************************************************************************************************************************************************/
      function llenarGrid() {
          var dato = "asda";
          $.ajax({
              url: 'Conprofesores.aspx/inicio',
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
                          { title: "Tipo contrato" },
                          { title: "Descripcion" },
                          { title: "Campus" },
                          { title: "Puesto" },                          
                          {

                              "width": "20%",
                              "render": function (datas, type, full, meta) {
                                  var buttonID = "rollover_" + full.id;
                                  //deplega el boton eliminar
                                  //return "<a id=" + buttonID + " class='btn btn-primary rolloverBtn' role='button'    onclick='eliminar(\"" + ds[datas][5] + "\"," + datas + ",\"" + full[datas] + "\");' ><img src='../images/EliminarR.png' width='30px' height='30px'   /></a>";
                                  return "<a id=" + buttonID + "  role='button'    onclick='eliminar(\"" + ds[datas][5] + "\"," + datas + ",\"" + full[datas] + "\");' ><img src='../images/EliminarR.png' width='30px' height='30px'   /></a>";
                              }
                          }

                      ]
                  });
              }
          });
      }
/**************************************************************************************************************************************************************************************************************************************************/
      function grids()
      {
          var dato = "asda";
          $.ajax({
              url: 'Conprofesores.aspx/iniciouno',
              type: 'post',
              contentType: "application/json; charset=utf-8",
              data: '{nomina:"' + dato + '"}',
              datatype: 'json',
              success: function (dd) {
                  var dataset = dd.d

                  var ds = jQuery.parseJSON(dataset);
                  var dss = ds;
                  tablita = $("#example").DataTable({
                      data: ds,
                      "language": {
                          "url": "http://cdn.datatables.net/plug-ins/1.10.11/i18n/Spanish.json"
                      },
                      "bSort": false,
                      responsive: true,
                      columns: [
                          { "title": "Tipo Contrato", "data": "Tipo contrato" },
                          { "title": "Descripcion", "data": "Descripcion" },
                          { "title": "Campus","data": "Campus" },
                          { "title": "Puesto", "data": "Puesto" },
                          {

                              "width": "20%",
                              "render": function (datas, type, full, meta) {
                                  var buttonID = "rollover_" + full.id;
                                  //deplega el boton eliminar
                                  //return "<a id=" + buttonID + " class='btn btn-primary rolloverBtn' role='button'    onclick='eliminars(\"" + full.id_conse + "\"," + datas + ",\"" + full[datas] + "\");' ><img src='../images/EliminarR.png' width='30px' height='30px'   /></a>";
                                  return "<a id=" + buttonID + "  role='button'    onclick='eliminars(\"" + full.id_conse + "\"," + datas + ",\"" + full[datas] + "\");' ><img src='../images/EliminarR.png' width='30px' height='30px'   /></a>";
                              }
                          }

                      ]
                  });
              }
          });
      }
/**************************************************************************************************************************************************************************************************************************************************/
      function llenarTipoProfesor()
      {
          var Menu = "sd";
          $.ajax({
              url: 'Conprofesores.aspx/llenarprofesor',
              type: 'post',
              contentType: "application/json; charset=utf-8",
              data: '{menu:"' + Menu + '"}',
              datatype: 'json',
              success: function (datos) {
                  $("#ddlprofesor").html(datos.d);
              }
          });
      }
/**************************************************************************************************************************************************************************************************************************************************/
      function pintarContratos()
      {
          var id_campus = $("#ddlCampus").val();
          var id_profesor = $("#ddlprofesor").val();
         
          $.ajax({
              url: 'Conprofesores.aspx/pintarContratos',
              type: 'post',
              contentType: "application/json; charset=utf-8",
              data: '{campus:"' + id_campus + '",profesor:"' + id_profesor + '"}',
              datatype: 'json',
              success: function (datos) {
                  $("#div-pintar").empty();
                  $("#div-pintar").html(datos.d);
              }
          });
      }
/**************************************************************************************************************************************************************************************************************************************************/
      function validarCamposGuardar()
      {
          LimpiarMensaje();
          var id_campus = $("#ddlCampus").val();
          var id_profesor = $("#ddlprofesor").val();
          if(id_campus!="")
          {
              if (id_profesor != "")
              {
                  var strcadena = "";
                  $("input:checkbox:checked").each(function () {
                      strcadena = strcadena + $(this).val() + "!";
                  });
                  if(strcadena!="")
                  {
                      guardarComponentes();
                  }
                  else
                  {
                      MensajeRojo("No has seleccionado algun contrato");
                  }
              }
              else
              {
                  MensajeRojo("No has seleccionado el tipo de profesor");
              }
          }
          else
          {
              MensajeRojo("No has seleccionado el campus");

          }

      }
/**************************************************************************************************************************************************************************************************************************************************/
      function guardarComponentes()
      {
          var strcadena = "";
          $("input:checkbox:checked").each(function () {
              strcadena = strcadena + $(this).val() + "!";
          });
          var str = strcadena;
          var id_campus = $("#ddlCampus").val();
          var id_profesor = $("#ddlprofesor").val();
          var table;
          $.ajax({
              async: false,
              type: "POST",
              dataType: "json",
              contentType: "application/json; charset=utf-8",
              data: '{seleccionados:"' + strcadena + '",id_campus:"' + id_campus+ '",id_profesor:"'+id_profesor+'"}',
              url: "Conprofesores.aspx/guardar",
              success: function (dd) {
                  var dato = dd.d
                  //Limpiamos la tabla
                  if (dato == "Ok")
                  {
                      pintarContratos();
                      MensajeVerde('Se guardaron correctamente los datos');
                      table = $('#example').DataTable();
                      //destruimos la tabla 
                      table.destroy();
                      //llenarGrid();
                      grids();
                  }                
                  
              }
          });
      }

/**********************************************();****************************************************************************************************/
      function MensajeRojo(valor) {
          $("#error").css({ display: "block" });
          $("#error").removeClass("rellanerverde text-center");
          $("#error").addClass("rellenar text-center");
          $("#error").text(valor);
      }
      /**************************************************************************************************************************************************/

      
      function MensajeVerde(valor) {
          $("#error").css({ display: "block" });
          $("#error").removeClass("rellenar text-center");
          $("#error").addClass("rellanerverde text-center");
          $("#error").text(valor);
      }
      /**************************************************************************************************************************************************/
      function LimpiarMensaje() {
          $("#error").css({ display: "none" });

      }
     
/*******************************************************************************************************************************************************************/
      //con esta funcion detecta la row apartir del id.
      function getRow(table_id, id) {
          var oTable = $('#' + table_id).dataTable(), registro,
              data = oTable.fnGetData(),
              row, i, l = data.length,tama=0;
          tama = l - 1;
          var table = $('#example').DataTable();
          var tes;
          for (i = 0; i < l; i++) {
              row = data[i];

              var strinf = row[7];
              // columns to search are hard-coded, but you could easily pass this in as well
              var rsss = row[5];
              if (tama == i)
              {
                  if (row[5] == id) {
                      //return $('#' + table_id + ' tr').eq((i));
                      registro = table.row("#row_" + id)
                      //return registro;
                  }
              }
              else
              {
                  if (row[5] == id) {
                      return $('#' + table_id + ' tr').eq((i + 1));
                      //registro = table.row("#row-" + i)
                      //return registro;
                  }
              }
              
          }
          return false;
      }

 /*****************************************************************************************************************************************************************/
     
      //con esta funcion detecta la row apartir del id.
      function getRows(table_id, id) {
          var oTable = $('#' + table_id).dataTable(), registro,
              data = oTable.fnGetData(),
              row, i, l = data.length, tama = 0;
          tama = l - 1;
          var table = $('#example').DataTable();
          var tes;
          for (i = 0; i < l; i++) {
              row = data[i];

              tes = row.id_conse;
              // columns to search are hard-coded, but you could easily pass this in as well
              var rsss = row[5];
              if (tama == i) {
                  if (row.id_conse == id) {
                      
                      registro = table.row("#" + row.DT_RowId)
                      return registro;
                  }
              }
              else {
                  if (row.id_conse == id) {
                      
                      registro = table.row("#" + row.DT_RowId);
                      return registro;
                  }
              }

          }
          return false;
      }


      /*****************************************************************************************************************************************************************/
      function eliminars(id, contador, idnose) {

          /*Agregamos la seleccion a la tabla */
          var table = $('#example').DataTable();
          //var row = table.row(contador).node();

          var regu = table.row(contador).data();


          /////////////////////////////////////////////////////////////////////////////////
          var ross = getRows('example', id);

          $(ross).addClass('selected');





          /*Preguntamos si se va a eliminar*/
          var decide = confirm("¿Esta seguro que deseas quitar el registro?");
          if (decide == true) {
              ajaxEliminars(id, contador, ross);
          }
          $(ross).removeClass('selected');

      }
/*****************************************************************************************************************************************************************/

      function eliminar(id, contador, idnose) {

          /*Agregamos la seleccion a la tabla */
          var table = $('#example').DataTable();
          //var row = table.row(contador).node();

          var regu = table.row(contador).data();


          /////////////////////////////////////////////////////////////////////////////////
          var ross = getRow('example', id);

          $(ross).addClass('selected');





          /*Preguntamos si se va a eliminar*/
          var decide = confirm("Esta seguro que deseas quitar el registro");
          if (decide == true) {
              ajaxEliminar(id, contador, ross);
          }
          $(ross).removeClass('selected');

      }

/*******************************************************************************************************************************************************************/      
      //esta funcion selecciona los registross a eliminar
      function ajaxEliminar(id, contador, ross) {
          // contador = contador - 1;
          var table = $('#example').DataTable();
          var datosTabla = table.row(ross).data();
       
          var campus_tabla = datosTabla[2];
          var campus_selec = $("#ddlCampus option:selected").text();



          $.ajax({
              url: 'Conprofesores.aspx/eleminardatos',
              type: 'post',
              contentType: "application/json; charset=utf-8",
              data: '{id:"' + id + '"}',
              datatype: 'json',
              success: function (datos) {                  
                  if (datos.d == "Eliminado") {
                      if(campus_tabla===campus_selec)
                      {
                          $("#div-pintar").empty();
                          //agregamos nuevamente los componentes  al div
                          pintarContratos();
                      }
                  }
              }
          });
          //Elinamos el registro correctamente            
          table.row('.selected').remove().draw(false);

      }

/**************************************************************************************************************************************************/
      
      //esta funcion selecciona los registross a eliminar
      function ajaxEliminars(id, contador, ross) {
          // contador = contador - 1;
          var table = $('#example').DataTable();
          var datosTabla = table.row(ross).data();

          var campus_tabla = datosTabla.Campus;
          var campus_selec = $("#ddlCampus option:selected").text();



          $.ajax({
              url: 'Conprofesores.aspx/eleminardatos',
              type: 'post',
              contentType: "application/json; charset=utf-8",
              data: '{id:"' + id + '"}',
              datatype: 'json',
              success: function (datos) {
                  if (datos.d == "Eliminado") {
                      if (campus_tabla === campus_selec) {
                          $("#div-pintar").empty();
                          //agregamos nuevamente los componentes  al div
                          pintarContratos();
                      }
                  }
              }
          });
          //Elinamos el registro correctamente                    
          table.row(ross).remove().draw(false);
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
              <h4>Configuración de contrato de profesores</h4>              
              
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
                <label>Campus</label>
            </div>
            <div class="col-md-3">
                <select id="ddlCampus" onchange="pintarContratos();" class="form-control">
                </select>
            </div>
            <div class="col-md-1">
                <label>Tipo de profesor</label>
            </div>
            <div class="col-md-3">
                <select id="ddlprofesor"   class="form-control">
                </select>
            </div>
            
        </div>
        <div class="row">
            <div class="col-md-12">
                <div id="div-pintar">
                    <%--Pintar respuestas--%>

                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-offset-5 col-md-1">
                <input id="btnGuardar" class="btn btn-primary" type="button" value="Guardar" onclick="validarCamposGuardar();" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">
            <%--Tabla con la que cargo todos los registros--%>       
                
              <table id="example"  class="table table-striped  dt-responsive nowrap"  width="100%">                  
                                                       
              </table>                        
        </div>
        
    </div>
    
</asp:Content>
