using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Solo.Utils;

namespace Solo.GUI
{
    public class GUIManager
    {
        public GUIStyle Style;

        public Rectangle Hover_target = new Rectangle(-50, -50, 5, 5);
        public InputType Input = InputType.Mouse;
        private Rectangle button_target = new Rectangle();
        private bool mousePressed = false;
        private bool EnterPressed = false;
        private bool APressed = false;

        public event GUI_button button_hovered;
        public event GUI_button button_unhovered;

        public GUIManager(GUIStyle style)
        {
            Style = style;
        }

        private bool Check_hover(Rectangle collider)
        {
            Point position = Mouse.GetState().Position;

            if (Hover_target.Intersects(collider))
                return true;
            if (Input == InputType.Mouse)
                if (position.X > collider.X && position.Y > collider.Y && position.X < collider.Right && position.Y < collider.Bottom)
                    return true;
            return false;
        }

        private bool Check_target(Rectangle collider)
        {
            bool answ = false;
                if (button_target.Intersects(collider))
                answ = true;
            return answ;
        }

        public void ResetHover()
        {
            Hover_target.X = -50; Hover_target.Y = -50;
        }

        public void ButtonsReset()
        {
            button_unhovered?.Invoke();
        }

        public void Frame(SpriteBatch spriteBatch, Rectangle collider, Dictionary<string, Texture2D> textures)
        {
            spriteBatch.Draw(textures[Style.TextureName], collider, Style.Notice, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.7f);

            spriteBatch.Draw(textures[Style.TextureName], new Rectangle(collider.X, collider.Y, Style.FrameTopLeft.Width, Style.FrameTopLeft.Height), 
                                                                        Style.FrameTopLeft, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.8f);
            spriteBatch.Draw(textures[Style.TextureName], new Rectangle(collider.X + Style.FrameTopLeft.Width, collider.Y, collider.Width - Style.FrameTopLeft.Width - Style.FrameTopRight.Width, Style.FrameTop.Height), 
                                                                        Style.FrameTop, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.8f);
            spriteBatch.Draw(textures[Style.TextureName], new Rectangle(collider.Right - Style.FrameTopRight.Width, collider.Y, Style.FrameTopRight.Width, Style.FrameTopRight.Height),
                                                                        Style.FrameTopRight, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.8f);

            spriteBatch.Draw(textures[Style.TextureName], new Rectangle(collider.X, collider.Y + Style.FrameLeft.Height, Style.FrameLeft.Width, collider.Height - Style.FrameTopLeft.Height - Style.FrameBottomLeft.Height),
                                                                        Style.FrameLeft, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.8f);
            spriteBatch.Draw(textures[Style.TextureName], new Rectangle(collider.Right - Style.FrameRight.Width, collider.Y + Style.FrameRight.Height, Style.FrameRight.Width, collider.Height - Style.FrameTopRight.Height - Style.FrameBottomRight.Height),
                                                                        Style.FrameRight, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.8f);


            spriteBatch.Draw(textures[Style.TextureName], new Rectangle(collider.X, collider.Bottom - Style.FrameBottomLeft.Height, Style.FrameBottomLeft.Width, Style.FrameBottomLeft.Height),
                                                                        Style.FrameBottomLeft, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.8f);
            spriteBatch.Draw(textures[Style.TextureName], new Rectangle(collider.X + Style.FrameBottomLeft.Width, collider.Bottom - Style.FrameBottom.Height, collider.Width - Style.FrameBottomLeft.Width - Style.FrameBottomRight.Width, Style.FrameBottom.Height),
                                                                        Style.FrameBottom, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.8f);
            spriteBatch.Draw(textures[Style.TextureName], new Rectangle(collider.Right - Style.FrameBottomRight.Width, collider.Bottom - Style.FrameBottomRight.Height, Style.FrameBottomRight.Width, Style.FrameBottomRight.Height),
                                                                        Style.FrameBottomRight, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.8f);
        }

