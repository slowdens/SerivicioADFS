<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="ApoyoFinaciero.aspx.cs" Inherits="ServicioBecario.Vistas.ApoyoFinaciero" %>
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
    <style type="text/css">
        input[type=checkbox].css-checkbox {
            display: none;
        }

            input[type=checkbox].css-checkbox + label.css-label {
                padding-left: 20px;
                height: 15px;
                display: inline-block;
                line-height: 15px;
                background-repeat: no-repeat;
                background-position: 0 0;
                /*font-size: 15px;*/
                font-weight: normal;
                vertical-align: middle;
                cursor: pointer;
            }

            input[type=checkbox].css-checkbox:checked + label.css-label {
                background-position: 0 -15px;
            }

        .css-label {
            /*background-image: url(http://html-generator.weebly.com/files/theme/checkboxd1.png);*/
            background-image: url("../images/checkboxd1.png");
        }
    </style>

    <script>
        function soloNumeros(e) {
            key = e.keyCode || e.which;
            tecla = String.fromCharCode(key).toLowerCase();
            letras = "0123456789";
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
            leerAceptados();
        }
        function quitarValidar() {
            //Esto es para omitir la validación
            jQuery("form").validationEngine('detach');
        }
       

        function leerAceptados()
        {
            
            /*Leemos la cadena*/
            var strcadena = "";
            $(".formass:checked").each(function () {
                strcadena = strcadena + $(this).val() + "!";
            });
            //alert(strcadena);
            var campus = $('#<%=ddlCampus.ClientID%>').val();
            var periodo = $('#<%=ddPeriodo.ClientID%>').val();
            var nivel = $('#<%=ddlNivel.ClientID%>').val();



            
            $.ajax({
                async: true,
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: '{cadena:"' + strcadena + '",campus:"' + campus + '",periodo:"' + periodo + '",nivel:"' + nivel + '"}',
                url: "ApoyoFinaciero.aspx/agregarElementos",
                success: function (dd) {
                    var dato = dd.d
                    $('#verdad').empty();
                   

                }
            });

        }
        function mostrarElementos()
        {


            var campus = $('#<%=ddlCampus.ClientID%>').val();
            var periodo = $('#<%=ddPeriodo.ClientID%>').val();
            var nivel = $('#<%=ddlNivel.ClientID%>').val();
            $.ajax({
                async: true,
                type: "POST",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: '{campus:"' + campus + '",periodo:"' + periodo + '",nivel:"' + nivel + '"}',
                url: "ApoyoFinaciero.aspx/mostrar",
                success: function (dd) {
                    var dato = dd.d
                    alert(dato);
              
                    //$('#<%=lblComponentes.ClientID%>').html(dato);
                }
            });
        }
    </script> 
    <div class="row">        
        <div class="col-md-12 text-center">
            <h1>Servicio Becario</h1>            
        </div>        
    </div>
    <div class="row">        
        <div class="col-md-12 text-center">
            <h4>Tipos de  apoyo financieros</h4>            
        </div>        
    </div>
    <a class="btn btn-primary btn-sm" id="btnA"><em class="glyphicon glyphicon-plus"></em> Agregar</a>
    <a class="btn btn-primary btn-sm" id="btnB"><em class="glyphicon glyphicon-search"></em> Buscar</a>
    <div class="ContAll">
    <div id="ContAg">
    <div class="jumbotron">
        <div class="row">
            <div class="col-md-1">
                <label>Periodo </label>
            </div>
            <div class="col-md-3">
                <asp:DropDownList CssClass="form-control validate[required]" runat="server" ID="ddPeriodo" OnDataBound="ddPeriodo_DataBound"></asp:DropDownList>
            </div>
            
            <div class="col-md-1">
                <label>Campus  </label>
            </div>
            <div class="col-md-3">
                <asp:DropDownList CssClass="form-control validate[required]" runat="server" ID="ddlCampus" OnDataBound="ddlCampus_DataBound"></asp:DropDownList>
            </div>
            <div class="col-md-1">
                <label>Nivel        </label>
            </div>
            <div class="col-md-3">
                <asp:DropDownList CssClass="form-control validate[required]" runat="server" ID="ddlNivel" AutoPostBack="true" OnDataBound="ddlNivel_DataBound" OnSelectedIndexChanged="ddlNivel_SelectedIndexChanged"></asp:DropDownList>
            </div>
                 
        </div>
        <div class="row">
            <div class="col-md-12">
                <asp:Label runat="server" ID="lblComponentes"></asp:Label>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-md-offset-5 col-md-1">
                <asp:Button runat="server" ID="btnGuardar" Text="Guardar" OnClientClick="validar();" CssClass="btn btn-primary"  OnClick="btnGuardar_Click1"/>
                <asp:Button runat="server" ID="btnModificar" Text="Modificar" Visible="false" CssClass="btn btn-primary" OnClick="btnModificar_Click" />
            </div>
            <div class="col-md-1">
                <asp:Button runat="server" ID="btnCancelar" Text="Cancelar" Visible="false" CssClass="btn btn-primary" OnClick="btnCancelar_Click" />
            </div>            
        </div>
        <div class="row">
            <div class="col-md-offset-5 col-md-2">
             <%--   <asp:CheckBox runat="server" AutoPostBack="true" CssClass="checkbox-inline checkbox" Text="Mostrar apoyos" ID="chkmostrar" OnCheckedChanged="chkmostrar_CheckedChanged" />
           --%> </div>
        </div>
    </div>
         </div>
        <div id="ContBu"> 
    <asp:Panel runat="server" ID="pnlmostradosDatosGrid" Visible="false">
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
            <asp:HiddenField ID="hdfid_apoyo" runat="server" />
            <div class="col-md-1">
                <label>Periodo        </label>
            </div>
            <div class="col-md-2">
                <asp:DropDownList CssClass="form-control validate[required]" runat="server" ID="ddlfiltrarperiodo" OnDataBound="ddlfiltrarperiodo_DataBound" ></asp:DropDownList>
            </div>
            <div class="col-md-1">
                <label>Campus        </label>
            </div>
            <div class="col-md-2">
                <asp:DropDownList CssClass="form-control validate[required]" runat="server" ID="ddlfiltrarCampus" OnDataBound="ddlfiltrarCampus_DataBound" ></asp:DropDownList>
            </div>
            <div class="col-md-1">
                <label>Nivel        </label>
            </div>
            <div class="col-md-2">
                <asp:DropDownList CssClass="form-control validate[required]" runat="server" ID="ddlfiltrarNivel" OnDataBound="ddlfiltrarNivel_DataBound" ></asp:DropDownList>
            </div>
            
            <div class="col-md-1">
                <label>Tipo de apoyo        </label>
            </div>
            <div class="col-md-2">
                <asp:TextBox runat="server" ID="txtfiltrarApoyo" placeholder="Becas" CssClass="form-control "></asp:TextBox>
            </div>            
        </div>
        <div class="row">
            <div class="col-md-offset-5 col-md-1">
                <asp:Button runat="server" ID="btnFiltrar" Text="Filtrar" OnClientClick="quitarValidar();" CssClass="btn btn-primary" OnClick="btnFiltrar_Click"/>                
            </div>
        </div>
    </div>    
    <div class="row table-condensed">
        <div class="col-md-12 col-lg-12 col-xs-12 col-sm-12 ">
            <asp:Panel ID="pnlGrid" runat="server" CssClass="hscrollbar" Visible="true">
                <asp:GridView ID="gvdatos" PageSize="20" runat="server" AllowPaging="false" CssClass="table" AutoGenerateColumns="false" CellPadding="4" DataKeyNames="id_tipo_apoyo" ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="gvdatos_SelectedIndexChanged" OnPageIndexChanging="gvdatos_PageIndexChanging" OnRowDeleting="gvdatos_RowDeleting" >             
                    <Columns>
                        <asp:BoundField DataField="id_tipo_apoyo" HeaderText="id_tipo_apoyo" SortExpression="id_tipo_apoyo" Visible="false" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" ></asp:BoundField>                        
                        <asp:BoundField DataField="Tipo_apoyo" HeaderText="Tipo de apoyo" SortExpression="Tipo_apoyo" HeaderStyle-CssClass="text-left" ItemStyle-CssClass="text-left" ></asp:BoundField>                        
                        <asp:BoundField DataField="Campus" HeaderText="Campus" SortExpression="Campus" HeaderStyle-CssClass="text-left" ItemStyle-CssClass="text-left" ></asp:BoundField>                        
                        <asp:BoundField DataField="Periodo" HeaderText="Periodo" SortExpression="Periodo" HeaderStyle-CssClass="text-left" ItemStyle-CssClass="text-left" ></asp:BoundField>                        
                        <asp:BoundField DataField="Nivel" HeaderText="Nivel" SortExpression="Nivel" HeaderStyle-CssClass="text-left" ItemStyle-CssClass="text-left" ></asp:BoundField>                        
                        <%--<asp:CommandField ButtonType="Image" SelectImageUrl="~/images/update.PNG" HeaderText="Modificar" ShowSelectButton="True" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center"/>                        --%>
                        <asp:TemplateField HeaderText="Eliminar" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="imbEliminar"  CommandArgument='<%#Eval("id_tipo_apoyo")  %>'  runat="server" ImageUrl="~/images/EliminarR.png" OnClick="imbEliminar_click" />
                                <cc1:ConfirmButtonExtender ID="cbe" runat="server" DisplayModalPopupID="mpe" TargetControlID="imbEliminar">
                                </cc1:ConfirmButtonExtender>
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
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775"></AlternatingRowStyle>
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


    <asp:HiddenField ID="hdfCampus" runat="server" />
    <asp:HiddenField ID="hdfNivel" runat="server" />
    <asp:HiddenField ID="hdfPeriodo" runat="server" />

</asp:Content>


