using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Data;
using ServicioBecario.Codigo;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;


namespace ServicioBecario.Vistas
{
    public partial class CatPeriodo : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query;        
        DataTable dt;        
        BasedeDatos db = new BasedeDatos();
         bool bandera1 = false;
         bool bandera2 = false;
         bool vacio =false;
         Match match;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                pnlGridview.Visible = true;
              
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                
                int vooleano;
                if (db.validaNumeroSinEspacio(txtnombre.Text))
                {
                   //txtDescripcion
                    if (db.NumeroLetraConEspacio(txtDescripcion.Text))
                    {

                        match = Regex.Match(txtFechaInicio.Text, @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$");
                        if (match.Success)
                        {

                            match = Regex.Match(txtFechaFin.Text, @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$");
                            if (match.Success)
                            {
                                if (Convert.ToDateTime(txtFechaInicio.Text) <= Convert.ToDateTime(txtFechaFin.Text))
                                {
                                    if (chkactivo.Checked)
                                    {
                                        vooleano = 1;
                                    }
                                    else
                                    {
                                        vooleano = 0;
                                    }

                                    //txtDescripcion.Text = Regex.Replace(txtDescripcion.Text, @"[^\w\s\.@-]", "");
                                    //txtFechaInicio.Text = Regex.Replace(txtFechaInicio.Text, @"[^\/\d]", "");
                                    //txtFechaFin.Text = Regex.Replace(txtFechaFin.Text, @"[^\/\d]", "");


                                    query = "exec sp_crear_periodo_academico  '" + txtnombre.Text + "','" + db.convertirFecha(txtFechaInicio.Text) + "','" + db.convertirFecha(txtFechaFin.Text) + "','" + Session["usuario"].ToString() + "','" + txtDescripcion.Text + "'," + vooleano + "";
                                    dt = db.getQuery(conexionBecarios, query);
                                    if (dt.Rows.Count > 0)
                                    {
                                        if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                                        {
                                            verModal("Éxito", "Se creó correctamente el periodo");
                                            pnlGridview.Visible = true;
                                            llenarGrid();
                                        }
                                        else
                                        {
                                            verModal("Error", dt.Rows[0]["Mensaje"].ToString());
                                        }

                                    }
                                }
                                else
                                {
                                    verModal("Error", "La fecha incio es mayor que la fecha fin");
                                }
                            }
                            else
                            {
                                verModal("Error", "La fecha fin no tiene el formato dd/mm/aaaa");
                            }
                        }
                        else
                        {
                            verModal("Error", "Fecha inicio no tiene el formato dd/mm/aaaa");
                        }
                    }
                    else
                    {
                        verModal("Error","El campo descrición no tiene el formato correcto");
                    }
                    
                }
                else
                {
                    verModal("Error","El campo código no tiene el formato correcto");
                }
                
                
               



                
                
                

            }catch(Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }



        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }

        


        public void llenarGrid()
        {
            query = @"
                            select 
	                            Periodo,
	                            Descripcion,
	                            convert(varchar(30),Fecha_inicio,103) as FechaInicio,
	                            convert(varchar(30),Fecha_fin,103) AS  FechaFin,
	                            Activo 
                            from cat_periodos
                            where Periodo !='-1'
                ";
            if (txtFiltrarCodigo.Text != "")
            {
                query += " and Periodo ='" + txtFiltrarCodigo.Text.Trim() + "' ";
            }
            if (txtFiltrarDescripcion.Text != "")
            {
                /*Quitamos los caracteres especiales y solo consideramos estos*/
                txtFiltrarDescripcion.Text = Regex.Replace(txtFiltrarDescripcion.Text, @"[^\w\s\.@-]", "");
                query += " and Descripcion ='" + txtFiltrarDescripcion.Text.Trim() + "' ";
            }
            if (txtFiltrarFechainicio.Text != "")
            {
                txtFiltrarFechainicio.Text = Regex.Replace(txtFiltrarFechainicio.Text, @"[^\/\d]", "");
                query += " and Fecha_inicio = '" + db.convertirFecha(txtFiltrarFechainicio.Text) + "' ";
            }
            if (txtFiltrarFechaFin.Text != "")
            {
                txtFiltrarFechaFin.Text = Regex.Replace(txtFiltrarFechaFin.Text, @"[^\/\d]", "");
                query += " and  Fecha_fin = '" + db.convertirFecha(txtFiltrarFechaFin.Text) + "'";
            }
            if (ddlEstatus.SelectedValue!="")
            {
                query += " and  Activo = "+ddlEstatus.SelectedValue+" ";
            }
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            else
            {
                verModal("Alerta", "No se encontro información");
                GridView1.DataSource = null;
                GridView1.DataBind();
            }
        }

        public void limpiarComponentes()
        {
            txtnombre.Text = "";
            txtDescripcion.Text = "";
            txtFechaInicio.Text = "";
            txtFechaFin.Text = "";
        }

