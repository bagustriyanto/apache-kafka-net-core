using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private ProducerConfig _producerConfig;
        public RegistrationController(ProducerConfig producerConfig)
        {
            _producerConfig = producerConfig;
        }
        // GET: api/Registration
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Registration/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Registration
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] RegistrationModel registration)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                //Serialize 
                string serializedOrder = JsonConvert.SerializeObject(registration);

                Console.WriteLine("========");
                Console.WriteLine("Info: RegistrationController => Post => Recieved a new registration user:");
                Console.WriteLine(serializedOrder);
                Console.WriteLine("=========");

                var producer = new ProducerWrapper(_producerConfig, "new-user");
                await producer.WriteMessage(serializedOrder);

                return Ok("Your registartion is success");
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Registration/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
