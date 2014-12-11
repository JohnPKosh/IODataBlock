using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Business.Common.Validation
{
    public abstract class ValidationObjectBase
    {

        /* http://msdn.microsoft.com/en-us/library/system.componentmodel.dataannotations.validator(v=vs.110).aspx */

        /*
        Sample Usage:
        if (!obj.TryValidate(errs))
            {
                foreach (var v in errs)
                {
                    MessageBox.Show(v.ErrorMessage);
                }
            }
        */

        public virtual bool TryValidate(ICollection<ValidationResult> validationResults, bool validateAllProperties = true)
        {
            return Validator.TryValidateObject(this, new ValidationContext(this), validationResults, validateAllProperties);
        }

        /*
        Sample Usage:
        foreach (var msg in obj.GetValidationMessages())
        {
            MessageBox.Show(msg);
        }
        */

        public virtual IEnumerable<string> GetValidationMessages()
        {
            var errs = new List<ValidationResult>();
            var rv = new List<string>();
            if (!TryValidate(errs))
            {
                rv.AddRange(errs.Select(x => x.ErrorMessage));
            }
            return rv;
        }

    }
}
