using ItaliaPizza.Cliente.Platillos.Screens;
using ItaliaPizza.Cliente.PlatillosModulo.Screens;
using ItaliaPizza.Cliente.Screens;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;

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
        var ventana = new LogIn();
        ventana.Show();
    }

    private void App_Startup(object sender, StartupEventArgs e)
    {
        // Suscribirse al evento de cada ventana que se cree
        this.DispatcherUnhandledException += App_DispatcherUnhandledException;
    }

    // Evento para manejar cuando el estado de la ventana cambia
    private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        if (e.Exception is InvalidOperationException && e.Exception.Message.Contains("Window cannot be made visible"))
        {
            MessageBox.Show("Error: Window cannot be made visible");
            e.Handled = true;
        }
    }

    public static void MaintainMaximized(Window window)
    {
        window.StateChanged += (sender, args) =>
        {
            if (window.WindowState != WindowState.Maximized)
            {
                window.WindowState = WindowState.Maximized;
            }
        };
    }
}

