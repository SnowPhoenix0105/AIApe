using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buaa.AIBot.Bot.AlphaBot
{
    public static class Key
    {
        public static readonly string SimpleDescribe = WorkingModule.QuestionBuilder.SimpleDescribe;
        public static readonly string DetailDescribe = WorkingModule.QuestionBuilder.DetailDescribe;
        public static readonly string OS = WorkingModule.QuestionBuilder.OS;
        public static readonly string OS_detail = WorkingModule.QuestionBuilder.OS_detail;
        public static readonly string IDE = WorkingModule.QuestionBuilder.IDE;
        public static readonly string IDE_detail = WorkingModule.QuestionBuilder.IDE_detail;
    }

    public static class Value
    {
        public static readonly string WindowsOS = "Windows";
        public static readonly string LinuxOS = "Linux";
        public static readonly string MaxOS = "macOS";

        public static readonly string DevCpp = "Dev C++";
        public static readonly string VisualCpp = "Visual C++";
        public static readonly string VSCode = "VS Code";
        public static readonly string VS = "Visual Studio";
    }
}
