using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Buaa.AIBot.Services.CodeAnalyze
{
    public static class CppCheckResultTanslation
    {
        private static IReadOnlyDictionary<string, Func<CppCheckResult, string>> dict = new Dictionary<string, Func<CppCheckResult, string>>()
        {
            ["uninitvar"] = Uninitvar,
            ["unassignedVariable"] = UnassignedVariable,
        };

        public static string Translate(CppCheckResult cppCheckResult)
        {
            if (dict.TryGetValue(cppCheckResult.Category, out var func))
            {
                return func(cppCheckResult);
            }
            return $"[{cppCheckResult.Category}]{cppCheckResult.Message}";
        }


        private static string Uninitvar(CppCheckResult cppCheckResult)
        {
            var mc = Regex.Match(cppCheckResult.Message, "Uninitialized variable: (?<name>[\\d\\w]+)");
            var name = mc.Groups["name"].ToString();
            return $"使用未初始化的变量：{name}";
        }

        private static string UnassignedVariable(CppCheckResult cppCheckResult)
        {
            var mc = Regex.Match(cppCheckResult.Message, "Variable '(?<name>[\\d\\w]+)' is not assigned a value\\.");
            var name = mc.Groups["name"].ToString();
            return $"变量'{name}'没有进行赋值";
        }
    }
}
