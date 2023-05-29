using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace Koko_ClientServer;
public class KokoServer {

    public static IPAddress SERVERIP = IPAddress.Parse("127.0.0.1");
    public static int SERVERPORT = 4040;

    private List<IPEndPoint> _Senders = new();

    private List<NetworkComponent> _NetworkComponents = new();
    int frameDelay = 1000 / 4; // 32 frames per second

    public void StartServer(List<NetworkComponent>? networkComponents = null) {
        _NetworkComponents = networkComponents ?? new();
        IPEndPoint ipep = new(SERVERIP, SERVERPORT);
        UdpClient _UpdServer = new(ipep);

        IPEndPoint anClientIp = new IPEndPoint(IPAddress.Any, 0);
        while (true) {
            if (_UpdServer.Available > 0) {
                var receivedFrom = ReceiveData(anClientIp, _UpdServer);
                if (receivedFrom != null && !_Senders.Contains(receivedFrom)) {
                    _Senders.Add(receivedFrom);
                }
            }

            for (int i = 0; i < _Senders.Count; i++) {
                SendData(_Senders[i], _UpdServer);
            }

            Thread.Sleep(frameDelay);
        }
    }

    protected void SendData(IPEndPoint sender, UdpClient me) {
        try {
            var data = JsonConvert.SerializeObject(_NetworkComponents);
            var serializedData = Encoding.UTF8.GetBytes(data);
            me.Send(serializedData, data.Length, sender);
        } catch {
            _Senders.Remove(sender);
        }
    }

    protected IPEndPoint? ReceiveData(IPEndPoint sender, UdpClient me) {
        try {
            var data = me.Receive(ref sender);
            Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));
            return sender;
        } catch {
            _Senders.Remove(sender);
        }

        return null;
    }
}
