namespace Solo.Utils
{
    public enum InputType : int
    {
        /// <summary>
        /// WASD and Arrows
        /// </summary>
        KeyBoard = 0,
        WASD = 1,
        Arrows = 2,
        /// <summary>
        /// Dpad and Stick
        /// </summary>
        GamePad = 3,
        Stick = 4,
        DPad = 5,
        /// <summary>
        /// Keyboard (WASD and ARROWS) and Gamepad (DPAD and STICK)
        /// </summary>
        All = 6,
        Mouse = 7
    }

    public enum TargetType : int
    {
        Stuck = 0,
        /// <summary>
        /// Requires two rectangles in the array. Left and Right
        /// </summary>
        HorizontalOffset = 1
    }
}
