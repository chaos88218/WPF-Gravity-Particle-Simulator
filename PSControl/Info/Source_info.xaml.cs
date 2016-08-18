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
    /// Source_info.xaml 的互動邏輯
    /// </summary>
    public partial class Source_info : UserControl
    {
        public Source_info(int n, int type, int[] two_num, float[] seven_float)
        {
            InitializeComponent();
            this.number_label.Content = n + 1;
            this.n_life_label.Content = String.Format("{0}/{1}", two_num[0]/100, two_num[1]/100);
            this.type_label.Content = type;
            this.scale_label.Content = seven_float[0];
            this.x_label.Content = seven_float[1];
            this.y_label.Content = seven_float[2];
            this.z_label.Content = seven_float[3];
            this.Vx_label.Content = seven_float[4];
            this.Vy_label.Content = seven_float[5];
            this.Vz_label.Content = seven_float[6];
        }
    }
}
