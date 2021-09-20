using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace TestProject1
{
    public class YieldReturn
    {
        private readonly ITestOutputHelper _output;

        public YieldReturn(ITestOutputHelper output) => 
            _output = output;

        [Fact]
        public void First() =>
            Next()
                .First()
                .Should()
                .Be(0);
        
        [Fact]
        public void NextNext()
        {
            Next()
                .Skip(1)
                .First()
                .Should()
                .Be(1);
        }

        [Fact]
        public void NextNextNext()
        {
            Action act = () => Next()
                .Skip(2)
                .First()
                .Should()
                .Be(1);

            act
                .Should()
                .Throw<InvalidOperationException>();
        }
        
        [Fact]
        public void NextForeach()
        {
            foreach (var item in Next())
            {
                _output.WriteLine(item.ToString());
            }
        }

        private static IEnumerable<int> Next()
        {
            yield return 0;
            yield return 1;
        }
    }
}