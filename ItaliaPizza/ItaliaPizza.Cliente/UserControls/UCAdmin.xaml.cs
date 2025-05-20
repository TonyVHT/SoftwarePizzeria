using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ItaliaPizza.Cliente.Screens.Admin;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ItaliaPizza.Cliente.Utils;
using ItaliaPizza.Cliente.Screens;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ItaliaPizza.Cliente.UserControls
{
    /// <summary>
    /// Lógica de interacción para UCManager.xaml
    /// </summary>
    public partial class UCAdmin : UserControl
    {
        private NavigationService? Navigation => NavigationService.GetNavigationService(this);

        public UCAdmin()
        {
            InitializeComponent();
        }


        

        private void buttonSettings_Click(object sender, RoutedEventArgs e)
        {
            SessionManagerHelper.CerrarSesionUniversal();
            
            
        }

        private void GoToAnalyticsOptions(object sender, RoutedEventArgs e)
        {
            var analyticsOptions = new AnalyticsOptions();
            Navigation?.Navigate(analyticsOptions);
        }

        private void GoToUserOptions(object sender, RoutedEventArgs e)
        {
            var userOptions = new UserOptions();
            Navigation?.Navigate(userOptions);
        }

        private void GoToProductsOptions(object sender, RoutedEventArgs e)
        {
            var productOptions = new SearchProduct();
            Navigation?.Navigate(productOptions);
        }

        private void GoToProviderOptions(object sender, RoutedEventArgs e)
        {
            var providerOptions = new ProviderOptions();
            Navigation?.Navigate(providerOptions);
        }


        private void GoToHomePage(object sender, RoutedEventArgs e)
        {
            var homePage = new HomePageAdmin();
            Navigation?.Navigate(homePage);
        }

        private void GoToRecipeOptions(object sender, RoutedEventArgs e)
        {
            var recipeOptions = new RecipeOptions();
            Navigation?.Navigate(recipeOptions);
        }

        private void GoToCustomerOptions(object sender, RoutedEventArgs e)
        {
            var customerOptions = new CustomerOptiones();
            Navigation?.Navigate(customerOptions);
        }
    }
}
