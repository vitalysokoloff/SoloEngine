using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Solo.Utils;
using Solo.GUI;
using Solo.d2D;

namespace Demo
{
    public class Menu : Scene
    {
        new public event GettingStringDelegate SceneEvent;

        SoundEffect theme;
        SoundEffectInstance sound;
        GUIStyle style;
        GUIManager gui;

        int guiPointer = 0; // для навигации по меню при помощи клавиатуры
        bool ifKeyDown = false;
        bool ifKeyUp = false;
        bool ifKeyLeft = false;
        bool ifKeyRight = false;

        string label = "Навигация по меню: стрелки / мышь";

        public Menu(Config cfg) : base(cfg)
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
                textures.Add("player", content.Load<Texture2D>("player")); // Загрузка текстуры
                textures.Add("bg1", content.Load<Texture2D>("bg1"));
                textures.Add("bg2", content.Load<Texture2D>("bg2"));
                textures.Add("spritepack", content.Load<Texture2D>("spritepack"));
                textures.Add("gui", content.Load<Texture2D>("gui"));

                SpriteFont font = content.Load<SpriteFont>("Arial");
                style = new GUIStyle("style", "gui", font); // для стиля GUI необходимо указать имя текстуры в массиве с текстурами и шрифт
                gui = new GUIManager(style);

                theme = content.Load<SoundEffect>("theme"); // Загрузка мелодии
                sound = theme.CreateInstance();
                sound.IsLooped = true;
                sound.Volume = cfg.Ints["sound volume"] / (float)100;                

                GameObject go = new Guitarist("Guitarist", new Vector2(cfg.Ints["window width"] / 2 - 32, cfg.Ints["window height"] - 192), 0.3f, new Rectangle(0, 0, 64, 128), "player"); // Создание игрового объекта
                gameObjects.Add(go.Name, go); //[Обязательно] Добавление игрового объекта в массив игровых объектов

                GameObject bg1 = new GameObject("bg1", new Vector2(0, 0), 0f, new Rectangle(0, 0, cfg.Ints["window width"], cfg.Ints["window height"]), "bg1");
                gameObjects.Add(bg1.Name, bg1);

                GameObject bg2 = new GameObject("bg2", new Vector2(0, 0), 0.1f, new Rectangle(0, 0, cfg.Ints["window width"], cfg.Ints["window height"]), "bg2");
                gameObjects.Add(bg2.Name, bg2);

