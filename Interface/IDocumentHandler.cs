using Amazon.S3.Model;
using Amazon.S3;
using DocumentManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagementAPI.Interface
{
    public interface IDocumentHandler
    {
        public Task<string> UploadDocumentAsync(DocItem item);

        public Task<string> MoveDocumentAsync(string file);

        public Task<string> MoveAllDocumentAsync();
    }
}
 