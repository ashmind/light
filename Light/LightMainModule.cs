using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Light.BuiltIn;
using Light.Description;
using Light.Parsing;
using Light.Processing;
using Light.Processing.Helpers;
using Light.Processing.Scoping;

namespace Light {
    public class LightMainModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterType<LightGrammar>().AsSelf();
            builder.RegisterType<LightParser>().AsSelf();

            builder.RegisterType<BuiltInTypeMap>().AsSelf();

            RegisterProcessing(builder);

            builder.RegisterType<TypeFormatter>().AsSelf();
        }

        private void RegisterProcessing(ContainerBuilder builder) {
            builder.RegisterAssemblyTypes(ThisAssembly).As<IProcessingStep>();
            builder.RegisterType<OverloadResolver>().AsSelf();
            builder.RegisterType<DelegateTypeBuilder>().AsSelf();
            builder.RegisterType<BuiltInTypesNameSource>().As<INameSource>();
            builder.RegisterType<LightProcessor>().AsSelf();
        }
    }
}
