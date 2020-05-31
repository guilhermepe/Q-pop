using System.Collections.Generic;

namespace Q_pop
{

    public class SkillReport
    {
       public IList<SkillData> ReportData { get; set; }
    }

    public class SkillData
    {
        public string Skill { get; set; }
        public string ServiceLevel { get; set; }
        
    }
}