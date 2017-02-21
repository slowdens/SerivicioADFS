<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="EspecificaIndividual.aspx.cs" Inherits="ServicioBecario.Vistas.EspecificaIndividual" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>

    <!-- JQuery UI-->    
    <link rel="Stylesheet" href="../scripts/jquery/jquery-ui.css" />
    <script src="../scripts/jquery/jquery-ui.js" ></script>
 
    <!-- Validate CSS -->
    <link rel="stylesheet" type="text/css" href="../scripts/Enginee/css/validationEngine.jquery.css"/>

    <!-- Validation Engine -->    
    <script type="text/javascript" src="../scripts/Enginee/js/jquery.validationEngine-es.js"></script>    
    <script type="text/javascript" src="../scripts/Enginee/js/jquery.validationEngine.js"></script>
    
    <script>
        $(document).ready(function () {

            // iniciamos una llamada AJAX
            $.ajax({
                type: "POST",
                url: "../Servicios/WebService2.asmx/ObtenerLenguajes",// lugar donde se aloja nuestro web service
                dataType: "json",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    // separamos el resultado por el caracter ":"
                    var dataFromServer = data.d.split(":");
                    $("[id$=Matricula]").autocomplete({
                        source: dataFromServer
                    });
               
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        })




        function Complete() {

            var elem = $("#<%=Matricula.ClientID%>").val();
            if (elem.length >= 6) {
                $.ajax({
                    type: "POST",
                    url: "../Servicios/WebService2.asmx/ObtenerLenguajes",// lugar donde se aloja nuestro web service
                    dataType: "json",
                    data: "{}",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        // separamos el resultado por el caracter ":"
                        var dataFromServer = data.d.split(":");
                        $(".matricula-autocomplete").autocomplete({
                            source: dataFromServer
                        });
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert(textStatus);
                    }
                });
            }
        }
    </script>

    <script type="text/javascript">
        //       $(function () {
        $(document).ready(function () {
            // Initiate the validation engine.
            $('#form1').validationEngine();
          
        });
    </script>

    <div class=" container-fluid">
        <div class="row"><div class="col-md-12 ">&nbsp;</div></div>
        <div class="row">
            <div class="col-md-12 " >
                 <h1 class="text-center">Servicio Becario</h1>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 " >
                 <h4 class="text-center">Solicitud Individual</h4>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 ">
                <asp:Label ID="ErrViewer" runat="server" Text="" Visible="true"></asp:Label>                 
            </div>
        </div>
        <div class="jumbotron">
            <div class="form-group table-responsive">
                <div class="row" >
                    <div class="col-md-1 " ></div>
                    <div class="col-md-2 " >
                        <label for="Solicitante" class="control-label">Solicitante</label>
                    </div>
                    <div class="col-md-3 " >
                        <asp:Label ID="NombreSolicitante" runat="server" Text="Label" CssClass="control-label">Nombre del solicitante</asp:Label>
                    </div>
                    <div class="col-md-2 " >
                        <label for="UbicacionFisica" class="control-label">Ubicación física</label>
                    </div>
                    <div class="col-md-3 " >
                        <asp:Label ID="DatosUbicacion" runat="server" Text="Label" CssClass="control-label">Datos del solicitante</asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-1 " ></div>
                    <div class="col-md-2 " >
                        <label for="Nomina" class="control-label">Nómina</label>
                    </div>
                    <div class="col-md-3 " >
                        <asp:Label ID="DatosNomina" runat="server" Text="Label" CssClass="control-label">Datos de nómina</asp:Label>
                    </div>
                    <div class="col-md-2 " >
                        <label for="Puesto" class="control-label">Puesto</label>
                    </div>
                    <div class="col-md-3 " >
                        <asp:Label ID="DatosPuesto" runat="server" Text="Label" CssClass="control-label">Datos de puesto</asp:Label>
                    </div>
                </div>
                <div class="row" >
                    <div class="col-md-1 " ></div>
                    <div class="col-md-2 " >
                        <label for="Departamento" class="control-label">Departamento</label>
                    </div>
                    <div class="col-md-3 " >
                        <asp:Label ID="DatosDepartamento" runat="server" Text="Label" CssClass="control-label">Datos de departamento</asp:Label>
                    </div>
                    <div class="col-md-2 " >
                        <label for="Departamento" class="control-label">Correo</label>
                    </div>
                    <div class="col-md-3 " >
                        <asp:Label ID="DatosCorreo" runat="server" Text="Correo" CssClass="control-label">Datos de correo</asp:Label>
                    </div>
                </div>
                <div class="row" >
                    <div class="col-md-1 " ></div>
                    <div class="col-md-2 " >
                        <label for="Departamento" class="control-label">Campus</label>
                    </div>
                    <div class="col-md-3 " >
                        <asp:Label ID="DatosCampus" runat="server" Text="Label" CssClass="control-label">Datos del campus</asp:Label>
                        <asp:Label ID="IdCampus" runat="server" Text="Label" CssClass="control-label" Visible="false">Id campus</asp:Label>
                    </div>
                    <div class="col-md-2 " >
                        <label for="DatosCampus" class="control-label">División</label>
                    </div>
                    <div class="col-md-3 " >
                        <asp:Label ID="DatosDivision" runat="server" Text="Correo" CssClass="control-label">Datos de la división</asp:Label>
                    </div>
                    <div class="col-md-1 " >
                        <asp:Label ID="solicitanteCampus" runat="server" Text="Campus" CssClass="control-label" Visible ="false">Datos de campus</asp:Label>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 center-block text-center" >
                <table id ="TblAlta" runat="server" class="table-responsive table">
                    <tr>
                        <td>
                            <div class="row"><div class="col-md-12 ">&nbsp;</div></div>
                            <div class="row">
                                <div class="col-md-1 " >
                                </div>
                                <div class="col-md-2 text-center" >
                                    <label for="Periodo" class="control-label">Periodo</label>
                                </div>
                                <div class="col-md-3 text-center" >
                                    <asp:DropDownList ID="CbPeriodos" runat="server" CssClass=" input-group form-control validate[required]"></asp:DropDownList>
                                </div>
                                <div class="col-md-2 text-center" >
                                    <label for="Periodo" class="control-label"><a href="#" data-toggle="popover" data-placement="left" title="Becarios requeridos" data-content="Es la cantidad de becarios que se requieren para esta solicitud">Becarios Requeridos</a></label>
                                </div>
                                <div class="col-md-2 text-center" >
                                    <asp:DropDownList ID="CbTotalBecarios" runat="server" CssClass=" input-group form-control validate[required]" ToolTip ="Es la cantidad de becarios necesarios para esta solicitud"></asp:DropDownList>
                                </div>
                                <div class="col-md-1 " >
                                </div>
                            </div>
                            <div class="row"><div class="col-md-12 ">&nbsp;</div></div>
                            <div class="row">
                                <div class="col-md-12 center-block text-center" >
                                    <asp:Table ID="TblCampusAlterno" runat="server" HorizontalAlign="Center" Visible="false" Width="250px">
                                        <asp:TableRow HorizontalAlign="Center">
                                            <asp:TableCell HorizontalAlign="Right" Width="100px">
                                                <asp:Label ID="Campus" runat="server" Text="Campus " CssClass="control-label"></asp:Label> 
                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Center" Width="50px">&nbsp;</asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Left" Width="200px">
                                                <asp:DropDownList ID="campusAlterno" runat="server" CssClass=" input-group form-control"></asp:DropDownList>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </div>
                            </div>  
                            <div class="row"><div class="col-md-12 ">&nbsp;</div></div>
                            <div class="row">
                                <div class="col-md-12 text-center" >
                                    <asp:Button ID="BtnDatos" runat="server" Text="Capturar" OnClick="BtnDatos_Click" CssClass="btn btn-primary"/>
                                </div>                
                            </div>              
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 " >
                &nbsp;
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 ">
                <table id ="TblCaptura" runat="server" visible ="false" class="table-responsive table">
                    <tr>
                        <td>
                            <div class="row"><div class="col-md-12 ">&nbsp;</div></div>
                            <div class="row">
                                <div class="col-md-12 text-center" >
                                    <label for="Periodo" class="control-label"><a href="#" data-toggle="popover" data-placement="bottom" title="Alta de Becarios" 
                                        data-content="A continuación, aparece la cantidad de becarios que puedes solicitar de manera individual,
	                                    podrás solicitar un alumno en específico capturando su matrícula o a un alumno de programa 
	                                    académico y de un semestre en específico. Si se requiere mayor cantidad de becarios, se deberá
	                                    de dar de alta un proyecto en la sección *Solicitud por Proyecto* (Sujeto a la aprobación
	                                    de la dirección de Apoyos Financieros del Campus)                       
                                        ">Becarios Requeridos</a></label>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-12 text-center " >
                                    <label for="Solicitud" class="control-label"></label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12 text-center" >
                                    <asp:TextBox ID="TxtSolicitud" runat="server" Enabled ="False" visible ="false"></asp:TextBox>
                                    <asp:TextBox ID="TxtEstatusSolicitud" runat="server" Text="" Visible="false"></asp:TextBox>  
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12 text-center" >
                                    <asp:Label ID="NoBecarios" runat="server" Text="Becario 1" ></asp:Label>
                                </div>
                            </div>
                            <div class="row"><div class="col-md-12 ">&nbsp;</div></div>
                            <div class="row">
                                <div class="col-md-12 text-center" >
                                    <label for="Periodo" class="control-label">En caso de querer un alumno en específico, teclee la matrícula</label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 " >
                                </div>
                                <div class="col-md-6 text-center" >
                                    <asp:TextBox ID="Matricula" runat="server" placeholder="Ingrese la matrícula" CssClass="form-control" ></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-3 " >
                                </div>
                            <div class="row"><div class="col-md-12 ">&nbsp;</div></div>
                            <div class="row">
                                <div class="col-md-12 text-center center-block">
                                    <asp:CheckBox ID="otroalumno" runat="server"  Checked="true" Text="&nbsp;&nbsp;Si no esta disponible o no cumple con los criterios solicitados, "/>
                                </div>
                                <div class="col-md-12 text-center center-block">
                                     <label for="Nivel" class="control-label text-center">estoy de acuerdo que se me asigne cualquier otro alumno</label>
                                </div>
                            </div>
                            <div class="row"><div class="col-md-12 ">&nbsp;</div></div>
                            <div class="row">
                                <div class="col-md-12 text-center center-block" >
                                    <asp:Table ID="TblOtro" runat="server" Visible="true" HorizontalAlign="Center" Width="100%">
                                        <asp:TableRow Width="100%">
                                            <asp:TableCell HorizontalAlign="Right" Width="50%">
                                                <label for="Nivel" class="control-label text-right">Nivel &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Left" Width="50%">
                                                <asp:DropDownList OnSelectedIndexChanged="CbNivel_SelectedIndexChanged" AutoPostBack="true" ID="CbNivel" runat="server" CssClass=" input-group form-control" Width="180px"></asp:DropDownList>
                                            </asp:TableCell>
                                        </asp:TableRow>    
                                        <asp:TableRow Width="100%">
                                            <asp:TableCell HorizontalAlign="Right" Width="50%">
                                                <label for="Programa" class="control-label text-right">Programa&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Left" Width="60%">
                                                <asp:DropDownList ID="CbPrograma" runat="server" CssClass=" input-group form-control" Width="180px" OnSelectedIndexChanged="CbPrograma_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            </asp:TableCell>
                                        </asp:TableRow>    
                                        <asp:TableRow Width="100%">
                                            <asp:TableCell HorizontalAlign="Right" Width="50%">
                                                <label for="Periodo" class="control-label text-right">Semestre Cursado&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Left" Width="60%">
                                                <asp:DropDownList ID="CbPeriodo" runat="server" CssClass=" input-group form-control" Width="180px" ></asp:DropDownList>
                                            </asp:TableCell>
                                        </asp:TableRow>                   
                                    </asp:Table>
                                </div>
                            </div>
                            <div class="row"><div class="col-md-12 ">&nbsp;</div></div>
                            <div class="row">
                                <div class="col-md-12 text-center" >
                                    <label for="Nivel" class="control-label text-center">Funciones del becario</label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 " >
                                </div>
                                <div class="col-md-6 text-center" >
                                    <asp:TextBox ID="Funciones" runat="server" CssClass="form-control" placeholder="Describe aquí las principales funciones a realizar por el becario" Height="36px"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row"><div class="col-md-12 ">&nbsp;</div></div>
                            <div class="row">
                                <div class="col-md-12 text-center center-block">
                                    <asp:Button ID="Agregar" runat="server" Text="Guardar Becario" CssClass="btn btn-primary" Width="150px" OnClick="Agregar_Click" />
                                </div>
                            </div>
                            <div class="row"><div class="col-md-12 ">&nbsp;</div></div>
                            <div class="row table-condensed">

                                <div class="col-md-12">
        	                        <asp:Panel ID="pnlxx" runat="server" CssClass="hscrollbar">
                                        <asp:GridView ID="GrdBecarios" runat="server" AutoGenerateColumns="False" OnRowDataBound="GrdBecarios_RowDataBound" OnRowCommand="GrdBecarios_RowCommand"
                                    
                                        CssClass="table" Width="100%" runat="server" CellPadding="4" ForeColor="#333333"  GridLines="None" PageIndex="10">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775"  />
                                        <columns>
                                            <asp:boundfield ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" datafield="ConsecutivoId" headertext="ConsecutivoId"/>
                                            <asp:boundfield ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" datafield="Matricula" headertext="Matricula"/>
                                            <asp:boundfield ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" datafield="Becario" headertext="Becario"/>
                                            <asp:boundfield ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" datafield="Otro" headertext="Otro Alumno"/>
                                            <asp:boundfield ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" datafield="Programa" headertext="Programa"/>
                                            <asp:boundfield ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" datafield="Periodo" headertext="Semestre Cursado"/>
                                            <asp:boundfield ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" datafield="Nivel" headertext="Nivel"/>
                                            <asp:buttonfield ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"  buttontype="Image" commandname="Eliminar" text="Cancelar" ImageUrl="~/images/Eliminar.png" HeaderText="Cancelar"/>
                                        </columns>
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
                            <div class="col-md-3 center-block text-center" >
                            </div>
                            </div>
                            <div class="row"><div class="col-md-12 ">&nbsp;</div></div>
                            <div class="row">
                                <div class="col-md-12 text-center " >
                                    <label for="Nivel" class="control-label text-center">Si desea que el(los) becario(s) se presente(n) en una ubicación diferente, favor de especificar</label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 " >
                                </div>
                                <div class="col-md-6 text-center" >
                                    <asp:TextBox ID="TxtUbicacionAlterna" runat="server" CssClass=" form-control" placeholder="Ubicacion necesaria" Height="36px"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12 text-center " >
                                    <label for="Nivel" class="control-label text-center">NOTA: Favor de detallar con claridad que edifico, taller, aula, laboratorio, etc.</label>
                                </div>
                            </div>
                            <div class="row"><div class="col-md-12 ">&nbsp;</div></div>
                            <div class="row">
                                <div class="col-md-12 text-center center-block">
                                    <asp:CheckBox ID="ChkAcuerdo" runat="server" AutoPostBack="false" Text="&nbsp;&nbsp;Acepto y estoy de acuerdo con el " Enabled="false"/>
                                </div>
                                <div class="col-md-12 text-center center-block">
                                     <label for="Nivel" class="control-label text-center">
                                         <%--<a href="http://sitios.itesm.mx/va/reglamentos/regapedybec_novo.pdf" data-toggle="popover" data-placement="left" target="_blank">Reglamento del servicio de becario</a></label>--%>
                                         <a id="reglaelegida" data-toggle="popover" data-placement="left" >Reglamento del servicio de becario</a></label>
                                </div>
                            </div>
                            <div class="row"><div class="col-md-12 ">&nbsp;</div></div>
                            <div class="row">
                                <div class="col-md-12 text-center center-block">
                                    <asp:Button ID="Enviar" runat="server" Text="Enviar" CssClass="btn btn-primary" OnClick="Enviar_Click"/>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12 text-center" >
                                    &nbsp;
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12 text-center" >
                                    &nbsp;
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12 " >
                                    <asp:TextBox ID="TxtNumeroBecarios" runat="server" Visible ="false"></asp:TextBox>
                                        </div>
                                        </div>
                                    </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="row"><div class="col-md-12 ">&nbsp;</div></div>
                            <div class="row">
                                <div class="col-md-12 center-block text-center" >
                                    <asp:Table ID="TblResultado" runat="server" HorizontalAlign="Center" Visible="false" Width="100%">
                                        <asp:TableRow HorizontalAlign="Center">
                                            <asp:TableCell HorizontalAlign="Center">
                                                <asp:Label ID="NoSolicitud" runat="server" Text="Se ha registrado la solicitud con el folio " CssClass="control-label"></asp:Label> 
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow HorizontalAlign="Center">
                                            <asp:TableCell HorizontalAlign="Center">
                                                <asp:Label ID="TotalRegistrados" runat="server" Text="total" CssClass="control-label"></asp:Label>            
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow HorizontalAlign="Center">
                                            <asp:TableCell HorizontalAlign="Center">
                                                &nbsp;
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow HorizontalAlign="Center">
                                            <asp:TableCell HorizontalAlign="Center">&nbsp;</asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow HorizontalAlign="Center" Width="100%">
                                            <asp:TableCell>
                                                <div class="row table-condensed">
                                                    <div class="col-md-12">
        	                                            <asp:Panel ID="Panel2" runat="server" CssClass="hscrollbar">
                                                            <asp:GridView ID="GrdResultado"  runat="server" HorizontalAlign="Center" AllowPaging="True"
                                                                CssClass="table " Width="100%" CellPadding="4" ForeColor="#333333" 
                                                                 GridLines="None" PageIndex="10" 
                                                                   HeaderStyle-CssClass="text-center"
                                                                    HeaderStyle-HorizontalAlign="Center"
                                                                   >
                                                               
                                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775"/>

                                                                <EditRowStyle BackColor="#999999" />
                                                                <FooterStyle BackColor="#3B83C0" Font-Bold="True" ForeColor="White" CssClass="letra" />
                                                                <HeaderStyle BackColor="#3B83C0" Font-Bold="True" ForeColor="White" CssClass="letra" />
                                                                <PagerStyle BackColor="#3B83C0" ForeColor="White"/>
                                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333"/>
                                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                                                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                                                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                                                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
		                                                    </asp:GridView>
	                                                    </asp:Panel>        
                                                    </div>
                                                </div>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </div>
                            </div>
        
        <div id="TblOculta" runat="server">
                        <div class="row">
                            <div class="col-md-12 text-center" >
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12 text-center" >
                                <asp:TextBox ID="TxtBoxNominaEmpleado" runat="server" Text="" Visible="false"></asp:TextBox>  
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 text-center" >
                                <asp:Label ID="DatosCorreo1" runat="server" Text="" Visible="false"></asp:Label>  
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 text-center" >
                                <asp:TextBox ID="TxtBoxNombre" runat="server" Text="" Visible="false"></asp:TextBox>  
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 text-center" >
                                <asp:TextBox ID="TxtBoxJustificacion" runat="server" Text="" Visible="false"></asp:TextBox>  
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 text-center" >
                                <asp:TextBox ID="TxtNumeroProgramas" runat="server" Text="" Visible="false"></asp:TextBox>  
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 text-center" >
                                <asp:TextBox ID="NombreSolicitante1" runat="server" Text="" Visible="false"></asp:TextBox>  
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 text-center" >
                                <asp:TextBox ID="TxtBoxSolicitud" runat="server" Text="" Visible="False"></asp:TextBox>  
                            </div>
                        </div>
        <div class="row">
                            <div class="col-md-12 text-center" >
                                <asp:Label ID="Mensaje" runat="server" Text="" Visible="false"></asp:Label>  
                            </div>
                        </div>

            <div class="row">
                <div class="col-md-12 ">&nbsp;</div>

            </div>
            </div>
        </div><!--Aqui finaliza div principal-->
    
    <%--Se tiene que copiar este face para junto un metodo masa para poderse ejecuatar--%>
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

    <script>
        $(document).ready(function(){
            $('[data-toggle="popover"]').popover();

            tomarLink();


        });

        function tomarLink() {
            var Menu = "";
            $.ajax({
                url: 'EspecificaIndividual.aspx/tomaLink',
                type: 'post',
                contentType: "application/json; charset=utf-8",
                data: '{colection:"' + Menu + '"}',
                datatype: 'json',
                success: function (datos) {
                    var dat = datos.d;
                    $("#reglaelegida").attr({
                        "href": dat,
                        "target":"_blank"
                    });
                }
            });
        }

    </script>
     <style>
        #ui-id-1{
            z-index:1000;
        }
    </style> 



</asp:Content>
