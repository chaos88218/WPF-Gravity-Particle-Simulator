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

                    float[] acce = sources[h].particles[i].get_Acceleration();
                    float[] velo = sources[h].particles[i].getObjectVelocity();
                    float[] posi = sources[h].particles[i].getObjectPosition();
                    for (int j = 0; j < 3; j++)
                    {
                        posi[j] = posi[j] + velo[j];
                        velo[j] = velo[j] + acce[j];
                    }

                    this.sources[h].set_particles_position(i, posi);
                    this.sources[h].set_particles_velocity(i, velo);
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
                                new float[] {fpos[0]*Dist + acce[0],
                                    fpos[1]*Dist+acce[1],
                                    fpos[2]*Dist+acce[2] });
                        }
                        else
                        {
                            float[] fpos = this.field[j].getObjectPosition();
                            float[] ppos = this.sources[h].particles[i].getObjectPosition();

                            float Dist = (float)Math.Sqrt(Math.Pow(ppos[0] - fpos[0], 2)
                                + Math.Pow(ppos[1] - fpos[1], 2) + Math.Pow(ppos[2] - fpos[2], 2));
                            if (Dist < 0.01f)
                            {
                                Dist = 0.01f;
                            }
                            float mmforce = field[j].get_G_para() / (Dist * Dist * Dist);

                            if (!this.field[j].get_push_or_not())
                            {
                                mmforce = -mmforce;
                            }

                            float[] acce = this.sources[h].particles[i].get_Acceleration();
                            this.sources[h].set_particles_acceleration(i,
                                new float[] { mmforce * (ppos[0] - fpos[0])+acce[0],
                                    mmforce * (ppos[1] - fpos[1])+acce[1],
                                    mmforce * (ppos[2] - fpos[2])+acce[2] });
                        }
                    }
                }
            }
            return;
        }
    }
}
