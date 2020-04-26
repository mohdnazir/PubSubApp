using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PubSubApp.WebSocketModule
{
    public static class Extentions
    {
        public static IApplicationBuilder MapWebSocketManager(this IApplicationBuilder app,
                                                        PathString path, IWebSocketHandler handler)
        {
            return app.Map(path, (_app) => _app.UseMiddleware<WebSocketManagerMiddleware>(handler));
        }

        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            services.AddTransient<ConnectionManager>();
            //services.AddSingleton(typeof(IWebSocketHandler), new ChatMessageHandler(new ConnectionManager()));
            //services.AddSingleton<IWebSocketHandler, ChatMessageHandler>();
            //services.AddSingleton(typeof(ChatMessageHandler));
            foreach (var type in Assembly.GetEntryAssembly().ExportedTypes)
            {
                if (type.GetTypeInfo().BaseType == typeof(WebSocketHandler))
                {
                    services.AddSingleton(typeof(IWebSocketHandler), type);
                }
            }

            return services;
        }
    }
}
