using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Web.Services;
using ServicioBecario.Codigo;
using System.Data.SqlClient;
using System.Web.Script.Serialization;


namespace ServicioBecario.Vistas
{
    public partial class Permisos_especial_nuevo : System.Web.UI.Page
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
                    ejecutaModal("Alerta", "Se agrego correctamente");
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
            //query = "exec sp_muestra_menus_de_nomina '" + txtnomina.Text + "'";
            query = "exec sp_muestra_menus_de_nomina_nuevo '" + txtnomina.Text + "'";
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

                    //chk.CssClass = "form-control letraSinegritas ";                    
                    chk.CssClass = "inputradio";
                    if (dt.Rows[i]["rol"].ToString() != "N/A")
                    {
                        chk.Checked = true;
                    }
                    celda = new TableCell();
                    if (maximo <= 5)
                    {
                        celda.CssClass = "form-control col-md-3 col-sm-1 col-xs-1 letraSinegritas";
                        celda.VerticalAlign = VerticalAlign.Middle;
                        
                        celda.Height = 50;
                        celda.Controls.Add(chk);
                        fila.Cells.Add(celda);
                        maximo++;
                    }
                    else
                    {
                        maximo = 1;
                        fila = new TableRow();
                        celda.CssClass = "form-control col-md-3 col-sm-1 col-xs-1 letraSinegritas";
                        celda.VerticalAlign = VerticalAlign.Middle;
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

            //query = "sp_usuario_rol_mostrar  " + hdfid_usuario.Value;
            query = "sp_usuario_rol_mostrar_nuevo " + hdfid_usuario.Value;
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
            //query = "exec sp_guarda_permisos_especiales " + hdfid_usuario.Value + "," + p_id_menu + " ,'" + txtFechaIncio.Text + "','" + txtFechafin.Text + "'";
            query = "exec sp_guarda_permisos_especiales_nuevo " + hdfid_usuario.Value + "," + p_id_menu + " ,'" + txtFechaIncio.Text + "','" + txtFechafin.Text + "'";
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
            //string querystatic = "exec sp_muestra_menus_de_nomina '" + nomina + "'";
            string querystatic = "exec sp_muestra_menus_de_nomina_nuevo '" + nomina + "'";
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
                        strColumnas = "<div class='col-md-3'style='height:50px' >";
                        if (columna > 0)
                        {
                            componentes = strRows + strColumnas + componentes + " <input id='che" + dt.Rows[i]["id_menu"] + "' name='radios' class='css-checkbox'   type='checkbox' value='" + dt.Rows[i]["id_menu"] + "' />  <label for='che" + dt.Rows[i]["id_menu"] + "' name='che" + dt.Rows[i]["id_menu"] + "' class='css-label'>" + dt.Rows[i]["Nombre"] + "</label> </div>  ";
                        }
                        else
                        {
                            componentes = strRows + strColumnas + componentes + " <input id='che" + dt.Rows[i]["id_menu"] + "' name='radios' class='css-checkbox'   type='checkbox' value='" + dt.Rows[i]["id_menu"] + "' />  <label for='che" + dt.Rows[i]["id_menu"] + "' name='che" + dt.Rows[i]["id_menu"] + "' class='css-label'>" + dt.Rows[i]["Nombre"] + "</label> </div> ";
                        }
                    }
                    if (columna < 3)
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
                    insertarPermisos_especial(dato, usiario, convertirFecha(inicio), convertirFecha(fin));
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
                //string query = "exec sp_guarda_permisos_especiales '" + nomina + "'," + id + ",'" + inicio + "','" + fin + "' ";
                string query = "exec sp_guarda_permisos_especiales_nuevo '" + nomina + "'," + id + ",'" + inicio + "','" + fin + "' ";
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
           // Response.Redirect("Permisos_especial_nuevo.aspx?dev=Ok");

            txtnomina.Text = "";
            LblNombreEmpleado.Text = "";
            lblRol.Text = "";
            txtFechaIncio.Text = "";
            txtFechafin.Text = "";

            ClientScript.RegisterStartupScript(this.GetType(), "prog", "limpiarComtroles();", true);
        }

        protected void chkVerRegistros_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string gil = "sasdas";
                         
            }
            catch (Exception es)
            {
                
            }
        }

        [WebMethod]
        public static object delete(string menu,string pantalla,string nomina)
        {
            string query = "sp_borra_menus_especiales '"+menu+"', '"+pantalla+"','"+nomina+"'";

            DataTable dt = getQuery(conexionBecariosestatico, query);

            return "Eliminado";
        }


        [WebMethod]
        public static object inicio(string nomina)
        {
            string json;
            //json ="[";
            json = "[";
            try
            {


                string queryes = @"select m.Nombre as Menu, m.Link Pantalla, convert(varchar(90),Fecha_inicio,103)as  inicio, CONVERT(varchar(90), Fecha_fin, 103) as fin, e.Nomina , e.Nombre+' ' + e.Apellido_paterno+' ' + e.Apellido_materno as NombreCompleto from cat_menus m inner join tbl_especiales es on m.id_menu=es.id_menu
inner join tbl_empleados e on e.id_empleado=es.id_empleado";

                DataTable dr = getQuery(conexionBecariosestatico, queryes);
                int i = 0;
                if (dr.Rows.Count > 0)
                {
                    foreach (DataRow row in dr.Rows)
                    {
                        if (i == dr.Rows.Count - 1)
                        {

                            json += "[\"" + dr.Rows[i]["Menu"].ToString() + "\",\"" + dr.Rows[i]["Pantalla"].ToString() + "\",\"" + dr.Rows[i]["inicio"].ToString() + "\",\"" + dr.Rows[i]["fin"].ToString() + "\",\"" + dr.Rows[i]["Nomina"].ToString() + "\",\"" + dr.Rows[i]["NombreCompleto"].ToString() + "\",  \"" + i + "\"]";
                            // json += "{\"id_rol\":" + dr.Rows[i]["id_rol"].ToString() + ",\"Nombre\":\"" + dr.Rows[i]["Nombre"].ToString() + "\",\"Descripcion\":\"" + dr.Rows[i]["Descripcion"].ToString() + "\"}";
                        }
                        else
                        {
                            json += "[\"" + dr.Rows[i]["Menu"].ToString() + "\",\"" + dr.Rows[i]["Pantalla"].ToString() + "\",\"" + dr.Rows[i]["inicio"].ToString() + "\",\"" + dr.Rows[i]["fin"].ToString() + "\",\"" + dr.Rows[i]["Nomina"].ToString() + "\",\"" + dr.Rows[i]["NombreCompleto"].ToString() + "\",  \"" + i + "\"],";
   
                            //json += "{\"id_rol\":" + dr.Rows[i]["id_rol"].ToString() + ",\"Nombre\":\"" + dr.Rows[i]["Nombre"].ToString() + "\",\"Descripcion\":\"" + dr.Rows[i]["Descripcion"].ToString() + "\"},";
                        }
                        i++;
                    }
                }

                json += "]";

                JavaScriptSerializer j = new JavaScriptSerializer();
                object a = j.Deserialize(json, typeof(object));

                return json;
            }
            catch (Exception es)
            {
                string error = es.Message.ToString();
            }

            return json;

        }
 
    }
}