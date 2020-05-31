using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Q_pop
{
    class Queue
    {
        private String queueToMonitor { get; set; }
        public String availability { get; private set; }
        public String callsWaiting { get; private set; }
        public String connectioStatus { get; private set; }
        public  bool availabilityChange { get; private set; }
        public bool callsWaitingChange { get; private set; }

        private String payload;
        

        private CookieAwareWebClient client = new CookieAwareWebClient();

        /* Constructor */
        public Queue(String queueToMonitor)
        {
            this.queueToMonitor = queueToMonitor;
            client.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
            this.availability = "No data";
            this.callsWaiting = "No data";

        }

        private bool available() // Check if aceyus site is reachable. 
        {
            bool status = false;

            try
            {                
                payload = client.DownloadString("http://ald-amer.us.dell.com/ALD/Home/Agent");
                this.connectioStatus = " Connection OK ";
                return true;

            }
            catch (Exception)
            {
                this.connectioStatus = " Can't reach http://ald-amer.us.dell.com/ALD/Home/Agent";
                this.availability = "No data";
                this.callsWaiting = "No data";
                return status;
                throw;
            }
        }

        /*Connects on aceyus reports, extract payload and fill strings for display*/
        public void checkAceyus()
        {
           
            if (this.available())
            {
                //Request the Skill service level which will be used on sebsequent string composition.
                payload = client.DownloadString("http://ald-amer.us.dell.com/ALD/Home//Report9");
                // Str replace to solve Json property with space char
                payload = payload.Replace("Service Level", "ServiceLevel");
                Console.WriteLine(ExtractReportPayload(payload));
                SkillReport skillReport;
                skillReport = JsonSerializer.Deserialize<SkillReport>(ExtractReportPayload(payload));
                IList<SkillData> skills = skillReport.ReportData;
                SkillData sd = (from s in skills where s.Skill.Equals(this.queueToMonitor) select s).FirstOrDefault();


                //Request the Skill first report to catch the headcount and availability.
                payload = client.DownloadString("http://ald-amer.us.dell.com/ALD/Home//Report7");
                AgentsReport agenstsReport;
                agenstsReport = JsonSerializer.Deserialize<AgentsReport>(ExtractReportPayload(payload));
                IList<ReportData> reports = agenstsReport.ReportData;
                ReportData rd = (from r in reports where r.Skill.Equals(this.queueToMonitor) select r).FirstOrDefault();

                String pre = "Available: " + rd.Avail + " -- Staffed: " + rd.Staffed + " -- SLA: " + sd.ServiceLevel;
                availability = pre;
                if (!pre.Equals(availability))
                {
                    availability = pre;
                    this.availabilityChange = true;

                }


                //Request the Skill second report to catch calls waiting.
                payload = client.DownloadString("http://ald-amer.us.dell.com/ALD/Home//Report8");
                CallsWaitingReport callsWaitingReport;
                callsWaitingReport = JsonSerializer.Deserialize<CallsWaitingReport>(ExtractReportPayload(payload));
                IList<CallWaitingData> calls = callsWaitingReport.ReportData;
                CallWaitingData cd = (from c in calls where c.Skill.Equals(this.queueToMonitor) select c).FirstOrDefault();

                pre = "Calls waiting: " + cd.CallsWaiting + " -- Oldest: " + cd.Oldest + " -- SLA: " + sd.ServiceLevel;
                callsWaiting = pre;
                if (!pre.Equals(callsWaiting))
                {
                    callsWaiting = pre;
                    callsWaitingChange = true;
                }

            }

        }

        private String ExtractReportPayload(String input)
        {
            String payload = "";
            //String Search to catch payload
            int first = input.IndexOf("ReportData") - 3;
            int last = input.LastIndexOf("}';");
            payload = input.Substring(first, last - first);
            return payload;
        }
      
    }
}
