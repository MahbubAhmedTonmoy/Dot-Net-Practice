using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeliseExam.pagging
{
    public class PaggingParms
    {
        private const int MaxPageSize = 100;
        public int PageNumber { get; set; } = 1;

        private int pageSize = 10;
        public int PageSize 
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

        //filter
        public string Gender { get; set; }
        public int MinAge { get; set; } = 70;
        public int MaxAge { get; set; } = 200;

        //sorting
        public int OrderBy { get; set; } = 1;
    }
}
