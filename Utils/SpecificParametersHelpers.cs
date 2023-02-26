using Utils.Coordinates;

namespace Utils;

public static class NamedParameters
{
    public static string Coordinates { get; } = nameof(Coordinates);
    public static string Weight { get; } = nameof(Weight);
    public static string PropName { get; } = nameof(PropName);
    public static string UniqueId { get; } = nameof(UniqueId);
    public static string Level { get; } = nameof(Level);
    public static string BaseCost { get; } = nameof(BaseCost);
    public static string Damage { get; } = nameof(Damage);
    public static string HitPoints { get; } = nameof(HitPoints);
    public static string ArmourClass { get; } = nameof(ArmourClass);
}

public static class SpecificParametersHelpers
{
    private static Parameters Add(Parameters parameters, string name, Coordinate value)
    {
        if (value != Coordinate.NotSet)
        {
            parameters.AppendParameter(name, value);
        }

        return parameters;
    }

    private static Parameters Add(Parameters parameters, string name, double value)
    {
        if (!value.IsZero())
        {
            parameters.AppendParameter(name, value);
        }

        return parameters;
    }

    private static Parameters Add(Parameters parameters, string name, int value)
    {
        if (value != 0)
        {
            parameters.AppendParameter(name, value);
        }

        return parameters;
    }

    private static Parameters Add(Parameters parameters, string name, string value)
    {
        if (! value.IsNullOrEmpty())
        {
            parameters.AppendParameter(name, value);
        }

        return parameters;
    }

    private static T Get<T>(Parameters parameters, string name) where T : struct
    {
        return parameters.HasValue(name) ?
            parameters.ToValue<T>(name) :
            default;
    }

    private static string Get(Parameters parameters, string name)
    {
        return parameters.SingleOrDefault(parameter => parameter.Name.IsSame(name)).Value;
    }

    public static Parameters AddUniqueId(this Parameters parameters, string uniqueId) => Add(parameters, NamedParameters.PropName, uniqueId);
    public static string GetUniqueId(this Parameters parameters) => Get(parameters, NamedParameters.PropName);

    public static Parameters AddPropName(this Parameters parameters, string name) => Add(parameters, NamedParameters.PropName, name);
    public static string GetPropName(this Parameters parameters) => Get(parameters, NamedParameters.PropName);

    public static Parameters AddWeight(this Parameters parameters, double weight) => Add(parameters, NamedParameters.Weight, weight);
    public static double GetWeight(this Parameters parameters) => Get<double>(parameters, NamedParameters.Weight);

    public static Parameters AddLevel(this Parameters parameters, int level) => Add(parameters, NamedParameters.Level, level);
    public static int GetLevel(this Parameters parameters) => Get<int>(parameters, NamedParameters.Level);

    public static Parameters AddDamage(this Parameters parameters, string damage) => Add(parameters, NamedParameters.Damage, damage);
    public static string GetDamage(this Parameters parameters) => Get(parameters, NamedParameters.Damage);

    public static Parameters AddBaseCost(this Parameters parameters, double cost) => Add(parameters, NamedParameters.BaseCost, cost);
    public static double GetOriginalCost(this Parameters parameters) => Get<double>(parameters, NamedParameters.BaseCost);

    public static Parameters AddCoordinates(this Parameters parameters, Coordinate coordinates) => Add(parameters, NamedParameters.Coordinates, coordinates);
    public static Parameters AddCoordinates(this Parameters parameters, int row, int column) => Add(parameters, NamedParameters.Coordinates, new Coordinate(row, column));
    public static Coordinate GetCoordinates(this Parameters parameters) => Get<Coordinate>(parameters, NamedParameters.Coordinates);

    public static Parameters AddHitPoints(this Parameters parameters, int hitPoints) => Add(parameters, NamedParameters.HitPoints, hitPoints);
    public static int GetHitPoints(this Parameters parameters) => Get<int>(parameters, NamedParameters.HitPoints);

    public static Parameters AddArmourClass(this Parameters parameters, int hitPoints) => Add(parameters, NamedParameters.ArmourClass, hitPoints);
    public static int GetArmourClass(this Parameters parameters) => Get<int>(parameters, NamedParameters.ArmourClass);
}
