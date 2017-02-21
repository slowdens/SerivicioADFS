using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Collections;
using ServicioBecario.Codigo;
using System.Web.Services;
using System.Data.SqlClient;


namespace ServicioBecario.Vistas
{
    public partial class ProcesoAsignacion : System.Web.UI.Page
    {
        static string conexionBecariosTatic = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query, caracter;
        static string querys;
        DataTable dt;
      //  Usuario usuario = new Usuario();   
        BasedeDatos db = new BasedeDatos();
        protected void Page_Load(object sender, EventArgs e)
        {
            // usuario.Nomina= Session["Usuario"].ToString();
            hdfUsuario.Value = Session["Usuario"].ToString();
            if(!IsPostBack)
            {

                vermisosdeCampus();
                Periodo();
            }
        }
        
       public void campus()
        {
            query = "select Codigo_campus,Nombre from cat_campus where Codigo_campus!='PRT' order by Nombre asc";
            dt = db.getQuery(conexionBecarios,query);
            if(dt.Rows.Count>0)
            {
                ddlCampus.DataTextField = "Nombre";
                ddlCampus.DataValueField = "Codigo_campus";
                ddlCampus.DataSource = dt;
                ddlCampus.DataBind();
            }
        }

       public void vermisosdeCampus()
       {
           query = "Sp_muestra_perifil_campus'" + Session["Usuario"].ToString() + "'";
           dt = db.getQuery(conexionBecarios, query);
           if (dt.Rows.Count > 0)
           {
               if (dt.Rows[0]["id_rol"].ToString() == "4")//Este es administrador multicampus
               {
                   hdfActivarRol.Value = "1";                  
                   campus();
               }
               else
               {
                   lblCampus.Text = dt.Rows[0]["Campus"].ToString();
                   hdf_id_campus.Value = dt.Rows[0]["Codigo_campus"].ToString();
                   lblCampus.Visible = true;
                   ddlCampus.Visible = false;
                   hdfActivarRol.Value = "0";
               }
           }
           else
           {
               verModal("Alerta", "El usuario no se encontró registrado");
           }
       }


        public void Periodo()
        {
            query = "select Periodo,Descripcion from cat_periodos where Activo=1";
            dt = db.getQuery(conexionBecarios ,query);
            if(dt.Rows.Count>0)
            {
                ddlPeriodo.DataValueField = "Periodo";
                ddlPeriodo.DataTextField = "Descripcion";
                ddlPeriodo.DataSource = dt;
                ddlPeriodo.DataBind();
            }
        }

        protected void btprueba_Click(object sender, EventArgs e)
        {
            
            if(hdfActivarRol.Value=="1")
            {
                hdf_id_campus.Value = ddlCampus.SelectedValue;
            }
            query = "sp_verifica_fin_fecha_solicitud "+ddlPeriodo.SelectedValue+","+hdf_id_campus.Value+" ";
            dt = db.getQuery(conexionBecarios,query);
            if(dt.Rows.Count>0)
            {
                
                if(dt.Rows[0]["Mensaje"].ToString()=="Si")
                {
                    //Ya estamos fuera de las fechas de solicitudes                    
                    query = "sp_execucion_procesos_asignacion " + ddlPeriodo.SelectedValue + "," + hdf_id_campus.Value + ",1";
                    dt = db.getQuery(conexionBecarios,query);
                    llenarGrid(1);
                    hdfTipoEjecucion.Value = "1";
                    lblMensajito.Text = @"<div class='colores'>
                                               Resultados de prueba
                                               </div>
                                               
                                            ";
                }
                else
                {
                    verModal("Alerta", "Esta función se activa hasta que se terminen las fechas de solicitudes.");
                }
            }

        }

        protected void ddlCampus_DataBound(object sender, EventArgs e)
        {
            ddlCampus.Items.Insert(0, new ListItem("--Seleccione--", ""));
        }

