using System;
using System.Collections.Generic;

namespace Data.MsAccess
{
    public class SelectIntoAccessFromSqlCommand
    {
        public String InsertIntoTemplate { get; set; }

        public String AccessTableName { get; set; }

        public String SqlServer { get; set; }

        public String DatabaseName { get; set; }

        public String SqlUserName { get; set; }

        public String SqlPassword { get; set; }

        public String Password { get; set; }

        public Boolean OverwriteTable { get; set; }

        public Boolean OverwriteView { get; set; }

        public Dictionary<String, String> NamedArgs { get; set; }

        public List<Object> NumberedArgs { get; set; }

        public Int32 LockWaitMs { get; set; }
    }
}