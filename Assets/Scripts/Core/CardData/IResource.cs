using System;

public interface IResource
{
    ResourceType Type { get; }
    int CurrentMana { get; }
    bool TryConsume(int amount);
    void Gain(int amount);
    event Action OnChanged;
}