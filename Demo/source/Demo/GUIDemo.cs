using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Solo.Utils;
using Solo.GUI;
using Solo.d2D;

namespace Demo
{
    class GUIDemo : Scene
    {
        GUIStyle style;
        GUIManager gui;

        string label = "[Backspace] - Вернуться в меню";

        public GUIDemo(Config cfg) : base(cfg)
        {
            isInited = false;
        }

        // Переопределить Init, [Обязательно]  всё инициализировать в Init
        public override void Init(ContentManager content, GraphicsDeviceManager graphics)
        {
            if (!isInited) // Чтобы инициализация проходила только один раз
            {
                base.Init(content, graphics); // [Обязательно] Запускаем стандартный Start
                camera = new Camera2D(0, Vector2.Zero, new Point(cfg.Ints["window width"], cfg.Ints["window height"])); // [Обязательно]  инициализация камеры
                textures.Add("gui", content.Load<Texture2D>("gui")); // текстура с gui
                textures.Add("spritepack", content.Load<Texture2D>("spritepack")); // текстура с тайлами

                SpriteFont font = content.Load<SpriteFont>("Arial");
                style = new GUIStyle("style", "gui", font); // для стиля GUI необходимо указать имя текстуры в массиве с текстурами и шрифт
                gui = new GUIManager(style); // Новый гуи менеджер

                isInited = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Back))
            {
                Stop();
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            graphics.GraphicsDevice.Clear(Color.DarkGray); // перерисовка фона
            base.Draw(gameTime, spriteBatch, graphics); // Базовый дроу   

            gui.ButtonsReset(); // [Обязательно] перед вызовом кнопок
            //Отрисовка интерфейса
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            gui.Label(spriteBatch, new Vector2(20, 30), "Label with background", textures, true);
            gui.Label(spriteBatch, new Vector2(290, 30), "gui.Label(spriteBatch, new Vector2(20, 30), \"Label with background\", textures, true);", textures, true);
            gui.Label(spriteBatch, new Vector2(20, 60), "Label without background", textures, false);
            gui.Label(spriteBatch, new Vector2(290, 60), "gui.Label(spriteBatch, new Vector2(20, 60), \"Label with background\", textures, false);", textures, true);
            gui.Button(spriteBatch, new Rectangle(20, 90, 250, 24), "Button with notice", "notice", textures);
            gui.Label(spriteBatch, new Vector2(290, 90), "gui.Button(spriteBatch, new Rectangle(20, 90, 250, 24), \"Button with notice\", \"notice\", textures);", textures, true);
            gui.Button(spriteBatch, new Rectangle(20, 120, 250, 24), "Button without notice", null, textures);
            gui.Label(spriteBatch, new Vector2(290, 120), "gui.Button(spriteBatch, new Rectangle(20, 120, 250, 24), \"Button without notice\", null, textures);", textures, true);
            gui.ButtonBlocked(spriteBatch, new Rectangle(20, 150, 250, 24), "Button blocked", textures);
            gui.Label(spriteBatch, new Vector2(290, 150), "gui.ButtonBlocked(spriteBatch, new Rectangle(20, 150, 250, 24), \"Button blocked\", textures);", textures, true);
            gui.Button(spriteBatch, new Rectangle(20, 180, 40, 40), "Button with icon", new Rectangle(0, 128, 64, 64), "spritepack", textures);
            gui.Label(spriteBatch, new Vector2(290, 180), "gui.Button(spriteBatch, new Rectangle(20, 180, 40, 40),\"Button with icon\",\n new Rectangle(0, 128, 64, 64), \"spritepack\", textures);", textures, true);
            gui.Frame(spriteBatch, new Rectangle(20, 230, 250, 40), textures);
            gui.Label(spriteBatch, new Vector2(290, 230), "gui.Frame(spriteBatch, new Rectangle(20, 230, 250, 40), textures);", textures, true);

            gui.Label(spriteBatch, new Vector2(cfg.Ints["window width"] / 2 - style.Font.MeasureString(label).X / 2, cfg.Ints["window height"] - 35), label, textures, true);
            spriteBatch.End();
        }
    }
}
