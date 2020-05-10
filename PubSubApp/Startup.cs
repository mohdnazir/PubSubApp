#define UseOptions // or NoOptions or UseOptionsAO
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Debug;
using System;
using System.Timers;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Threading;
using Timer = System.Timers.Timer;
using PubSubApp.PubSub;
using PubSubApp.WebSocketModule;
using PubSubApp.Models;
using Microsoft.Extensions.Options;
using PubSubApp.Services;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;

namespace PubSubApp
{
    public class Startup
    {
        Timer timer;
        //TimeServiceHandler timeService;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option => { option.EnableEndpointRouting = false; })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(options => options.UseMemberCasing());
            services.AddLogging(builder =>
            {
                builder.AddConsole()
                    .AddDebug()
                    .AddFilter<ConsoleLoggerProvider>(category: null, level: LogLevel.Debug)
                    .AddFilter<DebugLoggerProvider>(category: null, level: LogLevel.Debug);
            });
            services.AddWebSocketManager();
            services.Configure<PubSubAppDatabaseSettings>(
                    Configuration.GetSection(nameof(PubSubAppDatabaseSettings)));

            services.AddSingleton<IPubSubAppDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<PubSubAppDatabaseSettings>>().Value);

            services.AddSingleton<UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            var ser = (IWebSocketHandler)app.ApplicationServices.GetService(typeof(IWebSocketHandler));
            //var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            //var serviceProvider = serviceScopeFactory.CreateScope().ServiceProvider;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseWebSockets();
            app.MapWebSocketManager("/ws", ser);
            //timeService = serviceProvider.GetService<TimeServiceHandler>();
            //app.MapWebSocketManager("/ws", timeService);
            app.UseMvc();
            app.UseStaticFiles();
            //app.UseFileServer();

            timer = new Timer(5000);
            timer.Elapsed += (a, b) =>
            {
                ser.SendMessageToAllAsync(DateTime.Now.ToLongTimeString());
            };
            //timer.Start();

            //EventAggregator eve = new EventAggregator();
            //pub = new Publisher(eve);
            //Subscriber sub = new Subscriber(eve);

            //#if NoOptions
            //            #region UseWebSockets
            //            app.UseWebSockets();
            //            #endregion
            //#endif

            //#if UseOptions
            //            #region UseWebSocketsOptions
            //            var webSocketOptions = new WebSocketOptions()
            //            {
            //                KeepAliveInterval = TimeSpan.FromSeconds(120),
            //                ReceiveBufferSize = 4 * 1024
            //            };

            //            app.UseWebSockets(webSocketOptions);
            //            #endregion
            //#endif

            //#if UseOptionsAO
            //            #region UseWebSocketsOptionsAO
            //            var webSocketOptions = new WebSocketOptions()
            //            {
            //                KeepAliveInterval = TimeSpan.FromSeconds(120),
            //                ReceiveBufferSize = 4 * 1024
            //            };
            //            webSocketOptions.AllowedOrigins.Add("https://client.com");
            //            webSocketOptions.AllowedOrigins.Add("https://www.client.com");

            //            app.UseWebSockets(webSocketOptions);
            //            #endregion
            //#endif
            //            #region AcceptWebSocket
            //            app.Use(async (context, next) =>
            //            {
            //                if (context.Request.Path == "/ws")
            //                {
            //                    if (context.WebSockets.IsWebSocketRequest)
            //                    {
            //                        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
            //                        await Echo(context, webSocket);
            //                    }
            //                    else
            //                    {
            //                        context.Response.StatusCode = 400;
            //                    }
            //                }
            //                else
            //                {
            //                    await next();
            //                }

            //            });
            //            #endregion

        }


        //#region Echo
        //private async Task Echo(HttpContext context, WebSocket webSocket)
        //{
        //    var buffer = new byte[1024 * 4];
        //    WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        //    while (!result.CloseStatus.HasValue)
        //    {
        //        await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

        //        result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        //    }
        //    await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        //}
        //#endregion
    }
}
