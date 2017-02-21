using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using ServicioBecario.Codigo;
using System.Text.RegularExpressions;


namespace ServicioBecario.Vistas
{
    public partial class FechaSolicitud : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query, cadena;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //CalendarExtender1.StartDate = DateTime.Now;
                    vermisosdeCampus();
                    llenarPeriodo();
                    llenarFiltrarCampus();
                    llenarFiltrarPeriodo();
                    try
                    {
                        mostraDatosGrid();
                        pnlBusquedaGrid.Visible = true;
                    }
                    catch (Exception es)
                    {
                        verModal("Error", es.Message.ToString());
                    }
                }

            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }

        public void llenarFiltrarPeriodo()
        {
            query = "select Periodo,Descripcion from cat_periodos where Activo=1";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlFiltrarPeriodo.DataTextField = "Descripcion";
                ddlFiltrarPeriodo.DataValueField = "Periodo";
                ddlFiltrarPeriodo.DataSource = dt;
                ddlFiltrarPeriodo.DataBind();
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
                    mostarSeleccionCampus();
                    lblCampus.Visible = false;
                }
                else
                {
                    lblCampus.Text = dt.Rows[0]["Campus"].ToString();
                   hdfidCampus.Value = dt.Rows[0]["Codigo_campus"].ToString();
                    PnlFiltrarMostrarCampus.Visible = false;
                    hdfActivarRol.Value = "0";
                    ddlCampus.Visible = false;
                }
            }
            else
            {
                verModal("Alerta", "El usuario no se encontró registrado");
            }
        }


        public void mostarSeleccionCampus()
        {
            pnlMostrarCampus.Visible = false;
            query = "select Codigo_campus,Nombre from cat_campus order by Nombre asc";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlCampus.DataValueField = "Codigo_campus";
                ddlCampus.DataTextField = "Nombre";
                ddlCampus.DataSource = dt;
                ddlCampus.DataBind();
            }
            else
            {
                verModal("Alerta", "No hay campus para mostrar");
            }
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
        protected void ddlPeriodo_DataBound(object sender, EventArgs e)
        {
            ddlPeriodo.Items.Insert(0, new ListItem("--Seleccione --", ""));
        }
        protected void ddlCampus_DataBound(object sender, EventArgs e)
        {
            ddlCampus.Items.Insert(0, new ListItem("--Seleccione --", ""));
        }
        protected void ddlCampus_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblCampus.Text = ddlCampus.SelectedItem.Text;
            hdfMostrarId.Value = ddlCampus.SelectedValue;
            
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {

                Match match = Regex.Match(txtFechaInicio.Text, @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$");
                if(match.Success)
                {
                    match = Regex.Match(txtFechaFin.Text, @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$");
                    if(match.Success)
                    {
                        if (Convert.ToDateTime(txtFechaInicio.Text) <= Convert.ToDateTime(txtFechaFin.Text))
                        {
                            query = "sp_guardar_campus_periodo_fecha " + hdfMostrarId.Value + "," + ddlPeriodo.SelectedValue + ",'" + db.convertirFecha(txtFechaInicio.Text.Trim()) + "','" + db.convertirFecha(txtFechaFin.Text.Trim()) + "'";
                            dt = db.getQuery(conexionBecarios, query);
                            string mensaje = dt.Rows[0]["Mensaje"].ToString();
                            if (dt.Rows.Count > 0)
                            {
                                if (mensaje == "Ok")
                                {
                                    verModal("Éxito", "La información se guardado correctamente");
                                    mostraDatosGrid();
                                }
                                if (mensaje == "Exite")
                                {
                                    verModal("Alerta", "Ya existe un registro con los mismos datos");
                                }
                            }
                        }
                        else
                        {
                            verModal("Error", "En la fecha fin es mayor a fecha inicio");
                        }
                    }
                    else
                    {
                        verModal("Error","La fecha fin no tiene el formato correcto dd/mm/aaaa");
                        txtFechaFin.Focus();
                    }
                    
                }
                else
                {
                    verModal("Error","La fecha inicio no tiene el formato correcto de  dd/mm/aaaa");
                    txtFechaInicio.Focus();
                }


                
                
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }

        }

        
        public void llenarFiltrarCampus()
        {
            query = "select Nombre,Codigo_campus from cat_campus";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlFiltraCampus.DataValueField = "Codigo_campus";
                ddlFiltraCampus.DataTextField = "Nombre";
                ddlFiltraCampus.DataSource = dt;
                ddlFiltraCampus.DataBind();
            }
        }

        public void mostraDatosGrid()
        {

            bool bandera1 = false, bandera2 = false; bool vacio = false;
            Match match;
            if(!string.IsNullOrEmpty(txtFiltrarFechaInicio.Text))
            {
                match = Regex.Match(txtFiltrarFechaInicio.Text, @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$");
                if (match.Success)
                {
                    bandera1 = true;
                    vacio = true;
                }
                else
                {
                    verModal("Error", "La fecha inicio no tiene el formato de fecha dd/mm/aaaa");
                }
            }
            else
            {
                bandera1 = true;
            }

            if (!string.IsNullOrEmpty(txtFiltrarFechaFin.Text))
            {
                match = Regex.Match(txtFiltrarFechaFin.Text, @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$");
                if (match.Success)
                {

                    if(vacio)
                    {
                        if (Convert.ToDateTime(txtFiltrarFechaInicio.Text) <= Convert.ToDateTime(txtFiltrarFechaFin.Text))
                          {
                            bandera2 = true;
                          }
                        else
                        {
                            verModal("Error", "La fecha inicio no puede ser mayor a la fecha fin");
                        }
                    }
                    else
                    {
                        bandera2 = true;
                    }
                }
                else
                {
                    verModal("Error", "La fecha fin no tiene el formato de fecha dd/mm/aaaa");
                }
            }
            else
            {
                bandera2 = true;
            }


            if(bandera1 && bandera2)
            {
                if (hdfActivarRol.Value == "1") //Esta activiado el rol
                {

                    query = @"select fs.id_fecha_solicitud, CONVERT(varchar(10),  fs.Fecha_inicio,103) as Fecha_inicio, 
        CONVERT(varchar(10),fs.Fecha_fin,103) as Fecha_fin , p.Descripcion as Periodo,c.Nombre as Campus
        from tbl_campus_periodo  cp inner join tbl_fechas_solicitudes fs on cp.id_campus_periodo=fs.id_campus_periodo
        inner join cat_campus c on c.Codigo_campus=cp.Codigo_campus and c.Codigo_campus!='PRT'
        inner join cat_periodos p on p.Periodo = cp.Periodo and p.Activo=1 where p.Descripcion!='' ";
                    if (txtFiltrarFechaInicio.Text != "") { query += " AND CONVERT(date,  fs.Fecha_inicio,101)>CONVERT(date," + txtFiltrarFechaInicio.Text + ",101)"; }
                    if (txtFiltrarFechaFin.Text != "") { query += " AND CONVERT(date,  fs.Fecha_fin,101)<CONVERT(date," + txtFiltrarFechaFin.Text + ",101)"; }
                    if (ddlFiltrarPeriodo.SelectedValue != "") { query += "  AND cp.Periodo='"+ddlFiltrarPeriodo.SelectedValue+"'"; }
                    if (ddlFiltraCampus.SelectedValue != "") { query += "  AND cp.Codigo_campus='" + ddlFiltraCampus.SelectedValue + "'"; }
                    

                }
                else
                {
                    query = @" select fs.id_fecha_solicitud, CONVERT(varchar(90),fs.Fecha_inicio,103)  as Fecha_inicio,convert(varchar(90),fs.Fecha_fin,103) as Fecha_fin, c.Nombre as Campus,p.Descripcion as Periodo 
                              from tbl_campus_periodo  cp inner join tbl_fechas_solicitudes fs on cp.id_campus_periodo=fs.id_campus_periodo
                              inner join cat_campus c on c.Codigo_campus=cp.Codigo_campus
                              inner join cat_periodos p on p.Periodo=cp.Periodo and p.Activo=1 where  p.Descripcion!='' AND cp.Codigo_campus='" + hdfidCampus.Value + "' ";
                    if (txtFiltrarFechaInicio.Text != "") { query += " AND CONVERT(date,  fs.Fecha_inicio,101)>CONVERT(date," + txtFiltrarFechaInicio.Text + ",101)"; }
                    if (txtFiltrarFechaFin.Text != "") { query += " AND CONVERT(date,  fs.Fecha_fin,101)<CONVERT(date," + txtFiltrarFechaFin.Text + ",101)"; }
                    if (ddlFiltrarPeriodo.SelectedValue != "") { query += "  AND cp.Periodo='" + ddlFiltrarPeriodo.SelectedValue + "'"; }
                    //if (ddlFiltraCampus.SelectedValue != "") { query += "  AND cp.Codigo_campus='" + ddlFiltraCampus.SelectedValue + "'"; }
                   
                }
                dt = db.getQuery(conexionBecarios, query);
                if (dt.Rows.Count > 0)
                {
                    GVMostarFechas.DataSource = dt;
                    GVMostarFechas.DataBind();
                }
                else
                {
                    verModal("Alerta", "No se encontró información");
                    GVMostarFechas.DataSource = null;
                    GVMostarFechas.DataBind();
                }
            }
            
        }

        protected void ddlFiltraCampus_DataBound(object sender, EventArgs e)
        {
            ddlFiltraCampus.Items.Insert(0, new ListItem("--Seleccione --", ""));
        }

        protected void ddlFiltrarPeriodo_DataBound(object sender, EventArgs e)
        {
            ddlFiltrarPeriodo.Items.Insert(0, new ListItem("--Seleccione --", ""));
        }


        protected void GVMostarFechas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GVMostarFechas.PageIndex = e.NewPageIndex;
                mostraDatosGrid();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }
        protected void GVMostarFechas_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdfMostrarId.Value = GVMostarFechas.SelectedDataKey.Value.ToString();//id
            txtFechaInicio.Text = GVMostarFechas.SelectedRow.Cells[2].Text;//inicio
            txtFechaFin.Text = GVMostarFechas.SelectedRow.Cells[3].Text;//fin            
            lblCampus.Text = GVMostarFechas.SelectedRow.Cells[1].Text.Trim();
            ddlPeriodo.SelectedValue = ddlPeriodo.Items.FindByText(GVMostarFechas.SelectedRow.Cells[4].Text.Trim()).Value;
            if (hdfActivarRol.Value == "1")//valida rol
            {
                string valor =HttpUtility.HtmlDecode( GVMostarFechas.SelectedRow.Cells[1].Text.Trim());
                ddlCampus.SelectedValue = ddlCampus.Items.FindByText(valor).Value;//campus
                ddlCampus.Enabled = false;
            }
            btnActualizar.Visible = true;
            btnGuardar.Visible = false;
            btncancelar.Visible = true;
            ddlPeriodo.Enabled = false;
            ClientScript.RegisterStartupScript(GetType(), "BanderaFSol", "Bandera();", true);
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                bool bandera1 = false, bandera2 = false;
                Match match;
                if (!string.IsNullOrEmpty(txtFechaInicio.Text))
                {
                    match = Regex.Match(txtFechaInicio.Text, @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$");
                    if (match.Success)
                    {
                        bandera1 = true;
                    }
                    else
                    {
                        verModal("Error", "La fecha inicio no tiene el formato de fecha dd/mm/aaaa");
                    }
                }
                else
                {
                    bandera1 = true;
                }

                if (!string.IsNullOrEmpty(txtFechaFin.Text))
                {
                    match = Regex.Match(txtFechaFin.Text, @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$");
                    if (match.Success)
                    {
                        bandera2 = true;
                    }
                    else
                    {
                        verModal("Error", "La fecha fin no tiene el formato de fecha dd/mm/aaaa");
                    }
                }
                else
                {
                    bandera2 = true;
                }


                if(bandera1 && bandera2)
                {
                    if(Convert.ToDateTime(txtFechaInicio.Text)<=Convert.ToDateTime(txtFechaFin.Text))
                    {
                        query = "sp_actualiza_campus_periodo_fecha '" + db.convertirFecha(txtFechaInicio.Text.Trim()) + "','" + db.convertirFecha(txtFechaFin.Text.Trim()) + "'," + hdfMostrarId.Value + "";
                        dt = db.getQuery(conexionBecarios, query);
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                            {
                                verModal("Éxito", "Su información se actualizó con éxito");
                            }
                            mostraDatosGrid();
                            ddlPeriodo.Enabled = true;
                            btnActualizar.Visible = false;
                            btncancelar.Visible = false;
                            btnGuardar.Visible = true;
                            llimpiarControles();
                            ddlCampus.Enabled = true;
                        }
                    }
                    else
                    {
                        verModal("Error", "La fecha inicio es mayor que la fecha fin");
                    }
                    
                    
                }
                
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        public void llimpiarControles()
        {
            ddlPeriodo.SelectedValue = "";
            txtFechaInicio.Text = "";
            txtFechaFin.Text = "";

        }


        public void llimpiarFiltros()
        {
            ddlFiltraCampus.SelectedValue = "";
            txtFiltrarFechaFin.Text = "";
            txtFiltrarFechaInicio.Text = "";
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                mostraDatosGrid();
            }catch(Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }

        protected void btncancelar_Click(object sender, EventArgs e)
        {
            try {

                llimpiarFiltros();
                llimpiarControles();
                btncancelar.Visible = false;
                btnActualizar.Visible = false;
                btnGuardar.Visible = true;
                ddlPeriodo.Enabled = true;
                ddlCampus.Enabled = true;
                GVMostarFechas.SelectedIndex = -1;
            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }

    }
}