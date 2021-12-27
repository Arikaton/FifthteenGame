namespace GameScripts.Utils
{
    public static class MathUtils
    {
        public static int Sign(this int value)
        {
            if (value > 0)
                return 1;
            return value == 0 ? 0 : -1;
        }
    }
}