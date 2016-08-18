using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_ParticleSimulator
{
    public class Particle : mObject
    {
        private int life = 0;
        private bool launched = false;
        private float mass;
        private float[] acceleration;

        private void initialized_Particle()
        {
            this.mass = 1;
            this.acceleration = new float[] { 0, 0, 0 };
        }

        private float[] hsvToRgb(float h)
        {
            float r = 0, g = 0, b = 0;
            float s = 1.0f;
            float v = 1.0f;

            h *= -0.67f * 2f;
            h += 0.67f;

            if (h < 0) { h = 0; }
            if (h > 0.67) { h = 0.67f; }

            int i = (int)(h * 6);
            float f = h * 6 - i;
            float p = v * (1 - s);
            float q = v * (1 - f * s);
            float t = v * (1 - (1 - f) * s);

            switch (i % 6)
            {
                case 0: r = v; g = t; b = p; break;
                case 1: r = q; g = v; b = p; break;
                case 2: r = p; g = v; b = t; break;
                case 3: r = p; g = q; b = v; break;
                case 4: r = t; g = p; b = v; break;
                case 5: r = v; g = p; b = q; break;
            }

            return new float[] { r*255f, g*255f, b*255f };
        }

        //****//
        //constructor
        //****//
        public Particle()
            : base()
        {
            initialized_Particle();
        }

        public Particle(float[] inPosition)
            : base(inPosition)
        {
            initialized_Particle();
        }

        public Particle(float[] inPosition, float[] inVelocity)
            : base(inPosition, inVelocity)
        {
            initialized_Particle();
        }

        //****//
        //set
        //****//
        public void life_pp()
        {
            this.life++;
        }

        public void set_life_zero()
        {
            this.life = 0;
        }

        public void set_launched(bool y_n)
        {
            this.launched = y_n;
        }

        public void set_Mass(float M)
        {
            this.mass = M;
            this.set_Drawing_Radious((float)(Math.Pow(M * 3.0 / 4.0 / Math.PI, 1.0 / 3.0)));
        }

        public void set_Acceleration(float[] inAcceleration)
        {
            for (int i = 0; i < 3; i++)
                this.acceleration[i] = inAcceleration[i];
        }

        public override void setObjectVelocity(float[] inVelocity)
        {
            base.setObjectVelocity(inVelocity);
            this.set_Drawing_Color(hsvToRgb(this.getObjectABSVelocity()/6.0f));
        }

        //****//
        // get
        //****//
        public int get_life_num()
        {
            return this.life;
        }

        public bool get_launched()
        {
            return this.launched;
        }

        public float[] get_Acceleration()
        {
            return this.acceleration;
        }

        public override string ToString()
        {
            return String.Format("Mass: {0}\n Acceration: {1}, {2}, {3}"
                , this.mass, this.acceleration[0], this.acceleration[1], this.acceleration[2])
                + "\n" + base.ToString();
        }

    }
}
