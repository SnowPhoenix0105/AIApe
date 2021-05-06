using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buaa.AIBot.Bot.Framework;

namespace Buaa.AIBot.Bot.WorkingModule
{
    public class IdeAndCompilerDocumentCollection
    {
        public static readonly string WindowsOS = ConstantStrings.OS.WindowsOS;
        public static readonly string LinuxOS = ConstantStrings.OS.LinuxOS;
        public static readonly string MacOS = ConstantStrings.OS.MacOS;

        public static readonly string DevCpp = ConstantStrings.IDE.DevCpp;
        public static readonly string VisualCpp = ConstantStrings.IDE.VisualCpp;
        public static readonly string VSCode = ConstantStrings.IDE.VSCode;
        public static readonly string VS = ConstantStrings.IDE.VS;

        public static readonly string Gcc = ConstantStrings.Compiler.Gcc;
        public static readonly string Clang = ConstantStrings.Compiler.Clang;
        public static readonly string Msvc = ConstantStrings.Compiler.Msvc;

        public void SendIDEDocumentMessages(string os, string ide, string compiler, IBotSender sender)
        {
            // TODO
        }
    }
}
