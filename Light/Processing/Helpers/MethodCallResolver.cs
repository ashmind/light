using System;
using System.Collections.Generic;
using System.Linq;
using AshMind.Extensions;
using Light.Ast;
using Light.Ast.References;
using Light.Ast.References.Methods;
using Light.Ast.References.Types;

namespace Light.Processing.Helpers {
    public class MethodCallResolver {
        private readonly GenericTypeHelper genericHelper;

        #region DeconstructingAdapter Class

        private class DeconstructingAdapter {
            private readonly Func<IAstTypeReference, bool> matches;
            public IList<IAstTypeReference> TypeArguments { get; private set; }

            public DeconstructingAdapter(Func<IAstTypeReference, bool> matches, IList<IAstTypeReference> arguments) {
                this.TypeArguments = arguments;
                this.matches = matches;
            }

            public bool Matches(IAstTypeReference other) {
                return this.matches(other);
            }
        }

        #endregion

        public MethodCallResolver(GenericTypeHelper genericHelper) {
            this.genericHelper = genericHelper;
        }

        public IAstMethodReference Resolve(IList<IAstMethodReference> methods, IAstElement target, IList<IAstExpression> arguments) {
            var candidates = GetCandidates(methods, target, arguments)
                                    .GroupBy(c => c.Item2)
                                    .OrderBy(g => g.Key)
                                    .FirstOrDefault();

            if (candidates == null)
                throw new NotImplementedException("MethodCallResolver: Could not adapt " + methods[0].Name + " to this call.");

            var candidatesAsArray = candidates.ToArray();
            if (candidatesAsArray.Length > 1)
                throw new NotImplementedException(string.Format("MethodCallResolver: Ambiguous best match found for {0}: {1}.", methods[0].Name, string.Join(", ", candidates.AsEnumerable())));

            return candidatesAsArray[0].Item1;
        }

        private IEnumerable<Tuple<IAstMethodReference, int>> GetCandidates(IEnumerable<IAstMethodReference> methods, IAstElement target, IList<IAstExpression> arguments) {
            foreach (var method in methods) {
                var parameterTypes = method.ParameterTypes;
                var argumentTypes = arguments.Select(a => a.ExpressionType);
                if (method.Location == MethodLocation.Extension)
                    argumentTypes = new[] { ((IAstExpression)target).ExpressionType }.Concat(argumentTypes);

                var match = GetOrMakeMatch(method, parameterTypes, argumentTypes);
                if (match != null)
                    yield return match;
            }
        }

        private Tuple<IAstMethodReference, int> GetOrMakeMatch(IAstMethodReference method, IEnumerable<IAstTypeReference> parameterTypes, IEnumerable<IAstTypeReference> argumentTypes) {
            var genericTheories = new Dictionary<IAstTypeReference, ISet<IAstTypeReference>>();
            var distance = GetDistanceForAll(method, parameterTypes, argumentTypes, genericTheories);
            if (distance == -1)
                return null;

            if (method.IsGeneric) {
                var genericParameters = method.GetGenericParameterTypes().ToArray();
                var genericArguments = new IAstTypeReference[genericParameters.Length];
                for (var i = 0; i < genericParameters.Length; i++) {
                    var theories = genericTheories.GetValueOrDefault(genericParameters[i]);
                    if (theories == null)
                        return null;

                    var aggregate = ReconcileGenericTheories(theories);
                    if (aggregate == null)
                        return null;

                    genericArguments[i] = aggregate;
                }

                var generic = new AstGenericMethodWithTypeArguments(method, genericArguments, this.genericHelper);
                return Tuple.Create((IAstMethodReference)generic, distance);
            }

            return Tuple.Create(method, distance);
        }

