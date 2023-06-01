using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzureQueueStorage.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class QueuesController : ControllerBase
    {
        private IConfiguration config;
        private string connstring = "";
        public QueuesController(IConfiguration config)
        {
            this.config = config;
            this.connstring = this.config.GetConnectionString("QueueConnectionString");
        }

        [HttpPost]
        [Route("createQueue/{queuename}")]
        public IActionResult CreateQueue(string queuename)
        {
            QueueClient queue = new QueueClient(this.connstring, queuename);
            if(queue.Exists().Value)
            {
                var res = new
                {
                    text = "Queue '" + queuename + "' exists!"
                };
                return Conflict(res);
            }
            queue.Create();
            return Ok(new {
                text = "Queue '" + queuename + "' created successfully!"
            });
        }

        [HttpGet]
        [Route("sendmessage/{queuename}/{message}")]
        public IActionResult SendMessage(string queuename, string message)
        {
            QueueClient queue = new QueueClient(this.connstring, queuename);
            if(!queue.Exists().Value)
            {
                return NotFound(new
                {
                    text = "Queue '" + queuename + "' Not Found!"
                });
            }
            queue.SendMessage(message);
            return Ok(new
            {
                text = "Message sent to '" + queuename + "' successfully"
            });
        }

        [HttpGet]
        [Route("readmessage/{queuename}")]
        public IActionResult ReadMessage(string queuename)
        {
            QueueClient queue = new QueueClient(this.connstring, queuename);
            if (!queue.Exists().Value)
            {
                return NotFound(new
                {
                    text = "Queue '" + queuename + "' Not Found!"
                });
            }
            QueueMessage queueMessage = queue.ReceiveMessage().Value;
            if(queueMessage == null)
            {
                return NotFound(new
                {
                    text = "No Messages to read!"
                });
            }
            queue.DeleteMessage(queueMessage.MessageId, queueMessage.PopReceipt);
            return Ok(queueMessage.Body.ToString());
        }

        [HttpDelete]
        [Route("delete/{queuename}")]
        public IActionResult DeleteQueue(string queuename)
        {
            QueueClient queue = new QueueClient(this.connstring, queuename);
            if (!queue.Exists().Value)
            {
                return NotFound(new
                {
                    text = "Queue '" + queuename + "' Not Found!"
                });
            }
            queue.Delete();
            return Ok(new
            {
                text = "Queue '" + queuename + "' deleted!"
            });
        }
    }
}
