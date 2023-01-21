using System;

public static class OscilationMath
{
    public static float Sin(double time)
    {
        return (float)Math.Sin(time * Math.PI * 2);
    }

    public static float Quad(double time)
    {
        var sin = (float)Math.Sin(time * Math.PI * 2);
        return (float)Math.Round(sin * 0.5F + 0.5F) * 2 - 1;
    }

    public static float PingPong(double time)
    {
        if (time == 0.0)
        {
            return 0.0f;
        }
        else
        {
            var rep = Repeate01(time);

            return (float)(Math.Abs(rep * 2.0 - 1.0) * 2 - 1);
        }
    }

    public static double Repeate01(double time)
    {
        return time - Math.Floor(time);
    }
}
