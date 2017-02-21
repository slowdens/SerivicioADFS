<%@ Page Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="SolicitudEspeciales.aspx.cs" Inherits="ServicioBecario.Vistas.SolicitudEspeciales" %>
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
          /*  $.ajax({
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
            });*/
        })
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            // Initiate the validation engine.
            $('#form1').validationEngine();
        });
    </script>
    <script type="text/javascript" >
        function SoloNumero(evento) {
            var browser = window.Event ? true : false;
            var key = browser ? evento.keyCode : evento.which;
            return (key <= 13 || (key >= 48 && key <= 57));
        }


        function paranumeros(e) {
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

    </script>
    <script type="text/javascript" >
        function SoloNomina(evento) {
            var browser = window.Event ? true : false;
            var key = browser ? evento.keyCode : evento.which;
            return (key <= 13 || (key >= 48 && key <= 57)) || key == 76 || key == 108;
        }
        function Complete() {
           
            var elem = $("#<%=Matricula.ClientID%>").val();
            if (elem.length >= 6)
                {
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
        }}


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
            if (dato.length <= 8) {
                if (letras.indexOf(tecla) == -1 && !tecla_especial) {
                    return false;
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
    </script>
    <div class=" container-fluid">
        <div class="row">
            <div class="col-md-12 " >
                 <h1 class="text-center">Servicio Becario</h1>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 " >
                 <h4 class="text-center">Solicitud Especial</h4>
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
                    <div class="col-md-1 " >
                    </div>
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
                        <label for="Email" class="control-label">Correo</label>
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
            <div class="col-md-12 text-center" >
                <asp:Label ID="ErrViewer" runat="server" Text="" Font-Bold="True" Font-Italic="True" ForeColor="Red"></asp:Label>
            </div>
        </div>  
        <div class="row">
            <div class="col-md-12 center-block" >
                <table id ="TblAlta" runat="server" class="table-responsive table">
                    <tr>
                        <td>
                            <div class="row"><div class="col-md-12 " >&nbsp;</div></div>
                            <div class="row">
                                <div class="col-md-4 " >
                                </div>
                                <div class="col-md-2 text-center" >
                                    <label for="Periodo" class="control-label" >Periodo</label>
                                </div>
                                <div class="col-md-3 text-center" >
                                    <asp:DropDownList ID="CbPeriodos" runat="server" CssClass=" input-group form-control validate[required]"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row"><div class="col-md-12 " >&nbsp;</div></div>
                            <div class="row">
                                <div class="col-md-12 text-center" >
                                    <asp:Label ID="Label1" runat="server" Text="A continuación deberá registrar la información básica del proyecto a realizar por los becarios"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12 " >
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12 " >
                                    <h4 class="text-center ">Información del Proyecto</h4>
                                </div>
                            </div>
                            <div class="row"><div class="col-md-12 " >&nbsp;</div></div>
                            <div class="row">
                                <div class="col-md-3" >
                                </div>
                                <div class="col-md-2" >
                                    <label for="Periodo" class="control-label" >Nombre del Proyecto</label>
                                </div>
                                <div class="col-md-4" >
                                    <asp:TextBox ID="TxtBoxNombre" placeholder="Servicio Becario" runat="server" CssClass="form-control validate[required]"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3" >
                                </div>
                                <div class="col-md-2" >
                                    <label for="Periodo" class="control-label" >Objetivo</label>
                                </div>
                                <div class="col-md-4" >
                                    <asp:TextBox ID="TxtBoxObjetivo" placeholder="¿Cuál es mi objetivo?" runat="server" CssClass="form-control validate[required]"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3" >
                                </div>
                                <div class="col-md-2 " >
                                    <label for="Periodo" class="control-label" >Justificación</label>
                                </div>
                                <div class="col-md-4" >
                                    <asp:TextBox ID="TxtBoxJustificacion" placeholder="¿Porque lo realizaré?" TextMode="MultiLine" runat="server" CssClass="form-control validate[required]"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3" >
                                </div>
                                <div class="col-md-2 " >
                                    <label for="Periodo" class="control-label" >Funciones</label>
                                </div>
                                <div class="col-md-4" >
                                    <asp:TextBox ID="TxtBoxFunciones" placeholder="¿Cuáles serán la funciones a realizar?" TextMode="MultiLine" runat="server" CssClass="form-control validate[required]"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row"><div class="col-md-12 " >&nbsp;</div></div>
                            <div class="row">
                                <div class="col-md-12 text-center" >
                                    <label for="CantidadBecarios" class="control-label" >Cantidad becarios por asignar</label>
                                </div>
                                <div class="col-md-3"></div>
                            </div>
                            <div class="row"><div class="col-md-12 " >&nbsp;</div></div>
                            <div class="row">
                                <div class="col-md-3"></div>
                                <div class="col-md-2 " >
                                    <label for="totalMatricula" class="control-label" >Cantidad de Matrículas</label>
                                </div>
                                <div class="col-md-4" >
                                    <asp:TextBox ID="totalMatricula" runat="server" CssClass="form-control validate[required]" Text="0" onkeypress="return paranumeros(event)"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3"></div>
                                <div class="col-md-2 " >
                                    <label for="ProgPre" class="control-label" > Cantidad programa/semestre</label>
                                </div>
                                <div class="col-md-4 " >
                                    <asp:TextBox ID="totalPeriodo" runat="server" CssClass="form-control validate[required]" Text="0" onkeypress="return paranumeros(event)"></asp:TextBox>
                                </div>
                            </div>                            
                            <div class="row">
                                <div class="col-md-3" >
                                </div>
                                <div class="col-md-2" >
                                    <label for="lblNominaEmpleado" class="control-label" >Nómina del Empleado Responsable</label>
                                </div>
                                <div class="col-md-2 " >
                                    <asp:TextBox ID="TxtBoxNominaEmpleado" placeholder="L00000000" runat="server" CssClass="form-control validate[required]" OnKeyPress = "return paraNomina(event,this);"></asp:TextBox>
                                </div>
                                <div class="col-md-2 text-center" >
                                    <asp:Button ID="BtnVerificarNomina"  runat="server" Text="Verificar" CssClass="btn btn-primary" OnClick="BtnVerificarNomina_Click"/>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12 " >
                                </div>
                            </div>
                            <table id="TblAsignado" runat="server" visible ="false" class="table-responsive table">
                                <tr>
                                    <td>
                                        <div class="jumbotron">
                                            <div class="form-group table-responsive">
                                                <div class="row" >
                                                    <div class="col-md-1 " >
                                                    </div>
                                                    <div class="col-md-2 " >
                                                        <label for="Solicitante1" class="control-label">Solicitante</label>
                                                    </div>
                                                    <div class="col-md-3 " >
                                                        <asp:Label ID="NombreSolicitante1" runat="server" Text="Label" CssClass="control-label">Nombre del solicitante</asp:Label>
                                                    </div>
                                                    <div class="col-md-2 " >
                                                        <label for="UbicacionFisica1" class="control-label">Ubicación física</label>
                                                    </div>
                                                    <div class="col-md-3 " >
                                                        <asp:Label ID="DatosUbicacion1" runat="server" Text="Label" CssClass="control-label">Datos del solicitante</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-1 " >
                                                    </div>
                                                    <div class="col-md-2 " >
                                                        <label for="Nomina1" class="control-label">Nómina</label>
                                                    </div>
                                                    <div class="col-md-3 " >
                                                        <asp:Label ID="DatosNomina1" runat="server" Text="Label" CssClass="control-label">Datos de nómina</asp:Label>
                                                    </div>
                                                    <div class="col-md-2 " >
                                                        <label for="Puesto1" class="control-label">Puesto</label>
                                                    </div>
                                                    <div class="col-md-3 " >
                                                        <asp:Label ID="DatosPuesto1" runat="server" Text="Label" CssClass="control-label">Datos de puesto</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="row" >
                                                    <div class="col-md-1 " >
                                                    </div>
                                                    <div class="col-md-2 " >
                                                        <label for="Departamento1" class="control-label">Departamento</label>
                                                    </div>
                                                    <div class="col-md-3 " >
                                                        <asp:Label ID="DatosDepartamento1" runat="server" Text="Label" CssClass="control-label">Datos de departamento</asp:Label>
                                                    </div>
                                                    <div class="col-md-2 " >
                                                        <label for="Correo1" class="control-label">Correo</label>
                                                    </div>
                                                    <div class="col-md-3 " >
                                                        <asp:Label ID="DatosCorreo1" runat="server" Text="Label" CssClass="control-label">Datos de correo</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="row" >
                                                    <div class="col-md-1 " ></div>
                                                    <div class="col-md-2 " >
                                                        <label for="Departamento1" class="control-label">Campus</label>
                                                    </div>
                                                    <div class="col-md-3 " >
                                                        <asp:Label ID="DatosCampus1" runat="server" Text="Label" CssClass="control-label">Datos del campus</asp:Label>
                                                    </div>
                                                    <div class="col-md-2 " >
                                                        <label for="DatosCampus1" class="control-label">División</label>
                                                    </div>
                                                    <div class="col-md-3 " >
                                                        <asp:Label ID="DatosDivision1" runat="server" Text="Correo" CssClass="control-label">Datos de la división</asp:Label>
                                                    </div>
                                                 </div>
                                                <div class="row" >
                                                    <div class="col-md-1 " >
                                                    </div>
                                                    <div class="col-md-2 " >
                                                        <label for="Extension1" class="control-label">Extensión</label>
                                                    </div>
                                                    <div class="col-md-3 " >
                                                        <asp:Label ID="DatosExtension1" runat="server" Text="Label" CssClass="control-label">Datos de extension</asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="row"><div class="col-md-12 " >&nbsp</div></div>
                            <div class="row">
                                <div class="col-md-12 text-center" >
                                    <asp:Button ID="BtnDatos" runat="server" Text="Guardar y Continuar" OnClick="BtnDatos_Click" CssClass="btn btn-primary" Width="250px" Enabled="false"/>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 ">
                <table id ="TblCaptura" runat="server" visible ="false" class="table-responsive table">
                    <tr>
                        <td>
                            <div class="row">
                                <div class="col-md-12 " >
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12 text-center" >
                                    <label for="Periodo"  class="control-label"><a href="#" data-toggle="popover" data-placement="bottom" title="Alta de Becarios" 
                                        data-content="A continuación, aparece la cantidad de becarios que puedes solicitar de manera individual,
	                                    podras solicitar un alumno en específico capturando su matrícula o a un alumno de programa 
	                                    académico y de un semestre en específico. Si se requiere mayor cantida de becarios, se deberá
	                                    de dar de alta un proyecto en la sección *Proyectos Servicio Becario* (Sujeto a la aprobación
	                                    de la dirección de Apoyos Financieros del Campus)                       
                                        "><h3> Becarios Requeridos </h3></a></label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12 " >
                                    <h4 class="text-center">Captura individual</h4>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12 text-center " >
                                    <label for="Solicitud" class="control-label"></label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12 text-center" >
                                    <asp:Label ID="matriculaInstruccion" runat="server" Text="Label" CssClass="control-label">Teclee la matrícula</asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 " ></div>
                                <div class="col-md-6 text-center" >
                                    <asp:TextBox ID="Matricula" runat="server" placeholder="A00000000" CssClass="form-control matricula-autocomplete" OnKeyUP="Complete();"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-3 " ></div>
                            <div class="row"><div class="col-md-12 " >&nbsp</div></div>
                            <div class="row">
                                <div class="col-md-12 text-center center-block">
                                    <asp:CheckBox ID="otroalumno" runat="server" Checked="true" CssClass="control-label text-center" Text="&nbsp;&nbsp;Si no esta disponible o no cumple con los criterios solicitados, "/>
                                </div>
                                <div class="col-md-12 text-center center-block">
                                    <asp:Label ID="textoOtroAlumno" runat="server" Text="Label" CssClass="control-label text-center"><B>estoy de acuerdo que se me asigne cualquier otro alumno</B></asp:Label>
                                </div>
                            </div>
                            <div class="row"><div class="col-md-12 " >&nbsp</div></div>
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
                                            <asp:TableCell HorizontalAlign="Left" Width="50%">
                                                <asp:DropDownList ID="CbPrograma" runat="server" CssClass=" input-group form-control" Width="180px" OnSelectedIndexChanged="CbPrograma_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            </asp:TableCell>
                                        </asp:TableRow>    
                                        <asp:TableRow Width="100%">
                                            <asp:TableCell HorizontalAlign="Right" Width="50%">
                                                <label for="Periodo" class="control-label text-right">Semestre Cursado&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="Left" Width="50%">
                                                <asp:DropDownList ID="CbPeriodo" runat="server" CssClass=" input-group form-control" Width="180px"></asp:DropDownList>
                                            </asp:TableCell>
                                        </asp:TableRow>                   
                                    </asp:Table>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12 " >
                                    &nbsp;
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12 text-center" >
                                    <label for="Nivel" class="control-label text-center">Funciones</label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3 " >
                                </div>
                                <div class="col-md-6 text-center" >
                                    <asp:TextBox ID="Funciones" runat="server" CssClass=" form-control" placeholder="Describe aquí las principales funciones a realizar por el becario" Height="36px"></asp:TextBox>
                                </div>
                            </div> 
                            <div class="row">
                                <div class="col-md-12 " >
                                    &nbsp;
                                </div>
                            </div>
                            <div class="row text-center">

                                <div class="col-md-12 text-center center-block">
                                    <asp:Button ID="Agregar" runat="server" Text="Guardar" CssClass="btn btn-primary" Width="150px" OnClick="Agregar_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="BtnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-primary" Width="150px" OnClick="BtnCancelar_Click" UseSubmitBehavior="false"/>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12 " >
                                    &nbsp;
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <table id ="TblCargaMasiva" runat="server" visible ="false" class="table-responsive table">
                    <tr>
                        <td>
                            <div class="row">
                                <div class="col-md-12 text-center">
                                    <asp:Label ID="lblCarga" runat="server" Text="Carga masiva" CssClass=" control-label h4"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12 " >
                                    &nbsp;
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12 text-center" >
                                    <asp:Label ID="Label5" runat="server" Text="Si requieres algún formato, selecciona uno de los siguientes para su descarga"></asp:Label>
                                </div>
                            </div>
                            <div class="row"><div class="col-md-12 " >&nbsp;</div></div>
                            <div class="row">
                                <div class="col-md-4 " ></div>
                                <div class="col-md-1 " >
                                    <a href="../formatos/matriculas.xlsx">
                                        <img src="../images/excel.png" />
                                    </a>
                                </div>
                                <div class="col-md-4" >
                                    <asp:Label ID="Label3" runat="server" Text="Formato de carga por matricula"></asp:Label>
                                </div>
                            </div>
                            <div class="row"><div class="col-md-12 " >&nbsp;</div></div>
                            <div class="row">
                                <div class="col-md-4 " ></div>
                                <div class="col-md-1 " >
                                    <a href="../formatos/programas.xlsx">
                                        <img src="../images/excel.png" />
                                    </a>
                                </div>
                                <div class="col-md-4 " >
                                    <asp:Label ID="Label4" runat="server" Text="Formato de carga por programa / semestre"></asp:Label>
                                </div>
                            </div>
                            <div class="row center-block jumbotron">
                                <div class="col-md-12 text-center" >
                                    <asp:Label ID="Label6" runat="server" Text="A continuación, sube el archivo con los datos necesarios"></asp:Label>
                                </div>
                            </div>
                            <div class="row"><div class="col-md-12 " >&nbsp;</div></div>
                            <div class="row">
                                <div class="col-md-1 text-right">&nbsp;</div>
                                <div class="col-md-5 text-right">
                                    <asp:FileUpload ID="FileUpload1" runat="server" />
                                </div>
                                <div class="col-md-1 text-right">&nbsp;</div>
                                <div class="col-md-5 center-block" >
                                    <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Cargar" CssClass="btn btn-primary" width="100px" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="BtnCancelarMasiva" runat="server" Text="Cancelar" CssClass="btn btn-primary" OnClick="BtnCancelarMasiva_Click" width="100px"/>
                                </div>
                            </div>
                            
                        </td>               
                        </tr>
                </table>
            </div>
        </div>
<!-- hasta aqui tabla -->
        <div class="row">
            <div class="col-md-12 text-center" >
                &nbsp;
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 text-center" >
                <asp:Label ID="Mensaje" runat="server" Text="" Visible="false"></asp:Label>
            </div>
        </div>
    </div>
        <table id ="TblBecariosProgramas" runat="server" visible ="false" class="table-responsive table">
            <tr>
                <td>
                    <div class="row"><div class="col-md-12 " >&nbsp;</div></div>
                    <div class="row">
                        <div class="col-md-12 text-center " >
                            <asp:Label ID="nombreProyecto2" runat="server" Text="Nombre del Proyecto" CssClass="control-label">Datos del proyecto</asp:Label>
                        </div>
                    </div>
                    <div class="row"><div class="col-md-12 " >&nbsp;</div></div>
                    <div class="row">
                        <div class="col-md-12 text-center" >
                            <asp:Label ID="NoBecarios" runat="server" Text="Becario 1" ></asp:Label>
                        </div>
                    </div>
                    <div class="row">&nbsp;</div>
                    <div class="row">
                        <div class="col-md-3 text-center center-block"></div>
                        <div class="col-md-2 text-center center-block">
                            <asp:Button ID="btnAgregarMatricula" runat="server" Text="Cargar Matrícula" CssClass="btn btn-primary" OnClick="btnAgregarMatricula_Click" width="150px"/>
                        </div>
                        <div class="col-md-2 text-center center-block"></div>
                        <div class="col-md-2 text-center center-block">
                            <asp:Button ID="btnAgregarPrograma" runat="server" Text="Cargar Programa" CssClass="btn btn-primary" OnClick="btnAgregarPrograma_Click" width="150px"/>
                        </div>
                    </div>
                    <div class="row">&nbsp;</div>

                    <div class="row table-condensed">
                        <div class="col-md-3"></div>
                        <div class="col-md-6">
                            <asp:Panel ID="pnlxx" runat="server" CssClass="hscrollbar">
                                <asp:GridView ID="GrdBecarios" ShowHeaderWhenEmpty="True" runat="server" OnRowCommand="GrdBecarios_RowCommand" 
                                    OnRowCreated="GrdBecarios_RowCreated" AutoGenerateColumns="False"
                	                CssClass="table" Width="100%" runat="server" CellPadding="4" ForeColor="#333333"  GridLines="None" PageIndex="10">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775"  />
                                    <columns>
                                        <asp:boundfield datafield="Consecutivo" headertext="#" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                                        <asp:boundfield datafield="ID" headertext="ID" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"/>
                                        <asp:boundfield datafield="Matricula" headertext="Matricula" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"/>
                                        <asp:boundfield datafield="Nombre" headertext="Becario" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                                        <asp:boundfield datafield="Otro" headertext="Otro Alumno" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                                        <asp:buttonfield buttontype="Image" commandname="Eliminar" text="Cancelar" ImageUrl="~/images/Eliminar.png" HeaderText="Cancelar" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"/>
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
                        <div class="col-md-3">
                            <asp:Button ID="btnCargaMasivaMatricula" runat="server" Text="Carga Masiva" CssClass="btn btn-primary" OnClick="btnCargaMasivaMatricula_Click" Width="150px" />
                        </div>
                    </div>
                    <div class="row">&nbsp;</div>
                    <div class="row">
                        <div class="col-md-6 text-right">
                            <asp:Label ID="Label2" runat="server" Text="Nivel"></asp:Label>
                        </div>
                        <div class="col-md-6">
                            <asp:DropDownList ID="DropDownListNivel" runat="server" CssClass=" input-group form-control" Width="150px"></asp:DropDownList>
                        </div>                   
                    </div>
                    <div class="row">&nbsp;</div>
                    <div class="row table-condensed">
                        <div class="col-md-3"></div>
                        <div class="col-md-6">
                            <asp:Panel ID="Panel2" runat="server" CssClass="hscrollbar">
                                <asp:GridView ID="GrdProgramas" ShowHeaderWhenEmpty="True" runat="server" AllowPaging="True"
                    	            OnRowCommand="GrdProgramas_RowCommand" OnRowCreated="GrdProgramas_RowCreated" AutoGenerateColumns="False"
                                    CssClass="table" Width="100%" runat="server" CellPadding="4" ForeColor="#333333"  GridLines="None" PageIndex="10">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775"  />
                                    <columns>
                                        <asp:boundfield datafield="Consecutivo" headertext="#" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"/>
                                        <asp:boundfield datafield="ID" headertext="ID" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"/>
                                        <asp:boundfield datafield="Programa" headertext="Programa" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"/>
                                        <asp:boundfield datafield="Periodo" headertext="Periodo Cursado" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"/>
                                        <asp:boundfield datafield="Otro" headertext="Otro Alumno" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                                        <asp:boundfield datafield="Nivel" headertext="Nivel" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                                        <asp:buttonfield buttontype="Image" commandname="Eliminar" text="Eliminar" ImageUrl="~/images/Eliminar.png" HeaderText="Cancelar" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
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
                        <div class="col-md-3">
                            <asp:Button ID="btnCargaMasivaPrograma" runat="server" Text="Carga Masiva" CssClass="btn btn-primary" OnClick="btnCargaMasivaPrograma_Click" Width="150px" />
                        </div>
                    </div>
                    <div class="row">&nbsp;</div>
                    <div class="row">
                        <div class="col-md-12 text-center " >
                            <label for="Nivel" class="control-label text-center">Si desea que el(los) becarios(s) se presente(n) en una ubicación diferente, favor de especificarla</label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3 " >
                        </div>
                        <div class="col-md-6 text-center" >
                            <asp:TextBox ID="TxtUbicacionAlterna" runat="server" CssClass=" form-control" placeholder="Ubicación" Height="36px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12 text-center " >
                            <label for="Nivel" class="control-label text-center">NOTA: Favor de detallar con claridad que edificio, taller, aula, laboratorio u otros.</label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12 text-center" >
                            &nbsp;
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12 text-center center-block">
                            <asp:Button ID="btnEnviar" runat="server" Text="Enviar" CssClass="btn btn-primary" OnClick="btnEnviar_Click"/>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
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
            <div class="col-md-12 center-block text-center" >
                <asp:Table ID="TblResultado" runat="server" HorizontalAlign="Center" Visible="false">
                    <asp:TableRow HorizontalAlign="Center">
                        <asp:TableCell HorizontalAlign="Center">
                            <asp:Label ID="lblRegistro" runat="server" Text="Se ha registrado la solicitud con el folio " CssClass="control-label"></asp:Label> 
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
                        <asp:TableCell HorizontalAlign="Center">
                            &nbsp;
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow HorizontalAlign="Center">
                        <asp:TableCell>
                            <asp:GridView ID="GrdResultado" 
                                 runat="server" CssClass="table table-responsive"  HorizontalAlign="Center" ></asp:GridView>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
        </div>
        <table id="TblOculta" runat="server" visible="false" class="table-responsive table">
            <tr>
                <td>
                                    <p>
                        <label for="TxtBoxSolicitud" class="control-label" >Solicitud:</label>
                        <asp:TextBox ID="TxtBoxSolicitud" runat="server" Enabled ="False"></asp:TextBox>

                    </p>
                    <p>
                        <label for="lblIdEmpleadoSolicitud" class="control-label" >Empleado:</label>
                        Empleado:<asp:Label ID="lblIdEmpleadoSolicitud" runat="server" Text="IdEmpleadoSolicitud" >0</asp:Label>

                    </p>
                    <p>
                        <label for="TxtBoxProyecto" class="control-label" >Proyecto:</label>
                        <asp:TextBox ID="TxtBoxProyecto" runat="server" Enabled ="False" ></asp:TextBox>
                        <asp:TextBox ID="estatus" runat="server" Enabled ="False" Visible="false" ></asp:TextBox>
                    </p>

                    <p>
                        <label for="TxtEstatusSolicitud" class="control-label" >Estatus:</label>
                        <asp:TextBox ID="TxtEstatusSolicitud" runat="server" Enabled ="False"></asp:TextBox>
                    </p>
                    <p>
                        <label for="TxtNumeroBecarios" class="control-label" >Becarios por Matricula:</label>
                        <asp:TextBox ID="TxtNumeroBecarios" runat="server" value="0" Enabled="false"></asp:TextBox>

                    </p>
                    <p>
                        <label for="TxtNumeroProgramas" class="control-label" >Becarios por Programa:</label>
                        <asp:TextBox ID="TxtNumeroProgramas" runat="server" value="0" Enabled="false"></asp:TextBox>
                    </p>
                    <p>
                        <label for="TextBoxTipoCargaExcel" class="control-label" >Tipo de Carga Excel:</label>
                        <asp:TextBox ID="TextBoxTipoCargaExcel" runat="server" Enabled="false" ></asp:TextBox>
                    </p>
                   
                </td>
            </tr>

        </table>
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
        });
    </script>



    
    <style>
        #ui-id-1{
            z-index:1000;
        }
    </style> 

</asp:Content>