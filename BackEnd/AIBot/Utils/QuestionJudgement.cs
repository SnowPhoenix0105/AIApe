using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buaa.AIBot.Utils
{
    public static class QuestionJudgement
    {
        private static List<char> CharsThatCodesMustContain = new List<char>()
        {
            '{', '}', '(', ')', ';', '='
        };

        private static HashSet<char> CharsThatCodesMayContain = new HashSet<char>()
        {
            '+', '-', '*', '/', '.', '>', '<', '_', ',', ':', '?', '[', ']', '&', '^', '|', '#', '!', '~', 
            '\n', ' ', '\t', '\r', '\\', '\"', '\''
        };

        private static List<string> StringsThatCodesMayContain = new List<string>()
        {
            "#include ", "main", "int", "char", "for", "if", "while", "else", "static", "scanf", "printf", 
            "break", "continue",
            "import", 
            "unsigned", "NULL",
            "malloc", "free",
            "getchar", "getc", "putc", "putchar", "sqrt", "fputs", "fgets", "fputc", "fgetc", "size_t", "null", "nullptr", 
            
        };

        static QuestionJudgement()
        {
            CharsThatCodesMustContain.ForEach(c => CharsThatCodesMayContain.Add(c));
            Enumerable.Range('a', 'z').Select(i => (char)i).ToList().ForEach(c => CharsThatCodesMayContain.Add(c));
            Enumerable.Range('A', 'Z').Select(i => (char)i).ToList().ForEach(c => CharsThatCodesMayContain.Add(c));
            Enumerable.Range('0', '9').Select(i => (char)i).ToList().ForEach(c => CharsThatCodesMayContain.Add(c));
        }

        public static bool IsCode(string title, string content)
        {
            if (content.Length < 100)
            {
                return false;
            }
            foreach (char ch in CharsThatCodesMustContain)
            {
                if (!content.Contains(ch))
                {
                    return false;
                }
            }
            int target = (int)(content.Length * 0.7);
            if (content.Where(c => CharsThatCodesMayContain.Contains(c)).Take(target).Count() >= target)
            {
                return true;
            }
            if (content.Contains("```"))
            {
                return true;
            }
            target = 6;
            if (StringsThatCodesMayContain.Where(s => content.Contains(s)).Take(target).Count() >= target)
            {
                return true;
            }
            return false;
        }

        private static Dictionary<string, int> AddTagIfSatisfy(this Dictionary<string, int> dict, string content, string target, IReadOnlyDictionary<string, int> tags, params string[] alias)
        {
            string k = target;
            int v;
            if (!tags.TryGetValue(k, out v))
            {
                k = target.ToLowerInvariant();
                if (!tags.TryGetValue(k, out v))
                {
                    return dict;
                }
            }
            string contentLower = content.ToLowerInvariant();
            foreach (string alia in alias.Append(target))
            {
                if (contentLower.Contains(alia.ToLower()))
                {
                    dict.Add(k, v);
                    return dict;
                }
            }
            return dict;
        }

        public static async Task<Dictionary<string, int>> GenerageTagsForQuestionAsync(Repository.ITagRepostory tagRepostory, string title, string content)
        {
            var tags = await tagRepostory.SelectAllTagsAsync();
            var ret = new Dictionary<string, int>();

            ret.AddTagIfSatisfy(content, "Linux", tags,
                "ubuntu", "Redhat", "Debain", "Fedora", "openSUSE", "Mandriva", "Mint",
                "PCLinuxOS", "Cent OS", "CentOS", "Slackware", "Gentoo")
                .AddTagIfSatisfy(content, "Windows", tags, "win")
                .AddTagIfSatisfy(content, "macOS", tags, "mac os", "Macbook", "iMac")
                .AddTagIfSatisfy(content, "Dev C++", tags, "dev-cpp", "dev-c++")
                .AddTagIfSatisfy(content, "Visual C++", tags, "VC++", "VCPP")
                .AddTagIfSatisfy(content, "VS Code", tags, "vscode", "Visual Studio Code")
                .AddTagIfSatisfy(content, "Visual Studio", tags, "vs")
                .AddTagIfSatisfy(content, "gcc", tags, "GNU Compiler Collection", "g++")
                .AddTagIfSatisfy(content, "clang", tags, "llvm")
                .AddTagIfSatisfy(content, "msvc", tags, "Microsoft Visual C++")
                ;
            return ret;
        }
    }
}
