using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Collections.Generic;
using System.IO;
using ItaliaPizza.Cliente.Models;
using System;

namespace ItaliaPizza.Cliente.Utils
{
    public static class InventoryPdfExporter
    {
        public static void ExportToPdf(string filePath, List<ProductoInventarioDTO> productos)
        {
            var document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var titleFont = new XFont("Arial", 14, XFontStyle.Bold);
            var headerFont = new XFont("Arial", 10, XFontStyle.Bold);
            var rowFont = new XFont("Arial", 9);

            int y = 30;
            gfx.DrawString("Reporte de Inventario", titleFont, XBrushes.Black, new XRect(0, y, page.Width, 20), XStringFormats.TopCenter);
            y += 40;

            var green = new XSolidBrush(XColor.FromArgb(0x32, 0xD4, 0x83));

            string[] headers = { "Nombre", "Categoría", "Unidad", "Actual", "Mínima", "Precio", "Obs.", "Ingrediente", "Activo" };
            int[] colWidths = { 70, 60, 40, 40, 40, 50, 70, 40, 40 };
            int x = 20;

            // Draw headers
            for (int i = 0; i < headers.Length; i++)
            {
                gfx.DrawRectangle(green, x, y, colWidths[i], 20);
                gfx.DrawString(headers[i], headerFont, XBrushes.White, new XRect(x + 2, y + 2, colWidths[i], 20), XStringFormats.TopLeft);
                x += colWidths[i];
            }

            y += 25;

            foreach (var p in productos)
            {
                x = 20;
                string[] values = {
                    p.Nombre,
                    p.Categoria,
                    p.UnidadMedida,
                    p.CantidadActual.ToString("0.##"),
                    p.CantidadMinima.ToString("0.##"),
                    $"{p.Precio:C}",
                    p.ObservacionesInventario ?? "",
                    p.EsIngrediente ? "Sí" : "No",
                    p.Estatus ? "Sí" : "No"
                };

                for (int i = 0; i < values.Length; i++)
                {
                    gfx.DrawString(values[i], rowFont, XBrushes.Black, new XRect(x + 2, y, colWidths[i], 20), XStringFormats.TopLeft);
                    x += colWidths[i];
                }

                y += 22;

                if (y > page.Height - 40)
                {
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    y = 30;
                }
            }

            using var stream = File.Create(filePath);
            document.Save(stream);
        }
    }
}
