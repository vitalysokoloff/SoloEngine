# SoloEngine
Kinda Game Engine based on Monogame 3.7.1.189 and .NET Framework 4.5
***
## Features
Сцены, игровые объекты, GUI, Рисование форм (n-угольники, прямоугольники), пересечение форм
***
## Getting started guid
Coming soon...
***
## Reference
### Solo.Utils
+ public enum InputType : int
+ public enum TargetType : int
+ public class Timer
+ + public int Period 
+ + public int Count
+ + public Timer(int period)
+ + public void Start()
+ + public void Stop()
+ + public void Reset()
+ + public bool Beat(GameTime gameTime)
+ static public class Drawing
+ + static public void Line(Texture2D texture, Color color, Vector2 va, Vector2 vb)
+ + public static Texture2D MakeSolidColorTexture(Point size, GraphicsDeviceManager graphics, Color color)
+ public static class Debag
+ + public static void Log(string path, string log)
+ public class Config
+ + public Dictionary<string, int> Ints
+ + public Dictionary<string, bool> Bools
+ + public Dictionary<string, string> Strings
+ + public Config(string path)
+ + public void Save(string path)
### Solo.Physics2D
+ public abstract class Shape
+ + public Vector2 Position
+ + public Vector2 Pivot 
+ + public float AngleRotation
+ + public Vector2 Size 
+ + public Vector2[] Points
+ + public Texture2D Texture
+ + protected Vector2 _position
+ + protected Vector2[] basePoints
+ + protected Vector2 center
+ + public Shape(Rectangle rectangle)
+ + public Shape(int x, int y, int width, int height)
+ + public virtual void Move(Vector2 delta)
+ + public virtual void SetAngle(float angle)
+ + public virtual void SetAngle(int angle)
+ + public void Rotate(int deltaAngle)
+ + public virtual bool Intersects(Shape shape)
+ + public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 delta, float scale)
+ + public override string ToString()
+ + public virtual void SetTexture(GraphicsDeviceManager graphics, Color color)
+ + public virtual void SetPosition(Vector2 newPosition)
+ + protected void RotatePoints()
+ + protected abstract void SetBasePoints();
+ public class Rect : Shape
+ public class RegularPolygon : Shape
+ public static class GJK
+ + public static bool CheckCollision(Shape s1, Shape s2)
### Solo.GUI
+ public struct GUIStyle
+ + public string Name;
+ + public string TextureName;
+ + public SpriteFont Font;
+ + public Rectangle Button;
+ + public Color ButtonTextColor;
+ + public Rectangle ButtonHover;
+ + public Color ButtonHoverTextColor;
+ + public Rectangle ButtonActive;
+ + public Color ButtonActiveTextColor;
+ + public Rectangle ButtonBlocked;
+ + public Color ButtonBlockedTextColor;
+ + public Rectangle Notice;
+ + public Color NoticeTextColor;
+ + public GUIStyle(string name, string textureName,SpriteFont font)
+ public class GUIManager
+ + public GUIStyle Style;
+ + public Rectangle Hover_target = new Rectangle(-50, -50, 5, 5);
+ + public InputType Input = InputType.Mouse;
+ + public event GUI_button button_hovered;
+ + public event GUI_button button_unhovered;
+ + public GUIManager(GUIStyle style)
+ + public void ResetHover()
+ + public void ButtonsReset()
+ + public bool Button(SpriteBatch spriteBatch, Rectangle collider, string text, string note, Dictionary<string, Texture2D> textures)
+ + public void ButtonBlocked(SpriteBatch spriteBatch, Rectangle collider, string text, Dictionary<string, Texture2D> textures)
+ + public void Notice(SpriteBatch spriteBatch, string text, Dictionary<string, Texture2D> textures, bool useBackGround)
+ + public void Label(SpriteBatch spriteBatch, Vector2 position, string text, Dictionary<string, Texture2D> textures, bool useBackGround)
### Solo.d2D
+ public class Scene
+ + public event GettingStringDelegate SceneEvent
+ + public Point Size 
+ + public string Name 
+ + public bool Debag
+ + public bool isStarted 
+ + public bool isFinished 
+ + public bool isInited 
+ + protected Dictionary<string, Texture2D> textures
+ + protected Dictionary<string, GameObject> gameObjects
+ + protected Camera2D camera
+ + protected Config cfg
+ + public Scene(Config cfg)
+ + public virtual void Init(ContentManager content, GraphicsDeviceManager graphics)
+ + public virtual void Start()
+ + public virtual void Stop()
+ + public virtual void Update(GameTime gameTime)
+ + public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
+ public class GameObject
+ + public string Name 
+ + public Vector2 Position 
+ + public Vector2 Pivot
+ + public Shape Collider 
+ + public Color Color
+ + public Rectangle SourceRectangle 
+ + public int Speed
+ + public Vector2 Direction 
+ + public Vector2 Velocity
+ + public bool Alive
+ + public event GettingStringDelegate SceneEvent
+ + public float AngleRotation 
+ + public float OwnScale
+ + public float Layer   
+ + public Dictionary<string, GameObject> childs 
+ + protected GameObject[] forColliding
+ + protected float _angle
+ + protected Vector2 _direction
+ + protected Vector2 _velocity
+ + protected Rectangle _sourceRectangle
+ + protected string textureName
+ + protected SpriteEffects effect
+ + public GameObject()
+ + public GameObject(string name, Vector2 position, float layer, Rectangle sourceRectangle, string textureName)
+ + public virtual void Start()
+ + public virtual void Update(GameTime gameTime)
+ + public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 delta, float scale, Dictionary<string, Texture2D> textures)
+ + public void SetForCollidingArray(GameObject [] array)
+ + public virtual void AddChild(GameObject gameObject)
+ + public virtual void SetCollider(Shape shape)
+ + public virtual void SetAngleRotation(int angle)
+ + public virtual void SetPosition(Vector2 newPosition)
+ + public virtual void Move(Vector2 delta)
+ public class Camera2D
+ + public Vector2 Position
+ + public int Width
+ + public int Height
+ + public float Scale
+ + public int Speed
+ + public int ExSpeed
+ + public Rect UpdateView
+ + public Rect view
+ + public Rectangle[] rectangles
+ + public Camera2D(int speed, Vector2 position, Point size)
+ + public void Set_position(Vector2 position)
+ + public void Move(Vector2 delta)
+ + public void Move(InputType inputType)
+ + public void Move(TargetType targetType, GameObject gameObject)
***
