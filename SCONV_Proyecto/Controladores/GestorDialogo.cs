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
            Console.WriteLine(peticion.QueryResult.Intent.DisplayName);
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
                case "pedirComida.elegirPlatos.masPlatos":
                    respuesta.FulfillmentText = MasPlatos();
                    break;
                case "pedirComida.elegirPlatos.noMasPlatos":
                    respuesta.FulfillmentText = ResumirPedidoCompleto();
                    break;
                case "pedirComida.elegirPlatos.noMasPlatos.continuarPedido":
                    respuesta.FulfillmentText = MasPlatos();
                    break;
                case "pedirComida.elegirPlatos.noMasPlatos.confirmarPedido":
                    respuesta.FulfillmentText = ConfirmarPedido();
                    break;
                case "pedirComida.elegirPlatos.quitarUltimoPlato":
                    respuesta.FulfillmentText = QuitarUltimoPlato();
                    break;
                case "pedirComida.cancelarPedido":
                    respuesta.FulfillmentText = CancelarPedido();
                    break;
                case "pedirComida.cancelarPedido.confirmarCancelacion":
                    respuesta.FulfillmentText = ConfirmarCancelacionPedido();
                    break;
                case "pedirComida.cancelarPedido.cancelarCancelacion":
                    respuesta.FulfillmentText = CancelarCancelacionPedido();
                    break;

                default: 
                    break;
            }
            return respuesta;
        }

        /// <summary>
        /// Devuelve la frase para iniciar la cancelación del pedido
        /// </summary>
        /// <returns>la frase de respuesta</returns>
        private static string CancelarPedido()
        {
            string frase = "El pedido se va a cancelar. ¿Está seguro?";
            return frase;
        }

        /// <summary>
        /// Devuelve la frase para indicar que el pedido no ha sido cancelado.
        /// </summary>
        /// <returns>La frase de respuesta</returns>
        private static string CancelarCancelacionPedido()
        {
            string frase = "El pedido no se ha cancelado. ¿Qué desea?";
            return frase;
        }

        /// <summary>
        /// Devuelve la frase para indicar que el pedido se ha cancelado definitivamente.
        /// </summary>
        /// <returns>La frase de respuesta</returns>
        private static string ConfirmarCancelacionPedido()
        {
            string frase = "El pedido se ha cancelado.";
            Pedido.pedidoEnCurso.PlatosPedidos.Clear(); //Vaciamos los platos del pedido en curso porque se ha cancelado
            return frase;
        }

        

        /// <summary>
        /// Devuelve la frase que indica que se ha quitado el último plato del pedido.
        /// </summary>
        /// <returns>La frase de respuesta</returns>
        private static string QuitarUltimoPlato()
        {
            Plato ultimoPlato = Pedido.pedidoEnCurso.PlatosPedidos.Last();
            Pedido.pedidoEnCurso.PlatosPedidos.Remove(ultimoPlato);
            string frase = "Quitado plato "+ultimoPlato.Nombre +" del pedido. ¿Desea algo más?";
            return frase;
        }

        /// <summary>
        /// Devuelve una frase de confirmación del pedido
        /// </summary>
        /// <returns>La frase de respuesta</returns>
        private static string ConfirmarPedido()
        {
            string frase = "Pedido confirmado. Muchas gracias.";
            Pedido.pedidoEnCurso.PlatosPedidos.Clear(); //Vaciamos los platos del pedido en curso porque se ha confirmado
            return frase;
        }

        /// <summary>
        /// Devuelve una frase para preguntar si quiere más platos
        /// </summary>
        /// <returns>La frase de respuesta</returns>
        private static string MasPlatos()
        {
            string frase = "De acuerdo. ¿Qué más quiere pedir?";
            return frase;
        }

        /// <summary>
        /// Devuelve una frase para resumir el pedido completo que lleva el usuario hasta el momento
        /// </summary>
        /// <returns>La frase completa de respuesta</returns>
        private static string ResumirPedidoCompleto()
        {
            string frase = Pedido.pedidoEnCurso.ResumirPedido();

            return frase;
        }

        /// <summary>
        /// Devuelve una frase con los establecimientos disponibles para pedir
        /// </summary>
        /// <param name="fraseInicial">Frase inicial a parti de la cual se continúa construyendo la frase completa</param>
        /// <returns>La frase completa de respuesta</returns>
        public static string EnumerarEstablecimientos(string fraseInicial)
        {
            string frase = fraseInicial + " Las opciones son: ";
            int i = 1;
            List<Establecimiento> establecimientos = FachadaBbdd.GetSingleton().GetEstablecimientos();
            int numEstablecimientos = establecimientos.Count;
            if (numEstablecimientos > 1)
            {
                foreach (Establecimiento establecimiento in establecimientos)
                {
                    if (i < numEstablecimientos)
                    {
                        frase = frase + establecimiento.ToString();
                        if (i < numEstablecimientos - 1)
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
            }else if (numEstablecimientos == 1)
            {
                frase = frase + establecimientos.First().ToString() + ".";
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
            List<Plato> platosDeEstablecimiento = FachadaBbdd.GetSingleton().GetPlatosDeEstablecimiento(establecimiento);
            int i = 1;
            int numPlatos = platosDeEstablecimiento.Count();
            string frase = "Pidiendo a "+nombreEstablecimiento+". Las opciones disponibles son: ";
            if (numPlatos > 1)
            {
                foreach (Plato plato in platosDeEstablecimiento)
                {
                    if (i < numPlatos)
                    {
                        frase = frase + plato.ToString();
                        if (i < numPlatos - 1)
                        {
                            frase = frase + ", ";
                        }
                    }
                    else
                    {
                        frase = frase + " y " + plato.ToString() + ".";
                    }
                    i++;
                }
            }else if (numPlatos == 1)
            {
                frase = frase + platosDeEstablecimiento.First().ToString()+".";
            }
            else
            {
                frase = frase + "No hay opciones disponibles.";
            }
            frase = frase + " ¿Qué desea?";
            Pedido.establecimientoEnCurso = establecimiento; //Seteamos el establecimiento al que se está pidiendo
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
            string frase = "Añadiendo al pedido: " +plato.Nombre+ ". ¿Quiere algo más?";
            Pedido.AddPlatoAPedidoEnCurso(plato);
            return frase;
        }

    }
}
