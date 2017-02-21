using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using ServicioBecario.Codigo;
using System.Text;


namespace ServicioBecario.Vistas
{
    public partial class General : System.Web.UI.Page
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
                    ddlPAcademico.Items.Insert(0, new ListItem("-- Seleccione --", "-1"));
                    llenarPerido();
                    llenarNivel();
                    //llenarCampus();
                    vermisosdeCampus();
                    llenarCalificacion();
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
        public void llenarPerido()
        {
            query = "select Periodo,Descripcion from cat_periodos";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlperiodo.DataValueField = "Periodo";
                ddlperiodo.DataTextField = "Descripcion";
                ddlperiodo.DataSource = dt;
                ddlperiodo.DataBind();
            }
        }
        public void llenarNivel()
        {
            query = "select Codigo_nivel_academico,Nivel_academico from cat_nivel_academico";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlNivel.DataTextField = "Nivel_academico";
                ddlNivel.DataValueField = "Codigo_nivel_academico";
                ddlNivel.DataSource = dt;
                ddlNivel.DataBind();
            }
        }

        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }

        protected void ddlperiodo_DataBound(object sender, EventArgs e)
        {
            ddlperiodo.Items.Insert(0, new ListItem("-- Seleccione --", "-1"));
        }

        protected void ddlNivel_DataBound(object sender, EventArgs e)
        {
            ddlNivel.Items.Insert(0, new ListItem("-- Seleccione --", "-1"));
        }

        protected void ddlNivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                llenarProgramaAcademico();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }
        public void llenarProgramaAcademico()
        {
            query = "select Codigo_programa_academico , Nombre_programa_academico AS programa from cat_programa_acedemico where Codigo_nivel_academico =" + ddlNivel.SelectedValue+" order by Nombre_programa_academico";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlPAcademico.DataValueField = "Codigo_programa_academico";
                ddlPAcademico.DataTextField = "programa";
                ddlPAcademico.DataSource = dt;
                ddlPAcademico.DataBind();
            }
        }

        protected void ddlPAcademico_DataBound(object sender, EventArgs e)
        {
            ddlPAcademico.Items.Insert(0, new ListItem("-- Seleccione --", "-1"));
        }

        public void llenarCampus()
        {
            query = "select Codigo_campus,Nombre from cat_campus order by Nombre";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlCampus.DataValueField = "Codigo_campus";
                ddlCampus.DataTextField = "Nombre";
                ddlCampus.DataSource = dt;
                ddlCampus.DataBind();
            }
        }

        protected void ddlCampus_DataBound(object sender, EventArgs e)
        {
            ddlCampus.Items.Insert(0, new ListItem("-- Seleccione --", ""));
        }
        public void llenarCalificacion()
        {
            query = "select id_calificacion,Valor_calificacion from cat_calificacion";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlevaluados.DataValueField = "id_calificacion";
                ddlevaluados.DataTextField = "Valor_calificacion";
                ddlevaluados.DataSource = dt;
                ddlevaluados.DataBind();
            }
        }

        protected void ddlevaluados_DataBound(object sender, EventArgs e)
        {
            ddlevaluados.Items.Insert(0, new ListItem("-- Seleccione --", "-1"));
        }

        protected void btnfiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                filtrarDatos();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        public void filtrarDatos()
        {
          /*  query = "sp_muestra_reportes_becarios ";

            if (ddlperiodo.SelectedValue != "-1") { query += "'" + ddlperiodo.SelectedValue + "',"; } else { query += "'-1',"; }
            if (ddlCampus.SelectedValue != "-1") { query += "'" + ddlCampus.SelectedValue + "',"; } else { query += "'-1',"; }
            if (ddlPAcademico.SelectedItem.Text == "-- Seleccione --") { query += "null,"; } else { query += "'"+ddlPAcademico.SelectedItem.Text+"',"; }
            if (ddlNivel.SelectedItem.Text == "-- Seleccione --") { query += "null,"; } else { query += "'" + ddlNivel.SelectedItem.Text + "',"; }
            if ( ddlevaluados.SelectedItem.Text == "-- Seleccione --") { query += "null,"; } else { query += "1"; }
            if (chkNoEvaluados.Checked==false) { query += "0"; } else { query += "-1"; }


            */

            query = @" select  a.Matricula,
                               a.nombre +' '+ a.Apellido_paterno + ' ' + a.Apellido_materno as  Alumno,
                               p.Descripcion as Periodo,
                               sb.Programa ,
                               case  
                               when ps.Nombre is null then 'N/A'
                               else ps.Nombre
                               end  as Proyecto,
                               sb.Becario_calificacion as Evaluacion
                               from tbl_solicitudes s 
                               inner join tbl_solicitudes_becarios sb on s.id_MiSolicitud=sb.id_Misolicitud
                               inner join cat_periodos p on p.Periodo=s.Periodo
                               inner join tbl_empleados e on e.Nomina=s.Nomina
                               inner join cat_campus c on c.Codigo_campus=e.Codigo_campus
                               inner join cat_estatus_asignacion ea on ea.id_estatus_asignacion=sb.id_estatus_asignacion
                               inner join tbl_alumnos a on a.Matricula=sb.Matricula
                               left join tbl_proyectos ps on ps.id_proyecto=s.id_proyecto
                               where a.nombre!='' ";

            if (ddlperiodo.SelectedValue != "-1") { query += " AND p.Periodo='" + ddlperiodo.SelectedValue + "'"; }
            if(hdfActivarRol.Value=="1")
            {
                if (ddlCampus.SelectedValue != "-1") { query += " AND c.Codigo_campus='" + ddlCampus.SelectedValue + "'"; }
            }
            else
            {
                query += " AND c.Codigo_campus='" + hdfMostrarId.Value + "'";
            }
            
            if (ddlPAcademico.SelectedItem.Text != "-- Seleccione --") { query += " AND sb.Programa='" + ddlPAcademico.SelectedValue + "'"; }
            if (ddlNivel.SelectedItem.Text != "-- Seleccione --") { query += " AND sb.Codigo_nivel_academico='" + ddlNivel.SelectedValue + "'"; }
            if (ddlevaluados.SelectedItem.Text != "-- Seleccione --") { query += " AND sb.Becario_calificacion='" + ddlevaluados.SelectedItem.Text+"'"; }
            if (chkNoEvaluados.Checked == true) { query += " AND ea.Estatus_asignacion='Asignado'"; } 


















            /*

            if (ddlperiodo.SelectedValue != "-1" && ddlCampus.SelectedValue == "-1" && ddlPAcademico.SelectedItem.Text == "-- Seleccione --" && ddlNivel.SelectedItem.Text == "-- Seleccione --" && ddlevaluados.SelectedItem.Text == "-- Seleccione --" && chkNoEvaluados.Checked == false)//1
            {
                query = "sp_muestra_reportes_becarios " + ddlperiodo.SelectedValue + ",-1,null,null,null,0";
            }
            if (ddlperiodo.SelectedValue == "-1" && ddlCampus.SelectedValue != "-1" && ddlPAcademico.SelectedItem.Text == "-- Seleccione --" && ddlNivel.SelectedItem.Text == "-- Seleccione --" && ddlevaluados.SelectedItem.Text == "-- Seleccione --" && chkNoEvaluados.Checked == false)//2
            {
                query = "sp_muestra_reportes_becarios -1," + ddlCampus.SelectedValue + ",null,null,null,0";
            }
            if (ddlperiodo.SelectedValue == "-1" && ddlCampus.SelectedValue == "-1" && ddlPAcademico.SelectedItem.Text != "-- Seleccione --" && ddlNivel.SelectedItem.Text == "-- Seleccione --" && ddlevaluados.SelectedItem.Text == "-- Seleccione --" && chkNoEvaluados.Checked == false)//3
            {
                query = "sp_muestra_reportes_becarios -1,-1,'" + ddlPAcademico.SelectedItem.Text + "',null,null,0";
            }
            if (ddlperiodo.SelectedValue == "-1" && ddlCampus.SelectedValue == "-1" && ddlPAcademico.SelectedItem.Text == "-- Seleccione --" && ddlNivel.SelectedItem.Text != "-- Seleccione --" && ddlevaluados.SelectedItem.Text == "-- Seleccione --" && chkNoEvaluados.Checked == false)//4
            {
                query = "sp_muestra_reportes_becarios -1,-1,null,'" + ddlNivel.SelectedItem.Text + "',null,0";
            }
            if (ddlperiodo.SelectedValue == "-1" && ddlCampus.SelectedValue == "-1" && ddlPAcademico.SelectedItem.Text == "-- Seleccione --" && ddlNivel.SelectedItem.Text == "-- Seleccione --" && ddlevaluados.SelectedItem.Text != "-- Seleccione --" && chkNoEvaluados.Checked == false)//5
            {
                query = "sp_muestra_reportes_becarios -1,-1,null,null,'" + ddlevaluados.SelectedItem.Text + "',0";
            }
            if (ddlperiodo.SelectedValue == "-1" && ddlCampus.SelectedValue == "-1" && ddlPAcademico.SelectedItem.Text == "-- Seleccione --" && ddlNivel.SelectedItem.Text == "-- Seleccione --" && ddlevaluados.SelectedItem.Text == "-- Seleccione --" && chkNoEvaluados.Checked != false)//6
            {
                query = "sp_muestra_reportes_becarios -1,-1,null,null,null,1";
            }
            if (ddlperiodo.SelectedValue != "-1" && ddlCampus.SelectedValue != "-1" && ddlPAcademico.SelectedItem.Text == "-- Seleccione --" && ddlNivel.SelectedItem.Text == "-- Seleccione --" && ddlevaluados.SelectedItem.Text == "-- Seleccione --" && chkNoEvaluados.Checked == false)//7
            {
                query = "sp_muestra_reportes_becarios " + ddlperiodo.SelectedValue + "," + ddlCampus.SelectedValue + ",null,null,null,0";
            }
            if (ddlperiodo.SelectedValue != "-1" && ddlCampus.SelectedValue == "-1" && ddlPAcademico.SelectedItem.Text != "-- Seleccione --" && ddlNivel.SelectedItem.Text == "-- Seleccione --" && ddlevaluados.SelectedItem.Text == "-- Seleccione --" && chkNoEvaluados.Checked == false)//8
            {
                query = "sp_muestra_reportes_becarios " + ddlperiodo.SelectedValue + ",-1,'" + ddlPAcademico.SelectedItem.Text + "',null,null,0";
            }
            if (ddlperiodo.SelectedValue != "-1" && ddlCampus.SelectedValue == "-1" && ddlPAcademico.SelectedItem.Text == "-- Seleccione --" && ddlNivel.SelectedItem.Text != "-- Seleccione --" && ddlevaluados.SelectedItem.Text == "-- Seleccione --" && chkNoEvaluados.Checked == false)//9
            {
                query = "sp_muestra_reportes_becarios " + ddlperiodo.SelectedValue + ",-1,null,'" + ddlNivel.SelectedItem.Text + "',null,0";
            }
            if (ddlperiodo.SelectedValue != "-1" && ddlCampus.SelectedValue == "-1" && ddlPAcademico.SelectedItem.Text == "-- Seleccione --" && ddlNivel.SelectedItem.Text == "-- Seleccione --" && ddlevaluados.SelectedItem.Text != "-- Seleccione --" && chkNoEvaluados.Checked == false)//10
            {
                query = "sp_muestra_reportes_becarios " + ddlperiodo.SelectedValue + ",-1,null,null,'" + ddlevaluados.SelectedItem.Text + "',0";
            }
            if (ddlperiodo.SelectedValue != "-1" && ddlCampus.SelectedValue == "-1" && ddlPAcademico.SelectedItem.Text == "-- Seleccione --" && ddlNivel.SelectedItem.Text == "-- Seleccione --" && ddlevaluados.SelectedItem.Text == "-- Seleccione --" && chkNoEvaluados.Checked != false)//11
            {
                query = "sp_muestra_reportes_becarios " + ddlperiodo.SelectedValue + ",-1,null,null,null,1";
            }
            if (ddlperiodo.SelectedValue == "-1" && ddlCampus.SelectedValue != "-1" && ddlPAcademico.SelectedItem.Text != "-- Seleccione --" && ddlNivel.SelectedItem.Text == "-- Seleccione --" && ddlevaluados.SelectedItem.Text == "-- Seleccione --" && chkNoEvaluados.Checked == false)//12
            {
                query = "sp_muestra_reportes_becarios -1," + ddlCampus.SelectedValue + ",'" + ddlPAcademico.SelectedItem.Text + "',null,null,0";
            }
            if (ddlperiodo.SelectedValue == "-1" && ddlCampus.SelectedValue != "-1" && ddlPAcademico.SelectedItem.Text == "-- Seleccione --" && ddlNivel.SelectedItem.Text != "-- Seleccione --" && ddlevaluados.SelectedItem.Text == "-- Seleccione --" && chkNoEvaluados.Checked == false)//13
            {
                query = "sp_muestra_reportes_becarios -1," + ddlCampus.SelectedValue + ",null,'" + ddlNivel.SelectedItem.Text + "',null,0";
            }
            if (ddlperiodo.SelectedValue == "-1" && ddlCampus.SelectedValue != "-1" && ddlPAcademico.SelectedItem.Text == "-- Seleccione --" && ddlNivel.SelectedItem.Text == "-- Seleccione --" && ddlevaluados.SelectedItem.Text != "-- Seleccione --" && chkNoEvaluados.Checked == false)//14
            {
                query = "sp_muestra_reportes_becarios -1," + ddlCampus.SelectedValue + ",null,null,'" + ddlevaluados.SelectedItem.Text + "',0";
            }
            if (ddlperiodo.SelectedValue == "-1" && ddlCampus.SelectedValue != "-1" && ddlPAcademico.SelectedItem.Text == "-- Seleccione --" && ddlNivel.SelectedItem.Text == "-- Seleccione --" && ddlevaluados.SelectedItem.Text == "-- Seleccione --" && chkNoEvaluados.Checked != false)//15
            {
                query = "sp_muestra_reportes_becarios -1," + ddlCampus.SelectedValue + ",null,null,null,1";
            }
            if (ddlperiodo.SelectedValue == "-1" && ddlCampus.SelectedValue == "-1" && ddlPAcademico.SelectedItem.Text != "-- Seleccione --" && ddlNivel.SelectedItem.Text != "-- Seleccione --" && ddlevaluados.SelectedItem.Text == "-- Seleccione --" && chkNoEvaluados.Checked == false)//16
            {
                query = "sp_muestra_reportes_becarios -1,-1,'" + ddlPAcademico.SelectedItem.Text + "','" + ddlNivel.SelectedItem.Text + "',null,0";
            }
            if (ddlperiodo.SelectedValue == "-1" && ddlCampus.SelectedValue == "-1" && ddlPAcademico.SelectedItem.Text != "-- Seleccione --" && ddlNivel.SelectedItem.Text == "-- Seleccione --" && ddlevaluados.SelectedItem.Text != "-- Seleccione --" && chkNoEvaluados.Checked == false)//17
            {
                query = "sp_muestra_reportes_becarios -1,-1,'" + ddlPAcademico.SelectedItem.Text + "',null,'" + ddlevaluados.SelectedItem.Text + "',0";
            }
            if (ddlperiodo.SelectedValue == "-1" && ddlCampus.SelectedValue == "-1" && ddlPAcademico.SelectedItem.Text != "-- Seleccione --" && ddlNivel.SelectedItem.Text == "-- Seleccione --" && ddlevaluados.SelectedItem.Text == "-- Seleccione --" && chkNoEvaluados.Checked != false)//18
            {
                query = "sp_muestra_reportes_becarios -1,-1,'" + ddlPAcademico.SelectedItem.Text + "',null,null,1";
            }
            if (ddlperiodo.SelectedValue == "-1" && ddlCampus.SelectedValue == "-1" && ddlPAcademico.SelectedItem.Text == "-- Seleccione --" && ddlNivel.SelectedItem.Text != "-- Seleccione --" && ddlevaluados.SelectedItem.Text != "-- Seleccione --" && chkNoEvaluados.Checked == false)//19
            {
                query = "sp_muestra_reportes_becarios -1,-1,null,'" + ddlNivel.SelectedItem.Text + "','" + ddlevaluados.SelectedItem.Text + "',0";
            }
            if (ddlperiodo.SelectedValue == "-1" && ddlCampus.SelectedValue == "-1" && ddlPAcademico.SelectedItem.Text == "-- Seleccione --" && ddlNivel.SelectedItem.Text != "-- Seleccione --" && ddlevaluados.SelectedItem.Text == "-- Seleccione --" && chkNoEvaluados.Checked != false)//20
            {
                query = "sp_muestra_reportes_becarios -1,-1,null,'" + ddlNivel.SelectedItem.Text + "',null,1";
            }
            if (ddlperiodo.SelectedValue != "-1" && ddlCampus.SelectedValue != "-1" && ddlPAcademico.SelectedItem.Text != "-- Seleccione --" && ddlNivel.SelectedItem.Text == "-- Seleccione --" && ddlevaluados.SelectedItem.Text == "-- Seleccione --" && chkNoEvaluados.Checked == false)//21
            {
                query = "sp_muestra_reportes_becarios " + ddlperiodo.SelectedValue + "," + ddlCampus.SelectedValue + ",'" + ddlPAcademico.SelectedItem.Text + "',null,null,0";
            }
            if (ddlperiodo.SelectedValue != "-1" && ddlCampus.SelectedValue != "-1" && ddlPAcademico.SelectedItem.Text == "-- Seleccione --" && ddlNivel.SelectedItem.Text != "-- Seleccione --" && ddlevaluados.SelectedItem.Text == "-- Seleccione --" && chkNoEvaluados.Checked == false)//22
            {
                query = "sp_muestra_reportes_becarios " + ddlperiodo.SelectedValue + "," + ddlCampus.SelectedValue + ",null,'" + ddlNivel.SelectedItem.Text + "',null,0";
            }
            if (ddlperiodo.SelectedValue != "-1" && ddlCampus.SelectedValue != "-1" && ddlPAcademico.SelectedItem.Text == "-- Seleccione --" && ddlNivel.SelectedItem.Text == "-- Seleccione --" && ddlevaluados.SelectedItem.Text != "-- Seleccione --" && chkNoEvaluados.Checked == false)//23
            {
                query = "sp_muestra_reportes_becarios " + ddlperiodo.SelectedValue + "," + ddlCampus.SelectedValue + ",null,null,'" + ddlevaluados.SelectedItem.Text + "',0";
            }
            if (ddlperiodo.SelectedValue != "-1" && ddlCampus.SelectedValue != "-1" && ddlPAcademico.SelectedItem.Text == "-- Seleccione --" && ddlNivel.SelectedItem.Text == "-- Seleccione --" && ddlevaluados.SelectedItem.Text == "-- Seleccione --" && chkNoEvaluados.Checked != false)//24
            {
                query = "sp_muestra_reportes_becarios " + ddlperiodo.SelectedValue + "," + ddlCampus.SelectedValue + ",null,null,null,1";
            }
            if (ddlperiodo.SelectedValue != "-1" && ddlCampus.SelectedValue == "-1" && ddlPAcademico.SelectedItem.Text != "-- Seleccione --" && ddlNivel.SelectedItem.Text != "-- Seleccione --" && ddlevaluados.SelectedItem.Text == "-- Seleccione --" && chkNoEvaluados.Checked == false)//25
            {
                query = "sp_muestra_reportes_becarios " + ddlperiodo.SelectedValue + ",-1,'" + ddlPAcademico.SelectedItem.Text + "','" + ddlNivel.SelectedItem.Text + "',null,0";
            }
            if (ddlperiodo.SelectedValue != "-1" && ddlCampus.SelectedValue == "-1" && ddlPAcademico.SelectedItem.Text != "-- Seleccione --" && ddlNivel.SelectedItem.Text == "-- Seleccione --" && ddlevaluados.SelectedItem.Text != "-- Seleccione --" && chkNoEvaluados.Checked == false)//26
            {
                query = "sp_muestra_reportes_becarios " + ddlperiodo.SelectedValue + ",-1,'" + ddlPAcademico.SelectedItem.Text + "',null,'" + ddlevaluados.SelectedItem.Text + "',0";
            }
            if (ddlperiodo.SelectedValue != "-1" && ddlCampus.SelectedValue == "-1" && ddlPAcademico.SelectedItem.Text != "-- Seleccione --" && ddlNivel.SelectedItem.Text == "-- Seleccione --" && ddlevaluados.SelectedItem.Text == "-- Seleccione --" && chkNoEvaluados.Checked != false)//27
            {
                query = "sp_muestra_reportes_becarios " + ddlperiodo.SelectedValue + ",-1,'" + ddlPAcademico.SelectedItem.Text + "',null,null,1";
            }
            if (ddlperiodo.SelectedValue == "-1" && ddlCampus.SelectedValue == "-1" && ddlPAcademico.SelectedItem.Text != "-- Seleccione --" && ddlNivel.SelectedItem.Text != "-- Seleccione --" && ddlevaluados.SelectedItem.Text != "-- Seleccione --" && chkNoEvaluados.Checked == false)//28
            {
                query = "sp_muestra_reportes_becarios -1,-1,'" + ddlPAcademico.SelectedItem.Text + "','" + ddlNivel.SelectedItem.Text + "','" + ddlevaluados.SelectedItem.Text + "',0";
            }
            if (ddlperiodo.SelectedValue == "-1" && ddlCampus.SelectedValue == "-1" && ddlPAcademico.SelectedItem.Text != "-- Seleccione --" && ddlNivel.SelectedItem.Text != "-- Seleccione --" && ddlevaluados.SelectedItem.Text == "-- Seleccione --" && chkNoEvaluados.Checked != false)//29
            {
                query = "sp_muestra_reportes_becarios -1,-1,'" + ddlPAcademico.SelectedItem.Text + "','" + ddlNivel.SelectedItem.Text + "',null,1";
            }
            if (ddlperiodo.SelectedValue != "-1" && ddlCampus.SelectedValue == "-1" && ddlPAcademico.SelectedItem.Text != "-- Seleccione --" && ddlNivel.SelectedItem.Text == "-- Seleccione --" && ddlevaluados.SelectedItem.Text != "-- Seleccione --" && chkNoEvaluados.Checked == false)//30
            {
                query = "sp_muestra_reportes_becarios " + ddlperiodo.SelectedValue + ",-1,'" + ddlPAcademico.SelectedItem.Text + "',null,'" + ddlevaluados.SelectedItem.Text + "',0";
            }
            if (ddlperiodo.SelectedValue == "-1" && ddlCampus.SelectedValue == "-1" && ddlPAcademico.SelectedItem.Text == "-- Seleccione --" && ddlNivel.SelectedItem.Text != "-- Seleccione --" && ddlevaluados.SelectedItem.Text != "-- Seleccione --" && chkNoEvaluados.Checked != false)//31
            {
                query = "sp_muestra_reportes_becarios -1,-1,null,'" + ddlNivel.SelectedItem.Text + "','" + ddlevaluados.SelectedItem.Text + "',1";
            }
            if (ddlperiodo.SelectedValue != "-1" && ddlCampus.SelectedValue != "-1" && ddlPAcademico.SelectedItem.Text != "-- Seleccione --" && ddlNivel.SelectedItem.Text != "-- Seleccione --" && ddlevaluados.SelectedItem.Text == "-- Seleccione --" && chkNoEvaluados.Checked == false)//32
            {
                query = "sp_muestra_reportes_becarios " + ddlperiodo.SelectedValue + "," + ddlCampus.SelectedValue + ",'" + ddlPAcademico.SelectedItem.Text + "','" + ddlNivel.SelectedItem.Text + "',null,0";
            }
            if (ddlperiodo.SelectedValue != "-1" && ddlCampus.SelectedValue != "-1" && ddlPAcademico.SelectedItem.Text != "-- Seleccione --" && ddlNivel.SelectedItem.Text == "-- Seleccione --" && ddlevaluados.SelectedItem.Text != "-- Seleccione --" && chkNoEvaluados.Checked == false)//33
            {
                query = "sp_muestra_reportes_becarios " + ddlperiodo.SelectedValue + "," + ddlCampus.SelectedValue + ",'" + ddlPAcademico.SelectedItem.Text + "',null,'" + ddlevaluados.SelectedItem.Text + "',0";
            }
            if (ddlperiodo.SelectedValue != "-1" && ddlCampus.SelectedValue != "-1" && ddlPAcademico.SelectedItem.Text != "-- Seleccione --" && ddlNivel.SelectedItem.Text == "-- Seleccione --" && ddlevaluados.SelectedItem.Text == "-- Seleccione --" && chkNoEvaluados.Checked != false)//34
            {
                query = "sp_muestra_reportes_becarios " + ddlperiodo.SelectedValue + "," + ddlCampus.SelectedValue + ",'" + ddlPAcademico.SelectedItem.Text + "',null,null,1";
            }
            if (ddlperiodo.SelectedValue == "-1" && ddlCampus.SelectedValue != "-1" && ddlPAcademico.SelectedItem.Text != "-- Seleccione --" && ddlNivel.SelectedItem.Text != "-- Seleccione --" && ddlevaluados.SelectedItem.Text != "-- Seleccione --" && chkNoEvaluados.Checked == false)//35
            {
                query = "sp_muestra_reportes_becarios -1," + ddlCampus.SelectedValue + ",'" + ddlPAcademico.SelectedItem.Text + "','" + ddlNivel.SelectedItem.Text + "','" + ddlevaluados.SelectedItem.Text + "',0";
            }
            if (ddlperiodo.SelectedValue == "-1" && ddlCampus.SelectedValue != "-1" && ddlPAcademico.SelectedItem.Text != "-- Seleccione --" && ddlNivel.SelectedItem.Text != "-- Seleccione --" && ddlevaluados.SelectedItem.Text == "-- Seleccione --" && chkNoEvaluados.Checked != false)//36
            {
                query = "sp_muestra_reportes_becarios -1," + ddlCampus.SelectedValue + ",'" + ddlPAcademico.SelectedItem.Text + "','" + ddlNivel.SelectedItem.Text + "',null,1";
            }
            if (ddlperiodo.SelectedValue == "-1" && ddlCampus.SelectedValue != "-1" && ddlPAcademico.SelectedItem.Text == "-- Seleccione --" && ddlNivel.SelectedItem.Text != "-- Seleccione --" && ddlevaluados.SelectedItem.Text != "-- Seleccione --" && chkNoEvaluados.Checked != false)//37
            {
                query = "sp_muestra_reportes_becarios -1," + ddlCampus.SelectedValue + ",null,'" + ddlNivel.SelectedItem.Text + "','" + ddlevaluados.SelectedItem.Text + "',1";
            }
            if (ddlperiodo.SelectedValue == "-1" && ddlCampus.SelectedValue == "-1" && ddlPAcademico.SelectedItem.Text == "-- Seleccione --" && ddlNivel.SelectedItem.Text == "-- Seleccione --" && ddlevaluados.SelectedItem.Text == "-- Seleccione --" && chkNoEvaluados.Checked == false)//38
            {
                query = "sp_muestra_reportes_becarios -1,-1,null,null,null,0";
            }
            if (ddlperiodo.SelectedValue != "-1" && ddlCampus.SelectedValue != "-1" && ddlPAcademico.SelectedItem.Text == "-- Seleccione --" && ddlNivel.SelectedItem.Text != "-- Seleccione --" && ddlevaluados.SelectedItem.Text != "-- Seleccione --" && chkNoEvaluados.Checked == false)//39
            {
                query = "sp_muestra_reportes_becarios " + ddlperiodo.SelectedValue + "," + ddlCampus.SelectedValue + ",null,'" + ddlNivel.SelectedItem.Text + "','" + ddlevaluados.SelectedItem.Text + "',0";
            }
            if (ddlperiodo.SelectedValue != "-1" && ddlCampus.SelectedValue != "-1" && ddlPAcademico.SelectedItem.Text != "-- Seleccione --" && ddlNivel.SelectedItem.Text != "-- Seleccione --" && ddlevaluados.SelectedItem.Text != "-- Seleccione --" && chkNoEvaluados.Checked == false)//40
            {
                query = "sp_muestra_reportes_becarios " + ddlperiodo.SelectedValue + "," + ddlCampus.SelectedValue + ",'" + ddlPAcademico.SelectedItem.Text + "','" + ddlNivel.SelectedItem.Text + "','" + ddlevaluados.SelectedItem.Text + "',0";
            }
            if (ddlperiodo.SelectedValue == "-1" && ddlCampus.SelectedValue != "-1" && ddlPAcademico.SelectedItem.Text != "-- Seleccione --" && ddlNivel.SelectedItem.Text != "-- Seleccione --" && ddlevaluados.SelectedItem.Text != "-- Seleccione --" && chkNoEvaluados.Checked != false)//41
            {
                query = "sp_muestra_reportes_becarios -1," + ddlCampus.SelectedValue + ",'" + ddlPAcademico.SelectedItem.Text + "','" + ddlNivel.SelectedItem.Text + "','" + ddlevaluados.SelectedItem.Text + "',1";
            }
            if (ddlperiodo.SelectedValue != "-1" && ddlCampus.SelectedValue == "-1" && ddlPAcademico.SelectedItem.Text != "-- Seleccione --" && ddlNivel.SelectedItem.Text != "-- Seleccione --" && ddlevaluados.SelectedItem.Text != "-- Seleccione --" && chkNoEvaluados.Checked != false)//42
            {
                query = "sp_muestra_reportes_becarios " + ddlperiodo.SelectedValue + ",-1,'" + ddlPAcademico.SelectedItem.Text + "','" + ddlNivel.SelectedItem.Text + "','" + ddlevaluados.SelectedItem.Text + "',1";
            }
            if (ddlperiodo.SelectedValue != "-1" && ddlCampus.SelectedValue != "-1" && ddlPAcademico.SelectedItem.Text != "-- Seleccione --" && ddlNivel.SelectedItem.Text == "-- Seleccione --" && ddlevaluados.SelectedItem.Text != "-- Seleccione --" && chkNoEvaluados.Checked != false)//43
            {
                query = "sp_muestra_reportes_becarios " + ddlperiodo.SelectedValue + "," + ddlCampus.SelectedValue + ",'" + ddlPAcademico.SelectedItem.Text + "',null,'" + ddlevaluados.SelectedItem.Text + "',1";
            }
            if (ddlperiodo.SelectedValue != "-1" && ddlCampus.SelectedValue != "-1" && ddlPAcademico.SelectedItem.Text != "-- Seleccione --" && ddlNivel.SelectedItem.Text != "-- Seleccione --" && ddlevaluados.SelectedItem.Text == "-- Seleccione --" && chkNoEvaluados.Checked != false)//44
            {
                query = "sp_muestra_reportes_becarios " + ddlperiodo.SelectedValue + "," + ddlCampus.SelectedValue + ",'" + ddlPAcademico.SelectedItem.Text + "','" + ddlNivel.SelectedItem.Text + "',null,1";
            }
            if (ddlperiodo.SelectedValue != "-1" && ddlCampus.SelectedValue != "-1" && ddlPAcademico.SelectedItem.Text != "-- Seleccione --" && ddlNivel.SelectedItem.Text != "-- Seleccione --" && ddlevaluados.SelectedItem.Text != "-- Seleccione --" && chkNoEvaluados.Checked != false)//44
            {
                query = "sp_muestra_reportes_becarios " + ddlperiodo.SelectedValue + "," + ddlCampus.SelectedValue + ",'" + ddlPAcademico.SelectedItem.Text + "','" + ddlNivel.SelectedItem.Text + "','" + ddlevaluados.SelectedItem.Text + "',1";
            }*/
            query = query.Replace("\r\n ", " ");
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                gvDatos.DataSource = dt;
                gvDatos.DataBind();
            }
            else
            {
                gvDatos.DataSource = null;
                gvDatos.DataBind();
            }
            ViewState["dt"] = dt;
        }

        protected void Unnamed1_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                dt = (DataTable)ViewState["dt"];//Sacamos los dato del datable                
                descargarInfo(dt);//Mandamos la informacion a descargar.             
            }
            catch (Exception es)
            {
                string error = es.Message.ToString();
                verModal("Error", es.Message.ToString());
            }
        }
        public void descargarInfo(DataTable ds)
        {
            if (ds != null)
            {
                string html = "", columas = "", registros = "";
                string attachment = "attachment; filename=ReporteBecarios.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";
                Response.Charset = "UTF-8";
                string tab = "";
                html = @"<table>                             
                          <tr>
                                <td colspan='6' style='text-align:center;font-size:20px;color:#113FB9'>
                                    REPORTE DE BECARIOS 
                                </td>    
                          <tr>";
                foreach (DataColumn dc in ds.Columns)
                {

                    columas += "<th>" + dc.ColumnName + "</th>";
                }
                html += columas + @"</tr>
                        ";

                int i;

                foreach (DataRow dr in ds.Rows)
                {
                    tab = "";
                    registros += "<tr>";
                    for (i = 0; i < ds.Columns.Count; i++)
                    {
                        registros += "<td>" + dr[i].ToString() + "</td>";
                    }
                    registros += "</tr>";
                }
                html += registros + "</table>";
                Response.Write(html);
                Response.End();

            }
            else
            {
                verModal("Alerta", "No hay información para descargar");
            }
        }


        public void descargaArchivo(DataTable ds)
        {
            if (ds != null)
            {
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=\"Demo.xls\"");
                Response.Charset = "UTF-8";
                Response.ContentEncoding = Encoding.Default;
                string tab = "";
                foreach (DataColumn dc in ds.Columns)
                {
                    Response.Write(tab + dc.ColumnName);
                    tab = "\t";
                }
                Response.Write("\n");
                int i;
                foreach (DataRow dr in ds.Rows)
                {
                    tab = "";
                    for (i = 0; i < ds.Columns.Count; i++)
                    {
                        Response.Write(tab + dr[i].ToString());
                        tab = "\t";
                    }
                    Response.Write("\n");
                }
                Response.End();
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDatos.PageIndex = e.NewPageIndex;
                filtrarDatos();
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }

        protected void ddlevaluados_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlevaluados.SelectedItem.Text != "-- Seleccione --")
                {
                    chkNoEvaluados.Checked = false;
                    chkNoEvaluados.Visible = false;
                }
                else
                {
                    
                    chkNoEvaluados.Visible = true;
                }
            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }

        protected void chkNoEvaluados_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkNoEvaluados.Checked == true)
                {
                    
                    ddlevaluados.SelectedValue = "-1";
                    pnlEvaluados.Visible = false;

                }
                else
                {
                    pnlEvaluados.Visible = true;
                }
            }
            catch (Exception es)
            {
                verModal("Error",es.Message.ToString());
            }
        }

    }
}