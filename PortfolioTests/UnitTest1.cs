using DataLayer;
using Xunit;

namespace PortfolioTests
{
    public class TitleDataServiceTests
    {
        /* Titles */

        [Fact]
        public void Title_Object_HasProperties()
        {
            var title = new Title();
            Assert.Null(title.Id);
            Assert.Null(title.PrimaryTitle);
            Assert.Null(title.OriginalTitle);
            Assert.False(title.IsAdult);
            Assert.Null(title.StartYear);
            // Add asserts for other properties as needed
        }
    }
    }