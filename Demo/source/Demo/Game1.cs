using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Solo.d2D;
using Solo.GUI;
using Solo.Physics2D;
using Solo.Utils;

namespace Demo
{    
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

       
        Config cfg;  // Необходимо для работы движка, содержит настройки

        // Сцены
        Dictionary<string, Scene> scenes;
        string curScene; // Текущая сцена

        public Game1()
        {
            cfg = new Config("Config.txt"); // Инициализация фала конфигурации
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = cfg.Ints["window width"]; // Ширина окна
            graphics.PreferredBackBufferHeight = cfg.Ints["window height"]; // Высота окна
            graphics.IsFullScreen = cfg.Bools["full screen"]; // Режим отображения окна
            IsMouseVisible = cfg.Bools["mouse visibility"]; // Видимость курсора мыши
            Content.RootDirectory = cfg.Strings["root directory"]; // Директория с контентом  

            scenes = new Dictionary<string, Scene>();

            Scene logo = new LogoScene(cfg); // Заставка
            scenes.Add("logo", logo);
            Menu menu = new Menu(cfg); // Чтоб работало событие переопределённое в классе Menu
            menu.SceneEvent += Action;// Подписка на события сцены "меню"
            scenes.Add("menu", menu);             
            curScene = "logo";
        }
        protected override void Initialize()
        {
            Window.Title = cfg.Strings["window title"] + " " + cfg.Strings["version"]; // Баг этой версии моногейм (3.7.1) для опен гл в конструктаре если устанавливать тайтл, 
                                                                                       //то он не установится - будет имя проекта в заголовке окна
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }
        
        protected override void UnloadContent()
        {
            
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (!scenes[curScene].isStarted) // Если сцена заставки не запущена - запустить
            {
                scenes[curScene].Init(Content, graphics);
                scenes[curScene].Start();
                
            }
            if (!scenes[curScene].isFinished) // Если сцена не завершена выполнять update
                scenes[curScene].Update(gameTime);

            if (scenes[curScene].isFinished && curScene != "menu") // Если сцена закончилась - запустить меню
            {
                if (curScene != "logo")
                    scenes.Remove(curScene);
                curScene = "menu";                
            }

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            if (!scenes[curScene].isFinished) // отрисовка сцены
                scenes[curScene].Draw(gameTime, spriteBatch, graphics);

            base.Draw(gameTime);
        }

        private void Action(string type)
        {
            switch (type)
            {
                case "off":
                    cfg.Save("Config.txt");
                    Exit();
                    break;
                case "intro":
                    scenes["menu"].Stop();
                    scenes["logo"] = new LogoScene(cfg);
                    curScene = "logo";
                    break;
                case "gui":
                    scenes["menu"].Stop();
                    Scene gui = new GUIDemo(cfg); 
                    scenes.Add("gui", gui);
                    curScene = "gui";
                    break;
                case "draw shapes":
                    scenes["menu"].Stop();
                    Scene s1 = new DrawShapeScene(cfg);
                    scenes.Add("draw shapes", s1);
                    curScene = "draw shapes";
                    break; 
                case "collide shapes":
                    scenes["menu"].Stop();
                    Scene s2 = new ShapeCollidingScene(cfg);
                    scenes.Add("collide shapes", s2);
                    curScene = "collide shapes";
                    break;
                case "platformer":
                    scenes["menu"].Stop();
                    Scene cp = new PlatformerCameraScene(cfg);
                    scenes.Add("platformer", cp);
                    curScene = "platformer";
                    break;
            }
        }
    }   
}
