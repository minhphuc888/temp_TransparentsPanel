using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace temp_TransparentsPanel
{
    public static class RectangleDrawer
    {
        private static Form mMask;
        private static Point mPos;
        private static PictureBox pBox;

        public static Rectangle Draw(PictureBox pictureBox)
        {
            // Record the start point
            mPos = pictureBox.PointToClient(Control.MousePosition);
            pBox = pictureBox;
            // Create a transparent form on top of <frm>
            mMask = new Form();
            mMask.FormBorderStyle = FormBorderStyle.None;
            mMask.BackColor = Color.Magenta;
            mMask.TransparencyKey = mMask.BackColor;
            mMask.ShowInTaskbar = false;
            mMask.StartPosition = FormStartPosition.Manual;
            mMask.Size = pictureBox.Size;
            mMask.Location = pictureBox.PointToScreen(Point.Empty);
            mMask.MouseMove += MouseMove;
            mMask.MouseUp += MouseUp;
            mMask.Paint += PaintRectangle;
            mMask.Load += DoCapture;
            // Display the overlay
            mMask.ShowDialog(pictureBox);
            // Clean-up and calculate return value
            mMask.Dispose();
            mMask = null;
            Point pos = pictureBox.PointToClient(Control.MousePosition);
            int x = Math.Min(mPos.X, pos.X);
            int y = Math.Min(mPos.Y, pos.Y);
            int w = Math.Abs(mPos.X - pos.X);
            int h = Math.Abs(mPos.Y - pos.Y);
            return new Rectangle(x, y, w, h);
        }
        private static void DoCapture(object sender, EventArgs e)
        {
            // Grab the mouse
            mMask.Capture = true;
        }
        private static void MouseMove(object sender, MouseEventArgs e)
        {
            // Repaint the rectangle
            mMask.Invalidate();
        }
        private static void MouseUp(object sender, MouseEventArgs e)
        {
            // Done, close mask
            mMask.Close();
            Paint();
        }



        private static void PaintRectangle(object sender, PaintEventArgs e)
        {
            // Draw the current rectangle
            pos = mMask.PointToClient(Control.MousePosition);
            using (Pen pen = new Pen(Brushes.Black))
            {
                pen.DashStyle = DashStyle.Dot;
                e.Graphics.DrawLine(pen, mPos.X, mPos.Y, pos.X, mPos.Y);
                e.Graphics.DrawLine(pen, pos.X, mPos.Y, pos.X, pos.Y);
                e.Graphics.DrawLine(pen, pos.X, pos.Y, mPos.X, pos.Y);
                e.Graphics.DrawLine(pen, mPos.X, pos.Y, mPos.X, mPos.Y);
                e.Graphics.DrawString("a", new Font("Times New Roman", 12.0f), Brushes.Black, mPos.X - 15, mPos.Y - 15);
                e.Graphics.DrawString("b", new Font("Times New Roman", 12.0f), Brushes.Black, pos.X, pos.Y - 15);
            }
        }

        static Point pos;
        private static void Paint()
        {
            using (Pen pen = new Pen(Brushes.Black))
            {
                //pen.DashStyle = DashStyle.Dot;
                Bitmap myBitmap = new Bitmap(pBox.Width, pBox.Height);
                var g = Graphics.FromImage(myBitmap);

                g.DrawLine(pen, mPos.X, mPos.Y, pos.X, mPos.Y);
                g.DrawLine(pen, pos.X, mPos.Y, pos.X, pos.Y);
                g.DrawLine(pen, pos.X, pos.Y, mPos.X, pos.Y);
                g.DrawLine(pen, mPos.X, pos.Y, mPos.X, mPos.Y);
                g.DrawString("a", new Font("Times New Roman", 12.0f), Brushes.Black, mPos.X - 15, mPos.Y - 15);
                g.DrawString("b", new Font("Times New Roman", 12.0f), Brushes.Black, pos.X, pos.Y - 15);

                Bitmap bmp = new Bitmap(100, 100, g);
                pBox.BackgroundImage = myBitmap;
            }

        }
    }
}
