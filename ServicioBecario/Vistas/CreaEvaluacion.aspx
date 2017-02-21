<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="CreaEvaluacion.aspx.cs" Inherits="ServicioBecario.Vistas.CreaEvaluacion" %>
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
            function Bandera() {
                localStorage["Bandera"] = 1;
            }
            function validar() {
                //Esto es para validar campos
                jQuery("form").validationEngine();
            }
            function quitarValidar() {
                //Esto es para omitir la validación
                jQuery("form").validationEngine('detach');
            }
            function desavilitaCampus()
            {
                var textBox = document.getElementById("<%=ddlFiltraCampus.ClientID %>");
                textBox.disabled = true;

            }
            function puntoNumeros(e) {
                key = e.keyCode || e.which;
                tecla = String.fromCharCode(key).toLowerCase();
                letras = ".0123456789";
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
      <div class="row">
            <div class="col-md-12 text-center">
                <h1>Servicio Becario</h1>

            </div>
        </div>
       <div class="row">
            <div class="col-md-12  text-center">
             <h4>   Crear Evaluación </h4>                
                
            </div>
       </div>       
     <a class="btn btn-primary btn-sm" id="btnA"><em class="glyphicon glyphicon-plus"></em> Agregar</a>
    <a class="btn btn-primary btn-sm" id="btnB"><em class="glyphicon glyphicon-search"></em> Buscar</a>
    <div class="ContAll">
    <div id="ContAg">
    
                <asp:Panel ID="pnlCapturaPregunta" runat="server">
                <div class="jumbotron">
                    <div class="row">
                        <div class="col-md-1 col-md-offset-2">
                            <label>Campus </label>
                        </div>
                        <div class="col-md-3">
                            <asp:DropDownList ID="ddlCampus" CssClass="form-control validate[required]" runat="server" OnDataBound="ddlCampus_DataBound"></asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <label>Dirigido a: </label>
                        </div>
                        
                        <div class="col-md-3">
                            <asp:DropDownList ID="ddlDirigido" CssClass="form-control validate[required]" runat="server" OnDataBound="ddlDirigido_DataBound"></asp:DropDownList>
                        </div>
                    </div>
                    <br>                                       
                        <div class="row">
                            <div class="col-md-1">
                                <label>Pregunta:</label>
                            </div>
                            <div class="col-md-11">
                                <asp:TextBox ID="txtPregunta" CssClass="form-control validate[required]" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-5">
                                <label>Tipo de componente para respuesta</label>
                            </div>
                            <div class="col-md-3">
                                <asp:DropDownList ID="ddlComponente" CssClass="form-control validate[required]" runat="server" OnDataBound="ddlComponente_DataBound"></asp:DropDownList>
                            </div>
                            <div class="col-md-1">
                                <asp:Button ID="btnGuardarPregunta" CssClass="btn btn-primary" Text="Guardar" runat="server" OnClientClick="validar();" OnClick="btnGuardarPregunta_Click" />
                            </div>
                            
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlCapturaRespuesta" Visible="False" runat="server">
                    <div class="jumbotron">
                        <div class="row">
                            <div class="col-md-12 text-center">

                                <fieldset>
                                    <legend class="text-center"><h4>Captura de respuesta</h4></legend>
                                </fieldset>


                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Label ID="lblPregunta" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-1">
                                <label></label>
                            </div>
                            <div class="col-md-3 col-md-offset-2">
                                <label>Agregar respuesta </label>
                            </div>
                            <div class="col-md-4">
                                <label>Valor</label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-1 col-md-offset-2 text-right">
                                <label>1.- </label>
                            </div>
                            <div class="col-md-2 ">
                                <asp:TextBox ID="txtRespuesta1" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-offset-1 col-md-2">
                                <asp:TextBox ID="txtValor1" CssClass="form-control validate[required,custom[puntoflotante]]" OnKeyPress = "return  puntoNumeros(event);" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-1 col-md-offset-2 text-right ">
                                <label>2.- </label>
                            </div>
                            <div class="col-md-2 ">
                                <asp:TextBox ID="txtRespuesta2" CssClass="form-control  validate[required]" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2  col-md-offset-1">
                                <asp:TextBox ID="txtValor2" OnKeyPress = "return  puntoNumeros(event);" CssClass="form-control validate[required,custom[puntoflotante]]" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-1 col-md-offset-2 text-right ">
                                <label>3.- </label>
                            </div>
                            <div class="col-md-2 ">
                                <asp:TextBox ID="txtRespuesta3" CssClass="form-control  validate[required]" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2  col-md-offset-1">
                                <asp:TextBox ID="txtValor3" OnKeyPress = "return  puntoNumeros(event);" CssClass="form-control validate[required,custom[puntoflotante]] " runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-offset-2 col-md-1 text-right ">
                                <label>4.- </label>
                            </div>
                            <div class="col-md-2 ">
                                <asp:TextBox ID="txtRespuesta4"  CssClass="form-control  validate[required]" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2  col-md-offset-1">
                                <asp:TextBox ID="txtValor4" OnKeyPress = "return  puntoNumeros(event);" CssClass="form-control validate[required,custom[puntoflotante]]" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class=" col-md-offset-2 col-md-1  text-right">
                                <label class="text-right">5.- </label>
                            </div>
                            <div class="col-md-2 ">
                                <asp:TextBox ID="txtRespuesta5"  CssClass="form-control " runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-2  col-md-offset-1">
                                <asp:TextBox ID="txtValor5" OnKeyPress = "return  puntoNumeros(event);" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-1">
                                <asp:Button ID="btbAgregarRespuesta" CssClass="btn btn-primary" runat="server" Text="Agregar" OnClick="btbAgregarRespuesta_Click" OnClientClick="validar();" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        <div id="ContBu"> 
                <asp:Panel ID="pnlMostrarPreguntas" Visible="true" runat="server">
                    <div class="jumbotron">
                        <div class="row">
                            <div class="col-md-1">
                                <label>Pregunta</label>
                            </div>
                            <div class="col-md-11">
                                <asp:TextBox ID="txtFiltrarPregunas" CssClass="form-control validate[required]" runat="server" placeholder="¿Cuál es la pregunta a buscar?"></asp:TextBox>
                            </div>
                        </div>
                         <br />
                        <div class="row">
                            <div class="col-md-1">
                                <label>Componente</label>
                            </div>
                            <div class="col-md-2">
                                <asp:DropDownList ID="ddlfiltrarComponentes" CssClass="form-control validate[required]" runat="server" OnDataBound="ddlfiltrarComponentes_DataBound"></asp:DropDownList>
                            </div>
                            <div class="col-md-1">
                                <label>Dirigido</label>
                            </div>
                            <div class="col-md-3">
                                <asp:DropDownList ID="ddlFiltrarDirigido" runat="server" CssClass="form-control validate[required]" OnDataBound="ddlFiltrarDirigido_DataBound"></asp:DropDownList>
                            </div>                                                        
                            <div class="col-md-1">
                                <label>Campus</label>
                            </div>
                            <div class="col-md-3">
                                <asp:DropDownList ID="ddlFiltraCampus" runat="server" CssClass="form-control validate[required]" OnDataBound="ddlFiltraCampus_DataBound"></asp:DropDownList>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            
                            <div class="col-md-1 col-md-offset-5">
                                <asp:Button ID="btnFiltrarPregunta" CssClass="btn btn-primary" runat="server" Text="Filtrar" OnClick="btnFiltrarPregunta_Click" OnClientClick="quitarValidar();" />
                                <asp:Button ID="btnActulizarPreguntas" OnClientClick="validar();" CssClass="btn btn-primary" runat="server" OnClick="btnActulizarPreguntas_click" Text="Actualizar" Visible="false" />
                            </div>
                            <div class="col-md-1">
                                <asp:Button ID="btnCancelarEdicionPregunta" Visible="false"  runat="server" CssClass="btn btn-primary" OnClick="btnCancelarEdicionPregunta_Click" Text="Cancelar" OnClientClick="quitarValidar();"/>
                            </div>
                        </div>
                    </div>
                    <div class="row table-condensed">
                        <div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">
                            <asp:Panel ID="pnlxx" runat="server" CssClass="hscrollbar">
                                <asp:GridView ID="GvMostrarPregutas" runat="server" CssClass="table"
                                    CellPadding="4" AutoGenerateColumns="false" AllowPaging="True"
                                    ForeColor="#333333" GridLines="None" DataKeyNames="id_pregunta" Width="100%" OnSelectedIndexChanged="GvMostrarPregutas_SelectedIndexChanged" OnPageIndexChanging="GvMostrarPregutas_PageIndexChanging" PageSize="8">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField DataField="id_pregunta" HeaderText="id_pregunta" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" SortExpression="id_pregunta" Visible="false" />
                                        <asp:TemplateField HeaderText="Preguntas" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkVista" OnClick="lnkVista_click" CommandArgument='<%#Eval("Combinado") %>' CommandName='<%#Eval("Pregunta") %>' runat="server"><%#Eval("Pregunta") %></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="componente" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="Componente" Visible="true" />
                                        <asp:BoundField DataField="dirigido" HeaderText="Dirigido" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" Visible="true" />
                                        <asp:BoundField DataField="Campus" HeaderText="Campus" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" Visible="true" />
                                        <asp:CommandField ButtonType="Image" SelectImageUrl="~/images/update.PNG" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" HeaderText="Modificar" ShowSelectButton="True" />
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
                            </asp:Panel>
                            
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="PnlVerRespuestas" Visible="false" runat="server">
                    <div class="row">
                        <div class="col-md-12 text-center">
                            <fieldset>
                                <legend class="text-center">
                                    <h4>Respuestas creadas</h4>
                                </legend>
                            </fieldset>
                          
                          
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-1">
                            <label>Pregunta: </label>
                        </div>
                        <div class="col-md-11">
                            <asp:Label ID="lblRespuestaFiltrada" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-1">
                            <label>Campus</label>
                        </div>
                        <div class="col-md-2">
                            <asp:Label ID="lblCampusFiltrado" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                    <asp:Panel ID="pnlActualizarRespuestas" Visible="false" runat="server">
                        <div class="jumbotron">
                            <div class="row">
                                <div class="col-md-12">
                                    <fieldset>
                                        <legend class="text-center">
                                            <h4>Modificar respuesta</h4>
                                        </legend>
                                    </fieldset>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-2">
                                    <label>Respuesta</label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtRepuestaActulizar" CssClass="form-control validate[required]" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-md-1">
                                    <label>Valor</label>
                                </div>
                                <div class="col-md-2">
                                    <asp:TextBox ID="txtValorActualizar" OnKeyPress = "return  puntoNumeros(event);" runat="server" CssClass="form-control validate[required,custom[puntoflotante]]"></asp:TextBox>
                                </div>
                                <div class="col-md-1">
                                    <asp:Button ID="btnActulizarRespuestas" OnClientClick="validar();" OnClick="btnActulizarRespuestas_Click" CssClass="btn btn-primary" runat="server" Text="Actualizar" />
                                </div>
                                <div class="col-md-1">
                                    <asp:Button ID="btnCancelarRespuesta" OnClientClick="quitarValidar();" OnClick="btnCancelarRespuesta_Click" CssClass="btn btn-primary" runat="server" Text="Cancelar" />
                                </div>
                            </div>                               
                        </div>
                        
                    </asp:Panel>
                    <div class="row table-condensed">
                        <div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">
                            <asp:Panel ID="pnlmostrarRespuestas" runat="server" CssClass="hscrollbar">
                                <asp:GridView ID="GvMostrarRespuestas" runat="server" CssClass="table"
                                    CellPadding="4" AutoGenerateColumns="false" AllowPaging="True"
                                    ForeColor="#333333" GridLines="None" DataKeyNames="id_respuesta" OnSelectedIndexChanged="GvMostrarRespuestas_SelectedIndexChanged" Width="100%" PageSize="10">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField DataField="id_respuesta" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="id_respuesta" SortExpression="id_respuesta" Visible="false" />
                                        <asp:BoundField DataField="Respuesta" ItemStyle-CssClass="text-center" HeaderText="Respuesta" HeaderStyle-CssClass="text-center" Visible="true" />
                                        <asp:BoundField DataField="Valor" ItemStyle-CssClass="text-center" HeaderText="Valor" HeaderStyle-CssClass="text-center" Visible="true" />
                                        <asp:CommandField ButtonType="Image" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" SelectImageUrl="~/images/update.PNG" HeaderText="Modificar" ShowSelectButton="True" />
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
                            </asp:Panel>
                            
                        </div>
                    </div>           
                    <div class="row">
                        <div class="col-md-offset-5 col-md-1">
                            <asp:Button runat="server" ID="btnRegresar" Text="Ver preguntas" CssClass="btn btn-primary" OnClick="btnRegresar_Click" OnClientClick="quitarValidar();"/>
                        </div>
                    </div>
                </asp:Panel>
          </div></div>
      



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
    <asp:HiddenField ID="hdfId_pregunta" runat="server" />
    <asp:HiddenField ID="hdfId_campus" runat="server" />
    <asp:HiddenField ID="hdid_respuesta" runat="server" />
    <asp:HiddenField ID="hdfid_pregunta_" runat="server" />
    <asp:HiddenField ID="hdfid_campus_" runat="server" />
</asp:Content>
