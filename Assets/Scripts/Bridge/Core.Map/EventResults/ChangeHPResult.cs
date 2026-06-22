using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

[CreateAssetMenu(menuName = "Events/Results/ChangeHP")]
public class ChangeHPResult : EventResult
{
    public override void Apply(IGameContext context, IFlagManager flagManager, EventOption option)
    {
        if (option.HPChangeAmount > 0)
        {
            context.Hero.HPResource.Gain(option.HPChangeAmount);
        }
        else if (option.HPChangeAmount < 0)
        {
            context.Hero.HPResource.LoseHP(-option.HPChangeAmount);
        }
        else if (option.HPChangePer > 0)
        {
            context.Hero.HPResource.Gain(Convert.ToInt32(Math.Floor(context.Hero.HPResource.MaxHP * option.HPChangePer * 0.01)));
        }
        else
        {
            context.Hero.HPResource.LoseHP(Convert.ToInt32(Math.Floor(context.Hero.HPResource.MaxHP * option.HPChangePer * -0.01)));
        }
    }
}
