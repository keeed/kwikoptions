using System.Collections.Generic;

namespace KwikOptions.Options
{
    public class RootKwikOptions
    {
        public bool LoadExternalAssemblies { get; set; }
        public List<OptionsType> OptionsTypes { get; set; }
        public List<OptionsProviderOption> OptionsProviders { get; set; }

        public RootKwikOptions()
        {
            LoadExternalAssemblies = true;
        }
    }
}