        protected void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {
                int activo = 0;
                if(chkactivo.Checked==true)
                {
                    activo = 1;
                }
                txtDescripcion.Text = Regex.Replace(txtDescripcion.Text, @"[^\w\s\.@-]", "");
                txtFechaInicio.Text = Regex.Replace(txtFechaInicio.Text, @"[^\/\d]", "");
                txtFechaFin.Text = Regex.Replace(txtFechaFin.Text, @"[^\/\d]", "");

                query = "EXEC sp_editar_periodos '" + hdfId_periodo.Value + "','" + txtDescripcion.Text + "','" + db.convertirFecha(txtFechaInicio.Text) + "','" + db.convertirFecha(txtFechaFin.Text) + "','" + Session["usuario"].ToString() + "'," + activo + "";

                dt  =  db.getQuery(conexionBecarios,query);
                if(dt.Rows.Count>0)
                {
                    if(dt.Rows[0]["Mensaje"].ToString()=="Ok")
                    {
                        verModal("Éxito", "Se modificó correctamente el periodo");
                        btnupdate.Visible = false;
                        btnGuardar.Visible = true;
                        limpiarComponentes();
                        llenarGrid();
                        btnCancelar.Visible = false;
                    }
                    else
                    {
                        verModal("Error",dt.Rows[0]["Mensaje"].ToString());
                    }
                    
                }
                else
                {
                    verModal("Error", "No se puede hacer nada, hay un error");
                }
                txtnombre.Enabled = true;
                
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                hdfId_periodo.Value = GridView1.SelectedDataKey.Value.ToString();

                txtnombre.Text = GridView1.SelectedDataKey.Value.ToString();
                    
                txtDescripcion.Text = GridView1.SelectedRow.Cells[1].Text;//inicio
                txtFechaInicio.Text = GridView1.SelectedRow.Cells[2].Text;//inicio
                txtFechaFin.Text = GridView1.SelectedRow.Cells[3].Text;//inicio
                CheckBox c = (CheckBox) GridView1.SelectedRow.Cells[4].Controls[0];
                ClientScript.RegisterStartupScript(GetType(), "BanderasCP", "Bandera();", true);
                txtnombre.Enabled = false;
                if (c.Checked ==true)
                {
                    chkactivo.Checked = true;
                }
                else
                {
                    chkactivo.Checked = false;
                }
                btnGuardar.Visible = false;
                btnupdate.Visible = true;
                btnCancelar.Visible = true;
            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GridView1.PageIndex = e.NewPageIndex;
                llenarGrid();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }




        protected void imbEliminar_click(object sender, EventArgs e)
        {
            try
            {
                int i = int.Parse((sender as ImageButton).CommandArgument);
                //Con este método desactivo una bandera para simulación de eliminar un registro
                eliminarRegistro(i);
                //Con este método refresco el Gridview
                llenarGrid();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        public void eliminarRegistro(int id)
        {
            query = "EXEC sp_eliminar_periodo "+id+"";
            db.getQuery(conexionBecarios,query);
            verModal("Éxito","Se eliminó correctamente el periodo");
        }

        public void limpiarContorles()
        {
            txtnombre.Text = "";
            txtDescripcion.Text = "";
            txtFechaFin.Text = "";
            txtFechaInicio.Text = "";
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                btnGuardar.Visible = true;
                btnupdate.Visible = false;
                btnCancelar.Visible = false;
                GridView1.SelectedIndex = -1;
                limpiarContorles();
                txtnombre.Enabled = true;
            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }

        protected void btnFiltar_Click(object sender, EventArgs e)
        {
            try
            {
                bandera1 = false;
                bandera2 = false;
                vacio = false;
                //txtFiltrarCodigo
                if (db.validaNumeros(txtFiltrarCodigo.Text))
                {
                    if (!string.IsNullOrEmpty(txtFiltrarFechainicio.Text))
                    {
                        match = Regex.Match(txtFiltrarFechainicio.Text, @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$");
                        if (match.Success)
                        {
                            bandera1 = true;
                            vacio = true;
                        }
                        else
                        {
                            verModal("Error", "La fecha inicio no tiene el formato dd/mm/aaaa");
                        }

                    }
                    else
                    {
                        bandera1 = true;
                    }

                    if (!string.IsNullOrEmpty(txtFiltrarFechaFin.Text))
                    {
                        match = Regex.Match(txtFiltrarFechainicio.Text, @"^(0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/](19|20)\d\d$");
                        if (match.Success)
                        {
                            if (vacio)
                            {
                                if (Convert.ToDateTime(txtFiltrarFechainicio.Text) <= Convert.ToDateTime(txtFiltrarFechaFin.Text))
                                {
                                    bandera2 = true;
                                }
                                else
                                {
                                    verModal("Error", "La fecha incio es mayor que la fecha fin");
                                }
                            }
                        }
                        else
                        {
                            verModal("Error", "La fecha fin no tiene el formato dd/mm/aaaa");
                        }

                    }
                    else
                    {
                        bandera2 = true;
                    }
                    if (bandera1 && bandera2)
                    {
                        llenarGrid();
                    }
                }
                else
                {
                    verModal("Error","El campo código no tiene el formato correcto");
                }
                
                
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

    }
}