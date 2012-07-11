using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Light.Compilation;

namespace Light.Tests {
    public static class TestEnvironment {
        public static IContainer Container { get; private set; }

        static TestEnvironment() {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(typeof(LightCompiler).Assembly);

            Container = builder.Build();
        }
    }
}
