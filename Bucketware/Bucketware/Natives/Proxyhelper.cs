using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENet.Managed;
using ENet.Managed.Allocators;
using ENet.Managed.Native;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using GrowbrewProxy;
using Bucketware.Layouts;

namespace Bucketware.Natives
{
    class Proxyhelper
    {
        public class StateObject
        {
            // Client socket.  
            public Socket workSocket = null;
            // Size of receive buffer.  
            public const int BufferSize = 1024;
            // Receive buffer.  
            public byte[] buffer = new byte[BufferSize];
            // Received data string.  
            public StringBuilder sb = new StringBuilder();
        }

        public static byte GrowbrewHNetVersion = 1;
        public static bool isHTTPRunning = false;

        public static bool multibottingEnabled = false;

        public static bool skipCache = false;
        public static bool logallpackettypes = false;

        public static TcpClient tClient = new TcpClient();
        public static StateObject stateObj = new StateObject();


        public static string LogText = string.Empty;

        private delegate void SafeCallDelegate(string text);
        private delegate void SafeCallDelegatePort(ushort port);

        public static ENetHost client;
        public static ENetHost m_Host;

        public static ENetPeer realPeer;
        public static ENetPeer proxyPeer;
#pragma warning disable CS0436 // Type conflicts with imported type
        // unnecessary as botting isnt made for open src anyway
#pragma warning restore CS0436 // Type conflicts with imported type

        public class UserData
        {
            public ulong connectIDReal = 0;
            public ulong connectID = 0;

            public bool didQuit = false;
            public bool mayContinue = false;
            public bool srvRunning = false;
            public bool clientRunning = false;
            public int Growtopia_Port = 17196;
            public string Growtopia_IP = "213.179.209.168";
            public string Growtopia_Master_IP = "213.179.209.168";
            public int Growtopia_Master_Port = 17196;

            public bool isSwitchingServer = false;
            public bool blockEnterGame = false;
            public bool serializeWorldsAdvanced = true;
            public bool bypass10PlayerMax = true;

            // internal variables =>
            public string tankIDName = "";
            public string tankIDPass = "";
            public string game_version = "4.20";
            public string country = "de";
            public string requestedName = "";
            public int token = 0;
            public bool resetStuffNextLogon = false;
            public int userID = -1;
            public int lmode = -1;
            public byte[] skinColor = new byte[4];
            public bool enableSilentReconnect = false;
            public bool hasLogonAlready = false;
            public bool hasUpdatedItemsAlready = false;
            public bool bypassAAP = false;
            public bool ghostSkin = false;
            // CHEAT VARS/DEFS
            public bool cheat_magplant = false;
            public bool cheat_rgbSkin = false;
            public bool cheat_autoworldban_mod = false;
            public bool cheat_speedy = false;
            public bool isAutofarming = false;
            public bool cheat_Autofarm_magplant_mode = false;
            public bool redDamageToBlock = false; // exploit discovered in servers at time of client being in version 3.36/3.37
                                                  // CHEAT VARS/DEFS
            public string macc = "02:15:01:20:30:05";
            public string doorid = "";
            public string rid = "", sid = "";


            public bool ignoreonsetpos = false;
            public bool unlimitedZoom = false;
            public bool isFacingSwapped = false;
            public bool blockCollecting = false;
            public short lastWrenchX = 0;
            public short lastWrenchY = 0;
            public bool awaitingReconnect = false;
            public bool enableAutoreconnect = false;
            public string autoEnterWorld = "";
            public bool dontSerializeInventory = false;
            public bool skipGazette = false;
        }

