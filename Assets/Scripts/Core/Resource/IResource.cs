using System;

public interface IResource
{
    ResourceType Type { get; }
    int CurrentResource { get; }
    bool TryConsume(int amount);
    void Gain(int amount);
    event Action OnChanged;
}