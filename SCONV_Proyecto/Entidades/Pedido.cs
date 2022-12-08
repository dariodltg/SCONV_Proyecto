namespace SCONV_Proyecto.Entidades
{
    public class Pedido
    {
        public uint Id { get; set; }
        public List<Plato> PlatosPedidos { get; set; }

        /// <summary>
        /// Devuelve una cadena que representa el pedido completo
        /// </summary>
        /// <returns>La información del pedido </returns>
        public string ResumirPedido()
        {
            string pedido = "";
            foreach(Plato plato in PlatosPedidos)
            {
                pedido = pedido+ ", " + plato.ToString();
            }
            return pedido;
        }

        /// <summary>
        /// Devuelve el precio total del pedido, que es la suma del precio de todos los platos añadidos
        /// </summary>
        /// <returns>El precio total del pedido</returns>
        public double GetPrecioTotal()
        {
            return PlatosPedidos.Select(x => x.Precio).Sum();
        }
    }
}
