using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Web.Services;
using ServicioBecario.Codigo;
using System.Data.SqlClient;
namespace ServicioBecario.Vistas
{
    public partial class Permisos_especial : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        static string conexionBecariosestatico = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query, caracter;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();
        protected void Page_Load(object sender, EventArgs e)
        {
            string decide = Request.QueryString["dev"];
            if (!IsPostBack)
            {
                if (decide == "Ok" && !String.IsNullOrEmpty(decide))
                {
                    ejecutaModal("Alerta", "Funciono.......");
                }
            }

        }
        //Metodo para hacer el autocomplete en el texbox
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> SearchCustomers(string prefixText, int count)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager
                        .ConnectionStrings["ServiobecarioConnectionString"].ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select Nomina from tbl_empleados  where " +
                    "Nomina like '%'+ @SearchText + '%'";
                    cmd.Parameters.AddWithValue("@SearchText", prefixText);
                    cmd.Connection = conn;
                    conn.Open();
                    List<string> customers = new List<string>();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            customers.Add(sdr["Nomina"].ToString());
                        }
                    }
                    conn.Close();
                    return customers;
                }
            }
        }

        public DataTable SacarMenus()
        {
            DataTable der;
            query = "exec sp_muestra_menus_de_nomina '" + txtnomina.Text + "'";
            der = db.getQuery(conexionBecarios, query);
            return der;
        }

        public void llenarTabla()
        {
            //Con este método lo que hago es que se agreguen los componentes checkbox en la tabla dinámica
            try
            {
                int i = 1;
                int maximo = 1;
                TableRow fila = new TableRow();
                TableCell celda = new TableCell();
                dt = SacarMenus();
                foreach (DataRow ros in dt.Rows)
                {
                    CheckBox chk = new CheckBox();
                    chk.ID = "chk" + i.ToString();
                    chk.Text = dt.Rows[i]["Nombre"].ToString();
                    chk.ToolTip = dt.Rows[i]["id_menu"].ToString();
                    chk.AutoPostBack = false;
                    chk.EnableViewState = true;
                    chk.CssClass = "letraSinegritas";
                    if (dt.Rows[i]["rol"].ToString() != "N/A")
                    {
                        chk.Checked = true;
                    }
                    celda = new TableCell();
                    if (maximo <= 5)
                    {
                        celda.CssClass = "col-md-3 col-sm-1 col-xs-1 letraSinegritas";
                        celda.VerticalAlign = VerticalAlign.Top;
                        celda.Height = 50;
                        celda.Controls.Add(chk);
                        fila.Cells.Add(celda);
                        maximo++;
                    }
                    else
                    {
                        maximo = 1;
                        fila = new TableRow();
                        celda.CssClass = "col-md-3 col-sm-1 col-xs-1 letraSinegritas";
                        celda.VerticalAlign = VerticalAlign.Top;
                        celda.Height = 50;
                        celda.Controls.Add(chk);
                        fila.Cells.Add(celda);
                    }
                    TblComponentes.Rows.Add(fila);
                    i++;
                }

            }
            catch (Exception es)
            {
                caracter = es.Message.ToString().Replace("' \t\n\r\' '.'.", " ").Replace("'", " ").Replace("\n", "").Replace("\r", "");
                caracter = caracter.Replace("'", " ");
                //db.sacarPop("Error", caracter, this);
              
            }
        }

        public void consultarNomina()
        {
            query = "exec dbo.sp_nombre_empleado '" + txtnomina.Text + "'";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                LblNombreEmpleado.Text = dt.Rows[0]["Completo"].ToString();
                hdfid_usuario.Value = dt.Rows[0]["id_empleado"].ToString();
            }

            query = "sp_usuario_rol_mostrar  " + hdfid_usuario.Value;
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                lblRol.Text = dt.Rows[0]["Nombre"].ToString();
            }
        }


        public void recorrerTabla()
        {
            int i = 1;
            foreach (TableRow fila in TblComponentes.Rows)
            {
                foreach (TableCell celda in fila.Cells)
                {

                    CheckBox chk = (CheckBox)celda.FindControl("chk" + i.ToString());
                    if (chk.Checked)
                    {
                        //insertarPermisos_especiales(chk.ToolTip);
                    }
                    i++;
                }
            }
        }
        public void insertarPermisos_especiales(string p_id_menu)
        {
            query = "exec sp_guarda_permisos_especiales " + hdfid_usuario.Value + "," + p_id_menu + " ,'" + txtFechaIncio.Text + "','" + txtFechafin.Text + "'";
            dt = db.getQuery(conexionBecarios, query);
        }
        protected void txtnomina_TextChanged(object sender, EventArgs e)
        {
            try
            {
                consultarNomina();

            }
            catch (Exception es)
            {
                caracter = es.Message.ToString().Replace("' \t\n\r\' '.'.", " ").Replace("'", " ").Replace("\n", "").Replace("\r", "");
                caracter = caracter.Replace("'", " ");
                //db.sacarPop("Error", caracter, this);
            }
        }
        public void ejecutaModal(string cabecera, string cuerpo)
        {
            lblCabeza.Text = cabecera;
            lblcuerpo.Text = cuerpo;
            ModalPopupExtender1.Show();
        }
        [WebMethod]
        public static string agregarElementos(string nomina)
        {
            int i = 0;
            int columna = 0;
            string cadena = "";
            string componentes = "";
            string strColumnas = "";
            string strRows = "";
            string querystatic = "exec sp_muestra_menus_de_nomina '" + nomina + "'";
            DataTable dt = getQuery(conexionBecariosestatico, querystatic);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (columna == 0)
                    {
                        strRows = "<div class='Row' >";
                    }
                    else
                    {
                        strRows = "";
                    }
                    if (dt.Rows[i]["rol"].ToString() != "N/A")
                    {
                        strColumnas = "<div class='col-md-3'>";
                        if (columna > 0)
                        {
                            componentes = strRows + strColumnas + componentes + "<input id='che" + dt.Rows[i]["id_menu"] + "' name='radios'  checked  class='checkbox' type='checkbox' value='" + dt.Rows[i]["id_menu"] + "' /> " + dt.Rows[i]["Nombre"] + "</div>";
                        }
                        else
                        {
                            componentes = strRows + strColumnas + componentes + "<input id='che" + dt.Rows[i]["id_menu"] + "' name='radios'  checked class='checkbox'  type='checkbox' value='" + dt.Rows[i]["id_menu"] + "' /> " + dt.Rows[i]["Nombre"] + "</div>";
                        }
                    }
                    else
                    {
                        strColumnas = "<div class='col-md-3'>";
                        if (columna > 0)
                        {
                            componentes = strRows + strColumnas + componentes + "<input id='che" + dt.Rows[i]["id_menu"] + "' name='radios' class='checkbox'  type='checkbox' value='" + dt.Rows[i]["id_menu"] + "' /> " + dt.Rows[i]["Nombre"] + "</div>";
                        }
                        else
                        {
                            componentes = strRows + strColumnas + componentes + "<input id='che" + dt.Rows[i]["id_menu"] + "' name='radios' class='checkbox'   type='checkbox' value='" + dt.Rows[i]["id_menu"] + "' /> " + dt.Rows[i]["Nombre"] + "</div>";
                        }
                    }
                    if (columna < 4)
                    {
                        columna++;
                    }
                    else
                    {
                        componentes = componentes + "</div>";
                        columna = 0;
                    }
                    i++;
                    cadena = cadena + componentes;
                    componentes = "";
                }
            }
            return cadena;
        }
        [WebMethod]
        public static string guardar(string p_encrii, string usiario, string inicio, string fin)
        {
            string[] ar = p_encrii.Split('!');

            foreach (string dato in ar)
            {
                if (dato != "")
                {
                    insertarPermisos_especial(dato, usiario,  convertirFecha( inicio), convertirFecha (fin));
                }
            }

            return "hola";
        }

        public static string convertirFecha(string fecha)
        {
            //Me entregan la fecha en formato Dia/Mes/Año            
            string mes, dia, anio;
            dia = fecha.Substring(0, 2);
            mes = fecha.Substring(3, 2);
            anio = fecha.Substring(6, 4);
            //Entrega la fecha en formato Mes/Dia/Año
            return (mes + "/" + dia + "/" + anio);
        }
        

        public static void insertarPermisos_especial(string id, string nomina, string inicio, string fin)
        {
            try
            {
                string query = "exec sp_guarda_permisos_especiales '" + nomina + "'," + id + ",'" + inicio + "','" + fin + "' ";
                DataTable dt = getQuery(conexionBecariosestatico, query); ;
                if (dt.Rows.Count > 0)
                {
                    string resu = dt.Rows[0]["Mensaje"].ToString();
                }
            }
            catch (Exception es)
            {

            }

        }
        public static DataTable getQuery(string conexion, string query)
        {
            //Se crea el datatable
            DataTable dt = new DataTable();
            //Creamos la conexion
            SqlConnection conn = new SqlConnection(conexion);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            //Llenanos nuestro  data table
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            conn.Close();
            da.Dispose();
            //Retorno mi data table
            return dt;
        }

        protected void btnGuardar_Click1(object sender, EventArgs e)
        {
            //string urlSharepoint = System.Configuration.ConfigurationManager.AppSettings["urlsharepoint"];
            //urlSharepoint = urlSharepoint.Replace("**", "&");
            //Response.Redirect("Permisos_especial.aspx?" + urlSharepoint + " &dev=Ok");
            Response.Redirect("Permisos_especial.aspx?dev=Ok");
        }

 
    }
}