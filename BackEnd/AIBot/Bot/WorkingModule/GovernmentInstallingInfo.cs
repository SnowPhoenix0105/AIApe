using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buaa.AIBot.Bot.Framework;

namespace Buaa.AIBot.Bot.WorkingModule
{
    public class GovernmentInstallingInfo
    {
        public static readonly string WindowsOS = ConstantStrings.OS.WindowsOS ;
        public static readonly string LinuxOS   = ConstantStrings.OS.LinuxOS;
        public static readonly string MacOS = ConstantStrings.OS.MacOS;

        public static readonly string DevCpp = ConstantStrings.IDE.DevCpp;
        public static readonly string VisualCpp = ConstantStrings.IDE.VisualCpp;
        public static readonly string VSCode = ConstantStrings.IDE.VSCode;
        public static readonly string VS = ConstantStrings.IDE.VS;

        public void SendInstallingMessages(string os, string target, IBotSender sender)
        {
            if (target == DevCpp)
            {
                if (os == WindowsOS)
                {
                    WinDev(sender);
                    return;
                }
                else if (os == LinuxOS)
                {
                    LinuxDev(sender);
                    return;
                }
                else if (os == MacOS)
                {
                    MacDev(sender);
                    return;
                }
                throw new ArgumentException($"unknown os={os}");
            }
            else if (target == VisualCpp)
            {
                if (os == WindowsOS)
                {
                    WinVC(sender);
                    return;
                }
                else if (os == LinuxOS)
                {
                    LinuxVC(sender);
                    return;
                }
                else if (os == MacOS)
                {
                    MacVC(sender);
                    return;
                }
                throw new ArgumentException($"unknown os={os}");
            }
            else if (target == VSCode)
            {

                if (os == WindowsOS)
                {
                    WinVSCode(sender);
                    return;
                }
                else if (os == LinuxOS)
                {
                    LinuxVSCode(sender);
                    return;
                }
                else if (os == MacOS)
                {
                    MacVSCode(sender);
                    return;
                }
                throw new ArgumentException($"unknown os={os}");
            }
            else if (target == VS)
            {
                if (os == WindowsOS)
                {
                    WinVS(sender);
                    return;
                }
                else if (os == LinuxOS)
                {
                    LinuxVS(sender);
                    return;
                }
                else if (os == MacOS)
                {
                    MacVS(sender);
                    return;
                }
                throw new ArgumentException($"unknown os={os}");
            }
            throw new ArgumentException($"unknown target={target}");
        }

        private void WinDev(IBotSender sender)
        {
            // TODO
        }

        private void LinuxDev(IBotSender sender)
        {
            // TODO
        }

        private void MacDev(IBotSender sender)
        {
            // TODO
        }

        private void WinVC(IBotSender sender)
        {
            // TODO
        }

        private void LinuxVC(IBotSender sender)
        {
            // TODO
        }

        private void MacVC(IBotSender sender)
        {
            // TODO
        }

        private void WinVS(IBotSender sender)
        {
            // TODO
        }

        private void LinuxVS(IBotSender sender)
        {
            // TODO
        }

        private void MacVS(IBotSender sender)
        {
            // TODO
        }

        private void WinVSCode(IBotSender sender)
        {
            // TODO
        }

        private void LinuxVSCode(IBotSender sender)
        {
            // TODO
        }

        private void MacVSCode(IBotSender sender)
        {
            // TODO
        }
    }
}
