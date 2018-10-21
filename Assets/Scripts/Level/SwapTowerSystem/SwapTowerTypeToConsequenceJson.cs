using System;

/// <summary>
/// This will have the minion type that will trigger the swap.
/// Also, the array of tower types that would be created and will replace and old one if the trigger is raised.
/// </summary>
[Serializable]
public class SwapTowerTypeToConsequenceJson
{
    public string triggerType;
    public string[] newTowers;
    public string[] toBeReplaced;
}
