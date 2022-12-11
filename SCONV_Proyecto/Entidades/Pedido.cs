using System.Diagnostics;

namespace SCONV_Proyecto.Entidades
{
    public class Pedido
    {
        public uint Id { get; set; }
        public List<Plato> PlatosPedidos { get; set; }

        public static Pedido pedidoEnCurso = new Pedido
        {
            Id = 0,
            PlatosPedidos = new List<Plato>()
        };
        public static Establecimiento establecimientoEnCurso;

        /// <summary>
        /// Devuelve una cadena que representa el pedido completo, indicando además su precio total.
        /// </summary>
        /// <returns>La información del pedido </returns>
        public string ResumirPedido()
        {
            int i = 1;
            int numPlatos = PlatosPedidos.Count();
            string frase = "El pedido a "+establecimientoEnCurso+" contiene los siguientes platos: ";
            if(numPlatos > 1) { 
                foreach (Plato plato in PlatosPedidos)
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
            }
            else if(numPlatos == 1)
            {
                frase = frase + PlatosPedidos.First().ToString() +".";
            }
            else
            {
                frase = frase + " No hay ningún plato en el pedido todavía.";
            }
            frase = frase + " El coste total es de " + GetPrecioTotal() + " euros. ¿Desea confirmar el pedido?";
            return frase;
        }

        /// <summary>
        /// Devuelve el precio total del pedido, que es la suma del precio de todos los platos añadidos
        /// </summary>
        /// <returns>El precio total del pedido</returns>
        public double GetPrecioTotal()
        {
            return PlatosPedidos.Select(x => x.Precio).Sum();
        }

        public static void AddPlatoAPedidoEnCurso(Plato plato)
        {
            pedidoEnCurso.PlatosPedidos.Add(plato);
        }
    }
}
