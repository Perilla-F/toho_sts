public class BattleCell : CellBehavior
{
    public EncounterData AssignedEncounter;

    public override void OnPlayerEnter()
    {
        if (AssignedEncounter != null)
        {
            OnBattleRequest?.Invoke(AssignedEncounter);
        }
    }
}