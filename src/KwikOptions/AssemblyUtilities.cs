using System;
using System.Linq;
using System.Reflection;

namespace KwikOptions
{
    public static class AssemblyUtilities
    {
        public static void LoadAssemblyIfNotLoaded(string assemblyName)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var assembly = assemblies.FirstOrDefault(
                    a => a.GetName().Name == assemblyName.Remove(assemblyName.Length - 4));

            if (assembly == null)
            {
                Assembly.LoadFrom(AppDomain.CurrentDomain.BaseDirectory + assemblyName);
            }
        }
    }
}