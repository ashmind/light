using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AshMind.Extensions;

namespace Light.Processing {
    public class ProcessingOptions {
        private static readonly ISet<ProcessingStage> AllStages = Enum.GetValues(typeof(ProcessingStage)).Cast<ProcessingStage>().ToSet();
        private static readonly ISet<Assembly> DefaultAssemblies = new HashSet<Assembly> { typeof(object).Assembly, typeof(Enumerable).Assembly };

        public ISet<ProcessingStage> Stages { get; private set; }
        public ISet<Assembly> ReferencedAssemblies { get; private set; }

        public ProcessingOptions(params ProcessingStage[] stages) : this((IEnumerable<ProcessingStage>)stages) {
        }

        public ProcessingOptions(IEnumerable<ProcessingStage> stages) {
            // in both cases we call ToSet on something that is already Set to make sure we got a copy
            this.Stages = stages.ToSet();
            if (!this.Stages.Any())
                this.Stages = AllStages.ToSet();
            
            this.ReferencedAssemblies = DefaultAssemblies.ToSet();
        }
    }
}