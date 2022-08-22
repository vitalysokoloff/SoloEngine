using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Solo.Utils;
using Solo.Physics2D;
using Solo.d2D;

namespace Demo
{
    public class Guitarist : GameObject
    {
        private int frame = 0; // Номер текущего кадра для анимации
        private int frameLimit = 5; // Лимит кадров
        private Timer timer = new Timer(250); // Таймер для Анимации

        public Guitarist(string name, Vector2 position, float layer, Rectangle sourceRectangle, string textureName) : base (name, position, layer, sourceRectangle, textureName)
        {

        }

        public override void Start()
        {
            base.Start();
            timer.Start();// Таймер обязательно нужно стартонуть
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
    }
}