        public bool Button(SpriteBatch spriteBatch, Rectangle collider, string text, string note, Dictionary<string, Texture2D> textures)
        {
            if (!Check_hover(collider))
            {
                spriteBatch.Draw(textures[Style.TextureName], collider, Style.Button, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.8f);
                spriteBatch.DrawString(Style.Font, text, new Vector2(collider.X + 10, collider.Y + 3), Style.ButtonTextColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);
            }
            else
            {
                button_hovered?.Invoke();
                if (note != null)
                    Notice(spriteBatch, note, textures, true);
                spriteBatch.Draw(textures[Style.TextureName], collider, Style.ButtonHover, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.8f);
                spriteBatch.DrawString(Style.Font, text, new Vector2(collider.X + 10, collider.Y + 3), Style.ButtonHoverTextColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);
                if (Mouse.GetState().LeftButton == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter) || GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
                {                    
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                        mousePressed = true;
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                        EnterPressed = true;
                    if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
                        APressed = true;
                    spriteBatch.Draw(textures[Style.TextureName], collider, Style.ButtonActive, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.9f);
                    spriteBatch.DrawString(Style.Font, text, new Vector2(collider.X + 10, collider.Y + 3), Style.ButtonActiveTextColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.92f);
                    if (!Check_target(collider))
                    {
                        if (Input == InputType.Mouse)
                        {
                            Point position = Mouse.GetState().Position;
                            button_target = new Rectangle(position.X, position.Y, 5, 5);
                        }
                        else
                        {
                            button_target = Hover_target;
                        }
                        return true;
                    }                    
                }
            }

            if (Mouse.GetState().LeftButton == ButtonState.Released && mousePressed)
            {
                button_target = new Rectangle();
                mousePressed = false;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Enter) && EnterPressed)
            {
                button_target = new Rectangle();
                EnterPressed = false;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Released && APressed)
            {
                button_target = new Rectangle();
                APressed = false;
            }
            return false;
        }

        public bool Button(SpriteBatch spriteBatch, Rectangle collider, string note, Rectangle iconSourceRectangle, string iconTextureName, Dictionary<string, Texture2D> textures)
        {
            bool r = Button(spriteBatch, collider, "", note, textures);
            spriteBatch.Draw(textures[iconTextureName], new Rectangle(collider.X + 5, collider.Y +5, collider.Width - 10, collider.Height - 10), iconSourceRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.93f);
            return r;
        }

        public void ButtonBlocked(SpriteBatch spriteBatch, Rectangle collider, string text, Dictionary<string, Texture2D> textures)
        {
            spriteBatch.Draw(textures[Style.TextureName], collider, Style.ButtonBlocked, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.8f);
            spriteBatch.DrawString(Style.Font, text, new Vector2(collider.X + 10, collider.Y + 3), Style.ButtonBlockedTextColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);
        }

        public void Notice(SpriteBatch spriteBatch, string text, Dictionary<string, Texture2D> textures, bool useBackGround)
        {
            Point mouse_position = Mouse.GetState().Position;
            if (useBackGround)
            {
                Vector2 size = Style.Font.MeasureString(text);
                spriteBatch.Draw(textures[Style.TextureName], new Rectangle(mouse_position.X, mouse_position.Y - 24, (int)size.X + 8, (int)size.Y + 4), Style.Notice, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.99f);
            }
            spriteBatch.DrawString(Style.Font, text, new Vector2(mouse_position.X + 4, mouse_position.Y - 22), Style.NoticeTextColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }

        public void Label(SpriteBatch spriteBatch, Vector2 position, string text, Dictionary<string, Texture2D> textures, bool useBackGround)
        {
            if (useBackGround)
            {
                Vector2 size = Style.Font.MeasureString(text);
                spriteBatch.Draw(textures[Style.TextureName], new Rectangle((int)position.X, (int)position.Y, (int)size.X + 8, (int)size.Y + 4), Style.Notice, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.8f);
            }
            spriteBatch.DrawString(Style.Font, text, new Vector2(position.X + 4, position.Y + 2), Style.NoticeTextColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.91f);
        }
    }

    public delegate void GUI_button();
}
