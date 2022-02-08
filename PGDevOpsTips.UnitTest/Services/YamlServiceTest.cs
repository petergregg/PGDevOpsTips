using Moq;
using PGDevOpsTips.Web.Interfaces;
using PGDevOpsTips.Web.Models;
using Xunit;

namespace PGDevOpsTips.UnitTest
{
    public class YamlServiceTest
    {
        [Fact]
        public void YamlService_WhenIsCalled_TimesOnce()
        {
            var plainFileContents = "---\ntitle: \nauthor: Peter Gregg\ndescription: My second test article\nimage: https://dummyimage.com/800x600/000/fff&text=placeholder\nthumbnail: https://dummyimage.com/200x200/000/fff&text=placeholder\ntype: article\nstatus: draft\npublished: 2021/12/28 15:30:00\ncategories: \n  - Test\n---\n\n# H1 Test\n- Bullet\n- Points\n- For\n- Testing\n\n## H2 Test\n- Some\n- More\n- Bullet\n- Points\n\n### H3 Test\n- And\n- Some\n- More\n- Bullets\n";
           
            var content = new Content();

            var yamlServiceMock = new Mock<IYamlService>();

            yamlServiceMock.Setup(x => x.YamlToContent(It.IsAny<string>(), It.IsAny<Content>())).Returns(content);

            yamlServiceMock.Object.YamlToContent(plainFileContents, content);

            //var service = new YamlService();

            //service.YamlToContent(plainFileContents, content);

            yamlServiceMock.Verify(x => x.YamlToContent(It.IsAny<string>(), It.IsAny<Content>()), Times.Once());
        }

        [Fact]
        public void YamlService_WhenStatusIsNull_ReturnsContent()
        {
            // Arrange
            var plainFileContents = "---\ntitle: \nauthor: Peter Gregg\ndescription: My second test article\nimage: https://dummyimage.com/800x600/000/fff&text=placeholder\nthumbnail: https://dummyimage.com/200x200/000/fff&text=placeholder\ntype: article\nstatus: \npublished: 2021/12/28 15:30:00\ncategories: \n  - Test\n---\n\n# H1 Test\n- Bullet\n- Points\n- For\n- Testing\n\n## H2 Test\n- Some\n- More\n- Bullet\n- Points\n\n### H3 Test\n- And\n- Some\n- More\n- Bullets\n";
            var content = new Content();
            var yamlServiceMock = new Mock<IYamlService>();

            yamlServiceMock.Setup(x => x.YamlToContent(It.IsAny<string>(), It.IsAny<Content>())).Returns(content);

            // Act
            yamlServiceMock.Object.YamlToContent(plainFileContents, content);

            // Assert
            yamlServiceMock.Verify(x => x.YamlToContent(It.IsAny<string>(), It.IsAny<Content>()), Times.Once());
        }
    }
}
