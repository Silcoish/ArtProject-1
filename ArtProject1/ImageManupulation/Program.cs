using System;
using System.Drawing;
using System.IO;

namespace ImageManupulation
{
    class Program
    {
        public static int horizontalSquares = 10;
        public static int verticalSquares = 13;
        public static int width = 72;
        public static int border = 12;
        
        
        static void Main(string[] args)
        {
            using (FileStream pngFileStream = new FileStream(args[0], FileMode.Open, FileAccess.Read))
            {
                using (var image = new Bitmap(pngFileStream))
                {
                    for (int i = 0; i < horizontalSquares; i++)
                    {
                        for (int j = 0; j < verticalSquares; j++)
                        {
                            int startX = i * width + border;
                            int startY = j * width + border;
                            var color = CalculateMeanColour(image, startX, startY, width - (border * 2),
                                width - (border * 2));
                            for (int x = 0; x < (width - 2*border); x++)
                            {
                                for (int y = 0; y < (width - 2 * border); y++)
                                {
                                    image.SetPixel(startX + x, startY + y, color);
                                }
                            }
                        }
                    }

                    image.Save(args[1], image.RawFormat);
                }
            }
        }

        public static Color CalculateMeanColour(Bitmap image, int startX, int startY, int width, int height)
        {
            (double, double, double) colours = (0, 0, 0);
            
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    Color pixelColor = image.GetPixel(startX + x, startY + y);
                    colours.Item1 += pixelColor.R;
                    colours.Item2 += pixelColor.G;
                    colours.Item3 += pixelColor.B;
                }
            }

            int total = width * height;

            return Color.FromArgb(
                (int) (Math.Clamp(Math.Floor(colours.Item1 / total) - 10, 0, 255)),
                (int) (Math.Clamp(Math.Floor(colours.Item2 / total) - 10, 0, 255)),
                (int) (Math.Clamp(Math.Floor(colours.Item3 / total) - 10, 0, 255))
            );
        }
    }
}