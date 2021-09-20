using System.Linq;
using FluentAssertions;
using Xunit;

namespace TestProject1
{
    public class Params
    {
        [Fact]
        public void Test1() => 
            Sum(1, 2)
                .Should()
                .Be(3);

        [Fact]
        public void TestMultipleInputs() => 
            Sum(1, 2, 3)
                .Should()
                .Be(6);
        
        [Fact]
        public void TestCombineWithOtherInputs()
        {
            int[] remainder = { 1, 2, 3, 4 };
            Sum(remainder.Concat(new[] { 1, 2, 3}).ToArray()) // Not so fancy in C#
                .Should()
                .Be(6);
        }

        [Fact]
        public void TestUsingArray() => 
            Sum(new[] { 1, 2, 3 })
                .Should()
                .Be(6);
        
        [Fact]
        public void Empty() => 
            Sum()
                .Should()
                .Be(0);
        
        [Fact]
        public void Overload() => 
            Sum(2)
                .Should()
                .Be(1234);

        private static int Sum(int one) => 
            1234;

        private static int Sum(params int[] inputs)
        {
            var sum = 0;
            foreach (var item in inputs)
            {
                sum += item;
            }

            return sum;
        }
    }
}