<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="corros.aspx.cs" Inherits="ServicioBecario.Vistas.corros" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script type="text/javascript">
         function paraMatriculasdos(e, control) {
            var dato;
            key = e.keyCode || e.which;
            tecla = String.fromCharCode(key).toLowerCase();
            letras = "a0123456789";
            especiales = "8-37-39-46";
            var array = especiales.split("-");
            tecla_especial = false

           
            

             
            //for (var i in especiales) {
            //    dato = especiales[i];
            //    if (key == especiales[i]) {
            //        tecla_especial = true;
            //        break;

            //    }
            //}

            for (var i in array)
            {
                var ar = array[i];
                if (key == array[i])
                {
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
                if (key == "8") {
                    return true
                }
                else {
                    return false;
                }

            }

        }


    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox runat="server" ID="txtejemplo"></asp:TextBox>
        <asp:Button  ID="Button1" runat="server" Text="Correo" OnClick="Button1_Click" />
    
        <asp:Button  ID="Button2" runat="server" Text="llenargrid" OnClick="Button2_Click" />
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" PageSize="100" OnPageIndexChanging="GridView1_PageIndexChanging"></asp:GridView>
        
    </div>
    </form>
</body>
</html>
