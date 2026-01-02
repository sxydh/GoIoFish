using System;
using NLog;
using NLog.Targets;

namespace GoIoFish.Logging
{
    [Target("UiTarget")]
    public class UiTarget : TargetWithLayout
    {
        protected override void Write(LogEventInfo logEvent)
        {
            var msg = Layout.Render(logEvent);
            UiLogStore.Add(msg.Substring(0, Math.Min(300, msg.Length)), logEvent.Level.Name);
        }
    }
}