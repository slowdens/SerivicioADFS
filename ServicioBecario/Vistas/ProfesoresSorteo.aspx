<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="ProfesoresSorteo.aspx.cs" Inherits="ServicioBecario.Vistas.ProfesoresSorteo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
     <link rel="stylesheet"  href="../scripts/Enginee/css/validationEngine.jquery.css" type="text/css"/>    
    <link rel="stylesheet" href="../scripts/footable.bootstrap.min.css" type="text/css" /> 
    <link rel="stylesheet"  href="../scripts/Enginee/css/validationEngine.jquery.css" type="text/css"/> 
    <link rel="stylesheet" href="../scripts/Estiloss.css" type="text/css" />   
    <%--<script type="text/javascript" src="../scripts/Enginee/js/jquery-1.8.2.min.js"  charset="utf-8"></script>--%>
    <script type="text/javascript" src="../scripts/Enginee/js/jquery.validationEngine-es.js" charset="utf-8"></script>
    <script type="text/javascript" src="../scripts/Enginee/js/jquery.validationEngine.js" charset="utf-8"></script>
    
        <script>
            function fnAlerta() {
                alert("Me dio clic");
            }


            function validarTipoProfesorSorteo() {
                //Esto es para validar campos
                //validate[required]
              

                quitarValidar();
                ocultarGlobos();
                /*Quita los globos a este componente*/
                $("#<%=ddlRegla.ClientID %>").removeClass("validate[required]");

                /*Agrega los globos a estos componentes*/
                $("#<%=ddlCampus.ClientID %>").addClass("validate[required]");
                $("#<%=ddlPuesto.ClientID %>").addClass("validate[required]");
                $("#<%=ddlEnproceso.ClientID %>").addClass("validate[required]");
                jQuery("form").validationEngine();
            }
            function validarReglaAsignacion() {
                jQuery("form").validationEngine('detach');
                ocultarGlobos();
                $("#<%=ddlPuesto.ClientID %>").removeClass("validate[required]");

                $("#<%=ddlEnproceso.ClientID %>").removeClass("validate[required]");

                $("#<%=ddlCampus.ClientID %>").addClass("validate[required]");
                $("#<%=ddlRegla.ClientID %>").addClass("validate[required]");
                jQuery("form").validationEngine();
            }
            function quitarValidar() {
                //Esto es para omitir la validación
                jQuery("form").validationEngine('detach');
            }
            function ocultarGlobos() {
                jQuery("form").validationEngine('hide');
            }

            function validarRegla() {

                //Agregamos las clase
                $("#<%=ddlCampus.ClientID %>").addClass("validate[required]");


                jQuery("form").validationEngine().submit();


            }

            function DesHabilitaPuesto() {
                var tddlPuesto = document.getElementById("<%=ddlPuesto.ClientID %>");
                tddlPuesto.disabled = true;
                var ddlRegla = document.getElementById("<%= ddlRegla.ClientID %>");
                ddlRegla.disabled = true;
                var tddlCampus = document.getElementById("<%=ddlCampus.ClientID %>");
                tddlCampus.disabled = true;
                var ddlEnproceso = document.getElementById("<%= ddlEnproceso.ClientID %>");
                ddlEnproceso.disabled = false;
            }
            
            function HabilitaTodo()
            {
                var tddlPuesto = document.getElementById("<%=ddlPuesto.ClientID %>");
                tddlPuesto.disabled = false;
                var tddlCampus = document.getElementById("<%=ddlCampus.ClientID %>");
                tddlCampus.disabled = false;
                var ddlEnproceso = document.getElementById("<%=ddlEnproceso.ClientID %>");
                ddlEnproceso.disabled = false;
                var btnGuardarRegla = document.getElementById("<%= btnGuardarRegla.ClientID %>");
                btnGuardarRegla.disabled = false;
                var ddlRegla = document.getElementById("<%= ddlRegla.ClientID %>");
                ddlRegla.disabled = false;
                var btnGuardarSorteo = document.getElementById("<%= btnGuardarSorteo.ClientID %>");
                btnGuardarSorteo.disabled = false;
            }
           
            function validacionClienteDdlCampus() {
                $("#<%=ddlPuesto.ClientID %>").removeClass("validate[required]");
                $("#<%=ddlEnproceso.ClientID %>").removeClass("validate[required]");
                $("#<%=ddlRegla.ClientID %>").removeClass("validate[required]");
                $("#<%=ddlCampus.ClientID %>").addClass("validate[required]");
                //Esto es para validar campos
                //jQuery("form").validationEngine().submit();
                validar();

                $('#<%=ddlCampus.ClientID %>').focus().select();

            }
            //
            function validar() {
                //Esto es para validar campos
                jQuery("form").validationEngine();
            }
            function quitarValidar() {
                //Esto es para omitir la validación
                jQuery("form").validationEngine('detach');
            }
        </script>
    <asp:HiddenField ID="hdfActivarRol" runat="server" />
    <asp:HiddenField ID="hdfMostrarId" runat="server" />
     <div class="row">
           <div class="col-md-12">
                <h1 class="text-center">Servicio Becario</h1>
           </div>
       </div>    
    <div class="row">
        <div class="col-md-12 text-center">
           <h4>  Profesores a sorteo</h4>            
        </div>
        </div>
        <div class="jumbotron">
            <div class="row">
                <div class="col-md-1 col-md-offset-4">
                    <label>Campus</label>
                </div>
                <div class="col-md-4">
                    <asp:DropDownList ID="ddlCampus" CssClass="form-control " runat="server" OnDataBound="ddlCampus_DataBound"  OnSelectedIndexChanged="ChangeCampus_SelectedIndexChange" AutoPostBack="True" ></asp:DropDownList>
                    <asp:Label ID="lblCampusPrincipal" runat="server" ></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">                    
                    <div class="row">
                        <div class="col-md-12">
                            <fieldset>                                
                                <h4 class="text-center">Tipo porfesor a sorteo</h4> 
                                <legend class="text-center"></legend>                               
                                <div class="row">
                                    <div class="col-md-1">
                                        <label>Contrato</label>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:DropDownList ID="ddlPuesto" CssClass="form-control" runat="server" OnDataBound="ddlPuesto_DataBound"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-3">
                                        <label>¿Considerar en proceso de asignación por sorteo?</label>    
                                    </div>
                                    <div class="col-md-2">
                                        <asp:DropDownList ID="ddlEnproceso" CssClass="form-control" runat="server">
                                            <asp:ListItem Value="">--Seleccione--</asp:ListItem>
                                            <asp:ListItem Value="Si">Si</asp:ListItem>
                                            <asp:ListItem Value="No">No</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-1">
                                    <%--     <asp:LinkButton ID="btnActualizarSorteo"  visible="false" ToolTip="Actualizar" OnClientClick="validarTipoProfesorSorteo();" runat="server" CssClass="btn btn-primary" OnClick="btnActualizarSorteo_Click">
                                             <span aria-hidden="true" class="glyphicon glyphicon-pencil">Actualizar</span>
                                        </asp:LinkButton>--%>

                                        <asp:Button ID="btnActualizarSorteo" Visible="false" runat="server" Text="Actualizar"  CssClass="btn btn-primary" OnClick="btnActualizarSorteo_Click"/>


