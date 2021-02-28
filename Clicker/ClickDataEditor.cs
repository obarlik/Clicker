using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace Clicker
{

    public partial class ClickDataEditor : Component
    {
        public ClickDataEditor()
        {
            InitializeComponent();
        }

        public ClickDataEditor(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }


        public TimeSpan Wait { get; set; }

        public MouseButton Button { get; set; }

        public ButtonState State { get; set; }

        public MousePoint Target { get; set; }
    }
}
