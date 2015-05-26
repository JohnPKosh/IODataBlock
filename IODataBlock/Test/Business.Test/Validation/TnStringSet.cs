using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Business.Common.Validation;

namespace Business.Test.Validation
{
    public class TnStringList : ValidationObjectBase
    {
        #region Class Initialization

        public TnStringList(){}

        public TnStringList(IEnumerable<TnString> tns)
        {
            Tns = new List<TnString>(tns);
        }

        #region Factory Methods

        public static TnStringList Create(IEnumerable<string> tns)
        {
            return tns.ToList();
        }

        #endregion Factory Methods

        #endregion Class Initialization

        #region Fields and Properties

        [Required]
        public List<TnString> Tns { get; set; }

        [Required]
        public bool? Required { get; set; }

        //public Dictionary<String, ICollection<ValidationResult>> TnValidationResults { get; set; }

        #endregion Fields and Properties

        #region Conversion Operators

        static public implicit operator List<string>(TnStringList tns)
        {
            return tns.Tns.Select(x => x.Value).ToList();
        }

        static public implicit operator TnStringList(List<string> tns)
        {
            return new TnStringList(tns.Select(x => (TnString)x));
        }

        #endregion Conversion Operators

        #region Base Class Overridden Methods

        //public override bool TryValidate(ICollection<ValidationResult> validationResults, bool validateAllProperties = true)
        //{
        //    // TODO: Find best way to inject custom validation below.  Perhaps method overload of this method with additional function param.
        //    // If I meet my custom validations then TryValidate with base.
        //    if (Tns != null && Tns.Any())
        //    {
        //        return base.TryValidate(validationResults, validateAllProperties);
        //    }
        //    // Else add my error and check for base ValidationResults an return false.
        //    validationResults.Add(new ValidationResult("Empty collections are not allowed!", new[] { "Tns" }));
        //    base.TryValidate(validationResults, validateAllProperties);
        //    return false;
        //}

        public override bool TryValidate(ICollection<ValidationResult> validationResults, bool validateAllProperties = true)
        {
            // TODO: Find best way to inject custom validation below.  Perhaps method overload of this method with additional function param.
            // If I meet my custom validations then TryValidate with base.
            if (Tns != null && Tns.Any())
            {
                var tnsvalid = true;
                foreach (var tn in Tns.Where(tn => !tn.TryValidate(validationResults, validateAllProperties)))
                {
                    tnsvalid = false;
                }
                if (tnsvalid)
                {
                    return base.TryValidate(validationResults, validateAllProperties);
                }
                base.TryValidate(validationResults, validateAllProperties);
                return false;
            }
            // Else add my error and check for base ValidationResults an return false.
            validationResults.Add(new ValidationResult("Empty collections are not allowed!", new[] { "Tns" }));
            base.TryValidate(validationResults, validateAllProperties);
            return false;
        }

        public override IEnumerable<string> GetValidationMessages()
        {
            var errs = new List<ValidationResult>();
            var rv = new List<string>();
            if (!TryValidate(errs))
            {
                rv.AddRange(errs.Select(x => x.ErrorMessage));
            }
            return rv;
        }



        //public override string ToString()
        //{
        //    return Value;
        //}

        #endregion Base Class Overridden Methods
    }
}