        protected void btnConfigurar_Click(object sender, EventArgs e)
        {
            try
            {
                string hmtl;
                hmtl = "";
                pnlGeneral.Visible = true;

                if (hdfActivarRol.Value == "1")
                {
                    hdf_id_campus.Value = ddlCampus.SelectedValue;
                }


                //Mustra el compus que esta seleccionando

                /*Tomamos los procesos que esta en cero*/
                query = "sp_configuracion_proceso_asignacion_inicio_Cero " + hdf_id_campus.Value + ",'" + Session["Usuario"].ToString() + "' ";
                dt = db.getQuery(conexionBecarios,query);
                hmtl += @"<div class='contDrag'>
                            <table>
                                <tbody>
                                    <tr>
                                        <td>
                                            <label>Prioridades No Asignadas</label>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <label> Prioridades Asignadas </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <ul id='sortable1'>";
                                                foreach(DataRow registros in dt.Rows)
                                                {
                                                    hmtl += @"<li class='elementli' id='" + registros["id_proceso_asignacion_campus"] + "'>" + registros["Nombre"] + "<hr width='80%'></li>";
                                                }
                                   hmtl += @"</ul>
                                        </td>
                                        <td>
                                            <img src='../images/flecha_asignacion.png' width='50px' height='50px' title='Arrastre los elementos de manera \n ascendente para asignarles prioridad'>
                                        </td>
                                        <td>
                                            <ul id='sortable2'>";

                                            query = "sp_configuracion_proceso_asignacion_inicio_ordenado " + hdf_id_campus.Value + "";
                                            dt = db.getQuery(conexionBecarios,query);

                                            foreach(DataRow registros in dt.Rows)
                                            {
                                                hmtl += @"<li class='elementli' id='" + registros["id_proceso_asignacion_campus"] + "'>" + registros["Nombre"] + "<hr width='80%' ></li>";
                                                ClientScript.RegisterStartupScript(GetType(), "ocultarbotoones" + registros["Prioridad"], "iniciarElementos(" + registros["Prioridad"] + ");", true);
                                            }
                                   hmtl += @"</ul>
                                        </td>
                                        <td style='vertical-align:top;'>
                                            <img id='img-ord1' style='display:none;width:30px;height:72px' src='../images/1.png'  >
                                            <img id='img-ord2' style='display:none;width:30px;height:72px' src='../images/2.png' >
                                            <img id='img-ord3' style='display:none;width:30px;height:72px' src='../images/3.png' >
                                            <img id='img-ord4' style='display:none;width:30px;height:72px' src='../images/4.png' >
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                    </div>";
                Label1.Text = hmtl;


                llenarGridProcesoConfiguracion();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }
        public void llenarGridProcesoConfiguracion()
        {
            int i = 0;
            string numeros = "";
            //query = "Sp_configuracion_por_campus " + ddlCampus.SelectedValue + " ";
            if(hdfActivarRol.Value=="1")
            {
                hdf_id_campus.Value = ddlCampus.SelectedValue;

            }
            query = "Sp_configuracion_proceso_asignacion_por_campus  " + hdf_id_campus.Value + " ";
            dt = db.getQuery(conexionBecarios, query);
            
            
        }
        

        
         public void actualizarPrioridad()
        {
            //query = "sp_configuracion_por_proceso_asignacion_actualizar " + hdf_id_proceso_asignacion_campus.Value + "," + hdf_id_precoso_asignacion.Value + "," + hdf_id_campus.Value + "," + ddlPrioridad.SelectedValue + "";
           // query = "sp_configuracion_proceso_asignacion_actualizar_datos " + hdf_id_proceso_asignacion_campus.Value + "," + hdf_id_precoso_asignacion.Value + "," + hdf_id_campus.Value + "," + ddlPrioridad.SelectedValue + "";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
            {
       
                verModal("Éxito", "Se modificó correctamente la información");
            }
        }

        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }

        
        protected void btnGuarConfiguracion_Click(object sender, EventArgs e)
        {
            try
            {
                if(dt !=null)
                {
                    dt.Clear();
                }
                gvCorreAsignaciones.DataSource = null;
                gvCorreAsignaciones.DataBind();

                gvSinasignar.DataSource = null;
                gvSinasignar.DataBind();

                lbltitiloGrid.Text = "";
                query = "sp_eliminar_asignacion_de_prueba ";
                dt = db.getQuery(conexionBecarios,query);
                if(hdfActivarRol.Value=="1")
                {
                    hdf_id_campus.Value = ddlCampus.SelectedValue;
                }
                /*Validamos que las fechas este dentro del valor deceado*/
                query = "sp_fechas_fin_proceso_inscripcion '" + ddlPeriodo.SelectedValue + "','" + hdf_id_campus.Value + "'";
                dt = db.getQuery(conexionBecarios,query);
                if(dt.Rows.Count>0)
                {
                    
                    /*Vemos si esta dentro del periodo*/
                    if (dt.Rows[0]["Mensaje"].ToString()=="Ok")
                    {
                        query = "sp_execucion_procesos_asignacion '" + ddlPeriodo.SelectedValue + "','" + hdf_id_campus.Value + "',2 ";
                        dt = db.getQuery(conexionBecarios, query);
                        llenarGrid(2);
                        hdfTipoEjecucion.Value = "2";

                        lblMensajito.Text = @"<div class='colores'>
                                               Resultados de asignación
                                               </div>
                                          
                        
                                ";
                        //Sacamos la información de las solicitudes sin asignar
                        llenarSinAsignar();
                    }
                    else
                    {
                        verModal("Alerta","Todavía no concluye el periodo de solicitudes!!");
                    }
                    

                }
                
            }catch(Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }

        public void llenarGrid(int tipo)
        {
            pnlGridview.Visible = true;

            if(dt !=null)
            {
                dt.Clear();
            }
            //Limpiamos gridview    
            gvCorreAsignaciones.DataSource = null;
            gvCorreAsignaciones.DataBind();

            if (hdfActivarRol.Value == "1")
            {
                //tipo 1 prueba 2 guardar
                query = "sp_sacar_informacion_de_asignaciones_corridas " + tipo + "," + ddlPeriodo.SelectedValue + " ,'"+ddlCampus.SelectedValue+"'";
            }
            else
            {
                query = "sp_sacar_informacion_de_asignaciones_corridas " + tipo + "," + ddlPeriodo.SelectedValue + ", '" + hdf_id_campus.Value + "'";
            }
            
            dt = db.getQuery(conexionBecarios,query);    
            if(dt.Rows.Count>0)
            {
                gvCorreAsignaciones.DataSource = dt;
                gvCorreAsignaciones.DataBind();
                ViewState["dt"] = dt;
            }
        }
    

        [WebMethod]
        public static string guardaCambios(string element_s1, string element_s2, string id_campus)
        {
            string usuario= HttpContext.Current.Session["Usuario"].ToString();
            DataTable dt;            
            string[] arr1 = element_s1.Split(",".ToCharArray());
            string[] arr2 = element_s2.Split(",".ToCharArray());

            /*esta parte es para guardar los metodos no utilizados**/
            if(!string.IsNullOrWhiteSpace(arr1[0]))
            {
                for(int x=arr1.Length-1;x>=0;x--)
                {
                    querys = "sp_procesos_no_utilizados " + id_campus + "," + arr1 [x]+ ",'"+usuario+"'";
                    dt = getQuery(conexionBecariosTatic,querys);
                }
            }
            


            /*esta parte es para guardar los metodos asignados*/
            if (!string.IsNullOrWhiteSpace(arr2[0]))
            {
                for (int x = arr2.Length - 1; x >= 0; x--)
                {
                    querys = "sp_proceso_utilizados "+id_campus+","+arr2[x]+","+(x+1)+",'"+usuario+"'";
                    dt = getQuery(conexionBecariosTatic,querys);
                }
            }


            return "";
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

        protected void ddlPeriodo_DataBound(object sender, EventArgs e)
        {
            ddlPeriodo.Items.Insert(0, new ListItem("--Seleccione--", ""));
        }

        protected void imgbtnDescargar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                dt = (DataTable)ViewState["dt"];
                descargarReporte( dt);
            }catch(Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        public void descargarReporte(DataTable ds)
        {
            if (ds != null)
            {
                string attachment = "attachment; filename=log_asignaciones.xls";
                string columnas = "", reglones = "", html = "";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";
                Response.Charset = "UTF-8";
                html = @"<table>
                            <tr>
                                <td colspan='6' style='text-align:center;font-size:20px;color:#113FB9'>
                                    REPORTE DE ASIGNADOS
                                </td>
                            </tr>
                            <tr>";
                foreach (DataColumn dc in ds.Columns)
                {
                    columnas += "<th>" + dc.ColumnName + "</th>";
                }
                html += "</tr>" + columnas;
                int i;
                foreach (DataRow dr in ds.Rows)
                {
                    reglones += "<tr>";
                    for (i = 0; i < ds.Columns.Count; i++)
                    {
                        reglones += "<td>" + dr[i].ToString() + "</td>";
                    }
                    reglones += "</tr>";

                }
                html += reglones + "</table>";
                Response.Write(html);
                Response.End();
            }
            else
            {
                verModal("Alerta", "No hay información para descargar");
            }
        }

        protected void gvCorreAsignaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvCorreAsignaciones.PageIndex = e.NewPageIndex;
                llenarGrid(int.Parse(hdfTipoEjecucion.Value));
            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }

        protected void gvSinasignar_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvSinasignar.PageIndex = e.NewPageIndex;
                llenarSinAsignar();
            }catch(Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }

        public void llenarSinAsignar()
        {
            //Asignamos la cantidad al grid si es huviera información
            query = "exec sp_SolicitudesNo_asignadas '" + ddlPeriodo.SelectedValue + "','" + hdf_id_campus.Value + "'";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                verModal("Alerta", "Existen " + dt.Rows[0]["Cantidad"].ToString() + " solicitudes sin asignar");
                gvSinasignar.DataSource = dt;
                gvSinasignar.DataBind();
                lbltitiloGrid.Text = @"<div class='colores'>
                                               Solcitude sin asignar
                                               </div>";

            }

        }
    }
}