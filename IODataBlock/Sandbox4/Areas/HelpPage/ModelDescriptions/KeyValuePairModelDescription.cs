namespace Sandbox4.Areas.HelpPage.ModelDescriptions
{
    /// <summary>
    /// KeyValuePairModelDescription class, Inherits from ModelDescription.
    /// </summary>
    public class KeyValuePairModelDescription : ModelDescription
    {
        /// <summary>
        /// Gets or sets the key model description.
        /// </summary>
        /// <value>
        /// The key ModelDescription.
        /// </value>
        public ModelDescription KeyModelDescription { get; set; }

        /// <summary>
        /// Gets or sets the value model description.
        /// </summary>
        /// <value>
        /// The value ModelDescription.
        /// </value>
        public ModelDescription ValueModelDescription { get; set; }
    }
}