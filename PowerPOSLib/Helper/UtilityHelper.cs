using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ComponentModel;
using System.Drawing;

namespace PowerPOSLib.Helper
{
    public static class UtilityHelper
    {
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public static Bitmap ImageSharpen(Bitmap image)
        {
            Bitmap sharpenImage = new Bitmap(image.Width, image.Height);

            int filterWidth = 3;
            int filterHeight = 3;
            int w = image.Width;
            int h = image.Height;

            double[,] filter = new double[filterWidth, filterHeight];

            filter[0, 0] = filter[0, 1] = filter[0, 2] = filter[1, 0] = filter[1, 2] = filter[2, 0] = filter[2, 1] = filter[2, 2] = -1;
            filter[1, 1] = 9;

            double factor = 1.0;
            double bias = 0.0;

            Color[,] result = new Color[image.Width, image.Height];

            for (int x = 0; x < w; ++x)
            {
                for (int y = 0; y < h; ++y)
                {
                    double red = 0.0, green = 0.0, blue = 0.0;

                    for (int filterX = 0; filterX < filterWidth; filterX++)
                    {
                        for (int filterY = 0; filterY < filterHeight; filterY++)
                        {
                            int imageX = (x - filterWidth / 2 + filterX + w) % w;
                            int imageY = (y - filterHeight / 2 + filterY + h) % h;

                            //=====[INSERT LINES]========================================================
                            // Get the color here - once per fiter entry and image pixel.
                            Color imageColor = image.GetPixel(imageX, imageY);
                            //===========================================================================

                            red += imageColor.R * filter[filterX, filterY];
                            green += imageColor.G * filter[filterX, filterY];
                            blue += imageColor.B * filter[filterX, filterY];
                        }
                        int r = Math.Min(Math.Max((int)(factor * red + bias), 0), 255);
                        int g = Math.Min(Math.Max((int)(factor * green + bias), 0), 255);
                        int b = Math.Min(Math.Max((int)(factor * blue + bias), 0), 255);

                        result[x, y] = Color.FromArgb(r, g, b);
                    }
                }
            }
            for (int i = 0; i < w; ++i)
            {
                for (int j = 0; j < h; ++j)
                {
                    sharpenImage.SetPixel(i, j, result[i, j]);
                }
            }
            return sharpenImage;
        }
    }
}
