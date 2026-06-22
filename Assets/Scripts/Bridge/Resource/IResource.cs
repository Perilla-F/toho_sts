using System;
using Cysharp.Threading.Tasks;

public interface IResource
{
    ResourceType Type { get; }
    int CurrentResource { get; }
    bool TryConsume(int amount);
    void Gain(int amount);
}