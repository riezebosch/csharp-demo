using Xunit;

namespace TestProject1
{
    public abstract class Abstract
    {
        public void NonOverridable()
        {
        }
        
        public virtual void First()
        {
        }

        public abstract void Second();
    }

    class Derived : Abstract
    {
        public void NonOverridable()
        {
        }

        public override void First()
        {
            base.First();
        }

        public override void Second()
        {
        }
    }

    public class AbstractDemo
    {
        [Fact]
        public void Polymorphism()
        {
            Abstract a = new Derived();
            a.First(); // <- only at runtime it is known which actual method to invoke
            
            a.NonOverridable(); // <- compiler already knows which method to invoke
        }
    }
}