        private int GetDistanceForAll(IAstMethodReference reflected, IEnumerable<IAstTypeReference> parameterTypes, IEnumerable<IAstTypeReference> argumentTypes, Dictionary<IAstTypeReference, ISet<IAstTypeReference>> genericTheories) {
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

        private int GetDistance(IAstTypeReference parameterType, IAstTypeReference argumentType, IAstMethodReference method, Dictionary<IAstTypeReference, ISet<IAstTypeReference>> genericTheories) {
            var argumentCompatibleTypes = GetCompatibleTypes(argumentType);
            var distance = argumentCompatibleTypes.GetValueOrDefault(parameterType, -1);
            if (distance != -1)
                return distance;

            if (!method.IsGeneric)
                return -1;

            if (CanMatchPotentialGeneric(parameterType, argumentType, () => argumentCompatibleTypes, genericTheories))
                return 0;

            return -1;
        }

        private IDictionary<IAstTypeReference, int> GetCompatibleTypes(IAstTypeReference type) {
            var dictionary = new Dictionary<IAstTypeReference, int> { { type, 0 } };
            var distance = 1;
            
            foreach (var ancestor in type.GetAncestors()) {
                dictionary.Add(ancestor, distance);
                distance += 1;
            }

            foreach (var @interface in type.GetInterfaces()) {
                dictionary.Add(@interface, 1);
            }

            return dictionary;
        }

        private bool CanMatchPotentialGeneric(
            IAstTypeReference parameterType,
            IAstTypeReference argumentType,
            Func<IDictionary<IAstTypeReference, int>> getArgumentCompatibleTypes,
            Dictionary<IAstTypeReference, ISet<IAstTypeReference>> genericTheories
        ) {
            if (parameterType is AstGenericPlaceholderType) {
                if (!(argumentType is AstGenericPlaceholderType))
                    AddGenericTheory(genericTheories, parameterType, argumentType);
                return true;
            }

            var parameterTypeDeconstructed = DeconstructIfHasTypeArguments(parameterType);
            if (parameterTypeDeconstructed == null)
                return false;

            var parameterTypeArguments = parameterTypeDeconstructed.TypeArguments;
            var anyMatchesFound = false;
            foreach (var type in getArgumentCompatibleTypes().Keys) {
                if (!parameterTypeDeconstructed.Matches(type))
                    continue;

                var typeArguments = DeconstructIfHasTypeArguments(type).TypeArguments;
                for (var i = 0; i < typeArguments.Count; i++) {
                    if (Equals(typeArguments[i], parameterTypeArguments[i]))
                        continue;

                    if (!CanMatchPotentialGeneric(parameterTypeArguments[i], typeArguments[i], () => GetCompatibleTypes(typeArguments[i]), genericTheories))
                        return false;

                    anyMatchesFound = true;
                }
            }

            return anyMatchesFound;
        }

        private DeconstructingAdapter DeconstructIfHasTypeArguments(IAstTypeReference type) {
            var generic = type as AstGenericTypeWithArguments;
            if (generic != null) {
                return new DeconstructingAdapter(other => {
                    var otherAsGeneric = other as AstGenericTypeWithArguments;
                    return otherAsGeneric != null && Equals(otherAsGeneric.PrimaryType, generic.PrimaryType);
                }, generic.TypeArguments);
            }

            var function = type as IAstFunctionTypeReference;
            if (function != null) {
                return new DeconstructingAdapter(
                    other => other is IAstFunctionTypeReference,
                    function.GetParameterTypes().Concat(new[] { function.ReturnType }).ToArray()
                );
            }

            return null;
        }

        private void AddGenericTheory(Dictionary<IAstTypeReference, ISet<IAstTypeReference>> genericTheories, IAstTypeReference genericType, IAstTypeReference actualType) {
            var set = genericTheories.GetValueOrDefault(genericType);
            if (set == null) {
                genericTheories.Add(genericType, new HashSet<IAstTypeReference> { actualType });
                return;
            }

            set.Add(actualType);
        }

        private IAstTypeReference ReconcileGenericTheories(ISet<IAstTypeReference> theories) {
            if (theories.Count > 1)
                throw new NotImplementedException("OverloadResolver: Generic theory aggregation is not yet supported (theories:" + string.Join(", ", theories) + ")");

            return theories.First();
        }
    }
}
