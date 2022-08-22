using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Solo.Utils;
using Solo.GUI;
using Solo.d2D;
using Solo.Physics2D;

namespace Demo
{
    class PlatformerCameraScene : Scene
    {
        GUIStyle style;
        GUIManager gui;

        Shape[] shapes;

        string label = "[Стрелки] - ходить / [Backspace] - Вернуться в меню";

        public PlatformerCameraScene(Config cfg) : base(cfg)
        {
            isInited = false;
        }

        // Переопределить Init, [Обязательно]  всё инициализировать в Init
        public override void Init(ContentManager content, GraphicsDeviceManager graphics)
        {
            if (!isInited) // Чтобы инициализация проходила только один раз
            {
                base.Init(content, graphics);
                Size = new  Point(3840, Size.Y);
                camera = new Camera2D(2, new Vector2(1280, 0), new Point(cfg.Ints["window width"], cfg.Ints["window height"]));
                camera.rectangles = new Rectangle[]
                {
                    new Rectangle(400, 0, 200, cfg.Ints["window height"]),
                    new Rectangle(cfg.Ints["window width"] - 600, 0, 200, cfg.Ints["window height"])
                };

                shapes = new Shape[]
                {
                    new Rect(camera.rectangles[0]),
                    new Rect(camera.rectangles[1])
                };

                for (int i = 0; i < shapes.Length; i++) // Инициализация текстур форм
                    shapes[i].SetTexture(graphics, new Color(255, 100, 0));

                textures.Add("gui", content.Load<Texture2D>("gui")); // текстура с gui
                textures.Add("tiles", content.Load<Texture2D>("spritepack"));

                SpriteFont font = content.Load<SpriteFont>("Arial");
                style = new GUIStyle("style", "gui", font); // для стиля GUI необходимо указать имя текстуры в массиве с текстурами и шрифт
                gui = new GUIManager(style); // Новый гуи менеджер

                Player player = new Player("player", new Vector2(cfg.Ints["window width"] + cfg.Ints["window width"] / 2, cfg.Ints["window height"] - 128), 0.8f, new Rectangle(0, 0, 64, 64), "tiles");
                gameObjects.Add(player.Name, player);

                for (int i = 0; i < 60; i++)
                {
                    GameObject platform = new GameObject("platform_" + i, new Vector2(0 + i * 64, cfg.Ints["window height"] - 64), 0.2f, new Rectangle(0, 128, 64, 64), "tiles");
                    gameObjects.Add(platform.Name, platform);
                }

                isInited = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Back))
            {
                Stop();
            }

            if (camera.Position.X > -1 && camera.Position.X + camera.Width < Size.X + 1)
                camera.Move(TargetType.HorizontalOffset, gameObjects["player"]);
            if (camera.Position.X <= 0)
                camera.Set_position(new Vector2(0, camera.Position.Y));
            if (camera.Position.X + camera.Width >= Size.X)
                camera.Set_position(new Vector2(Size.X - camera.Width, camera.Position.Y));

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            graphics.GraphicsDevice.Clear(Color.Black); // перерисовка фона
            base.Draw(gameTime, spriteBatch, graphics); // Базовый дроу   

            gui.ButtonsReset(); // [Обязательно] перед вызовом кнопок
            //Отрисовка интерфейса
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            for (int i = 0; i < shapes.Length; i++) 
                shapes[i].Draw(gameTime, spriteBatch, Vector2.Zero, 1);
            gui.Label(spriteBatch, new Vector2(cfg.Ints["window width"] / 2 - style.Font.MeasureString(label).X / 2, cfg.Ints["window height"] - 35), label, textures, true);
            spriteBatch.End();
        }
    }

    public class Player : GameObject
    {
        public event GettingStringDelegate PlayerEvent;

        public Player(string name, Vector2 position, float layer, Rectangle sourceRectangle, string textureName) : base(name, position, layer, sourceRectangle, textureName)
        {
            
        }

        public override void Start()
        {
            base.Start();
            Legs legs = new Legs("legs", Position, 0.8f, new Rectangle(0, 64, 64, 64), "tiles");
            PlayerEvent += legs.GetParentEvent;
            AddChild(legs);
            Speed = 5;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Keyboard.GetState().IsKeyUp(Keys.Left) || Keyboard.GetState().IsKeyUp(Keys.Right))
                PlayerEvent?.Invoke("");


            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                PlayerEvent?.Invoke("left");
                effect = SpriteEffects.FlipHorizontally;
                Move(new Vector2(-Speed, 0));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                PlayerEvent?.Invoke("right");
                effect = SpriteEffects.None;
                Move(new Vector2(Speed, 0));
            }
        }
    }

    public class Legs : GameObject
    {
        private int frame = 0; // Номер текущего кадра для анимации
        private int frameLimit = 7; // Лимит кадров
        private Timer timer = new Timer(32); // Таймер для Анимации

        public Legs(string name, Vector2 position, float layer, Rectangle sourceRectangle, string textureName) : base(name, position, layer, sourceRectangle, textureName)
        {

        }

        public override void Start()
        {
            base.Start();
            timer.Start();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Анимация
            if (timer.Beat(gameTime))
            {
                frame++;
                if (frame >= frameLimit)
                    frame = 0;
            }
            _sourceRectangle.X = _sourceRectangle.Width * frame; // sourceRectangle указывает откуда из текстуры брать данные
        }

        public void GetParentEvent(string str)
        {
            if (str == "left")
                effect = SpriteEffects.FlipHorizontally;
            if (str == "right")
                effect = SpriteEffects.None;
        }
    }
}