using Base.Application.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;

namespace Base.Infrastructure.Implementation.MinIoService
{
    public class MinIoService : IMinIoService
    {
        private const string BucketName = "volunteertaskmanagement";
        private readonly IMinioClient _minio;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<MinIoService> _logger;

        public MinIoService(
            IMinioClient minio,
            IHttpClientFactory httpClientFactory,
            IWebHostEnvironment hostingEnvironment,
            ILogger<MinIoService> logger)
        {
            _minio = minio;
            _httpClientFactory = httpClientFactory;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        public async Task<bool> UploadFile(IFormFile file, string folderPath)
        {
            if (file == null || file.Length == 0)
                return false;

            if (file.Length > 100 * 1024 * 1024) // 100MB limit
                return false;

            try
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                var bucketExists = await _minio.BucketExistsAsync(
                    new BucketExistsArgs().WithBucket(BucketName));

                if (!bucketExists)
                {
                    await _minio.MakeBucketAsync(
                        new MakeBucketArgs().WithBucket(BucketName));
                }

                var putArgs = new PutObjectArgs()
                    .WithBucket(BucketName)
                    .WithObject($"{folderPath}/{Path.GetFileName(file.FileName)}")
                    .WithStreamData(memoryStream)
                    .WithObjectSize(memoryStream.Length)
                    .WithContentType(file.ContentType);

                await _minio.PutObjectAsync(putArgs);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file to MinIO");
                return false;
            }
        }

        public async Task<string> GetDownloadUrl(string fileName, string folderPath)
        {
            try
            {
                return await _minio.PresignedGetObjectAsync(new PresignedGetObjectArgs()
                    .WithBucket(BucketName)
                    .WithObject($"{folderPath}/{fileName}")
                    .WithExpiry(3600)); // 1 hour expiry
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating presigned URL", ex);
            }
        }
    }
}