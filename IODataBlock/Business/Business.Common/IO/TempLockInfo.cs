using System;
using System.Runtime.Serialization;

namespace Business.Common.IO
{
    [DataContract(Namespace = "http://www.ExtensionBase.com/TempLockInfo/")]
    public class TempLockInfo
    {
        [DataMember]
        public DateTime ExpirationDate { get; set; }
    }
}