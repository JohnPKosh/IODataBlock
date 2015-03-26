using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Common.Configuration
{
    public interface IConfigLoader
    {
        dynamic LoadConfiguration();
        bool TryLoadConfiguration(out dynamic config);

        void SaveConfiguration(dynamic config);
        bool TrySaveConfiguration(dynamic config);
    }
}
