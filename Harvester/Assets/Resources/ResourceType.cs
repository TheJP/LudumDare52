public enum ResourceType
{
    None,
    BuildingMaterial,
    Fuel,
    Health,
    Research,
}

public static class ResourceTypeExtensions
{
    public static ResourceType[] Types => new[]
    {
        ResourceType.BuildingMaterial,
        ResourceType.Fuel,
        ResourceType.Health,
        ResourceType.Research
    };
}