<%--                                        <asp:LinkButton ID="btnGuardarSorteo" ToolTip="Guardar" Visible="true" OnClientClick="validarTipoProfesorSorteo();" runat="server" CssClass="btn btn-primary" OnClick="btnGuardarSorteo_Click">
                                             <span aria-hidden="true" class="glyphicon glyphicon-floppy-disk">Guardar</span>
                                        </asp:LinkButton>--%>


                                        
                                        <asp:Button ID="btnGuardarSorteo" OnClientClick="validarTipoProfesorSorteo();" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardarSorteo_Click" />
                                    </div>
                                    <div class="col-md-1">
                                  <%--      <asp:LinkButton ID="btncancelarsorte" ToolTip="Cancelar" Visible="false" runat="server" CssClass="btn btn-primary" OnClick="btncancelarsorte_Click">
                                            <span aria-hidden="true" class="glyphicon glyphicon-remove">Cancelar</span>
                                        </asp:LinkButton>--%>

                                        <asp:Button ID="btncancelarsorte" Visible="false" runat="server" Text="Cancelar"  CssClass="btn btn-primary" OnClick="btncancelarsorte_Click"/>
                                    </div>
                                </div>                                
                            </fieldset>
                            <fieldset>
                                <h4 class="text-center">Regla de asignación</h4>
                                <legend></legend>
                                <div class="row">
                                    <asp:Panel ID="pnlSeleccionCampus" Visible="false" runat="server">
                                        <div class="col-md-1">
                                            <label>Campus</label>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:Label ID="lblCampus" runat="server" Text="Label"></asp:Label>
                                        </div>
                                    </asp:Panel>
                                    
                                    <div class="col-md-2">
                                        <label>Regla de asignación</label>
                                    </div>                                    
                                    <div class="col-md-2">
                                        <asp:DropDownList ID="ddlRegla" runat="server" CssClass="form-control" OnDataBound="ddlRegla_DataBound"></asp:DropDownList>
                                    </div>                                 
                                     <div class="col-md-1">

                                        <%-- <asp:LinkButton ID="btnActualizarRegla"  ToolTip="Actualizar" Visible="false"  runat="server" CssClass="btn btn-primary" OnClick="btnActualizarRegla_Click">
                                             <span aria-hidden="true" class="glyphicon glyphicon-pencil"></span>
                                        </asp:LinkButton>--%>
                                         <asp:Button ID="btnActualizarRegla" runat="server" Visible="false" Text="Actualizar" CssClass="btn btn-primary" OnClick="btnActualizarRegla_Click" />

                                       <%-- <asp:LinkButton ID="btnGuardarRegla" Text="Cancelar"  ToolTip="Guardar" OnClientClick="validarReglaAsignacion();" runat="server" CssClass="btn btn-primary" OnClick="btnGuardarRegla_Click">
                                             <span aria-hidden="true" class="glyphicon glyphicon-floppy-disk"></span>
                                        </asp:LinkButton>--%>

                                         <asp:Button ID="btnGuardarRegla" OnClientClick="validarReglaAsignacion();" runat="server" Text="Guardar"  CssClass="btn btn-primary" OnClick="btnGuardarRegla_Click" />
                                    </div>
                                    <div class="col-md-1">
                                         <%--    <asp:LinkButton ID="btncancelarRegla" ToolTip="Cancelar" Visible="false" runat="server" CssClass="btn btn-primary" OnClick="btncancelarRegla_Click">
                                                <span aria-hidden="true" class="glyphicon glyphicon-remove"></span>
                                            </asp:LinkButton>--%>
                                        <asp:Button ID="btncancelarRegla"  runat="server" Text="Cancelar"  CssClass="btn btn-primary" OnClick="btncancelarRegla_Click"  Visible="false"/>
                                    </div>
                                </div>                               
                            </fieldset>
                            <fieldset>
                                <h4 class="text-center">Mostrar configuración</h4>                               
                                <legend></legend>
                                <div class="row">
                                    <div class="col-md-12">
                                        
                                        <div class="row">
                                            <div class=" col-md-offset-4 col-md-6">
                                               <asp:CheckBox ID="chkProforSorteo"  runat="server" CssClass=" checkbox-inline checkbox" Text="Profesores a sorteo" AutoPostBack="true"    OnCheckedChanged="chkProforSorteo_CheckedChanged"  />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-offset-4 col-md-6">
                                                <asp:CheckBox ID="chkReglaAsignacion"  AutoPostBack="true" runat="server" CssClass="checkbox-inline checkbox" Text="Regla de asignación" OnCheckedChanged="chkReglaAsignacion_CheckedChanged"  />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </fieldset>
                        </div>                 
                        
                    </div>
                </div> 
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <asp:Panel ID="pnlProfesorSorteo" runat="server" Visible="false">
                    <div class="row">
                        <div class="col-md-12">
                            <fieldset>                        
                                    <legend class="text-center"><h4 class="text-center">Profesores a sorteo</h4></legend>
                            </fieldset>
                        </div>
                    </div>
                    <div class="row table-condensed">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="table-responsive">
                                <asp:Panel ID="Panel2" runat="server" CssClass="hscrollbar">
                                    <asp:GridView ID="GvProfesorSorteo" CssClass="table" Width="100%" DataKeyNames="id_auxiliar" AutoGenerateColumns="false" Height="60px" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="GvProfesorSorteo_SelectedIndexChanged">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="id_auxiliar" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="id_respuesta" SortExpression="id_respuesta" Visible="false" />
                                            <asp:BoundField DataField="Nombre_puesto" ItemStyle-CssClass="text-center" HeaderText="Puesto" HeaderStyle-CssClass="text-center" Visible="true" />
                                            <asp:BoundField DataField="Considerar" ItemStyle-CssClass="text-center" HeaderText="Considerar" HeaderStyle-CssClass="text-center" Visible="true" />
                                            <asp:BoundField DataField="Nombre" ItemStyle-CssClass="text-center" HeaderText="Campus" HeaderStyle-CssClass="text-center" Visible="true" />
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
                    </div>
                </asp:Panel>
                    
               
            </div>
            <div class="col-md-6">
                <asp:Panel ID="pnlRegla" runat="server" Visible="false">
                    <div class="row">
                        <div class="col-md-12">
                           <fieldset>
                                <legend class="text-center">
                                     <h4 class="text-center">Regla de asignación</h4>
                                </legend>
                           </fieldset>                        
                        </div>
                    </div>
                    <div class="row table-condensed">
                        <div class="col-md-12 col-sm-12 col-lg-12 col-xs-12">
                            <asp:Panel ID="pnlxx" runat="server" CssClass="hscrollbar">
                                <asp:GridView ID="GvRegla" CssClass="table" AutoGenerateColumns="false" runat="server" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="GvRegla_SelectedIndexChanged">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField DataField="Codigo_campus" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="id_campus" SortExpression="id_respuesta" Visible="false" />
                                        <asp:BoundField DataField="Nombre" ItemStyle-CssClass="text-center" HeaderText="Campus" HeaderStyle-CssClass="text-center" Visible="true" />
                                        <asp:BoundField DataField="Descripcion_regla" ItemStyle-CssClass="text-center" HeaderText="Regla" HeaderStyle-CssClass="text-center" Visible="true" />
                                        <asp:CommandField ButtonType="Image" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" SelectImageUrl="~/images/update.PNG" HeaderText="Modificar" ShowSelectButton="True" />
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
                
  
            </div>
        </div>

    <asp:HiddenField ID="hdfPuestoTexto" runat="server" />
    <asp:HiddenField ID="hdfCampusTexto" runat="server" />

  
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
