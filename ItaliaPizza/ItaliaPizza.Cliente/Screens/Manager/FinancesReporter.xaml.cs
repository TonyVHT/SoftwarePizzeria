using ItaliaPizza.Cliente.Helpers;
using ItaliaPizza.Cliente.Models;
using ItaliaPizza.Cliente.Singleton;
using ItaliaPizza.Cliente.UserControls;
using Newtonsoft.Json;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ItaliaPizza.Cliente.Screens.Manager
{
    public partial class FinancesReporter : Page
    {
        private readonly HttpClient _http = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7264/api/Finanza/")
        };

        public decimal TotalBalance { get; set; }

        public FinancesReporter()
        {
            InitializeComponent();
            string rol = UserSessionManager.Instance.GetRol()?.ToLower();

            switch (rol)
            {
                case "administrador":
                    MenuLateral.Content = new UCAdmin();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCAdmin, "Estadísticas");
                    break;
                
                case "gerente":
                    MenuLateral.Content = new UCManager();
                    CambiarBotonSeleccionado(MenuLateral.Content as UCManager, "Estadísticas");
                    break;
                default:
                    MessageBox.Show("Ocurrió un error, por favor inicie sesión nuevamente");
                    NavigationService.Navigate(new LogIn());
                    return;
            }
        }

        private void CambiarBotonSeleccionado(UserControl menuControl, string botonSeleccionado)
        {
            ButtonSelectionHelper.DesmarcarBotones(menuControl);
            ButtonSelectionHelper.MarcarBotonSeleccionado(menuControl, botonSeleccionado);
        }

        private async void BuscarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (FechaInicioDatePicker.SelectedDate is not DateTime fechaSeleccionada)
                {
                    MessageBox.Show("Por favor selecciona una fecha válida.");
                    return;
                }

                string fechaFormateada = fechaSeleccionada.ToString("yyyy-MM-dd");

                var response = await _http.GetAsync($"reporte/{fechaFormateada}");
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Error al generar el reporte: {response.StatusCode} ({response.ReasonPhrase})");
                    return;
                }

                var json = await response.Content.ReadAsStringAsync();
                var reporte = JsonConvert.DeserializeObject<List<FinanzaDTO>>(json);

                // Mostrar lista
                FinanzasListView.ItemsSource = reporte;

                // Totales
                decimal totalEntradas = reporte.Where(f => f.TipoTransaccion == "Entrada").Sum(f => f.Monto);
                decimal totalSalidas = reporte.Where(f => f.TipoTransaccion != "Entrada").Sum(f => f.Monto);
                TotalBalance = totalEntradas - totalSalidas;

                TotalEntradasTextBlock.Text = $"{totalEntradas:C}";
                TotalSalidasTextBlock.Text = $"{totalSalidas:C}";
                TotalBalanceTextBlock.Text = $"{TotalBalance:C}";

                // Permitir binding del fondo dinámico
                DataContext = this;

                // Mostrar gráficas
                GenerarGrafico(reporte);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el reporte: {ex.Message}");
            }
        }

        private void GenerarGrafico(List<FinanzaDTO> reporte)
        {
            var plotModelGanancias = new PlotModel { Title = "Ganancias" };
            var plotModelPerdidas = new PlotModel { Title = "Pérdidas" };
            var plotModelCombinado = new PlotModel { Title = "Ganancias y Pérdidas" };

            void AgregarEjes(PlotModel model)
            {
                model.Axes.Add(new DateTimeAxis
                {
                    Position = AxisPosition.Bottom,
                    StringFormat = "dd/MM",
                    Title = "Fecha",
                    IntervalType = DateTimeIntervalType.Days,
                    MinorIntervalType = DateTimeIntervalType.Days,
                    IsZoomEnabled = false,
                    IsPanEnabled = false
                });

                model.Axes.Add(new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Title = "Monto",
                    IsZoomEnabled = false,
                    IsPanEnabled = false
                });
            }

            AgregarEjes(plotModelGanancias);
            AgregarEjes(plotModelPerdidas);
            AgregarEjes(plotModelCombinado);

            var serieGanancias = new LineSeries
            {
                Title = "Ganancias",
                MarkerType = MarkerType.Circle,
                MarkerSize = 5,
                Color = OxyColors.Green
            };

            var seriePerdidas = new LineSeries
            {
                Title = "Pérdidas",
                MarkerType = MarkerType.Circle,
                MarkerSize = 5,
                Color = OxyColors.Red
            };

            foreach (var f in reporte)
            {
                var fecha = DateTimeAxis.ToDouble(f.Fecha);
                var monto = (double)f.Monto;

                if (f.TipoTransaccion == "Entrada")
                    serieGanancias.Points.Add(new DataPoint(fecha, monto));
                else
                    seriePerdidas.Points.Add(new DataPoint(fecha, monto));
            }

            plotModelGanancias.Series.Add(serieGanancias);
            plotModelPerdidas.Series.Add(seriePerdidas);

            var serieGananciasCombo = new LineSeries
            {
                Title = "Ganancias",
                MarkerType = MarkerType.Circle,
                MarkerSize = 5,
                Color = OxyColors.Green
            };
            serieGananciasCombo.Points.AddRange(serieGanancias.Points);

            var seriePerdidasCombo = new LineSeries
            {
                Title = "Pérdidas",
                MarkerType = MarkerType.Circle,
                MarkerSize = 5,
                Color = OxyColors.Red
            };
            seriePerdidasCombo.Points.AddRange(seriePerdidas.Points);

            plotModelCombinado.Series.Add(serieGananciasCombo);
            plotModelCombinado.Series.Add(seriePerdidasCombo);

            GraficoGanancias.Model = plotModelGanancias;
            GraficoPerdidas.Model = plotModelPerdidas;
            GraficoGananciasYPerdidas.Model = plotModelCombinado;
        }

        private async void DescargarReporteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (FechaInicioDatePicker.SelectedDate is not DateTime fechaSeleccionada)
                {
                    MessageBox.Show("Selecciona una fecha válida.");
                    return;
                }

                string fechaFormateada = fechaSeleccionada.ToString("yyyy-MM-dd");

                var response = await _http.GetAsync($"reporte/{fechaFormateada}");
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Error al obtener datos: {response.StatusCode}");
                    return;
                }

                var json = await response.Content.ReadAsStringAsync();
                var reporte = JsonConvert.DeserializeObject<List<FinanzaDTO>>(json);

                var saveDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "CSV Files (*.csv)|*.csv|PDF Files (*.pdf)|*.pdf",
                    FileName = $"Reporte_Finanzas_{fechaFormateada}"
                };

                if (saveDialog.ShowDialog() == true)
                {
                    if (saveDialog.FileName.EndsWith(".csv"))
                    {
                        string csv = ConvertToCsv(reporte);
                        File.WriteAllText(saveDialog.FileName, csv);
                        MessageBox.Show("Reporte CSV guardado correctamente.");
                    }
                    else if (saveDialog.FileName.EndsWith(".pdf"))
                    {
                        GenerarPDF(reporte, saveDialog.FileName);
                        MessageBox.Show("Reporte PDF guardado correctamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al descargar el reporte: {ex.Message}");
            }
        }

        private string ConvertToCsv(List<FinanzaDTO> reporte)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Id,TipoTransaccion,Concepto,Monto,Fecha,UsuarioId");

            foreach (var f in reporte)
            {
                sb.AppendLine($"{f.Id},{f.TipoTransaccion},{f.Concepto},{f.Monto},{f.Fecha:yyyy-MM-dd},{f.UsuarioId}");
            }

            return sb.ToString();
        }


        private void GenerarPDF(List<FinanzaDTO> reporte, string rutaArchivo)
        {
            using var doc = new PdfSharpCore.Pdf.PdfDocument();
            var page = doc.AddPage();
            var gfx = PdfSharpCore.Drawing.XGraphics.FromPdfPage(page);
            var font = new PdfSharpCore.Drawing.XFont("Arial", 12);
            double y = 40;

            gfx.DrawString("Reporte de Finanzas", new PdfSharpCore.Drawing.XFont("Arial", 16, PdfSharpCore.Drawing.XFontStyle.Bold),
                PdfSharpCore.Drawing.XBrushes.Black, new PdfSharpCore.Drawing.XPoint(40, 20));

            foreach (var f in reporte)
            {
                string linea = $"{(f.TipoTransaccion == "Entrada" ? "+" : "-")} {f.Concepto} | {f.Monto:C}";
                gfx.DrawString(linea, font, PdfSharpCore.Drawing.XBrushes.Black, new PdfSharpCore.Drawing.XPoint(40, y));
                y += 20;
                if (y > page.Height - 40)
                {
                    page = doc.AddPage();
                    gfx = PdfSharpCore.Drawing.XGraphics.FromPdfPage(page);
                    y = 40;
                }
            }

            using var stream = File.Create(rutaArchivo);
            doc.Save(stream);
        }


        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();

        }

    }
}
