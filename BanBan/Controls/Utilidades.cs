using System.Windows.Controls;
using System.Windows.Media;

namespace BanBan.Controls
{
    //Clase solo para funciones comunes para varios formularios
    class Utilidades
    {
        //Instancia general para todos los formularios
        public static sBanBan sb = new sBanBan();
        //Para limpiar todos los textbox en una page o elemento "padre"
        public static void ClearTextboxes(Visual visual)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
            {
                Visual ctrl = (Visual)VisualTreeHelper.GetChild(visual, i);
                if (ctrl.GetType() == typeof(TextBox))
                {
                    TextBox tb = (TextBox)ctrl;
                    tb.Clear();
                }
                ClearTextboxes(ctrl);
            }
        }
    }
}
