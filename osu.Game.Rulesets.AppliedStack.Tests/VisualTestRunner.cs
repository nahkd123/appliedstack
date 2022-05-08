using System;
using osu.Framework;
using osu.Framework.Platform;
using osu.Game.Tests;

namespace osu.Game.Rulesets.AppliedStack.Tests
{
    public static class VisualTestRunner
    {
        [STAThread]
        public static int Main(string[] args)
        {
            using (DesktopGameHost host = Host.GetSuitableDesktopHost(@"osu", new HostOptions { BindIPC = true }))
            {
                host.Run(new OsuTestBrowser());
                return 0;
            }
        }
    }
}
