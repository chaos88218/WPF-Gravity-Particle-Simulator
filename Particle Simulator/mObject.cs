using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace WPF_ParticleSimulator
{
    public abstract class mObject
    {
        private float[] position;
        private float[] velocity;
        public SphereVisual3D Drawing;

        private void initialized_Drawing(float R)
        {
            this.Drawing = new SphereVisual3D();
            this.Drawing.Center = new Point3D(this.position[0], this.position[1], this.position[2]);
            this.Drawing.Radius = R;
            this.Drawing.PhiDiv = 4;
            this.Drawing.ThetaDiv = 8;
        }

        //****//
        //constructor
        //****//
        public mObject()
        {
            this.position = new float[] { 0, 0, 0 };
            this.velocity = new float[] { 0, 0, 0 };
            this.initialized_Drawing(1);
        }

        public mObject(float[] inPosition)
        {
            this.position = new float[] { inPosition[0], inPosition[1], inPosition[2] };
            this.velocity = new float[] { 0, 0, 0 };
            this.initialized_Drawing(1);
        }

        public mObject(float[] inPosition, float[] inVelocity)
        {
            this.position = new float[] { inPosition[0], inPosition[1], inPosition[2] };
            this.velocity = new float[] { inVelocity[0], inVelocity[1], inVelocity[2] };
            this.initialized_Drawing(1);
        }

        //****//
        //set
        //****//
        public void setObjectPosition(float[] inPosition)
        {
            for (int i = 0; i < 3; i++)
            { this.position[i] = inPosition[i]; }
            update_Drawing_position();
        }

        virtual public void setObjectVelocity(float[] inVelocity)
        {
            for (int i = 0; i < 3; i++)
            { this.velocity[i] = inVelocity[i]; }
        }
        
        //****//
        //get
        //****//
        public float[] getObjectPosition()
        {
            return this.position;
        }

        public float getObjectABSVelocity()
        {
            return (float)Math.Sqrt(this.velocity[0] * this.velocity[0]
                + this.velocity[1] * this.velocity[1] + this.velocity[2] * this.velocity[2]);
        }

        public float[] getObjectVelocity()
        {
            return this.velocity;
        }

        //****//
        //Drawing
        //****//
        public void update_Drawing_position()
        {
            this.Drawing.Center = new Point3D(this.position[0], this.position[1], this.position[2]);
        }

        public void set_Drawing_Radious(float R)
        {
            this.Drawing.Radius = R;
        }

        public void set_Drawing_Color(float[] in_rgb)
        {
            this.Drawing.Fill = new SolidColorBrush(Color.FromArgb(255, (byte)in_rgb[0], (byte)in_rgb[1], (byte)in_rgb[2]));
        }

        public override string ToString()
        {
            return String.Format("position:\n {0}, {1}, {2}\nvelocity:\n {3}, {4}, {5}\n"
                , this.position[0], this.position[1], this.position[2],
                this.velocity[0], this.velocity[1], this.velocity[2]);
        }
    }
}
