<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="NominaSinBecarios.aspx.cs" Inherits="ServicioBecario.Vistas.NominaSinBecarios" Culture="Auto" UICulture="Auto" %>
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
               else {
                   return false;
               }
           }
           
       }

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
           if (dato.length <= 9)
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

   </script>
      <div class="row">
           <div class="col-md-12">
                <h1 class="text-center">Servicio Becario</h1>
           </div>
    </div>
    <div class="row">
        <div class="col-md-12 text-center">
           <h4>Nóminas que no pueden tener becarios</h4> 
            
        </div>
    </div>
    <a class="btn btn-primary btn-sm" id="btnA"><em class="glyphicon glyphicon-plus"></em> Agregar</a>
    <a class="btn btn-primary btn-sm" id="btnB"><em class="glyphicon glyphicon-search"></em> Buscar</a>
    <div class="ContAll">
    <div id="ContAg">
    <div class="jumbotron">
        <div class="row">
            <div class="col-md-1">
                <label>Nómina</label>
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtNomina" runat="server" placeholder="L00000000" OnKeyPress = "return paraNomina(event,this);" CssClass="form-control validate[required]" OnTextChanged="txtNomina_TextChanged" AutoPostBack="true"></asp:TextBox>
            </div>
            <div class="col-md-4">
                <asp:Label runat="server" ID="lblNombreSolicitante"> </asp:Label>
            </div>

        </div>
        <div class="row">
            <div class="col-md-1">
                <label>Fecha inicio</label>
            </div>
            <div class="col-md-2">
                <asp:TextBox CssClass="form-control validate[required]"  placeholder="dd/mm/aaaa"  OnKeyPress = "return formatofecha(event,this);" runat="server" ID="txtFechaInicio"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender1"  runat="server" TargetControlID="txtFechaInicio" />
            </div> 
            <div class="col-md-1">
                <label>Fecha fin</label>
            </div>
            <div class="col-md-2">
                <asp:TextBox runat="server" ID="txtFechaFin"  placeholder="dd/mm/aaaa" OnKeyPress = "return formatofecha(event,this);" CssClass="form-control validate[required]"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender2"  runat="server" TargetControlID="txtFechaFin" />
            </div>
            <div class="col-md-1">
                <label>Justificación</label>
            </div>
            <div class="col-md-4">
                <asp:TextBox ID="txtJustificacion" placeholder="Descripción" runat="server" TextMode="MultiLine" CssClass="form-control validate[required]"></asp:TextBox>
            </div>
        </div>
        <div class="row">            
            <div class="col-md-offset-4 col-md-1">
                <asp:Button runat="server" Text="Guardar" OnClientClick="validar();" ID="btnGuardar" CssClass="btn btn-primary" OnClick="btnGuardar_Click" Visible="false" />
                <asp:Button runat="server" Text="Modificar"  ID="btnModificar" CssClass="btn btn-primary" OnClientClick="validar();" OnClick="btnModificar_Click"  Visible="false"/>
            </div> 
            <div class="col-md-1">
                <asp:Button runat="server" Text="Cancelar"  ID="btnCancelar" OnClientClick="quitarValidar();" CssClass="btn btn-primary" OnClick="btnCancelar_Click"  Visible="false"/>
            </div>
           
        </div>
    </div>
        </div>
        <div id="ContBu">  
       <asp:Panel ID="pnlGridview" runat="server" Visible="false">
    <div class="jumbotron">
        <div class="row">
            <div class="col-md-12">
                <fieldset>
                    <legend class="text-center">
                        <h4>Filtros</h4>
                    </legend>
                </fieldset>
            </div>
        </div>
     
             <div class="row">
            <div class="col-md-1">
                <label>Nómina</label>
            </div>
            <div class="col-md-2">
                <asp:TextBox runat="server" ID="txtFiltrarNomina" placeholder="L00000000" OnKeyPress = "return paraNomina(event,this);" CssClass="form-control" OnTextChanged="txtFiltrarNomina_TextChanged" AutoPostBack="true"></asp:TextBox>
            </div>
            <div class="col-md-1">
                <label>Fecha inicio</label>
            </div>
            <div class="col-md-2">
                    <asp:TextBox runat="server" ID="txtfiltrarFechainicio"  placeholder="dd/mm/aaaa"  OnKeyPress = "return formatofecha(event,this);" CssClass="form-control"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender3"  runat="server" TargetControlID="txtfiltrarFechainicio" />
            </div>
            <div class="col-md-1">
                    <label>Fecha inicio</label>
            </div>
            <div class="col-md-2">
                   <asp:TextBox runat="server" ID="txtfiltrarFechafin"  placeholder="dd/mm/aaaa" OnKeyPress = "return formatofecha(event,this);" CssClass="form-control"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender4"  runat="server" TargetControlID="txtfiltrarFechafin" />
            </div>
            <div class="col-md-1">
                <label>
                    Justificación
                </label>
            </div>
            <div class="col-md-2">
                <asp:TextBox runat="server" ID="txtFiltrarJustificacion" placeholder="Descripción" TextMode="MultiLine" CssClass="form-control" Text=""></asp:TextBox>
            </div>

            
        </div>
        <div class="row">
            <div class="col-md-offset-5 col-md-1">
                   <asp:Button runat="server" ID="btnFiltrar" OnClientClick="quitarValidar();" CssClass="btn btn-primary" OnClick="btnFiltrar_Click" Text="Filtrar" />
            </div>
        </div>
            <asp:HiddenField ID="hdid" runat="server" />
        </div>
    <div class="row table-condensed">
        <div class="col-md-12 col-sm-12 col-xs-12 col-lg-12">
            <asp:Panel ID="Pnlgistror" runat="server" CssClass="hscrollbar">
                <asp:GridView ID="gvRegistros" AllowPaging="true" PageSize="20" Width="100%" runat="server" CssClass="table" DataKeyNames="id_nomina_sin_becario" AutoGenerateColumns="false" CellPadding="4" ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="gvRegistros_SelectedIndexChanged">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775"></AlternatingRowStyle>

                    <Columns>
                        <asp:BoundField DataField="id_nomina_sin_becario" HeaderText="Nomina" Visible="false" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                        <asp:BoundField DataField="nomina" HeaderText="Nómina" Visible="true" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                        <asp:BoundField DataField="fecha_inicio" HeaderText="Fecha inicio" Visible="true" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                        <asp:BoundField DataField="fecha_fin" HeaderText="Fecha fin" Visible="true" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" />
                        <asp:BoundField DataField="justificacion" HeaderText="Justificación" Visible="true" ItemStyle-CssClass="text-left" HeaderStyle-CssClass="text-left" />
                        <asp:CommandField ButtonType="Image" HeaderText="Modificar" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" SelectImageUrl="~/images/update.PNG" ShowSelectButton="True" />
                        <asp:TemplateField HeaderText="Eliminar" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="imbEliminar" CommandArgument='<%# Eval("id_nomina_sin_becario") %>' runat="server" ImageUrl="~/images/EliminarR.png" OnClick="imbEliminar_click" />
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
                    <EditRowStyle BackColor="#999999"></EditRowStyle>
                    <FooterStyle BackColor="#3B83C0" Font-Bold="True" ForeColor="White"></FooterStyle>
                    <HeaderStyle BackColor="#3B83C0" Font-Bold="True" ForeColor="White"></HeaderStyle>
                    <PagerStyle HorizontalAlign="Center" BackColor="#284775" ForeColor="White"></PagerStyle>
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
</div></div>
    





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
    



</asp:Content>
