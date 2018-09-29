using System;

[Serializable]
public class EnvironmentEventItem
{
    public int levelId;
    public int[] eventTimer;

    public bool bridgeEnabled;
    //warning time before bridge moves.
    public float warningTime;

    public string[] pivotNames;
    public string[] destinationNames;
}
