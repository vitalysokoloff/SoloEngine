using Microsoft.Xna.Framework;

namespace Solo.Utils
{
    /// <summary>
    /// Timer. After creation, you should run the Start(). 1000 - is one second.
    /// </summary>
    public class Timer
    {
        private int current_time;
        private bool is_start;

        public int Period { get; }
        public int Count { get; private set; }

        public Timer(int period)
        {
            current_time = 0;
            is_start = false;
            Count = 0;
            Period = period;
        }

        public void Start()
        {
            if (!is_start)
            {
                is_start = true;
            }
        }

        public void Stop()
        {
            if (is_start)
            {
                is_start = false;
            }
        }

        public void Reset()
        {
            current_time = 0;
            Count = 0;
        }
        /// <summary>
        /// Tick. Tact.
        /// </summary>
        public bool Beat(GameTime gameTime)
        {
            if (is_start)
            {
                current_time += gameTime.ElapsedGameTime.Milliseconds;
                if (current_time > Period)
                {
                    current_time = 0;
                    Count++;
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
    }
}
