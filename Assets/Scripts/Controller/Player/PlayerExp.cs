public static class PlayerExp
{
    public static long RequiredExp(int level)
        => 100L * level * level;

    public static long TotalExpAtLevel(int level)
        => 100L * level * (level + 1) * (2L * level + 1) / 6L;

    public static int LevelForTotalExp(long totalExp, int hiGuess = 1000)
    {
        int lo = 0, hi = hiGuess;

        while (TotalExpAtLevel(hi) <= totalExp) hi *= 2;

        while (lo < hi)
        {
            int mid = (lo + hi + 1) / 2;
            if (TotalExpAtLevel(mid) <= totalExp) lo = mid;
            else hi = mid - 1;
        }
        return lo;
    }
}
