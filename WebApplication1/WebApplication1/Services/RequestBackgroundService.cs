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
    public class RequestBackgroundService : IHostedService
    {
        private readonly ConsumerConfig _consumerConfig;
        private readonly ConsumerWrapper _consumerWrapper;
        private readonly ILogger _logger;

        public RequestBackgroundService(ConsumerConfig consumerConfig, ILogger<RequestBackgroundService> logger)
        {
            _consumerConfig = consumerConfig;
            _logger = logger;
            _consumerWrapper = new ConsumerWrapper(_consumerConfig, "post-fakejson");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting RequestBackgroundService registered in Startup");

            while (!cancellationToken.IsCancellationRequested)
            {
                string regsitrationMessage = _consumerWrapper.ReadMessage();

                if (!string.IsNullOrEmpty(regsitrationMessage))
                {
                    //Deserilaize 
                    RegistrationModel registration = JsonConvert.DeserializeObject<RegistrationModel>(regsitrationMessage);

                    //TODO:: Process New User
                    Console.WriteLine($"Info: RequestBackgroundService => Recieved response with userID: {registration.Id}");
                }
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping RequestBackgroundService registered in Startup");
            return Task.CompletedTask;
        }
    }
}
