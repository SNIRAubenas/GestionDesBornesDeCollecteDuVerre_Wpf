using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppGestionDesBornes.EM_400_MUD
{
    internal class EM_400_Mud 
    {
        // Informations sur le dispositif (identifiants)
        public End_device_ids end_device_ids {  get; set; }
        // Date de réception du message
        public string Received_at { get; set; }
        // Contenu du message uplink (données capteur)
        public Uplink_message Uplink_Message { get; set; }
        // Paramètres réseau
        public Settings Settings { get; set; }

        public EM_400_Mud() { }
    }
}