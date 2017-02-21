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

namespace ServicioBecario.Vistas
{
    public partial class ApoyoFinaciero : System.Web.UI.Page
    {
        string conexionBecarios = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        static string conexionBecariosestatico = ConfigurationManager.ConnectionStrings["ServiobecarioConnectionString"].ConnectionString.ToString();
        string query, mensaje;
        DataTable dt;
        BasedeDatos db = new BasedeDatos();
        string perido_p;
        string campus_p ;
        string nivel_p ;
        string apoyo_p;


        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                periodo();
                campus();
                nivel();
                filtrarcampus();
                filtrarnivel();
                filtrarperiodo();
                pnlmostradosDatosGrid.Visible = true;
                  
            }
        }

        public void filtrarperiodo()
        {
            query = "select Periodo,Descripcion from cat_periodos ";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlfiltrarperiodo.DataTextField = "Descripcion";
                ddlfiltrarperiodo.DataValueField = "Periodo";
                ddlfiltrarperiodo.DataSource = dt;
                ddlfiltrarperiodo.DataBind();
            }


        }

        public void filtrarcampus()
        {
            query = "select Codigo_campus,Nombre from cat_campus order by nombre asc";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlfiltrarCampus.DataTextField = "Nombre";
                ddlfiltrarCampus.DataValueField = "Codigo_campus";
                ddlfiltrarCampus.DataSource = dt;
                ddlfiltrarCampus.DataBind();
            }


        }


        public void filtrarnivel()
        {
            query = "select Codigo_nivel_academico , Nivel_academico  from cat_nivel_academico";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlfiltrarNivel.DataTextField = "Nivel_academico";
                ddlfiltrarNivel.DataValueField = "Codigo_nivel_academico";
                ddlfiltrarNivel.DataSource = dt;
                ddlfiltrarNivel.DataBind();
            }

        }




        public void periodo()
        {
            query = "select Periodo,Descripcion from cat_periodos ";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddPeriodo.DataTextField = "Descripcion";
                ddPeriodo.DataValueField = "Periodo";
                ddPeriodo.DataSource = dt;
                ddPeriodo.DataBind();
            }

        }

        public void campus()
        {
            query = "select Codigo_campus,Nombre from cat_campus order by nombre asc";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlCampus.DataTextField = "Nombre";
                ddlCampus.DataValueField = "Codigo_campus";
                ddlCampus.DataSource = dt;
                ddlCampus.DataBind();
            }

        }


        public void nivel()
        {
            query = "select Codigo_nivel_academico , Nivel_academico  from cat_nivel_academico";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlNivel.DataTextField = "Nivel_academico";
                ddlNivel.DataValueField = "Codigo_nivel_academico";
                ddlNivel.DataSource = dt;
                ddlNivel.DataBind();
            }

        }

        protected void ddlCampus_DataBound(object sender, EventArgs e)
        {
           ddlCampus.Items.Insert(0, new ListItem("-- Seleccione --", ""));
        }

        protected void ddlNivel_DataBound(object sender, EventArgs e)
        {
            ddlNivel.Items.Insert(0, new ListItem("-- Seleccione --", ""));
        }

        protected void ddPeriodo_DataBound(object sender, EventArgs e)
        {
            ddPeriodo.Items.Insert(0, new ListItem("-- Seleccione --", ""));
        }

        public void limpiarcontroles()
        {
            ddlCampus.SelectedValue = "";
            ddlNivel.SelectedValue = "";
            ddPeriodo.SelectedValue = "";
            
        }

        
        
        /*Mensaje*/
        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }

        public void llenarDatos()
        {
            //-- Seleccione --
            perido_p = ddlfiltrarperiodo.SelectedValue;
            campus_p = ddlfiltrarCampus.SelectedValue;
            nivel_p =  ddlfiltrarNivel.SelectedValue;
            apoyo_p = txtfiltrarApoyo.Text.Trim();

            query = @" 
						select
						  pa.id_tipo_apoyo,
						  ta.Tipo_apoyo,
						  c.Nombre as Campus,
						  p.Descripcion as Periodo,
						  na.Nivel_academico as Nivel
						 from tbl_apoyo_financiero pa
						inner join cat_campus c on c.Codigo_campus=pa.Codigo_campus
						inner join cat_periodos p on p.Periodo=pa.Periodo
						inner join Replica_cat_tipo_apoyo ta on ta.Clave_apoyo=pa.Clave_apoyo
						inner join cat_nivel_academico na on na.Codigo_nivel_academico=pa.Codigo_nivel_academico
						where c.Codigo_campus!='-1'
                    ";



            if (campus_p != "-1") { query+=" And c.Codigo_campus ='"+campus_p+"'"; }
            if (nivel_p != "-1") { query += " and na.Codigo_nivel_academico ="+nivel_p; }
            if (perido_p != "-1") { query += " and p.Periodo = '"+perido_p+"' "; }
            if (apoyo_p != "") { query +=" and ta.Tipo_apoyo like '%"+apoyo_p+"%' ";}

            
            
            
            
            
            
            
            
            
            //if (campus_p != "-1" && nivel_p == "-- Seleccione --" && perido_p =="-1" && apoyo_p=="")//1
            //{
            //    query = "sp_mostra_apoyos_financieros  '"+campus_p+"',NULL,-1,NULL";
            //}
            //if (campus_p == "-1" && nivel_p != "-- Seleccione --" && perido_p == "-1" && apoyo_p == "")//2
            //{
            //    query = "sp_mostra_apoyos_financieros  -1,'"+nivel_p+"',-1,NULL";
            //}
            //if (campus_p == "-1" && nivel_p == "-- Seleccione --" && perido_p != "-1" && apoyo_p == "")//3
            //{
            //    query = "sp_mostra_apoyos_financieros  -1,NULL,'"+perido_p+"',NULL";
            //}
            //if (campus_p == "-1" && nivel_p == "-- Seleccione --" && perido_p == "-1" && apoyo_p != "")//4
            //{
            //    query = "sp_mostra_apoyos_financieros  -1,NULL,-1,'"+apoyo_p+"'";
            //}
            //if (campus_p != "-1" && nivel_p != "-- Seleccione --" && perido_p == "-1" && apoyo_p == "")//5
            //{
            //    query = "sp_mostra_apoyos_financieros "+campus_p+",'"+nivel_p+"',-1,NULL";
            //}
            //if (campus_p != "-1" && nivel_p == "-- Seleccione --" && perido_p != "-1" && apoyo_p == "")//6
            //{
            //    query = "sp_mostra_apoyos_financieros '"+campus_p+"',NULL,'"+perido_p+"',NULL ";
            //}
            //if (campus_p != "-1" && nivel_p == "-- Seleccione --" && perido_p == "-1" && apoyo_p != "")//7
            //{
            //    query = "sp_mostra_apoyos_financieros  '"+campus_p+"',NULL,-1,'"+apoyo_p+"' ";
            //}
            //if (campus_p == "-1" && nivel_p != "-- Seleccione --" && perido_p != "-1" && apoyo_p == "")//8
            //{
            //    query = "sp_mostra_apoyos_financieros -1,'"+nivel_p+"','"+perido_p+"' ,NULL";
            //}
            //if (campus_p == "-1" && nivel_p != "-- Seleccione --" && perido_p == "-1" && apoyo_p != "")//9
            //{
            //    query = "sp_mostra_apoyos_financieros -1,'"+nivel_p+"',-1,'"+apoyo_p+"'   ";
            //}
            //if (campus_p == "-1" && nivel_p == "-- Seleccione --" && perido_p != "-1" && apoyo_p != "")//10
            //{
            //    query = "sp_mostra_apoyos_financieros -1, NULL,'"+perido_p+"','"+apoyo_p+"'  ";
            //}
            //if (campus_p != "-1" && nivel_p != "-- Seleccione --" && perido_p != "-1" && apoyo_p == "")//11
            //{
            //    query = "sp_mostra_apoyos_financieros '"+campus_p+"','"+nivel_p+"','"+perido_p+"',NULL ";
            //}
            //if (campus_p != "-1" && nivel_p != "-- Seleccione --" && perido_p == "-1" && apoyo_p != "")//12
            //{
            //    query = "sp_mostra_apoyos_financieros '"+campus_p+"','"+nivel_p+"',-1,'"+apoyo_p+"'";
            //}
            //if (campus_p == "-1" && nivel_p != "-- Seleccione --" && perido_p != "-1" && apoyo_p != "")//13
            //{
            //    query = "sp_mostra_apoyos_financieros  -1 , '"+nivel_p+"','"+perido_p+"','"+apoyo_p+"'";
            //}
            //if (campus_p != "-1" && nivel_p == "-- Seleccione --" && perido_p != "-1" && apoyo_p != "")//14
            //{
            //    query = "sp_mostra_apoyos_financieros '"+campus_p+"',NULL,'"+perido_p+"','"+apoyo_p+"' ";
            //}
            //if (campus_p != "-1" && nivel_p != "-- Seleccione --" && perido_p != "-1" && apoyo_p != "")//15
            //{
            //    query = "sp_mostra_apoyos_financieros '"+campus_p+"','"+nivel_p+"','"+perido_p+"','"+apoyo_p+"' ";
            //}
            //if (campus_p == "-1" && nivel_p == "-- Seleccione --" && perido_p == "-1" && apoyo_p == "")//16
            //{
            //    query = "sp_mostra_apoyos_financieros -1,NULL,-1, NULL ";
            //}
            dt = db.getQuery(conexionBecarios,query);
            if(dt.Rows.Count>0)
            {
                gvdatos.DataSource = dt;
                gvdatos.DataBind();
            }
            else
            {
                verModal("Alerta", "No se encontró información en la búsqueda");
                gvdatos.DataSource = null;
                gvdatos.DataBind();
            }
        }

        protected void ddlfiltrarCampus_DataBound(object sender, EventArgs e)
        {
            ddlfiltrarCampus.Items.Insert(0, new ListItem("-- Seleccione --", "-1"));
        }

        protected void ddlfiltrarNivel_DataBound(object sender, EventArgs e)
        {
            ddlfiltrarNivel.Items.Insert(0, new ListItem("-- Seleccione --", "-1"));
        }

        protected void ddlfiltrarperiodo_DataBound(object sender, EventArgs e)
        {
            ddlfiltrarperiodo.Items.Insert(0, new ListItem("-- Seleccione --", "-1"));
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                llenarDatos();

            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                //query = "sp_agregar_apoyo_financiero  '"+ddlCampus.SelectedValue+"','"+ddlNivel.SelectedItem.Text+"','"+ddPeriodo.SelectedValue+"','"+txtApoyo.Text+"'";
                //dt = db.getQuery(conexionBecarios,query);
                //if (dt.Rows.Count > 0)
                //{
                //    if(dt.Rows[0]["Mensaje"].ToString()=="Ok")
                //    {
                //        verModal("Éxito","Se guardó correctamente la información");
                //        limpiarcontroles();
                //        llenarDatos();
                //    }
                //    else
                //    {
                //        verModal("Éxito", "Ya existe el registro");
                //    }
                //}
            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }

        protected void gvdatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string dat;
                hdfid_apoyo.Value = gvdatos.SelectedDataKey.Value.ToString();
               
                ddlCampus.SelectedValue = ddlCampus.Items.FindByText( gvdatos.SelectedRow.Cells[2].Text).Value;
                ddPeriodo.SelectedValue = ddPeriodo.Items.FindByText( gvdatos.SelectedRow.Cells[3].Text).Value;               
                ddlNivel.SelectedValue =  ddlNivel.Items.FindByText(HttpUtility.HtmlDecode(gvdatos.SelectedRow.Cells[4].Text)).Value;
                btnGuardar.Visible = false;
                btnModificar.Visible = true;
                btnCancelar.Visible = true;
            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }


        protected void imbEliminar_click(object sender, EventArgs e)
        {
            try
            {   
                string id = (sender as ImageButton).CommandArgument;                             
                eliminarMenu(int.Parse(id));
                string campus = hdfCampus.Value;
                string nivel = hdfNivel.Value;
                string periodo = hdfCampus.Value;
               // ViewState["cont"] = "1";
                llenarDatos();
                if(periodo!="" && campus!="" && nivel!="")
                {
                    lblComponentes.Text = "";
                    llenar_check_versiondos(campus,periodo,nivel);
                }


            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        public void llenar_check_versiondos(string campus, string periodo, string nivel)
        {
            query = @"
                        SELECT 
                                ct.Clave_apoyo,
                                ct.Tipo_apoyo 
                        FROM Replica_cat_tipo_apoyo ct 
                        WHERE Clave_apoyo NOT IN(SELECT Clave_apoyo FROM tbl_apoyo_financiero d WHERE d.Codigo_nivel_academico= " + nivel + @"  
                        AND  d.Codigo_campus  IN ('" + campus+ "') AND d.Periodo='" + periodo + "' ) AND ct.Nivel_apoyo=" + nivel + "";

            string html = "", hdos = "";
            int renglon = 0, i = 0;
            bool bandera = false;
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                int cont = dt.Rows.Count;
                foreach (DataRow row in dt.Rows)
                {
                    hdos = " <input id='ches" + dt.Rows[i]["Clave_apoyo"].ToString() + "'  name='radios' class='formass css-checkbox'    type='checkbox' value=" + dt.Rows[i]["Clave_apoyo"].ToString() + " />   <label for='ches" + dt.Rows[i]["Clave_apoyo"] + "' name='che" + dt.Rows[i]["Clave_apoyo"] + "' class='css-label'>" + dt.Rows[i]["Tipo_apoyo"] + "</label> ";
                    if (renglon % 2 == 0)
                    {
                        html += " <div class='row'> <div class='col-md-6'>" + hdos + "</div>";
                    }
                    else
                    {
                        html += "<div class='col-md-6'> " + hdos + "  </div>  </div>";
                    }
                    i++;
                    renglon++;

                }
                if (dt.Rows.Count % 2 != 0)
                {
                    html += "</div>";
                }

                lblComponentes.Text = html;
            }
            else
            {
                lblComponentes.Text = "";
                verModal("Alerta", "ya están cargados los elementos o no existen");
            }
        }

        public void eliminarMenu(int i)
        {
            query = "sp_eliminar_apoyo_financiero "+i+"";
            dt = db.getQuery(conexionBecarios,query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                {
                    verModal("Éxito","Se eliminó correctamente la información");
                    btnGuardar.Visible = true;
                    btnModificar.Visible = false;
                    limpiarcontroles();
                }
                else
                {
                    verModal("Error", "Sucedió un error interno");
                }
            }
        }

        protected void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dts;
                query = "sp_modificar_apoyo_financiero "+hdfid_apoyo.Value+",'"+ddlCampus.SelectedValue+"','"+ddPeriodo.SelectedValue+"','"+ddlNivel.SelectedItem.Text+"'";
                dt = db.getQuery(conexionBecarios,query);
                if (dt.Rows.Count > 0)
                {
                    dts = dt;
                    if (dts.Rows[0]["Mensaje"].ToString() == "Ok")
                    {
                        llenarDatos();
                        verModal("Éxito", "Se modificó correctamente la información");
                    }
                    if (dts.Rows[0]["Mensaje"].ToString() == "Existe")
                    {
                        verModal("Error", "Ya existe un registro con las mismas caracteristicas");
                    }
                    btnCancelar.Visible = false;
                    btnModificar.Visible = false;
                    btnGuardar.Visible = true;
                    limpiarComponentes();
                }
            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }

        protected void gvdatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvdatos.PageIndex = e.NewPageIndex;
                llenarDatos();
            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }

        protected void gvdatos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string de = ViewState["cont"].ToString();
                if (de=="1")
                {
                    ViewState["cont"] = "2";
                    e.Cancel = true;
                    gvdatos.DeleteRow(e.RowIndex);
                    gvdatos.DataBind();
                }

            
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                btnCancelar.Visible = false;
                btnModificar.Visible = false;
                btnGuardar.Visible = true;
                gvdatos.SelectedIndex = -1;
                limpiarComponentes();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        public void limpiarComponentes()
        {
            ddlCampus.SelectedValue = "";
            ddlNivel.SelectedValue = "";
            ddPeriodo.SelectedValue = "";
          
        }

        protected void ddlNivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(ddlCampus.SelectedValue!="")                
                {
                    if (ddlCampus.SelectedValue != "")    
                    {
                        if (ddlNivel.SelectedValue != "")
                        {
                            hdfCampus.Value = ddlCampus.SelectedValue;
                            hdfNivel.Value = ddlNivel.SelectedValue;
                            hdfPeriodo.Value = ddPeriodo.SelectedValue;
                            llenar_check();
                        }
                        else
                        {
                            verModal("Alerta", "Selecciona nivel");
                        }
                        
                    }
                    else
                    {
                        verModal("Alerta", "Selecciona un campus");
                        
                        
                    }
                    
                }
                else
                {
                    verModal("Alerta", "Selecciona un periodo");
                }

                
            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
           
        }
        //llenamos el texto con html
        public void llenar_check()
        {
            query = @"
                        SELECT 
                                ct.Clave_apoyo,
                                ct.Tipo_apoyo 
                        FROM Replica_cat_tipo_apoyo ct 
                        WHERE Clave_apoyo NOT IN(SELECT Clave_apoyo FROM tbl_apoyo_financiero d WHERE d.Codigo_nivel_academico= "+ddlNivel.SelectedValue+ @"  
                        AND  d.Codigo_campus  IN ('" + ddlCampus.SelectedValue + "') AND d.Periodo='" + ddPeriodo.SelectedValue + "' ) AND ct.Nivel_apoyo="+ddlNivel.SelectedValue+"";
            
            string html = "", hdos = "";
            int renglon = 0, i = 0;
         
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                int cont = dt.Rows.Count;
                foreach (DataRow row in dt.Rows)
                {
                    hdos = " <input id='ches" + dt.Rows[i]["Clave_apoyo"].ToString() + "'  name='radios' class='formass css-checkbox'    type='checkbox' value=" + dt.Rows[i]["Clave_apoyo"].ToString() + " />   <label for='ches" + dt.Rows[i]["Clave_apoyo"] + "' name='che" + dt.Rows[i]["Clave_apoyo"] + "' class='css-label'>" + dt.Rows[i]["Tipo_apoyo"] + "</label> ";
                    if (renglon % 2 == 0)

                    {
                        html += " <div class='row'> <div class='col-md-6'>" + hdos + "</div>";
                    }
                    else
                    {
                        html += "<div class='col-md-6'> " + hdos + "  </div>  </div>"; 
                    }
                    i++;
                    renglon++;
        
                }
                if (dt.Rows.Count % 2 != 0)
                {
                    html += "</div>";
                }

                lblComponentes.Text = html;
            }
            else
            {
                lblComponentes.Text = "";
                verModal("Alerta","ya están cargados los elementos o no existen");
            }

        }

        [WebMethod]
        public static string agregarElementos(string cadena, string campus, string periodo, string nivel)
        {

            string solvencie="";
            string []dato = cadena.Split('!');
             foreach(string clave in dato)
             {
                 if(clave != "")
                 {
                     string query = @"insert into tbl_apoyo_financiero(Codigo_campus,Periodo,Clave_apoyo,Codigo_nivel_academico)
                                 values('" + campus + "','" + periodo + "',"+clave+"," + nivel + ") ";
                     getQuery(conexionBecariosestatico, query);
                     solvencie = "Agregadó correctamente";
                 }
                 
             }

             


            return solvencie;
        }


        [WebMethod]
        public static string mostrar( string campus, string periodo, string nivel )
        {
            string html = "";
            DataTable dt = new DataTable();
            string query = @"
                        SELECT 
                                ct.Clave_apoyo,
                                ct.Tipo_apoyo 
                        FROM Replica_cat_tipo_apoyo ct 
                        WHERE Clave_apoyo NOT IN(SELECT Clave_apoyo FROM tbl_apoyo_financiero d WHERE d.Codigo_nivel_academico= " + nivel + @"  
                        AND  d.Codigo_campus  IN ('" + campus + "') AND d.Periodo='" + periodo + "' )";
            //query = "SELECT Clave_apoyo,Tipo_apoyo FROM Replica_cat_tipo_apoyo where Nivel_apoyo = " + ddlNivel.SelectedValue;
            string  hdos = "";
            int renglon = 0, i = 0;
            bool bandera = false;
            dt = getQuery(conexionBecariosestatico, query);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    hdos = " <input id='che'  name='radios' class='formass'    type='checkbox' value=" + dt.Rows[i]["Clave_apoyo"].ToString() + " /> " + dt.Rows[i]["Tipo_apoyo"].ToString() + " ";
                    if (renglon == 0)
                    {
                        html += " <div class='row' id='verda'> <div class='col-md-6'>" + hdos + "</div>";
                        bandera = true;
                    }
                    if (renglon < 1 && bandera == false)
                    {
                        html += "<div class='col-md-6'>  " + hdos + "  </div> ";
                        bandera = false;
                    }
                    if (renglon == 1)
                    {
                        html += "<div class='col-md-6'> " + hdos + "  </div>  </div>";
                        renglon = -1;
                        bandera = false;
                    }
                    i++;
                    renglon++;
                    bandera = false;
                }
                //lblComponentes.Text = html;
            }

            return html;
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
             lblComponentes.Text = "";
             llenar_check();
         }
    }
}