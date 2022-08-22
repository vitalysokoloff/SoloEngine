using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Solo.Utils;
using Solo.Physics2D;

namespace Solo.d2D
{
    public class GameObject
    {
        public string Name { get; }
        public Vector2 Position { get; protected set; } // положение объекта
        public Vector2 Pivot; // Ось вращения
        public Shape Collider { get; protected set; }  //  для проверки столкновений
        public Color Color;
        public Rectangle SourceRectangle { get { return _sourceRectangle; } } // откуда в текстуре вырезать спрайт
        public int Speed { get; protected set; }
        public Vector2 Direction { get { return _direction; } }
        public Vector2 Velocity { get { return _velocity; } }
        public bool Alive;
        public event GettingStringDelegate SceneEvent;

        /// <summary>
        /// Expected degrees.
        /// </summary>
        public float AngleRotation // Для вращения;
        {
            get
            {
                return _angle;
            }

            set
            {
                _angle = value;
                if (Collider != null)
                    Collider.SetAngle(_angle);
            }
        } 
        public float OwnScale;
        public float Layer; // на каком слое отображать объект        

        public Dictionary<string, GameObject> childs { get; protected set; }



        protected GameObject[] forColliding;

        protected float _angle; // харинит в радианах 
        protected Vector2 _direction;
        protected Vector2 _velocity;
        protected Rectangle _sourceRectangle;
        protected string textureName; // текстуры должны храниться в дикшонари, а здесь имя хранимой текстурки лежать будет
        protected SpriteEffects effect;

        public GameObject()
        {
            Name = "GameObject";
            Position = Vector2.Zero;
            Pivot = Vector2.Zero;
            AngleRotation = 0;
            Layer = 0;
            _sourceRectangle = Rectangle.Empty;
            textureName = "default";
            effect = SpriteEffects.None;
            
            Start();
        }

        public GameObject(string name, Vector2 position, float layer, Rectangle sourceRectangle, string textureName)
        {
            Name = name;
            Position = position;
            Pivot = Vector2.Zero;
            AngleRotation = 0;
            Layer = layer;
            _sourceRectangle = sourceRectangle;
            this.textureName = textureName;
            effect = SpriteEffects.None;
            Start();
        }

        public virtual void Start()
        {
            childs = new Dictionary<string, GameObject>();
            Color = Color.White;
            OwnScale = 1;
            Alive = true;
            _direction = new Vector2(1, 0);
            _velocity = new Vector2(0, 0);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (childs.Count > 0)
                foreach (string k in childs.Keys)
                {
                    childs[k].Update(gameTime);
                }
        }
        
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 delta, float scale, Dictionary<string, Texture2D> textures)
        {
            spriteBatch.Draw(textures[textureName], Position - delta, _sourceRectangle, Color, AngleRotation, Pivot, scale * OwnScale, effect, Layer);

            if (childs.Count > 0)
                foreach (string k in childs.Keys)
                {
                    childs[k].Draw(gameTime, spriteBatch, delta, scale, textures);
                }
        }

        public void SetForCollidingArray(GameObject [] array)
        {
            forColliding = array;
        }

        /// <summary>
        /// The child's coordinates remain in the global coordinate system
        /// </summary>
        public virtual void AddChild(GameObject gameObject)
        {
            childs.Add(gameObject.Name,gameObject);
        }

        public virtual void SetCollider(Shape shape)
        {
            Collider = shape;
        }

        /// <summary>
        /// Angle in degrees.
        /// </summary>
        /// <param name="angle"></param>
        public virtual void SetAngleRotation(int angle)
        {
            _angle = (float)(angle * Math.PI / 180);
            if (Collider != null)
                Collider.SetAngle(_angle);
        }

        /// <summary>
        /// Sets the position of an object to the top left
        /// </summary>
        public virtual void SetPosition(Vector2 newPosition)
        {
            Position = newPosition;
            if (Collider != null)
                Collider.SetPosition(newPosition);
            
            if (childs.Count > 0)
                foreach(string k in childs.Keys)
                {
                    childs[k].SetPosition(newPosition);
                }
        }

        public virtual void Move(Vector2 delta)
        {
            _velocity = delta;
            Position += _velocity;
            if (delta.X > 0)
                _direction.X = 1;
            if (delta.X < 0)
                _direction.X = -1;

            if (delta.Y > 0)
                _direction.Y = 1;
            if (delta.Y < 0)
                _direction.Y = -1;

            if (Collider != null)
                Collider.SetPosition(Position);

            if (childs.Count > 0)
                foreach (string k in childs.Keys)
                {
                    childs[k].Move(delta);
                }
        }

        public virtual void Log(string path)
        {
           // Что-нибудь логировать
        }
    }
}
