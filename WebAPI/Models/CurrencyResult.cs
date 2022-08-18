using System;
using System.Collections.Generic;

namespace WebAPI.Models
{
    public class CurrencyResult
    {
        public MetaProp Meta { get; set; }

        public ResponseProp Response { get; set; }

        public class ResponseProp
        {
            public DateTime Date { get; set; }

            public Dictionary<string, double> Rates { get; set; }
        }


        public class MetaProp
        {
            public int Code { get; set; }
        }
    }
}