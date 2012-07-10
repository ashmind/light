using System;

namespace Light.Compilation {
    public class CompilationArguments {
        public string AssemblyName      { get; set; }
        public Version AssemblyVersion  { get; set; }
        public CompilationTarget Target { get; set; }
    }
}