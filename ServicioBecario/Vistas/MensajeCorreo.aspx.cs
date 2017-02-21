using ServicioBecario.Codigo;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;

namespace ServicioBecario.Vistas
{
    public partial class MensajeCorreo : System.Web.UI.Page
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
                    llenarCampus();
                    llenarTipoDeCorreo();
                    validarTipoUsuario();
                    //Validar();

                }
                txtCuerpoCorreo.Text = Server.HtmlDecode(txtCuerpoCorreo.Text);
            }
            catch (Exception es)
            {
                verModal("Error", es.Message.ToString());
            }
        }
        public void deshabilitarCampusCliente()
        {
            ClientScript.RegisterStartupScript(GetType(), "Mostrar", "desactivarComponente();", true);
        }

        public void validarTipoUsuario()
        {
            query = "exec	sp_muestra_Rol_al_usuario '" + Session["Usuario"].ToString() + "'";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["id_rol"].ToString() != "4")//Este rol es el de multicampus
                {
                    ddlCampus.SelectedValue = ddlCampus.Items.FindByText(dt.Rows[0]["Campus"].ToString()).Value;
                    hdfid_campus.Value = ddlCampus.SelectedValue;
                    ddlCampus.Visible = false;// desactiva el campus
                    lblCampus.Visible = true;
                    lblCampus.Text = dt.Rows[0]["Campus"].ToString();
                    PnlTipoCorreo.Visible = true;
                }
            }
            else
            {
                verModal("Error", "El usuario " + Session["Usuario"].ToString() + " no tiene rol asignado ");
            }
        }
        public void Validar()
        {
            ClientScript.RegisterStartupScript(GetType(), "Mostrar", "cambioDeItem();", true);
        }
        public void llenarCampus()
        {
            query = "select Codigo_campus,Nombre as Campus from cat_campus  order by nombre asc";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlCampus.DataTextField = "Campus";
                ddlCampus.DataValueField = "Codigo_campus";
                ddlCampus.DataSource = dt;
                ddlCampus.DataBind();
            }
            else
            {
                verModal("Alerta", "No existen campus Registrados");
            }
        }
        public void verModal(string header, string body)
        {
            lblCabeza.Text = header;
            lblcuerpo.Text = body;
            mp1.Show();
        }
        protected void ddlCampus_DataBound(object sender, EventArgs e)
        {
            ddlCampus.Items.Insert(0, new ListItem("--Seleccione --", "0"));
        }

        public void llenarTipoDeCorreo()
        {
            query = "select id_correo,Tipo_correo from cat_correos";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                ddlTipoCorreo.DataTextField = "Tipo_correo";
                ddlTipoCorreo.DataValueField = "id_correo";
                ddlTipoCorreo.DataSource = dt;
                ddlTipoCorreo.DataBind();
            }
        }

        protected void ddlTipoCorreo_DataBound(object sender, EventArgs e)
        {
            ddlTipoCorreo.Items.Insert(0, new ListItem("--Seleccione --", "0"));
        }

        protected void ddlTipoCorreo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTipoCorreo.SelectedValue != "0")
            {
                try
                {
                    hdfid_campus.Value = ddlCampus.SelectedValue;
                    sacarCampos();//sacamos la informacion de de los campos
                    sacarRegistrosDeCorreos();
                    PnlCampos.Visible = true;
                    PnlCorreo.Visible = true;
                    Literal1.Visible = true;
                }
                catch (Exception es)
                {
                    verModal("Error", es.Message.ToString());
                }
            }
            else
            {
                PnlCampos.Visible = false;
                PnlCorreo.Visible = false;
                Literal1.Visible = false;
            }
        }

        public void sacarRegistrosDeCorreos()
        {
            query = "sp_Mostrar_cuerpo_correo_campus " + ddlTipoCorreo.SelectedValue + "," + hdfid_campus.Value + "";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                //string htm = Server.HtmlEncode(dt.Rows[0]["Cuerpo"].ToString());
                txtCuerpoCorreo.Text = "";
                txtCuerpoCorreo.Text = Server.HtmlDecode(dt.Rows[0]["Cuerpo"].ToString());
                txtAsunto.Text = "";
                txtAsunto.Text = dt.Rows[0]["Asunto"].ToString();
            }
            else
            {
                txtCuerpoCorreo.Text = "Escribe el cuerpo de correo";
            }

        }
        public void sacarCampos()
        {
            string html = "";
            int i = 1,r=0;
            query = "select Campo,Titulo_completo from cat_campos where id_correo=" + ddlTipoCorreo.SelectedValue;
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                html = "<div id='sort1' class='con'>";
                foreach (DataRow row in dt.Rows)
                {
                  /*  if (i % 2 == 0)
                    {
                        html = html + "<button id=\"btn" + i + "\" class=\"bontoncito btn btn-info\"  onclick=\"elemento('" + row[0].ToString() + "');\" name='" + row[0].ToString() + "' title=\"" + dt.Rows[r]["Titulo_completo"].ToString() + "\"  >" + row[0].ToString() + "</button><br/>";
                    }
                    else
                    {
                        html = html + "<button id=\"btn" + i + "\" class=\"bontoncito btn btn-info\"  onclick=\"elemento('" + row[0].ToString() + "');\" name='" + row[0].ToString() + "'  title=\"" + dt.Rows[r]["Titulo_completo"].ToString() + "\"  >" + row[0].ToString() + "</button>";
                    }*/

                    //html = html + "<button id=\"btn" + i + "\" type=\"submit\" runat=\"server\" OnServerClick=\"MyButton_Click\" >" + row[0].ToString() + "</button><br/>";
                    html +="<div id=\"btn" + i + "\" class=\"btn btn-primary btn-sm\" style='width:180px;padding:5px;margin:3px;border-radius:0px;z-index:3001'   title=\"" + dt.Rows[r]["Titulo_completo"].ToString() + "\"  >" + row[0].ToString() + "</div>";
                    
                    i++;
                    r++;
                }
                Literal1.Text = html+"</div>";

            }
            else
            {
                Literal1.Text = "";
                verModal("Alerta", "No existen campos para este tipo de correo");
            }
        }

        protected void btnGuardarMensaje_Click(object sender, EventArgs e)
        {
            query = "sp_guardar_mensaje_correo " + hdfid_campus.Value + "," + ddlTipoCorreo.SelectedValue + ",'" + txtAsunto.Text.Trim() + "','" + txtCuerpoCorreo.Text.Trim() + "' ";
            dt = db.getQuery(conexionBecarios, query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Mensaje"].ToString() == "Ok")
                {
                    verModal("Éxito", "Registro editado con éxito");
                }
            }
            else
            {
                txtCuerpoCorreo.Text = "Escribe el cuerpo de correo";
            }
        }


        protected void ddlCampus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCampus.SelectedValue != "")
            {
                PnlTipoCorreo.Visible = true;

            }
            hdfid_campus.Value = ddlCampus.SelectedValue;
        }

      
    }
}