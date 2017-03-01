using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridWithEFCore.Database
{
    public class Session
    {
        public int ID { get; set; }
        public Category Category { get; set; }
        public string Title { get; set; }
        public string Speaker { get; set; }
        public string Abtract { get; set; }
        public DateTime Start { get; set; }
        public int MinutesDuration { get; set; }
    }
}