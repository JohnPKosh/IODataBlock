using System;
using System.Reflection;

namespace Sandbox3.Areas.HelpPage.ModelDescriptions
{
    /// <summary>
    /// IModelDocumentationProvider Interface used by the ModelDescriptionGenerator class.
    /// </summary>
    public interface IModelDocumentationProvider
    {
        /// <summary>
        /// Gets the documentation by MemberInfo.
        /// </summary>
        /// <param name="member">The MemberInfo member.</param>
        /// <returns></returns>
        string GetDocumentation(MemberInfo member);

        /// <summary>
        /// Gets the documentation by Type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        string GetDocumentation(Type type);
    }
}