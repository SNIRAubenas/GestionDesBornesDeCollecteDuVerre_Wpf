using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppGestionDesBornes.EM_400_MUD
{
    // Contient les données décodées envoyées par le capteur
    internal class Decoded_payload
    {
        public double Battery {  get; set; }
        public double Distance { get; set; }
        public string Position { get; set; }
        public double Temperature { get; set; }


    }
}