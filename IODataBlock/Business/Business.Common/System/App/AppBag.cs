using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Common.System.App
{
    public class AppBag
    {
        private static AppBag _data = new AppBag();

        private AppBag()
        {
            Value = null;
        }

        public static AppBag Data
        {
            get { return _data ?? (_data = new AppBag()); }
        }

        public dynamic Value { get; set; }
    }
}
