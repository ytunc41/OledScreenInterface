using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OledScreen
{
    public static class Helper
    {
        public static Bitmap ImageResize(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return destImage;
        }

        public static Bitmap ImageConvertToGrayScale(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            using (Graphics g = Graphics.FromImage(newBitmap))
            {

                //create the grayscale ColorMatrix
                ColorMatrix colorMatrix = new ColorMatrix(
                   new float[][]
                   {
                        new float[] {.3f, .3f, .3f, 0, 0},
                        new float[] {.59f, .59f, .59f, 0, 0},
                        new float[] {.11f, .11f, .11f, 0, 0},
                        new float[] {0, 0, 0, 1, 0},
                        new float[] {0, 0, 0, 0, 1}
                   });

                //create some image attributes
                using (ImageAttributes attributes = new ImageAttributes())
                {

                    //set the color matrix attribute
                    attributes.SetColorMatrix(colorMatrix);

                    //draw the original image on the new image
                    //using the grayscale color matrix
                    g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                                0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
                }
            }
            return newBitmap;
        }

        public static Bitmap ConvertTo1BppImage(Bitmap srcBitmap)
        {
            //Be sure to have / create a Format32bppPArgb bitmap and resize it
            if (srcBitmap.PixelFormat != PixelFormat.Format32bppPArgb)
            {
                Bitmap temp = new Bitmap(srcBitmap.Width, srcBitmap.Height, PixelFormat.Format32bppPArgb);
                temp.SetResolution(200, 200);
                Graphics g = Graphics.FromImage(temp);
                g.DrawImage(srcBitmap, new Rectangle(0, 0, temp.Width, temp.Height), 0, 0, srcBitmap.Width, srcBitmap.Height, GraphicsUnit.Pixel);
                srcBitmap.Dispose();
                g.Dispose();
                srcBitmap = temp;
            }

            //lock the bits of the original bitmap
            BitmapData bmdo = srcBitmap.LockBits(new Rectangle(0, 0, srcBitmap.Width, srcBitmap.Height), ImageLockMode.ReadOnly, srcBitmap.PixelFormat);

            //and the new 1bpp bitmap
            Bitmap destBitmap = new Bitmap(srcBitmap.Width, srcBitmap.Height, PixelFormat.Format1bppIndexed);
            BitmapData bmdn = destBitmap.LockBits(new Rectangle(0, 0, destBitmap.Width, destBitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format1bppIndexed);
            destBitmap.SetResolution(200, 200); //I want a resolution of 200dpi x 200dpi

            //scan through the pixels Y by X
            int x, y;

            for (x = 0; x < srcBitmap.Width; x++)
            {
                for (y = 0; y < srcBitmap.Height; y++)
                {
                    //generate the address of the colour pixel

                    int index = y * bmdo.Stride + (x * 4);
                    float myBrigthness = Color.FromArgb(Marshal.ReadByte(bmdo.Scan0, index + 2),
                                     Marshal.ReadByte(bmdo.Scan0, index + 1),
                                     Marshal.ReadByte(bmdo.Scan0, index)).GetBrightness();

                    //check its brightness and if between 0.33f and 0.67f create fake gray by alternating black and white (note that you have to cross depending on x-y position)
                    if (myBrigthness < 0.33f)
                    {
                        //Black thus do nothing
                    }
                    else if (myBrigthness < 0.67f)
                    {
                        if ((x + (y % 2)) % 2 == 1)
                        {
                            //Black thus do nothing
                        }
                        else
                        {
                            //White
                            SetIndexedPixel(x, y, bmdn, true); //set it if its bright.
                        }
                    }
                    else
                    {
                        //White
                        SetIndexedPixel(x, y, bmdn, true); //set it if its bright.
                    }
                }
            }

            //tidy up
            destBitmap.UnlockBits(bmdn);
            srcBitmap.UnlockBits(bmdo);

            return destBitmap;
        }

        private static void SetIndexedPixel(int x, int y, BitmapData bmd, bool pixel)
        {
            int index = y * bmd.Stride + (x >> 3);
            byte p = Marshal.ReadByte(bmd.Scan0, index);
            byte mask = (byte)(0x80 >> (x & 0x7));
            if (pixel)
                p |= mask;
            else
                p &= (byte)(mask ^ 0xff);
            Marshal.WriteByte(bmd.Scan0, index, p);
        }

        public static Bitmap CreateNonIndexedImage(Image src)
        {
            Bitmap newBmp = new Bitmap(src.Width, src.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using (Graphics gfx = Graphics.FromImage(newBmp))
            {
                gfx.DrawImage(src, 0, 0);
            }

            return newBmp;
        }

        public static byte[] ImageToByteArray(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        public static void DrawPixel(int x, int y, int color, ref byte[] buffer)
        {
            int width = ResizeImage.Width;
            if (color == 0)
                buffer[x + (y / 8 * width)] |= (byte)(1 << (y % 8));
            else
                buffer[x + (y / 8 * width)] &= (byte)~(1 << (y % 8));
        }

        public static Thread threadIsConnect;

        public static Stopwatch stopWatch = new Stopwatch();
        public static List<string> comNames = new List<string>();

        public static void SerialPortDetect()
        {
            comNames.Clear();
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PnPEntity");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    if (queryObj["Caption"] != null)
                    {
                        if (queryObj["Caption"].ToString().Contains("(COM"))
                        {
                            comNames.Add(queryObj["Name"].ToString());
                        }
                    }
                }
            }
            catch (ManagementException)
            {
                MessageBox.Show("Unknown Error");
            }
        }



    }

    
}
