using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Confluent.Kafka;
using WebApplication1.Services;
using Hangfire;
using WebApplication1.Configs;
using HostedService = Microsoft.Extensions.Hosting;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("HangfireConn")));
            services.AddHangfireServer();

            var kafkaProducerConfig = new ProducerConfig();
            var kafkaConsumerConfig = new ConsumerConfig();

            Configuration.Bind("KafkaProducer", kafkaProducerConfig);
            Configuration.Bind("KafkaConsumer", kafkaConsumerConfig);

            services.AddSingleton(kafkaProducerConfig);
            services.AddSingleton(kafkaConsumerConfig);
            services.AddSingleton<RegistrationService>();
            services.AddSingleton<RequestBackgroundService>();
            services.AddHostedService<BackgroundServiceConfig>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            var SchedulerConfig = new SchedulerConfig(backgroundJobClient, recurringJobManager);
            SchedulerConfig.SchedulerActivation();

            app.UseHangfireServer();
        }
    }
}
