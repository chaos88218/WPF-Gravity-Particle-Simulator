using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WPF_ParticleSimulator
{
    public class Gravity_Caculator
    {
        //TODO : Add gravity between particles caculation
        private int s_num = 1;
        private int f_num = 1;

        public GField[] field;
        public mSource[] sources;

        private void initialization()
        {
            sources = new mSource[s_num];
            for (int i = 0; i < this.s_num; i++)
            {
                this.sources[i] = new mSource();
            }
            field = new GField[f_num];
            for (int i = 0; i < this.f_num; i++)
            {
                this.field[i] = new GField();
            }
        }

        //****//
        //constructor
        //****//
        public Gravity_Caculator()
        {
            initialization();
        }

        public Gravity_Caculator(int sources_nums, int field_num)
        {
            this.s_num = sources_nums;
            this.f_num = field_num;
            initialization();
        }

        //****//
        //numbers setter
        //****//
        public void setSourceNum(int sources_nums)
        {
            this.s_num = sources_nums;
            sources = new mSource[s_num];
        }

        public void setFieldNum(int field_num)
        {
            this.f_num = field_num;
            field = new GField[f_num];
        }

        //****//
        //caculate
        //****//
        public void NextFrame()
        {
            this.All_reset();
            this.source_launching();
            this.field_caculation();
            this.next_position();
        }

        private void source_launching()
        {
            for (int h = 0; h < s_num; h++)
            {
                this.sources[h].launch();
            }
        }

        private void next_position()
        {
            for (int h = 0; h < s_num; h++)
            {
                for (int i = 0; i < sources[h].get_particle_nums(); i++)
                {
                    // Roung-Kutta Intergration
                    if (sources[h].particles[i].get_launched())
                    {
                        float[] f = sources[h].particles[i].get_Acceleration();
                        float[] dY1 = sources[h].particles[i].getObjectVelocity();
                        float[] Y1 = sources[h].particles[i].getObjectPosition();

                        float[] K1 = dY1;
                        float[] D1 = f;
                        float[] K2 = new float[] { dY1[0] + D1[0] / 2.0f
                            , dY1[1] + D1[1] / 2.0f, dY1[2] + D1[2] / 2.0f };
                        float[] D2 = new float[] { 0, 0, 0 };
                        float[] K3 = new float[] { 0, 0, 0 };
                        float[] D3 = new float[] { 0, 0, 0 };
                        float[] K4 = new float[] { 0, 0, 0 };
                        float[] D4 = new float[] { 0, 0, 0 };

                        for (int j = 0; j < f_num; j++)
                        {
                            float[] fpos = field[j].getObjectPosition();

                            D2 = calculate_acce(this.field[j].get_push_or_not()
                                    , new float[] { Y1[0] + K1[0] / 2.0f - fpos[0]
                                    , Y1[1] + K1[1] / 2.0f - fpos[1]
                                    , Y1[2] + K1[2] / 2.0f - fpos[2] }
                                    , this.field[j].get_G_para()
                                    , D2);
                            K3 = new float[] { dY1[0] + D2[0] / 2.0f, dY1[1] + D2[1] / 2.0f, dY1[2] + D2[2] / 2.0f };
                            D3 = calculate_acce(this.field[j].get_push_or_not()
                                    , new float[] { Y1[0] + K2[0] / 2.0f - fpos[0]
                                    , Y1[1] + K2[1] / 2.0f - fpos[1]
                                    , Y1[2] + K2[2] / 2.0f - fpos[2] }
                                    , this.field[j].get_G_para()
                                    , D3);
                            K4 = new float[] { dY1[0] + D3[0], dY1[1] + D3[1], dY1[2] + D3[2] };
                            D4 = calculate_acce(this.field[j].get_push_or_not()
                                    , new float[] { Y1[0] + K3[0] - fpos[0]
                                    , Y1[1] + K3[1] - fpos[1]
                                    , Y1[2] + K3[2] - fpos[2] }
                                    , this.field[j].get_G_para()
                                    , D4);
                        }

                        this.sources[h].set_particles_position(i
                            , RK_intergration(Y1, K1, K2, K3, K4));
                        this.sources[h].set_particles_velocity(i
                            , RK_intergration(dY1, D1, D2, D3, D4));
                    }
                }
            }
        }

        private void All_reset()
        {
            for (int h = 0; h < s_num; h++)
            {
                for (int i = 0; i < sources[h].get_particle_nums(); i++)
                {
                    this.sources[h].set_particles_acceleration(i, new float[] { 0, 0, 0 });
                }
            }
        }

        private void field_caculation()
        {
            for (int h = 0; h < s_num; h++)
            {
                int p_num = sources[h].get_particle_nums();
                for (int i = 0; i < p_num; i++)
                {
                    if (sources[h].particles[i].get_launched())
                    {
                        for (int j = 0; j < f_num; j++)
                        {
                            if (this.field[j].get_infi_or_not())
                            {
                                float[] fpos = this.field[j].getObjectPosition();
                                float[] acce = this.sources[h].particles[i].get_Acceleration();
                                float Dist = (float)Math.Sqrt(Math.Pow(fpos[0], 2)
                                    + Math.Pow(fpos[1], 2) + Math.Pow(fpos[2], 2));
                                Dist = field[j].get_G_para() / Dist;

                                this.sources[h].set_particles_acceleration(i,
                                    new float[] {fpos[0]*Dist +acce[0],
                                    fpos[1]*Dist+acce[1],
                                    fpos[2]*Dist+acce[2]});
                            }
                            else
                            {
                                float[] fpos = this.field[j].getObjectPosition();
                                float[] ppos = this.sources[h].particles[i].getObjectPosition();
                                float[] acce = this.sources[h].particles[i].get_Acceleration();

                                float[] new_acce = calculate_acce(this.field[j].get_push_or_not()
                                    , new float[] { ppos[0] - fpos[0], ppos[1] - fpos[1], ppos[2] - fpos[2] }
                                    , this.field[j].get_G_para()
                                    , acce
                                    );

                                this.sources[h].set_particles_acceleration(i,
                                    new_acce);
                            }
                        }
                    }
                }
            }
            return;
        }

        private float[] calculate_acce(bool p_o_n, float[] in_vector
            , float in_g_para, float[] in_acce){

            float dist = (float)Math.Sqrt(in_vector[0]*in_vector[0] 
                + in_vector[1]*in_vector[1]
                + in_vector[2]*in_vector[2]);

            if (dist < 0.01f)
            {
                dist = 0.01f;
            }
            float mmforce = in_g_para / (dist * dist);

            if (!p_o_n)
            {
                mmforce = -mmforce;
            }
            return new float[] {mmforce * in_vector[0]/dist + in_acce[0],
                                    mmforce * in_vector[1]/dist + in_acce[1],
                                    mmforce * in_vector[2]/dist + in_acce[2]};
        }

        private float[] RK_intergration(float[] Y1, float[] K1, float[] K2, float[] K3, float[] K4) {
            return new float[]{
                Y1[0] + (K1[0] + 2 * K2[0] + 2 * K3[0] + K4[0])/6.0f,
                Y1[1] + (K1[1] + 2 * K2[1] + 2 * K3[1] + K4[1])/6.0f,
                Y1[2] + (K1[2] + 2 * K2[2] + 2 * K3[2] + K4[2])/6.0f
            };

            //return new float[]{
            //    Y1[0] + (K1[0] + 4 * K2[0] + K3[0])/6.0f,
            //    Y1[1] + (K1[1] + 4 * K2[1] + K3[1])/6.0f,
            //    Y1[2] + (K1[2] + 4 * K2[2] + K3[2])/6.0f
            //};
        }
    }
}
