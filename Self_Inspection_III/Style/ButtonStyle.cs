using System;
using System.Drawing;
using System.Windows.Forms;

namespace Self_Inspection_III.Style
{
    class ButtonStyle
    {
        public delegate void EventHandler(object sender, EventArgs e);

        public static void MouseHover(object sender, EventArgs e)
        {
            ((Button)sender).BackColor = Color.Red;
        }
    }

    class TsbtnStyle
    {
        public delegate void EventHandler(object sender, EventArgs e);

        public static void MouseMove(object sender, EventArgs e)
        {
            ((ToolStripItem)sender).BackgroundImage = ChangingBackImg(sender, Events.MouseMove, Color.Gray);
        }
        public static void MouseHover(object sender, EventArgs e)
        {
            ((ToolStripItem)sender).BackgroundImage = ChangingBackImg(sender, Events.MouseHover, Color.Gray);
        }
        public static void MouseDown(object sender, EventArgs e)
        {
            ((ToolStripItem)sender).BackgroundImage = ChangingBackImg(sender, Events.MouseDown, Color.Gray);
        }
        public static void MouseEnter(object sender, EventArgs e)
        {
            ((ToolStripItem)sender).BackgroundImage = ChangingBackImg(sender, Events.MouseEnter, Color.Gray);
        }
        public static void MouseUp(object sender, EventArgs e)
        {
            ((ToolStripItem)sender).BackgroundImage = ChangingBackImg(sender, Events.MouseUp, Color.Gray);
        }
        public static void MouseLeave(object sender, EventArgs e)
        {
            ((ToolStripItem)sender).BackgroundImage = ChangingBackImg(sender, Events.MouseLeave, Color.White);
        }

        private static Bitmap ChangingBackImg(object sender, Events events, Color color)
        {
            // Cast to allow reuse of method.
            ToolStripItem tsi = (ToolStripItem)sender;

            // Create semi-transparent picture.
            Bitmap bm = new Bitmap(tsi.Width + 1, tsi.Height + 1);
            int shadowPxls = 1, xStart = 0, yStart = 0, xEnd = bm.Width, yEnd = bm.Height;

            switch (events)
            {
                case Events.MouseMove:
                case Events.MouseHover:
                case Events.MouseLeave:
                    xStart = tsi.Width - shadowPxls;
                    yStart = tsi.Height - shadowPxls;
                    break;
                case Events.MouseDown:
                case Events.MouseEnter:
                case Events.MouseUp:
                    xEnd = shadowPxls;
                    yEnd = shadowPxls;
                    break;
            }

            for (int x = xStart; x < xEnd; x++)
                for (int y = 0; y < bm.Height; y++)
                    bm.SetPixel(x, y, color);

            for (int y = yStart; y < yEnd; y++)
                for (int x = 0; x < bm.Width; x++)
                    bm.SetPixel(x, y, color);

            return bm;
        }
    }

    enum Events
    {
        MouseMove,
        MouseHover,
        MouseDown,
        MouseEnter,
        MouseUp,
        MouseLeave
    }
}
