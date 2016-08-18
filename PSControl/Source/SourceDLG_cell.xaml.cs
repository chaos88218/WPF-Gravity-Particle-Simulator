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
    /// SourceDLG_cell.xaml 的互動邏輯
    /// </summary>
    public partial class SourceDLG_cell : UserControl
    {
        public int s_type = 0;
        public int[] two_num;
        public float[] seven_num;

        public SourceDLG_cell(int n, int in_type, int[] two_int, float[] seven_float)
        {
            InitializeComponent();

            this.s_type = in_type;
            this.number_label.Content = n + 1;
            this.two_num = two_int;
            this.seven_num = seven_float;
        }
        //****//
        //fuctions
        //****//
        public void all_set_in_text()
        {
            numbers_text.Text = this.two_num[0].ToString();
            life_text.Text = this.two_num[1].ToString();
            type_combo_box.SelectedIndex = this.s_type;

            scale_text.Text = this.seven_num[0].ToString();
            posiX_text.Text = this.seven_num[1].ToString();
            posiY_text.Text = this.seven_num[2].ToString();
            posiZ_text.Text = this.seven_num[3].ToString();
            veloX_text.Text = this.seven_num[4].ToString();
            veloY_text.Text = this.seven_num[5].ToString();
            veloZ_text.Text = this.seven_num[6].ToString();
        }

        public void all_text_get_num()
        {
            this.s_type = type_combo_box.SelectedIndex;
            this.two_num = new int[] { int.Parse(numbers_text.Text), int.Parse(life_text.Text) };
            this.seven_num = new float[]{ float.Parse( scale_text.Text)
                , float.Parse(posiX_text.Text), float.Parse(posiY_text.Text), float.Parse(posiZ_text.Text)
                , float.Parse(veloX_text.Text), float.Parse(veloY_text.Text), float.Parse(veloZ_text.Text)};
        }

        public bool check_all_text_not_empty()
        {

            bool y = (numbers_text.Text != "")
                && (life_text.Text != "")
            && (scale_text.Text != "")
            && (posiX_text.Text != "")
            && (posiY_text.Text != "")
            && (posiZ_text.Text != "")
            && (veloX_text.Text != "")
            && (veloY_text.Text != "")
            && (veloZ_text.Text != "");


            return y;
        }

        //****//
        //Input Validation
        //****//
        private void intValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex1 = new Regex(@"\d"); //[^a-zA-Z]
            Regex regex2 = new Regex(@"\."); //[^a-zA-Z]
            e.Handled = ((!regex1.IsMatch(e.Text)) || (regex2.IsMatch(e.Text)));
        }

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
    }
}
