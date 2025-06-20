namespace Clinic.APIS.Helpers
{
    public static class PrescriptionPdfGenerator
    {
        public static byte[] GeneratePrescriptionPdf(Prescription prescription, string backgroundPath= "G:\\Advanced c#\\project\\Clinic_Mangement\\Clinic.APIS\\IMag\\WhatsApp Image 2025-05-05 at 21.15.52_243be64e.jpg", string logoPath= "G:\\Advanced c#\\project\\Clinic_Mangement\\Clinic.APIS\\IMag\\Logo.png")
        {
            using var document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            // Add background image
            if (File.Exists(backgroundPath))
            {
                using var bgStream = File.OpenRead(backgroundPath);
                var bgImage = XImage.FromStream(() => bgStream);
                gfx.DrawImage(bgImage, 0, 0, page.Width, page.Height);
            }

            // Add logo
            if (File.Exists(logoPath))
            {
                using var logoStream = File.OpenRead(logoPath);
                var logoImage = XImage.FromStream(() => logoStream);
                gfx.DrawImage(logoImage, 30, 30, 100, 100);
            }

            // Draw border
            gfx.DrawRectangle(new XPen(XColors.Black, 2), 20, 20, page.Width - 40, page.Height - 40);

            // Content
            var font = new XFont("Arial", 14);
            gfx.DrawString("Prescription", new XFont("Arial", 24, XFontStyle.Bold), XBrushes.Black, new XRect(0, 140, page.Width, 40), XStringFormats.TopCenter);

            gfx.DrawString($"Patient: {prescription.Patient?.FullName}", font, XBrushes.Black, new XPoint(50, 200));
            gfx.DrawString($"Doctor: {prescription.Vet?.Name}", font, XBrushes.Black, new XPoint(50, 230));
            gfx.DrawString($"Medication: {prescription.MedicationName}", font, XBrushes.Black, new XPoint(50, 260));
            gfx.DrawString($"Dosage: {prescription.Dosage}", font, XBrushes.Black, new XPoint(50, 290));
            gfx.DrawString($"Frequency: {prescription.Frequency}", font, XBrushes.Black, new XPoint(50, 320));
            gfx.DrawString($"Instructions: {prescription.Instructions}", font, XBrushes.Black, new XPoint(50, 350));
            gfx.DrawString($"Start: {prescription.StartDate:yyyy-MM-dd}", font, XBrushes.Black, new XPoint(50, 380));
            gfx.DrawString($"End: {prescription.EndDate:yyyy-MM-dd}", font, XBrushes.Black, new XPoint(50, 410));

            // Generate QR Code (as byte[] using PngByteQRCode to avoid System.Drawing)
            var qrText = $"Rx for {prescription.Patient?.FullName}: {prescription.MedicationName}";
            using var qrGen = new QRCodeGenerator();
            using var qrData = qrGen.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
            var pngQrCode = new PngByteQRCode(qrData);
            var qrBytes = pngQrCode.GetGraphic(20);

            using var qrStream = new MemoryStream(qrBytes);
            var qrImage = XImage.FromStream(() => qrStream);
            gfx.DrawImage(qrImage, 50, page.Height - 170, 120, 120);

            // Output PDF
            using var outputStream = new MemoryStream();
            document.Save(outputStream, false);
            return outputStream.ToArray();
        }
    }
}
