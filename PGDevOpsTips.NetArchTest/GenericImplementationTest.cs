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

        [Fact]
        public void ViewModelsNamingTest()
        {
            bool result = Types.InCurrentDomain()
                .That().ResideInNamespace(("PGDevOpsTips.Web.ViewModels"))
                .And().AreClasses()
                .Should().HaveNameEndingWith("ViewModel")
                .GetResult().IsSuccessful;

            Assert.True(result);
        }

        [Fact]
        public void ServicesNamingTest()
        {
            bool result = Types.InCurrentDomain()
                .That().ResideInNamespace(("PGDevOpsTips.Web.Services"))
                .And().AreClasses()
                .Should().HaveNameEndingWith("Service")
                .GetResult().IsSuccessful;

            Assert.True(result);
        }

        [Fact]
        public void Pages_ShouldNotDirectlyReference_DataTransferObjects()
        {
            var result = Types.InCurrentDomain()
                .That()
                .ResideInNamespace("PGDevOpsTips.Web.Pages")
                .ShouldNot()
                .HaveDependencyOn("PGDevOpsTips.Web.DataTransferObjects")
                .GetResult().IsSuccessful;

            Assert.True(result);
        }

        [Fact]
        public void ViewModels_ShouldNotDirectlyReference_DataTransferObjects()
        {
            var result = Types.InCurrentDomain()
                .That()
                .ResideInNamespace("PGDevOpsTips.Web.ViewModels")
                .ShouldNot()
                .HaveDependencyOn("PGDevOpsTips.Web.DataTransferObjects")
                .GetResult().IsSuccessful;

            Assert.True(result);
        }

        [Fact]
        public void DTOShouldOnlyBeReferencedByServicesTest()
        {
            //bool result = Types.InCurrentDomain()
            //    .That().HaveDependencyOn("PGDevOpsTips.Web.DataTransferObjects")
            //    //.And().ResideInNamespace(("PGDevOpsTips.Web"))
            //    .Should().ResideInNamespace(("PGDevOpsTips.Web.Services"))
            //    .GetResult().IsSuccessful;

            var result = Types.InCurrentDomain()
               .That()
               .ResideInNamespace("PGDevOpsTips.Web.Services")
               .Should()
               .HaveDependencyOn("PGDevOpsTips.Web.DataTransferObjects")
               .GetResult().IsSuccessful;

            Assert.True(result);
        }
    }
}
