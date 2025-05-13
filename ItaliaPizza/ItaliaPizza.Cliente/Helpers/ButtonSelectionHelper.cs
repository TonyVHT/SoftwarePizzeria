using ItaliaPizza.Cliente.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ItaliaPizza.Cliente.Helpers
{
    public class ButtonSelectionHelper
    {

       

        public static void DesmarcarBotones(UserControl menuControl)
        {
            if (menuControl is UCManager ucManager)
            {
                ucManager.buttonOrder.Tag = "Inactive";
                ucManager.buttonInicio.Tag = "Inactive";
                ucManager.buttonRecipes.Tag = "Inactive";
                ucManager.buttonCustomers.Tag = "Inactive";
                ucManager.buttonAnalytics.Tag = "Inactive";
                ucManager.buttonProductos.Tag = "Inactive";
                ucManager.buttonProviders.Tag = "Inactive";
                ucManager.buttonSettings.Tag = "Inactive";
                ucManager.buttonUsuarios.Tag = "Inactive";

            }
            else if (menuControl is UCWaiter ucWaiter)
            {
                ucWaiter.buttonInicio.Tag = "Inactive";
                ucWaiter.buttonOrders.Tag = "Inactive";
                ucWaiter.buttonSettings.Tag = "Inactive";


            }
            else if (menuControl is UCCook ucCook)
            {
                ucCook.buttonInicio.Tag = "Inactive";
                //ucCook.buttonOrders.Tag = "Inactive";
                ucCook.buttonMenu.Tag = "Inactive";
                ucCook.buttonProducts.Tag = "Inactive";
                ucCook.buttonSettings.Tag = "Inactive";


            }
            else if (menuControl is UCAdmin ucAdmin)
            {
                ucAdmin.buttonInicio.Tag = "Inactive";
                ucAdmin.buttonUsuarios.Tag = "Inactive";
                ucAdmin.buttonMenu.Tag = "Inactive";
                ucAdmin.buttonCustomers.Tag = "Inactive";
                ucAdmin.buttonProductos.Tag = "Inactive";
                ucAdmin.buttonProveedores.Tag = "Inactive";
                ucAdmin.buttonAnalytics.Tag = "Inactive";
                ucAdmin.buttonSettings.Tag = "Inactive";

            }
            else if (menuControl is UCCashier uCCashier)
            {
                uCCashier.buttonInicio.Tag = "Inactive";
                uCCashier.buttonOrders.Tag = "Inactive";
                uCCashier.buttonCustomers.Tag = "Inactive";
                uCCashier.buttonProducts.Tag = "Inactive";
                uCCashier.buttonSettings.Tag = "Inactive";
            }
            else if (menuControl is UCDelivery uCDelivery) 
            {
                uCDelivery.buttonInicio.Tag = "Inactive";
                uCDelivery.buttonOrders.Tag = "Inactive";
                uCDelivery.buttonSettings.Tag = "Inactive";
            }
            else if (menuControl is UCKitchenManager uCKitchenManager)
            {
                uCKitchenManager.buttonInicio.Tag = "Inactive";
                uCKitchenManager.buttonOrders.Tag = "Inactive";
                uCKitchenManager.buttonMenu.Tag = "Inactive";
                uCKitchenManager.buttonProducts.Tag = "Inactive";
                uCKitchenManager.buttonSettings.Tag = "Inactive";
            }

        }

        public static void MarcarBotonSeleccionado(UserControl menuControl, string botonSeleccionado)
        {
            switch (botonSeleccionado)
            {
                case "Inicio":
                    if (menuControl is UCManager manager)
                        manager.buttonInicio.Tag = "Active";
                    else if (menuControl is UCWaiter waiter1)
                        waiter1.buttonInicio.Tag = "Active";
                    else if (menuControl is UCCook cook1)
                        cook1.buttonInicio.Tag = "Active";
                    else if (menuControl is UCAdmin admin1)
                        admin1.buttonInicio.Tag = "Active";
                    else if (menuControl is UCCashier cashier)
                        cashier.buttonInicio.Tag = "Active";
                    else if (menuControl is UCDelivery delivery)
                        delivery.buttonInicio.Tag = "Active";
                    else if (menuControl is UCKitchenManager kitchenManager)
                        kitchenManager.buttonInicio.Tag = "Active";
                    break;

                case "Pedidos":
                    if (menuControl is UCWaiter waiter)
                        waiter.buttonOrders.Tag = "Active";
                    else if (menuControl is UCCashier cashier)
                        cashier.buttonOrders.Tag = "Active";
                    else if (menuControl is UCKitchenManager kitchenManager)
                        kitchenManager.buttonOrders.Tag = "Active";
                    else if (menuControl is UCDelivery delivery)
                        delivery.buttonOrders.Tag = "Active";
                    break;

                case "Platillos":
                    if (menuControl is UCAdmin admin01)
                        admin01.buttonMenu.Tag = "Active";
                    else if (menuControl is UCManager manager1)
                        manager1.buttonRecipes.Tag = "Active";
                    else if (menuControl is UCCook cook)
                        cook.buttonMenu.Tag = "Active";
                    else if (menuControl is UCKitchenManager kitchenManager)
                        kitchenManager.buttonMenu.Tag = "Active";
                    break;

                case "Clientes":
                    if (menuControl is UCManager manager2)
                        manager2.buttonCustomers.Tag = "Active";
                    else if (menuControl is UCAdmin admin2)
                        admin2.buttonCustomers.Tag = "Active";
                    else if (menuControl is UCCashier cashier)
                        cashier.buttonCustomers.Tag = "Active";
                    break;

                case "Estadísticas":
                    if (menuControl is UCAdmin admin)
                        admin.buttonAnalytics.Tag = "Active";
                    else if (menuControl is UCManager manager3)
                        manager3.buttonAnalytics.Tag = "Active";
                    break;

                case "Productos":
                    if (menuControl is UCAdmin admin3)
                        admin3.buttonProductos.Tag = "Active";
                    else if (menuControl is UCManager manager4)
                        manager4.buttonProductos.Tag = "Active";
                    else if (menuControl is UCCashier cashier)
                        cashier.buttonProducts.Tag = "Active";
                    else if (menuControl is UCCook cook1)
                        cook1.buttonProducts.Tag = "Active";
                    else if (menuControl is UCKitchenManager kitchenManager)
                        kitchenManager.buttonProducts.Tag = "Active";
                    break;

                case "Proveedores":
                    if (menuControl is UCAdmin admin4)
                        admin4.buttonProveedores.Tag = "Active";
                    else if (menuControl is UCManager manager3)
                        manager3.buttonProviders.Tag = "Active";
                    break;

                case "Usuarios":
                    if (menuControl is UCAdmin admin5)
                        admin5.buttonUsuarios.Tag = "Active";
                    else if (menuControl is UCManager manager1)
                        manager1.buttonUsuarios.Tag = "Active";

                    break;
            }
        }
    }

}
