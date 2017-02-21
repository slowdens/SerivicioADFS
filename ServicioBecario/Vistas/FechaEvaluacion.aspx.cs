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
    public partial class FechaEvaluacion : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query, cadena;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();
        Match match;
        bool bandera1 = false, bandera2 = false, vacio=false,error=false;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    verpermisosDeCampus();
                    llenarPerido();
                    llenarFiltroPeriodo();




                    pnlVerFechas.Visible = true;

                    if (hdfActivarRol.Value == "1")//Verificamo que sea el rol administrador
                    {
                        pnlFiltarCampus.Visible = true;
                        llenarFiltarCampus();
                    }
                    mostrarDatos();
                }
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        public void llenarFiltroPeriodo()
        {
            query = "select Descripcion ,Periodo from cat_periodos ";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlFiltrarPeriodo.DataValueField = "Periodo";
                ddlFiltrarPeriodo.DataTextField = "Descripcion";
                ddlFiltrarPeriodo.DataSource = dt;
                ddlFiltrarPeriodo.DataBind();
            }
        }
        public void llenarPerido()
        {
            query = "select Descripcion ,Periodo from cat_periodos ";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlPeriodo.DataValueField = "Periodo";
                ddlPeriodo.DataTextField = "Descripcion";
                ddlPeriodo.DataSource = dt;
                ddlPeriodo.DataBind();
            }
        }
        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }
        public void verpermisosDeCampus()
        {
            query = "Sp_muestra_perifil_campus '" + Session["Usuario"].ToString() + "'";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["id_rol"].ToString() == "4")//Rol Administrador multicampus
                {
                    hdfActivarRol.Value = "1";
                    pnlCampus.Visible = false;
                    llenarCampus();
                    ddlCampus.Visible = true;
                    lblCampusSeleccionado.Visible = false;
                }
                else
                {
                    hdfActivarRol.Value = "0";
                    lblCampusSeleccionado.Text = dt.Rows[0]["Campus"].ToString();
                    hdfId.Value = dt.Rows[0]["Codigo_campus"].ToString();
                    pnlCampus.Visible = false;
                    ddlCampus.Visible = false;
                    
                }

            }
        }

        public void llenarFiltarCampus()
        {
            query = "select Codigo_campus,Nombre from cat_campus order by Nombre asc";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlFiltrarCampus.DataValueField = "Codigo_campus";
                ddlFiltrarCampus.DataTextField = "Nombre";
                ddlFiltrarCampus.DataSource = dt;
                ddlFiltrarCampus.DataBind();
            }
        }
        public void llenarCampus()
        {
            query = "select Codigo_campus,Nombre from cat_campus order by Nombre asc";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlCampus.DataValueField = "Codigo_campus";
                ddlCampus.DataTextField = "Nombre";
                ddlCampus.DataSource = dt;
                ddlCampus.DataBind();
            }
        }

        protected void ddlCampus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCampus.SelectedValue != "")
            {
                lblCampusSeleccionado.Text = ddlCampus.SelectedItem.Text;
                hdfId.Value = ddlCampus.SelectedValue;
            }
            else
            {
                verModal("Alerta", "Este valor no se debe seleccionar");
                lblCampusSeleccionado.Text = "";
            }
        }

        protected void ddlCampus_DataBound(object sender, EventArgs e)
        {
            ddlCampus.Items.Insert(0, new ListItem("--Seleccione --", ""));
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                agregarInfo();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        public void agregarInfo()
        {
           bandera1 = false;
           bandera2 = false;
            if (!string.IsNullOrEmpty(txtFechaInicio.Text))
            {
                match = Regex.Match(txtFechaInicio.Text, @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$");
                if (match.Success)
                {
                    bandera1 = true;
                    vacio = true;
                }
                else
                {
                    verModal("Error", "La fecha inicio no tiene el formato de fecha dd/mm/aaaa");
                    error = true;
                }
            }
            else
            {
                bandera1 = true;
            }

            if(error==false)
            {
                if (!string.IsNullOrEmpty(txtFechaFin.Text))
                {
                    match = Regex.Match(txtFechaFin.Text, @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$");
                    if (match.Success)
                    {
                        if (vacio)
                        {
                            if (Convert.ToDateTime(txtFechaInicio.Text) <= Convert.ToDateTime(txtFechaFin.Text))
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
            }
            


            if(bandera1 && bandera2)
            {
                    query = "sp_guarda_fecha_evaluacion " + hdfId.Value + "," + ddlPeriodo.SelectedValue + ",'" + db.convertirFecha(txtFechaInicio.Text) + "','" + db.convertirFecha(txtFechaFin.Text) + "'";
                    dt = db.getQuery(conexionBecarios, query);
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                        {
                            verModal("Exito", "Su Fechas de evaluación ha sido guardada con éxito");
                            mostrarDatos();
                        }
                        else
                        {
                            verModal("Alerta", "Ya hay un registro que contiene una fecha para el campus favor de actualizar las fechas");
                        }
                    }               
                
            }
            
        }

        protected void ddlPeriodo_DataBound(object sender, EventArgs e)
        {
            ddlPeriodo.Items.Insert(0, new ListItem("--Seleccione --", ""));
        }

        protected void ddlFiltrarPeriodo_DataBound(object sender, EventArgs e)
        {
            ddlFiltrarPeriodo.Items.Insert(0, new ListItem("--Seleccione --", "-1"));
        }

       


        public void mostrarDatos()
        {
            bandera1 = false;
            bandera2 = false;
            vacio = false;
            if (!string.IsNullOrEmpty(txtFiltrarFechaInicio.Text))
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
                if (hdfActivarRol.Value == "1")//este solo para los que estan con el rol administrador multicampus
                {
                    query = @"select f.id_fecha_evaluacion, c.Nombre as Campus,convert (varchar(10),f.Fecha_inicio,103) as Fecha_inicio , 
                        CONVERT(varchar(10), f.Fecha_fin,103 ) Fecha_fin,p.Descripcion as Periodo 
                        from tbl_fechas_evaluacion f inner join tbl_campus_periodo cp on cp.id_campus_periodo=f.id_campus_periodo
                        inner join cat_campus c on c.Codigo_campus = cp.Codigo_campus
                        inner join cat_periodos p on p.Periodo=cp.Periodo where p.Periodo!='' ";


                    if (ddlFiltrarPeriodo.SelectedValue != "-1") { query += " AND p.Periodo='" + ddlFiltrarPeriodo.SelectedValue + "'"; }
                    if (ddlFiltrarCampus.SelectedValue != "-1") { query += " AND cp.Codigo_campus='" + ddlFiltrarCampus.SelectedValue + "'"; }
                    if (txtFiltrarFechaInicio.Text != "") { query += " AND CONVERT(date,f.Fecha_inicio,103)>CONVERT(date,'" + txtFiltrarFechaInicio.Text + "',103)"; }
                    if (txtFiltrarFechaFin.Text != "") { query += " AND CONVERT(date,f.Fecha_fin,103)<CONVERT(date,'" + txtFiltrarFechaFin.Text + "',103)"; }



                }
                else
                {
                    query = @"select f.id_fecha_evaluacion, c.Nombre as Campus,convert (varchar(10),f.Fecha_inicio,103) as Fecha_inicio , 
                        CONVERT(varchar(10), f.Fecha_fin,103 ) Fecha_fin,p.Descripcion as Periodo 
                        from tbl_fechas_evaluacion f inner join tbl_campus_periodo cp on cp.id_campus_periodo=f.id_campus_periodo
                        inner join cat_campus c on c.Codigo_campus = cp.Codigo_campus
                        inner join cat_periodos p on p.Periodo=cp.Periodo where p.Periodo!='' and c.Codigo_campus='" + hdfId.Value + "'  ";

                    if (ddlFiltrarPeriodo.SelectedValue != "-1") { query += " AND p.Periodo='" + ddlFiltrarPeriodo.SelectedValue + "'"; }
                    if (txtFiltrarFechaInicio.Text != "") { query += " AND CONVERT(date,f.Fecha_inicio,103)>CONVERT(date,'" + txtFiltrarFechaInicio.Text + "',103)"; }
                    if (txtFiltrarFechaFin.Text != "") { query += " AND CONVERT(date,f.Fecha_fin,103)<CONVERT(date,'" + txtFiltrarFechaFin.Text + "',103)"; }

                }

                query = query.Replace("\r\n", " ");
                dt = db.getQuery(conexionBecarios, query);
                if (dt.Rows.Count > 0)
                {
                    GVDatos.DataSource = dt;
                    GVDatos.DataBind();
                }
                else
                {
                    verModal("Alerta", "No se encontró el valor filtrado");
                    GVDatos.DataSource = null;
                    GVDatos.DataBind();
                }
                
            }
            
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                mostrarDatos();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }
        //con este evento lo que se hace es desarrollar 
        protected void GVDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdfIdSeleccion.Value = GVDatos.SelectedDataKey.Value.ToString();
            if (hdfActivarRol.Value == "1")
            {
                string camso =HttpUtility.HtmlDecode( GVDatos.SelectedRow.Cells[1].Text.Trim());
                ddlCampus.SelectedValue = ddlCampus.Items.FindByText(camso).Value;
            }
            lblCampusSeleccionado.Text = GVDatos.SelectedRow.Cells[1].Text.Trim();
            txtFechaInicio.Text = GVDatos.SelectedRow.Cells[2].Text.Trim();
            txtFechaFin.Text = GVDatos.SelectedRow.Cells[3].Text.Trim();
            string datpo = GVDatos.SelectedRow.Cells[4].Text.Trim();
            ddlPeriodo.SelectedValue = ddlPeriodo.Items.FindByText(GVDatos.SelectedRow.Cells[4].Text.Trim()).Value;
            btnAgregar.Visible = false;
            BtnActualizar.Visible = true;
            //deshabilito el periodo
            deshabilitar();
            btncancelar.Visible = true;
            ddlCampus.Enabled = false;
            ClientScript.RegisterStartupScript(GetType(), "BanderaF", "Bandera();", true);
        }

        protected void BtnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                bandera1 = false; bandera2 = false;               
                vacio = false;
                if (!string.IsNullOrEmpty(txtFechaInicio.Text.Trim()))
                {
                    match = Regex.Match(txtFechaInicio.Text.Trim(), @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$");
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

                if (!string.IsNullOrEmpty(txtFechaFin.Text.Trim()))
                {
                    match = Regex.Match(txtFechaFin.Text.Trim(), @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$");
                    if (match.Success)
                    {
                        if (vacio)
                        {
                            if (Convert.ToDateTime(txtFechaInicio.Text) <= Convert.ToDateTime(txtFechaFin.Text))
                            {
                                bandera2 = true;
                            }
                            else
                            {
                                verModal("Error", "la fecha inicio no puede ser mayor a la fecha fin");
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
                    actualizar();
                    btnAgregar.Visible = true;
                    BtnActualizar.Visible = false;
                    hdfIdSeleccion.Value = "";
                    GVDatos.SelectedIndex = -1;
                    // mostrarDatos();
                    btncancelar.Visible = false;
                    ddlCampus.Enabled = true;
                    llimpiarcomponentes();
                }
                
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        protected void ddlFiltrarCampus_DataBound(object sender, EventArgs e)
        {
            ddlFiltrarCampus.Items.Insert(0, new ListItem("--Seleccione --", "-1"));
           
        }

        public void actualizar()
        {
            query = "sp_Actualiza_Fechas_evaluacion '" + db.convertirFecha(txtFechaInicio.Text.Trim()) + "','" + db.convertirFecha(txtFechaFin.Text.Trim()) + "'," + hdfIdSeleccion.Value + "";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                {
                    verModal("Éxito", "La información se actualizó correctamente");
                }
            }
        }

        protected void GVDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GVDatos.PageIndex = e.NewPageIndex;
                mostrarDatos();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }


        public void deshabilitar()
        {
            ClientScript.RegisterStartupScript(GetType(), "Mostrar", "deshabilitaddlPeriodo();", true);
        }

        protected void btncancelar_Click(object sender, EventArgs e)
        {
            try
            {
                llimpiarcomponentes();
                llimpiarFitros();
                btncancelar.Visible = false;
                BtnActualizar.Visible = false;
                btnAgregar.Visible = true;
                ddlCampus.Enabled = true;
                GVDatos.SelectedIndex = -1;

            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }
        public void llimpiarcomponentes()
        {
            ddlPeriodo.SelectedValue = "";
            txtFechaFin.Text = "";
            txtFechaInicio.Text = "";

        }

        public void llimpiarFitros()
        {
            ddlFiltrarPeriodo.SelectedValue = "-1";
            txtFiltrarFechaInicio.Text = "";
            txtFiltrarFechaFin.Text = "";
            if (hdfActivarRol.Value == "1")
            {
                ddlFiltrarCampus.SelectedValue = "-1";
            } 
           
        }
    }
}