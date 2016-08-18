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
    /// Particles__info.xaml 的互動邏輯
    /// </summary>
    public partial class Particles__info : UserControl
    {
        public Particles__info(int n, float[] in_four_num, Brush color)
        {
            InitializeComponent();
            this.number_label.Content = n + 1;
            this.number_label.Background = color;
            this.v_label.Content = in_four_num[0];
            this.x_label.Content = in_four_num[1];
            this.y_label.Content = in_four_num[2];
            this.z_label.Content = in_four_num[3];
        }
    }
}
