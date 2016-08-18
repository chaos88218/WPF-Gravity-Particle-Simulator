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
using System.Windows.Shapes;

namespace PSControl
{
    /// <summary>
    /// FieldDLG.xaml 的互動邏輯
    /// </summary>
    public partial class FieldDLG : Window
    {
        int num = 0;
        public FieldDLG(int n)
        {
            num = n;
            InitializeComponent();
        }

        public void add_child(FieldDLG_cell f_dlg_cell)
        {
            this.FieldList.Items.Add(f_dlg_cell);
        }

        private void Yes_button(object sender, RoutedEventArgs e)
        {
            bool y = true;
            for (int i = 0; i < this.num; i++)
            {
                FieldDLG_cell temp = (FieldDLG_cell)this.FieldList.Items[i];
                y = y && temp.check_all_text_not_empty();
            }
            if (y)
            { DialogResult = true; }
            else
            {
                MessageBox.Show("Got empty informationd", "Source", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
