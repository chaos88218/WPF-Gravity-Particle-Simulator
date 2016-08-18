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
    /// FieldDLG_cell.xaml 的互動邏輯
    /// </summary>
    public partial class FieldDLG_cell : UserControl
    {
        public bool flat = false;
        public bool push = false;
        public float[] four_num;

        public FieldDLG_cell(int n, bool in_push, bool flat_or_n, float[] in_four_num)
        {
            InitializeComponent();
            this.flat = flat_or_n;
            this.number_label.Content = n + 1;
            this.push = in_push;
            this.four_num = in_four_num;
        }

        private void push_checkbox_Checked(object sender, RoutedEventArgs e)
        {
            this.push = true;
        }

        private void push_checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.push = false;
        }
        //****//
        //fuctions
        //****//
        public void all_set_in_text()
        {
            g_text.Text = this.four_num[0].ToString();

            push_checkbox.IsChecked = this.push;
            flat_checkbox.IsChecked = this.flat;

            posiX_text.Text = this.four_num[1].ToString();
            posiY_text.Text = this.four_num[2].ToString();
            posiZ_text.Text = this.four_num[3].ToString();
        }

        public void all_text_get_num()
        {
            this.push = push_checkbox.IsChecked.Value;
            this.flat = flat_checkbox.IsChecked.Value;

            this.four_num = new float[]{ float.Parse(g_text.Text)
                , float.Parse(posiX_text.Text), float.Parse(posiY_text.Text), float.Parse(posiZ_text.Text)};
        }

        public bool check_all_text_not_empty()
        {

            bool y = (g_text.Text != "")
            && (posiX_text.Text != "")
            && (posiY_text.Text != "")
            && (posiZ_text.Text != "");

            return y;
        }

        //****//
        //Input Validation
        //****//
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex1 = new Regex(@"\d"); //[^a-zA-Z]
            Regex regex2 = new Regex(@"\."); //[^a-zA-Z]
            Regex regex3 = new Regex(@"-"); //[^a-zA-Z]
            e.Handled = ((!regex1.IsMatch(e.Text)) && (!regex2.IsMatch(e.Text)) && (!regex3.IsMatch(e.Text)));
            return;
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Regex regex1 = new Regex(@"ImeProcessed"); //[^a-zA-Z]
            bool test = regex1.IsMatch(e.Key.ToString());
            if (test)
            {
                MessageBox.Show("Plz Switch to English IME.", "Key in", MessageBoxButton.OK, MessageBoxImage.Warning);
                e.Handled = true;
                return;
            }
        }

        private void flat_checkbox_Checked(object sender, RoutedEventArgs e)
        {
            this.flat = true;
            push_checkbox.IsChecked = false;
            x_label.Content = "ax:";
            y_label.Content = "ay:";
            z_label.Content = "az:";

            push_checkbox.IsHitTestVisible = false;
        }

        private void flat_checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.flat = false;
            x_label.Content = "X:";
            y_label.Content = "Y:";
            z_label.Content = "Z:";
            push_checkbox.IsHitTestVisible = true;
        }
    }
}
