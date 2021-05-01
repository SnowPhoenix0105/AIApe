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
        public static readonly string WindowsOS = WorkingModule.GovernmentInstallingInfo.WindowsOS;
        public static readonly string LinuxOS = WorkingModule.GovernmentInstallingInfo.LinuxOS;
        public static readonly string MacOS = WorkingModule.GovernmentInstallingInfo.MacOS;
        public static readonly string DevCpp = WorkingModule.GovernmentInstallingInfo.DevCpp;
        public static readonly string VisualCpp = WorkingModule.GovernmentInstallingInfo.VisualCpp;
        public static readonly string VSCode = WorkingModule.GovernmentInstallingInfo.VSCode;
        public static readonly string VS = WorkingModule.GovernmentInstallingInfo.VS;

        public static bool ToLowerContains(this string msg, params string[] alias)
        {
            string target = msg.ToLowerInvariant();
            foreach (var alia in alias)
            {
                if (target.Contains(alia.ToLowerInvariant()))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
