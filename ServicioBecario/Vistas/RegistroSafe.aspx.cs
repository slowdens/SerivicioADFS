using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using ServicioBecario.Codigo;


namespace ServicioBecario.Vistas
{
    public partial class RegistroSafe : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();
        Safe safe = new Safe();
        SafePut safeput = new SafePut();
        protected void Page_Load(object sender, EventArgs e)
        {
            
            //string respuesta = safe.conocerResultadoRespuesta("A00767891", "201313", "5");
            ////string respuesta = safeput.escribirCalificacion("201313", "5", "NS", "L03036903", "A00767891"); 
            //Label1.Text = respuesta;
            if(!IsPostBack)
            {
                cargarPeriodo();
                vermisosdeCampus();
                cargarCalificacion();
                try
                {

                   // llenargrid();
                }
                catch (Exception es)
                {
                    verModal("Error", es.Message.ToString());
                }
            }
        }

        public void vermisosdeCampus()
        {
            query = "Sp_muestra_perifil_campus '" + Session["Usuario"].ToString() + "'";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["id_rol"].ToString() == "4")//Este es administrador multicampus
                {
                    hdfActivarRol.Value = "1";
                    ddlCampus.Visible = true;
                    cargarCampus();
                    lblCampus.Visible = false;
                }
                else
                {
                    lblCampus.Text = dt.Rows[0]["Campus"].ToString();
                    hdfMostrarId.Value = dt.Rows[0]["Codigo_campus"].ToString();

                    hdfActivarRol.Value = "0";
                    ddlCampus.Visible = false;


                    /*opcional para la carga de campus en el filtro */
                    CampusFiltro.Visible = false;
                    lblfiltrarCampus.Text = dt.Rows[0]["Campus"].ToString();
                    
                }
            }
            else
            {
                verModal("Alerta", "El usuario no se encontró registrado");
            }
        }

        public void cargarPeriodo()
        {
            query = "select Periodo,Descripcion from cat_periodos where activo=1";
            dt = db.getQuery(conexionBecarios,query);
            if(dt.Rows.Count>0)
            {
                ddlperiodo.DataTextField = "Descripcion";
                ddlperiodo.DataValueField = "Periodo";
                ddlperiodo.DataSource = dt;
                ddlperiodo.DataBind();
                PeriodoFiltro.DataTextField = "Descripcion";
                PeriodoFiltro.DataValueField = "Periodo";
                PeriodoFiltro.DataSource = dt;
                PeriodoFiltro.DataBind();
            }
            else
            {
                ddlperiodo.Visible = false;
                PeriodoFiltro.Visible = false;
            }
        }

        public void cargarCampus()
        {
            query = "select Codigo_campus,Nombre from cat_campus order by Nombre ";
            dt = db.getQuery(conexionBecarios,query);
            if(dt.Rows.Count>0)
            {
                ddlCampus.DataTextField = "Nombre";
                ddlCampus.DataValueField = "Codigo_campus";
                ddlCampus.DataSource = dt;
                ddlCampus.DataBind();
                CampusFiltro.DataTextField = "Nombre";
                CampusFiltro.DataValueField = "Codigo_campus";
                CampusFiltro.DataSource = dt;
                CampusFiltro.DataBind();
            }
        }

        protected void ddlperiodo_DataBound(object sender, EventArgs e)
        {
            ddlperiodo.Items.Insert(0, new ListItem("--Seleccione--", ""));
        }

        protected void ddlCampus_DataBound(object sender, EventArgs e)
        {
            ddlCampus.Items.Insert(0, new ListItem("--Seleccione--", ""));
        }
        protected void CalificacionFiltro_DataBound(object sender, EventArgs e)
        {
            CalificacionFiltro.Items.Insert(0, new ListItem("--Seleccione--", ""));
        }
        protected void FiltroPeriodo_DataBound(object sender, EventArgs e)
        {
            PeriodoFiltro.Items.Insert(0, new ListItem("--Seleccione--", ""));
        }

        protected void FiltroCampus_DataBound(object sender, EventArgs e)
        {
             CampusFiltro.Items.Insert(0, new ListItem("--Seleccione--", ""));
        }


        protected void btnMandar_Click(object sender, EventArgs e)
        {
            try
            {
                int i =0 ;
                Label1.Text = "";
                string []arr;
                string cadena="";
                if(hdfActivarRol.Value=="1")
                {
                    query = @"sp_mandar_informacion_banner_fase '" + ddlCampus.SelectedValue + "', '" + ddlperiodo.SelectedValue + "' ";
                }
                else
                {
                    query = @"sp_mandar_informacion_banner_fase '" + hdfMostrarId.Value + "', '" + ddlperiodo.SelectedValue + "' ";
                }
                dt = db.getQuery(conexionBecarios,query);
                if (dt.Rows.Count > 0)
                {
                    //Tomamos la informacion del dt para envierla a banner safe
                    foreach(DataRow row in dt.Rows)
                    {
                        //Mandamos la calificacion a banner safe
                        //lblimprimir.Text += "ESCRIBIMOS RESPUESTA A BANNER </br>";
                        cadena=safeput.escribirCalificacion(dt.Rows[i]["CodigoPeriodo"].ToString(), dt.Rows[i]["FolioBanner"].ToString(), dt.Rows[i]["Becario_calificacion_baner"].ToString(), Session["Usuario"].ToString(), dt.Rows[i]["Matricula"].ToString());
                        arr=cadena.Split('!');
                        //lblimprimir.Text += cadena;

                        //safeput.escribirCalificacion(dt.Rows[i]["CodigoPeriodo"].ToString(), dt.Rows[i]["FolioBanner"].ToString(), dt.Rows[i]["Becario_calificacion_baner"].ToString(), Session["Usuario"].ToString(), dt.Rows[i]["Matricula"].ToString());

                        //lblimprimir.Text += "LEEMOS RESULTADO DE PRESPUESTA</br>";
                        //lblimprimir.Text+= safe.conocerResultadoRespuesta(dt.Rows[i]["Matricula"].ToString(), dt.Rows[i]["CodigoPeriodo"].ToString(), dt.Rows[i]["FolioBanner"].ToString());
                        safe.conocerResultadoRespuesta(dt.Rows[i]["Matricula"].ToString(), dt.Rows[i]["CodigoPeriodo"].ToString(), dt.Rows[i]["FolioBanner"].ToString());

                        query = "sp_guar_informacion_ya_enviados_banner '" + dt.Rows[i]["CodigoPeriodo"].ToString() + "','" + dt.Rows[i]["Matricula"].ToString() + "','" + dt.Rows[i]["Becario_calificacion_baner"].ToString() + "','" + Session["usuario"].ToString() + "','" + dt.Rows[i]["FolioBanner"].ToString() + "','" + arr[4] + "'";

                        db.getQuery(conexionBecarios,query);

                        i++;
                    }


                    verModal("Alerta","La informacion se envio a banner");
                }

                
                llenargrid();
            }catch(Exception es){
                verModal("Error", es.Message.ToString());
            }
        }


        public void llenargrid()
        {
            /*mostramos la informacion que tiene la tabla donde se almacena la información que se envio de a banner safe*/
          //  query = "sp_mostrar_informacion_quese_mando_banner '" + ddlperiodo.SelectedValue + "'";
            try
            {
                query = "select estatusEnvio, cp.Descripcion as Periodo,eb.Matricula,CASE WHEN eb.Calificion='SS' THEN 'SATISFACTORIO' WHEN eb.Calificion='NS' THEN 'INSATISFACTORIO' END as Calificion,eb.Nomina_envio_banner,eb.Fecha_envio,eb.Folio from tbl_envio_banner eb inner join cat_periodos cp on cp.Periodo=eb.Periodo inner join tbl_alumnos a on a.Matricula=eb.Matricula where cp.Periodo!='' ";
                if(hdfActivarRol.Value=="1")
                {
                    if (CampusFiltro.SelectedValue != "") { query += " AND a.Codigo_campus='" + CampusFiltro.SelectedValue + "'"; }
                }
                else
                {
                    query += " AND a.Codigo_campus='" + hdfMostrarId.Value+ "'";
                }
                
                if (PeriodoFiltro.SelectedValue != "") { query += " AND cp.Periodo='" + PeriodoFiltro.SelectedValue + "'"; }
                if (MatriculaFiltro.Text != "") { query += " AND a.Matricula='" + MatriculaFiltro.Text + "'"; }
                if (CalificacionFiltro.SelectedValue == "1") { query += " AND eb.Calificion='SS'"; } else if (CalificacionFiltro.SelectedValue == "2") { query += " AND eb.Calificion!='SS'"; }

                dt = db.getQuery(conexionBecarios, query);
                if (dt.Rows.Count > 0)
                {
                    Gvdatos.DataSource = dt;
                    Gvdatos.DataBind();
                    pnlgrid.Visible = true;
                }
                else
                {
                                       
                    Gvdatos.DataSource = null;
                    Gvdatos.DataBind();
                    verModal("Alerta","No se encontro información en la busqueda");
                }
               
            }
        catch(Exception e)
            {
                verModal("Error",e.Message.ToString());
            }
        }

        //Esta funcion imprime el msj
        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }

        protected void Gvdatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Gvdatos.PageIndex = e.NewPageIndex;
                llenargrid();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }
        public void cargarCalificacion()
        {
            query = "select id_calificacion,Valor_calificacion from cat_calificacion ";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                CalificacionFiltro.DataValueField = "id_calificacion";
                CalificacionFiltro.DataTextField = "Valor_calificacion";
                CalificacionFiltro.DataSource = dt;
                CalificacionFiltro.DataBind();
                
            }
        }

        protected void Filtro_Click(object sender, EventArgs e)
        {
            llenargrid();
        }
        

    }
}