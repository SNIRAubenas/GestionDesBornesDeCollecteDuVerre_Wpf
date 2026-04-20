using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppGestionDesBornes.EM_400_MUD
{
    internal class Uplink_message
    {
        private string frm_payload;
        // Payload encodé en Base64 envoyé par le capteur
        public string Frm_payload { 
            get
            {
                return frm_payload;
            } 
            set
            {
                frm_payload = value;
                // Conversion Base64 → tableau d'octets
                byte[] bytes = Convert.FromBase64String(value);
                //AXVhA2fPAASCdQAFAAE=
                byte [] test = Convert.FromBase64String("AXVhA2fPAASCdQAFAAE=");
                // Objet qui contiendra les données décodées
                Decoded_payload decoded = new Decoded_payload();
                // Décodage du protocole propriétaire du capteur
                for (var i = 0; i < bytes.Length;)
                {
                    var channel_id = bytes[i++];
                    var channel_type = bytes[i++];
                    // BATTERY
                    if (channel_id == 0x01 && channel_type == 0x75)
                    {
                        decoded.Battery = bytes[i];
                        i += 1;
                    }
                    // TEMPERATURE
                    else if (channel_id == 0x03 && channel_type == 0x67)
                    {
                        //decoded.Temperature = readInt16LE(bytes.slice(i, i + 2)) / 10;
                        decoded.Temperature = bytes[i] / 10;
                        i += 2;
                    }
                    // DISTANCE
                    else if (channel_id == 0x04 && channel_type == 0x82)
                    {
                        decoded.Distance = bytes[i];
                        //decoded.Distance = readUInt16LE(bytes.slice(i, i + 2));
                        i += 2;
                    }
                    // POSITION
                    else if (channel_id == 0x05 && channel_type == 0x00)
                    {
                        decoded.Position = bytes[i] == 0 ? "normal" : "tilt";
                        i += 1;
                    }
                    // TEMPERATURE WITH ABNORMAL
                    else if (channel_id == 0x83 && channel_type == 0x67)
                    {
                        decoded.Temperature = bytes[i] / 10;
                        //decoded.Temperature = readInt16LE(bytes.slice(i, i + 2)) / 10;
                        //decoded.temperature_abnormal = bytes[i + 2] == 0 ? false : true;
                        i += 3;
                    }
                    // DISTANCE WITH ALARMING
                    else if (channel_id == 0x84 && channel_type == 0x82)
                    {
                        decoded.Distance = bytes[i];
                        //decoded.Distance = readUInt16LE(bytes.slice(i, i + 2));
                        //decoded.distance_alarming = bytes[i + 2] == 0 ? false : true;
                        i += 3;
                    }
                    else
                    {
                        break;
                    }
                }
                // Sauvegarde du résultat décodé
                Decoded_payload = decoded;
            }
        }
        // Données lisibles après décodage
        public Decoded_payload Decoded_payload { get; set; }
        // public Rx_metadata Rx_metadata { get; set; }
        // Paramètres du message
        public Settings Settings {  get; set; }
    }
}