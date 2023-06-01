using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzureBlobStorage.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BlobsController : ControllerBase
    {
        private IConfiguration config;
        private string connstring = "";
        public BlobsController(IConfiguration config)
        {
            this.config = config;
            this.connstring = this.config.GetConnectionString("BlobConnectionString");
        }

        [HttpGet]
        [Route("containers")]
        public IActionResult GetContainers()
        {
            //1. Get access to blob service within the storage acc
            BlobServiceClient client = new BlobServiceClient(connstring);
            List<string> containers = new List<string>();

            foreach(var container in client.GetBlobContainers())
            {
                containers.Add(container.Name);
            }
            if(containers.Count == 0)
            {
                var result = new
                {
                    text = "No containers found",
                };
                return NotFound(result);
            }
            return Ok(containers);
        }

        [HttpGet]
        [Route("createcontainer/{containername}")]
        public IActionResult CreateContainer(string containername)
        {
            BlobContainerClient container = new BlobContainerClient(connstring,containername);

            if(container.Exists().Value)
            {
                var result = new
                {
                    text = "Container named " + containername + " exists!",
                };
                return Conflict(result);
            }
            container.Create();
            var createdresult = new
            {
                text = "Container " + containername + " created!",
            };
            return Ok(createdresult);
        }

        [HttpGet]
        [Route("{containername}")]
        public IActionResult GetBlobs(string containername)
        {
            BlobContainerClient container = new BlobContainerClient(connstring,containername);

            if(!container.Exists().Value)
            {
                var res = new
                {
                    text = "Blob container " + container.Name + " not found!",
                };
                return NotFound(res);
            }
            List<string> blobs = new List<string>();
            foreach (var blob in container.GetBlobs())
            {
                blobs.Add(blob.Name);
            }
            if(blobs.Count == 0)
            {
                var res1 = new
                {
                    text = "no blobs in container " + containername,
                };
                return NotFound(res1);
            }
            return Ok(blobs);
        }

        [HttpPost]
        [Route("upload/{containername}")]
        public IActionResult UploadBlob(string containername, IFormFile blob)
        {
            BlobContainerClient container = new BlobContainerClient(connstring, containername);
            if(!container.Exists().Value)
            {
                var res = new
                {
                    text = "Container '" + containername + "' not found!",
                };
                return NotFound(res);
            }
            BlobClient currentBlob = container.GetBlobClient(blob.FileName);
            if (currentBlob.Exists().Value)
            {
                var res2 = new
                {
                    text = "Blob '" + blob.FileName + "' already exists!",
                };
                return NotFound(res2);
            }
            container.UploadBlob(blob.FileName, blob.OpenReadStream());
            var res1 = new
            {
                text = "Blob named " + blob.FileName + " uploaded!",
            };
            return Ok(res1);
        }

        [HttpGet]
        [Route("download/{containername}/{blobname}")]
        public IActionResult DownloadBlob(string containername, string blobname)
        {
            BlobContainerClient container = new BlobContainerClient(connstring, containername);

            if (!container.Exists().Value)
            {
                var res = new
                {
                    text = "Container '" + containername + "' not found!",
                };
                return NotFound(res);
            }
            BlobClient blob = container.GetBlobClient(blobname);
            if (!blob.Exists().Value)
            {
                var res1 = new
                {
                    text = "Blob '" + blobname + "' not found!",
                };
                return NotFound(res1);
            }
            MemoryStream ms = new MemoryStream();
            blob.DownloadTo(ms);

            return File(ms.ToArray(), "application/octet-stream", blob.Name);
        }

        [HttpDelete]
        [Route("deletecontainer/{containername}")]
        public IActionResult DeleteContainer(string containername)
        {
            BlobContainerClient container = new BlobContainerClient(connstring, containername);
            if (!container.Exists().Value)
            {
                var res = new
                {
                    text = "Container '" + containername + "' not found!",
                };
                return NotFound(res);
            }
            container.Delete();
            var res2 = new
            {
                text = "Container '" + containername + "' deleted!",
            };
            return Ok(res2);
        }

        [HttpDelete]
        [Route("deleteblob/{containername}/{blobname}")]
        public IActionResult DeleteBlob(string containername, string blobname)
        {
            BlobContainerClient container = new BlobContainerClient(connstring, containername);
            if (!container.Exists().Value)
            {
                var res = new
                {
                    text = "Container '" + containername + "' not found!",
                };
                return NotFound(res);
            }
            BlobClient blob = container.GetBlobClient(blobname);
            if (!blob.Exists().Value)
            {
                var res1 = new
                {
                    text = "Blob '" + blobname + "' not found!",
                };
                return NotFound(res1);
            }
            blob.Delete();
            var res2 = new
            {
                text = "Blob '" + blobname + "' deleted!",
            };
            return Ok(res2);
        }
    }
}
