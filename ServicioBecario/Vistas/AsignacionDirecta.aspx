<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="AsignacionDirecta.aspx.cs" Inherits="ServicioBecario.Vistas.AsignacionDirecta" %>
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
    <script>
        
       
        function paraMatriculas(e, control) {
            var dato;
            key = e.keyCode || e.which;
            tecla = String.fromCharCode(key).toLowerCase();
            letras = "a0123456789";
            especiales = "8-37-39-46";

            var array = especiales.split("-");
            tecla_especial = false
            //for (var i in especiales) {
            //    dato = especiales[i];
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

        /************************Validar datos****************************/
        function validarDatos()
        {
                   
            
            jQuery("form").validationEngine();
        }
        /************************Termina el validar datos******************/

    </script>
    <div class="row">        
        <div class="col-md-12 text-center">
            <h1>Servicio Becario</h1>            
        </div>        
    </div>
    <div class="row">
        <div class="col-md-12 text-center " >
            <h4>Asignación directa nómina matrícula </h4>    
        </div>
    </div>

            <div class="jumbotron">
                <div class="row">
                    <div class="col-md-1">
                        <label>Periodo</label>
                    </div>
                    <div class="col-md-3">
                        <asp:DropDownList ID="ddlPeriodo" CssClass="form-control validate[required]" runat="server" OnDataBound="ddlPeriodo_DataBound"></asp:DropDownList>
                    </div>
                    <div class="col-md-1">
                        <label>Matrícula</label>
                    </div>
                    <div class="col-md-2">
                        <asp:TextBox ID="txtMatricula" runat="server" placeholder="A00000000" OnKeyPress="return paraMatriculas(event,this);" CssClass="form-control  validate[required]  validate[custom[matricula]]   " OnTextChanged="txtMatricula_TextChanged" AutoPostBack="true"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="lblMatricula" runat="server"></asp:Label>
                   <br /><br /><br /><br /> </div>
                    <div class="col-md-1">
                        <asp:Button ID="btnverAsignacion" Visible="false" runat="server" CssClass="btn btn-primary" OnClientClick="validarDatos();" Text="Validar" OnClick="btnverAsignacion_Click" />
                    <br /><br /></div>
                </div>
            </div>
        
        <%--Cuando el becario tiene una signación ya hecha--%>
        <asp:Panel ID="pnlConAsignacion" runat="server" Visible="false">      
                    <asp:HiddenField ID="hdfDecideDos" runat="server" />
                    <div class="row">
                        <div class="col-md-12">
                            <fieldset>
                                <legend class="text-center">
                                    <h4>
                                        <label>Ya cuenta con una asignación el becario</label></h4>
                                </legend>
                            </fieldset>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-1">
                            <label>Proyecto</label>
                        </div>
                        <div class="col-md-10">
                            <asp:Label ID="lblAsignadoproyecto" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-1">
                            <label>Nómina</label>
                        </div>
                        <div class="col-md-1">
                            <asp:Label ID="lblAsignadoNomina" Text="" runat="server"></asp:Label>
                        </div>
                        <div class="col-md-1">
                            <label>Solicitante</label>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="lblAsignadoSolicitante" runat="server"> </asp:Label>
                        </div>
                        <div class="col-md-1">
                            <label>Puesto</label>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="lblAsignadopueto" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-2">
                            <label>Departamento</label>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="lblAsignadoDepartamento" runat="server"></asp:Label>
                        </div>
                        <div class="col-md-2">
                            <label>Ubicación fisica</label>
                        </div>
                        <div class="col-md-3">
                            <asp:Label ID="lblAsignadoUbicacionFisica" runat="server"></asp:Label>
                        </div>
                    </div>
                    <br />
            <br />
            <br />
                    <div class="row">
                        <div class="col-md-12">
                            <fieldset>
                                <legend class="text-center">
                                    <h4>
                                        <label>Quitar asignación</label></h4>
                                </legend>
                            </fieldset>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-1">
                            <label>Justificación</label>
                        </div>
                        <div class="col-md-11">
                            <asp:TextBox runat="server" TextMode="MultiLine" placeholder="¿Porqué deseas eliminar la asignación?" ID="txtDJustificacion" CssClass="form-control"> </asp:TextBox>
                        </div>
                    </div>
            <br />
                    <div class="row">
                        <div class= " col-md-offset-5 col-md-1">
                            
                            <asp:Button ID="btnDesasignar" CssClass="btn btn-primary" runat="server" Text="Desasociar" OnClick="btnDesasignar_Click" />
                        </div>
                        <div class="col-md-1">
                            <asp:Button runat="server" ID="btnCancelar" CssClass="btn btn-primary" Text="Cancelar" OnClick="btnCancelar_Click" />
                        </div>
                    </div>                    
            
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlAsignar" Visible="false" >
            <div class="row">
                <div class="col-md-1">
                    <label>Nómina</label>
                </div>
                <div class="col-md-2">
                    <asp:TextBox ID="txtAsignarNomina" placeholder="L00000000" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <asp:CheckBox  runat="server" ID="chkProyecto" CssClass="checkbox" AutoPostBack="true"  Text="Agregar proyecto" OnCheckedChanged="chkProyecto_CheckedChanged"/>
                </div>                
                <div class="col-md-7">
                    <div class="row">
                        <asp:Panel runat="server" ID="pnlProyecto" Visible="false">
                    
                    
                        <div class="col-md-2">
                            <label>Proyecto</label>
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="txtProyecto" placeholder="Servicio Becario" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>    
                    
                    
                </asp:Panel>
                    </div>
                    
                </div>
                
            </div>
            <div class="row">
                <div class="col-md-1">
                    <label>Funciones becario</label>
                </div>
                <div class="col-md-4">
                    <asp:TextBox runat="server" ID="txtfuncionesBecario" placeholder="¿Cuáles serán las funciones a realizar?" CssClass="form-control" ></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-md-1">
                    <asp:Button  ID="btnVerficanombreAsigador" CssClass="btn btn-primary" runat="server" Text="Verificar" OnClick="btnVerficanombreAsigador_Click"/>
                </div>
                <div class="col-md-1">
                    <asp:Button ID="btnCan" Text="Cancelar" runat="server" CssClass="btn btn-primary" OnClick="btnCan_Click" />
                </div>
            </div>
            </asp:Panel>
            
            <asp:Panel runat="server" ID="pnlDatoSolicitante"  Visible="false" >
                 <div class="row">
                <div class="col-md-12">
                    <fieldset>
                       <legend class="text-center">
                           <h4><label>Datos de nómina</label></h4>
                       </legend>
                    </fieldset>
                </div>
            </div>
            <div class="row">
                <div class="col-md-1">
                    <label>Nomina</label>
                </div>
                <div class="col-md-2">
                    <asp:Label runat="server" ID="lblNominaAsignar"></asp:Label>
                </div>
                <div class="col-md-1">
                    <label>Nombre</label>
                </div>
                <div class="col-md-3">
                    <asp:Label ID="lblNombreSolicitanteAsignar" runat="server" ></asp:Label>
                </div>
                <div class="col-md-1">
                    <label>Puesto</label>
                </div>
                <div class="col-md-3">
                    <asp:Label runat="server" ID="lblPuestoAsignar"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <label>Departamento</label>
                </div>
                <div class="col-md-2">
                    <asp:Label runat="server" ID="lblDepartamentoAsignar" ></asp:Label>
                </div>
                <div class="col-md-2">
                   <label> Ubicación fisica                 
                   </label>
                </div>
                <div class="col-md-3">
                    <asp:Label runat="server" ID="lbUbicacionFisicaAsignar" ></asp:Label>
                </div>  
                
            </div>
            <div class="row">
                <div class="col-md-offset-5 col-md-1">
                    <asp:Button ID="btnAsignar" Text="Asignar" runat="server" CssClass="btn btn-primary" OnClick="btnAsignar_Click" />
                </div>
            </div>   
            </asp:Panel>
            
            
       




    
    
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
                                    <!--<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>-->
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
