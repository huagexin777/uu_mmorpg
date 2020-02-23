using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Mmcoy.Framework
{
    public static class MFWebUtil
    {
        #region FileterImg 过滤内容中的图片
        /// <summary>
        /// 过滤内容中的图片
        /// </summary>
        /// <param name="strhtml">内容中图片</param>
        /// <returns></returns>
        public static string FilterImg(this string strhtml)
        {
            string stroutput = strhtml;
            Regex regex = new Regex(@"<img[^>]+>|</[^>]+img>");

            stroutput = regex.Replace(stroutput, "");
            return stroutput;
        }
        #endregion

        #region FilterHtmlAndConvert 去除HTML标记
        /// <summary>
        /// 去除Html和JS标记并替换转义字符(如将&nbsp;替换成空格)
        /// </summary>
        /// <param name="Htmlstring">包括HTML的源码 </param>
        /// <returns>已转换后的文字</returns>
        public static string FilterHtmlAndConvert(this string Htmlstring)
        {
            if (Htmlstring.IsNullOrEmpty()) return Htmlstring;
            //删除脚本

            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);

            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring = Htmlstring.Replace("<", "");
            Htmlstring = Htmlstring.Replace(">", "");
            Htmlstring = Htmlstring.Replace("\r\n", "");
            Htmlstring = Htmlstring.Replace("&nbsp;", ".");
            if (HttpContext.Current != null)
            {
                Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            }
            return Htmlstring;
        }
        #endregion

        #region FilterHtml
        /// <summary>
        /// 过滤html和JS字符
        /// </summary>
        /// <param name="strhtml">html格式字符串</param>
        /// <returns></returns>
        public static string FilterHtml(this string html)
        {
            //System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex(@"<script[\s\S]+</script *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex2 = new System.Text.RegularExpressions.Regex(@" href *= *[\s\S]*script *:", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex3 = new System.Text.RegularExpressions.Regex(@" no[\s\S]*=", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex4 = new System.Text.RegularExpressions.Regex(@"<iframe[\s\S]+</iframe *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex5 = new System.Text.RegularExpressions.Regex(@"<frameset[\s\S]+</frameset *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex6 = new System.Text.RegularExpressions.Regex(@"\<img[^\>]+\>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex7 = new System.Text.RegularExpressions.Regex(@"</p>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex8 = new System.Text.RegularExpressions.Regex(@"<p>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex9 = new System.Text.RegularExpressions.Regex(@"<[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            html = regex2.Replace(html, ""); //过滤href=javascript: (<A>) 属性
            html = regex3.Replace(html, " _disibledevent="); //过滤其它控件的on...事件
            html = regex4.Replace(html, ""); //过滤iframe
            html = regex5.Replace(html, ""); //过滤frameset
            html = regex6.Replace(html, ""); //过滤frameset
            html = regex7.Replace(html, ""); //过滤frameset
            html = regex8.Replace(html, ""); //过滤frameset
            html = regex9.Replace(html, "");
            html = html.Replace(" ", "");
            html = html.Replace("</strong>", "");
            html = html.Replace("<strong>", "");
            return html;
        }
        #endregion

        #region FilterHtmlForMobile 手机浏览器去除HTML标记
        /// <summary>
        /// 手机浏览器去除HTML标记
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string FilterHtmlForMobile(this string html)
        {
            System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex(@"<script[\s\S]+</script *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex2 = new System.Text.RegularExpressions.Regex(@" href *= *[\s\S]*script *:", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex3 = new System.Text.RegularExpressions.Regex(@" no[\s\S]*=", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex4 = new System.Text.RegularExpressions.Regex(@"<iframe[\s\S]+</iframe *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex5 = new System.Text.RegularExpressions.Regex(@"<frameset[\s\S]+</frameset *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            //System.Text.RegularExpressions.Regex regex6 = new System.Text.RegularExpressions.Regex(@"\<img[^\>]+\>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex7 = new System.Text.RegularExpressions.Regex(@"</p>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex8 = new System.Text.RegularExpressions.Regex(@"<p>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex9 = new System.Text.RegularExpressions.Regex(@"(?i)<(?!img\b)[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex1.Replace(html, ""); //过滤<script></script>标记
            html = regex2.Replace(html, ""); //过滤href=javascript: (<A>) 属性
            html = regex3.Replace(html, " _disibledevent="); //过滤其它控件的on...事件
            html = regex4.Replace(html, ""); //过滤iframe
            html = regex5.Replace(html, ""); //过滤frameset
            //html = regex6.Replace(html, "");
            html = regex7.Replace(html, ""); //过滤frameset
            html = regex8.Replace(html, ""); //过滤frameset
            html = regex9.Replace(html, "");
            html = html.Replace("</strong>", "");
            html = html.Replace("<strong>", "");
            html = html.Replace("<img", "<br /><img onload=\"javascript:ResizePic(this)\" ");
            return html;
        }
        #endregion

        #region GreateMiniImage 生成缩略图
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="oldImagePath">Server.MapPath(原图片地址)</param>
        /// <param name="newImagePath">Server.MapPath(新图片地址)</param>
        /// <param name="width">缩略图的宽</param>
        /// <param name="height">缩略图的高</param>
        /// <param name="Mode">W H Cut</param>
        public static void GreateMiniImage(string oldImagePath, string newImagePath, int width, int height, string Mode)
        {
            System.Drawing.Image oldImage = null;
            System.Drawing.Image bitmap = null;
            bool isCut = true;
            try
            {
                oldImage = System.Drawing.Image.FromFile(oldImagePath);
                int towidth = width;
                int toheight = height;

                int x = 0;
                int y = 0;
                int ow = oldImage.Width;
                int oh = oldImage.Height;

                switch (Mode)
                {
                    case "HW"://指定高宽缩放（可能变形）                
                        break;
                    case "W"://指定宽，高按比例                    
                        toheight = oldImage.Height * width / oldImage.Width;
                        break;
                    case "H"://指定高，宽按比例
                        towidth = oldImage.Width * height / oldImage.Height;
                        break;
                    case "Cut"://指定高宽裁减（不变形）

                        if ((double)oldImage.Width / (double)oldImage.Height > (double)towidth / (double)toheight)
                        {
                            if (oldImage.Width < towidth)
                            {
                                isCut = false;

                                oh = oldImage.Height;
                                ow = oldImage.Width;
                                y = 0;
                                x = (width - ow) / 2;

                                if (oldImage.Height < toheight)
                                {
                                    y = (height - oh) / 2;
                                }
                            }
                            else
                            {
                                oh = oldImage.Height;
                                ow = oldImage.Height * towidth / toheight;
                                y = 0;
                                x = (oldImage.Width - ow) / 2;
                            }
                        }
                        else
                        {
                            if (oldImage.Height < toheight)
                            {
                                isCut = false;

                                ow = oldImage.Width;
                                oh = oldImage.Height;
                                x = 0;
                                y = (height - oh) / 2;
                                if (oldImage.Width < towidth)
                                {
                                    x = (width - ow) / 2;
                                }
                            }
                            else
                            {
                                ow = oldImage.Width;
                                oh = oldImage.Width * height / towidth;
                                x = 0;
                                y = (oldImage.Height - oh) / 2;
                            }
                        }
                        break;
                    default:
                        break;
                }

                //新建一个bmp图片
                bitmap = new System.Drawing.Bitmap(towidth, toheight);

                //新建一个画板
                using (Graphics g = System.Drawing.Graphics.FromImage(bitmap))
                {
                    //设置高质量插值法
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                    //设置高质量,低速度呈现平滑程度
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    //清空画布并以透明背景色填充
                    g.Clear(Color.White);

                    //在指定位置并且按指定大小绘制原图片的指定部分
                    if (isCut)
                    {
                        g.DrawImage(oldImage, new Rectangle(0, 0, towidth, toheight), new Rectangle(x, y, ow, oh), GraphicsUnit.Pixel);
                    }
                    else
                    {
                        g.DrawImage(oldImage, new Rectangle(x, y, ow, oh), new Rectangle(0, 0, ow, oh), GraphicsUnit.Pixel);
                    }

                    //以jpg格式保存缩略图
                    oldImage.Dispose();
                    if (File.Exists(newImagePath))
                    {
                        File.Delete(newImagePath);
                    }
                    bitmap.Save(newImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
            catch
            {
                
            }
            finally
            {
                if (bitmap != null)
                {
                    bitmap.Dispose();
                }
            }
        }
        #endregion

        #region AddWater 添加水印
        /// <summary>
        /// 添加水印
        /// </summary>
        /// <param name="oldPath">原图片绝对地址</param>
        /// <param name="newPath">新图片放置的绝对地址</param>
        ///  <param name="waterType">什么水　I　图　T 文字</param>
        ///  <param name="waterPath">水印图片路径</param>
        ///  <param name="waterText">水印文字</param>
        public static void AddWater(string oldPath, string newPath, string waterType, string waterPath, string waterText)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(oldPath);
            Bitmap b = new Bitmap(image.Width, image.Height);
            try
            {
                using (Graphics g = Graphics.FromImage(b))
                {
                    g.Clear(Color.White);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    g.DrawImage(image, 0, 0, image.Width, image.Height);

                    switch (waterType)
                    {
                        //是图片的话               
                        case "I":
                            AddWaterImage(g, waterPath, "BR", image.Width, image.Height);
                            break;
                        //如果是文字                    
                        case "T":
                            AddWaterText(g, waterText, "BR", image.Width, image.Height);
                            break;
                    }
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                image.Dispose();
                if (File.Exists(newPath))
                {
                    File.Delete(newPath);
                }
                b.Save(newPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                b.Dispose();
            }
        }
        #endregion

        #region AddWaterText 加水印文字
        /// <summary>
        ///  加水印文字
        /// </summary>
        /// <param name="picture">imge 对象</param>
        /// <param name="_watermarkText">水印文字内容</param>
        /// <param name="_watermarkPosition">水印位置 TL TR BR BL</param>
        /// <param name="_width">被加水印图片的宽</param>
        /// <param name="_height">被加水印图片的高</param>
        public static void AddWaterText(Graphics picture, string _watermarkText, string _watermarkPosition, int _width, int _height)
        {
            int[] sizes = new int[] { 30, 20, 10 };
            Font crFont = null;
            SizeF crSize = new SizeF();
            for (int n = 0; n < sizes.Length; n++)
            {
                crFont = new Font("arial", sizes[n], FontStyle.Bold);
                crSize = picture.MeasureString(_watermarkText, crFont);

                if ((ushort)crSize.Width < (ushort)_width)
                    break;
            }

            float xpos = 0;
            float ypos = 0;

            switch (_watermarkPosition)
            {
                case "TL":
                    xpos = ((float)_width * (float).01) + (crSize.Width / 2);
                    ypos = (float)_height * (float).01;
                    break;
                case "TR":
                    xpos = ((float)_width * (float).99) - (crSize.Width / 2);
                    ypos = (float)_height * (float).01;
                    break;
                case "BR":
                    xpos = ((float)_width * (float).99) - (crSize.Width / 2);
                    ypos = ((float)_height * (float).99) - crSize.Height;
                    break;
                case "BL":
                    xpos = ((float)_width * (float).01) + (crSize.Width / 2);
                    ypos = ((float)_height * (float).99) - crSize.Height;
                    break;
            }

            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;

            SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(153, 0, 0, 0));
            picture.DrawString(_watermarkText, crFont, semiTransBrush2, xpos + 1, ypos + 1, StrFormat);

            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255));
            picture.DrawString(_watermarkText, crFont, semiTransBrush, xpos, ypos, StrFormat);


            semiTransBrush2.Dispose();
            semiTransBrush.Dispose();
        }
        #endregion

        #region AddWaterImage 加水印图片
        /// <summary>
        ///  加水印图片
        /// </summary>
        /// <param name="picture">imge 对象</param>
        /// <param name="WaterMarkPicPath">水印图片的地址</param>
        /// <param name="_watermarkPosition">水印位置 TL TR BL BR</param>
        /// <param name="_width">被加水印图片的宽</param>
        /// <param name="_height">被加水印图片的高</param>
        public static void AddWaterImage(Graphics picture, string WaterMarkPicPath, string _watermarkPosition, int _width, int _height)
        {
            System.Drawing.Image watermark = new Bitmap(WaterMarkPicPath);
            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap colorMap = new ColorMap();
            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
            ColorMap[] remapTable = { colorMap };
            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);
            float[][] colorMatrixElements = {
                                                new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                                 new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                                new float[] {0.0f,  0.0f,  0.0f,  0.5f, 0.0f},
                                                 new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                             };

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            int xpos = 0;
            int ypos = 0;
            int WatermarkWidth = 0;
            int WatermarkHeight = 0;
            double bl = 1d;
            //计算水印图片的比率
            //取背景的1/4宽度来比较
            //if ((_width > watermark.Width * 4) && (_height > watermark.Height * 4))
            //{
            //    bl = 1;
            //}
            //else if ((_width > watermark.Width * 4) && (_height < watermark.Height * 4))
            //{
            //    bl = Convert.ToDouble(_height / 4) / Convert.ToDouble(watermark.Height);
            //}
            //else
            //{
            //    if ((_width < watermark.Width * 4) && (_height > watermark.Height * 4))
            //    {
            //        bl = Convert.ToDouble(_width / 4) / Convert.ToDouble(watermark.Width);
            //    }
            //    else
            //    {
            //        if ((_width * watermark.Height) > (_height * watermark.Width))
            //        {
            //            bl = Convert.ToDouble(_height / 4) / Convert.ToDouble(watermark.Height);
            //        }
            //        else
            //        {
            //            bl = Convert.ToDouble(_width / 4) / Convert.ToDouble(watermark.Width);

            //        }
            //    }
            //}

            WatermarkWidth = Convert.ToInt32(watermark.Width * bl);
            WatermarkHeight = Convert.ToInt32(watermark.Height * bl);

            switch (_watermarkPosition)
            {
                case "TL":
                    xpos = 10;
                    ypos = 10;
                    break;
                case "TR":
                    xpos = _width - WatermarkWidth - 10;
                    ypos = 10;
                    break;
                case "BR":
                    xpos = _width - WatermarkWidth - 10;
                    ypos = _height - WatermarkHeight - 10;
                    break;
                case "BL":
                    xpos = 10;
                    ypos = _height - WatermarkHeight - 10;
                    break;
                case "CE":
                    xpos = (_width - WatermarkWidth) / 2;
                    ypos = (_height - WatermarkHeight) / 2;
                    break;
            }

            picture.DrawImage(watermark, new Rectangle(xpos, ypos, WatermarkWidth, WatermarkHeight), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);
            watermark.Dispose();
            imageAttributes.Dispose();
        }
        #endregion

        #region 生成验证码
        /// <summary>
        /// 生成图片验证码
        /// </summary>
        /// <param name="code">验证码字符</param>
        /// <param name="length">验证码长度</param>
        /// <returns>生成的验证码</returns>
        public static byte[] CreateValidateCode(out string code, int length = 4)
        {
            var valid = new MFValidateCode();
            code = valid.CreateVerifyCode(length);
            return valid.CreateImageCode(code);
        }
        #endregion

        #region GetDisplayEMail 获取email的显示格式
        /// <summary>
        /// 获取email的显示格式
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static string GetDisplayEMail(string email)
        {
            string strDisplay = string.Empty;
            string strMailF = email.Substring(0, email.LastIndexOf('@'));
            if (strMailF.Length >= 5)
            {
                strDisplay = strMailF.Substring(0, 3) + GetStar(strMailF.Length - 3) + email.Substring(email.LastIndexOf('@'));
            }
            else
            {
                strDisplay = strMailF.Substring(0, 1) + GetStar(strMailF.Length - 1) + email.Substring(email.LastIndexOf('@'));
            }
            return strDisplay;
        }

        private static string GetStar(int length)
        {
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                s.Append("*");
            }
            return s.ToString();
        }
        #endregion
    }
}