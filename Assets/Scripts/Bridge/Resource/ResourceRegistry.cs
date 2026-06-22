using System.Collections.Generic;

public class ResourceRegistry
{
    private readonly Dictionary<ResourceType, IResource> _resources = new();

    public void Register(IResource resource)
    {
        _resources[resource.Type] = resource;
    }

    public IResource Get(ResourceType type)
    {
        return _resources.TryGetValue(type, out var r) ? r : null;
    }
}