using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Solo.Utils;

namespace Solo.Physics2D
{
    public abstract class Shape
    {
        public Vector2 Position
        {
            get
            {
                return _position;
            }
            protected set
            {
                _position = value;
                center = _position + Pivot;
            }
        } // положение
        public Vector2 Pivot { get; protected set; } // положение центра
        public float AngleRotation { get; protected set; }
        public Vector2 Size { get; protected set; }
        public Vector2[] Points { get; protected set; } // массив точек для отрисовки фигуры
        public Texture2D Texture { get; protected set; }

        protected Vector2 _position;
        protected Vector2[] basePoints;
        protected Vector2 center;

        /// <summary>
        /// the pivot is always in the middle of the shape
        /// </summary>
        public Shape(Rectangle rectangle)
        {
            AngleRotation = 0f;
        }

        /// <summary>
        /// the pivot is always in the middle of the shape
        /// </summary>
        public Shape(int x, int y, int width, int height)
        {
            AngleRotation = 0f;
        }

        public virtual void Move(Vector2 delta)
        {
            Position += delta;

            for (int i = 0; i < Points.Length; i++)
            {
                basePoints[i] += new Vector2((int)delta.X, (int)delta.Y);
                Points[i] += new Vector2((int)delta.X, (int)delta.Y);
            }
        }

        public virtual void SetAngle(float angle)
        {
            AngleRotation = angle;
            RotatePoints();
        }

        public virtual void SetAngle(int angle)
        {
            SetAngle((float)(angle * Math.PI / 180));
        }

        public void Rotate(int deltaAngle)
        {
            float oldAngle = AngleRotation;
            float newAngle = AngleRotation + (float)(deltaAngle * Math.PI / 180);
            float delta = newAngle - oldAngle;
            float a360 = (float)(360 * Math.PI / 180);
            if (delta < 0)
            {
                AngleRotation = a360 - delta;
            }
            if (newAngle > a360)
                AngleRotation = delta;
            if (newAngle >= 0 && newAngle <= a360)
                AngleRotation = newAngle;
            RotatePoints();
        }

        public virtual bool Intersects(Shape shape)
        {
            return GJK.CheckCollision(this, shape);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 delta, float scale)
        {
            if (Texture != null)
                spriteBatch.Draw(Texture, center - delta, new Rectangle(0, 0, (int)Size.X + 1, (int)Size.Y + 1), Color.White, AngleRotation, Pivot, scale * 1, SpriteEffects.None, 0.99f /* Самым верхним рисуем*/);
        }

        public override string ToString()
        {
            return "pos:" + Position + " pivot:" + Pivot + " angle:" + AngleRotation + " size:" + Size;
        }

        public virtual void SetTexture(GraphicsDeviceManager graphics, Color color)
        {
            Texture = new Texture2D(graphics.GraphicsDevice, (int)Size.X + 1, (int)Size.Y + 1);
            Color[] data = new Color[Texture.Width * Texture.Height];
            Texture.SetData(data);
            Vector2 point0 = Position; // для приведения к новой системе координат в верхней левой точке
            for (int i = 0; i < Points.Length - 1; i++)
                Drawing.Line(Texture, color, Points[i] - point0, Points[i + 1] - point0);
            Drawing.Line(Texture, color, Points[Points.Length - 1] - point0, Points[0] - point0);
        }

        public virtual void SetPosition(Vector2 newPosition)
        {
            Position = newPosition;
            SetBasePoints(); // Установка точек относительно новых координат позишн
            SetAngle(AngleRotation); // Поворот на заданный угол, относительно базовой позиции точек
        }

        protected void RotatePoints()
        {
            for (int i = 0; i < Points.Length; i++)
                Points[i] = new Vector2(
                (float)((basePoints[i].X - center.X) * Math.Cos(AngleRotation) - (basePoints[i].Y - center.Y) * Math.Sin(AngleRotation) + center.X),
                (float)((basePoints[i].X - center.X) * Math.Sin(AngleRotation) + (basePoints[i].Y - center.Y) * Math.Cos(AngleRotation) + center.Y)
                );
        }

        protected abstract void SetBasePoints();
    }

    public class Rect : Shape
    {
        /// <summary>
        /// the pivot is always in the middle of the shape
        /// </summary>
        public Rect(Rectangle rectangle) : base(rectangle)
        {
            basePoints = new Vector2[4];
            Points = new Vector2[4];
            Size = new Vector2(rectangle.Width, rectangle.Height);
            Pivot = new Vector2(rectangle.Width / 2, rectangle.Height / 2);
            Position = new Vector2(rectangle.X, rectangle.Y);
            SetBasePoints();
        }

        /// <summary>
        /// the pivot is always in the middle of the shape
        /// </summary>
        public Rect(int x, int y, int width, int height) : base(x, y, width, height)
        {
            basePoints = new Vector2[4];
            Points = new Vector2[4];
            Size = new Vector2(width, height);
            Pivot = new Vector2(width / 2, height / 2);
            Position = new Vector2(x, y);
            SetBasePoints();
        }

        protected override void SetBasePoints()
        {
            basePoints[0] = Position;
            basePoints[1] = Position + new Vector2(Size.X, 0);
            basePoints[2] = Position + new Vector2(Size.X, Size.Y);
            basePoints[3] = Position + new Vector2(0, Size.Y);
            Points[0] = Position;
            Points[1] = Position + new Vector2(Size.X, 0);
            Points[2] = Position + new Vector2(Size.X, Size.Y);
            Points[3] = Position + new Vector2(0, Size.Y);
        }
    }

    public class RegularPolygon : Shape
    {
        protected float radius;
        protected int verties;

        /// <summary>
        /// the pivot is always in the middle of the shape
        /// </summary>
        public RegularPolygon(Rectangle rectangle) : base(rectangle)
        {
            verties = rectangle.Height;
            radius = rectangle.Width;
            basePoints = new Vector2[verties];
            Points = new Vector2[verties];
            Size = new Vector2(radius * 2, radius * 2);
            Pivot = new Vector2(radius, radius);
            Position = new Vector2(rectangle.X, rectangle.Y);

            SetBasePoints();
        }

        /// <summary>
        /// the pivot is always in the middle of the shape
        /// </summary>
        public RegularPolygon(int x, int y, int width, int height) : base(x, y, width, height)
        {
            verties = height;
            radius = width;
            basePoints = new Vector2[verties];
            Points = new Vector2[verties];
            Size = new Vector2(radius * 2, radius * 2);
            Pivot = new Vector2(radius, radius);
            Position = new Vector2(x, y);
            SetBasePoints();
        }

        protected override void SetBasePoints()
        {
            // Используется радиус описанной окружности
            float a = (float)(360 * Math.PI / 180) / verties; // угол альфа, через который друг от друга находятся вершины правильного многоугольника

            for (int i = 0; i < verties; i++)
            {
                basePoints[i] = new Vector2( // получаем координаты вершин x=x0+r*cosA, y=y0+r*sinA 
                    (float)(center.X + radius * Math.Cos(+a * i)),
                    (float)(center.Y + radius * Math.Sin(0 + a * i))
                    );
                Points[i] = new Vector2( // получаем координаты вершин x=x0+r*cosA, y=y0+r*sinA 
                    (float)(center.X + radius * Math.Cos(0 + a * i)),
                    (float)(center.Y + radius * Math.Sin(0 + a * i))
                    );
            }
        }
    }
}
