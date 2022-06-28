using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIM_API_LINKS
{
    public class Project
    {
        public int ProjectID
        {
            get;
            set;
        }
        public string ProjectName
        {
            get;
            set;
        }
        public string ProjectNote
        {
            get;
            set;
        }
        public int ModelTypeID
        {
            get;
            set;
        }
        public string ModelType
        {
            get;
            set;
        }
        public string DateCreated
        {
            get;
            set;
        }
        public string LastModifiedDate
        {
            get;
            set;
        }
    }
}
