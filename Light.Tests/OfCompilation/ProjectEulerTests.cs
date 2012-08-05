using System;
using System.Collections.Generic;
using System.Linq;
using Light.Framework;
using Light.Tests.Helpers;
using MbUnit.Framework;

namespace Light.Tests.OfCompilation {
    [TestFixture]
    public class ProjectEulerTests {
        // Please note that I am not aiming for a best solution from math perspective
        // I will always select a simpler solution that is easy to understand, to make tests more readable

        [Test] // improve: either?
        public void Problem1_AddAllNaturalNumbersBelow1000_ThatAreMultiplesOf3Or5() {
            var code = "(1..1000).Where(x => (x mod 3 == 0) or (x mod 5 == 0)).Sum()";
            var result = CompilationHelper.CompileAndEvaluate(code);
            Assert.AreEqual(new Integer(233168), result);
        }

        // let psi = (1-5**0.5)/2
        // let phi = (1+5**0.5)/2
        // let fib n = round ((phi**(fromIntegral n) - psi** (fromIntegral n))/5**0.5)
        // sum (filter even (takeWhile (<4000000) (map fib [1..])))

        [Test]
        public void Problem2_FindSumOfEvenValuedFibonacciNumbers_Under4000000() {
            var code = @"
                import System
                import System.Linq

                public class Problem2
                    decimal sqrtOf5 = Math.Sqrt((5.0).ToDouble()).ToDecimal()
                    decimal ψ = (1.0-sqrtOf5)/2.0
                    decimal φ = (1.0+sqrtOf5)/2.0

                    function Fibonacci(integer n)
                        return Math.Round(((φ ** n.ToDecimal() - ψ ** n.ToDecimal()) / sqrtOf5).ToDouble()).ToInteger()
                    end

                    function Evaluate()
                        return (1..10000000).Select(Fibonacci).TakeWhile(x => x < 4000000).Where(x => x mod 2 == 0).Sum()
                    end
                end
            ".Trim();
            var instance = CompilationHelper.CompileAndGetInstance(code, "Problem2");
            Assert.AreEqual<object>(new Integer(4613732), instance.Evaluate());
        }
    }
}
