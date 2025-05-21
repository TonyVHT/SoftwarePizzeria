using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ItaliaPizza.Cliente
{
    public class BaseWindow : Window
    {
        public BaseWindow()
        {
            // Nos suscribimos al evento StateChanged para detectar cuando el estado de la ventana cambia
            this.StateChanged += Window_StateChanged;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            // Si la ventana no está maximizada, la forzamos a maximizarse
            if (this.WindowState != WindowState.Maximized)
            {
                this.WindowState = WindowState.Maximized;
            }
        }
    }

}
