using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Clicker
{


    [StructLayout(LayoutKind.Sequential)]
    public struct MousePoint
    {
        public int X;
        public int Y;

        public MousePoint(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
