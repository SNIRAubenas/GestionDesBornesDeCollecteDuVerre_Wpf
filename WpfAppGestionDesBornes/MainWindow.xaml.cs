using Newtonsoft.Json;
using System.Text;
using System.Windows;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using WpfAppGestionDesBornes.Core;
using WpfAppGestionDesBornes.EM_400_MUD;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace WpfAppGestionDesBornes
{
    public partial class MainWindow : Window
    {
        double lastDistance = 0;
        double lastTemperature = 0;
        double lastBattery = 0;

        public void StartApi()
        {
            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();

            app.MapGet("/api/data", () =>
            {
                return new
                {
                    distance = lastDistance,
                    temperature = lastTemperature,
                    battery = lastBattery
                };
            });

            app.RunAsync("http://localhost:5000");
        }

        MqttClient client;

        // Service de sauvegarde des données dans la BDD
        MesureService mesureService = new MesureService();

        public MainWindow()
        {
            InitializeComponent();
            StartApi();

            try
            {
                // Connexion au broker MQTT de The Things Network
                client = new MqttClient("eu1.cloud.thethings.network");

                // Événement déclenché lorsqu'un message est reçu
                client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

                // Identifiant unique du client MQTT
                var clientId = Guid.NewGuid().ToString();

                var subscriptionId = client.Subscribe(
                  new string[] { "#" },
                  new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

                // Gestion des événements de connexion
                client.ConnectionClosed += Client_ConnectionClosed;
                client.MqttMsgSubscribed += Client_MqttMsgSubscribed;

                // Connexion avec identifiants TTN
                var connectionId = client.Connect(clientId,
                    "em310@ttn",
                    "NNSXS.WBSTJ75NW476RQWXJHUQQVZFOQ3FQR5NYCB5VKI.RWOQVMR4BUOJFHPNE2ECDDFSCDWGE45LUTGAYHDOOZ5FYHAZERQQ");

                if (connectionId == 0)
                {
                    Console.WriteLine("connected");
                    lblConnected.Content = "Connected";
                }
                else
                {
                    Console.WriteLine("Not CONNECTED");
                    lblConnected.Content = "NOT Connected";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur message = " + ex.Message);
                lblError.Content = "Error " + ex.Message;
            }
        }

        string jsonText;
        EM_400_Mud EM_400_Mud_Class;

        // Méthode appelée lorsqu'un message MQTT est reçu
        void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            // Conversion du message binaire en texte JSON
            jsonText = Encoding.ASCII.GetString(e.Message);

            // Désérialisation JSON → objet C#
            EM_400_Mud_Class = JsonConvert.DeserializeObject<EM_400_Mud>(jsonText);

            // Vérification que les données existent
            if (EM_400_Mud_Class?.Uplink_Message?.Decoded_payload != null)
            {
                int distance = (int)EM_400_Mud_Class.Uplink_Message.Decoded_payload.Distance;

                lastDistance = EM_400_Mud_Class.Uplink_Message.Decoded_payload.Distance;
                lastTemperature = EM_400_Mud_Class.Uplink_Message.Decoded_payload.Temperature;
                lastBattery = EM_400_Mud_Class.Uplink_Message.Decoded_payload.Battery;

                // Enregistrer dans la BDD
                mesureService.SaveMesure(distance);

                // Mise à jour de l'interface (thread UI)
                Dispatcher.Invoke(() =>
                {
                    lblConnected.Content = EM_400_Mud_Class.end_device_ids.Device_id;
                    lblPayload.Content = EM_400_Mud_Class.Uplink_Message.Frm_payload;
                    lblTemperature.Content = EM_400_Mud_Class.Uplink_Message.Decoded_payload.Temperature;
                    lblBattery.Content = EM_400_Mud_Class.Uplink_Message.Decoded_payload.Battery;
                    lblDistance.Content = EM_400_Mud_Class.Uplink_Message.Decoded_payload.Distance;
                });
            }
        }

        void Client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            Console.WriteLine("Client_MqttMsgSubscribed:" + e.ToString());
        }

        void Client_ConnectionClosed(object sender, EventArgs e)
        {
            Console.WriteLine("Client_ConnectionClosed: " + e.ToString());
        }

        // Fermeture propre de la connexion MQTT
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.client.IsConnected)
            {
                client.Disconnect();
            }
        }
    }
}