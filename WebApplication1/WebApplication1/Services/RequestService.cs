using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class RequestService
    {
        private static HttpClient client = new HttpClient();

        private ProducerConfig _producerConfig;

        public RequestService(ProducerConfig producerConfig)
        {
            _producerConfig = producerConfig;
        }

        public async Task PostToFakeJson()
        {
            Console.WriteLine("============== START POST TO FAKESJON ==============");
            object param = new
            {
                token = "iQVOFmMwRs1S6S3KsFmHzQ",
                data = new
                {
                    Id = "personNickname",
                }
            };

            Console.WriteLine("=> Start request");
            string json = JsonConvert.SerializeObject(param);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage responseMessage = await client.PostAsync("https://app.fakejson.com/q", content);
            string data = await responseMessage.Content.ReadAsStringAsync();
            Console.WriteLine("=> End request");

            Console.WriteLine("=> Send data to kafka topic");
            var producer = new ProducerWrapper(_producerConfig, "post-fakejson");
            await producer.WriteMessage(data);
            Console.WriteLine("=> End proccess of kafka");
            Console.WriteLine("============== END POST TO FAKESJON ==============");
        } 
    }
}
