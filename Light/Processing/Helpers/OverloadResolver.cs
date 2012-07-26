using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;
using Light.Ast;
using Light.Ast.References;
using Light.Ast.References.Methods;
using Light.Ast.References.Types;

namespace Light.Processing.Helpers {
    public class OverloadResolver {
        public IAstMethodReference ResolveMethodGroup(AstMethodGroup group, IAstElement target, IList<IAstExpression> arguments) {
            var candidates = GetCandidates(group, target, arguments)
                                .GroupBy(c => c.Item2)
                                .OrderBy(g => g.Key)
                                .First()
                                .ToArray();

            if (candidates.Length > 1)
                throw new NotImplementedException(string.Format("OverloadResolver: Ambiguous best match found for {0}: {1}.", @group.Name, string.Join(", ", candidates.AsEnumerable())));

            if (candidates.Length == 0)
                throw new NotImplementedException("OverloadResolver: No match found for " + @group.Name);

            return candidates[0].Item1;
        }

        private IEnumerable<Tuple<IAstMethodReference, int>> GetCandidates(AstMethodGroup @group, IAstElement target, IList<IAstExpression> arguments) {
            // little bit of cheating for now

            foreach (var method in @group.Methods) {
                var reflected = method as AstReflectedMethod;
                if (reflected == null)
                    throw new NotImplementedException("OverloadResolver: Reference type " + method.GetType() + " is not yet supported.");

                var parameterTypes = reflected.Method.GetParameters().Select(p => p.ParameterType);
                var argumentTypes = arguments.Select(a => ((AstReflectedType)a.ExpressionType).ActualType);
                if (method.Location == MethodLocation.Extension)
                    argumentTypes = new[] {((AstReflectedType)((IAstExpression)target).ExpressionType).ActualType}.Concat(argumentTypes);

                var match = GetOrMakeMatch(reflected, parameterTypes, argumentTypes);
                if (match != null)
                    yield return match;
            }
        }

        private Tuple<IAstMethodReference, int> GetOrMakeMatch(AstReflectedMethod reflected, IEnumerable<Type> parameterTypes, IEnumerable<Type> argumentTypes) {
            var genericTheories = new Dictionary<Type, IList<Type>>();
            var distance = GetDistanceForAll(reflected, parameterTypes, argumentTypes, genericTheories);
            if (distance == -1)
                return null;

            if (reflected.Method.IsGenericMethodDefinition) {
                var genericParameters = reflected.Method.GetGenericArguments();
                var genericArguments = new Type[genericParameters.Length];
                for (var i = 0; i < genericParameters.Length; i++) {
                    var theories = genericTheories.GetValueOrDefault(genericParameters[i]);
                    if (theories == null)
                        return null;

                    var aggregate = ReconcileGenericTheories(theories);
                    if (aggregate == null)
                        return null;

                    genericArguments[i] = aggregate;
                }

                var generic = reflected.Method.MakeGenericMethod(genericArguments);
                return Tuple.Create((IAstMethodReference)new AstReflectedMethod(generic), distance);
            }

            return Tuple.Create((IAstMethodReference)reflected, distance);
        }

        private int GetDistanceForAll(AstReflectedMethod reflected, IEnumerable<Type> parameterTypes, IEnumerable<Type> argumentTypes, Dictionary<Type, IList<Type>> genericTheories) {
            var totalDistance = 0;

            using (var parameterTypeEnumerator = parameterTypes.GetEnumerator())
            using (var argumentTypeEnumerator = argumentTypes.GetEnumerator()) {
                while (parameterTypeEnumerator.MoveNext()) {
                    if (!argumentTypeEnumerator.MoveNext())
                        return -1;

                    var distance = GetDistance(parameterTypeEnumerator.Current, argumentTypeEnumerator.Current, reflected, genericTheories);
                    if (distance == -1)
                        return -1;

                    totalDistance += distance;
                }

                if (argumentTypeEnumerator.MoveNext())
                    return -1;
            }

            return totalDistance;
        }

        private int GetDistance(Type parameterType, Type argumentType, AstReflectedMethod reflected, IDictionary<Type, IList<Type>> genericTheories) {
            var argumentCompatibleTypes = GetCompatibleTypes(argumentType);
            var distance = argumentCompatibleTypes.GetValueOrDefault(parameterType, -1);
            if (distance != -1)
                return distance;

            if (!reflected.Method.IsGenericMethodDefinition)
                return -1;

            if (CanMatchPotentialGeneric(parameterType, argumentType, argumentCompatibleTypes, genericTheories))
                return 0;

            return -1;
        }

        private IDictionary<Type, int> GetCompatibleTypes(Type type) {
            var dictionary = new Dictionary<Type, int> {{type, 0}};

            var baseDistance = 1;
            var @base = type.BaseType;
            dictionary.Add(@base, baseDistance);
            while (@base != typeof(object)) {
                @base = @base.BaseType;
                baseDistance += 1;
                dictionary.Add(@base, baseDistance);
            }

            foreach (var @interface in type.GetInterfaces()) {
                dictionary.Add(@interface, 1);
            }

            return dictionary;
        }

        private bool CanMatchPotentialGeneric(Type parameterType, Type argumentType, IDictionary<Type, int> argumentCompatibleTypes, IDictionary<Type, IList<Type>> genericTheories) {
            if (parameterType.IsGenericParameter) {
                AddGenericTheory(genericTheories, parameterType, argumentType);
                return true;
            }

            if (!parameterType.IsGenericType)
                return false;

            var parameterTypeDefinition = parameterType.GetGenericTypeDefinition();
            var parameterTypeArguments = parameterType.GetGenericArguments();

            foreach (var type in argumentCompatibleTypes.Keys) {
                if (!type.IsGenericType)
                    continue;

                var definition = type.GetGenericTypeDefinition();
                if (definition != parameterTypeDefinition)
                    continue;

                var typeArguments = type.GetGenericArguments();
                for (var i = 0; i < typeArguments.Length; i++) {
                    if (typeArguments[i] == parameterTypeArguments[i])
                        continue;

                    if (!CanMatchPotentialGeneric(parameterTypeArguments[i], typeArguments[i], GetCompatibleTypes(typeArguments[i]), genericTheories))
                        return false;
                }
            }

            return true;
        }

        private void AddGenericTheory(IDictionary<Type, IList<Type>> genericTheories, Type genericType, Type actualType) {
            var list = genericTheories.GetValueOrDefault(genericType);
            if (list == null) {
                genericTheories.Add(genericType, new List<Type> {actualType});
                return;
            }

            list.Add(actualType);
        }

        private Type ReconcileGenericTheories(IList<Type> theories) {
            if (theories.Count > 1)
                throw new NotImplementedException("OverloadResolver: Generic theory aggregation is not yet supported (theories:" + string.Join(", ", theories) + ")");

            return theories[0];
        }
    }
}
