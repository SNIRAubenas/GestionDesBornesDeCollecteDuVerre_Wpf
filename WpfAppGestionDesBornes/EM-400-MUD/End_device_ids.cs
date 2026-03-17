using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppGestionDesBornes.EM_400_MUD
{
    internal class End_device_ids
    {
        // Identifiant du device dans TTN
        public string Device_id { get; set; }
        // Identifiant unique matériel
        public string Dev_eui { get; set; }
        // Identifiant de jointure réseau
        public string Join_eui { get; set; }
        

    }
}