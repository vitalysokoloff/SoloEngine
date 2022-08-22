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
    class DrawShapeScene : Scene
    {
        GUIStyle style;
        GUIManager gui;

        Shape[] shapes;

        string label = "[Backspace] - Вернуться в меню";

        public DrawShapeScene(Config cfg) : base(cfg)
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

                SpriteFont font = content.Load<SpriteFont>("Arial");
                style = new GUIStyle("style", "gui", font); // для стиля GUI необходимо указать имя текстуры в массиве с текстурами и шрифт
                gui = new GUIManager(style); // Новый гуи менеджер

                shapes = new Shape[]
                {
                    new Rect(new Rectangle(20, 20, 40, 40)), // Квадрат
                    new Rect(20, 70, 60, 40), // Прямоугольник, можно задать и через Rectangle, как квадрат
                    new RegularPolygon(20, 120, 20, 2), // Отрезок
                    new RegularPolygon(20, 170, 20, 3), // Треугльник
                    new RegularPolygon(20, 220, 20, 4), // Ромб
                    new RegularPolygon(20, 270, 20, 5), // 5-и угольник
                    new RegularPolygon(20, 320, 20, 6), // 6-и угольник
                    new RegularPolygon(20, 370, 20, 8), // 8-и угольник
                    new RegularPolygon(20, 420, 20, 20), // 20-и угольник
                };

                for (int i = 0; i < shapes.Length; i++) // Инициализация текстур форм
                    shapes[i].SetTexture(graphics, new Color(255, 100 + i * 20, 255 - i * 20));

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
            graphics.GraphicsDevice.Clear(Color.Black); // перерисовка фона
            base.Draw(gameTime, spriteBatch, graphics); // Базовый дроу   

            gui.ButtonsReset(); // [Обязательно] перед вызовом кнопок
            //Отрисовка интерфейса
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            shapes[0].Draw(gameTime, spriteBatch, Vector2.Zero, 1);
            gui.Label(spriteBatch, new Vector2(120, 20), "Rect(new Rectangle(40, 40, 40, 40)); // Квадрат", textures, true);
            shapes[1].Draw(gameTime, spriteBatch, Vector2.Zero, 1);
            gui.Label(spriteBatch, new Vector2(120, 70), "Rect(40, 90, 60, 40); // Прямоугольник, можно задать и через Rectangle, как квадрат", textures, true);
            shapes[2].Draw(gameTime, spriteBatch, Vector2.Zero, 1);
            gui.Label(spriteBatch, new Vector2(120, 120), "RegularPolygon(40{x}, 140{y}, 20{radius}, 2{verties}), // Отрезок, можно и через Rectangle", textures, true);
            shapes[3].Draw(gameTime, spriteBatch, Vector2.Zero, 1);
            gui.Label(spriteBatch, new Vector2(120, 170), "RegularPolygon(40, 190, 20, 3), // Треугльник", textures, true);
            shapes[4].Draw(gameTime, spriteBatch, Vector2.Zero, 1);
            gui.Label(spriteBatch, new Vector2(120, 220), "RegularPolygon(40, 240, 20, 4), // Ромб", textures, true);
            shapes[5].Draw(gameTime, spriteBatch, Vector2.Zero, 1);
            gui.Label(spriteBatch, new Vector2(120, 270), "RegularPolygon(40, 290, 20, 5), // 5-и угольник", textures, true);
            shapes[6].Draw(gameTime, spriteBatch, Vector2.Zero, 1);
            gui.Label(spriteBatch, new Vector2(120, 320), "RegularPolygon(40, 340, 20, 6), // 6-и угольник", textures, true);
            shapes[7].Draw(gameTime, spriteBatch, Vector2.Zero, 1);
            gui.Label(spriteBatch, new Vector2(120, 370), "RegularPolygon(40, 390, 20, 8), // 8-и угольник", textures, true);
            shapes[8].Draw(gameTime, spriteBatch, Vector2.Zero, 1);
            gui.Label(spriteBatch, new Vector2(120, 420), "RegularPolygon(40, 440, 20, 20), // 20-и угольник", textures, true);

            gui.Label(spriteBatch, new Vector2(cfg.Ints["window width"] / 2 - style.Font.MeasureString(label).X / 2, cfg.Ints["window height"] - 35), label, textures, true);
            spriteBatch.End();
        }
    }
}