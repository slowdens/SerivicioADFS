<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="FechaEvaluacion.aspx.cs" Inherits="ServicioBecario.Vistas.FechaEvaluacion" Culture="Auto" UICulture="Auto" %>
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
       function deshabilitaddlPeriodo()
       {
           
           var textBox = document.getElementById("<%=ddlPeriodo.ClientID %>");
           textBox.disabled = true;
       }
       function validar() {
           //Esto es para validar campos
           jQuery("form").validationEngine();
       }
       function quitarValidar() {
           //Esto es para omitir la validación
           jQuery("form").validationEngine('detach');
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
           if(dato.length<=9)
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
            <h4 class="text-center">Fechas para evaluación</h4>       
        </div>
    </div>
    <a class="btn btn-primary btn-sm" id="btnA"><em class="glyphicon glyphicon-plus"></em> Agregar</a>
    <a class="btn btn-primary btn-sm" id="btnB"><em class="glyphicon glyphicon-search"></em> Buscar</a>
    <div class="ContAll">
    <div id="ContAg">
    <asp:Panel ID="pnlCampus" runat="server" Visible="false" >
        <div class="jumbotron">
            <div class="row">
                <div class="col-md-1 col-md-offset-4">
                    <label>Campus</label>
                    <%--<asp:Label ID="Label2" runat="server" Text="Campus"></asp:Label>--%>
                </div>
                <div class="col-md-3">
                    <%--<asp:DropDownList ID="ddlCampus" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCampus_SelectedIndexChanged" OnDataBound="ddlCampus_DataBound"></asp:DropDownList>--%>
                </div>
            </div>
        </div>
    </asp:Panel>
     <div class="jumbotron">
        <div class="row">
            <div class="col-md-offset-4 col-md-1 ">
                <label>Campus</label>
                <%--<asp:Label ID="Label6" runat="server" Text="Campus: "></asp:Label>--%>
            </div>
            <div class="col-md-3">
                <asp:Label ID="lblCampusSeleccionado" runat="server" Text=""></asp:Label>
                <asp:DropDownList ID="ddlCampus" CssClass="form-control validate[required]" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCampus_SelectedIndexChanged" OnDataBound="ddlCampus_DataBound"></asp:DropDownList>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-md-1">
               <label>Periodo</label>
                <%--<asp:Label ID="Label3" runat="server" Text="Periodo"></asp:Label>--%>
            </div>
            <div class="col-md-3">
                <asp:DropDownList ID="ddlPeriodo"  CssClass="form-control validate[required] "  runat="server" OnDataBound="ddlPeriodo_DataBound"></asp:DropDownList>
            </div>
            <div class="col-md-1">
                <label>Fecha inicio</label>
                
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtFechaInicio" placeholder="dd/mm/aaaa" OnKeyPress = "return formatofecha(event,this);"  CssClass="form-control validate[required]" runat="server"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender1"  runat="server" TargetControlID="txtFechaInicio" />
            </div>
            <div class="col-md-1">
                <label>Fecha fin</label>
                <%--<asp:Label ID="Label5" runat="server" Text="Fecha fin"></asp:Label>--%>
            </div>
            <div class="col-md-2">
                <asp:TextBox ID="txtFechaFin" placeholder="dd/mm/aaaa" OnKeyPress = "return formatofecha(event,this);" CssClass="form-control validate[required]" runat="server"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender2" runat="server"  TargetControlID="txtFechaFin" />
            </div>
            <div class="col-md-1">
                
            </div>
            <div class="col-md-1">
                
            </div>
        </div>
         <div class="row">
             <div class="col-md-offset-4 col-md-1">
                 <asp:Button ID="btnAgregar" runat="server" Text="Agregar" OnClientClick="validar();" CssClass="btn btn-primary" OnClick="btnAgregar_Click" visuble="true"/>
                <asp:Button ID="BtnActualizar" runat="server" Text="Modificar" CssClass="btn btn-primary" OnClick="BtnActualizar_Click" Visible="false" OnClientClick="validar();" />
                
             </div>
             <div class="col-md-1">
                 <asp:Button ID="btncancelar" OnClientClick="quitarValidar();" Text="Cancelar" runat="server" CssClass="btn btn-primary" OnClick="btncancelar_Click" Visible="false"/>
             </div>
         </div>
        
    </div> 
        </div>
        <div id="ContBu"> 
    <asp:Panel ID="pnlVerFechas" Visible="false" runat="server">
        <div class="jumbotron">
            <div class="row">
                <div class="col-md-12 text-center">
                    <%--<center>
                         <asp:Label ID="Label7" runat="server" Text="Filtro"></asp:Label>
                    </center>--%>
                    <fieldset>
                        <legend class="text-center">
                            <h4 class="text-center">Filtros</h4>
                        </legend>
                    </fieldset> 
                </div>
            </div>
            <div class="row">
                <div class="col-md-1">
                    <label>Periodo</label>
                    <%--<asp:Label ID="Label8" runat="server" Text="Periodo"></asp:Label>--%>
                </div>
                <div class="col-md-3">
                    <asp:DropDownList ID="ddlFiltrarPeriodo" CssClass="form-control" runat="server" OnDataBound="ddlFiltrarPeriodo_DataBound"></asp:DropDownList>
                </div>
                <div class="col-md-1">
                    <label>
                        Fecha inicio
                    </label>
                  <%--  <asp:Label ID="Label9" runat="server" Text="Fecha inicio"></asp:Label>--%>
                </div>
                <div class="col-md-2">
                    <asp:TextBox CssClass="form-control" placeholder="dd/mm/aaaa" ID="txtFiltrarFechaInicio" OnKeyPress = "return formatofecha(event,this);" runat="server"></asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtFiltrarFechaInicio" />
                </div>
                <div class="col-md-1">
                    <label>Fecha fin</label>
                    <%--<asp:Label ID="Label10" runat="server" Text="Fecha fin"></asp:Label>--%>
                </div>
                <div class="col-md-2">
                    <asp:TextBox ID="txtFiltrarFechaFin" placeholder="dd/mm/aaaa" OnKeyPress = "return formatofecha(event,this);" CssClass="form-control" runat="server"></asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtFiltrarFechaFin" />
                </div>
             </div>
            <div class="row">

            
                <asp:Panel ID="pnlFiltarCampus" runat="server" Visible="false">
                    <div class="col-md-1">
                        <label>Campus</label>
                       <%-- <asp:Label ID="Label11" runat="server" Text="Campus"></asp:Label>--%>
                    </div>
                    <div class="col-md-3">
                        <asp:DropDownList ID="ddlFiltrarCampus" CssClass="form-control" runat="server" OnDataBound="ddlFiltrarCampus_DataBound"></asp:DropDownList>
                    </div>
                </asp:Panel>
                 
           </div>
            <div class="row">
                <div class="col-md-1 col-md-offset-5">
                    <asp:Button ID="btnFiltrar" CssClass="btn btn-primary" runat="server" Text="Filtrar" OnClick="btnFiltrar_Click"  OnClientClick="quitarValidar();"/>
                </div>
            </div>
            
        </div>

        <div class="row table-condensed">
            <div class="col-md-12 col-sm-12 col-xs-12 col-lg-12" >
                <asp:Panel ID="pnlxx" runat="server" CssClass="hscrollbar">
                    <asp:GridView Width="100%" ID="GVDatos" CssClass="table" PageIndex="4" AutoGenerateColumns="false" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" DataKeyNames="id_fecha_evaluacion" OnSelectedIndexChanged="GVDatos_SelectedIndexChanged" OnPageIndexChanging="GVDatos_PageIndexChanging">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="id_fecha_evaluacion" HeaderText="id_fecha_evaluacion" SortExpression="id_fecha_evaluacion" Visible="false" />
                        <asp:BoundField DataField="Campus" HeaderText="Campus" Visible="true" HeaderStyle-CssClass="text-left" ItemStyle-CssClass="text-left" />
                        <asp:BoundField DataField="Fecha_inicio" HeaderText="Fecha inicio" Visible="true" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" />
                        <asp:BoundField DataField="Fecha_fin" HeaderText="Fecha fin" Visible="true" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" />
                        <asp:BoundField DataField="Periodo" HeaderText="Periodo" Visible="true" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" />
                        <asp:CommandField SelectImageUrl="~/images/update.png" ShowSelectButton="True" HeaderText="Modificar" ButtonType="Image" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center"/>

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
    <asp:HiddenField ID="hdfActivarRol" runat="server" />
    <asp:HiddenField ID="hdfId" runat="server" />
    <asp:HiddenField ID="hdfIdSeleccion" runat="server" />


</asp:Content>
