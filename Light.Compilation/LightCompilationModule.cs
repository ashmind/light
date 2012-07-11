using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Light.Compilation.Cil;
using Light.Compilation.Types;

namespace Light.Compilation {
    public class LightCompilationModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterAssemblyTypes(ThisAssembly).As<ICilCompiler>();
            builder.RegisterAssemblyTypes(ThisAssembly).As<ITypeResolver>();

            builder.RegisterType<LightCompiler>().AsSelf();
        }
    }
}
