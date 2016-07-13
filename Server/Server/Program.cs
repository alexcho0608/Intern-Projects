using Fleck;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
namespace Server
{
    class Program
    {
        static Random r = new Random();
        static IList<IWebSocketConnection> allSockets = new List<IWebSocketConnection>();
        static IList<IWebSocketConnection> avaialable = new List<IWebSocketConnection>();

        static void Main(string[] args)
        {
            //System.Timers.Timer timer = new System.Timers.Timer(100);
            //timer.Enabled = true;
            //timer.Elapsed += new ElapsedEventHandler(CheckIfAvaialable);
            FleckLog.Level = LogLevel.Debug;
            var server = new WebSocketServer("ws://0.0.0.0:8181");
            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.WriteLine("Open!");
                    allSockets.Add(socket);
                };
                socket.OnClose = () =>
                {
                    Console.WriteLine("Close!");
                    allSockets.Remove(socket);
                };
                socket.OnMessage = message =>
                {
                    Console.WriteLine(message);
                    allSockets.ToList().ForEach(s => s.Send("Echo: " + message));

                };
                socket.OnPong = (x) =>
                {
                    Console.WriteLine("pong");
                    avaialable.Add(socket);
                };
            });

            Thread thread = new Thread(new ThreadStart(CheckIfAvaialable));
            thread.Start();
            string input;
            while (true)
            {
                input = RandomRates();
                ParallelOptions options = new ParallelOptions
                {
                    MaxDegreeOfParallelism = 10
                };

                if (allSockets.Count != 0)
                {
                    Parallel.ForEach(allSockets, options, (x) => { x.Send(input); });
                    Thread.Sleep(r.Next(100, 1000));
                }
            }
        }

        static string RandomRates()
        {
            var currencies = new List<string>() { "USD", "EUR", "BGN", "FFS", "ASD", "ART", "OPR", "OOP" };
            double rate = r.NextDouble() + r.Next(0, 100);
            ExchangeRate exchangeRate = new ExchangeRate();
            exchangeRate.Rate = rate;
            exchangeRate.Date = DateTime.Now;
            do
            {
                exchangeRate.From = currencies[r.Next(0, 8)];
                exchangeRate.To = currencies[r.Next(0, 8)];
            } while (exchangeRate.From == exchangeRate.To);

            return JsonConvert.SerializeObject(exchangeRate);
        }

        static void CheckIfAvaialable()
        {
            while (true)
            {
                Console.WriteLine("tesr");
                foreach (var socket in allSockets)
                {
                    socket.SendPing(new byte[] { 1 });
                }

                Thread.Sleep(100);

                allSockets = avaialable;
                avaialable = new List<IWebSocketConnection>();
                Thread.Sleep(1000 * 10);
            }
        }
    }
}
