using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Business.Common.Validation;

namespace Business.Test.Validation
{
    public class TnString : ValidationObjectBase
    {
        #region Class Initialization

        public TnString(string tn)
        {
            Value = tn;
        }

        #endregion Class Initialization

        #region Fields and Properties

        [Required]
        [RegularExpression("[0-9]{10}", ErrorMessage = "Field must be exactly 10 characters long and digits only!")]
        public string Value { get; set; }

        #endregion Fields and Properties

        #region Conversion Operators

        static public implicit operator string(TnString tn)
        {
            return tn.Value;
        }

        static public implicit operator TnString(string value)
        {
            return new TnString(value);
        }

        #endregion Conversion Operators

        #region Base Class Overridden Methods

        public override bool TryValidate(ICollection<ValidationResult> validationResults, bool validateAllProperties = true)
        {
            // TODO: Find best way to inject custom validation below.  Perhaps method overload of this method with additional function param.
            // If I meet my custom validations then TryValidate with base.
            if (Value == "2165132288")
            {
                return base.TryValidate(validationResults, validateAllProperties);
            }
            // Else add my error and check for base ValidationResults an return false.
            validationResults.Add(new ValidationResult("You did something wrong! Just Kidding!!!!!", new[] { "Value" }));
            base.TryValidate(validationResults, validateAllProperties);
            return false;
        }

        //public override IEnumerable<string> GetValidationMessages()
        //{
        //    var errs = new List<ValidationResult>();
        //    var rv = new List<string>();
        //    if (!TryValidate(errs))
        //    {
        //        rv.AddRange(errs.Select(x => x.ErrorMessage));
        //    }
        //    return rv;
        //}

        public override string ToString()
        {
            return Value;
        }

        #endregion Base Class Overridden Methods
    }
}

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

/*
    Sample Usage:
    foreach (var msg in obj.GetValidationMessages())
    {
        MessageBox.Show(msg);
    }
*/