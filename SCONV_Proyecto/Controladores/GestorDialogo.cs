using Google.Cloud.Dialogflow.V2;
using SCONV_Proyecto.Entidades;

namespace SCONV_Proyecto.Controladores
{
    public class GestorDialogo
    {
        public static WebhookResponse GenerarRespuesta(WebhookRequest peticion)
        {
            WebhookResponse respuesta = new WebhookResponse();
            //en funcion del intent y del contexto hacemos cosas
            switch (peticion.QueryResult.Intent.DisplayName)
            {
                case "pedirComida.inicio":
                    respuesta.FulfillmentText = EnumerarEstablecimientos(peticion.QueryResult.FulfillmentText);
                    break;
                case "pedirComida.elegirEstablecimiento":
                    respuesta.FulfillmentText = EnumerarPlatosDeEstablecimiento(peticion);
                    break;
                case "pedirComida.elegirPlatos":
                    respuesta.FulfillmentText = AddPlatoAPedido(peticion);
                    break;
                default: 
                    break;
            }
            return respuesta;
        }

        /// <summary>
        /// Devuelve una frase con los establecimientos disponibles para pedir
        /// </summary>
        /// <param name="fraseInicial">Frase inicial a parti de la cual se continúa construyendo la frase completa</param>
        /// <returns>La frase completa de respuesta</returns>
        public static string EnumerarEstablecimientos(string fraseInicial)
        {
            string frase = fraseInicial + "Las opciones son: ";
            int i = 1;
            List<Establecimiento> establecimientos = FachadaBbdd.GetSingleton().GetEstablecimientos();
            int numEstablecimientos = establecimientos.Count;
            foreach (Establecimiento establecimiento in establecimientos)
            {
                if (i < numEstablecimientos)
                {
                    frase = frase + establecimiento.ToString();
                    if(i < numEstablecimientos - 1)
                    {
                        frase = frase + ", "; 
                    }
                }
                else
                {
                    frase = frase + " y " + establecimiento.ToString() + ".";
                }
                i++;
            }
            return frase; 
        }

        /// <summary>
        /// Devuelve una frase con los platos disponibles para un establecimiento elegido
        /// </summary>
        /// <param name="peticion">La peticion que contiene el restaurante elegido</param>
        /// <returns>La frase completa de respuesta</returns>
        public static string EnumerarPlatosDeEstablecimiento(WebhookRequest peticion)
        {
            string nombreEstablecimiento = peticion.QueryResult.Parameters.Fields.GetValueOrDefault("Establecimiento").StringValue;
            Establecimiento establecimiento = FachadaBbdd.GetSingleton().GetEstablecimientoByNombre(nombreEstablecimiento);
            List<Plato> platosDeEstablecimiento = establecimiento.Platos;
            int i = 1;
            int numPlatos = platosDeEstablecimiento.Count();
            string frase = "Pidiendo a "+nombreEstablecimiento+". Las opciones disponibles son: ";
            foreach(Plato plato in platosDeEstablecimiento)
            {
                if(i < numPlatos)
                {
                    frase = frase + plato.ToString();
                    if(i < numPlatos - 1)
                    {
                        frase = frase + ", ";
                    }
                }
                else
                {
                    frase = frase + " y " + establecimiento.ToString() + ".";
                }
            }

            return frase;
        }

        /// <summary>
        /// Añade un plato al pedido actual
        /// </summary>
        /// <param name="peticion">La petición que contiene el plato elegido</param>
        /// <returns>La frase de respuesta</returns>
        public static string AddPlatoAPedido(WebhookRequest peticion)
        {
            string nombreEstablecimiento = Pedido.establecimientoEnCurso.Nombre;
            string nombrePlato = peticion.QueryResult.Parameters.Fields.GetValueOrDefault("Plato").StringValue;
            Plato plato = Pedido.establecimientoEnCurso.Platos.First(x => x.Nombre == nombrePlato);
            string frase = "Añadiendo al pedido: " +plato.Nombre+ "¿Quiere algo más?";
            Pedido.AddPlatoAPedidoEnCurso(plato);
            return frase;
        }






    }
}
