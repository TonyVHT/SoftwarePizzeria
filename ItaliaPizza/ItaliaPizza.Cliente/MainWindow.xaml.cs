using ItaliaPizza.Cliente.Screens;
using ItaliaPizza.Cliente.Screens.Admin;
using ItaliaPizza.Cliente.Screens.Cashier;
using ItaliaPizza.Cliente.Screens;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ItaliaPizza.Cliente;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
<<<<<<< HEAD
        /*RegisterOrder registerOrder = new RegisterOrder();
        registerOrder.Show();*/
=======
        
>>>>>>> main
        InitializeComponent();
        MainFrame.Navigate(new HomePageAdmin());
    }

    public void SetVista(UserControl control)
    {
        VistaContenedor.Content = control;
    }
}