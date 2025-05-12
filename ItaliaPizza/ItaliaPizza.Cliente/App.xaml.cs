using ItaliaPizza.Cliente.Platillos.Screens;
using ItaliaPizza.Cliente.PlatillosModulo.Screens;
using System.Configuration;
using System.Data;
using System.Windows;

namespace ItaliaPizza.Cliente;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Aquí decides qué ventana probar
        var ventana = new BuscarPlatillosScreen();
        ventana.Show();
    }
}

