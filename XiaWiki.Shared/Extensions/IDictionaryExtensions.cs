namespace XiaWiki.Shared.Extensions;

public static class IDictionaryExtensions
{
    public static TValue? GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue? defaultValue = default)
    {
        if (dict.TryGetValue(key, out TValue? value))
            return value;

        return defaultValue;
    }
}
