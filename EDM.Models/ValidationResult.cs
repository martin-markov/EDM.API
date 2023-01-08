using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDM.Models
{
    public class ValidationResult
    {
        public ValidationResult()
        {
            this.Errors = new List<string>();
        }

        public ICollection<string> Errors;
        public bool IsValid
        {
            get
            {
                return !Errors.Any();
            }
        }
    }
}
