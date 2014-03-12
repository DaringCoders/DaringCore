using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace DaringCore.Azure
{
    /// <summary>
    /// Static wrapper for common Azure Blob Storage methods.
    /// 
    /// This class is thread safe so long as the connection string is not changed.
    /// 
    /// Ussage:
    ///     AzureBlob.ConnectionString = "YourAzureStorageAccountConnectionString";
    ///     bool exists = AzureBlob.BlobExists("mycontainer", "path/to/file.txt");
    /// </summary>
    public static class AzureBlob
    {
        public static CloudBlobClient BlobClient;

        /// <summary>
        /// Required
        /// </summary>
        public static string ConnectionString
        {
            set
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(value);
                BlobClient = storageAccount.CreateCloudBlobClient();
            }
        }

        public static bool BlobExists(string container, string path)
        {
            var blobContainer = BlobClient.GetContainerReference(container);
            var blob = blobContainer.GetBlockBlobReference(path);

            return blob.Exists();
        }

        public static List<String> ListAllBlobsInContainer(string container)
        {
            return ListBlobsInFolder(container, null);
        }

        public static List<String> ListBlobsInFolder(string container, string path)
        {
            var blobContainer = BlobClient.GetContainerReference(container);

            return blobContainer.ListBlobs(path).Select(b => b.Uri.ToString()).ToList();
        }

        public static bool DeleteBlob(string container, string file)
        {
            var blobContainer = BlobClient.GetContainerReference(container);
            var blob = blobContainer.GetBlockBlobReference(file);

            return blob.DeleteIfExists();
        }

        public static void DownloadBlobToStream(string container, string fileName, Stream toStream)
        {
            var blobContainer = BlobClient.GetContainerReference(container);
            var blob = blobContainer.GetBlockBlobReference(fileName);
            blob.DownloadToStream(toStream);
        }

        public static void UploadStream(string container, string fileName, Stream fileStream, string contentType)
        {
            var blobContainer = BlobClient.GetContainerReference(container);
            var blob = blobContainer.GetBlockBlobReference(fileName);

            blob.DeleteIfExists();

            fileStream.Seek(0, SeekOrigin.Begin);

            blob.UploadFromStream(fileStream);

            if (!String.IsNullOrWhiteSpace(contentType))
            {
                blob.Properties.ContentType = contentType;
            }

            blob.SetProperties();

            if (!blob.Exists())
            {
                throw new FileNotFoundException(string.Format("Blob was uploaded to {0} but couldn't be found after upload. URI: {1}", fileName, blob.Uri));
            }
        }

        public static void UploadText(string container, string fileName, string content, string contentType)
        {
            var blobContainer = BlobClient.GetContainerReference(container);
            var blob = blobContainer.GetBlockBlobReference(fileName);

            blob.DeleteIfExists();

            blob.UploadText(content);

            if (!String.IsNullOrWhiteSpace(contentType))
            {
                blob.Properties.ContentType = contentType;
            }

            blob.SetProperties();

            if (!blob.Exists())
            {
                throw new FileNotFoundException(string.Format("Blob was uploaded to {0} but couldn't be found after upload. URI: {1}", fileName, blob.Uri));
            }
        }
    }
}
