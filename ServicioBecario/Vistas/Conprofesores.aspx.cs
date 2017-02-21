using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using ServicioBecario.Codigo;

namespace ServicioBecario.Vistas
{
    public partial class Conprofesores : System.Web.UI.Page
    {
        static string conexionBecariosestatico = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                inicio();
                llenarCampuss();
            }
        }

        [WebMethod]
        public static string llenarcampus(string menu)
        {
            string query = "select Codigo_campus,Nombre from cat_campus where Codigo_campus !='PRT'";
            string retorno="";
            DataTable dt = getQuery(conexionBecariosestatico, query);
            int i = 0;
            retorno+= "<option value=''> --Seleccione-- </option>";
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    retorno += " <option value=" + dt.Rows[i]["Codigo_campus"].ToString() + ">" + dt.Rows[i]["Nombre"].ToString() + "</option>";
                    i++;
                }
            }
            return retorno;
        }
        [WebMethod]
        public static string llenarprofesor(string menu)
        {
            string query = "select id_puesto_campus,Nombre_puesto from cat_puestos_campus where id_puesto_campus<=2";
            string retorno = "";
            DataTable dt = getQuery(conexionBecariosestatico, query);
            int i = 0;
            retorno += "<option value=''> --Seleccione-- </option>";
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    retorno += " <option value=" + dt.Rows[i]["id_puesto_campus"].ToString() + ">" + dt.Rows[i]["Nombre_puesto"].ToString() + "</option>";
                    i++;
                }
            }
            return retorno;
        }

        //pintarContratos
        [WebMethod]
        public static string pintarContratos(string campus, string profesor)
        {
         
                string query = @"                           
                                    exec sp_pinta_contrato_profesores "+campus+@"
                                ";
                int i = 0;
                int columna = 0;
                string cadena = "";
                string componentes = "";
                string strColumnas = "";
                string strRows = "";
                //string querystatic = "exec sp_muestra_menus_de_nomina '" + nomina + "'";
                DataTable dt = getQuery(conexionBecariosestatico, query);
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
                                componentes = strRows + strColumnas + componentes + "<input id='che" + dt.Rows[i]["Tipo_contrato"] + "' name='radios'  checked  class='checkbox' type='checkbox' value='" + dt.Rows[i]["Tipo_contrato"] + "' /> " + dt.Rows[i]["Contrato"] + "</div>";
                            }
                            else
                            {
                                componentes = strRows + strColumnas + componentes + "<input id='che" + dt.Rows[i]["Tipo_contrato"] + "' name='radios'  checked class='checkbox'  type='checkbox' value='" + dt.Rows[i]["Tipo_contrato"] + "' /> " + dt.Rows[i]["Contrato"] + "</div>";
                            }
                        }
                        else
                        {
                            strColumnas = "<div class='col-md-3'style='height:50px' >";
                            if (columna > 0)
                            {
                                componentes = strRows + strColumnas + componentes + " <input id='che" + dt.Rows[i]["Tipo_contrato"] + "' name='radios' class='css-checkbox'   type='checkbox' value='" + dt.Rows[i]["Tipo_contrato"] + "' />  <label for='che" + dt.Rows[i]["Tipo_contrato"] + "' name='che" + dt.Rows[i]["Tipo_contrato"] + "' class='css-label'>" + dt.Rows[i]["Contrato"] + "</label> </div>  ";
                            }
                            else
                            {
                                componentes = strRows + strColumnas + componentes + " <input id='che" + dt.Rows[i]["Tipo_contrato"] + "' name='radios' class='css-checkbox'   type='checkbox' value='" + dt.Rows[i]["Tipo_contrato"] + "' />  <label for='che" + dt.Rows[i]["Tipo_contrato"] + "' name='che" + dt.Rows[i]["Tipo_contrato"] + "' class='css-label'>" + dt.Rows[i]["Contrato"] + "</label> </div> ";
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
        public static string guardar(string seleccionados, string id_campus, string id_profesor)
        {
            //Manda todos una serie de permisos casados a una nomina
            string[] ar = seleccionados.Split('!');
            string retorno = "";
            foreach (string dato in ar)
            {
                if (dato != "")
                {
                    retorno = insertarcontrato(id_profesor, dato, id_campus);

                }
            }

            return retorno;
        }
        public void inicio()
        {
            try
            {
                string queryes = @"exec sp_mostrar_campus_comtraro_puesto   ";
                if (CampusFiltro.SelectedValue != "") { queryes += "'" + CampusFiltro.SelectedValue + "'"; }
                DataTable dr = getQuery(conexionBecariosestatico, queryes);
                if(dr.Rows.Count>0)
                {
                    DatosGrid.DataSource = dr;
                    DatosGrid.DataBind();
                }
                else
                {
                    DatosGrid.DataSource = null;
                    DatosGrid.DataBind();
                }



            }catch(Exception ex)
            {
                verModal("Error",ex.Message);
            }
        }
        public void llenarCampuss()
        {
            BasedeDatos obj = new BasedeDatos();
          
            string query = "select Codigo_campus,Nombre from cat_campus order by Nombre";
            DataTable dt = obj.getQuery(conexionBecariosestatico, query);
            if (dt.Rows.Count > 0)
            {
                CampusFiltro.DataValueField = "Codigo_campus";
                CampusFiltro.DataTextField = "Nombre";
                CampusFiltro.DataSource = dt;
                CampusFiltro.DataBind();
            }
        }
        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }
       /* [WebMethod]
          public static string inicio(string nomina)
        {
            string retorno = "";
            string json;
            json = "[";
            try{
                string queryes = @"exec sp_mostrar_campus_comtraro_puesto   ";
                DataTable dr = getQuery(conexionBecariosestatico, queryes);
                int i = 0;
                if (dr.Rows.Count > 0)
                {
                    foreach (DataRow row in dr.Rows)
                    {
                        if (i == dr.Rows.Count - 1)
                        {
                            json += "[\"" + dr.Rows[i]["Tipo_contrato"].ToString() + "\",\"" + dr.Rows[i]["Descripcion_contrato"].ToString() + "\",\"" + dr.Rows[i]["Campus"].ToString() + "\",\"" + dr.Rows[i]["Nombre_puesto"].ToString() + "\",  \"" + i + "\",\"" + dr.Rows[i]["id_conse"].ToString() + "\"]";
                        }
                        else
                        {
                            json += "[\"" + dr.Rows[i]["Tipo_contrato"].ToString() + "\",\"" + dr.Rows[i]["Descripcion_contrato"].ToString() + "\",\"" + dr.Rows[i]["Campus"].ToString() + "\",\"" + dr.Rows[i]["Nombre_puesto"].ToString() + "\",  \"" + i + "\",\"" + dr.Rows[i]["id_conse"].ToString() + "\"],";
                        }
                        i++;
                    }
                }

                json += "]";

                JavaScriptSerializer j = new JavaScriptSerializer();
                object a = j.Deserialize(json, typeof(object));

                
            }catch(Exception es){
                json = es.Message.ToString();
            }

            return json;
        }
      [WebMethod]
        public static string iniciouno(string nomina)
        {
            string retorno = "";
            string json;
            json = "[";
            try
            {
                string queryes = @"exec sp_mostrar_campus_comtraro_puesto   ";
                string secuente;
                DataTable dr = getQuery(conexionBecariosestatico, queryes);
                int i = 0;
                if (dr.Rows.Count > 0)
                {
                    foreach (DataRow row in dr.Rows)
                    {
                        if (i == dr.Rows.Count - 1)
                        {
                           // json += "{\"NombreComleto\":\"" + dt.Rows[i]["NombreCompleto"].ToString() + "\",\"Rol\":\"" + dt.Rows[i]["Rol"].ToString() + "\",\"Mensaje\":\"Ok\"}";
                            secuente = "row_" + dr.Rows[i]["id_conse"].ToString();
                            json += "{\"DT_RowId\": \"" + secuente + "\",\"Tipo contrato\":\"" + dr.Rows[i]["Tipo_contrato"].ToString() + "\",\"Descripcion\":\"" + dr.Rows[i]["Descripcion_contrato"].ToString() + "\",\"Campus\":\"" + dr.Rows[i]["Campus"].ToString() + "\",\"Puesto\":\"" + dr.Rows[i]["Nombre_puesto"].ToString() + "\",  \"i\":\"" + i + "\",\"id_conse\":\"" + dr.Rows[i]["id_conse"].ToString() + "\"}";
                        }
                        else
                        {
                            //json += "[\"" + dr.Rows[i]["Tipo_contrato"].ToString() + "\",\"" + dr.Rows[i]["Descripcion_contrato"].ToString() + "\",\"" + dr.Rows[i]["Campus"].ToString() + "\",\"" + dr.Rows[i]["Nombre_puesto"].ToString() + "\",  \"" + i + "\",\"" + dr.Rows[i]["id_conse"].ToString() + "\"],";
                            secuente = "row_" + dr.Rows[i]["id_conse"].ToString();
                            json += "{\"DT_RowId\": \"" + secuente + "\",\"Tipo contrato\":\"" + dr.Rows[i]["Tipo_contrato"].ToString() + "\",\"Descripcion\":\"" + dr.Rows[i]["Descripcion_contrato"].ToString() + "\",\"Campus\":\"" + dr.Rows[i]["Campus"].ToString() + "\",\"Puesto\":\"" + dr.Rows[i]["Nombre_puesto"].ToString() + "\",  \"i\":\"" + i + "\",\"id_conse\":\"" + dr.Rows[i]["id_conse"].ToString() + "\"},";
                        }
                        i++;
                    }
                }

                json += "]";

                JavaScriptSerializer j = new JavaScriptSerializer();
                object a = j.Deserialize(json, typeof(object));


            }
            catch (Exception es)
            {
                json = es.Message.ToString();
            }

            return json;
        }*/


        public static string insertarcontrato(string id_profesor, string tipo_contrato,string id_campus)
        {
            //Inserta un permiso por nomina dentro de la base de datos
            string retorno = "";
            try
            {   
                string query = "exec sp_guardar_contrato_profesor_campus_masiva_sorteo "+id_profesor+","+id_campus+",'"+tipo_contrato+"'";
                DataTable dt = getQuery(conexionBecariosestatico, query); ;
                if (dt.Rows.Count > 0)
                {
                    retorno = dt.Rows[0]["Mensaje"].ToString();
                }
            }
            catch (Exception es)
            {
                retorno = es.Message.ToString();
            }
            return retorno;
            
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

        [WebMethod]
        public static string eleminardatos(string id)
        {
            //Elimina a permiso temporal de una nomina, dentro de la base de datos.
            string query = "sp_elimina_contrato_campus_profesor_seleccionado " + id + "";

            DataTable dt = getQuery(conexionBecariosestatico, query);

            return "Eliminado";
        }

        protected void DatosGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
            DatosGrid.PageIndex = e.NewPageIndex;
            inicio();
            }catch(Exception ex)
            {
                verModal("Error",ex.Message);
            }
            
        }

        protected void DatosGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                inicio();
            }catch(Exception ex)
            {
                verModal("Error",ex.Message);
            }
        }

        protected void imbEliminar_Click(object sender, ImageClickEventArgs e)
        {

            try
            {
                string id = (sender as ImageButton).CommandArgument;
                string query = "exec sp_elimina_contrato_campus_profesor_seleccionado " +  id + "";
                DataTable dt = getQuery(conexionBecariosestatico, query);
                inicio();
            }catch(Exception ex)
            {
                verModal("Error",ex.Message);
            }
           

           
        }

        protected void CampusFiltro_DataBound(object sender, EventArgs e)
        {
            CampusFiltro.Items.Insert(0, new ListItem("-- Seleccione --", ""));
        }

        protected void Filtro_Click(object sender, EventArgs e)
        {
            inicio();
        }

    }
}