using Fakelaki.Api.Lib.Services.Interfaces;
using QRCoder;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Fakelaki.Api.Lib.Services.Implementation
{
    public class QRCoderService : IQRCoderService
    {
        public string Create(string code, string logo = null)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);


            // This option will print a logo in the middle of the bar code
            Bitmap qrCodeImage =  string.IsNullOrWhiteSpace(logo) ? qrCode.GetGraphic(20) : qrCode.GetGraphic(20, Color.Black, Color.White, (Bitmap)Bitmap.FromFile("E:\\logo.png"));

            MemoryStream memoryStream = new MemoryStream();
            qrCodeImage.Save(memoryStream, ImageFormat.Png);

            // Converting to base64
            memoryStream.Position = 0;
            byte[] byteBuffer = memoryStream.ToArray();

            memoryStream.Close();

            string base64String = Convert.ToBase64String(byteBuffer);
            byteBuffer = null;

            // Display the barcode in image
            return base64String;
        }
    }
}
