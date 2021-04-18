using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DiscordRPC;
using DiscordRPC.Logging;
using Bucketware.Layouts;

namespace Bucketware
{
    static class Program
    {
        //DiscordRPC
        static public DiscordRpcClient client;
		static void Initialize()
		{
			client = new DiscordRpcClient("791017480063549470");

			client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

			client.OnReady += (sender, e) =>
			{
				Console.WriteLine("Received Ready from user {0}", e.User.Username);
			};

			client.OnPresenceUpdate += (sender, e) =>
			{
				Console.WriteLine("Received Update! {0}", e.Presence);
			};

			client.Initialize();

			client.SetPresence(new RichPresence()
			{
				Details = "Launcher",
				State = "https://??????",
				Assets = new Assets()
				{
					LargeImageKey = "anorych",
					LargeImageText = "Free opensource AutoFarm/Hack tool",
					SmallImageKey = "anorych2"
				}
			});
		}
		[STAThread]
        static void Main()
        {
			Initialize();//Initialize DiscordRPC

			Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Launcher());
        }
    }
}
