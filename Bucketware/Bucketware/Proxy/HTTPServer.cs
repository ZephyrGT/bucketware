// thanks to iProgramInCpp#0489, most things are made by him in the GrowtopiaCustomClient, 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Threading;
using System.Linq.Expressions;
using System.Windows.Forms;
using Bucketware.Layouts;
using Bucketware.Natives;

namespace GrowbrewProxy
{
    public class HTTPServer
    {
        static string Version = "HTTP/1.0";

        
        private static Proxyhelper mf;
        private static HttpListener listener = new HttpListener();
        public static void HTTPHandler()
        {
            while (listener.IsListening)
            {
                try
                {

                    string server_metadata = string.Empty;
                    using (WebClient client = new WebClient())
                    {
                        server_metadata = client.DownloadString("http://www.growtopia2.com/growtopia/server_data.php");
                        client.Dispose();
                    }

                    if (server_metadata != "")
                    {
#if DEBUG

#endif
                        Console.WriteLine("Parsing server metadata...");

                        

                        string[] tokens = server_metadata.Split('\n');
                        foreach (string s in tokens)
                        {
                            if (s.Length <= 0) continue;
                            if (s[0] == '#') continue;
                            if (s.StartsWith("RTENDMARKERBS1001")) continue;
                            string key = s.Substring(0, s.IndexOf('|')).Replace("\n", "");
                            string value = s.Substring(s.IndexOf('|') + 1);
                            

                            switch (key)
                            {
                                case "server":
                                    {
                                        // server ip

                                        Proxyhelper.globalUserData.Growtopia_Master_IP = value.Substring(0, value.Length);
                                        break;
                                    }
                                case "port":
                                    {
                                        ushort portval = ushort.Parse(value);
                                        //mf.UpdatePortBoxSafe(portval);
                                        Proxyhelper.globalUserData.Growtopia_Master_Port = portval;
                                        break;
                                    }
                                default:
                                    break;
                            }
                        }
                        Proxyhelper.globalUserData.Growtopia_IP = Proxyhelper.globalUserData.Growtopia_Master_IP;
                        Proxyhelper.globalUserData.Growtopia_Port = Proxyhelper.globalUserData.Growtopia_Master_Port;
                    }

                    HttpListenerContext context = listener.GetContext();
                    HttpListenerRequest request = context.Request;
                    HttpListenerResponse response = context.Response;
#if DEBUG
                    
#endif

                    if (request.HttpMethod == "POST")
                    {

                        byte[] buffer = Encoding.UTF8.GetBytes(
                            "server|127.0.0.1\n" +
                            "port|2\n" +
                            "type|1\n" +
                            "beta_server|127.0.0.1\n" +
                            "beta_port|2\n" +
                            "meta|homebrew.com\n" +
                            "type2|1\n");

                        response.ContentLength64 = buffer.Length;
                        System.IO.Stream output = response.OutputStream;
                        output.Write(buffer, 0, buffer.Length);
                        output.Close();
                        response.Close();
                    }
                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine(ex.Message);
                    Thread.Sleep(1000);
                    // probably cuz we stopped it, no need to worry.
                }
            }
        }

        public static MainForm mo;
        public static void StartHTTP(MainForm mainForm, string[] prefixes)
        {
            mo = mainForm;
            Console.WriteLine("Setting up HTTP Server...");
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            foreach (string s in prefixes)
            {
                listener.Prefixes.Add(s);
            }
           
            listener.Start();
            if (listener.IsListening) Console.WriteLine("Listening!");
            else Console.WriteLine("Could not listen to port 80, an error occured!");
            Thread t = new Thread(HTTPHandler);
            t.Start();
            Console.WriteLine("HTTP Server is running.");
        }

        public static void StopHTTP()
        {
            if (listener == null) return;
            if (listener.IsListening) listener.Stop();
        }
    }
}
