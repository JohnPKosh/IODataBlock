using System.Collections.ObjectModel;

namespace Sandbox3.Areas.HelpPage.ModelDescriptions
{
    /// <summary>
    /// ComplexTypeModelDescription class, Inherits from ModelDescription.
    /// </summary>
    public class ComplexTypeModelDescription : ModelDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexTypeModelDescription"/> class.
        /// </summary>
        public ComplexTypeModelDescription()
        {
            Properties = new Collection<ParameterDescription>();
        }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>
        /// The ParameterDescription Collection properties.
        /// </value>
        public Collection<ParameterDescription> Properties { get; private set; }
    }
}