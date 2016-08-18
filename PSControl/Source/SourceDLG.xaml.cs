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
    /// SourceDLG.xaml 的互動邏輯
    /// </summary>
    public partial class SourceDLG : Window
    {
        int num = 0;
        public SourceDLG(int n)
        {
            num = n;
            InitializeComponent();
        }

        public void add_child(SourceDLG_cell s_dlg_cell)
        {
            this.SourceList.Items.Add(s_dlg_cell);
        }

        private void Yes_button(object sender, RoutedEventArgs e)
        {
            bool y = true;
            for (int i = 0; i < this.num; i++)
            {
                SourceDLG_cell temp = (SourceDLG_cell)this.SourceList.Items[i];
                temp.all_text_get_num();
                if (temp.two_num[0] > 500)
                {
                    if (MessageBox.Show("More than 500 particles will cause massive computing.\n \'Yes\' to continue.\n\'No\' to change to smaller number?", "Fields", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        return;
                    }
                }
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
