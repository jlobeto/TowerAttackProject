using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerSkill : BaseMinionSkill
{

    protected override void Start()
    {
        base.Start();
        skillType = SkillType.GiveHealth;
    }

    /// <summary>
    /// It is not necessary to run ExecuteSkill, this is a one frame thing, so we do that in here.
    /// </summary>
    public void InitializeHealerSkill(List<Minion> toGiveHealth, float cooldown, ProjectilePS ps, int healAmount)
    {
        var ok = Initialize(0, cooldown);
        if (!ok) return;

        foreach (var item in toGiveHealth)
        {
            var toGive = (healAmount * item.InitialHP) / 100;
            if (toGive < 15)
                toGive = 15;

            item.GetHealth(toGive);
            var particles = Instantiate<ProjectilePS>(ps);
            particles.Init(transform, item.transform, 7);
        }
    }

}
