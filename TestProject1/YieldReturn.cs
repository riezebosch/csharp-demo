using System;
using System.Collections;
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
            return new MyEnumerable();
        }
    }

    internal class MyEnumerable : IEnumerable<int>
    {
        public IEnumerator<int> GetEnumerator() => 
            new MyEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => 
            GetEnumerator();
    }

    internal class MyEnumerator : IEnumerator<int>
    {
        public bool MoveNext()
        {
            switch (Current)
            {
                case -1:
                    Current = 0;
                    return true;
                case 0:
                    Current = 1;
                    return true;
                default:
                    Current = -1;
                    return false;
            }
        }

        public void Reset() =>
            Current = -1;

        public int Current { get; private set; } = -1;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }
    }
}