using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Solo.Physics2D;

namespace Solo.Utils
{
    static public class Drawing
    {
        /// <summary>
        /// Drawing new line between to points at a texture
        /// </summary>   
        static public void Line(Texture2D texture, Color color, Vector2 va, Vector2 vb)
        {
            //https://ru.wikipedia.org/wiki/Алгоритм_Брезенхэма
            //https://ru.wikibooks.org/wiki/Реализации_алгоритмов/Алгоритм_Брезенхэма

            Point a = new Point((int)va.X, (int)va.Y);
            Point b = new Point((int)vb.X, (int)vb.Y);

            int deltaX = Math.Abs(b.X - a.X);
            int deltaY = Math.Abs(b.Y - a.Y);
            int signX = Sign(b.X - a.X);
            int signY = Sign(b.Y - a.Y);
            int error = deltaX - deltaY;
            Color[] data = new Color[texture.Width * texture.Height];
            texture.GetData(data);
            int coor = b.X + b.Y * texture.Width;
            if (coor < data.Length && coor > 0)
                data[b.X + b.Y * texture.Width] = color;
            int x = a.X, y = a.Y;
            while (x != b.X || y != b.Y)
            {
                coor = x + y * texture.Width;
                if (coor < data.Length && coor > 0)
                    data[coor] = color;
                int error2 = error * 2;
                if (error2 > -deltaY)
                {
                    error -= deltaY;
                    x += signX;
                }
                if (error2 < deltaX)
                {
                    error += deltaX;
                    y += signY;
                }
            }  

            texture.SetData(data);
        }

        public static Texture2D MakeSolidColorTexture(Point size, GraphicsDeviceManager graphics, Color color)
        {
            Texture2D texture = new Texture2D(graphics.GraphicsDevice, size.X, size.Y);

            Color[] data = new Color[size.X * size.Y];
            for (int i = 0; i < data.Length; i++)
                data[i] = color;
            texture.SetData(data);
            return texture;
        }

        static private int Sign(int x)
        {
            return (x > 0) ? 1 : (x < 0) ? -1 : 0;
        }



    }
}
