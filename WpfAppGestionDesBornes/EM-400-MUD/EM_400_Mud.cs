using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppGestionDesBornes.EM_400_MUD
{
    internal class EM_400_Mud 
    {
        public End_device_ids end_device_ids {  get; set; }
        public string Received_at { get; set; }
        public Uplink_message Uplink_Message { get; set; }
        public Settings Settings { get; set; }

        public EM_400_Mud() { }
    }
}