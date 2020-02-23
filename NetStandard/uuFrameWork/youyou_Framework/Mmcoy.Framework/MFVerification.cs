using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Security.Cryptography;

namespace Mmcoy.Framework
{
    public class MFVerification
    {
        public static bool CheckCode(string _code)
        {
            bool result = false;
            if (HttpContext.Current.Session[sessionKey] != null && !string.IsNullOrEmpty(_code))
            {
                if (Convert.ToString(HttpContext.Current.Session[sessionKey]).ToLower() == _code.ToLower())
                {
                    result = true;
                }
            }
            return result;
        }
        public static void GetImage(int width, int height, Color bgcolor, int textcolor)
        {
            Random rnd = new Random();
            string code = "";
            for (int i = 0; i < 4; i++) code += AllCode[rnd.Next(AllCode.Length)];
            HttpContext.Current.Session[sessionKey] = code;
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            Graphics g = Graphics.FromImage(bitmap);
            Rectangle rect = new Rectangle(0, 0, width, height);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            g.Clear(bgcolor);

            int fixedNumber = textcolor == 2 ? 60 : 0;

            SolidBrush drawBrush = new SolidBrush(Color.FromArgb(Next(100), Next(100), Next(100)));
            for (int x = 0; x < 3; x++)
            {
                Pen linePen = new Pen(Color.FromArgb(Next(150) + fixedNumber, Next(150) + fixedNumber, Next(150) + fixedNumber), 1);
                g.DrawLine(linePen, new PointF(0.0F + Next(20), 0.0F + Next(height)), new PointF(0.0F + Next(width), 0.0F + Next(height)));
            }

            int space = (int)Math.Ceiling((double)width / (double)5);

            Matrix m = new Matrix();
            for (int x = 0; x < code.Length; x++)
            {
                m.Reset();
                m.RotateAt(Next(3) - 2, new PointF(Convert.ToInt64(width * (0.10 * x)), Convert.ToInt64(height * 0.5)));
                g.Transform = m;
                drawBrush.Color = Color.FromArgb(Next(150) + fixedNumber + 20, Next(150) + fixedNumber + 20, Next(150) + fixedNumber + 20);
                PointF drawPoint = new PointF(0.0F + Next(1) + x * space, 0.0F + Next(0));
                g.DrawString(Next(1) == 1 ? code[x].ToString() : code[x].ToString().ToUpper(), fonts[Next(fonts.Length - 1)], drawBrush, drawPoint);
                g.ResetTransform();
            }



            double distort = Next(2, 4) * (Next(10) == 1 ? 1 : -1);

            using (Bitmap copy = (Bitmap)bitmap.Clone())
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int newX = (int)(x + (distort * Math.Sin(Math.PI * y / 84.5)));
                        int newY = (int)(y + (distort * Math.Cos(Math.PI * x / 54.5)));
                        if (newX < 0 || newX >= width)
                            newX = 0;
                        if (newY < 0 || newY >= height)
                            newY = 0;
                        bitmap.SetPixel(x, y, copy.GetPixel(newX, newY));
                    }
                }
            }
            drawBrush.Dispose();
            g.Dispose();

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ContentType = "image/Gif";
            HttpContext.Current.Response.BinaryWrite(ms.ToArray());
        }
        private const string AllCode = "23456789ABCDEFGHJKLMNPQRSTUWXYZ";
        private const string sessionKey = "checkcode";
        private static byte[] randb = new byte[4];
        private static RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();
        private static Font[] fonts = {
                                       new Font(new FontFamily("Times New Roman"), 15 + Next(1),System.Drawing.FontStyle.Bold),
                                       new Font(new FontFamily("Georgia"), 15 + Next(1),System.Drawing.FontStyle.Bold),
                                       new Font(new FontFamily("Arial"), 15 + Next(1),System.Drawing.FontStyle.Bold),
                                       new Font(new FontFamily("Comic Sans MS"), 15 + Next(1),System.Drawing.FontStyle.Bold)
                                    };
        private static int Next(int max)
        {
            rand.GetBytes(randb);
            int value = BitConverter.ToInt32(randb, 0);
            value = value % (max + 1);
            if (value < 0)
                value = -value;
            return value;
        }

        private static int Next(int min, int max)
        {
            int value = Next(max - min) + min;
            return value;
        }
    }
}
