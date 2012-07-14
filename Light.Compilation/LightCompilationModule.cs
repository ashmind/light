using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Light.Compilation.Cil;
using Light.Compilation.Definitions;
using Light.Compilation.References;

namespace Light.Compilation {
    public class LightCompilationModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterAssemblyTypes(ThisAssembly).As<IReferenceProvider>();

            builder.RegisterAssemblyTypes(ThisAssembly).As<IDefinitionBuilder>();
            builder.RegisterAssemblyTypes(ThisAssembly).As<ICilCompiler>();
            
            builder.RegisterType<LightCompiler>().AsSelf();
        }
    }
}
