using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Solo.Utils;

namespace Solo.d2D
{
    public struct Tile
    {
        public string Name;
        public int X;
        public int Y;
        public float Layer; // глубина при отрисовке.

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="layer">between 0 and 1. depth</param>
        public Tile(string name, int x, int y, float layer)
        {
            Name = name;
            X = x;
            Y = y;
            Layer = layer;
        }        
    }
}