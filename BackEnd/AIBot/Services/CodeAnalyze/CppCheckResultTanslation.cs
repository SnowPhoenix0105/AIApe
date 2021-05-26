using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Buaa.AIBot.Services.CodeAnalyze
{
    public static class CppCheckResultTanslation
    {
        private static readonly IReadOnlyDictionary<string, Func<CppCheckResult, string>> dict;

        static CppCheckResultTanslation()
        {
            var tmp = new Dictionary<string, Func<CppCheckResult, string>>()
            {
                ["uninitvar"] = Uninitvar,
                ["uninitdata"] = Uninitdata,
                ["unassignedVariable"] = UnassignedVariable,
                ["memleak"] = MemLeak,
                ["unusedAllocatedMemory"] = UnusedAllocatedMemory,
                ["unreadVariable"] = UnreadVariable,
                ["arrayIndexOutOfBounds"] = ArrayIndexOutOfBounds,
                ["unusedFunction"] = UnusedFunction,
                ["CastAddressToIntegerAtReturn"] = CastAddressToIntegerAtReturn,
                ["AssignmentIntegerToAddress"] = AssignmentIntegerToAddress,
                ["deallocret"] = Deallocret,
                ["deallocuse"] = Deallocuse,
            };

            dict = new Dictionary<string, Func<CppCheckResult, string>>(
                tmp.Select(kv => 
                    new KeyValuePair<string, Func<CppCheckResult, string>>(kv.Key.ToLowerInvariant(), kv.Value)));
        }

        public static string Translate(CppCheckResult cppCheckResult)
        {
            if (dict.TryGetValue(cppCheckResult.Category.ToLowerInvariant(), out var func))
            {
                return func(cppCheckResult);
            }
            return $"暂无翻译的错误信息：[{cppCheckResult.Category}] {cppCheckResult.Message}";
        }

        private static string Deallocuse(CppCheckResult cppCheckResult)
        {
            var mc = Regex.Match(cppCheckResult.Message, "Dereferencing '(?<name>[\\d\\w]+)' after it is deallocated / released");
            var name = mc.Groups["name"].ToString();
            return $"尝试访问一个已失效的指针'{name}'，因为其指向的动态分配的内存已经被释放";
        }

        private static string Deallocret(CppCheckResult cppCheckResult)
        {
            var mc = Regex.Match(cppCheckResult.Message, "Returning/dereferencing '(?<name>[\\d\\w]+)' after it is deallocated / released");
            var name = mc.Groups["name"].ToString();
            return $"尝试返回或者访问一个已失效的指针'{name}'，因为其指向的动态分配的内存已经被释放";
        }

        private static string AssignmentIntegerToAddress(CppCheckResult cppCheckResult)
        {
            return "将一个指针赋值给一个整型，将进行可能导致错误的指针向整型的转型";
        }

        private static string CastAddressToIntegerAtReturn(CppCheckResult cppCheckResult)
        {
            return "在返回值为整型的函数中，返回了指针类型，将进行可能导致错误的指针向整型的转型";
        }

        private static string UnusedFunction(CppCheckResult cppCheckResult)
        {
            var mc = Regex.Match(cppCheckResult.Message, "The function '(?<name>[\\d\\w]+)' is never used");
            var name = mc.Groups["name"].ToString();
            return $"从未使用过的函数：'{name}'";
        }

        private static string ArrayIndexOutOfBounds(CppCheckResult cppCheckResult)
        {
            var mc = Regex.Match(cppCheckResult.Message, "Array '(?<name>[\\d\\w]+)' accessed at index '(?<index>[\\d\\w]+)', which is out of bounds");
            var name = mc.Groups["name"].ToString();
            var index = mc.Groups["index"].ToString();
            return $"数组'{name}'访问了下标为{index}的元素，该下标越界";
        }

        private static string UnreadVariable(CppCheckResult cppCheckResult)
        {
            var mc = Regex.Match(cppCheckResult.Message, "Variable '(?<name>[\\d\\w]+)' is assigned a value that is never used");
            var name = mc.Groups["name"].ToString();
            return $"变量初始化后没有使用：'{name}'";
        }

        private static string UnusedAllocatedMemory(CppCheckResult cppCheckResult)
        {
            var mc = Regex.Match(cppCheckResult.Message, "Variable '(?<name>[\\d\\w]+)' is allocated memory that is never used");
            var name = mc.Groups["name"].ToString();
            return $"申请了动态内存但没有使用：'{name}'";
        }

        private static string MemLeak(CppCheckResult cppCheckResult)
        {
            var mc = Regex.Match(cppCheckResult.Message, "Memory leak: (?<name>[\\d\\w]+)");
            var name = mc.Groups["name"].ToString();
            return $"内存泄漏：'{name}'";
        }

        private static string Uninitdata(CppCheckResult cppCheckResult)
        {
            var mc = Regex.Match(cppCheckResult.Message, "Memory is allocated but not initialized: (?<name>[\\d\\w]+)");
            var name = mc.Groups["name"].ToString();
            return $"访问了动态申请的内存，但是其尚未进行初始化：'{name}'";
        }

        private static string Uninitvar(CppCheckResult cppCheckResult)
        {
            var mc = Regex.Match(cppCheckResult.Message, "Uninitialized variable: (?<name>[\\d\\w]+)");
            var name = mc.Groups["name"].ToString();
            return $"使用未初始化的变量：'{name}'";
        }

        private static string UnassignedVariable(CppCheckResult cppCheckResult)
        {
            var mc = Regex.Match(cppCheckResult.Message, "Variable '(?<name>[\\d\\w]+)' is not assigned a value");
            var name = mc.Groups["name"].ToString();
            return $"变量'{name}'没有进行赋值";
        }
    }
}
