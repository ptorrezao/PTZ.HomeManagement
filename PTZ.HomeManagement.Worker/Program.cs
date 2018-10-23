using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using System.Threading;
using AutoMapper;
using System.Collections.Generic;
using PTZ.HomeManagement.ExpirationReminder.Data.Core;
using PTZ.HomeManagement.ExpirationReminder.Services;
using PTZ.HomeManagement.Interfaces;
using PTZ.HomeManagement.Core.Data;
using PTZ.HomeManagement.Data;
using PTZ.HomeManagement.ExpirationReminder.Data.Core.EF;
using Microsoft.EntityFrameworkCore;
using PTZ.HomeManagement.Enums;
using PTZ.HomeManagement.Utils;
using PTZ.HomeManagement.Services;
using PTZ.HomeManagement.Models;

namespace PTZ.HomeManagement.Worker
{
    public class Program
    {
        private Timer _timer;
        private static AutoResetEvent waitHandle = new AutoResetEvent(false);
        private IServiceProvider servicesProvider;

        public static void Main(string[] args)
        {
            Program program = new Program();
            var minutes = 5;

            program.Run(minutes * 60 * 1000);
        }

        private void Run(int timerMiliseconds)
        {
            PrepareIoC();

            SetTimerElapsedTime(timerMiliseconds);

            WriteHeader();

            PrepareCancelAction();
        }

        private void PrepareCancelAction()
        {
            Console.CancelKeyPress += (o, e) =>
            {
                Console.WriteLine("Canceling...");
                waitHandle.Set();
                _timer.Dispose();
            };

            waitHandle.WaitOne();
        }

        private void SetTimerElapsedTime(int timerMiliseconds)
        {
            _timer = new Timer(TimerElapsed, null, 0, timerMiliseconds);
        }

        private void PrepareIoC()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddAutoMapper();

            services.Configure<EmailSettings>(x =>
            {
                x.ApiKey = Environment.GetEnvironmentVariable("MailGun_ApiKey") ?? "";
                x.ApiBaseUri = Environment.GetEnvironmentVariable("MailGun_ApiBaseUri") ?? "";
                x.RequestUri = Environment.GetEnvironmentVariable("MailGun_RequestUri") ?? "";
                x.From = Environment.GetEnvironmentVariable("MailGun_From") ?? "";
                x.Domain = Environment.GetEnvironmentVariable("MailGun_Domain") ?? "";
            });

            services.AddTransient<ICoreService, CoreService>();
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddDbContext<ApplicationDbContext>(options => this.SetCorrectProvider(options));
            services.AddDbContext<ExpirationReminderDbContext>(options => this.SetCorrectProvider(options));

            services.AddTransient<IApplicationRepository, ApplicationDbRepository>();
            services.AddTransient<IExpirationReminderRepository, ExpirationReminderRepository>();
            services.AddTransient<IExpirationReminderService, ExpirationReminderService>();
            services.AddTransient<IWorker, ExpirationReminderService>();

            servicesProvider = services.BuildServiceProvider();
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

        private void TimerElapsed(object state)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"#==================================================================================#");

            var services = servicesProvider.GetServices<IWorker>();
            foreach (var service in services)
            {
                var jobs = service.GetJobs();

                foreach (var job in jobs)
                {
                    WriteMessage(service.GetName(), LogLevel.Info, job.Invoke());
                }
            }
        }

        private void WriteMessage(string service, LogLevel level, string message)
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

        private void SetCorrectProvider(DbContextOptionsBuilder options)
        {
            string envVar = Environment.GetEnvironmentVariable("DB_TYPE");
            DatabaseType dbType;
            Enum.TryParse(envVar ?? DatabaseUtils.GetDefaultDb(), out dbType);

            string connectionString = DatabaseUtils.GetConnectionString(dbType);

            switch (dbType)
            {
                case DatabaseType.SqlServer:
                    options.UseSqlServer(connectionString);
                    break;
                case DatabaseType.PostgreSQL:
                    options.UseNpgsql(connectionString);
                    break;
                default:
                    options.UseSqlite(connectionString);
                    break;
            }
        }
    }
}
