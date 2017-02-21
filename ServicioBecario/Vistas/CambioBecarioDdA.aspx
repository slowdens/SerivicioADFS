<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="CambioBecarioDdA.aspx.cs" Inherits="ServicioBecario.Vistas.CambioBecarioDdA" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <link rel="stylesheet" href="../scripts/footable.bootstrap.min.css" type="text/css" /> 
    <link rel="stylesheet"  href="../scripts/Enginee/css/validationEngine.jquery.css" type="text/css"/> 
    <link rel="stylesheet" href="../scripts/Estiloss.css" type="text/css" />   
    
    <script type="text/javascript" src="../scripts/Enginee/js/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../scripts/Enginee/js/jquery.validationEngine.js" charset="utf-8"></script>
    <script>
        function validar() {
            //Esto es para validar campos
            jQuery("form").validationEngine();
        }
        function confirmar() {
            var opt = confirm("Desea asignar al becario");
            $('#<%=hdfDecide.ClientID%>').val(opt);

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

    </script>
     <div class="row">
        <div class="col-md-12 text-center">
                 <h1 class="text-center">Servicio Becario</h1>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 text-center">
            <h4>Cambiar Becario despues de asignación</h4>
        </div>

    </div>
    <div class="jumbotron">
        <div class="row">
            <div class="col-md-1">
                <label>Periodo</label>
            </div>
            <div class="col-md-3">
                <asp:DropDownList runat="server" ID="ddlPeriodo" CssClass="form-control validate[required]" OnDataBound="ddlPeriodo_DataBound"></asp:DropDownList>
            </div>            
            <div class="col-md-1">
                <label>Matrícula</label>
            </div>
            <div class="col-md-2">
                <asp:TextBox CssClass="form-control validate[required]" placeholder="A00000000" OnKeyPress="return paraMatriculas(event,this);" runat="server" AutoPostBack="true" ID="txtMatricula" OnTextChanged="txtMatricula_TextChanged"></asp:TextBox>
            <br /><br /><br /><br /></div>
            <div class="col-md-3">
                <asp:Label runat="server" ID="lblNombreBecario"></asp:Label>
            </div>
            <div class="col-md-1">
                <asp:Button runat="server" ID="btnAsignacion" OnClientClick="validar();" CssClass="btn btn-primary" Text="Ver asignación" OnClick="btnAsignacion_Click" Visible="false"/>
            <br /><br /></div>
        </div>
    </div>
    <asp:Panel runat="server" ID="pnlVerAsignacion" Visible="false">
        <div class="row">
            <div class="col-md-12">
                <fieldset>
                    <legend class="text-center">
                       <h4>
                           <label>
                               Datos de nómina responsable
                           </label>
                       </h4>
                    </legend>
                </fieldset>
            </div>
        </div>
        <div class="row">
            <div class="col-md-1">
                <asp:HiddenField runat="server" ID="hdfId_miSolicitud" />
                <label>Nómina</label>
            </div>
            <div class="col-md-1">
                <asp:Label runat="server" ID="lblNomina"></asp:Label>
            </div>
            <div class="col-md-1">
                <label>Nombre</label>
            </div>
            <div class="col-md-3">
                <asp:Label runat="server" ID="lblNombreSolicitante"></asp:Label>
            </div>
            <div class="col-md-1">
                <label>
                    Puesto
                </label>
            </div>
            <div class="col-md-3">
                <asp:Label runat="server" ID="lblPuesto"></asp:Label>
            </div>            
        </div>
        <div class="row">
            <div class="col-md-1">
                <label>Campus</label>
            </div>
            <div class="col-md-2">
                <asp:Label ID="lblCampus" runat="server"></asp:Label>
            </div>
            <div class="col-md-1">
                <label>Ubicación</label>
            </div>
            <div class="col-md-2">
                <asp:Label ID="lblUbicacion" runat="server" ></asp:Label>
            </div>
            <div class="col-md-1">
                <label>Extención</label>
            </div>
            <div class="col-md-2">
                <asp:Label ID="lblExtencion" runat="server"></asp:Label>
            </div>
        </div>
     </asp:Panel>
    <asp:Panel ID="pnlMotovos" runat="server" Visible="false" >
        <div class="row">
            <div class="col-md-12">
                <fieldset>
                    <legend class="text-center">
                        <h4>
                            <label>
                                Motivos
                            </label>
                        </h4>
                    </legend>
                </fieldset>
            </div>
        </div>
        <div class="row">
            <div class="col-md-1">
                <label>
                    Justificación
                </label>
            </div>
            <div class="col-md-11">
                <asp:TextBox ID="txtJustificacion" TextMode="MultiLine" placeholder="¿Cuál es el motivo por el que deseas cambiar  tu Becario?" CssClass="form-control" runat="server"></asp:TextBox>
            </div>            
        </div>
        <br />
        <div class="row">
            <div class="col-md-offset-5 col-md-1">
                <asp:Button  runat="server" ID="btnDesasingar" Text="Desasociar" CssClass="btn btn-primary" OnClick="btnDesasingar_Click"/>
            </div>
            <div class="col-md-1">
                <asp:Button runat="server" ID="btnCance" CssClass="btn btn-primary" Text="Cancelar" OnClick="btnCance_Click"  />
            </div>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlBecariosNoAsignados" Visible="false">
        <div class="row table-condensed">
            <div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">
                <asp:Panel runat="server" ID="pnlxxx" CssClass="hscrollbar">
                    <asp:GridView ID="gvDatos" AutoGenerateColumns="false" runat="server" CssClass="table" CellPadding="4" ForeColor="#333333" GridLines="None">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775"></AlternatingRowStyle>
                        <Columns>
                            <asp:BoundField ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" DataField="Matricula" HeaderText="Matricula" SortExpression="Matricula"></asp:BoundField>
                            <asp:BoundField ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" DataField="Becario" HeaderText="Becario" SortExpression="Becario"></asp:BoundField>
                            <asp:BoundField ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" DataField="Campus" HeaderText="Campus" SortExpression="Campus"></asp:BoundField>
                            <asp:TemplateField HeaderText="Asignar">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkselecioname" OnClick="confirmar();" runat="server" ToolTip='<%#Eval("Matricula")%>' OnCheckedChanged="chkselecioname_CheckedChanged" AutoPostBack="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle BackColor="#999999"></EditRowStyle>
                        <FooterStyle BackColor="#3B83C0" Font-Bold="True" ForeColor="White"></FooterStyle>
                        <HeaderStyle BackColor="#3B83C0" Font-Bold="True" ForeColor="White"></HeaderStyle>
                        <PagerStyle HorizontalAlign="Center" BackColor="#3B83C0" ForeColor="White"></PagerStyle>
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333"></RowStyle>
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333"></SelectedRowStyle>
                        <SortedAscendingCellStyle BackColor="#E9E7E2"></SortedAscendingCellStyle>
                        <SortedAscendingHeaderStyle BackColor="#506C8C"></SortedAscendingHeaderStyle>
                        <SortedDescendingCellStyle BackColor="#FFFDF8"></SortedDescendingCellStyle>
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE"></SortedDescendingHeaderStyle>
                    </asp:GridView>
                </asp:Panel>
            </div>
        </div>
    </asp:Panel>
    <asp:HiddenField runat="server" ID="hdfDecide"/>
    
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
