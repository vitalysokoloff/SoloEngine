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
    class ShapeCollidingScene : Scene
    {
        GUIStyle style;
        GUIManager gui;

        Shape[] shapes;
        Timer timer; // Таймер для анимации вращения

        bool isColliding;
        string label = "[Стрелки] - Движение / [Space] - Вращать / [Backspace] - Вернуться в меню";

        public ShapeCollidingScene(Config cfg) : base(cfg)
        {
            isInited = false;
        }

        // Переопределить Init, [Обязательно]  всё инициализировать в Init
        public override void Init(ContentManager content, GraphicsDeviceManager graphics)
        {
            if (!isInited) // Чтобы инициализация проходила только один раз
            {
                timer = new Timer(32);
                timer.Start(); // [Обязательно] для запуска таймера
                isColliding = false;
                base.Init(content, graphics); // [Обязательно] Запускаем стандартный Start
                camera = new Camera2D(0, Vector2.Zero, new Point(cfg.Ints["window width"], cfg.Ints["window height"])); // [Обязательно]  инициализация камеры
                textures.Add("gui", content.Load<Texture2D>("gui")); // текстура с gui

                SpriteFont font = content.Load<SpriteFont>("Arial");
                style = new GUIStyle("style", "gui", font); // для стиля GUI необходимо указать имя текстуры в массиве с текстурами и шрифт
                gui = new GUIManager(style); // Новый гуи менеджер

                shapes = new Shape[]
                {
                    new RegularPolygon(new Rectangle(cfg.Ints["window width"] / 2, cfg.Ints["window height"] / 2, 20, 3)), 
                    new RegularPolygon(200, 250, 150, 40),
                    new RegularPolygon(1000, 250, 150, 5),
                    new RegularPolygon(cfg.Ints["window width"] / 2 - 300, cfg.Ints["window height"] / 2 - 300, 300, 2),
                    new Rect(900, 500, 300, 2)
                };

                for (int i = 0; i < shapes.Length; i++) // Инициализация текстур форм
                    shapes[i].SetTexture(graphics, new Color(255 - i * 50, 100, 150 + i * 50));


                isInited = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if(timer.Beat(gameTime))
            {
                shapes[2].Rotate(1);
                shapes[3].Rotate(-1);
                shapes[4].Rotate(1);
            }

            for (int i = 1; i < 5; i++)
            {
                if (isColliding = shapes[0].Intersects(shapes[i])) // Проыерка столкновений
                    break;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                shapes[0].Move(new Vector2(0, -2));
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                shapes[0].Move(new Vector2(0, 2));
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                shapes[0].Move(new Vector2(-2, 0));
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                shapes[0].Move(new Vector2(2, 0));

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                shapes[0].Rotate(1);

            if (Keyboard.GetState().IsKeyDown(Keys.Back))
            {
                Stop();
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            graphics.GraphicsDevice.Clear(Color.Black); // перерисовка фона
            base.Draw(gameTime, spriteBatch, graphics); // Базовый дроу   

            gui.ButtonsReset(); // [Обязательно] перед вызовом кнопок
            //Отрисовка интерфейса
            spriteBatch.Begin(SpriteSortMode.FrontToBack);

            for (int i = 0; i < shapes.Length; i++) 
                shapes[i].Draw(gameTime, spriteBatch, camera.Position, 1);
            if (isColliding)
                gui.Label(spriteBatch, shapes[0].Position, "Бум!!!", textures, true); 

            gui.Label(spriteBatch, new Vector2(cfg.Ints["window width"] / 2 - style.Font.MeasureString(label).X / 2 , cfg.Ints["window height"] - 35), label, textures, true);
            gui.Label(spriteBatch, new Vector2(0,0), shapes[0].ToString(), textures, true);
            gui.Label(spriteBatch, new Vector2(0, 20), shapes[2].ToString(), textures, true);
            gui.Label(spriteBatch, new Vector2(0, 40), shapes[3].ToString(), textures, true);
            spriteBatch.End();
        }
    }
}