                for (int i = 0; i < 20; i++)
                {
                    GameObject platform = new GameObject("platform_" + i, new Vector2(0 + i * 64, cfg.Ints["window height"] - 64), 0.2f, new Rectangle(0, 128, 64, 64), "spritepack");
                    gameObjects.Add(platform.Name, platform);
                }
                isInited = true;
            }
        }

        public override void Start()
        {
            base.Start(); //[Обязательно] 
            sound.Play();
        }

        public override void Stop()
        {
            base.Stop(); //[Обязательно] 
            sound.Stop(); // Остановка фоновой мелодии
        }

        // [Обязательно] нужно переопределить
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            KeysInput(); // Управление меню с клавиатуры
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            graphics.GraphicsDevice.Clear(Color.LightCyan); // перерисовка фона
            base.Draw(gameTime, spriteBatch, graphics); // Базовый дроу   

            gui.ButtonsReset(); // [Обязательно] перед вызовом кнопок
            //Отрисовка интерфейса
            spriteBatch.Begin(SpriteSortMode.FrontToBack); 
            gui.Label(spriteBatch, new Vector2(cfg.Ints["window width"] / 2 - 115, 175), cfg.Strings["window title"] + " " + cfg.Strings["version"], textures, true);            
            gui.Frame(spriteBatch, new Rectangle(cfg.Ints["window width"] / 2 - 110, 200, 220, 212),textures);
            gui.Label(spriteBatch, new Vector2(cfg.Ints["window width"] / 2 - 100, 210), "Громкость: " + cfg.Ints["sound volume"], textures, true);
            if (cfg.Ints["sound volume"] != 0)
            {
                if (gui.Button(spriteBatch, new Rectangle(cfg.Ints["window width"] / 2 + 52, 210, 24, 24), "-", "Убавить", textures))
                {
                    cfg.Ints["sound volume"] -= 10;
                    if (cfg.Ints["sound volume"] < 0)
                        cfg.Ints["sound volume"] = 0;
                    sound.Volume = cfg.Ints["sound volume"] / (float)100;
                }
            }
            else
                gui.ButtonBlocked(spriteBatch, new Rectangle(cfg.Ints["window width"] / 2 + 52, 210, 24, 24), "-", textures);
            if (cfg.Ints["sound volume"] != 100)
            {
                if (gui.Button(spriteBatch, new Rectangle(cfg.Ints["window width"] / 2 + 76, 210, 24, 24), "+", "Прибавить", textures))
                {
                    cfg.Ints["sound volume"] += 10;
                    if (cfg.Ints["sound volume"] > 100)
                        cfg.Ints["sound volume"] = 100;
                    sound.Volume = cfg.Ints["sound volume"] / (float)100;
                }
            }
            else
                gui.ButtonBlocked(spriteBatch, new Rectangle(cfg.Ints["window width"] / 2 + 76, 210, 24, 24), "+", textures);

            if (gui.Button(spriteBatch, new Rectangle(cfg.Ints["window width"] / 2 - 100, 234, 200, 24), "Заставка", null, textures))
            { SceneEvent?.Invoke("intro"); }
            if (gui.Button(spriteBatch, new Rectangle(cfg.Ints["window width"] / 2 - 100, 258, 200, 24), "Пример GUI", null, textures))
            { SceneEvent?.Invoke("gui"); }
            if (gui.Button(spriteBatch, new Rectangle(cfg.Ints["window width"] / 2 - 100, 282, 200, 24), "Рисование форм", null, textures))
            { SceneEvent?.Invoke("draw shapes"); }
            if (gui.Button(spriteBatch, new Rectangle(cfg.Ints["window width"] / 2 - 100, 306, 200, 24), "Столкновение форм", null, textures))
            { SceneEvent?.Invoke("collide shapes"); }
            if (gui.Button(spriteBatch, new Rectangle(cfg.Ints["window width"] / 2 - 100, 330, 200, 24), "Камера платформер", null, textures))
            { SceneEvent?.Invoke("platformer"); }
            if (gui.Button(spriteBatch, new Rectangle(cfg.Ints["window width"] / 2 - 100, 354, 200, 24), "Камера TDS", null, textures))
            { }
            if (gui.Button(spriteBatch, new Rectangle(cfg.Ints["window width"] / 2 - 100, 378, 200, 24), "Выход [ESC]", null, textures))
            { SceneEvent?.Invoke("off"); }
            gui.Label(spriteBatch, new Vector2(cfg.Ints["window width"] / 2 - style.Font.MeasureString(label).X / 2, cfg.Ints["window height"] - 35), label, textures, true);
            spriteBatch.End();
        }

        private void KeysInput()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                SceneEvent?.Invoke("off");
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up) && !ifKeyUp)
            {
                gui.Input = InputType.KeyBoard;
                guiPointer--;
                if (guiPointer < 0)
                    guiPointer = 7;
                gui.Hover_target.X = cfg.Ints["window width"] / 2 + 52;
                gui.Hover_target.Y = 210 + 24 * guiPointer;
                ifKeyUp = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && !ifKeyDown)
            {
                gui.Input = InputType.KeyBoard;
                guiPointer++;
                if (guiPointer > 7)
                    guiPointer = 0;
                gui.Hover_target.X = cfg.Ints["window width"] / 2 + 52;
                gui.Hover_target.Y = 210 + 24 * guiPointer;
                ifKeyDown = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && !ifKeyLeft)
            {
                gui.Input = InputType.KeyBoard;
                gui.Hover_target.X = cfg.Ints["window width"] / 2 + 52;
                gui.Hover_target.Y = 210 + 24 * guiPointer;
                ifKeyLeft = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && !ifKeyRight)
            {
                gui.Input = InputType.KeyBoard;
                gui.Hover_target.X = cfg.Ints["window width"] / 2 + 76;
                gui.Hover_target.Y = 210 + 24 * guiPointer;
                ifKeyRight = true;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Up) && ifKeyUp)
                ifKeyUp = false;
            if (Keyboard.GetState().IsKeyUp(Keys.Down) && ifKeyDown)
                ifKeyDown = false;
            if (Keyboard.GetState().IsKeyUp(Keys.Left) && ifKeyLeft)
                ifKeyLeft = false;
            if (Keyboard.GetState().IsKeyUp(Keys.Right) && ifKeyRight)
                ifKeyRight = false;

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                gui.Input = InputType.Mouse;
                gui.ResetHover();
            }
        }
    }
}
