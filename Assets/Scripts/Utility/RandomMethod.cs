using System;

public class RandomMethod
{
    static int N = 0;
    static float M = 0f;

    public static int Range(int min, int maxExclusuive)
    {
        N += DateTime.Now.Millisecond;
        var r = new System.Random(N);
        var b = r.Next(min, maxExclusuive);
        N += b;
        N = N > 1000000 ? N - 1000000 : N;
        return b;
    }
    public static float Range(float min, float max)
    {
        M += DateTime.Now.Millisecond;
        var r = new System.Random((int)M);
        var b = r.Next((int)(min * 2048), (int)(max * 2048));
        M += b;
        M = M > 1000000f ? M - 1000000f : M;
        return b / 2048f;
    }
    public static T[] Disruption<T>(T[] obj) 
    {
        N += DateTime.Now.Millisecond;
        var r = new System.Random((int)N);

        T temp;
        for (int i = obj.Length - 1; i > 0; i--)
        {
            int j = r.Next(i + 1);
            temp = obj[i];
            obj[i] = obj[j];
            obj[j] = temp;
        }
        return obj;
    }
}
