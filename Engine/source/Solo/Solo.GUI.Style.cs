using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Solo.GUI
{
    public struct GUIStyle
    {
        public string Name;
        public string TextureName;
        public SpriteFont Font;

        public Rectangle Button;
        public Color ButtonTextColor;
        public Rectangle ButtonHover;
        public Color ButtonHoverTextColor;
        public Rectangle ButtonActive;
        public Color ButtonActiveTextColor;
        public Rectangle ButtonBlocked;
        public Color ButtonBlockedTextColor;
        public Rectangle Notice;
        public Color NoticeTextColor;


        public GUIStyle(string name, string textureName,SpriteFont font)
        {
            Name = name;
            TextureName = textureName;
            Font = font;
            Button = new Rectangle(0, 0, 8, 8);
            ButtonTextColor = Color.Black;
            ButtonHover = new Rectangle(0, 8, 8, 8);
            ButtonHoverTextColor = Color.White;
            ButtonActive = new Rectangle(0, 16, 8, 8);
            ButtonActiveTextColor = Color.White;
            ButtonBlocked = new Rectangle(0, 24, 8, 8);
            ButtonBlockedTextColor = Color.Gray;
            Notice = new Rectangle(0, 32, 8, 8);
            NoticeTextColor = Color.Black;
        }   
    }
}
