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

namespace PSControl
{
    /// <summary>
    /// Field_info.xaml 的互動邏輯
    /// </summary>
    public partial class Field_info : UserControl
    {
        public Field_info(int n, bool in_push, bool flat_or_n, float[] in_four_num)
        {
            InitializeComponent();
            this.number_label.Content = n + 1;
            this.g_mag_label.Content = in_four_num[0];
            this.x_label.Content = in_four_num[1];
            this.y_label.Content = in_four_num[2];
            this.z_label.Content = in_four_num[3];

            if (flat_or_n)
            {
                this.type_label.Content = "Gravity";
                this.x_or_ax_label.Content = "ax:";
                this.y_or_ay_label.Content = "ax:";
                this.z_or_az_label.Content = "ax:";
            }
            else if (in_push)
            {
                this.type_label.Content = "Push";
            }
            else
            {
                this.type_label.Content = "Pull";
            }
        }
    }
}
