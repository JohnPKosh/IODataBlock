using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Business.Common.Validation
{
    public interface IValidationObjectBase
    {
        bool TryValidate(ICollection<ValidationResult> validationResults, bool validateAllProperties = true);

        //bool TryValidate(ICollection<ValidationResult> validationResults, bool validateAllProperties = true, IEnumerable<Func<IValidationObjectBase, ValidationResult>> validationFunctions = null);

        IEnumerable<string> GetValidationMessages();

        //IEnumerable<string> GetValidationMessages(bool validateAllProperties);

        //IEnumerable<string> GetValidationMessages(bool validateAllProperties = true, IEnumerable<Func<IValidationObjectBase, ValidationResult>> validationFunctions = null);
    }
}