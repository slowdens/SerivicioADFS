using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace ServicioBecario.Codigo
{
    public class ElmData
    {
        public string name { get; set; }
        public object value { get; set; }
    }

    public class Items
    {
        public List<ElmData> data { get; set; }
    }
    public class Collections
    {
        public string version { get; set; }
        public string href { get; set; }
        public List<Items> items { get; set; }
    }

    public class myJson
    {
        public Collections collection { get; set; }
    }
   

    public class Safe
    {
        
        private string usuario = "sSISTEMA-ESCOLAR";
        private string password = "G=v&5Gn,";

        public string conocerResultadoRespuesta(string matricula,string codigoPeriodo, string folio)
        {
            var url = "https://esbsvrqa01.itesm.mx:8081/TMTY/alumnos/" + matricula + "/becas/serviciobecario?periodo=" + codigoPeriodo + "&folio=" + folio + "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            string authInfo = this.usuario+":"+this.password;
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers.Add("Authorization", "Basic " + authInfo);
            request.Credentials = new NetworkCredential(this.usuario, this.password);
            request.Method = WebRequestMethods.Http.Get;
            request.Accept = "application/json";
            request.AllowAutoRedirect = true;
            request.Proxy = null;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader streamreader = new StreamReader(response.GetResponseStream());
            var json = streamreader.ReadToEnd();
            JavaScriptSerializer deserialize = new JavaScriptSerializer();
            myJson ObJSON = deserialize.Deserialize<myJson>(json);
            string resultado = "";
            if(ObJSON !=null)
            {
                resultado += ObJSON.collection.href.ToString();
                resultado += ObJSON.collection.version;
                resultado += "collection: <br> ";
                resultado += "version: " + ObJSON.collection.version.ToString() + " <br> ";
                resultado += "href: " + ObJSON.collection.href.ToString() + " <br><br> ";
                for (int x = 0; x < ObJSON.collection.items[0].data.Count(); x++)
                {
                    resultado += "Nombre: " + ObJSON.collection.items[0].data[x].name.ToString() + " <br>";
                    resultado += "Value: " + ObJSON.collection.items[0].data[x].value.ToString() + " <br><br>";
                }
            }
            else
            {
                resultado = "Los datos son incorrectos";
            }
            
            return resultado;
        }

    }
}