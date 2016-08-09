using System.Collections.ObjectModel;

namespace Sandbox3.Areas.HelpPage.ModelDescriptions
{
    /// <summary>
    /// EnumTypeModelDescription class, Inherits from ModelDescription.
    /// </summary>
    public class EnumTypeModelDescription : ModelDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumTypeModelDescription"/> class.
        /// </summary>
        public EnumTypeModelDescription()
        {
            Values = new Collection<EnumValueDescription>();
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>
        /// The EnumValueDescription values.
        /// </value>
        public Collection<EnumValueDescription> Values { get; private set; }
    }
}