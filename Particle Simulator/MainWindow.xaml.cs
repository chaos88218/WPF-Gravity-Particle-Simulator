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
using System.Windows.Media.Media3D;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading;
using System.IO;
using Microsoft.Win32;

using HelixToolkit.Wpf;
using PSControl;

namespace WPF_ParticleSimulator
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow
    {
        //timer
        DispatcherTimer timer;
        public static float d_timer = 0.02f;
        bool timer_up = false;
        long last_time = 0;

        //numbers
        int s_num = 2;
        int f_num = 2;

        //setting check;
        bool set_scene = false;
        bool set_fields = false;
        bool set_source = false;
        bool set_show = false;

        //Object
        Gravity_Caculator karma;
        Source_info[] s_i;
        Field_info[] f_i;
        Particles__info[][] p_i;

        public MainWindow()
        {
            this.s_num = 0;
            this.f_num = 0;

            this.InitializeComponent();

            //Drawings
            set_scene_child();

        }

        //****//
        //Functions
        //****//

        public void set_info_stack_p()
        {

            this.particles_info_stack.Items.Clear();
            this.p_i = new Particles__info[this.s_num][];

            for (int h = 0; h < s_num; h++)
            {
                Label nlabel = new Label();
                nlabel.Content = String.Format("Source {0}", h);
                this.particles_info_stack.Items.Add(nlabel);
                this.p_i[h] = new Particles__info[karma.sources[h].get_particle_nums()];
                for (int i = 0; i < karma.sources[h].get_particle_nums(); i += 2)
                {
                    if (this.karma.sources[h].particles[i].get_launched())
                    {
                        float[] s_posi = this.karma.sources[h].particles[i].getObjectPosition();
                        float s_velo = this.karma.sources[h].particles[i].getObjectABSVelocity();
                        Brush bb = this.karma.sources[h].particles[i].Drawing.Fill;
                        this.p_i[h][i] = new Particles__info(i, new float[] { s_velo, s_posi[0], s_posi[1], s_posi[2] }, bb);
                        this.particles_info_stack.Items.Add(this.p_i[h][i]);
                    }
                }
            }
        }

        public void set_info_stack_s()
        {

            this.source_info_stack.Items.Clear();
            this.s_i = new Source_info[this.s_num];
            for (int h = 0; h < s_num; h++)
            {
                float[] s_posi = this.karma.sources[h].getObjectPosition();
                float[] s_velo = this.karma.sources[h].getObjectVelocity();
                this.s_i[h] = new Source_info(h, this.karma.sources[h].get_type()
                    , new int[] { this.karma.sources[h].get_particle_nums(), this.karma.sources[h].get_particle_life() }
                    , new float[] { this.karma.sources[h].get_velo_scale()
                            ,s_posi[0] ,s_posi[1] ,s_posi[2]
                            ,s_velo[0]/this.karma.sources[h].get_velo_scale() 
                            ,s_velo[1]/this.karma.sources[h].get_velo_scale() 
                            ,s_velo[2]/this.karma.sources[h].get_velo_scale()});
                this.source_info_stack.Items.Add(this.s_i[h]);
            }
        }

        public void set_info_stack_f()
        {
            Thread.Sleep(500);
            this.field_info_stack.Items.Clear();
            this.f_i = new Field_info[this.f_num];
            for (int h = 0; h < f_num; h++)
            {
                float[] s_posi = this.karma.field[h].getObjectPosition();
                float[] s_velo = this.karma.field[h].getObjectVelocity();
                this.f_i[h] = new Field_info(h
                        , this.karma.field[h].get_push_or_not()
                        , this.karma.field[h].get_infi_or_not()
                        , new float[] { this.karma.field[h].get_G_para()
                            ,s_posi[0] ,s_posi[1] ,s_posi[2]});
                this.field_info_stack.Items.Add(this.f_i[h]);
            }
        }


        private void set_scene_child()
        {
            var lights = new DefaultLights();
            myHelixViewport3D.Children.Add(lights);

            for (int h = 0; h < f_num; h++)
            {
                myHelixViewport3D.Children.Add(karma.field[h].Drawing);
            }

            for (int h = 0; h < s_num; h++)
            {
                myHelixViewport3D.Children.Add(karma.sources[h].Drawing);

                for (int i = 0; i < karma.sources[h].get_particle_nums(); i++)
                {
                    myHelixViewport3D.Children.Add(karma.sources[h].particles[i].Drawing);
                }
            }

            myHelixViewport3D.ResetCamera();
        }

        private void timer_tick(object sender, EventArgs e)
        {
            karma.NextFrame();
            long ab = DateTime.Now.Ticks/10000;
            Status_bar.Text = String.Format("FPS : {0:F2}", 1000.0/(ab - last_time));
            last_time = ab;
        }

        //****//
        //Animation
        //****//
        private void Show_button(object sender, RoutedEventArgs e)
        {
            if ((!set_fields || !set_source || !set_scene))
            {
                MessageBox.Show("Setting not complete!", "Show", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            set_scene_child();
            set_scene = false;
            set_show = true;
        }

        private void Run_button(object sender, RoutedEventArgs e)
        {
            if (!set_show)
            {
                MessageBox.Show("Need to \'Show\' all Object first!", "Run", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!timer_up)
            {
                timer = new DispatcherTimer(DispatcherPriority.Render);
                int temp = (int)(1.0 / d_timer);
                timer.Interval = new TimeSpan(0, 0, 0, 0, temp);
                timer.Tick += new EventHandler(timer_tick);
                timer.Start();
                this.particles_info_stack.Items.Clear();
                run_button.Content = "Stop";
                timer_up = true;
            }
            else
            {
                timer.Stop();
                run_button.Content = "Run";
                set_info_stack_p();
                timer_up = false;
            }
        }

        private void Reset_button(object sender, RoutedEventArgs e)
        {
            if (!set_show)
            {
                return;
            }

            float[] zero = new float[] { 0, 0, 0 };
            for (int i = 0; i < s_num; i++)
            {
                //
                for (int j = 0; j < karma.sources[i].get_particle_nums(); j++)
                {
                    karma.sources[i].particles[j].set_launched(false);
                    karma.sources[i].particles[j].setObjectPosition(karma.sources[i].getObjectPosition());
                    karma.sources[i].particles[j].setObjectVelocity(zero);
                    karma.sources[i].particles[j].set_Acceleration(zero);
                }
            }
        }

        //****//
        //Create
        //****//
        private void New_button(object sender, RoutedEventArgs e)
        {
            SceneDLG newScene = new SceneDLG();
            newScene.Owner = this;

            bool? dialogResult = newScene.ShowDialog();

            if (dialogResult != null && (bool)dialogResult == true)
            {
                this.f_num = newScene.enter_f_num;
                this.s_num = newScene.enter_s_num;
                karma = new Gravity_Caculator(s_num, f_num);
                myHelixViewport3D.Children.Clear();

                set_scene = true;

                set_fields = false;
                set_source = false;
                set_show = false;

                if (timer_up)
                {
                    timer.Stop();
                    run_button.Content = "Run";
                    timer_up = false;
                }
            }
        }

        //****//
        //Object
        //****//
        private void Field_button(object sender, RoutedEventArgs e)
        {
            if (this.f_num != 0)
            {
                float[] s_posi;

                FieldDLG fld_dlg = new FieldDLG(this.f_num);
                fld_dlg.Owner = this;
                FieldDLG_cell[] fld_dlg_cells = new FieldDLG_cell[this.f_num];

                for (int i = 0; i < this.f_num; i++)
                {
                    s_posi = this.karma.field[i].getObjectPosition();

                    fld_dlg_cells[i] = new FieldDLG_cell(i
                        , this.karma.field[i].get_push_or_not()
                        , this.karma.field[i].get_infi_or_not()
                        , new float[] { this.karma.field[i].get_G_para()
                            ,s_posi[0] ,s_posi[1] ,s_posi[2]});

                    fld_dlg_cells[i].all_set_in_text();
                    fld_dlg.add_child(fld_dlg_cells[i]);
                }

                bool? dialogResult = fld_dlg.ShowDialog();

                if (dialogResult != null && (bool)dialogResult == true)
                {
                    float[] four_num = new float[4];

                    for (int i = 0; i < this.f_num; i++)
                    {
                        fld_dlg_cells[i].all_text_get_num();

                        four_num = fld_dlg_cells[i].four_num;
                        karma.field[i].setObjectPosition(new float[] { four_num[1], four_num[2], four_num[3] });
                        karma.field[i].set_G_para(four_num[0]);
                        karma.field[i].set_push_or_not(fld_dlg_cells[i].push);
                        karma.field[i].set_Infi_or_not(fld_dlg_cells[i].flat);

                    }
                }
            }
            else
            {
                MessageBox.Show("No Field!", "Field", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            set_fields = true;
            set_info_stack_f();
        }

        private void Source_button(object sender, RoutedEventArgs e)
        {
            if (this.s_num != 0)
            {
                float[] s_posi;
                float[] s_velo;

                SourceDLG src_dlg = new SourceDLG(this.s_num);
                src_dlg.Owner = this;
                SourceDLG_cell[] src_dlg_cells = new SourceDLG_cell[this.s_num];
                for (int i = 0; i < this.s_num; i++)
                {
                    s_posi = this.karma.sources[i].getObjectPosition();
                    s_velo = this.karma.sources[i].get_lauching_Velocity();
                    src_dlg_cells[i] = new SourceDLG_cell(i, this.karma.sources[i].get_type()
                        , new int[] { this.karma.sources[i].get_particle_nums(), this.karma.sources[i].get_particle_life() }
                        , new float[] { this.karma.sources[i].get_velo_scale()
                            ,s_posi[0] ,s_posi[1] ,s_posi[2]
                            ,s_velo[0]/this.karma.sources[i].get_velo_scale() ,s_velo[1]/this.karma.sources[i].get_velo_scale() ,s_velo[2]/this.karma.sources[i].get_velo_scale()});

                    src_dlg_cells[i].all_set_in_text();
                    src_dlg.add_child(src_dlg_cells[i]);
                }

                bool? dialogResult = src_dlg.ShowDialog();

                if (dialogResult != null && (bool)dialogResult == true)
                {
                    int[] two_num = new int[2];
                    float[] seven_num = new float[7];

                    for (int i = 0; i < this.s_num; i++)
                    {
                        src_dlg_cells[i].all_text_get_num();
                        two_num = src_dlg_cells[i].two_num;
                        seven_num = src_dlg_cells[i].seven_num;

                        if (two_num[0] != this.karma.sources[i].get_particle_nums())
                        {
                            for (int j = 0; j < this.karma.sources[i].get_particle_nums(); j++)
                            {
                                myHelixViewport3D.Children.Remove(this.karma.sources[i].particles[j].Drawing);
                            }
                            karma.sources[i].set_particle_numbers(two_num[0]);
                            if (set_show)
                            {
                                for (int j = 0; j < this.karma.sources[i].get_particle_nums(); j++)
                                {
                                    myHelixViewport3D.Children.Add(this.karma.sources[i].particles[j].Drawing);
                                }
                            }
                        }
                        karma.sources[i].set_particles_life(two_num[1]);
                        karma.sources[i].set_Source_type(src_dlg_cells[i].s_type);
                        karma.sources[i].setObjectPosition(new float[] { seven_num[1], seven_num[2], seven_num[3] });
                        karma.sources[i].set_Velo_scale(seven_num[0]);
                        karma.sources[i].set_Velo_vec(new float[] { seven_num[4], seven_num[5], seven_num[6] });
                    }
                }
            }
            else
            {
                MessageBox.Show("No Sources!", "Source", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            set_source = true;
            set_info_stack_s();
        }


        //****//
        //Examples
        //****//
        private void karma_ex_Click(object sender, RoutedEventArgs e)
        {
            this.s_num = 1;
            this.f_num = 2;

            karma = new Gravity_Caculator(s_num, f_num);
            myHelixViewport3D.Children.Clear();
            set_scene = true;

            karma.sources[0].set_particle_numbers(500);
            karma.sources[0].set_particles_life(500);
            karma.sources[0].set_Source_type(0);
            karma.sources[0].setObjectPosition(new float[] { -50, 0, 0 });
            karma.sources[0].set_Velo_scale(0.65f);
            karma.sources[0].set_Velo_vec(new float[] { 50, 0, 0 });
            set_source = true;
            set_info_stack_s();

            karma.field[0].setObjectPosition(new float[] { 0, 0, 0 });
            karma.field[0].set_G_para(40);
            karma.field[0].set_push_or_not(true);//push

            karma.field[1].setObjectPosition(new float[] { 50, 0, 0 });
            karma.field[1].set_push_or_not(false);
            karma.field[1].set_G_para(300);
            set_fields = true;
            set_info_stack_f();

            set_scene_child();
            set_scene = false;
            set_show = true;
        }

        private void orbit_ex_Click(object sender, RoutedEventArgs e)
        {
            this.s_num = 1;
            this.f_num = 1;

            karma = new Gravity_Caculator(s_num, f_num);
            myHelixViewport3D.Children.Clear();
            set_scene = true;

            karma.sources[0].set_particle_numbers(500);
            karma.sources[0].set_particles_life(500);
            karma.sources[0].set_Source_type(1);
            karma.sources[0].setObjectPosition(new float[] { 50, 0, 0 });
            karma.sources[0].set_Velo_scale((float)Math.Sqrt(10));
            karma.sources[0].set_Velo_vec(new float[] { 0, 1, 0 });
            set_source = true;
            set_info_stack_s();



            karma.field[0].setObjectPosition(new float[] { 0, 0, 0 });
            karma.field[0].set_push_or_not(false);
            karma.field[0].set_G_para(500);
            set_fields = true;
            set_info_stack_f();

            set_scene_child();
            set_scene = false;
            set_show = true;
        }

        private void sping_ex_Click(object sender, RoutedEventArgs e)
        {
            this.s_num = 1;
            this.f_num = 5;

            karma = new Gravity_Caculator(s_num, f_num);
            myHelixViewport3D.Children.Clear();
            set_scene = true;

            karma.sources[0] = new mSource(500, 500, new float[] { 0, 0, 50 });
            karma.sources[0].set_Source_type(2);
            karma.sources[0].set_Velo_scale(2f);
            karma.sources[0].set_Velo_vec(new float[] { 0, 0, 1 });
            set_source = true;
            set_info_stack_s();

            karma.field[0].setObjectPosition(new float[] { -70, 0, 0 });
            karma.field[0].set_push_or_not(false);
            karma.field[0].set_G_para(200);

            karma.field[1].setObjectPosition(new float[] { 70, 0, 0 });
            karma.field[1].set_push_or_not(false);
            karma.field[1].set_G_para(200);

            karma.field[2].setObjectPosition(new float[] { 0, 70, 0 });
            karma.field[2].set_push_or_not(false);
            karma.field[2].set_G_para(200);

            karma.field[3].setObjectPosition(new float[] { 0, -70, 0 });
            karma.field[3].set_push_or_not(false);
            karma.field[3].set_G_para(200);

            karma.field[4].set_Infi_or_not(true);
            karma.field[4].setObjectPosition(new float[] { 0, 0, -1 });
            karma.field[4].set_G_para(0.012f);
            set_fields = true;
            set_info_stack_f();

            set_scene_child();
            set_scene = false;
            set_show = true;
        }

        //****//
        //Files
        //****//
        private void save_burron_Click(object sender, RoutedEventArgs e)
        {
            if (set_show)
            {
                string str;
                str = String.Format("{0}\t{1}\n", this.f_num, this.s_num);

                for (int i = 0; i < this.f_num; i++)
                {
                    float[] s_posi = this.karma.field[i].getObjectPosition();
                    str += String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\n"
                        , this.karma.field[i].get_push_or_not()
                        , this.karma.field[i].get_infi_or_not()
                        , this.karma.field[i].get_G_para()
                            , s_posi[0], s_posi[1], s_posi[2]);
                }

                for (int i = 0; i < this.s_num; i++)
                {
                    float[] s_posi = this.karma.sources[i].getObjectPosition();
                    float[] s_velo = this.karma.sources[i].get_lauching_Velocity();
                    float velo_scale = this.karma.sources[i].get_velo_scale();

                    str += String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\n"
                        , this.karma.sources[i].get_type()
                        , this.karma.sources[i].get_particle_nums()
                        , this.karma.sources[i].get_particle_life() 
                        , this.karma.sources[i].get_velo_scale()
                            ,s_posi[0] ,s_posi[1] ,s_posi[2]
                            ,s_velo[0]/velo_scale ,s_velo[1]/velo_scale ,s_velo[2]/velo_scale);
                }

                SaveFileDialog save_dlg = new SaveFileDialog();
                save_dlg.Filter = "Text file (*.txt)|*.txt";
                save_dlg.InitialDirectory = Environment.CurrentDirectory;

                if (save_dlg.ShowDialog() == true)
                {
                    File.WriteAllText(save_dlg.FileName, str);
                    MessageBox.Show("File saved", "File", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("No Scene", "Save", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void load_button_Click(object sender, RoutedEventArgs e)
        {
            string file_path;
            OpenFileDialog load_dlg = new OpenFileDialog();
            load_dlg.Filter = "Text file (*.txt)|*.txt";
            load_dlg.InitialDirectory = Environment.CurrentDirectory;

            if (load_dlg.ShowDialog() == true)
            {
                file_path = load_dlg.FileName;
            }
            else {
                return;
            }

            try
            {
                string[] separators = { "\t", "\n"};
                string in_text = File.ReadAllText(file_path);
                string[] all_text = in_text.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                int read_num = 2;
                this.f_num = Int32.Parse(all_text[0]);
                this.s_num = Int32.Parse(all_text[1]);

                karma = new Gravity_Caculator(s_num, f_num);
                myHelixViewport3D.Children.Clear();
                set_scene = true;

                for (int i = 0; i < this.f_num; i++)
                {
                    karma.field[i].set_push_or_not(Boolean.Parse(all_text[read_num++]));
                    karma.field[i].set_Infi_or_not(Boolean.Parse(all_text[read_num++]));
                    karma.field[i].set_G_para(float.Parse(all_text[read_num++]));
                    karma.field[i].setObjectPosition(new float[] {
                        float.Parse(all_text[read_num++])
                        , float.Parse(all_text[read_num++])
                        , float.Parse(all_text[read_num++]) });
                }
                set_source = true;
                set_info_stack_f();
                
                for (int i = 0; i < this.s_num; i++)
                {
                    karma.sources[i].set_Source_type(Int32.Parse(all_text[read_num++]));
                    karma.sources[i].set_particle_numbers(Int32.Parse(all_text[read_num++]));
                    karma.sources[i].set_particles_life(Int32.Parse(all_text[read_num++]));
                    karma.sources[i].set_Velo_scale(float.Parse(all_text[read_num++]));
                    karma.sources[i].setObjectPosition(new float[] { 
                        float.Parse(all_text[read_num++]),
                        float.Parse(all_text[read_num++]),
                        float.Parse(all_text[read_num++])});
                    karma.sources[i].set_Velo_vec(new float[] { 
                        float.Parse(all_text[read_num++]),
                        float.Parse(all_text[read_num++]),
                        float.Parse(all_text[read_num++])});
                }
                set_fields = true;
                set_info_stack_s();

                set_scene_child();
                set_scene = false;
                set_show = true;

                MessageBox.Show("File Loaded", "Loaded", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (InvalidCastException error)
            {
                MessageBox.Show(error.ToString(), "Read", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


    }
}
