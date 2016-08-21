using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_ParticleSimulator
{
    public class mSource : mObject
    {
        private int p_life = 0;
        private int p_num = 1;
        private int SOURCE_TYPE;   // 0: corn source, 1: line, 2: parallel
        private float velocity_scale;
        private float[] velocity_vec;

        public Particle[] particles;

        private void initialize_Source(int n, int in_life)
        {
            this.p_num = n;
            this.p_life = in_life;
            particles = new Particle[p_num];
            for (int i = 0; i < this.p_num; i++)
            {
                this.particles[i] = new Particle();
                this.particles[i].setObjectPosition(this.getObjectPosition());
            }
            this.SOURCE_TYPE = 0;
            this.velocity_scale = 1;
            this.velocity_vec = new float[] { 1, 0, 0 };
            this.set_Drawing_Color(new float[] { 138f, 43f, 226f });
            this.Drawing.Radius = 2;
        }

        //****//
        //constructor
        //****//
        public mSource()
            : base()
        {
            initialize_Source(100, 100);
        }

        public mSource(int parn, int in_life)
            : base()
        {
            initialize_Source(parn, in_life);
        }

        public mSource(int parn, int in_life, float[] inPosition)
            : base(inPosition)
        {
            initialize_Source(parn, in_life);
        }

        public mSource(int parn, int in_life, float[] inPosition, float[] inVelocity)
            : base(inPosition, inVelocity)
        {
            initialize_Source(parn, in_life);
        }

        //****//
        //set
        //****//
        public void set_particles_life(int in_life)
        {
            this.p_life = in_life;
        }

        public void set_particle_numbers(int n)
        {
            this.p_num = n;
            this.particles = new Particle[this.p_num];
            for (int i = 0; i < this.p_num; i++)
            {
                this.particles[i] = new Particle();
                this.particles[i].setObjectPosition(this.getObjectPosition());
            }
        }

        public void set_Source_type(int type)
        {
            this.SOURCE_TYPE = type;
        }

        public void set_Velo_scale(float scale)
        {
            this.velocity_scale = scale;
        }

        public void set_Velo_vec(float[] vector)
        {
            float norm = (float)Math.Sqrt(vector[0] * vector[0] + vector[1] * vector[1] + vector[2] * vector[2]);
            this.velocity_vec = new float[] { vector[0] / norm, vector[1] / norm, vector[2] / norm };
        }

        public void set_particles_position(int n, float[] inPosi)
        {
            this.particles[n].setObjectPosition(inPosi);
        }

        public void set_particles_velocity(int n, float[] inVelo)
        {
            this.particles[n].setObjectVelocity(inVelo);
        }

        public void set_particles_acceleration(int n, float[] inAcce)
        {
            this.particles[n].set_Acceleration(inAcce);
        }


        //****//
        //get
        //****//
        public float[] get_lauching_Velocity()
        {
            return new float[]{this.velocity_scale*this.velocity_vec[0]
                , this.velocity_scale*this.velocity_vec[1], this.velocity_scale*this.velocity_vec[2]};
        }

        public int get_particle_nums()
        {
            return this.p_num;
        }

        public int get_particle_life()
        {
            return this.p_life;
        }

        public float get_velo_scale()
        {
            return this.velocity_scale;
        }

        public int get_type() {
            return this.SOURCE_TYPE;
        }

        //****//
        //functions
        //****//
        public void launch()
        {
            float[] tempV;
            Random rand = new Random();

            bool this_frame_lauched = false;


            for (int i = 0; i < this.p_num; i++)
            {
                //launched or not
                if (this.particles[i].get_launched())
                {
                    this.particles[i].life_pp();

                    if (this.particles[i].get_life_num() > this.p_life)
                    {
                        this.particles[i].setObjectPosition(this.getObjectPosition());
                        this.particles[i].setObjectVelocity(new float[] { 0, 0, 0 });
                        this.particles[i].set_Acceleration((new float[] { 0, 0, 0 }));
                        this.particles[i].set_launched(false);
                        this.particles[i].set_life_zero();
                    }
                }
                else if (!this_frame_lauched)
                {
                    tempV = this.get_lauching_Velocity();
                    if (SOURCE_TYPE == 0)   //cone
                    {
                        this.particles[i].setObjectPosition(this.getObjectPosition());
                        float r0 = rand.Next(0, 32767) / 32767f * 0.8f - 0.4f;
                        float r1 = rand.Next(0, 32767) / 32767f * 0.8f - 0.4f;
                        float r2 = rand.Next(0, 32767) / 32767f * 0.8f - 0.4f;
                        this.particles[i].setObjectVelocity(new float[] { tempV[0] + r0, tempV[1] + r1, tempV[2] + r2 });
                        this.particles[i].set_launched(true);
                        this_frame_lauched = true;
                    }
                    if (SOURCE_TYPE == 1)   //line
                    {
                        this.particles[i].setObjectPosition(this.getObjectPosition());
                        this.particles[i].setObjectVelocity(new float[] { tempV[0], tempV[1], tempV[2] });
                        this.particles[i].set_launched(true);
                        this_frame_lauched = true;
                    }
                    if (SOURCE_TYPE == 2)   //para
                    {
                        float[] tempP = this.getObjectPosition();
                        float r0 = rand.Next(0, 32767) / 32767f * 16f - 8f;
                        float r1 = rand.Next(0, 32767) / 32767f * 16f - 8f;
                        float r2 = rand.Next(0, 32767) / 32767f * 16f - 8f;
                        this.particles[i].setObjectPosition(new float[]{tempP[0] + r0, tempP[1] + r1, tempP[2] + r2});
                        this.particles[i].setObjectVelocity(new float[] { tempV[0], tempV[1], tempV[2] });
                        this.particles[i].set_launched(true);
                        this_frame_lauched = true;
                    }
                }
            }
        }

        public override string ToString()
        {
            return String.Format("Source tyoe: {0}\n Velocity scale: {1}\n Velocity vector : {2}, {3}, {4}"
                , this.SOURCE_TYPE, this.velocity_scale, this.velocity_vec[0], this.velocity_vec[1], this.velocity_vec[2]) +
                "\n" + base.ToString();
        }
    }

}
