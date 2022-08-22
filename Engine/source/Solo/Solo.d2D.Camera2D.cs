using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Solo.Utils;

namespace Solo.d2D
{
    /// <summary>
    /// It's not a real camera, but it stores a Vector2 Position which is needed to draw the map and units.
    /// </summary>
    using Solo.Physics2D;

    public class Camera2D
    {
        // плюс камера должна обрезать всё что за пределами экрана
        public Vector2 Position { get; private set; }        
        public int Width { get; }
        public int Height { get; }

        public float Scale;
        public int Speed;
        public int ExSpeed;
        public Rect UpdateView; // Если гейм обджект попадает в границы то должен выполнять апдейт
        public Rect view; // Если гейм обджект попадает в границы то должен отрисовываться
        public Rectangle[] rectangles;
              


        public Camera2D(int speed, Vector2 position, Point size)
        {
            Speed = speed;
            ExSpeed = speed * 2;
            Position = position;
            Scale = 1;
            Point tmp = new Point(200, 200);
            view = new Rect( new Rectangle(Point.Zero - tmp, size + tmp));
            Width = size.X;
            Height = size.Y;
        }        

        public void Set_position(Vector2 position)
        {
            Position = position;
        }

        public void Move(Vector2 delta)
        {
            Position += delta * Speed;
        }

        public void Move(InputType inputType)
        {
            Vector2 camera_delta = Vector2.Zero;
            if (inputType == InputType.All || inputType == InputType.KeyBoard || inputType == InputType.WASD)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                    camera_delta.Y = -1 * Speed;
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                    camera_delta.Y = 1 * Speed;
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    camera_delta.X = -1 * Speed;
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                    camera_delta.X = 1 * Speed;
            }
            if (inputType == InputType.All || inputType == InputType.KeyBoard || inputType == InputType.Arrows)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    camera_delta.Y = -1 * Speed;
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    camera_delta.Y = 1 * Speed;
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    camera_delta.X = -1 * Speed;
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    camera_delta.X = 1 * Speed;
            }
            if (inputType == InputType.All || inputType == InputType.GamePad || inputType == InputType.DPad)
            {
                if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed)
                    camera_delta.Y = -1 * Speed;
                if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed)
                    camera_delta.Y = 1 * Speed;
                if (GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed)
                    camera_delta.X = -1 * Speed;
                if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed)
                    camera_delta.X = 1 * Speed;
            }
            if (inputType == InputType.All || inputType == InputType.GamePad || inputType == InputType.DPad)
            {
                if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed)
                    camera_delta.Y = -1 * Speed;
                if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed)
                    camera_delta.Y = 1 * Speed;
                if (GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed)
                    camera_delta.X = -1 * Speed;
                if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed)
                    camera_delta.X = 1 * Speed;
            }
            if (inputType == InputType.All || inputType == InputType.GamePad || inputType == InputType.Stick)
            {
                if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y <= -0.5f)
                    camera_delta.Y = -1 * Speed;
                if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y >= 0.5f)
                    camera_delta.Y = 1 * Speed;
                if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X <= -0.5f)
                    camera_delta.X = -1 * Speed;
                if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X >= 0.5f)
                    camera_delta.X = 1 * Speed;
            }
            Position += camera_delta;
        }

        /// <summary>
        ///  HorizontalOffset needs to rectangle. left border and right
        /// </summary>
        public void Move(TargetType targetType, GameObject gameObject)
        {
            if (targetType == TargetType.Stuck)
            {
                Position = new Vector2(gameObject.Position.X + gameObject.SourceRectangle.Width / 2 - Width / 2, gameObject.Position.Y + gameObject.SourceRectangle.Height / 2 - Height / 2);
            }

            if (targetType == TargetType.HorizontalOffset)
            {
                float d = gameObject.Direction.X;
                Speed = (int)(gameObject.Velocity.X * d);
                if (d == 1)
                {
                    if (gameObject.Position.X + gameObject.SourceRectangle.Width - Position.X > rectangles[0].X + rectangles[0].Width)
                    {
                        Position += new Vector2(Speed, 0);
                    }
                    if (gameObject.Position.X - Position.X > rectangles[0].X + rectangles[0].Width)
                    {
                        Position += new Vector2(ExSpeed, 0);
                    }
                }
                if (d == -1)
                {
                    if (gameObject.Position.X - Position.X < rectangles[1].X)
                    {
                        Position -= new Vector2(Speed, 0);
                    }
                    if (gameObject.Position.X - Position.X < rectangles[1].X - gameObject.SourceRectangle.Width)
                    {
                        Position -= new Vector2(ExSpeed, 0);
                    }
                }
            }
        }
    }
    
}
