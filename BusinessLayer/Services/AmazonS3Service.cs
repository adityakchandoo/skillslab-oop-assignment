using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using BusinessLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class AmazonS3Service : IStorageService
    {
        const string bucketName = "skillslab-project";
        const string awsAccessKey = "AKIAYEKFVLAJIBZDJX7G";
        const string awsSecretKey = "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";

        IAmazonS3 client;

        public AmazonS3Service()
        {
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
