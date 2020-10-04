using Akka.Actor;
using Akka.Configuration;
using AkkaVisualDemo.Actors;
using AkkaVisualDemo.Messages;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace AkkaVisualDemo
{
    class Program
    {
        public static ActorSystem ActSystem { get; private set; }

        static void Main(string[] args)
        {
            var hoconFile = XElement.Parse(File.ReadAllText(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\AkkaHocon.config"));
            var config = ConfigurationFactory.ParseString(hoconFile.Descendants("hocon").Single().Value);
            ActSystem = ActorSystem.Create("akka-visual-demo", config);

            var sender = ActSystem.ActorOf<MainActor>("sender");

            Console.WriteLine("Press any key to send message");
            Console.ReadKey();

            sender.Tell(new CreateChild());

            Console.ReadKey();
        }
    }
}
