using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Light.Parsing;
using Light.Processing;
using Light.Processing.Scoping;

namespace Light {
    public class LightMainModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterAssemblyTypes(ThisAssembly).As<IProcessingStep>();
            builder.RegisterType<LightGrammar>().AsSelf();
            builder.RegisterType<LightParser>().AsSelf();
            builder.RegisterType<BuiltInTypesNameSource>().As<INameSource>();
            builder.RegisterType<LightProcessor>().AsSelf();
        }
    }
}
