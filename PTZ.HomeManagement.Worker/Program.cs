using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using System.Threading;

namespace PTZ.HomeManagement.Worker
{
    class Program
    {
        private static Timer _timer = new Timer(TimerElapsed, null, 0, 5000);
        private static AutoResetEvent waitHandle = new AutoResetEvent(false);
        private static ServiceProvider serviceProvider;

        static void Main(string[] args)
        {
            serviceProvider = new ServiceCollection()
                .BuildServiceProvider();

            WriteHeader();

            Console.CancelKeyPress += (o, e) =>
            {
                Console.WriteLine("Canceling...");
                waitHandle.Set();
            };

            waitHandle.WaitOne();
        }

        private static void WriteHeader()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"#==================================================================================#");
            Console.WriteLine(@"#      ':.                                                                         #");
            Console.WriteLine(@"#         []_____                                                                  #");
            Console.Write(@"#        /\      \             ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(@"Starting PTZ.HomeManagment Worker....");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"               #");
            Console.WriteLine(@"#    ___/  \__/\__\__                                                              #");
            Console.WriteLine(@"#---/\___\ |''''''|__\-- ---                                                       #");
            Console.WriteLine(@"#   ||'''| |''||''|''|                                                             #");
            Console.WriteLine(@"#   `´`´``´`````´``´```´```´``´``´``´``                                            #");
            Console.WriteLine(@"#==================================================================================#");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void TimerElapsed(object state)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"#==================================================================================#");
            //Get Jobs
            
            //Process Jobs w/(error control)

            //Output message
            WriteMessage("Core", LogLevel.Error, "I don't know");
            WriteMessage("Teste", LogLevel.Warning, "I don't know");
            WriteMessage("Core", LogLevel.Info, "I don't know");
        }

        public static void WriteMessage(string service, LogLevel level, string message)
        {
            StringBuilder builder = new StringBuilder(service);
            while (builder.Length < 10) { builder.Append(" "); }
            builder.Length = 10;

            StringBuilder levelbuilder = new StringBuilder(level.ToString());
            while (levelbuilder.Length < 7) { levelbuilder.Append(" "); }
            levelbuilder.Length = 7;

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mmm") + "|");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(builder);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("|");
            switch (level)
            {
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    break;
                case LogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Blue;
                    break;
                default:
                    break;
            }



            Console.Write(levelbuilder);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("|");
            Console.WriteLine("[" + message + "]");
        }
    }
}
