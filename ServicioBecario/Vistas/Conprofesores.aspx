<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="Conprofesores.aspx.cs" Inherits="ServicioBecario.Vistas.Conprofesores" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
    <link href="../scripts/Table2/dataTables.bootstrap.min.css" rel="stylesheet" />
    <link href="../scripts/Table2/responsive.bootstrap.min.css" rel="stylesheet" />

    <script src="../scripts/Table2/responsive.bootstrap.min.js"></script>


      <link rel="stylesheet"  href="../scripts/Enginee/css/validationEngine.jquery.css" type="text/css"/> 
     
    <script type="text/javascript" src="../scripts/Enginee/js/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../scripts/Enginee/js/jquery.validationEngine.js" charset="utf-8"></script>


    <style type="text/css">
       .table th{

            background-color: #3B83C0;
            color: #ffffff;
            border-color: #5d7b9d;
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
        /*  var dato = "asda";
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
          });*/
      }
/**************************************************************************************************************************************************************************************************************************************************/
      function grids()
      {
        /*  var dato = "asda";
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
          });*/
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
          if (id_campus != "")
          {

         
          $.ajax({
              url: 'Conprofesores.aspx/pintarContratos',
              type: 'post',
              contentType: "application/json; charset=utf-8",
              data: '{campus:"' + id_campus + '",profesor:"' + id_profesor + '"}',
              datatype: 'json',
              success: function (datos) {
                  $("#div-pintar").empty();
                  $("#div-pintar").html(datos.d);
                  LimpiarMensaje();
              }
          });
          } else {
              MensajeRojo("No has seleccionado ningun Campus");
              $("#div-pintar").empty();

          }
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
                     // table = $('#example').DataTable();
                      //destruimos la tabla 
                     // table.destroy();
                      //llenarGrid();
                      grids();
                  }                
                  
              }
          });
      }

/**********************************************();****************************************************************************************************/
      function MensajeRojo(valor) {
          $("#error").css({ display: "block" });
          $("#error2").css({ display: "none" });
          $("#error").text(valor);
      }
      /**************************************************************************************************************************************************/

      
      function MensajeVerde(valor) {
          $("#error2").css({ display: "block" });
          $("#error").css({ display: "none" });
          $("#error2").text(valor);
      }
      /**************************************************************************************************************************************************/
      function LimpiarMensaje() {
          $("#error").css({ display: "none" });
          $("#error2").css({ display: "none" });

      }
     
/*******************************************************************************************************************************************************************/
      //con esta funcion detecta la row apartir del id.
      function getRow(table_id, id) {
       /*   var oTable = $('#' + table_id).dataTable(), registro,
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
          return false;*/
      }

 /*****************************************************************************************************************************************************************/
     
      //con esta funcion detecta la row apartir del id.
      function getRows(table_id, id) {
       /*   var oTable = $('#' + table_id).dataTable(), registro,
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
          return false;*/
      }


      /*****************************************************************************************************************************************************************/
      function eliminars(id, contador, idnose) {

          /*Agregamos la seleccion a la tabla */
         // var table = $('#example').DataTable();
          //var row = table.row(contador).node();

          /*var regu = table.row(contador).data();


          /////////////////////////////////////////////////////////////////////////////////
          var ross = getRows('example', id);

          $(ross).addClass('selected');





          /*Preguntamos si se va a eliminar
          var decide = confirm("¿Esta seguro que deseas quitar el registro?");
          if (decide == true) {
              ajaxEliminars(id, contador, ross);
          }
          $(ross).removeClass('selected');*/

      }
/*****************************************************************************************************************************************************************/

      function eliminar(id, contador, idnose) {

          /*Agregamos la seleccion a la tabla 
          var table = $('#example').DataTable();
          //var row = table.row(contador).node();

          var regu = table.row(contador).data();


          /////////////////////////////////////////////////////////////////////////////////
          var ross = getRow('example', id);

          $(ross).addClass('selected');*/





          /*Preguntamos si se va a eliminar
          var decide = confirm("Esta seguro que deseas quitar el registro");
          if (decide == true) {
              ajaxEliminar(id, contador, ross);
          }
          $(ross).removeClass('selected');*/

      }

/*******************************************************************************************************************************************************************/      
      //esta funcion selecciona los registross a eliminar
      function ajaxEliminar(id, contador, ross) {
          // contador = contador - 1;
         /* var table = $('#example').DataTable();
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
          table.row('.selected').remove().draw(false);*/

      }

/**************************************************************************************************************************************************/
      
      //esta funcion selecciona los registross a eliminar
      function ajaxEliminars(id, contador, ross) {
          // contador = contador - 1;
       /*   var table = $('#example').DataTable();
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
          table.row(ross).remove().draw(false);*/
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
     <a class="btn btn-primary btn-sm" id="btnA"><em class="glyphicon glyphicon-plus"></em> Agregar</a>
    <a class="btn btn-primary btn-sm" id="btnB"><em class="glyphicon glyphicon-search"></em> Buscar</a>
    <div class="ContAll">
        
    <div id="ContAg">
    <div class="row">
        <div class="col-md-12">

            <br />
            <div id="error" class="alert alert-danger text-center" style="display:none">
                  
            </div>
            <div id="error2" class="alert alert-success text-center" style="display:none">
                  
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
     </div>
   
    <div id="ContBu">
    <div class="row">
        <div class="col-md-12">
            <%--Tabla con la que cargo todos los registros--%>       
                
             <br /><br />                    
        </div>
        
    </div>
        <div class="row">
            <div class="col-md-4"><asp:DropDownList ID="CampusFiltro" runat="server" OnDataBound="CampusFiltro_DataBound" CssClass="form-control"></asp:DropDownList></div>
            <div class="col-md-1"><asp:Button ID="Filtro" runat="server" Text="Filtro" CssClass="btn btn-primary" OnClick="Filtro_Click" /></div>
        </div>
        <div class="row">
                <div class="col-md-12 ">
                    <br />
                    
                    
                    
                <asp:Panel ID="VerFiltro" runat="server" Visible="true">
                    <asp:GridView ID="DatosGrid" runat="server" AutoGenerateColumns="false" PageSize="20" AllowPaging="true" GridLines="None" CellPadding="4" Visible="true" Width="100%" CssClass="table" OnPageIndexChanging="DatosGrid_PageIndexChanging" OnRowDeleting="DatosGrid_RowDeleting"  >
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775"  />
                        <Columns>
                            <asp:BoundField DataField="id_conse" HeaderText="ID" SortExpression="ID" Visible="false" />                        
                            <asp:BoundField DataField="Tipo_contrato"    HeaderText="Tipo de Contrato" Visible="true" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"  />
                            <asp:BoundField DataField="Descripcion_contrato"    HeaderText="Descripción del contrato" Visible="true" />
                            <asp:BoundField DataField="Campus"    HeaderText="Campus" Visible="true"/>
                            <asp:BoundField DataField="Nombre_puesto"    HeaderText="Puesto" Visible="true" />
                             <asp:TemplateField HeaderText="Eliminar" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:ImageButton ID="imbEliminar"  CommandArgument='<%#Eval("id_conse")  %>'  runat="server" ImageUrl="~/images/EliminarR.png" OnClick="imbEliminar_Click" />
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
</asp:Content>
