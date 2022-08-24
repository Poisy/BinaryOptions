using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class BetPreview
    {
        public string Currency { get; set; }
        
        public List<Option> Options { get; set; }

        public class Option
        {
            public DateTime ExpirationDate { get; set; }

            public List<OptionInfo> OptionInfos { get; set; }

            public class OptionInfo
            {
                public double Barrier { get; set; }

                public PercentageReward PercentageRewards { get; set; }

                public class PercentageReward
                {
                    public double Higher { get; set; }
                    
                    public double Lower { get; set; }
                }
            }
        }
    }
}