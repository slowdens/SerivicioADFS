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
    public class Datum
    {
        public string name { get; set; }
        public string value { get; set; }
        public string prompt { get; set; }
    }
    public class Template
    {
        public List<Datum> data { get; set; }
    }
    public class Collection
    {
        public string version { get; set; }
        public string href { get; set; }
        public Template template { get; set; }
    }

    public class RootObject
    {
        public Collection collection { get; set; }
    }
    public class DatumResponse
    {
        public string name { get; set; }
        public object value { get; set; }
    }
    public class ItemResponse
    {
        public List<DatumResponse> data { get; set; }
    }
    public class CollectionResponse
    {
        public string version { get; set; }
        public string href { get; set; }
        public List<ItemResponse> items { get; set; }
    }

    public class RootObjectResponse
    {
        public CollectionResponse collection { get; set; }
    }
    public class Error
    {
        public string title { get; set; }
        public string code { get; set; }
        public string message { get; set; }
    }

    public class CollectionError
    {
        public string version { get; set; }
        public string href { get; set; }
        public Error error { get; set; }
    }

    public class RootObjectError
    {
        public CollectionError collection { get; set; }
    }

    public class SafePut
    {
        private string usuario = "sSISTEMA-ESCOLAR";
        private string password = "G=v&5Gn,";
        public string escribirCalificacion(string codigoPeriodo,string folio,string cumple, string usuarioSB,string matricula)
        {
            string valor="";
            usuarioSB = usuarioSB + "_SB";
            RootObject JsonEnvio = new RootObject();
            JsonEnvio.collection = new Collection();
            JsonEnvio.collection.template = new Template();
            JsonEnvio.collection.template.data = new List<Datum>();

            //Aqui ira en foreach o algo asi para agregar todos los elementos que se van a enviar ya 
            //que pueden ser muchos supongo una consulta a bd o algo asi
            JsonEnvio.collection.template.data.Add(new Datum() { name = "periodo", value = codigoPeriodo, prompt = "Periodo activo del Servicio Becario" });
            JsonEnvio.collection.template.data.Add(new Datum() { name = "folio", value = folio, prompt = "Folio de Asignación para Servicio Becario" });
            JsonEnvio.collection.template.data.Add(new Datum() { name = "cumple", value = cumple, prompt = "Si cumple con el Servicios Becario." });
            JsonEnvio.collection.template.data.Add(new Datum() { name = "usuario", value = usuarioSB, prompt = "Quien es el que actualiza la información" });

            //Version y href
            JsonEnvio.collection.version = "1.0";
            JsonEnvio.collection.href = "https://esbsvrqa01.itesm.mx:8081/TMTY/alumnos/*/becas/serviciobecario";

            //conversion de mi objeto a json
            var jenviar = new JavaScriptSerializer().Serialize(JsonEnvio);
            //lo imprimo para ver que tenga la estructura que debe terner
            string respuesta="";

            respuesta += jenviar;
            respuesta += "<br><br>";

            var url = "https://esbsvrqa01.itesm.mx:8081/TMTY/alumnos/"+matricula+"/becas/serviciobecario";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            string authInfo = this.usuario+":"+this.password;
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers.Add("Authorization", "Basic " + authInfo);
            request.Credentials = new NetworkCredential(this.usuario, this.password);
            request.Accept = "application/json";
            request.Method = "PUT";
            using (StreamWriter enviarjson = new StreamWriter(request.GetRequestStream()))
            {
                enviarjson.Write(jenviar);
                enviarjson.Flush();
                enviarjson.Close();
            }
            request.AllowAutoRedirect = true;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader streamreader = new StreamReader(response.GetResponseStream());
            
            var json = streamreader.ReadToEnd();
            var deserialize = new JavaScriptSerializer();
            var ObJSONErr = deserialize.Deserialize<RootObjectError>(json);
            if (ObJSONErr.collection.error == null)
            {
                RootObjectResponse ObJSON = deserialize.Deserialize<RootObjectResponse>(json);
                valor += "";
                valor += "collection: <br> ";
                valor += "version: " + ObJSON.collection.version.ToString() + " <br> ";
                valor += "href: " + ObJSON.collection.href.ToString() + " <br><br> ";
        
                for (int x = 0; x < ObJSON.collection.items[0].data.Count(); x++)
                {
                    if (ObJSON.collection.items[0].data[x].name.ToString() == "Mensaje")
                    {
                        valor += "!Nombre!" + ObJSON.collection.items[0].data[x].name.ToString();
                        valor += "!Value!" + ObJSON.collection.items[0].data[x].value.ToString()+"!";
                        valor += "</br>";
                    }

                    valor += "Nombre: " + ObJSON.collection.items[0].data[x].name.ToString() + " <br>";
                    valor += "Value: " + ObJSON.collection.items[0].data[x].value.ToString() + " <br><br>";
                    
                }
           }
           else
           {
                    //muestro el error en caso de que la respuesta del server sea error
                   valor += ObJSONErr.collection.error.message;
           }

            

            return valor;
        }
    }
    
}