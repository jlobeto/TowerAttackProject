using System;
[Serializable]
public class WeatherEventItem
{
    public int levelId;
    public int[] eventTimer;

    public int rainAmount;
    public int[] rainTime;
    public float rainEffectDelta;
    
    public int windAmount;
    public int[] windTime;
    public float windEffectDelta;
    
}
