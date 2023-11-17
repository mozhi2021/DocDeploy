using Amazon.S3.Model;
using Amazon.S3;
using DocumentManagementAPI.Interface;
using DocumentManagementAPI.Models;
using Amazon;
using Microsoft.Extensions.Options;
using System.Diagnostics.Metrics;

namespace DocumentManagementAPI.Repositories
{
    public class DocumentHandler : IDocumentHandler
    {
       // private readonly string _awsBucketName;
        private readonly string _awssourceBucketName;
        private readonly string? _awsdestinationBucketName;

        public DocumentHandler(IOptions<AWSConfig> awsConfig)
        {
           // _awsBucketName = awsConfig.Value.BucketName;
            _awssourceBucketName = awsConfig?.Value.sourceBucketName;
            _awsdestinationBucketName = awsConfig?.Value.destinationBucketName;
    }

    //Upload file to S3 bucket
    public async Task<string> UploadDocumentAsync(DocItem item)
        {
            string awsRequestID = string.Empty;
            var stream = item.File.OpenReadStream();
            var fileNameInS3Bucket = item.File.FileName;

            AmazonS3Client client = new AmazonS3Client();
            var request = new PutObjectRequest()
                {
                   // BucketName = _awsBucketName,
                    Key = fileNameInS3Bucket,
                    InputStream = stream
                };
                try
                {
                    var response = await client.PutObjectAsync(request);
                    awsRequestID = response.ResponseMetadata.RequestId.ToString();
                }
                catch (AmazonS3Exception amazonS3exception)
                {
                    var error = amazonS3exception.Message;
                    throw new Exception(error);
                }
                return awsRequestID;
        }

        //Move one file S3 to another S3
        public async Task<string> MoveDocumentAsync(string file)
        {
            string awsRequestID = string.Empty;

            string sourceObjectKey = file;  
            string destinationObjectKey = file;

            //Console.WriteLine($"Copying{sourceObjectKey} from {sourceBucketName} to");
            //Console.WriteLine($"{destinationBucketName} as {destinationObjectKey}");

            AmazonS3Client client = new AmazonS3Client();

            var request = new CopyObjectRequest
                {
                    SourceBucket = _awssourceBucketName,
                    SourceKey = sourceObjectKey,
                     DestinationBucket = _awsdestinationBucketName,
                     DestinationKey = destinationObjectKey,
                };

                var response = await client.CopyObjectAsync(request);
                awsRequestID = response.ResponseMetadata.RequestId.ToString();
            return awsRequestID;
        }

        //Move all files S3 to another S3
        public async Task<string> MoveAllDocumentAsync()
        {

            int counter = 0;

            AmazonS3Client client = new AmazonS3Client();

            ListObjectsRequest request = new ListObjectsRequest
                {
                      BucketName = _awssourceBucketName,
                };

                ListObjectsResponse response = await client.ListObjectsAsync(request);                

                foreach (S3Object obj in response.S3Objects)
                {
                    string result = await MoveDocumentAsync(obj.Key);
                    counter++;
                }

            return counter.ToString() + " files are moved";
        }



    }

}
