<%@ Page Title="" Language="C#" MasterPageFile="~/Vistas/Site1.Master" AutoEventWireup="true" CodeBehind="MensajeCorreo.aspx.cs" Inherits="ServicioBecario.Vistas.MensajeCorreo" ValidateRequest="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet"  href="../scripts/Enginee/css/validationEngine.jquery.css" type="text/css"/>    
        <%--<script type="text/javascript" src="../scripts/Enginee/js/jquery-1.8.2.min.js"  charset="utf-8"></script>--%>
        <script type="text/javascript" src="../scripts/Enginee/js/jquery.validationEngine-es.js" charset="utf-8"></script>
        <script type="text/javascript" src="../scripts/Enginee/js/jquery.validationEngine.js" charset="utf-8"></script>
     <script src="../scripts/jquery/ui/1.11.4/jquery-ui.js"></script>
    <style>
        .space{
            background: url('../images/agregaCMini2.png') #027e1f no-repeat;
            width:177px;
            margin:3px;
            height:177px;
          
        }
        .Drop {
            opacity: 0.9;
            background:#027e1f;
            background-position:50%;
            display: block;
            position: fixed;
            right: -200px;
            transition: all 0.5s ease;
            top: 0px;
            height: 100%;
            width: 200px;
            padding-top: 50px;
            padding-left: 5px;
            padding-right: 5px;
            z-index:12001;
            box-shadow:3px 3px 3px #000;
        }
        .Drop:hover{
           opacity:0.9; 
        }
        .Drop:active{
           opacity:0.9; 
        }
        .contDrop{
            width:100%;
            height:100%;
            background-color:#000;
            opacity:0.7;
            display:none;
            position:fixed;
            z-index:1200;
            top:0;
            left:0;
        }
        
    </style>
        <script type="text/javascript">
         /*   var UTF8 = {
                encode: function (s) {
                    for (var c, i = -1, l = (s = s.split("")).length, o = String.fromCharCode; ++i < l;
                        s[i] = (c = s[i].charCodeAt(0)) >= 127 ? o(0xc0 | (c >>> 6)) + o(0x80 | (c & 0x3f)) : s[i]
                    );
                    return s.join("");
                },
                decode: function (s) {
                    for (var a, b, i = -1, l = (s = s.split("")).length, o = String.fromCharCode, c = "charCodeAt"; ++i < l;
                        ((a = s[i][c](0)) & 0x80) &&
                        (s[i] = (a & 0xfc) == 0xc0 && ((b = s[i + 1][c](0)) & 0xc0) == 0x80 ?
                        o(((a & 0x03) << 6) + (b & 0x3f)) : o(128), s[++i] = "")
                    );
                    return s.join("");
                }
            };*/

            function replaceAll(text, busca, reemplaza) {
             
                while (text.toString().indexOf(busca) != -1)
                    
                text = text.toString().replace(busca, reemplaza);
                
                return text;
                
            }


            function elemento(e) {
                //Con este metodo paso los datos en al textare
                var cadena="";
                var textos = $("#<%=txtCuerpoCorreo.ClientID%>").val();
                console.log(textos);
                var texto = document.getElementById('<%=txtCuerpoCorreo.ClientID%>').value;
                var array = texto.split(" ");

                texto = replaceAll(texto, "&gt;", ">");
                texto = replaceAll(texto, "&lt;", "<");
                texto = replaceAll(texto, "&nbsp;", "");
                texto = replaceAll(texto, "&amp;", "&");
              



                console.log(texto);
                texto += " !" + e + "!";
              
                $find("<%=HtmlEditorExtender1.ClientID%>")._editableDiv.innerHTML = texto;
                
                POSE();
            }
            
            function cambioDeItem()
            {
                //alert("Hola");
                jQuery("form").validationEngine();
            }
            function desactivarComponente()
            {
                var textBox = document.getElementById("<%=ddlCampus.ClientID %>");
                textBox.disabled = true;

            }
        </script>
     <div class="row">
           <div class="col-md-12">
                <h1 class="text-center">Servicio Becario</h1>
           </div>
       </div>
    <div class="row">
        <div class="col-md-12 text-center">
           <h4> Cofiguración texto correos</h4>            
        </div>
    </div>
    <div class="jumbotron">
        <div class="row">
            <div class="col col-sm-1 col-md-1">
                <label>Campus </label>
            </div>
            <div class="col-md-3 col-sm-4">
                <asp:Label ID="lblCampus" runat="server" Visible="false" Text=""></asp:Label>
                <asp:DropDownList ID="ddlCampus" AutoPostBack="true" CssClass="form-control validate[required]" runat="server" OnDataBound="ddlCampus_DataBound" OnSelectedIndexChanged="ddlCampus_SelectedIndexChanged"   ></asp:DropDownList>
            </div>
            <asp:Panel ID="PnlTipoCorreo" runat="server" Visible="false">
                <div class="col-md-3 col-sm-4">
                    <label>Seleccione tipo de correo</label>
                </div>
                <div class="col-md-5 col-sm-8">
                    <asp:DropDownList ID="ddlTipoCorreo" Visible="true" AutoPostBack="true" CssClass="form-control validate[required]" runat="server"  OnDataBound="ddlTipoCorreo_DataBound" OnSelectedIndexChanged="ddlTipoCorreo_SelectedIndexChanged"></asp:DropDownList>
                <br /><br /><br /><br /><br /><br />
                </div>
            </asp:Panel>
        </div>
        <br />
        <div class="jumbotron">            
            <div class="row">
                <div class="col-md-5">
                    <div class="row"><!--Es para la parte de campos-->
                       
                        <div class="col-md-4 text-center">
                           <asp:Panel ID="PnlCampos" runat="server" Visible="false"> <label>Campos</label></asp:Panel>
                        </div>
                        <div class="col-md-9">
                            <asp:Literal ID="Literal1" runat="server" Visible="false"></asp:Literal>
                        </div>
                    </div>                    
                </div><!--Termina la parte de campos-->
                <div class="col-md-7" >
                  
                    <fieldset>
                        <asp:Panel ID="PnlCorreo" runat="server" Visible="false">
                        <legend class="text-center">Escribe el correo</legend>
                        <div class="row">
                            <div class="col-md-1">
                                <label>Asunto </label>
                            </div>
                            <div class="col-md-11">
                                <asp:TextBox ID="txtAsunto" CssClass="form-control validate[required]" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-1 text-center">
                                <label>Cuerpo</label>
                            </div>
                            <div class="col-md-11" >
                                <!--Aqui se implementa otro codigo--->
                                <asp:TextBox CssClass="form-control  " TextMode="MultiLine" Height="300px" Text="Escribe el cuerpo de correo" ID="txtCuerpoCorreo"  runat="server"></asp:TextBox>
                                <cc1:HtmlEditorExtender  ID="HtmlEditorExtender1" EnableSanitization="false" TargetControlID="txtCuerpoCorreo" runat="server" DisplaySourceTab="false">
                                    <Toolbar>
                                        <cc1:Undo />
                                        <cc1:Redo />
                                        <cc1:Bold />
                                        <cc1:Italic />
                                        <cc1:Underline />
                                        <cc1:StrikeThrough />
                                        <cc1:Subscript />
                                        <cc1:Superscript />
                                        <cc1:JustifyLeft />
                                        <cc1:JustifyCenter />
                                        <cc1:JustifyRight />
                                        <cc1:JustifyFull />
                                        <cc1:InsertOrderedList />
                                        <cc1:InsertUnorderedList />
                                        <cc1:CreateLink />
                                        <cc1:UnLink />
                                        <cc1:RemoveFormat />
                                        <cc1:SelectAll />
                                        <cc1:UnSelect />
                                        <cc1:Delete />
                                        <cc1:Cut />
                                        <cc1:Copy />
                                        <cc1:Paste />
                                        <cc1:BackgroundColorSelector />
                                        <cc1:ForeColorSelector />
                                        <cc1:FontNameSelector />
                                        <cc1:FontSizeSelector />
                                        <cc1:Indent />
                                        <cc1:Outdent />
                                        <cc1:InsertHorizontalRule />
                                        <cc1:HorizontalSeparator />                                       
                                    </Toolbar>
                                </cc1:HtmlEditorExtender>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnGuardarMensaje" runat="server" CssClass="btn btn-primary" Text="Guardar" OnClientClick="cambioDeItem();" OnClick="btnGuardarMensaje_Click" />
                            </div>
                        </div>
                            </asp:Panel>
                    </fieldset>
                        </div>
             
            </div>
        </div>
    </div>
      <div id="droppable" class="Drop"></div>


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
    <div class="contDrop"></div>
 <%--   Fin de la face del modal--%>
    <asp:HiddenField ID="hdfid_campus" runat="server" />

    <script type="text/javascript">
        $(document).ready(function () {
     
            localStorage["TemporalEstilo"] = "";
            $("#sort1,#droppable").sortable({
                connectWith: "#sort1",
                placeholder: "space",
                revert: true,
                update: function (event,ui) {
                    exPOSE()
                elemento(ui.item.context.innerText);
                }

            }).disableSelection();
            $("#sort1 div").draggable({
                connectToSortable: "#droppable",
                helper: "clone",
                revert:true
            });
   
            $(".Drop").mouseenter(function () {
                $(".Drop").animate({ "opacity": "1" },500);
            });
            $(".Drop").mouseleave(function () {
                $(".Drop").animate({ "opacity": "0.9" },500);
            });

            $("#sort1").mousedown(function (elem) {
                var id = elem.target.id;
                POS(id);
               
             });
         
            
        })
        function actual()
        {
            $("#sort1,#droppable").sortable({
                connectWith: ".con",
                placeholder: "space",
                revert: true,
                update: function () {
                    exPOSE();
                }

            }).disableSelection();


            
        }
        $(document).click(function () {
            exPOSE();
        });

        function POS(id)
        {
            $("#" + id).animate({ "width": "170px", "height": "170px", "padding-top": "80px","border-radius":"30px"}, function () {
                $(".Drop").animate({"right": "0px"}, 50);
            });
            localStorage["TemporalEstilo"] = id;

        }
        function exPOSE() {
            POSE();
        }
        function POSE() {
            $(".Drop").animate({
                "right": "-200px"
            }, 500);
            if(localStorage["TemporalEstilo"]!="")
            {
                $("#" + localStorage["TemporalEstilo"]).animate({ "width": "180px", "height": "30px", "padding": "5px", "border-radius": "1px" });
            }
        }
    </script>
    <style>
        #ContentPlaceHolder1_HtmlEditorExtender1_ExtenderContentEditable{
            background-color:#FFF;
        }

    </style>
</asp:Content>
