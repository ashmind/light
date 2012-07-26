using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Light.BuiltIn;
using Light.Description;
using Light.Parsing;
using Light.Processing;
using Light.Processing.Complex;
using Light.Processing.Scoping;

namespace Light {
    public class LightMainModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterType<LightGrammar>().AsSelf();
            builder.RegisterType<LightParser>().AsSelf();

            builder.RegisterType<BuiltInTypeMap>().AsSelf();

            builder.RegisterAssemblyTypes(ThisAssembly).As<IProcessingStep>();
            builder.RegisterType<OverloadResolver>().AsSelf();
            builder.RegisterType<BuiltInTypesNameSource>().As<INameSource>();
            builder.RegisterType<LightProcessor>().AsSelf();

            builder.RegisterType<TypeFormatter>().AsSelf();
        }
    }
}
