using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUpgradePopup : AcceptPopup
{
    const string CURR_LEVEL = "CURRENT %:";
    const string NEXT_LEVEL = "NEXT %:";

    public Image currLevelFill;
    public Image nextLevelFill;

    public void SetPopup(MinionBoughtDef.StatNames statId, string currLevelVal, string nextLevelVal, int currLevel)
    {
        var currLevelFloat = float.Parse(currLevel.ToString());
        currLevelFill.fillAmount = currLevelFloat / ShopManager.MAX_MINION_LEVEL;
        currLevelFloat++;
        nextLevelFill.fillAmount = currLevelFloat / ShopManager.MAX_MINION_LEVEL;

        var currInfo = CURR_LEVEL.Replace("%", statId.ToString()) + currLevelVal;
        var nextInfo = NEXT_LEVEL.Replace("%", statId.ToString()) + nextLevelVal;

        if (description.text == "")
            description.text = statId == MinionBoughtDef.StatNames.HP ? ShopManager.HP_STAT_DESC : ShopManager.SPEED_STAT_DESC;
        description.text += '\n';
        description.text += '\n' + currInfo + '\n' + nextInfo;
    }


    protected override void CloseButton()
    {
        OnClosePopup();
    }

}
