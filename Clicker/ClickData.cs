using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Clicker
{
    public class ClickData
    {
        public ActionType ActionType { get; set; }

        public MouseButton Button { get; set; }

        public ButtonState State { get; set; }

        public MousePoint Target { get; set; }

        int ClickState { get; set; }


        [Flags]
        public enum MouseEventFlags : int
        {
            None = 0,
            LeftDown = 0x00000002,
            LeftUp = 0x00000004,
            MiddleDown = 0x00000020,
            MiddleUp = 0x00000040,
            Move = 0x00000001,
            Absolute = 0x00008000,
            RightDown = 0x00000008,
            RightUp = 0x00000010
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out MousePoint lpMousePoint);

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);


        public static ClickData CurrentData()
        {
            GetCursorPos(out MousePoint p);
            return new ClickData { Target = p };
        }

        public async Task Perform()
        {
            while (GetCursorPos(out MousePoint p))
            {
                if (ActionType.HasFlag(ActionType.Move))
                {
                    var dx = Target.X - p.X;
                    var dist = Math.Abs(dx);

                    if (dist > 10)
                        dx /= 10;
                    else if (dx > 0)
                        dx = 1;
                    else if (dx < 0)
                        dx = -1;

                    var dy = Target.Y - p.Y;
                    dist = Math.Abs(dy);

                    if (dist > 10)
                        dy /= 10;
                    else if (dy > 0)
                        dy = 1;
                    else if (dy < 0)
                        dy = -1;

                    if (dx != 0 || dy != 0)
                    {
                        mouse_event((int)MouseEventFlags.Move, dx, dy, 0, 0);
                        await Task.Delay(2);
                        continue;
                    }
                }

                if (ActionType.HasFlag(ActionType.Click)
                 || ActionType.HasFlag(ActionType.DoubleClick))
                {
                    var flags =
                        Button.HasFlag(MouseButton.Left)
                        ? (ClickState & 1) == 0
                          ? MouseEventFlags.LeftDown
                          : MouseEventFlags.LeftUp
                        : Button.HasFlag(MouseButton.Middle)
                          ? (ClickState & 1) == 0
                            ? MouseEventFlags.MiddleDown
                            : MouseEventFlags.MiddleUp
                        : Button.HasFlag(MouseButton.Right)
                          ? (ClickState & 1) == 0
                            ? MouseEventFlags.RightDown
                            : MouseEventFlags.RightUp
                        : 0;

                    mouse_event((int)flags, 0, 0, 0, 0);

                    var checkClicks = ActionType.HasFlag(ActionType.DoubleClick)
                        ? 4
                        : ActionType.HasFlag(ActionType.Click)
                        ? 2
                        : 0;

                    if (checkClicks > 0)
                    {
                        if (++ClickState < checkClicks)
                        {
                            await Task.Delay(10);
                            continue;
                        }

                        ClickState = 0;
                    }
                }
                
                break;
            }
        }


        //public IEnumerable<ClickData> Parse(string macro)
        //{
        //    foreach(var line in macro.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries))
        //    {
        //        var tmp = line.Trim();

        //        if (tmp.StartsWith('#') || tmp.StartsWith("//"))
        //            continue;

        //        var data = ParseLine(tmp);

        //        yield return
        //    }
        //}

        //private ClickData ParseLine(string cmd)
        //{
        //    var s = cmd.Split(';');

        //    var result = new ClickData();

        //    result.WaitBefore
        //}
    }
}
