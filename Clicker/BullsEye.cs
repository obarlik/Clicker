using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Clicker
{
    public partial class BullsEye : Form
    {
        public ClickData Last { get; private set; }

        public BullsEye()
        {
            InitializeComponent();
        }



        private void BullsEye_MouseMove(object sender, MouseEventArgs e)
        {
            if (!e.Button.HasFlag(MouseButtons.Left))
            {
                timer1.Enabled = false;
                Hide();

                Selected?.Invoke(this, EventArgs.Empty);
                return;
            }
        }

        private void BullsEye_Activated(object sender, EventArgs e)
        {
        }

        public void Start()
        {
            Show();
            Activate();

            timer1.Enabled = true;
        }


        public event EventHandler Selected;


        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            
            Last = ClickData.CurrentData();
            Left = Last.Target.X - Width / 2;
            Top = Last.Target.Y - Height / 2;

            timer1.Enabled = true;
        }
    }
}
