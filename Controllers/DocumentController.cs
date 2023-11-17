using Microsoft.AspNetCore.Mvc;
using DocumentManagementAPI.Models;
using DocumentManagementAPI.Interface;

namespace DocumentManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentHandler _documentHandler;

        public DocumentController(IDocumentHandler documentHandler)
        {
            _documentHandler = documentHandler;
        }

        // GET: api/<DocumentController>
        [HttpGet]

        public string Get()
        {

            Console.WriteLine("Hello World!!");

            return "Console Posted";
        }

        // GET api/<DocumentController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<DocumentController>
        [HttpPost]
        [Consumes("multipart/form-data")]
        [Produces("application/json")]
        public async Task<string> UploadDocumentAsync([FromForm] DocItem item)
        {
            try
            {
                string RequestID = await _documentHandler.UploadDocumentAsync(item);

                return RequestID;
            }
            catch (Exception)
            {
                throw;
            }

        }

        // PUT api/<DocumentController>/5
        [HttpPut ("file")]
        public async Task<string>MoveDocumentAsync( string file)
        {
            try
            {
                string RequestID = await _documentHandler.MoveDocumentAsync(file);

                return RequestID;
            }
            catch (Exception)
            {
                throw;

            }

        }

        // PUT api/<DocumentController>/5
        [HttpPut]
        public async Task<string> MoveAllDocumentAsync()
        {
            try
            {
                string response = await _documentHandler.MoveAllDocumentAsync();

                return response;
            }
            catch (Exception)
            {
                throw;

            }

        }

        // DELETE api/<DocController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
