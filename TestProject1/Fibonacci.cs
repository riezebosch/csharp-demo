using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace TestProject1
{
    public class FibonacciTests
    {
        [Fact]
        public void First() =>
            Fibonacci()
                .Take(8)
                .Should()
                .BeEquivalentTo(new[] { 0, 1, 1, 2, 3, 5, 8, 13 });

        private static IEnumerable<int> Fibonacci()
        {
            int a, b;
            yield return a = 0;
            yield return b = 1;

            while (true)
            {
                int c;
                yield return c = a + b;
                a = b;
                b = c;
            }
        }
    }
}