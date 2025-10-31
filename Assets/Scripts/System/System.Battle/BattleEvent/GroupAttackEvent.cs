using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GroupAttackEvent : BattleEvent
{
    public List<BattleUnit> Participants;
    public BattleUnit Leader;
    public float GroupMultiplier = 1.0f;
    public GroupAttackEvent(List<BattleUnit> participants, BattleUnit leader, int scheduledTime, int priority) : base(scheduledTime, priority)
    {
        Participants = participants;
        Leader = leader;
    }

    public override async Task Execute(BattleContext context)
    {
        // 実行前に全員健在かチェック
        bool allActive = Participants.All(e => e.IsAlive() && !e.IsDisabled());

        if (!allActive)
        {
            await HandleGroupCancel(context.Hero, context);
            return;
        }

        // 代表者の攻撃力ベース
        int damagePerUnit = Mathf.RoundToInt(Leader.Strength);
        int totalDamage = Mathf.RoundToInt(damagePerUnit + GroupMultiplier);

        context.Hero.TakeDamage(totalDamage);
    }

    private async Task HandleGroupCancel(IHeroUnit player, BattleContext context)
    {
        // 必ずリワード発生
        player.GainMana(1);
        await context.BattleSystem.Draw(1);
        // ここで「協調攻撃を阻止した」演出を入れると分かりやすい
    }
}