        public static UserData globalUserData = new UserData();
        public static ItemDatabase itemDB = new ItemDatabase();
        public static HandleMessages messageHandler = new HandleMessages();
        public static string GenerateRID()
        {
            string str = "0";
            Random random = new Random();
            const string chars = "ABCDEF0123456789";
            str += new string(Enumerable.Repeat(chars, 31)
               .Select(s => s[random.Next(s.Length)]).ToArray());
            return str;
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string GenerateUniqueWinKey()
        {
            string str = "7";
            Random random = new Random();
            const string chars = "ABCDEF0123456789";
            str += new string(Enumerable.Repeat(chars, 31)
               .Select(s => s[random.Next(s.Length)]).ToArray());
            return str;
        }

        public static string GenerateMACAddress()
        {
            var random = new Random();
            var buffer = new byte[6];
            random.NextBytes(buffer);
            var result = String.Concat(buffer.Select(x => string.Format("{0}:", x.ToString("X2"))).ToArray());
            return result.TrimEnd(':');
        }

        public static string CreateLogonPacket(string customGrowID = "", string customPass = "", int customUserID = -1, int customToken = -1, string doorID = "")
        {

            // this is kinda messy
            string gversion = globalUserData.game_version;
            string p = string.Empty;
            Random rand = new Random();
            bool requireAdditionalData = globalUserData.token > -1;

            if (customGrowID == "")
            {
                if (globalUserData.tankIDName != "")
                {
                    p += "tankIDName|" + (globalUserData.tankIDName + "\n");
                    p += "tankIDPass|" + (globalUserData.tankIDPass + "\n");
                }
            }
            else
            {
                //Console.WriteLine("CUSTOM GROWID IS : " + customGrowID);
                p += "tankIDName|" + (customGrowID + "\n");
                p += "tankIDPass|" + (customPass + "\n");
            }

            p += "requestedName|" + ("Growbrew" + rand.Next(0, 255).ToString() + "\n"); //"Growbrew" + rand.Next(0, 255).ToString() + "\n"
            p += "f|1\n";
            p += "protocol|120\n";
            p += "game_version|" + (gversion + "\n");
            if (requireAdditionalData) p += "lmode|" + globalUserData.lmode + "\n";
            p += "cbits|128\n";
            p += "player_age|100\n";
            p += "GDPR|1\n";
            p += "hash2|" + rand.Next(-777777776, 777777776).ToString() + "\n";
            p += "meta|playingo.co.uk-456236.999666420.de\n"; // soon auto fetch meta etc.
            p += "fhash|-716928004\n";
            p += "platformID|0\n";
            p += "deviceVersion|0\n";
            p += "country|" + (globalUserData.country + "\n");
            p += "hash|" + rand.Next(-777777776, 777777776).ToString() + "\n";
            p += "mac|" + globalUserData.macc + "\n";
            p += ("rid|" + (globalUserData.rid == "" ? GenerateRID() : globalUserData.rid) + "\n");
            if (requireAdditionalData) p += "user|" + (globalUserData.userID.ToString() + "\n");
            if (requireAdditionalData) p += "token|" + (globalUserData.token.ToString() + "\n");
            if (customUserID > 0) p += "user|" + (customUserID.ToString() + "\n");
            if (customToken > 0) p += "token|" + (customToken.ToString() + "\n");
            if (globalUserData.doorid != "" && doorID == "") p += "doorID|" + globalUserData.doorid + "\n";
            else if (doorID != "") p += "doorID|" + doorID + "\n";
            p += ("wk|" + (globalUserData.sid == "" ? GenerateUniqueWinKey() : globalUserData.sid) + "\n");
            p += "fz|1331849031";
            Console.WriteLine(p);
            p += "zf|-1331849031";
            return p;
        }
        public static void ConnectToServer(ref ENetPeer peer, UserData userData = null, bool FirstInitialUseOfBot = false)
        {
            Console.WriteLine("Internal proxy client is attempting a connection to server...");

            string ip = globalUserData.Growtopia_IP;
            int port = globalUserData.Growtopia_Port;


            if (peer == null)
            {
                peer = client.Connect(new IPEndPoint(IPAddress.Parse(ip), port), 2, 0);
            }
            else
            {
                if (peer.IsNull)
                {
                    peer = client.Connect(new IPEndPoint(IPAddress.Parse(ip), port), 2, 0);
                }
                else if (peer.State != ENetPeerState.Connected)
                {
                    peer = client.Connect(new IPEndPoint(IPAddress.Parse(ip), port), 2, 0);
                }
                else
                {

                    // peer = client.Connect(new IPEndPoint(IPAddress.Parse(ip), port), 2, 0);
                    globalUserData.awaitingReconnect = true;
                    peer.Disconnect(0);

                    //In this case, we will want the realPeer to be disconnected first 

                    // sub server switching, most likely.
                    peer = client.Connect(new IPEndPoint(IPAddress.Parse(ip), port), 2, 0);
                }
            }
        }

        public static void Host_OnConnect(ENetPeer peer)
        {

            proxyPeer = peer;
            globalUserData.connectID++;
            ConnectToServer(ref realPeer);

        }

        public static void Peer_OnDisconnect(object sender, uint e)
        {
            ENetPeer peer = (ENetPeer)sender;
            if (globalUserData.isSwitchingServer)
            {
                globalUserData.isSwitchingServer = false;
                GamePacketProton variantPacket = new GamePacketProton();
                variantPacket = new GamePacketProton();
                variantPacket.delay = 0; //Avoid too quick connection and give headroom for enetcommand to prevent random/rare freezing (fix by Toxic Vampor)
                variantPacket.NetID = -1;
                variantPacket.AppendString("OnSendToServer");
                variantPacket.AppendInt(2);
                variantPacket.AppendInt(globalUserData.token);
                variantPacket.AppendInt(globalUserData.userID);
                variantPacket.AppendString("127.0.0.1|" + globalUserData.doorid);
                variantPacket.AppendInt(globalUserData.lmode);

                messageHandler.packetSender.SendData(variantPacket.GetBytes(), proxyPeer);
                return;
            }

            if (globalUserData.enableSilentReconnect)
            {
                unsafe
                {
                    if (((ENetPeer)sender).GetNativePointer()->ConnectID != realPeer.GetNativePointer()->ConnectID) return;
                }

                try
                {
                    realPeer.Send(0, new byte[60], ENetPacketFlags.Reliable);
                }
                catch
                {

                    if (proxyPeer != null)
                    {
                        if (proxyPeer.State == ENetPeerState.Connected)
                        {
                            GamePacketProton variantPacket = new GamePacketProton();
                            variantPacket.AppendString("OnConsoleMessage");
                            variantPacket.AppendString("`6Bucketware - Reconnecting..``");
                            messageHandler.packetSender.SendData(variantPacket.GetBytes(), proxyPeer);
                        }
                    }
                }

                // ConnectToServer(useRealPeer ? ref realPeer : ref peer);

                ConnectToServer(ref realPeer);
            }
            else if (globalUserData.enableAutoreconnect)
            {
                unsafe
                {
                    if (((ENetPeer)sender).GetNativePointer()->ConnectID != realPeer.GetNativePointer()->ConnectID) return;
                }

                try
                {
                    realPeer.Send(0, new byte[60], ENetPacketFlags.Reliable);
                }
                catch
                {

                    if (proxyPeer != null)
                    {
                        if (proxyPeer.State == ENetPeerState.Connected)
                        {
                            GamePacketProton variantPacket2 = new GamePacketProton();
                            variantPacket2.AppendString("OnReconnect");
                            messageHandler.packetSender.SendData(variantPacket2.GetBytes(), proxyPeer);
                        }
                    }
                }
            }
            messageHandler.enteredGame = false;

            
        }

        public static void Peer_OnReceive(object sender, ENetPacket e)
        {
            ENetPeer peer = (ENetPeer)sender;
            string str = messageHandler.HandlePacketFromClient(ref peer, e);
        }

        public static void Peer_OnReceive_Client(object sender, ENetPacket e)
        {

            ENetPeer peer = (ENetPeer)sender;
            string str = messageHandler.HandlePacketFromServer(ref peer, e);
        }

        public static void Client_OnConnect(ENetPeer peer)
        {
            peer.Timeout(1000, 4000, 6000);
            //peer.PingInterval(TimeSpan.FromMilliseconds(1000));

            realPeer = peer;
            globalUserData.connectIDReal++;
        }

        public static void doServerService(int delay = 0)
        {
            doClientService(0);
            var Event = m_Host.Service(TimeSpan.FromMilliseconds(delay));

            switch (Event.Type)
            {
                case ENetEventType.None:

                    break;
                case ENetEventType.Connect:
                    Host_OnConnect(Event.Peer);
                    break;
                case ENetEventType.Disconnect:

                    break;
                case ENetEventType.Receive:

                    Peer_OnReceive(Event.Peer, Event.Packet);

                    Event.Packet.Destroy();
                    break;
                default:
                    throw new NotImplementedException();
            }
            doClientService(0);
        }

        public static void doClientService(int delay = 0)
        {
            if (client == null) return;
            if (client.Disposed) return;
            var Event = client.Service(TimeSpan.FromMilliseconds(delay));

            unsafe
            {
                switch (Event.Type)
                {
                    case ENetEventType.None:

                        break;
                    case ENetEventType.Connect:
                        Client_OnConnect(Event.Peer);
                        break;
                    case ENetEventType.Disconnect:
                        Peer_OnDisconnect(Event.Peer, 0);
                        Event.Peer.UnsetUserData();
                        break;
                    case ENetEventType.Receive:

                        Peer_OnReceive_Client(Event.Peer, Event.Packet);
                        Event.Packet.Destroy();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }


        public static void doClientServerServiceLoop()
        {
            while (true)
            {
                doClientService(1);
                doServerService(3);
            }
        }

        public static void doProxy()
        {
            if (client == null || m_Host == null)
            {
                MessageBox.Show("Failed to start proxy, either client was null or m_Host was, please check if the port 2 is free. (Growbrew Proxy port)");
                return;
            }

            Task.Run(() => doClientServerServiceLoop());
        }

        public static void LaunchProxy()
        {
            if (!globalUserData.srvRunning)
            {
                globalUserData.srvRunning = true;
                globalUserData.clientRunning = true;

                // Setting up ENet-Server ->

                m_Host = new ENetHost(new IPEndPoint(IPAddress.Any, 2), 32, 10, 0, 0);
                m_Host.ChecksumWithCRC32();
                m_Host.CompressWithRangeCoder();
                m_Host.EnableNewProtocol(2);

                // Setting up ENet-Client ->
                client = new ENetHost(null, 64, 10); // for multibotting, coming soon.
                client.ChecksumWithCRC32();
                client.CompressWithRangeCoder();
                client.EnableNewProtocol(1);

                // realPeer = client.Connect(new IPEndPoint(IPAddress.Parse(globalUserData.Growtopia_Master_IP), globalUserData.Growtopia_Master_Port), 2, 0);
                //realPeer = client.Connect(new IPEndPoint(IPAddress.Parse(globalUserData.Growtopia_Master_IP), globalUserData.Growtopia_Master_Port), 2, 0);
                doProxy();

                // Setting up controls
                Console.WriteLine("RUnning!");
            }
        }
        public static uint HashBytes(byte[] b) // Thanks to iProgramInCpp !
        {
            byte[] n = b;
            uint acc = 0x55555555;

            for (int i = 0; i < b.Length; i++)
            {
                acc = (acc >> 27) + (acc << 5) + n[i];
            }
            return acc;
        }
        public static void loadproxy()
        {
            ManagedENet.Startup();
            globalUserData.macc = GenerateMACAddress();

            if (!Directory.Exists("stored"))
                Directory.CreateDirectory("stored");
            itemDB.SetupItemDefs();
        }
    }
}
