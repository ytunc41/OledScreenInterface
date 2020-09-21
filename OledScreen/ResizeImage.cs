using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OledScreen
{
    public static class ResizeImage
    {
        private static string _filePath;
        public static string filePath { get => _filePath; set => _filePath = value; }

        private static int _width;
        public static int Width { get => _width; set => _width = value; }
        private static int _height;
        public static int Height { get => _height; set => _height = value; }

        private static int _maxWidth;
        public static int MaxWidth { get => _maxWidth; set => _maxWidth = value; }
        private static int _maxHeight;
        public static int MaxHeight { get => _maxHeight; set => _maxHeight = value; }

        private static int _pageNumber;
        public static int PageNumber { get { _pageNumber = MaxHeight / 8; return _pageNumber; } private set => _pageNumber = value; }

        private static byte[] _imageBuffer;
        public static byte[] ImageBuffer { get => _imageBuffer; set => _imageBuffer = value; }

        private static List<byte> _imageFullBuffer = new List<byte>();
        public static List<byte> ImageFullBuffer { get { return _imageFullBuffer; } set { _imageFullBuffer = value; } }

        private static uint _CRC32;
        public static uint CRC32 { get => _CRC32; set => _CRC32 = value; }

        public static void ClearAll()
        {
            ImageFullBuffer.Clear();
            Width = 0;
            Height = 0;
            CRC32 = 0;
        }


    }
}
