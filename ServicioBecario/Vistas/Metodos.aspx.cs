using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web.Services;
namespace ServicioBecario.Vistas
{
    public partial class Metodos : System.Web.UI.Page
    {
        static string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string hora(string dato)
        {
            string query = @"select * from cat_roles";
            DataTable d = getQuery(conexionBecarios, query);
            return DateTime.Now.ToString("HH:mm:ss") + " " + dato;
        }
        [WebMethod]
        public static string generaMenu(string p_usuario)
        {
            int i = 0, incremento = 1;
            DataTable dt;
            string p_html = "", boton = "", hijos = "", query = "";          
            //query = "EXEC trae_menus_papas '" + p_usuario + "'";
            query = "EXEC trae_menus_papas_nuevo '" + p_usuario + "'";
            dt = getQuery(conexionBecarios, query);
            string[,] icons = new string[9,2];
            icons[0, 0] = "glyphicon glyphicon-qrcode"; icons[0, 1] = "Seguridad";
            icons[1, 0] = "glyphicon glyphicon-wrench"; icons[1, 1] = "Configuración";
            icons[2, 0] = "glyphicon glyphicon-ok"; icons[2, 1] = "Evaluaciones";
            icons[3, 0] = "glyphicon glyphicon-tasks"; icons[3, 1] = "Información de mi asignación";
            icons[4, 0] = "glyphicon glyphicon-list"; icons[4, 1] = "Solicitud de becarios";
            icons[5, 0] = "glyphicon glyphicon-hdd"; icons[5, 1] = "Mis solicitudes";
            icons[6, 0] = "glyphicon glyphicon-ok-sign"; icons[6, 1] = "Asistencia de becarios";
            icons[7, 0] = "glyphicon glyphicon-usd"; icons[7, 1] = "Apoyo financiero";
            icons[8, 0] = "glyphicon glyphicon-list-alt"; icons[8, 1] = "Reportes";
            if (dt.Rows.Count > 0)
            {
                p_html = "";
                foreach (DataRow row in dt.Rows)
                {
                    string icon = "";
                    int banIcon =0;
                    for (int x = 0; x < (icons.Length)/(2);x++ )
                    {
                        string nomb = row[1].ToString();
                        if(icons[x,1]==nomb)
                        {
                            icon = icons[x, 0];
                            banIcon = 1;
                        }
                    }
                    if (banIcon == 0) { icon = "glyphicon glyphicon-cog"; }
                    boton = @"<a id='" + incremento + "pa'  class='MenuPadre'  onclick='VerHijos(" + incremento + ")'><span class='"+icon+"'></span> <label  class='descP'>" + dt.Rows[i]["Nombre"].ToString() + "</label>  </a>";
                    hijos = hijosNuevos(boton, dt.Rows[i]["id_menu"].ToString(), dt.Rows[i]["id_antiguo"].ToString(), incremento, p_usuario, dt.Rows[i]["Nombre"].ToString());
                    p_html += hijos;
                    i++;
                    incremento++;
                }
            }
            return p_html;
        }

        public static string hijosNuevos(string p_html,string id_menu,string id_antiguo,int cont,string nomina,string titulopapa)
        {
            DataTable dt; int i = 0,incremento=1;
            string pp_html = p_html;
            string cadena = "", li = "", div = "", a = "";            
            string query;
            if (string.IsNullOrEmpty(id_antiguo))
            {
                //query = "trae_menus_hijos " + id_menu + ",NULL";
                query = "trae_menus_hijos_nuevos " + id_menu + ",NULL,'"+nomina+"'";
                
            }
            else
            {
                //query = "trae_menus_hijos " + id_menu + "," + id_antiguo + "";                
                query = "trae_menus_hijos_nuevos " + id_menu + "," + id_antiguo + ",'"+nomina+"'";                
            }
                        
            dt = getQuery(conexionBecarios, query);

            div = "<div id='" + cont + "p' class='CHijo'><a class='TituloH'> " + titulopapa + "</a>";
            foreach (DataRow row in dt.Rows)
            {

                a += "  <a id='" + cont + incremento + "h'   href='" + dt.Rows[i]["Link"].ToString() + "'  class='MenuHijo'>" + dt.Rows[i]["Nombre"].ToString() + "</a>";
                i++;
                incremento++;
            }
            div = div + a + "</div>";

            pp_html = pp_html + div;



            return pp_html;
         
        }


        public static string traerHijos(string p_html, string p_papas, string p_id_usuario,int cont)
        {
            DataTable dt; int i = 0;
            string pp_html = p_html;
            string cadena = "", li = "", div="", a="";
         
            string query = "sp_menus_hijos " + p_id_usuario + "," + p_papas + "";
            dt = getQuery(conexionBecarios, query);   


            div = "<div class='nav collapse' id='submenu"+cont+"' role='menu' aria-labelledby='btn-"+cont+"'>";
            foreach (DataRow row in dt.Rows)
            {
                a += "<a class='list-group-item-child'   id='hijito"+i+"'   href='" + dt.Rows[i]["Link"].ToString() + "'>" + dt.Rows[i]["Nombre"].ToString() + "</a>";
                i++;
            }
            div = div + a +  "</div>";
            
            pp_html = pp_html + div;



            return pp_html;
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

        public static void setQuery(string conexion, string query)
        {
            SqlConnection conn = new SqlConnection(conexion);
            SqlCommand my = new SqlCommand(query, conn);
            my.CommandType = CommandType.Text;
            conn.Open();
            my.ExecuteNonQuery();
            my.Connection.Close();
            conn.Close();

        }


        [WebMethod]
        public static string guardaCambios(string a, string b, string c)
        {
            return "dato";
        }
    }
}