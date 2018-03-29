using System;

namespace Discord_BOT.Misc
{
    public static class LevelCounter
    {
        public static uint WhichLevelWith(long xp)
        {
            var level = 0;
            while (xp >= (4 * Math.Pow(level + 1, 4)) + 25 * Math.Pow(level + 1, 2) + 50 * (level + 1))
                level++;

            return (uint)level;
        }

        public static long HowMuchExpFor(int level)
        {
            double howMuch = (4 * Math.Pow(level, 4)) + 25 * Math.Pow(level, 2) + 50 * level;
            return (long)howMuch;
        }

        public static long HowMuchExpNeedForNextLevel(uint level, long xp)
        {
            double howMuch = (4 * Math.Pow(level + 1, 4)) + 25 * Math.Pow(level + 1, 2) + 50 * (level + 1) - xp;
            return (long)howMuch;
        }
    }
}
