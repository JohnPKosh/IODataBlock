using System;

namespace Sandbox4.Areas.HelpPage.ModelDescriptions
{
    /// <summary>
    /// Describes a type model.
    /// </summary>
    public abstract class ModelDescription
    {
        /// <summary>
        /// Gets or sets the documentation.
        /// </summary>
        /// <value>
        /// The string documentation.
        /// </value>
        public string Documentation { get; set; }

        /// <summary>
        /// Gets or sets the type of the model.
        /// </summary>
        /// <value>
        /// The type of the model.
        /// </value>
        public Type ModelType { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The string name.
        /// </value>
        public string Name { get; set; }
    }
}