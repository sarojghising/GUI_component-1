using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPL_Component1.Validations
{
    class ShapeValidator
    {
        private string baseValue ;

        public Boolean checkString( string haystack)
        {
            return this.baseValue.ToUpper().Equals(haystack);
        }

        public Boolean validate(string name)
        {
            this.baseValue = name;
            if (checkString("CIRCLE") || checkString("RECTANGLE") || checkString("SQUARE") || checkString("TRIANGLE"))
            {
                return true;
            }
            return false;
        }
    }
}
