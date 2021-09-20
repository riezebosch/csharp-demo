using Xunit;

namespace TestProject1
{
    public class Records
    {
        [Fact]
        public void Class()
        {
            var first = new A();
            var second = new A();
            
            Assert.NotEqual(first, second);
        }
        
        [Fact]
        public void Struct()
        {
            var first = new B(3);
            var second = new B(3);
            var third = new B(4);
            
            Assert.Equal(first, second);
            Assert.NotEqual(first, third);
        }
        
        [Fact]
        public void RecordsExample()
        {
            var first = new C(3);
            var second = new C(3);
            
            Assert.Equal(first, second);
        }
        
        [Fact]
        public void RecordsEqual()
        {
            var first = new D(3) ;
            var second = new D(3);
            
            Assert.Equal(first, second);
        }
    }

    public record C(int Id);

    public record D
    {
        public D(int Id)
        {
            this.Id = Id;
        }

        public int Id { get; init; }
        public string Name { get; set; }

        public void Deconstruct(out int Id)
        {
            Id = this.Id;
        }
    }


    public struct B
    {
        private readonly int _i;

        public B(int i) => _i = i;
    }

    public class A
    {
    }
}