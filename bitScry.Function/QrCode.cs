using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace bitScry.Function
{
    public static class QrCode
    {
        [FunctionName("QrCode")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // parse query parameter
            string data = req.Query["url"];

            if (!string.IsNullOrEmpty(data))
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);

                Bitmap qrCodeImage = qrCode.GetGraphic(20);

                log.Info("QR code image returned.");

                HttpResponseMessage response = new HttpResponseMessage();
                response.Content = new ByteArrayContent(ImageToByteArray(qrCodeImage));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

                return response;
            }
            else
            {
                log.Info("No data parameter provided.");
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        private static byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, ImageFormat.Png);
                return memoryStream.ToArray();
            }
        }
    }
}
