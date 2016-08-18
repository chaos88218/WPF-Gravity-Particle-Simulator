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
using System.Text.RegularExpressions;

namespace PSControl
{
    /// <summary>
    /// UserControl1.xaml 的互動邏輯
    /// </summary>
    public partial class SceneDLG : Window
    {
        public int enter_f_num = 0;
        public int enter_s_num = 0;


        //****//
        //Constructor
        //****//
        public SceneDLG()
        {
            InitializeComponent();
        }


        //****//
        //Control
        //****//
        private void Yes_button(object sender, RoutedEventArgs e)
        {
            string aaa = f_num_text.Text;
            if ((f_num_text.Text != "") && (s_num_text.Text != ""))
            {
                this.enter_f_num = int.Parse(f_num_text.Text);
                this.enter_s_num = int.Parse(s_num_text.Text);
                if (this.enter_f_num > 50) {
                    if (MessageBox.Show("More than 50 fields will cause massive computing.\n \'Yes\' to continue.\n\'No\' to change to smaller number?", "Fields", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        return;
                    }
                }
                if (this.enter_s_num > 5)
                {
                    if (MessageBox.Show("More than 5 sources will cause massive computing.\n \'Yes\' to continue.\n\'No\' to change to smaller number?", "Sources", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        return;
                    }
                }
                DialogResult = true;
            }
            else {
                warning_label.Content = "Invalid inputs.";
            }
        }


        //****//
        //Input Validation
        //****//
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex1 = new Regex(@"\d"); //[^a-zA-Z]
            Regex regex2 = new Regex(@"\."); //[^a-zA-Z]
            e.Handled = ((!regex1.IsMatch(e.Text)) || (regex2.IsMatch(e.Text)));
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Regex regex1 = new Regex(@"ImeProcessed"); //[^a-zA-Z]
            bool test = regex1.IsMatch(e.Key.ToString());
            if (test)
            {
                warning_label.Content = "Plz Switch to English IME.";
                e.Handled = true;
                return;
            }
            warning_label.Content = "";
        }

    }
}
