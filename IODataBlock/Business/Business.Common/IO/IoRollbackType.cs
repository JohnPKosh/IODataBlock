using System.Runtime.Serialization;

namespace Business.Common.IO
{
    public enum IoRollbackType
    {
        [EnumMember]
        None,

        [EnumMember]
        InMemory,

        [EnumMember]
        FileCopy
    }
}