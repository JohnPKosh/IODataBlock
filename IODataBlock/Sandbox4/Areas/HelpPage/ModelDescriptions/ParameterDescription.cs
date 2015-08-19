using System.Collections.ObjectModel;

namespace Sandbox4.Areas.HelpPage.ModelDescriptions
{
    /// <summary>
    /// ParameterDescription class.
    /// </summary>
    public class ParameterDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterDescription"/> class.
        /// </summary>
        public ParameterDescription()
        {
            Annotations = new Collection<ParameterAnnotation>();
        }

        /// <summary>
        /// Gets the annotations.
        /// </summary>
        /// <value>
        /// The annotations.
        /// </value>
        public Collection<ParameterAnnotation> Annotations { get; private set; }

        /// <summary>
        /// Gets or sets the documentation.
        /// </summary>
        /// <value>
        /// The string documentation.
        /// </value>
        public string Documentation { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The string name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type description.
        /// </summary>
        /// <value>
        /// The type description.
        /// </value>
        public ModelDescription TypeDescription { get; set; }
    }
}