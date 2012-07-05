using System;
using System.IO;

namespace Light.Tests {
    public static class Resource {
        public static string ReadAllText(Type type, string resourceName) {
            using (var stream = type.Assembly.GetManifestResourceStream(type.Namespace + "." + resourceName))
            using (var streamReader = new StreamReader(stream)) {
                return streamReader.ReadToEnd();
            }
        }
    }
}