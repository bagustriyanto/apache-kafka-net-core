using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApplication1.Services;

namespace WebApplication1.Configs
{
    public class BackgroundServiceConfig : IHostedService
    {
        private readonly ILogger _logger;
        private readonly RegistrationService _registrationService;
        private readonly RequestBackgroundService _requestBackgroundService;

        public BackgroundServiceConfig(ILogger<BackgroundServiceConfig> logger,
            RegistrationService registrationService,
            RequestBackgroundService requestBackgroundService)
        {
            _logger = logger;
            _registrationService = registrationService;
            _requestBackgroundService = requestBackgroundService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(async () => await _registrationService.StartAsync(cancellationToken));
            Task.Run(async () => await _requestBackgroundService.StartAsync(cancellationToken));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping RegistrationService registered in Startup");
            return Task.WhenAll(StartAsync(cancellationToken));
        }
    }
}
