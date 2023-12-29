using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using BusinessLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class AmazonS3Service : IStorageService
    {
        private const string bucketName = "skillslab-project";
        private readonly string awsAccessKey;
        private readonly string awsSecretKey;

        IAmazonS3 client;

        public AmazonS3Service(NameValueCollection appSettings)
        {
            awsAccessKey = appSettings["awsAccessKey"];
            awsSecretKey = appSettings["awsSecretKey"];

            client = new AmazonS3Client(awsAccessKey, awsSecretKey, RegionEndpoint.USWest2);
        }

        public async Task<Stream> Get(string systemFileName)
        {
            GetObjectRequest request = new GetObjectRequest()
            {
                BucketName = bucketName,
                Key = systemFileName
            };

            GetObjectResponse response = await client.GetObjectAsync(request);

            return response.ResponseStream;
        }

        public async Task Put(Stream stream, string systemFileName)
        {
            PutObjectRequest request = new PutObjectRequest()
            {
                InputStream = stream,
                BucketName = bucketName,
                Key = systemFileName
            };

            PutObjectResponse response = await client.PutObjectAsync(request);
        }
    }
}
