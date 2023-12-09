using NetArchTest.Rules;
using Xunit;

namespace PGDevOpsTips.NetArchTest
{
    public class GenericImplementationTest
    {
        [Fact]
        public void Interfaces_WhenNameStartsWithI_CanBeImplemented()
        {
            bool result = Types.InCurrentDomain()
                .That()
                .AreInterfaces()
                .Should()
                .HaveNameStartingWith("I")
                .GetResult()
                .IsSuccessful;

            Assert.True(result);
        }
    }
}
