<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="SeguimientoAsignacion.aspx.cs" Inherits="ServicioBecario.Vistas.SeguimientoAsignacion" %>
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
    
    <style type="text/css">
            #globo{
                width: 200px;
                height: 100px;
                padding: 15px;
                border-radius: 5px;
                box-shadow: 0 2px 5px #555;
                background-color: #666;
                position: relative;
                color: #FFF;
            }
          
      
       

</style>
    <script>
        $(document).ready(function () {
            $(".imgHelp").mouseover(function () {
                $(".Help").css({"display":"block"});
            });
            $(".imgHelp").mouseout(function () {
                $(".Help").css({ "display": "none" });
            });
        });
        function paraNomina(e, control) {
            var dato;
            key = e.keyCode || e.which;
            tecla = String.fromCharCode(key).toLowerCase();
            letras = "l0123456789";
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
            if (dato.length<=8)
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
  
        function paraMatriculas(e, control) {
            var dato;
            key = e.keyCode || e.which;
            tecla = String.fromCharCode(key).toLowerCase();
            letras = "a0123456789";
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
            if (dato.length <= 8) {
                if (letras.indexOf(tecla) == -1 && !tecla_especial) {
                    return false;
                    //No permite toner el numero
                }
            }
            else {

                if (key == "8" || key == "37" || key == "39" || key == "46") {
                    return true;
                }
                else {
                    return false;
                }

            }

        }

        function datos() {
            var s_usuario = 'informativo';
            $.ajax({
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: '{valor:"' + s_usuario + '"}',
                url: "SeguimientoAsignacion.aspx/pruebaMetodo",
                success: function (dd) {
                    var dato = dd.d
                   // alert(dato);
                   // $("#dos").html(dato);
                }
            });
        }

        function aparecer()
        {
            //si esta visible
            var componente = document.getElementById("globo");
            if(componente.style.display=='block')
            {
                 componente.style.display = 'none';
            }
            else
            {
                componente.style.display = 'block';
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
        
    </script>
     <div class="row">
           <div class="col-md-12">
                <h1 class="text-center">Servicio Becario</h1>
           </div>
       </div>
    <div class="row">
        <div class="col-md-12 text-center">
            <h4>Tablero de seguimiento de asignación</h4>
            
        </div>
    </div>
    <div class="jumbotron">
        <div class="row">
            <div class="col-md-12">
               <fieldset>
                   
                   <legend>
                       <h4 class="text-center">
                       Filtros
                   </h4>
                   </legend>
               </fieldset>
            </div>
        </div>
        <div class="row">
            <div class="col-md-1">
                <label>Periodo</label>
            </div>
            <div class="col-md-3">
                <asp:DropDownList ID="ddlPeriodo" CssClass="form-control" runat="server" OnDataBound="ddlPeriodo_DataBound"></asp:DropDownList>
            </div>
            <div class="col-md-2">
                <label>Nivel académico</label>
            </div>
            <div class="col-md-3">
                <asp:DropDownList ID="ddlNivelAcademico" CssClass="form-control" runat="server" OnDataBound="ddlNivelAcademico_DataBound" ></asp:DropDownList>
            </div>
          </div>
        <br />
        <div class="row">
            <div class="col-md-1">
                <label>Campus</label>
            </div>
            <div class="col-md-3">
                <table><tr><td>
                    <asp:DropDownList ID="ddlCampus" CssClass="form-control validate[required]" runat="server" OnDataBound="ddlCampus_DataBound"></asp:DropDownList> <asp:Label runat="server" ID="lblCampus"></asp:Label></td><td>
                <img src="../images/icono-question.png" width="30" height="30" class="imgHelp" /><div class="Help">Si requieres un reporte multicampus solicítalo a tu DBA</div>
                </td></tr>
                </table>
                

               
               
                <asp:HiddenField runat="server" ID="hdfMostrarId"/>
                  <asp:HiddenField runat="server" ID="hdfActivarRol"/>
            </div>
            <div class="col-md-1">
                <label>Matrícula</label>
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtMatricula" placeholder="A00000000" CssClass="form-control validate[validate,custom[matricula]]" OnKeyPress="return paraMatriculas(event,this);"  runat="server" OnTextChanged="txtMatricula_TextChanged" AutoPostBack="true"></asp:TextBox>
            </div>
            <div class="col-md-1">
                <label>Nómina</label>
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtNomina" placeholder="L00000000" CssClass="form-control validate[validate,custom[nomina]]" OnKeyPress = "return paraNomina(event,this);" runat="server" OnTextChanged="txtNomina_TextChanged" AutoPostBack="true"></asp:TextBox>
            </div>
            <div class="col-md-1">
                <asp:Button ID="btnFiltrar" runat="server" OnClientClick="validar();" Text="Filtrar" CssClass="btn btn-primary"  OnClick="btnFiltrar_Click" />
            </div>
            <div class="col-md-1">
                <asp:ImageButton ID="IbtnExportar"  AlternateText="Excel" ToolTip="Descargar a excel" runat="server" ImageUrl="~/images/Excel.jpg" OnClick="IbtnExportar_Click" />
            </div>
        </div>           
    </div>    
    <div class="row table-condensed">
        <div class="col-md-12 col-sm-12 col-xs-12 ">
           <div class="table-responsive">
              <%-- <table class="table">--%>
               <%--<div class="table">--%>
                   <asp:Panel ID="pnlVisualizatablero" CssClass="hscrollbar"  runat="server" Visible="true">
                        <asp:GridView ID="GvTableroAsignacion" AllowPaging="True" PageSize="50"  runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333"
                            GridLines="None" OnRowCreated="GvTableroAsignacion_RowCreated"
                            CssClass="table" OnPageIndexChanging="GvTableroAsignacion_PageIndexChanging"  >
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="Periodo" HeaderText="Periodo" Visible="true" ControlStyle-Width="10" ControlStyle-CssClass="text-center" >
                                <ControlStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Nivel academico" HeaderText="Nivel académico" Visible="true" ControlStyle-Width="10" ControlStyle-CssClass="text-center" >
                                <ControlStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Campus solicitante" HeaderText="Campus solicitante" Visible="true" ControlStyle-Width="10" ControlStyle-CssClass="text-center" >
                                <ControlStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Nomina" HeaderText="Nómina" Visible="true"  ControlStyle-Width="10" ControlStyle-CssClass="text-center" >
                                <ControlStyle Width="10px" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Empleado" ControlStyle-CssClass="text-center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButtonDetalles" Width="180px" runat="server" AlternateText='<%# Eval("Nombre solicitante") %>' Style="cursor: pointer;" />
                                        <cc1:PopupControlExtender ID="PopupDetalles" runat="server"
                                            DynamicServiceMethod="GetDynamicContent"
                                            DynamicContextKey='<%#Eval("Nomina")%>'
                                            TargetControlID="ImageButtonDetalles"
                                            Position="Right"
                                            DynamicControlID="detallesgridPanel"
                                            PopupControlID="detallesgridPanel">
                                        </cc1:PopupControlExtender>
                                    </ItemTemplate>
                                    <ControlStyle CssClass="text-center" />
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center"  />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Ubicacion fisica" HeaderText="Ubicación fisica" Visible="true" ControlStyle-Width="10" ControlStyle-CssClass="text-center" >
                                <ControlStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Ubicacion alterna" HeaderText="Ubicación alterna" Visible="true" ControlStyle-Width="10" ControlStyle-CssClass="text-center" >
                                <ControlStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Matricula" HeaderText="Matrícula" Visible="true" ControlStyle-Width="10" ControlStyle-CssClass="text-center" >
                                <ControlStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Estatus asignacion" HeaderText="Estatus asignación" Visible="true" ControlStyle-Width="10" ControlStyle-CssClass="text-center" >
                                <ControlStyle Width="10px" />
                                </asp:BoundField>                                
                                <asp:CheckBoxField DataField="Asistencia" HeaderText="Asistencia" ReadOnly="True" SortExpression="Asistencia" ControlStyle-CssClass="text-center" >                                
                                <ControlStyle CssClass="text-center" />
                                </asp:CheckBoxField>
                                <asp:BoundField DataField="Nombre Becario" HeaderText="Becario" Visible="true" ControlStyle-Width="10" >
                                <ControlStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Proyecto" HeaderText="Proyecto" Visible="true" ControlStyle-Width="10" ControlStyle-CssClass="text-center" >
                                <ControlStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Becario calificacion" HeaderText="Becario calificación" Visible="true" ControlStyle-Width="10" ControlStyle-CssClass="text-center" >
                                <ControlStyle Width="10px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Solicitante Calificacion" HeaderText="Calificación empleado" Visible="true" ControlStyle-Width="10" ControlStyle-CssClass="text-center" >
                                <ControlStyle Width="10px" />
                                </asp:BoundField>                                                                                              
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btnLlevarRegistro"  CommandName='<%#Eval("PeriodoID") + "!" +Eval("Matricula")%>' CssClass="btn btn-primary" runat="server" Text="Ver" OnClick="btnLlevarRegistro_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="#5091C8" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#5091C8" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                        </asp:GridView>
                        <asp:Panel ID="detallesgridPanel" runat="server"
                            Style="display: none; background-color: #5099F9; font-size: 11px; border: solid 1px Black;">
                        </asp:Panel>
                    </asp:Panel>
               <br />
               <br />             
               <%--</div>--%>
             <%--  </table>--%>
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
                                    <%--<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>--%>
                                    <h4 class="modal-title">
                                        <asp:Label ID="lblCabeza" runat="server" Text="Este es el titulo"></asp:Label></h4>
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











    
    <%--Confirmacion--%>
    
    <%--Se tiene que copear este face para junto un metodo masa para poderse ejecuatar--%>
    <asp:Label ID="Label15" runat="server" Text=""></asp:Label>
        <!-- ModalPopupExtender -->
        <cc1:ModalPopupExtender ID="mp2" runat="server" PopupControlID="Panel2" TargetControlID="Label15"
            CancelControlID="cancel1" BackgroundCssClass="modalBackground1">
        </cc1:ModalPopupExtender>
        <asp:Panel ID="Panel2" runat="server"  align="center" style = "display:none">
             <div class="modal-dialog">
                                <div class="modal-content" style="background-color:#E6E6E6">
                                  <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                    <h4 class="modal-title">
                                        <asp:Label ID="lblCabeza2" Font-Names="open sans" Font-Size="13px"  runat="server" Text="Este es el titulo"></asp:Label></h4>
                                  </div>
                                  <div class="modal-body">
                                        <div class="col-md-1">
                                            <asp:Image ID="Image2" runat="server" ImageUrl="~/images/Alertaz.png" />
                                        </div>
                                        <div class="col-md-11">
                                            <asp:Label ID="lblcuerpo2" runat="server" Font-Names="open sans" Font-Size="13px" Text=""></asp:Label>&hellip;
                                        </div>     
                                      <asp:HiddenField ID="hdfidCancelacion" runat="server" />                             
                                  </div>
                                  <div class="modal-footer">
                                      <asp:Button ID="btncanll" style="background-color:#1D4860;color:#F6F6F6" runat="server" OnClick="btncanll_Click" class="btn" Text="Aceptar" />
                                    <button type="button" id="cancel1" class="btn" style="background-color:#1D4860;color:#F6F6F6" data-dismiss="modal">Cerrar</button>        
                                  </div>
                                </div><!-- /.modal-content -->
                              </div><!-- /.modal-dialog -->
            
        </asp:Panel>
 <%--   Fin de la face del modal--%>





    <style>
        #ContentPlaceHolder1_detallesgridPanel{
               background-color:#FFF;
           }
    </style>
    <script type="text/javascript">
    $(document).ready(function(){
        $("#ContentPlaceHolder1_detallesgridPanel").css({
            "background-color": "#FFF",
            "border": "1px solid #eee",
            "box-shadow": "2px 2px 2px #eee",
            "-webkit-box-shadow":" 2px 2px 5px #eee",
            "-moz-box-shadow": "2px 2px 5px #eee"
        });
    });

    </script>
</asp:Content>
