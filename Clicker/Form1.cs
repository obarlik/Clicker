using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clicker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            bullsEye = new BullsEye();
            bullsEye.Selected += BullsEye_Selected;
        }

        private void BullsEye_Selected(object sender, EventArgs e)
        {
            numericUpDown1.Value = bullsEye.Last.Target.X;
            numericUpDown2.Value = bullsEye.Last.Target.Y;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var actions = new List<ClickData> {
                new ClickData {
                    ActionType = ActionType.Move,
                    Target = new MousePoint((int)numericUpDown1.Value, (int)numericUpDown2.Value)
                }
            };

            Task.Run(() => actions.ForEach(a => a.Perform().Wait()));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var data = ClickData.CurrentData();
            toolStripStatusLabel1.Text = $"X: {data.Target.X}  Y: {data.Target.Y}";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MoveAndClick();
        }

        private void MoveAndClick()
        {
            var actions = new List<ClickData> {
                new ClickData {
                    ActionType = ActionType.Move | ActionType.Click,
                    Target = new MousePoint((int)numericUpDown1.Value, (int)numericUpDown2.Value),
                    Button = MouseButton.Left,
                }
            };

            Task.Run(() => actions.ForEach(a => a.Perform().Wait()));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!timer2.Enabled)
            {
                timer2.Interval = (int)new TimeSpan(0, dateTimePicker1.Value.Minute, dateTimePicker1.Value.Second)
                    .TotalMilliseconds;

                timer2.Enabled = true;
                button1.Text = "Stop";
            }
            else
            {
                timer2.Enabled = false;
                button1.Text = "Start";
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            MoveAndClick();
        }


        BullsEye bullsEye;


        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            bullsEye.Start();
        }
    }
}
