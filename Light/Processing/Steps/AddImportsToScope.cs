﻿using System;
using System.Collections.Generic;
using System.Linq;
using Light.Ast;
using Light.Ast.Definitions;
using Light.Processing.Scoping;

namespace Light.Processing.Steps {
    public class AddImportsToScope : ProcessingStepBase<ImportDefinition> {
        public override IAstElement ProcessAfterChildren(ImportDefinition import, ProcessingContext context) {
            var @namespace = import.Namespace.ToString();
            if (!@namespace.StartsWith("System"))
                throw new NotImplementedException("AddImportsToScope: namespace " + @namespace + " is not supported.");

            context.Scope.Add(new ExternalNamespaceNameSource(typeof(object).Assembly, @namespace));
            return import;
        }
    }
}
