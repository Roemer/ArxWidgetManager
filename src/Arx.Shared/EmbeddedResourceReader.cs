using System.IO;
using System.Reflection;

namespace Arx.Shared
{
    public static class EmbeddedResourceReader
    {
        public static Stream GetStream(string resourceName, Assembly assembly = null)
        {
            if (assembly == null)
            {
                assembly = Assembly.GetEntryAssembly();
            }
            return assembly.GetManifestResourceStream(resourceName);
        }

        public static byte[] GetBytes(string resourceName, Assembly assembly = null)
        {
            using (var stream = GetStream(resourceName, assembly))
            {
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }
    }
}
