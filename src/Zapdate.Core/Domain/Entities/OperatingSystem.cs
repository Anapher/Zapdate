using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Zapdate.Core.Domain.Entities
{
    public class OperatingSystem
    {
        public OperatingSystem()
        {

        }

        public OperatingSystemType Type { get; private set; }
        public DetectedOperatingSystem DetectedSystem { get; private set; }
    }

    public enum DetectedOperatingSystem
    {
        [Description("Unknown Operating System")] [Group(null)] Unknown = 0,
        [Description("Windows Vista")] [Group("Windows")] WindowsVista = 1,
        [Description("Windows 7")] [Group("Windows")] Windows7 = 2,
        [Description("Windows 8/8.1")] [Group("Windows")] Windows8 = 3,
        [Description("Windows 10")] [Group("Windows")] Windows10 = 4,
        [Description("Windows Server 2008")] [Group("Windows")] WindowsServer2008 = 20,
        [Description("Windows Server 2012")] [Group("Windows")] WindowsServer2012 = 21,
        [Description("Windows Server 2016")] [Group("Windows")] WindowsServer2016 = 22,
        [Description("Windows IoT")] [Group("Windows")] WindowsIot = 31,
        [Description("Windows (Other)")] [Group("Windows")] WindowsOther = 32,
        [Description("Linux Debian")] [Group("Linux")] LinuxDebian = 100,
        [Description("Linux Ubuntu")] [Group("Linux")] LinuxUbuntu = 101,
        [Description("Linux Mint")] [Group("Linux")] LinuxMint = 102,
        [Description("Linux Fedora")] [Group("Linux")] LinuxFedora = 103,
        [Description("Linux CentOS")] [Group("Linux")] LinuxCentOs = 104,
        [Description("Linux Slackware")] [Group("Linux")] LinuxSlackware = 105,
        [Description("Linux (Other)")] [Group("Linux")] LinuxOther = 106,
        [Description("OSX (Other)")] [Group("OSX")] OsxOther = 200,
    }

    public enum OperatingSystemType
    {
        Windows,
        WindowsServer,
        Linux,
        OSX,
        Unknown
    }

    public class GroupAttribute : Attribute
    {
        public GroupAttribute(string? groupName)
        {
        }
    }
}
