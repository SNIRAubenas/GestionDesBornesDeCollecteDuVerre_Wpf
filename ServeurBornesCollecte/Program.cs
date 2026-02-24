using ServeurBornesCollecte.Core;

namespace ServeurBornesCollecte
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MesureService service = new MesureService();
            service.AfficherMesures();

            Console.ReadKey();
        }
    }
}