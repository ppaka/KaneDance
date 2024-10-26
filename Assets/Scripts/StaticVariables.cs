using UnityEngine;

public static class StaticVariables
{
    public static int isDmca = 1;
    public static int recentScore = 0;
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Reset()
    {
        isDmca = 1;
        recentScore = 0;
    }
}