using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_ParticleSimulator
{
    public class GField : mObject
    {
        //TODO :　Add flat field
        private bool push_or_not = false;
        private bool infi_Dist;
        private float virtual_Mass;
        private float G_para;

        private void initialize_GField(float inG_para)
        {
            this.infi_Dist = false;
            this.virtual_Mass = 1;
            this.G_para = inG_para;
            this.set_Drawing_Color(new float[] { 123, 123, 123 });
            this.Drawing.Radius = 3;
        }

        //****//
        //constructor
        //****//
        public GField() : base() { initialize_GField(1); }

        public GField(float inG_para) : base() { initialize_GField(inG_para); }

        public GField(float inG_para, float[] inPosition)
            : base(inPosition)
        {
            initialize_GField(inG_para);
        }

        public GField(float inG_para, float[] inPosition, float[] inVelocity)
            : base(inPosition, inVelocity)
        {
            initialize_GField(inG_para);
        }

        //****//
        //set get
        //****//
        public void set_push_or_not(bool p)
        {
            this.push_or_not = p;
        }

        public bool get_push_or_not()
        {
            return this.push_or_not;
        }

        public void set_Infi_or_not(bool y_n)
        {
            this.infi_Dist = y_n;
            if (y_n)
            {
                this.Drawing.Visible = false;
            }
            else
            {
                this.Drawing.Visible = true;
            }
        }

        public void set_virtua_mass(float inMass)
        {
            this.virtual_Mass = inMass;
        }

        public void set_G_para(float inG_para)
        {
            this.G_para = inG_para;
        }

        public float get_G_para()
        {
            return this.G_para;
        }

        public bool get_infi_or_not()
        {
            return this.infi_Dist;
        }

        public override string ToString()
        {
            return String.Format("Virtual Mass: {0}\n Infinite Distance: {1}\n G_para : {2}"
                , this.virtual_Mass, this.infi_Dist, this.G_para)
            + "\n" + base.ToString();
        }

    }
}
