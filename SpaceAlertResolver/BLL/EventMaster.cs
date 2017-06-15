using System;

namespace BLL
{
    public class EventMaster
    {
        public event EventHandler<PhaseEventArgs> EventTriggered = (sender, args) => { };
        public void LogEvent(string description)
        {
            EventTriggered(this, new PhaseEventArgs {PhaseHeader = description});
        }
    }
}
