using ItaliaPizza.Cliente.Screens.Admin;
using ItaliaPizza.Cliente.Screens;
using ItaliaPizza.Cliente.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ItaliaPizza.Cliente.UserControls
{
    /// <summary>
    /// Lógica de interacción para UCKitchenManager.xaml
    /// </summary>
    public partial class UCKitchenManager : UserControl
    {
        private NavigationService? Navigation => NavigationService.GetNavigationService(this);

        public UCKitchenManager()
        {
            InitializeComponent();
        }

        private void buttonSettings_Click(object sender, RoutedEventArgs e)
        {
            SessionManagerHelper.CerrarSesionUniversal();


        }

        private void GoToRecipeOptions(object sender, RoutedEventArgs e)
        {
            var recipeOptions = new RecipeOptions();
            Navigation?.Navigate(recipeOptions);
        }

        private void GoToProductsOptions(object sender, RoutedEventArgs e)
        {
            var productOptions = new SearchProduct();
            Navigation?.Navigate(productOptions);
        }

        private void GoToPedidosEnCocina(object sender, RoutedEventArgs e)
        {
            var pedidosEnCocina = new ItaliaPizza.Cliente.Screens.Orders.PedidosEnCocina();
            Navigation?.Navigate(pedidosEnCocina);
        }

        private void GoToHomePage(object sender, RoutedEventArgs e)
        {
            var homePage = new HomePageAdmin();
            Navigation?.Navigate(homePage);
        }
    }
}
