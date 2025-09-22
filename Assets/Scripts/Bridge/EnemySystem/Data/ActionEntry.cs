[System.Serializable]
public class ActionEntry
{
    public NormalAction action;  // AttackActionなど
    public int damageOverride;   // ここで上書き可能
    public int amountOverride;
}