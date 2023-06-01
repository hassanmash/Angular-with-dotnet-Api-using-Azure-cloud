using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;

namespace AzureTableStorage.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TablesController : ControllerBase
    {
        private IConfiguration config;
        private string connstring = "";
        private string tablename = "Users";
        public TablesController(IConfiguration config)
        {
            this.config = config;
            this.connstring = this.config.GetConnectionString("tableConnectionString");
        }

        [HttpPost]
        [Route("adduser")]
        public IActionResult AddUser(User u)
        {
            TableClient tc = new TableClient(connstring,tablename);
            try
            {
                tc.AddEntity(u);
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
            return Ok(new
            {
                text = "Added entity with user ID: '"+u.RowKey +"' in location: '" + u.PartitionKey + "'"
            });
        }

        [HttpGet]
        [Route("{pk}/{rk}")]
        public IActionResult GetUser(string pk, string rk)
        {
            TableClient tc = new TableClient(connstring,tablename);
            User? u = null;
            try
            {
                u = tc.GetEntity<User>(pk, rk);
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
            return Ok(u);
        }

        [HttpGet]
        [Route("allusers")]
        public IActionResult GetAllUsers()
        {
            TableClient tc = new TableClient(connstring,tablename);
            return Ok(tc.Query<User>().ToList());
        }

        [HttpGet]
        [Route("location/{location}")]
        public IActionResult GetUserbyLocation(string location)
        {
            TableClient tc = new TableClient(connstring,tablename);
            var result = tc.Query<User>(u => u.PartitionKey == location);

            if (result.Count() == 0)
                return NotFound(new
                {
                    text = "No Users found for location: " + location
                });

            return Ok(result.ToList());
        }
    }

    public class User: ITableEntity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsLoginAllowed { get; set; }
        public string ?PartitionKey { get; set; }
        public string ?RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
