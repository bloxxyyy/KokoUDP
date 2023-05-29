using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Koko_ClientServer;
public class KokoClient {

    public Action<List<NetworkComponent>>? HandleReceivedObjects { get; set; }

    public void StartClient() {
        IPAddress clientIp = IPAddress.Parse("127.0.0.1");
        int clientPort = 4041;
        IPEndPoint ipep = new(clientIp, clientPort);
        UdpClient _UpdClient = new(ipep);
        
        
        IPEndPoint server = new IPEndPoint(KokoServer.SERVERIP, KokoServer.SERVERPORT);
        SendData(server, _UpdClient, "Hello, I am new!");

        while (true) {
            if (_UpdClient.Available > 0) {
                ReceiveData(server, _UpdClient);
            }
        }
    }

    private void SendData(IPEndPoint sender, UdpClient me, string toSend) {
        var data = Encoding.ASCII.GetBytes(toSend);
        me.Send(data, data.Length, sender);
    }

    private void ReceiveData(IPEndPoint sender, UdpClient me) {
        var data = me.Receive(ref sender);
        string receivedMessage = Encoding.UTF8.GetString(data, 0, data.Length);
        Console.WriteLine("Received: " + receivedMessage);

        List<NetworkComponent> receivedObjects = DeserializeObjects(receivedMessage);
        HandleReceivedObjects?.Invoke(receivedObjects);
    }

    private List<NetworkComponent> DeserializeObjects(string json) {
        List<NetworkComponent> objects = new List<NetworkComponent>();
        try {
            JArray jsonArray = JArray.Parse(json);
            foreach (JObject jsonObject in jsonArray) {
                Type objectType = Type.GetType((string)jsonObject["type"]);
                if (objectType != null) {
                    NetworkComponent obj = (NetworkComponent)jsonObject.ToObject(objectType);
                    objects.Add(obj);
                }
            }
        } catch (Exception e) {
            Console.WriteLine(e);
        }

        return objects;
    }
}
