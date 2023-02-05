using Discord.WebSocket;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliveBot.Bot.Handler
{
    public class BotHealthCheck : IHealthCheck
    {
        private DiscordSocketClient client;
        public BotHealthCheck(DiscordSocketClient client) {
            this.client = client;
        }

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, 
            CancellationToken cancellationToken = default
        )   { 

            var connectionState = client.ConnectionState;
            var isHealthy = connectionState == Discord.ConnectionState.Connected;

            if (isHealthy)
            {
                return Task.FromResult(
                    HealthCheckResult.Healthy(
                        $"Discord client is connected: {Enum.GetName(connectionState)}"
                    )
                );
            }

            return Task.FromResult(
                new HealthCheckResult(
                    context.Registration.FailureStatus, $"Discord Client has an unhealty status: {Enum.GetName(connectionState)}."
                )
            );
        }
    }
}
