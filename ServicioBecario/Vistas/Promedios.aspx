<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="Promedios.aspx.cs" Inherits="ServicioBecario.Vistas.Promedios" %>

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
        function validar() {
            //Esto es para validar campos
            jQuery("form").validationEngine();
        }
        function quitarValidar() {
            //Esto es para omitir la validación
            jQuery("form").validationEngine('detach');
        }
        function soloNumeros(e) {
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
           <div class="col-md-12">
                <h1 class="text-center">Servicio Becario</h1>
           </div>
       </div>  
    <div class="row">
        <div class="col-md-12 text-center">
             <h4>Promedios</h4>            
        </div>
    </div>
    <div class="jumbotron">
        <div class="row">
            <div class=" col-md-offset-3 col-md-1">
                <label>Campus</label>
            </div>
            <div class="col-md-3">
                <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlCampus" CssClass="form-control" OnSelectedIndexChanged="ddlCampus_SelectedIndexChanged" OnDataBound="ddlCampus_DataBound"></asp:DropDownList>
            </div>
        </div>
        <asp:Panel ID="PnlModificacion" runat="server" Visible="false">
            <div class="row">
                <div class="col-md-1">
                    <label>Mayor o igual que</label>
                </div>
                <div class="col-md-2">
                    <asp:HiddenField ID="hdi_id_promedio" runat="server" />
                    <asp:TextBox Text="" runat="server" ID="txtMayor" OnKeyPress = "return  soloNumeros(event);" CssClass="form-control validate[required,custom[puntoflotante]]"></asp:TextBox>
                </div>
                <div class="col-md-1">
                    <label>Menor o igual que</label>
                </div>
                <div class="col-md-2">
                    <asp:TextBox Text="" runat="server" ID="txtMenor" OnKeyPress = "return  soloNumeros(event);" CssClass="form-control validate[required,custom[puntoflotante]]"></asp:TextBox>
                </div>
                
                <div class="col-md-1">
                    <label>Estatus</label>
                </div>
                <div class="col-md-2">
                    <asp:DropDownList ID="ddlEstatus" runat="server" CssClass="form-control" OnDataBound="ddlEstatus_DataBound"  ></asp:DropDownList>
                </div>
                <div class="col-md-1">
                    <asp:Button ID="btnModificar" Text="Modificar" OnClientClick="validar();" runat="server" CssClass="btn btn-primary" OnClick="btnModificar_Click" />
                </div>
                <div class="col-md-1">                   
                    <asp:Button ID="btnCancelar" Text="Cancelar" OnClientClick="quitarValidar();" runat="server" CssClass="btn btn-primary" OnClick="btnCancelar_Click" />
                </div>
            </div>
        </asp:Panel>
        
    </div>
    <asp:Panel runat="server" ID="pnlgrid" Visible="false">
            <div class="row table-condensed">
                <div class="col-md-12 col-sm-12 col-lg-12 col-xs-12">
                    <asp:Panel runat="server" ID="pnlGridDos" CssClass="hscrollbar">
                        <asp:GridView ID="gvDatos" DataKeyNames="id_promedio" Width="100%" CssClass="table" runat="server" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false" GridLines="None" OnSelectedIndexChanged="gvDatos_SelectedIndexChanged">
                            <AlternatingRowStyle  BackColor="White" ForeColor="#284775"></AlternatingRowStyle>
                            <Columns>
                                
                                <asp:BoundField DataField="id_promedio" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="id_promedio" SortExpression="id_promedio" Visible="false" />
                                <asp:BoundField DataField="Nombre" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="Nombre" Visible="true" />
                                <asp:BoundField DataField="Mayor_o_igual_que" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="Mayor o igual Que" Visible="true" />
                                <asp:BoundField DataField="Menor_o_igual_que" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="Menor o igual Que" Visible="true" />
                                <asp:BoundField DataField="Estatus" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center" HeaderText="Estatus" Visible="true" />
                                <asp:CommandField ButtonType="Image" HeaderText="Modificar" SelectImageUrl="~/images/update.PNG" ShowSelectButton="True"  HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center"/>
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
