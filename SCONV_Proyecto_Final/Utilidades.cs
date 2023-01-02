namespace SCONV_Proyecto_Final
{
    public class Utilidades
    {
        public static int etapaInteraccion = 0;

        public static void IncrementarEtapaInteraccion()
        {
            etapaInteraccion++;
            Console.WriteLine(etapaInteraccion);
        }
    }
}
