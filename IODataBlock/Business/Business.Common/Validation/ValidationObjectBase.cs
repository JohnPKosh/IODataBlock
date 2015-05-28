using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Business.Common.Validation
{
    public abstract class ValidationObjectBase : IValidationObjectBase
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

        public virtual bool TryValidate(ICollection<ValidationResult> validationResults, IEnumerable<Func<object, ValidationResult>> validationFunctions)
        {
            var hasErr = false;
            foreach (var function in validationFunctions)
            {
                var validatorResult = function.Invoke(this);
                if (validatorResult == null) continue;
                hasErr = true;
                validationResults.Add(validatorResult);
            }
            return !hasErr && Validator.TryValidateObject(this, new ValidationContext(this), validationResults, true);
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
            if (!TryValidate(errs, true))
            {
                rv.AddRange(errs.Select(x => x.ErrorMessage));
            }
            return rv;
        }

        //public virtual IEnumerable<string> GetValidationMessages(bool validateAllProperties)
        //{
        //    var errs = new List<ValidationResult>();
        //    var rv = new List<string>();
        //    if (!TryValidate(errs, validateAllProperties))
        //    {
        //        rv.AddRange(errs.Select(x => x.ErrorMessage));
        //    }
        //    return rv;
        //}
    }
}