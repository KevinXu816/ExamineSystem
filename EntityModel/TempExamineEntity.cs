using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataFrameworkLibrary.Core;

namespace EntityModel
{

    public class TempExamineEntity : Entity
    {
        public int QuestionId
        {
            get;
            set;
        }

        public string QuestionAnswer
        {
            get;
            set;
        }

        public string UserAnswer
        {
            get;
            set;
        }
    }
}