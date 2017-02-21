using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using ServicioBecario.Codigo;


namespace ServicioBecario.Vistas
{
    public partial class ProfesoresSorteo : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
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
                    //llenarCampus();
                    llenarPuesto();
                    llenarRegla();
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
                    lblCampusPrincipal.Visible = false;
                }
                else
                {
                    lblCampusPrincipal.Text = dt.Rows[0]["Campus"].ToString();
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


        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }

        public void llenarCampus()
        {
            query = "select Codigo_campus,Nombre from cat_campus order by Nombre";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)//Da a entender que si tiene informacion
            {
                ddlCampus.DataTextField = "Nombre";
                ddlCampus.DataValueField = "Codigo_campus";
                ddlCampus.DataSource = dt;
                ddlCampus.DataBind();
            }
        }

        public void llenarPuesto()
        {
            query = @"select id_puesto_campus,Nombre_puesto from cat_puestos_campus
                    where id_puesto_campus in (1,2)";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)//Da a entender que si tiene informacion
            {
                ddlPuesto.DataTextField = "Nombre_puesto";
                ddlPuesto.DataValueField = "id_puesto_campus";
                ddlPuesto.DataSource = dt;
                ddlPuesto.DataBind();
            }
        }
        public void llenarRegla()
        {
            query = "select id_regla_asignacion,Descripcion_regla from cat_regla_asignacion";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlRegla.DataValueField = "id_regla_asignacion";
                ddlRegla.DataTextField = "Descripcion_regla";
                ddlRegla.DataSource = dt;
                ddlRegla.DataBind();
            }
        }
        protected void ddlPuesto_DataBound(object sender, EventArgs e)
        {
            ddlPuesto.Items.Insert(0, new ListItem("--Seleccione --", ""));
        }

        protected void ddlCampus_DataBound(object sender, EventArgs e)
        {
            ddlCampus.Items.Insert(0, new ListItem("--Seleccione --", ""));
        }

        protected void ddlRegla_DataBound(object sender, EventArgs e)
        {
            ddlRegla.Items.Insert(0, new ListItem("--Seleccione --", ""));
        }
        public void mostrarvalidacionDdlCampus()
        {
            ClientScript.RegisterStartupScript(GetType(), "ValidaddlCampuster", "validacionClienteDdlCampus();", true);
        }
        protected void chkProforSorteo_CheckedChanged(object sender, EventArgs e)
        {
            if (chkProforSorteo.Checked)
            {
                if (hdfActivarRol.Value=="1")
                {
                    if (ddlCampus.SelectedValue != "")
                    {
                        pnlProfesorSorteo.Visible = true;
                        mostrarProfesoresASorteo();
                    }
                    else
                    {
                        verModal("Alerta", "Por favor selecciona un campus");
                        // mostrarvalidacionDdlCampus();
                        chkProforSorteo.Checked = false;


                    }
                }
                else
                {
                    pnlProfesorSorteo.Visible = true;
                    mostrarProfesoresASorteo();
                }

                
                

            }
            else
            {
                pnlProfesorSorteo.Visible = false;
            }
        }

        protected void chkReglaAsignacion_CheckedChanged(object sender, EventArgs e)
        {
            if (chkReglaAsignacion.Checked)
            {
                if (hdfActivarRol.Value=="1")
                {
                    if (ddlCampus.SelectedValue != "")
                    {
                        pnlRegla.Visible = true;
                        mostrarRegladeAsignacion();
                    }
                    else
                    {
                        verModal("Alerta", "Por favor selecciona un campus");
                        //validarParaMostrarRegla();
                        chkReglaAsignacion.Checked = false;
                    }
                }
                else
                {
                    pnlRegla.Visible = true;
                    mostrarRegladeAsignacion();
                }
                
            }
            else
            {
                pnlRegla.Visible = false;
            }
        }

        public void validarParaMostrarRegla()
        {
            ClientScript.RegisterStartupScript(GetType(), "salda", "validarRegla();", true);
        }

        public void mostrarProfesoresASorteo()
        {
            if(hdfActivarRol.Value=="1")
            {
                query = "sp_muestra_profespres_a_sorte '" + ddlCampus.SelectedItem.Text + "'";
            }
            else
            {
                query = "sp_muestra_profespres_a_sorte '" + lblCampusPrincipal.Text+ "'";
            }
            
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                GvProfesorSorteo.DataSource = dt;
                GvProfesorSorteo.DataBind();
            }
            else
            {
                verModal("Alerta", "No se encontraron los datos");
                pnlProfesorSorteo.Visible = false;
                chkProforSorteo.Checked = false;
            }
        }
        public void mostrarRegladeAsignacion()
        {
            if (hdfActivarRol.Value=="1")
            {
                query = "sp_muestra_regla_asignacion '" + ddlCampus.SelectedItem.Text + "' ";
            }
            else
            {
                query = "sp_muestra_regla_asignacion '" + lblCampusPrincipal.Text + "' ";
            }
            
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                GvRegla.DataSource = dt;
                GvRegla.DataBind();
            }
            else
            {
                verModal("Alerta", "No se encontraron los datos");
                pnlRegla.Visible = false;
                chkReglaAsignacion.Checked = false;
            }

        }
        public void mostrarProfesoresASorteo_A()
        {
            query = "sp_muestra_profespres_a_sorte '" + hdfCampusTexto.Value + "'";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                GvProfesorSorteo.DataSource = dt;
                GvProfesorSorteo.DataBind();
            }
            else
            {
                verModal("Alerta", "No se encontraron los datos");
               
            }
        }
        protected void ChangeCampus_SelectedIndexChange(object sender, EventArgs e)
        {
            chkProforSorteo.Checked = false;
            chkReglaAsignacion.Checked = false;
            pnlProfesorSorteo.Visible = false;
            pnlRegla.Visible = false;
           
        }
        protected void GvRegla_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (hdfActivarRol.Value=="1")
            {
                ddlCampus.SelectedValue = ddlCampus.Items.FindByText(GvRegla.SelectedRow.Cells[1].Text).Value;
            }
            
            ddlRegla.SelectedValue = ddlRegla.Items.FindByText(GvRegla.SelectedRow.Cells[2].Text).Value;
            pnlSeleccionCampus.Visible = true;
            lblCampus.Text = GvRegla.SelectedRow.Cells[1].Text;
            btnGuardarRegla.Visible = false;
            btnActualizarRegla.Visible = true;
            btncancelarRegla.Visible = true;
            /**/
            ddlCampus.Enabled = false;
            ddlPuesto.Enabled = false;
            ddlEnproceso.Enabled = false;
            btnGuardarSorteo.Enabled = false;
            btnActualizarSorteo.Visible = false;
            btncancelarsorte.Visible = false;

        }
        public void deshabilitarPuesto()
        {
            ClientScript.RegisterStartupScript(GetType(), "Mostrar", "DesHabilitaPuesto();", true);
        }
        protected void GvProfesorSorteo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (hdfActivarRol.Value=="1")
            {
                ddlCampus.SelectedValue = ddlCampus.Items.FindByText(GvProfesorSorteo.SelectedRow.Cells[3].Text).Value;
            }
            
            ddlPuesto.SelectedValue = ddlPuesto.Items.FindByText(GvProfesorSorteo.SelectedRow.Cells[1].Text).Value;
            ddlEnproceso.SelectedValue = GvProfesorSorteo.SelectedRow.Cells[2].Text;
            btnGuardarSorteo.Visible = false;
            btnActualizarSorteo.Visible = true;
            hdfPuestoTexto.Value = GvProfesorSorteo.SelectedRow.Cells[1].Text;
            hdfCampusTexto.Value = GvProfesorSorteo.SelectedRow.Cells[3].Text;
            deshabilitarPuesto();
            btncancelarsorte.Visible = true;
            btnActualizarRegla.Visible = false;
            btncancelarRegla.Visible = false;
        }
        public void deshabilitaRegla()
        {
            ClientScript.RegisterStartupScript(GetType(), "MV", "desavilitaRegla();", true);
        }

        public void deshabilitaCampus()
        {
            ClientScript.RegisterStartupScript(GetType(), "M", "desavilitaCampus();", true);
        }
        protected void btnActualizarSorteo_Click(object sender, EventArgs e)
        {
            try
            {
                btnActualizarRegla.Visible = false;
                btncancelarRegla.Visible = false;
                btnGuardarSorteo.Visible = true;
                btnActualizarSorteo.Visible = false;
                actualizarSorteo();
                mostrarProfesoresASorteo_A();
                btncancelarsorte.Visible = false;
                ddlEnproceso.Visible = true;
                ClientScript.RegisterStartupScript(GetType(), "Backtwo", "HabilitaTodo();", true);
                
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        public void mostrarRegladeAsignacion_a()
        {
            query = "sp_muestra_regla_asignacion '" + lblCampus.Text + "' ";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                GvRegla.DataSource = dt;
                GvRegla.DataBind();
            }
            else
            {
                verModal("Alerta", "No se encontraron los datos");
                pnlRegla.Visible = false;
                GvRegla.Visible = false;
                
            }
        }
        public void actualizarSorteo()
        {
            query = "sp_actualiza_sorteo_proceso '" + hdfPuestoTexto.Value + "','" + hdfCampusTexto.Value + "','" + ddlEnproceso.SelectedValue + "'";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                {
                    verModal("Éxito", "La información se actualizó correctamente");
                }
                else
                {
                    verModal("Error", "Acción no contralada");
                }
            }
        }


        public void actualizarRegla()
        {
            query = "sp_actualizar_regla '" + lblCampus.Text.Trim() + "'," + ddlRegla.SelectedValue + "";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                {
                    verModal("Éxito", "La información ha sido actualizada con éxito.");
                }
                else
                {
                    verModal("Error", "Algún error sucedió mediante la actualización de datos");
                }
            }
        }

        protected void btnActualizarRegla_Click(object sender, EventArgs e)
        {
            try
            {
                btncancelarRegla.Visible = false;
                btnActualizarSorteo.Visible = false;
                btncancelarsorte.Visible = false;
                actualizarRegla();
                mostrarRegladeAsignacion_a();
                btnActualizarRegla.Visible = false;
                btnGuardarRegla.Visible = true;
                lblCampus.Visible = false;
                ClientScript.RegisterStartupScript(GetType(), "Back", "HabilitaTodo();", true);
                btnGuardarSorteo.Enabled = true;
                ddlEnproceso.Enabled = true;
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        protected void btnGuardarSorteo_Click(object sender, EventArgs e)
        {
            try
            {
                guardarSorteo();
                mostrarProfesoresASorteo();
                ClientScript.RegisterStartupScript(GetType(), "Backtwo", "HabilitaTodo();", true);
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        protected void btnGuardarRegla_Click(object sender, EventArgs e)
        {
            try
            {
                guardarRegla();
                mostrarRegladeAsignacion();
                ClientScript.RegisterStartupScript(GetType(), "Backtwo", "HabilitaTodo();", true);
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        public void guardarSorteo()
        {
            if (hdfActivarRol.Value=="1")
            {
                hdfMostrarId.Value = ddlCampus.SelectedValue;
            }
            // movido query = "sp_guardarSorte_campus " + ddlCampus.SelectedValue + "," + ddlPuesto.SelectedValue + ",'" + ddlEnproceso.SelectedValue + "'";
            query = "sp_guardarSorte_campus " + hdfMostrarId.Value + "," + ddlPuesto.SelectedValue + ",'" + ddlEnproceso.SelectedValue + "'";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                switch (dt.Rows[0]["Mensaje"].ToString())
                {
                    case "YaExiste":
                        verModal("Alerta", "La información ya se encuentra registrada en el sistema.");
                        break;
                    case "Insertado":
                        verModal("Éxito", "La información fue guardada con éxito en el sistema.");
                        break;
                }
            }

        }
        public void guardarRegla()
        {
            //Verifica el rol asignado
            if (hdfActivarRol.Value=="1")
            {
                hdfMostrarId.Value = ddlCampus.SelectedValue;
            }

            // ante de mover query = "sp_guardar_regla " + ddlCampus.SelectedValue + "," + ddlRegla.SelectedValue + "";
            query = "sp_guardar_regla " + hdfMostrarId.Value + "," + ddlRegla.SelectedValue + "";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                switch (dt.Rows[0]["Mensaje"].ToString())
                {
                    case "Insertado":
                        verModal("Éxito", "La información fue guardada con éxito en el sistema.");
                        break;
                    case "YaInsertado":
                        verModal("Alerta", "La información ya se encuentra registrada en el sistema.");
                        break;

                }
            }
        }

        protected void btncancelarsorte_Click(object sender, EventArgs e)
        {
            try
            {
                btnActualizarSorteo.Visible = false;
                btnGuardarSorteo.Visible = true;
                btncancelarsorte.Visible = false;
                btnGuardarRegla.Visible = true;
                btnActualizarSorteo.Visible = false;
                btncancelarsorte.Visible = false;
                GvProfesorSorteo.SelectedIndex = -1;
                ClientScript.RegisterStartupScript(GetType(), "Backtwo", "HabilitaTodo();", true);
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        protected void btncancelarRegla_Click(object sender, EventArgs e)
        {
            try
            {
                btnGuardarRegla.Visible = true;
                btncancelarRegla.Visible = false;
                btnActualizarRegla.Visible = false;
                btnGuardarSorteo.Visible = true;
                btnActualizarSorteo.Visible = false;
                btncancelarsorte.Visible = false;
                ddlCampus.Enabled = true;
                ddlPuesto.Enabled = true;
                ddlEnproceso.Enabled = true;
                GvRegla.SelectedIndex = -1;
                ClientScript.RegisterStartupScript(GetType(), "Backtwo", "HabilitaTodo();", true);
                btnGuardarSorteo.Enabled = true;
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }


    }
}