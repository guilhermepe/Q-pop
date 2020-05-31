using System.Collections.Generic;

namespace Q_pop
{

    public class CallsWaitingReport
    {
       public IList<CallWaitingData> ReportData { get; set; }
    }

    public class CallWaitingData
    {
        public string Skill { get; set; }
        public string CallsWaiting { get; set; }
        public string Oldest { get; set; }        

    }
}