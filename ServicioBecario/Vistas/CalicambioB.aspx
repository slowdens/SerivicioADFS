﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="CalicambioB.aspx.cs" Inherits="ServicioBecario.Vistas.CalicambioB" %>
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
    <script type="text/javascript">


        function validar() {
            //Esto es para validar campos
            if (jQuery("form").validationEngine('validate') == true) {
                $('#<%=hdfValidacion.ClientID%>').val(true);
            }
            else {
                $('#<%=hdfValidacion.ClientID%>').val(false);
                jQuery("form").validationEngine();
            }

        }
        function mostrarGlobo() {
            jQuery("form").validationEngine('validate');
            //alert("Hola");
        }
        function quitarValidar() {
            //Esto es para omitir la validación
            jQuery("form").validationEngine('detach');
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
           <div class="col-md-12">
                <h1 class="text-center">Servicio Becario</h1>
           </div>
       </div>
    <div class="row">
        <div class="col-md-12 text-center">
            <h4> Evaluación realizada a becarios  </h4>            
        </div>
    </div>
     <asp:Panel ID="Pnlmuestra" runat="server"  Visible="false">
      <div class="jumbotron">
          <div class="row">
       
        
        <asp:HiddenField ID="hdfid" runat="server" />
                                <asp:HiddenField ID="hdfperiodo" runat="server" />
                                <asp:HiddenField ID="hdfValidacion" runat="server" />
                                <asp:HiddenField ID="hdfCorreo" runat="server" />
        
            <div class="col-md-1">
                <label>Matrícula   </label>
            </div>
            <div class="col-md-2">
                    <asp:Label ID="lblMatricula" Text="Matrícula" runat="server"></asp:Label>                                          
            </div>
            <div class="col-md-3">
                    <asp:Label ID="lblNombreBecario" runat="server"></asp:Label>
            </div>
            <div class="col-md-1">
                    <label>Calificación</label>
            </div>
            <div class="col-md-3">
                    <asp:DropDownList ID="ddlCalificacion" CssClass="form-control validate[required]" runat="server" OnDataBound="ddlCalificacion_DataBound"></asp:DropDownList>
            </div>
            <div class="col-md-1">
                
            </div>
            <div class="row">
              <div class="col-md-offset-5 col-md-1">
                  <asp:Button Text="Modificar" CssClass="btn btn-primary" ID="btnActualizar" runat="server" OnClick="btnActualizar_Click" OnClientClick="validar();"/>
              </div>
              <div class="col-md-1">
                  <asp:Button Text="Cancelar" CssClass="btn btn-primary" ID="btnCancelar" runat="server" OnClick="btnCancelar_Click" OnClientClick="quitarValidar();"/>
              </div>
          </div>
          </div>
          
      </div>
    
            </asp:Panel>
  

    <div class="jumbotron">   
            <div class="row">
                <div class="col-md-12">
                    <fieldset>
                        <legend class="text-center">
                            <h4>                               
                                    Filtros
                     
                            </h4>
                        </legend>
                    </fieldset>
                </div>
            </div>         
            <div class="row">
                <div class="col-md-1">
                    <label>Matrícula</label>
                </div>
                <div class="col-md-2">
                    <asp:TextBox ID="txtfiltraMatricula" OnKeyPress="return paraMatriculas(event,this);"  CssClass="form-control" runat="server" placeholder="A00000000" ></asp:TextBox>
                </div>
                <div class="col-md-1">
                    <label>Periodo</label>
                </div>
                <div class="col-md-3">
                    <asp:DropDownList ID="ddlFiltrarPeriodo" CssClass="form-control" runat="server" OnDataBound="ddlFiltrarPeriodo_DataBound"></asp:DropDownList>
                </div>
                <div class="col-md-1">
                    <asp:Button  runat="server" id="btnFiltrar"  CssClass="btn btn-primary"  Text="Filtrar" OnClick="btnFiltrar_Click"/>
                <br /><br /><br /><br /></div><br /><br />
            </div>
        </div>
      <div class="row table-condensed">
            <div class="col-md-12 col-lg-12 col-sm-12 col-xs-12 ">
                <asp:Panel ID="Panel2" runat="server"  CssClass="hscrollbar">
                
              
                        <asp:GridView ID="gvInformacion" DataKeyNames="id_consecutivo,Correo_alumno" AutoGenerateColumns="False" runat="server" CssClass="table" CellPadding="4" ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="gvInformacion_SelectedIndexChanged" OnPageIndexChanging="gvInformacion_PageIndexChanging" AllowPaging="True" PageSize="10">

                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />

                            <Columns>
                                <asp:BoundField DataField="Correo_alumno" HeaderText="Correo_alumno" SortExpression="Correo_alumno" Visible="false" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" />
                                <asp:BoundField DataField="id_consecutivo" HeaderText="id_consecutivo" SortExpression="id_consecutivo" Visible="false" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" />
                                <asp:BoundField DataField="Matricula" HeaderText="Matrícula" SortExpression="Matricula" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" />
                                <asp:BoundField DataField="Becario" HeaderText="Becario" SortExpression="Becario" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" />
                                <asp:BoundField DataField="Becario_calificacion" HeaderText="Calificación" SortExpression="Becario_calificacion" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" />
                                <asp:BoundField DataField="Periodo" HeaderText="Periodo" SortExpression="Periodo" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" />
                                <asp:CommandField SelectImageUrl="~/images/update.png" ShowSelectButton="True" ButtonType="Image" HeaderText="Modificar" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" />
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
                                            <asp:Image ID="Image2" runat="server" ImageUrl="~/images/Alertaz.png" />
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
