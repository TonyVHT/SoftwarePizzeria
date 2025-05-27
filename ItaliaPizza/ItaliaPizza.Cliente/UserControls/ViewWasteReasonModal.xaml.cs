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
    /// Interaction logic for ViewWasteReasonModal.xaml
    /// </summary>
    public partial class ViewWasteReasonModal : UserControl
    {
        public ViewWasteReasonModal()
        {
            InitializeComponent();
        }

        public void SetMotivo(string motivo)
        {
            TxtMotivo.Text = motivo;
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            (this.Parent as Border).Visibility = Visibility.Collapsed;
        }
    }

}
