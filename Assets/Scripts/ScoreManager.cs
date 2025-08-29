public static class ScoreManager
{
    public static string GetRank(float distance)
    {
        if (distance >= 1000) return "SSS";
        if (distance >= 800) return "SS";
        if (distance >= 600) return "S";
        if (distance >= 500) return "A";
        if (distance >= 400) return "B";
        if (distance >= 300) return "C";
        if (distance >= 200) return "D";
        if (distance >= 100) return "E";
        return "F";
    }
}
