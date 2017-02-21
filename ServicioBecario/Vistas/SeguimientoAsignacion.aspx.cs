using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Web.Services;
using System.Data.SqlClient;
using ServicioBecario.Codigo;


namespace ServicioBecario.Vistas
{
    public partial class SeguimientoAsignacion : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        static string staticconexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    vermisosdeCampus();
                    llenarPeriodo();
                    llenarNivelAcademico();
                }
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
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
                    llenarCampus();
                    lblCampus.Visible = false;
                }
                else
                {
                    lblCampus.Text = dt.Rows[0]["Campus"].ToString();
                    hdfMostrarId.Value = dt.Rows[0]["Codigo_campus"].ToString();

                    hdfActivarRol.Value = "0";
                    ddlCampus.Visible = false;
                }
            }
            else
            {
                verModal("Alerta", "El usuario no se encontró registrado");
            }
        }
        public void llenarCampus()
        {
            query = "select Codigo_campus,Nombre from cat_campus where Codigo_campus!='PRT' order by Nombre ";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlCampus.DataValueField = "Codigo_campus";
                ddlCampus.DataTextField = "Nombre";
                ddlCampus.DataSource = dt;
                ddlCampus.DataBind();
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

        public void llenarPeriodo()
        {
            query = "select Periodo,Descripcion from cat_periodos where Activo=1";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlPeriodo.DataSource = dt;
                ddlPeriodo.DataTextField = "Descripcion";
                ddlPeriodo.DataValueField = "Periodo";
                ddlPeriodo.DataBind();
            }
        }


        public void llenarNivelAcademico()
        {
            query = "select Codigo_nivel_academico,Nivel_academico from cat_nivel_academico";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlNivelAcademico.DataTextField = "Nivel_academico";
                ddlNivelAcademico.DataValueField = "Codigo_nivel_academico";
                ddlNivelAcademico.DataSource = dt;
                ddlNivelAcademico.DataBind();
            }
            else
            {
                verModal("Alerta", "No existe información en el catálogo de nivel académico");
            }
        }

        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }

        protected void ddlPeriodo_DataBound(object sender, EventArgs e)
        {
            ddlPeriodo.Items.Insert(0, new ListItem("--Seleccione --", "-1"));
        }

        protected void ddlCampus_DataBound(object sender, EventArgs e)
        {
            ddlCampus.Items.Insert(0, new ListItem("--Seleccione --", ""));
        }

        protected void ddlNivelAcademico_DataBound(object sender, EventArgs e)
        {
            ddlNivelAcademico.Items.Insert(0, new ListItem("--Seleccione --", "-1"));
        }

        protected void txtNomina_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string cadena = txtNomina.Text.ToLower().Trim();
                if (!String.IsNullOrEmpty(cadena))
                {
                    if (cadena.Contains("l") || cadena.Contains("L"))
                    {
                        txtNomina.Text = txtNomina.Text.ToUpper();
                    }
                    else
                    {
                        txtNomina.Text = "L" + txtNomina.Text;
                    }
                }
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        protected void txtMatricula_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string cadena = txtMatricula.Text.ToLower().Trim();
                if (!String.IsNullOrEmpty(cadena))
                {
                    if (cadena.Contains("a") || cadena.Contains("A"))
                    {
                        txtMatricula.Text = txtMatricula.Text.ToUpper();
                    }
                    else
                    {
                        txtMatricula.Text = "A" + txtNomina.Text;
                    }
                }
            }
            catch (Exception es)
            {
                verModal("Alerta", es.Message.ToString());
            }


        }


        public void llenartablero(bool bandera)
        {
            //query = @"select es.Nomina, p.Descripcion as Periodo, p.Periodo as PeriodoID,  sb.Nivel_academico as [Nivel academico] , cs.Nombre as  [Campus solicitante], es.Nomina,es.Nombre +' '+ es.Apellido_paterno+' '+es.Apellido_materno  as [Nombre solicitante], es.Ubicacion_fisica as [Ubicacion fisica], case when s.Ubicacion_alterna is null then 'N/A' else s.Ubicacion_alterna end as [Ubicacion alterna]  ,a.Matricula,sb.Asistencia,a.Nombre +' ' + a.Apellido_paterno+' '+a.Apellido_materno as [Nombre Becario] , case  when  po.Nombre is null then  'N/A' else po.Nombre end as Proyecto,sb.Becario_calificacion as [Becario calificacion],s.Empleado_puntuaje as [Solicitante Calificacion], es.Correo_electronico as Correo, es.Departamento, es.Puesto, es.Extencion_telefonica as [extencion telefonica], sa.Estatus_asignacion as [Estatus asignacion] from  tbl_solicitudes s inner join cat_periodos p on s.Periodo=p.Periodo inner join tbl_empleados es on es.Nomina=s.Nomina inner join cat_tipo_solicitudes ts on ts.id_tipo_solicitud=s.id_tipo_solicitud inner join Cat_solicitud_estatus se on se.id_solicitud_estatus=s.id_solicitud_estatus inner join tbl_solicitudes_becarios sb on sb.id_Misolicitud=s.id_MiSolicitud inner join tbl_alumnos a on a.Matricula=sb.Matricula inner join cat_estatus_asignacion ea on ea.id_estatus_asignacion= sb.id_estatus_asignacion inner join cat_campus as c on c.Codigo_campus=a.Codigo_campus --c es para campus alumno inner join cat_campus cs on cs.Codigo_campus=es.Codigo_campus left join tbl_proyectos  po on po.id_proyecto=s.id_proyecto inner join cat_estatus_asignacion sa on sa.id_estatus_asignacion=sb.id_estatus_asignacion where es.Nomina!='' ";
            
            if(bandera)
            {
                if (db.matriculaConEspacio(txtMatricula.Text))
                {
                    if (db.nominaconEspacio(txtNomina.Text))
                    {
                        query = @"select es.Nomina,p.Descripcion as Periodo,
                             p.Periodo as PeriodoID, 
                             sb.Nivel_academico as [Nivel academico] ,
                             cs.Nombre as  [Campus solicitante],
                             es.Nomina,es.Nombre +' '+ es.Apellido_paterno+' '+es.Apellido_materno  as [Nombre solicitante],
                             es.Ubicacion_fisica as [Ubicacion fisica],
                             case when s.Ubicacion_alterna is null then 'N/A'
                             else s.Ubicacion_alterna
                             end as [Ubicacion alterna] 
                             ,a.Matricula,sb.Asistencia,a.Nombre +' ' + a.Apellido_paterno+' '+a.Apellido_materno as [Nombre Becario] ,
                             case  when  po.Nombre is null then  'N/A'
                             else po.Nombre
                             end as Proyecto,sb.Becario_calificacion as [Becario calificacion],s.Empleado_puntuaje as [Solicitante Calificacion],
                             es.Correo_electronico as Correo,
                             es.Departamento,
                             es.Puesto,
                             es.Extencion_telefonica as [extencion telefonica],
                             sa.Estatus_asignacion as [Estatus asignacion]
                             from 
                             tbl_solicitudes s inner join cat_periodos p on s.Periodo=p.Periodo
                             inner join tbl_empleados es on es.Nomina=s.Nomina
                             inner join cat_tipo_solicitudes ts on ts.id_tipo_solicitud=s.id_tipo_solicitud
                             inner join Cat_solicitud_estatus se on se.id_solicitud_estatus=s.id_solicitud_estatus
                             inner join tbl_solicitudes_becarios sb on sb.id_Misolicitud=s.id_MiSolicitud
                             inner join tbl_alumnos a on a.Matricula=sb.Matricula
                             inner join cat_estatus_asignacion ea on ea.id_estatus_asignacion= sb.id_estatus_asignacion
                             inner join cat_campus as c on c.Codigo_campus=a.Codigo_campus
                             inner join cat_campus cs on cs.Codigo_campus=es.Codigo_campus
                             left join tbl_proyectos  po on po.id_proyecto=s.id_proyecto
                             inner join cat_estatus_asignacion sa on sa.id_estatus_asignacion=sb.id_estatus_asignacion
                             where es.Nomina!='' ";



                        if (ddlPeriodo.SelectedValue != "-1") { query += " AND s.Periodo = '" + ddlPeriodo.SelectedValue + "'"; }
                        if (ddlNivelAcademico.SelectedItem.Text != "--Seleccione --") { query += " AND sb.Codigo_nivel_academico = '" + ddlNivelAcademico.SelectedValue + "'"; }
                        if( hdfActivarRol.Value=="1")
                        {
                            if (ddlCampus.SelectedValue != "") { query += " AND c.Codigo_campus = '" + ddlCampus.SelectedValue + "'"; }
                        }
                        else{
                            query += " AND c.Codigo_campus = '" + hdfMostrarId.Value + "'"; 
                        }
                       // if (ddlCampus.SelectedValue != "-1") { query += " AND c.Codigo_campus = '" + ddlCampus.SelectedValue + "'"; }
                        if (txtMatricula.Text != "") { query += " AND a.Matricula = '" + txtMatricula.Text + "'"; }
                        if (txtNomina.Text != "") { query += " AND es.Nomina = '" + txtNomina.Text + "'"; }



                        query = query.Replace("\r\n ", " ");

                        dt = db.getQuery(conexionBecarios, query);
                        if (dt.Rows.Count > 0)
                        {
                            GvTableroAsignacion.DataSource = dt;
                            GvTableroAsignacion.DataBind();

                        }
                        else
                        {
                            verModal("Alerta", "No se encontró la información");
                        }
                        ViewState["dt"] = dt;
                    }
                    else
                    {
                        verModal("Error", "El campo al nómina no tiene el formato correcto");
                    }
                }
                else
                {
                    verModal("Error", "El campo matrícula no tiene el formato correcto");
                }

            }
                   
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                //llenartablero();
                if (ddlPeriodo.SelectedValue == "-1" && ddlNivelAcademico.SelectedValue == "-1" && ddlCampus.SelectedValue == "-1" && txtMatricula.Text == "" && txtNomina.Text == "" && hdfActivarRol.Value=="1")
                {
                    verConfirmacion("Alerta", "La consulta tomara demaciado tiempo en ejecutarse");
                }
                else
                {
                    llenartablero(true);//llenamos tablero
                }
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        [WebMethod]
        public static string pruebaMetodo(string valor)
        {
            string hola = "funciona";
            return hola;
        }

        protected void GvTableroAsignacion_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                AjaxControlToolkit.PopupControlExtender pce = e.Row.FindControl("PopupDetalles") as AjaxControlToolkit.PopupControlExtender;
                string behaviorID = String.Concat("pce", e.Row.RowIndex);
                pce.BehaviorID = behaviorID;
                Image i = (Image)e.Row.Cells[5].FindControl("ImageButtonDetalles");
                string OnMouseOverScript = String.Format("$find('{0}').showPopup();", behaviorID);
                string OnMouseOutScript = String.Format("$find('{0}').hidePopup();", behaviorID);
                i.Attributes.Add("onmouseover", OnMouseOverScript);
                i.Attributes.Add("onmouseout", OnMouseOutScript);
            }
        }

        [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
        public static string GetDynamicContent(string contextKey)
        {
            DataTable dts;
            string html = "";
            string query = "select Correo_electronico,Departamento,Puesto,Extencion_telefonica from tbl_empleados where Nomina='" + contextKey + "'";
            dts = getQuery(staticconexionBecarios, query);
            if (dts.Rows.Count > 0)
            {

                html = @"   <table  class='table table-hover'>
                                <tr><td> <label>Correo: </labe> </td> <td> " + dts.Rows[0]["Correo_electronico"].ToString() + @"   </td> </tr>
                                <tr><td><label>Departamento: </label> </td> <td> " + dts.Rows[0]["Departamento"].ToString() + @" </td>  <tr>
                                <tr> <td> <label>Puesto:  </label> </td> <td>" + dts.Rows[0]["Puesto"].ToString() + @" </td>  </tr>
                                <tr> <td> <label> Extención:</label> </td> <td> " + dts.Rows[0]["Extencion_telefonica"].ToString() + @" </td>   </tr>
                          </table>
                                                    
                        ";

            }
            return html;
        }

        protected void IbtnExportar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                dt = (DataTable)ViewState["dt"];
                descargarInfo(dt);


            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        public void descargarInfo(DataTable ds)
        {

            if (dt != null)
            {
                string attachment = "attachment; filename=Tablero.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/ms-excel";
                string tab = "";
                foreach (DataColumn dc in ds.Columns)
                {
                    if (dc.ColumnName != "id_empleado")
                    {
                        Response.Write(tab + dc.ColumnName);
                        tab = "\t";
                    }

                }
                Response.Write("\n");
                int i;
                foreach (DataRow dr in ds.Rows)
                {
                    tab = "";
                    for (i = 0; i < ds.Columns.Count; i++)
                    {
                        if (i != 0)
                        {
                            Response.Write(tab + dr[i].ToString());
                            tab = "\t";
                        }

                    }
                    Response.Write("\n");
                }
                Response.End();

            }
            else
            {
                verModal("Alerta", "No hay informacion disponible para descargar");
            }

        }

        protected void GvTableroAsignacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            try
            {
                GvTableroAsignacion.PageIndex = e.NewPageIndex;
                llenartablero(true);
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        protected void btnLlevarRegistro_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string periodo = btn.CommandName;
            string matricula = btn.CommandName;
            periodo = periodo.Substring(0, periodo.IndexOf('!'));
            matricula = matricula.Replace(periodo + "!", "");
            matricula = matricula.Trim();
            //string urlSharepoint = System.Configuration.ConfigurationManager.AppSettings["urlsharepoint"];
            //urlSharepoint = urlSharepoint.Replace("**", "&");
            //Response.Redirect("TableroTramite.aspx?" + urlSharepoint + "&mar=" + matricula + "&per=" + periodo);
            Response.Redirect("TableroTramite.aspx?mar=" + matricula + "&per=" + periodo);
            
        }


        //Esta funcion imprime el msj
        public void verConfirmacion(string header, string body)
        {
            lblCabeza2.Text = header;
            lblcuerpo2.Text = body;
            mp2.Show();
        }



        protected void btncanll_Click(object sender, EventArgs e)
        {

            try
            {
                llenartablero(true);
            }catch(Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }

    }
}