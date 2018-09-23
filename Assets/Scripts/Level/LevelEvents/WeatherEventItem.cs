using System;
[Serializable]
public class WeatherEventItem
{
    public int levelId;

    public int rainAmount;
    public int[] rainTime;
    public int[] rainCooldown;
    public float rainEffectDelta;
    
    public int windAmount;
    public int[] windTime;
    public int[] windCooldown;
    public float windEffectDelta;
    
}
