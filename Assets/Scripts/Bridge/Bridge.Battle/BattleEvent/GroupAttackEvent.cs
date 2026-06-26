using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class GroupAttackEvent : BattleEvent
{
    public List<IBattleUnit> Participants;
    public IBattleUnit Leader;
    public float GroupMultiplier = 1.0f;
    public GroupAttackEvent(List<IBattleUnit> participants, IBattleUnit leader, Sprite eventIcon, int scheduledTime, int priority) : base(eventIcon, scheduledTime, priority)
    {
        Participants = participants;
        Leader = leader;
        EventIcon = eventIcon;
    }

    public override async UniTask Execute(IBattleContext context)
    {
        // 実行前に全員健在かチェック
        bool allActive = Participants.All(e => e.IsAlive() && !e.IsDisabled());

        if (!allActive)
        {
            await HandleGroupCancel(context.Hero, context);
            return;
        }

        // 代表者の攻撃力ベース
        int damagePerUnit = Mathf.RoundToInt(Leader.StatusCount("strength"));
        int totalDamage = Mathf.RoundToInt(damagePerUnit + GroupMultiplier);

        context.BattleSystem.ExecuteAttack(Leader, context.Hero, totalDamage);
    }

    private async UniTask HandleGroupCancel(IHeroUnit player, IBattleContext context)
    {
        // 必ずリワード発生
        player.GainMana(1);
        await context.BattleSystem.DrawMultipleAsync(1);
        // ここで「協調攻撃を阻止した」演出を入れると分かりやすい
    }
}
