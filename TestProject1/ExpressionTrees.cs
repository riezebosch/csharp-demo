using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Unmockable;
using Xunit;
using Xunit.Abstractions;

namespace TestProject1
{
    public class ExpressionTrees
    {
        private readonly ITestOutputHelper _output;

        public ExpressionTrees(ITestOutputHelper output) => 
            _output = output;

        [Fact]
        public void YouHaveUsedItBefore()
        {
            var mock = new Mock<ISomeInterface>();
            mock
                .Setup(m => m.Fibonacci())
                .Returns(new[] { 0, 1, 1, 3, 5, 8 });

            mock
                .Object
                .Fibonacci()
                .Should()
                .BeEquivalentTo(new[] { 0, 1, 1, 3, 5, 8 });
        }

        delegate int MyDelegate(int a, int b);
        
        delegate TResult MyDelegate<T1, T2, TResult>(T1 a, T2 b) where T1: IComparable;
        
        [Fact]
        public void Delegates()
        {
            MyDelegate f = Sum;
            f.Invoke(2, 3)
                .Should()
                .Be(5);
            
            f(2, 3)
                .Should()
                .Be(5);

            DoSomething(f);

            MyDelegate<int, int, int> generics = Sum;
            generics(3, 7)
                .Should()
                .Be(10);
        }


        [Fact]
        public void FuncAndAction()
        {
            Func<int> f = () => 3;
            Action<int> a = x => { };
            Predicate<int> p = x => true;

            f().Should().Be(3);
            a(3);
        }

        private void DoSomething(MyDelegate myDelegate)
        {
            myDelegate(5, 6);
        }
        
        [Fact]
        public void ExpressionAndDelegates()
        {
            Func<int, int> f = i => i * 2;
            Expression<Func<int, int>> e = i => i * 2 / 2 * (1 + i) * int.Parse("3");
            
            _output.WriteLine("-- func:");
            _output.WriteLine(f.ToString());

            _output.WriteLine("-- expression:");
            _output.WriteLine(e.ToString());
            
            _output.WriteLine("-- compiled:");
            _output.WriteLine(e.Compile().ToString());

            e.Compile().Invoke(3).Should().Be(36);
        }

        [Fact]
        public void EntityFrameworkDemo()
        {
            var people = new[] { new Person() };
            people.Where(p => p.FirstName.StartsWith("M"));
            
            using var context = new MyContext();
            context
                .People
                .Where(p => p.FirstName.StartsWith("M"));
        }

        private int Sum(int x, int y) =>
            x + y;
        

        public interface ISomeInterface
        {
            IEnumerable<int> Fibonacci();
        }

        [Fact]
        public async Task HardToMock()
        {
            var client = Interceptor.For<HttpClient>();
            client
                .Setup(x => x.GetAsync(Arg.Ignore<string>()))
                .Returns(new HttpResponseMessage());
            
            var sut = new Target(client);

            await sut.Update();
            client.Verify();
        }

        public class Target
        {
            private readonly IIntercept<HttpClient> _client;

            public Target(IIntercept<HttpClient> client) => 
                _client = client;

            public Task Update() => 
                _client.Execute(x => x.GetAsync("https://asdfasdfasdf.adgasdf"));
        }

        [Fact]
        public void SelectPropertyDemo() =>
            Select<Person, string>(x => x.FirstName)
                .Should()
                .Be("FirstName");

        private static string Select<T, TResult>(Expression<Func<T, TResult>> func) =>
            ((MemberExpression)func.Body).Member.Name;
    }

    public class MyContext : DbContext
    {
        public DbSet<Person> People { get; set; }
    }

    public class Person
    {
        public string FirstName { get; set; }
    }
}