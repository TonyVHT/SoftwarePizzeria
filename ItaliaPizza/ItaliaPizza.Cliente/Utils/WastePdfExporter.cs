using ItaliaPizza.Cliente.Models;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Collections.Generic;
using System.IO;

namespace ItaliaPizza.Cliente.Utils
{
    public static class WastePdfExporter
    {
        public static void ExportToPdf(string path, List<MermaDto> mermas)
        {
            var document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var font = new XFont("Arial", 10);
            var boldFont = new XFont("Arial", 10, XFontStyle.Bold);
            var titleFont = new XFont("Arial", 14, XFontStyle.Bold);
            var greenBrush = new XSolidBrush(XColor.FromArgb(0x32, 0xd4, 0x83)); // #32d483

            int y = 30;
            gfx.DrawString("Reporte de Mermas", titleFont, greenBrush, new XRect(0, y, page.Width, 20), XStringFormats.TopCenter);
            y += 40;

            // Encabezados
            gfx.DrawString("Producto", boldFont, XBrushes.Black, new XRect(40, y, 100, 20), XStringFormats.TopLeft);
            gfx.DrawString("Cantidad", boldFont, XBrushes.Black, new XRect(150, y, 60, 20), XStringFormats.TopLeft);
            gfx.DrawString("Usuario", boldFont, XBrushes.Black, new XRect(220, y, 120, 20), XStringFormats.TopLeft);
            gfx.DrawString("Fecha", boldFont, XBrushes.Black, new XRect(360, y, 120, 20), XStringFormats.TopLeft);
            y += 25;

            foreach (var m in mermas)
            {
                gfx.DrawString(m.Producto, font, XBrushes.Black, new XRect(40, y, 100, 20), XStringFormats.TopLeft);
                gfx.DrawString(m.CantidadPerdida.ToString(), font, XBrushes.Black, new XRect(150, y, 60, 20), XStringFormats.TopLeft);
                gfx.DrawString(m.Usuario, font, XBrushes.Black, new XRect(220, y, 120, 20), XStringFormats.TopLeft);
                gfx.DrawString(m.Fecha.ToString("g"), font, XBrushes.Black, new XRect(360, y, 120, 20), XStringFormats.TopLeft);
                y += 20;

                if (y > page.Height - 50)
                {
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    y = 30;
                }
            }

            using var stream = File.Create(path);
            document.Save(stream);
        }
    }
}
