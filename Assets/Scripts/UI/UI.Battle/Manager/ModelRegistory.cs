using System.Collections.Generic;

public class ModelRegistory
{
    private Dictionary<IReadOnlyBattleUnit, BaseBattleModel> _modelDirectory = new();

    public BaseBattleModel GetModelForUnit(IReadOnlyBattleUnit unit)
    {
        return _modelDirectory.TryGetValue(unit, out var model) ? model : null;
    }

    public void RegisterModel(IReadOnlyBattleUnit unit, BaseBattleModel model)
    {
        _modelDirectory[unit] = model;
    }

    public void RemoveModel(IReadOnlyBattleUnit unit)
    {
        _modelDirectory.Remove(unit);
    }

    public void Clear()
    {
        _modelDirectory.Clear();
    }
}