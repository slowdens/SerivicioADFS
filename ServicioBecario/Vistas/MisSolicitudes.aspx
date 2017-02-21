<%@ Page Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="MisSolicitudes.aspx.cs" Inherits="ServicioBecario.Vistas.MisSolicitudes" %>
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
      
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 " >
                <h1 class="text-center">Mis Solicitudes</h1>
            </div>
        </div>
        <div>
            <asp:Label ID="Msg" runat="server" Text=""></asp:Label>
        </div>
        <div class="jumbotron">
        <fieldset>
            <div class="row">
                <div class="col-md-12 " >
                    <h4 class="text-center">Datos del solicitante</h4>
                </div>
            </div>
            <div class="row">
                <div class="col-md-1"></div>
                    <div class="col-md-2">
                        <label class="control-label">Solicitante</label>
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="LblSolicitante" runat="server" Text="LblSolicitante"></asp:Label>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label">Ubicación Física</label>
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="LblUbicacionFisica" runat="server" Text="LblUbicacionFisica"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-1"></div>
                    <div class="col-md-2">
                        <label class="control-label">Nómina</label>
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="LblNomina" runat="server" Text="LblNomina"></asp:Label>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label">Puesto</label>
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="LblPuesto" runat="server" Text="LblPuesto"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-1"></div>
                    <div class="col-md-2">
                        <label class="control-label">Departamento</label>
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="LblDepartamento" runat="server" Text="LblDepartamento"></asp:Label>
                    </div>
                    <div class="col-md-2">
                        <label class="control-label">Correo</label>
                    </div>
                    <div class="col-md-3">
                        <asp:Label ID="LblEmail" runat="server" Text="LblEmail"></asp:Label>
                    </div>
                </div>
                <div class="row" >
                    <div class="col-md-1 " ></div>
                    <div class="col-md-2 " >
                        <label for="Departamento" class="control-label">Campus</label>
                    </div>
                    <div class="col-md-3 " >
                        <asp:Label ID="DatosCampus" runat="server" Text="Label" CssClass="control-label">Datos del campus</asp:Label>
                    </div>
                    <div class="col-md-2 " >
                        <label for="DatosCampus" class="control-label">División</label>
                    </div>
                    <div class="col-md-3 " >
                        <asp:Label ID="DatosDivision" runat="server" Text="Correo" CssClass="control-label">Datos de la división</asp:Label>
                    </div>
                </div>
            <div class="row">
                <div class="col-md-1"></div>
            </div>
        </fieldset>

            </div>
            <div id="divSolicitudes" runat="server" class="row">
                <div class="col-md-12 center-block">
                        <asp:GridView ID="GrdSolicitudes" runat="server" AutoGenerateColumns="False" OnRowCommand="GrdSolicitudes_RowCommand" 
                        OnRowDataBound="GrdSolicitudes_RowDataBound" BorderStyle="Solid" BorderWidth="0px" CellPadding="4" ForeColor="#333333" 
                    	GridLines="None"  CssClass="table" Width="100%"> 
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775"/>
	                    <EditRowStyle BackColor="#999999" /> 
			            <FooterStyle BackColor="#3B83C0" Font-Bold="True" ForeColor="White" CssClass="letra" />
                    	<HeaderStyle BackColor="#3B83C0" Font-Bold="True" ForeColor="White" CssClass="letra" />
			            <PagerSettings Mode="NumericFirstLast" />
                    	<PagerStyle BackColor="#3B83C0" ForeColor="White" HorizontalAlign="Center" CssClass="letra" />
                    	<RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    	<SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    	<SortedAscendingCellStyle BackColor="#E9E7E2" />
                    	<SortedAscendingHeaderStyle BackColor="#506C8C" />
                    	<SortedDescendingCellStyle BackColor="#FFFDF8" />
                    	<SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                            <columns>
                                <asp:boundfield datafield="Solicitud" headertext="Solicitud" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center"/>
                                <asp:boundfield datafield="TipoID" headertext="TipoID" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center"/>
                                <asp:boundfield datafield="TipoDescripcion" headertext="Tipo" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center"/>
                                <asp:buttonfield buttontype="Image" commandname="Modificar" text="Modificar" HeaderText="Modificar" ImageUrl="~/images/00 banco de íconos-94-32.png" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center"/>
                                <asp:buttonfield buttontype="Image" commandname="Eliminar" text="Eliminar" ImageUrl="~/images/EliminarR.png" HeaderText="Eliminar" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center"/>
                                       
                            </columns>
                        </asp:GridView>
                </div>
            </div>
        </div>
        <div id="divAsistencia" runat="server" visible="false">
        <div class="row center-block">
            <div class="col-md-12 center-block">
                <asp:GridView ID="GrdBecario" ShowHeaderWhenEmpty="True" runat="server" AllowPaging="True"
                    BorderStyle="Solid" BorderWidth="0px" CellPadding="4" ForeColor="#333333" 
                    GridLines="None"  CssClass="letra" Width="70%" HorizontalAlign="Center">
                	<AlternatingRowStyle BackColor="White" ForeColor="#284775"/>
	                <EditRowStyle BackColor="#999999" /> 
			        <FooterStyle BackColor="#3B83C0" Font-Bold="True" ForeColor="White" CssClass="letra" />
                    <HeaderStyle BackColor="#3B83C0" Font-Bold="True" ForeColor="White" CssClass="letra" />
			        <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle BackColor="#3B83C0" ForeColor="White" HorizontalAlign="Center" CssClass="letra" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    
                </asp:GridView>
            </div>
        </div>
        <div class="row"><div class="col-md-12 " >&nbsp;</div></div>
        <div class="row center-block text-center">
            <div class="col-md-4 center-block text-center"></div>
            <div class="col-md-2 center-block text-center">
                <asp:Button ID="BtnGuardarAsistencia" runat="server" Text="Guardar Asistencia" CssClass="btn-primary" width="150px" OnClick="BtnGuardarAsistencia_Click"/>
            </div>
            <div class="col-md-2 col-md-offset-1 center-block text-center">
                <asp:Button ID="BtnGuardarAsistenciaG" runat="server" Text="Guardar Todos" CssClass="btn-primary" width="150px" OnClick="BtnGuardarAsistenciaG_Click"/>
            </div>
        </div>
            <div class="row"><div class="col-md-12 " >&nbsp;</div></div>
        <div class="row center-block text-center">
            <div class="col-md-12 center-block text-center">
                <asp:Button ID="BtnRegresar1" runat="server" Text="Regresar" CssClass="btn-primary" width="150px" OnClick="BtnRegresar1_Click"/>
            </div>
        </div>
            </div>
        <div id="divDetalleAsistencia" runat="server" visible="false">
        <div class="row center-block">
            <div class="col-md-12 center-block">
                <asp:GridView ID="GrdAsistenciaGeneral" ShowHeaderWhenEmpty="True" runat="server" AllowPaging="True"
                    	    BorderStyle="Solid" BorderWidth="0px" CellPadding="4" ForeColor="#333333" 
                    GridLines="None"  CssClass="letra" Width="70%" HorizontalAlign="Center">
                	<AlternatingRowStyle BackColor="White" ForeColor="#284775"/>
	                <EditRowStyle BackColor="#999999" /> 
			        <FooterStyle BackColor="#3B83C0" Font-Bold="True" ForeColor="White" CssClass="letra" />
                    <HeaderStyle BackColor="#3B83C0" Font-Bold="True" ForeColor="White" CssClass="letra" />
			        <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle BackColor="#3B83C0" ForeColor="White" HorizontalAlign="Center" CssClass="letra" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                </asp:GridView>
            </div>
        </div>
        <div class="row"><div class="col-md-12 " >&nbsp;</div></div>
        <div class="row center-block text-center">
            <div class="col-md-12 center-block text-center">
                <asp:Button ID="BtnRegresar2" runat="server" Text="Regresar" CssClass="btn-primary" width="150px" OnClick="BtnRegresar2_Click"/>
            </div>
        </div>
    </div>

<%--Se tiene que copiar este face para junto un metodo masa para poderse ejecuatar--%>
    <asp:Label ID="Label14" runat="server" Text=""></asp:Label>
    <!-- ModalPopupExtender -->
    <cc1:ModalPopupExtender ID="mp1" runat="server" PopupControlID="Panel1" TargetControlID="Label14" CancelControlID="cancel" BackgroundCssClass="modalBackground1">
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
 <%--Fin de la face del modal--%>   

</asp:Content>    
