using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Light.Processing;

namespace Light {
    public class LightMainModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterAssemblyTypes(ThisAssembly).As<IProcessingStep>();
            builder.RegisterType<LightProcessor>().AsSelf();
        }
    }
}
