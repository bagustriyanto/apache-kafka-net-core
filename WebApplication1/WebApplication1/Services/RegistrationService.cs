using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class RegistrationService : IHostedService
    {
        private readonly ConsumerConfig _consumerConfig;
        private readonly ConsumerWrapper _consumerWrapper;
        private readonly ILogger _logger;

        public RegistrationService(ConsumerConfig consumerConfig, ILogger<RequestBackgroundService> logger)
        {
            _consumerConfig = consumerConfig;
            _logger = logger;
            _consumerWrapper = new ConsumerWrapper(_consumerConfig, "new-user");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting RegistrationService registered in Startup");

            while (!cancellationToken.IsCancellationRequested)
            {
                string regsitrationMessage = _consumerWrapper.ReadMessage();

                if (!string.IsNullOrEmpty(regsitrationMessage))
                {
                    //Deserilaize 
                    RegistrationModel registration = JsonConvert.DeserializeObject<RegistrationModel>(regsitrationMessage);

                    //TODO:: Process New User
                    Console.WriteLine($"Info: RegistrationHandler => Processing the new user with email {registration.Email}");
                }
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping RegistrationService registered in Startup");
            return Task.CompletedTask;
        }
    }
}
