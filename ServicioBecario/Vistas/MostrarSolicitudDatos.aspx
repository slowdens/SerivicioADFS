<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="MostrarSolicitudDatos.aspx.cs" Inherits="ServicioBecario.Vistas.MostrarSolicitudDatos" %>
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
        function paraMatricula(e) {
            key = e.keyCode || e.which;
            tecla = String.fromCharCode(key).toLowerCase();
            letras = "aA0123456789";
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
        <div class="col-md-12 text-center">
            <h1>Reportes de becarios</h1>            
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 text-center">
            <h4><asp:Label ID="lblTipoSolicitud" runat="server" Text=""></asp:Label> </h4>            
        </div>
    </div>
        <div class="row">
        <div class="col-md-12">
            <asp:Panel ID="pnlEspecifica" runat="server" Visible="false">
                <div class="row">
                    <div class="col-md-2">
                        <label>Nombre del solicitante</label>
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="lblNombreSolicitante" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="col-md-1">
                        <label>Periodo</label>
                    </div>
                    <div class="col-md-1">
                        <asp:Label ID="lblPeriodo" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="col-md-2">
                        <label>Tipo de solicitud</label>
                    </div>
                    <div class="col-md-2">
                        <asp:Label ID="lblSolicitud" runat="server" Text=""></asp:Label>
                    </div>
                </div>
                <asp:Panel ID="pnlActualizarEspecifica"  Visible="false" runat="server">
                    <div class="row">
                        <div class="col-md-12">
                            <fieldset>
                                <h4 class="text-center">Seleccionar por matricula </h4>
                                <legend></legend>
                                <div class="row">
                                    <div class="col-md-1">
                                        <label>Matricula</label>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtAmatricula" OnKeyPress = "return paraMatricula(event);" AutoPostBack="true" runat="server" CssClass="form-control validate[validate,custom[matricula]]" OnTextChanged="txtAmatricula_TextChanged"></asp:TextBox>                                        
                                    </div>   
                                    <div class="col-md-4">
                                        <asp:Label ID="lblNombre" runat="server" Text=""></asp:Label>
                                    </div>                                 
                                </div>                               
                            </fieldset>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                             <fieldset >
                                 <h4 class="text-center">Seleccionar  por diferentes características  </h4>
                                 <legend></legend>
                                 <div class="row">
                                     <div class="col-md-2">
                                        <label>Nivel de estudios</label>
                                     </div>
                                     <div class="col-md-2">
                                         <asp:DropDownList CssClass="form-control" ID="ddlAnivelEstudios" runat="server" OnDataBound="ddlAnivelEstudios_DataBound">
                                         </asp:DropDownList>
                                     </div>
                                     <div class="col-md-2">
                                         <label>Programa academico</label>
                                     </div>
                                     <div class="col-md-2">
                                         <asp:DropDownList ID="ddlAProgramaAcademico" CssClass="form-control" runat="server" OnDataBound="ddlAProgramaAcademico_DataBound"></asp:DropDownList>
                                     </div>
                                     <div class="col-md-2">
                                         <label>Periodo Cursado</label>
                                     </div>
                                     <div class="col-md-1">
                                         <asp:DropDownList ID="ddlAPeriodoCursado" CssClass="form-control" runat="server" OnDataBound="ddlAPeriodoCursado_DataBound"></asp:DropDownList>
                                     </div>
                                 </div>
                             </fieldset>
                               
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <fieldset>
                                <h4 class="text-center">Funciones del alumno</h4>
                                <legend></legend>
                                <div class="row">
                                    <div class="col-md-1">
                                        <label>Funciones </label>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtAFunciones" OnKeyPress = "return paraMatricula(event);" CssClass="form-control"  TextMode="MultiLine" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-5">
                                        <asp:CheckBox ID="chkDeacuerdo" runat="server" Text="Estoy de acuerdo  que se me asigne cualquier otro alumno" Checked="true" />
                                    </div>
                                    <div class="col-md-1">
                                        <asp:Button  CssClass="btn btn-primary" runat="server" Text="Actualizar" ID="btnAActualizarInfor"  OnClick="btnAActualizarInfor_Click" OnClientClick="validar();"/>
                                        
                                    </div>
                                    <div class="col-md-1">
                                        <asp:Button  CssClass="btn btn-primary" runat="server" Text="Cancelar" ID="btncancelar"  OnClick="btncancelar_Click" OnClientClick="validar();"/>
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                    </div>
                </asp:Panel>
                <div class="row table-condensed">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <asp:Panel ID="pnlVistaEspecifica" runat="server" CssClass="hscrollbar">
                            <asp:GridView ID="GVSolicitudIndividual" AutoGenerateColumns="False" CssClass="table"
                                runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField DataField="Matricula" HeaderText="Matrícula" SortExpression="Matricula" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                                    <asp:BoundField DataField="Nivel_academico" HeaderText="Nivel academico" SortExpression="Nivel_academico" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                                    <asp:BoundField DataField="Programa academico" HeaderText="Programa Academico" SortExpression="Programa Academico" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                                    <asp:BoundField DataField="Periodo" HeaderText="Periodo" SortExpression="Periodo" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"  />
                                    <asp:CheckBoxField DataField="Otro_alumno" HeaderText="Otro alumno" SortExpression="Otro_alumno" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                                    <asp:BoundField DataField="Estatus_asignacion" HeaderText="Estatus asignación" SortExpression="Estatus_asignacion" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                                    
                                    <asp:TemplateField HeaderText="Modificar"  ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgActualizar" CommandName='<%# Eval("id_consecutivo")%>'  Visible='<%# Convert.ToBoolean( Eval("Valor"))%>'    runat="server"  ImageUrl="~/images/update.png"  OnClick="imgActualizar_Click"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Eliminar"  ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" >
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imbEliminar"  Visible='<%# Convert.ToBoolean( Eval("Valor"))%>'   CommandArgument='<%# Eval("id_consecutivo") %>' runat="server" ImageUrl="~/images/EliminarR.png" OnClick="imbEliminar_click" />
                                            <cc1:ConfirmButtonExtender ID="cbe" runat="server" DisplayModalPopupID="mpe" TargetControlID="imbEliminar"></cc1:ConfirmButtonExtender>
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
                                <FooterStyle BackColor="#3B83C0" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#3B83C0" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#3B83C0" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />

                            </asp:GridView>
                            <br />
                            <br />
                        </asp:Panel>

                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlPorProyecto" runat="server" Visible="false">
                <div class="row">
                    <div class="col-md-12">
                        <div class="row">
                            <div class="col-md-1">
                                <label>Solicitante</label>
                            </div>
                            <div class="col-md-3">
                                <asp:Label Text="" runat="server" ID="lblSolicitanteP"> </asp:Label>
                            </div>
                            <div class="col-md-1">
                                <label>periodo</label>
                            </div>
                            <div class="col-md-1">
                                <asp:Label ID="lblPeriodoP" runat="server"></asp:Label>
                            </div>
                            <div class="col-md-2">
                                <label>Tipo de solicitud</label>
                            </div>
                            <div class="col-md-2">
                                <asp:Label runat="server" ID="lblTipoSolicitudP" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="pnlActualizarPorProyecto" Visible="false" runat="server">
                                   
                                </asp:Panel>
                                <asp:Panel ID="pnlActualizarProyectoBecarios" Visible="false" runat="server">
                                     <div class="row">
                                        <div class="col-md-12">
                                            <fieldset>
                                                <h4 class="text-center">Seleccionar por matricula</h4>
                                                <legend></legend>
                                                  <div class="row">
                                                      <div class="col-md-1">
                                                          <label>Matricula</label>
                                                      </div>
                                                      <div class="col-md-3">
                                                          <asp:TextBox runat="server" OnKeyPress = "return paraMatricula(event);" CssClass="form-control" ID="txtMatriculaPorproyecto" OnTextChanged="txtMatriculaPorproyecto_TextChanged" ></asp:TextBox>
                                                      </div>
                                                      <div class="col-md-3">
                                                          <asp:Label  Text="" runat="server" ID="lblAlumnoPorProyecto"></asp:Label>
                                                      </div>
                                                  </div>      
                                            </fieldset>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <fieldset>
                                                <h4 class="text-center">Seleccionar por diferentes características </h4>
                                                <legend></legend> 
                                                <div class="row">
                                                    <div class="col-md-2">
                                                        <label>Nivel de estudios</label>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:DropDownList ID="ddlNivelEstudosPorproyecto" CssClass="form-control" runat="server" OnDataBound="ddlNivelEstudosPorproyecto_DataBound"></asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <label> Programa academico</label>
                                                    </div>
                                                    <div class=" col-md-2">
                                                        <asp:DropDownList ID="ddlprogramaAcademicoPorProyecto" runat="server" CssClass="form-control" OnDataBound="ddlprogramaAcademicoPorProyecto_DataBound"></asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <label>Periodo cursado</label>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <asp:DropDownList ID="ddlPeriodoCursadoPorproyecto" CssClass="form-control" runat="server" OnDataBound="ddlPeriodoCursadoPorproyecto_DataBound"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </fieldset>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <fieldset>
                                                <h4 class="text-center">Funciones del alumno </h4>
                                                <legend></legend>
                                            </fieldset>
                                            <div class="row">
                                                <div class="col-md-1">
                                                    <label>Funciones</label>
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:TextBox ID="txtFuncionesPorProyecto" CssClass="form-control" TextMode="MultiLine" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-md-5">
                                                    <asp:CheckBox ID="ChkDeacuerdoPorproyecto" runat="server" Text="Estoy de acuerdo  que se me asigne cualquier otro alumno" Checked="true" />
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:Button  ID="btnActualizarAlumnoPorProyecto" runat="server" Text="Actualizar" CssClass="btn btn-primary" OnClick="btnActualizarAlumnoPorProyecto_Click"/>
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:Button  ID="btncancelarproyecto" runat="server" Text="Cancelar" CssClass="btn btn-primary" OnClick="btncancelarproyecto_Click"/>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="row table-condensed">
                            <div class="col-md-12">
                                <asp:Panel ID="pnlVistaporProyecto" runat="server"  CssClass="hscrollbar">
                                    <asp:GridView ID="gvVistasProyecto" AutoGenerateColumns="false" CssClass="table" Width="100%" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
                                        <Columns>
                                            <asp:BoundField DataField="Matricula" HeaderText="Matricula" SortExpression="Matricula" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"/>
                                            <asp:BoundField DataField="Nivel_academico" HeaderText="Nivel academico" SortExpression="Nivel_academico" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                                            <asp:BoundField DataField="Programa Academico" HeaderText="Programa Academico" SortExpression="Programa Academico" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                                            <asp:BoundField DataField="Periodo" HeaderText="Periodo" SortExpression="Periodo"  ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"/>
                                            <asp:CheckBoxField DataField="Otro_alumno" HeaderText="Otro alumno" SortExpression="Otro_alumno" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center"/>
                                            <asp:BoundField DataField="Estatus_asignacion" HeaderText="Estatus asignacion" SortExpression="Estatus_asignacion" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />

                                            <asp:TemplateField HeaderText="Modificar"  ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgActualizarPorProyecto" CommandName='<%# Eval("id_consecutivo")%>' Visible='<%# Convert.ToBoolean( Eval("Valor"))%>' runat="server" ImageUrl="~/images/update.png" OnClick="imgActualizarPorProyecto_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Eliminar"  ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imbEliminar" Visible='<%# Convert.ToBoolean( Eval("Valor"))%>' CommandArgument='<%# Eval("id_consecutivo") %>' runat="server" ImageUrl="~/images/EliminarR.png" OnClick="btnporProyectoEliminar_Click" />
                                                    <cc1:ConfirmButtonExtender ID="cbe" runat="server" DisplayModalPopupID="mpe" TargetControlID="imbEliminar"></cc1:ConfirmButtonExtender>
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
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <EditRowStyle BackColor="#999999" />
                                        <FooterStyle BackColor="#3B83C0" Font-Bold="True" ForeColor="White" />
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
                    </div>
                </div>
                
            </asp:Panel>
            <asp:Panel ID="pnlSorteo" runat="server" Visible="false">
            </asp:Panel>
        </div>
    </div>






    


   



    
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

    <asp:HiddenField ID="hdfid_alumno" runat="server" />
    


</asp:Content>
