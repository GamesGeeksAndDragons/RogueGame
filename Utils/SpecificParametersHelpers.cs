using Utils.Coordinates;

namespace Utils;

public static class NamedParameters
{
    public const string Coordinates = nameof(Coordinates);
    public const string Weight = nameof(Weight);
    public const string PropName = nameof(PropName);
    public const string UniqueId = nameof(UniqueId);
    public const string Level = nameof(Level);
    public const string BaseCost = nameof(BaseCost);
    public const string Damage = nameof(Damage);
    public const string HitPoints = nameof(HitPoints);
    public const string ArmourClass = nameof(ArmourClass);
    public const string StrengthBase = nameof(StrengthBase);
    public const string StrengthCurrent = nameof(StrengthCurrent);
    public const string StrengthBaseExceptional = nameof(StrengthBaseExceptional);
    public const string StrengthCurrentExceptional = nameof(StrengthCurrentExceptional);
    public const string StrengthHitBonus = nameof(StrengthHitBonus);
    public const string StrengthDamageBonus = nameof(StrengthDamageBonus);
    public const string DexterityBase = nameof(DexterityBase);
    public const string DexterityCurrent = nameof(DexterityCurrent);
    public const string DexterityBaseExceptional = nameof(DexterityBaseExceptional);
    public const string DexterityCurrentExceptional = nameof(DexterityCurrentExceptional);
    public const string DexterityHitBonus = nameof(DexterityHitBonus);
    public const string DexterityAcBonus = nameof(DexterityAcBonus);
    public const string DexterityDisarmBonus = nameof(DexterityDisarmBonus);
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
    public static double GetBaseCost(this Parameters parameters) => Get<double>(parameters, NamedParameters.BaseCost);

    public static Parameters AddCoordinates(this Parameters parameters, Coordinate coordinates) => Add(parameters, NamedParameters.Coordinates, coordinates);
    public static Parameters AddCoordinates(this Parameters parameters, int row, int column) => Add(parameters, NamedParameters.Coordinates, new Coordinate(row, column));
    public static Coordinate GetCoordinates(this Parameters parameters) => Get<Coordinate>(parameters, NamedParameters.Coordinates);
    public static string ToCoordinateString(this Coordinate coordinate) => coordinate.ToParameter(NamedParameters.Coordinates);

    public static Parameters AddHitPoints(this Parameters parameters, int value) => Add(parameters, NamedParameters.HitPoints, value);
    public static int GetHitPoints(this Parameters parameters) => Get<int>(parameters, NamedParameters.HitPoints);
    public static string ToHitPointsString(this int hitPoints) => hitPoints.ToParameter(NamedParameters.HitPoints);

    public static Parameters AddArmourClass(this Parameters parameters, int value) => Add(parameters, NamedParameters.ArmourClass, value);
    public static int GetArmourClass(this Parameters parameters) => Get<int>(parameters, NamedParameters.ArmourClass);
    public static string ToArmourClassString(this int armourClass) => armourClass.ToParameter(NamedParameters.ArmourClass);

    // Strength
    public static Parameters AddStrengthBase(this Parameters parameters, int value) => Add(parameters, NamedParameters.StrengthBase, value);
    public static int GetStrengthBase(this Parameters parameters) => Get<int>(parameters, NamedParameters.StrengthBase);

    public static Parameters AddStrengthCurrent(this Parameters parameters, int value) => Add(parameters, NamedParameters.StrengthCurrent, value);
    public static int GetStrengthCurrent(this Parameters parameters) => Get<int>(parameters, NamedParameters.StrengthCurrent);

    public static Parameters AddStrengthBaseExceptional(this Parameters parameters, int value) => Add(parameters, NamedParameters.StrengthBaseExceptional, value);
    public static int GetStrengthBaseExceptional(this Parameters parameters) => Get<int>(parameters, NamedParameters.StrengthBaseExceptional);

    public static Parameters AddStrengthCurrentExceptional(this Parameters parameters, int value) => Add(parameters, NamedParameters.StrengthCurrentExceptional, value);
    public static int GetStrengthCurrentExceptional(this Parameters parameters) => Get<int>(parameters, NamedParameters.StrengthCurrentExceptional);

    public static Parameters AddStrengthHitBonus(this Parameters parameters, int value) => Add(parameters, NamedParameters.StrengthHitBonus, value);
    public static int GetStrengthHitBonus(this Parameters parameters) => Get<int>(parameters, NamedParameters.StrengthHitBonus);

    public static Parameters AddStrengthDamageBonus(this Parameters parameters, int value) => Add(parameters, NamedParameters.StrengthDamageBonus, value);
    public static int GetStrengthDamageBonus(this Parameters parameters) => Get<int>(parameters, NamedParameters.StrengthDamageBonus);

    // Dexterity
    public static Parameters AddDexterityBase(this Parameters parameters, int value) => Add(parameters, NamedParameters.DexterityBase, value);
    public static int GetDexterityBase(this Parameters parameters) => Get<int>(parameters, NamedParameters.DexterityBase);

    public static Parameters AddDexterityCurrent(this Parameters parameters, int value) => Add(parameters, NamedParameters.DexterityCurrent, value);
    public static int GetDexterityCurrent(this Parameters parameters) => Get<int>(parameters, NamedParameters.DexterityCurrent);

    public static Parameters AddDexterityBaseExceptional(this Parameters parameters, int value) => Add(parameters, NamedParameters.DexterityBaseExceptional, value);
    public static int GetDexterityBaseExceptional(this Parameters parameters) => Get<int>(parameters, NamedParameters.DexterityBaseExceptional);

    public static Parameters AddDexterityCurrentExceptional(this Parameters parameters, int value) => Add(parameters, NamedParameters.DexterityCurrentExceptional, value);
    public static int GetDexterityCurrentExceptional(this Parameters parameters) => Get<int>(parameters, NamedParameters.DexterityCurrentExceptional);

    public static Parameters AddDexterityHitBonus(this Parameters parameters, int value) => Add(parameters, NamedParameters.DexterityHitBonus, value);
    public static int GetDexterityHitBonus(this Parameters parameters) => Get<int>(parameters, NamedParameters.DexterityHitBonus);

    public static Parameters AddDexterityAcBonus(this Parameters parameters, int value) => Add(parameters, NamedParameters.DexterityAcBonus, value);
    public static int GetDexterityAcBonus(this Parameters parameters) => Get<int>(parameters, NamedParameters.DexterityAcBonus);

    public static Parameters AddDexterityDisarmBonus(this Parameters parameters, int value) => Add(parameters, NamedParameters.DexterityDisarmBonus, value);
    public static int GetDexterityDisarmBonus(this Parameters parameters) => Get<int>(parameters, NamedParameters.DexterityDisarmBonus);
}
