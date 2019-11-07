using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using App.Domain.Interfaces.Services;

namespace App.Domain.Services
{
    public class FtpService : IFtpService
    {
        private readonly AppSettings _appSettings;

        public FtpService(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public Task UploadFile(string fileName, string copyFileFromPath)
        {
            try
            {
                // Get the object used to communicate with the server.
                var request = (FtpWebRequest)WebRequest.Create($"{_appSettings.FtpUrl}/{fileName}");

                request.Method = WebRequestMethods.Ftp.UploadFile;

                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential(_appSettings.FtpUser, _appSettings.FtpPassword);

                // Copy the contents of the file to the request stream.
                byte[] fileContents;
                using (StreamReader sourceStream = new StreamReader(copyFileFromPath))
                {
                    fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                }

                request.ContentLength = fileContents.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(fileContents, 0, fileContents.Length);
                }

                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                return Task.FromException(e);
            }
        }
    }
}
