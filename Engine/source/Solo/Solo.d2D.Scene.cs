using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Solo.Utils;
using Solo.GUI;

namespace Solo.d2D
{
    public class Scene
    {
        public event GettingStringDelegate SceneEvent;

        public Point Size { get; protected set; } // Размеры комнаты
        public string Name { get; protected set; }
        public bool Debag;
        public bool isStarted { get; protected set; }
        public bool isFinished { get; protected set; }
        public bool isInited { get; protected set; }

        protected Dictionary<string, Texture2D> textures;
        protected Dictionary<string, GameObject> gameObjects;
        protected Camera2D camera;

        protected Config cfg;
        

        public Scene(Config cfg)
        {
            this.cfg = cfg;
            gameObjects = new Dictionary<string, GameObject>();
            isStarted = false;
            isFinished = true;
            textures = new Dictionary<string, Texture2D>();
            Size = new Point(5000, 1000);
        }

        public virtual void Init(ContentManager content, GraphicsDeviceManager graphics)
        { }

        public virtual void Start()
        {
            isStarted = true;
            isFinished = false;
        }

        public virtual void Stop()
        {
            isStarted = false;
            isFinished = true;
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (string k in gameObjects.Keys) // Запускаем Update  у всех игровых объектов в сцене
                if (gameObjects[k].Alive)
                    gameObjects[k].Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            if (gameObjects.Count > 0)
            {
                foreach (string k in gameObjects.Keys)
                    if (gameObjects[k].Alive)
                        gameObjects[k].Draw(gameTime, spriteBatch, camera.Position, camera.Scale, textures);
                if (Debag)
                    foreach (string k in gameObjects.Keys)
                    {
                        gameObjects[k].Collider.Draw(gameTime, spriteBatch, camera.Position, camera.Scale);
                        if (gameObjects[k].childs.Count > 0)
                            foreach (string k2 in gameObjects[k].childs.Keys)
                            {
                                gameObjects[k].childs[k2].Collider.Draw(gameTime, spriteBatch, camera.Position, camera.Scale);
                            }
                    }
            }
            spriteBatch.End();
        }
    }

    public class LogoScene : Scene
    {
        private Timer timer;
        private int count;
        private Texture2D bg;
        private SoundEffect intro;
        private SoundEffectInstance sound;
        private Color color;

        public LogoScene(Config cfg) : base (cfg)
        {
            Name = "logo";
            camera = new Camera2D(0, Vector2.Zero, new Point(cfg.Ints["window width"], cfg.Ints["window height"]));
        }

        public override void Init(ContentManager content, GraphicsDeviceManager graphics)
        {
            textures.Add("mono_logo", content.Load<Texture2D>("mono_logo"));
            textures.Add("solo_logo", content.Load<Texture2D>("solo_logo"));
            intro = content.Load<SoundEffect>("intro");
            sound = intro.CreateInstance();
            sound.IsLooped = false;
            sound.Volume = cfg.Ints["music volume"] / 100;
            bg = Drawing.MakeSolidColorTexture(new Point(1, 1), graphics, Color.Black);
            textures.Add("bg", bg);


            timer = new Timer(15);
            count = 0;
            color = Color.Black;
            timer.Start();            
            base.Init(content, graphics);
        }

        public override void Update(GameTime gameTime)
        {
            if (timer.Beat(gameTime))
            {
                color.R += 1;
                color.G += 1;
                color.B += 1;
            }
            if (color.R == 255)
            {
                color.R = 0;
                color.G = 0;
                color.B = 0;
                count++;
            }
            if (count == 1)
                sound.Play();
            if (count == 3)
                isFinished = true;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Begin();
                spriteBatch.Draw(textures["bg"], new Rectangle(0, 0, cfg.Ints["window width"], cfg.Ints["window height"]), Color.White);
            if (count == 1)
                spriteBatch.Draw(textures["mono_logo"], new Rectangle(cfg.Ints["window width"] / 2 - 75, cfg.Ints["window height"] / 2 - 95, 155 , 191), new Rectangle(0, 0, 155, 191), color);
            if (count == 2)
                spriteBatch.Draw(textures["solo_logo"], new Rectangle(cfg.Ints["window width"] / 2 - 75, cfg.Ints["window height"] / 2 - 95, 155, 191), new Rectangle(0, 0, 155, 191), color);
            spriteBatch.End();
            base.Draw(gameTime, spriteBatch, graphics);
        }
    }

    public class TestGUIScene : Scene
    {
        private GUIStyle style;
        private GUIManager gui;
        private int count = 0;

        public TestGUIScene(Config cfg) : base(cfg)
        {
            Name = "gui test";
            camera = new Camera2D(0, Vector2.Zero, new Point(cfg.Ints["window width"], cfg.Ints["window height"]));
            isStarted = false;
        }

        public override void Init(ContentManager content, GraphicsDeviceManager graphics)
        {
            textures.Add("gui", content.Load<Texture2D>("gui"));
            style = new GUIStyle("test", "gui", content.Load<SpriteFont>("Arial"));
            gui = new GUIManager(style);
            base.Init(content, graphics);
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                gui.Input = InputType.KeyBoard;
                gui.Hover_target.X = 10;
                gui.Hover_target.Y = 10;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                gui.Input = InputType.KeyBoard;
                gui.Hover_target.X = 10;
                gui.Hover_target.Y = 34;
            }
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                gui.Input = InputType.Mouse;
                gui.ResetHover();
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            base.Draw(gameTime, spriteBatch, graphics);
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            gui.ButtonsReset();
            if (gui.Button(spriteBatch, new Rectangle(10, 10, 150, 24), "test text", "count: " + count, textures))
            { count++; }
            if (gui.Button(spriteBatch, new Rectangle(10, 34, 150, 24), "test text", null, textures))
            { count--; }
            gui.Label(spriteBatch, new Vector2(10, 100), "dsfsdfsdfsdf", textures, true);
            gui.ButtonBlocked(spriteBatch, new Rectangle(10, 58, 150, 24), "button blocked", textures);
            spriteBatch.End();            
        }
    }

    public delegate void GettingStringDelegate(string type);
}
