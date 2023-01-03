namespace ApiEngine.Extensions;

public static class CommonExtension
{
    /// <summary>
    ///     是否为null或空
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this string str)
    {
        return string.IsNullOrEmpty(str);
    }

    /// <summary>
    ///     ToEmptyString
    /// </summary>
    /// <param name="obj"></param>
    /// a
    /// <returns></returns>
    public static string ToEmptyString(this object obj)
    {
        return (obj ?? "").ToString()?.Trim();
    }

    /// <summary>
    ///     将任意类型转化为字符串，如果是null或空，则返回指定的默认值
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static string ToStringWithDefault(this object obj, string defaultValue = "")
    {
        var val = obj.ToEmptyString();
        return val.IsNullOrEmpty() ? defaultValue : val.Trim();
    }

    /// <summary>
    ///     将数字字符串转为decimal类型，并保留对应的小数位数
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="i"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static decimal ToDecimal(this object obj, int i = 2, decimal defaultValue = 0)
    {
        var val = obj.ToEmptyString();
        return Math.Round(val.IsNullOrEmpty() ? defaultValue : decimal.Parse(val), i);
    }

    /// <summary>
    ///     将字符串转化为指定的类型，如果是null或空，则按照指定的默认值进行转化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="val"></param>
    /// <returns></returns>
    public static T ConvertType<T>(this string val)
    {
        if (typeof(T) != typeof(bool))
        {
            return (T)Convert.ChangeType(val, typeof(T));
        }

        if (val.IsNullOrEmpty() || val.ToLower() == "false" || val == "0")
        {
            val = "False";
        }
        else
        {
            val = "True";
        }

        return (T)Convert.ChangeType(val, typeof(T));
    }

    public static string ToBase64String(this string value)
    {
        if (value.IsNullOrEmpty())
        {
            return "";
        }

        var bytes = Encoding.UTF8.GetBytes(value);
        return Convert.ToBase64String(bytes);
    }

    public static string UnBase64String(this string value)
    {
        if (value.IsNullOrEmpty())
        {
            return "";
        }

        var bytes = Convert.FromBase64String(value);
        return Encoding.UTF8.GetString(bytes);
    }

    public static string StringJoin(this IEnumerable<object> enumerable, string separator)
    {
        return string.Join(separator, enumerable);
    }

    public static bool ContainsIgnoreCase(this string source, string substring)
    {
        return source?.IndexOf(substring, StringComparison.OrdinalIgnoreCase) > -1;
    }

    public static bool ContainsIgnoreCase(this List<string> list, string substring)
    {
        return list.FindAll(s => s.ContainsIgnoreCase(substring)).Count > 0;
    }
}