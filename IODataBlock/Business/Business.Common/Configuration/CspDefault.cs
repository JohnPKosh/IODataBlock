using System.IO;
using Business.Common.Extensions;

namespace Business.Common.Configuration
{
    internal class CspDefault
    {
        private const string Location = @"csp.dat";

        public static bool TryLoad(out CspDefault value, bool deleteOnLoad = true)
        {
            var fi = new FileInfo(Location);
            if (fi.Directory != null && (fi.Directory.Exists && fi.Exists))
            {
                try
                {
                    value = fi.BsonDeserialize<CspDefault>();
                    if(deleteOnLoad)fi.Delete();
                    return true;
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch{}
            }
            value = null;
            return false;
        }

        public static bool TryCreate(string napK = null, string naK = null, string napI = null, string naI = null)
        {
            //if (String.IsNullOrWhiteSpace(napK) || String.IsNullOrWhiteSpace(naK)) return false;
            var fi = new FileInfo(Location);
            if (fi.Directory == null || !fi.Directory.Exists) return false;
            try
            {
                var d = new CspDefault()
                {
                    NapK = napK,
                    NaK = naK,
                    NapI = napI,
                    NaI = naI
                };
                d.WriteBsonToFile(fi);
                return true;
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch { }
            return false;
        }

        public string NapK { get; set; }
        public string NaK { get; set; }
        public string NapI { get; set; }
        public string NaI { get; set; }
    }
}
