
namespace Portfolio.Tests
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
          
        }

        [Fact]
        public void GetTitles_ValidUserId_ReturnsListOfTitles()
        {
            var service = new TitleDataService();
            var titles = service.GetTitles(1); 
            Assert.NotNull(titles);
            Assert.True(titles.Count > 0);
        }

        [Fact]
        public void GetTitles_InvalidUserId_ThrowsArgumentException()
        {
            var service = new TitleDataService();
            Assert.Throws<ArgumentException>(() => service.GetTitles(-1));
        }

        [Fact]
        public void GetTitles_ValidUserIdWithPagination_ReturnsPagedListOfTitles()
        {
            var service = new TitleDataService();
            var page = 0;
            var pageSize = 10;
            var titles = service.GetTitles(1, page, pageSize);
            Assert.NotNull(titles);
            Assert.True(titles.Count <= pageSize);
        }

        [Fact]
        public void GetTitle_ValidUserIdAndTitleId_ReturnsTitle()
        {
            var service = new TitleDataService();
            var titleId = "tt26693752"; 
            var title = service.GetTitle(1, titleId);
            Assert.NotNull(title);
            Assert.Equal(titleId, title.Id);
        }


        [Fact]
        public void GetTitle_InvalidUserId_ThrowsArgumentException()
        {
            var service = new TitleDataService();
            Assert.Throws<ArgumentException>(() => service.GetTitle(-1, "tt26693752"));
        }



        [Fact]
        public void GetTitle_InvalidTitleId_ReturnsNull()
        {
            var service = new TitleDataService();
            var title = service.GetTitle(1, "invalid_id");
            Assert.Null(title);
        }

        [Fact]
        public void CreateTitle_ValidData_CreatesNewTitle()
        {
            var service = new TitleDataService();
            var title = service.CreateTitle(1, "Test Primary Title", "Test Original Title", false, "2023");
            Assert.NotNull(title);
            Assert.NotNull(title.Id);
            Assert.Equal("Test Primary Title", title.PrimaryTitle);
            Assert.Equal("Test Original Title", title.OriginalTitle);
            Assert.False(title.IsAdult);
            Assert.Equal("2023", title.StartYear);

            // Cleanup
            service.DeleteTitle(1, title.Id);
        }

        [Fact]
        public void CreateTitle_InvalidUserId_ThrowsArgumentException()
        {
            var service = new TitleDataService();
            Assert.Throws<ArgumentException>(() => service.CreateTitle(-1, "Title", "Title", false, "2023"));
        }

        [Fact]
        public void UpdateTitle_ValidData_UpdatesTitle()
        {
            var service = new TitleDataService();
            var title = service.CreateTitle(1, "Original Title", "Original Title", false, "2023");
            title.PrimaryTitle = "Updated Title";
            title.OriginalTitle = "Updated Original Title";
            title.IsAdult = true;
            title.StartYear = "2024";

            var result = service.UpdateTitle(1, title);
            Assert.True(result);

            var updatedTitle = service.GetTitle(1, title.Id);
            Assert.Equal("Updated Title", updatedTitle.PrimaryTitle);
            Assert.Equal("Updated Original Title", updatedTitle.OriginalTitle);
            Assert.True(updatedTitle.IsAdult);
            Assert.Equal("2024", updatedTitle.StartYear);

            // Cleanup
            service.DeleteTitle(1, title.Id);
        }

        [Fact]
        public void UpdateTitle_InvalidUserId_ThrowsArgumentException()
        {
            var service = new TitleDataService();
            var title = new Title { Id = "tt26693752" }; 
            Assert.Throws<ArgumentException>(() => service.UpdateTitle(-1, title));
        }

        [Fact]
        public void UpdateTitle_NonExistingTitle_ReturnsFalse()
        {
            var service = new TitleDataService();
            var title = new Title { Id = "non_existing_id" };
            var result = service.UpdateTitle(1, title);
            Assert.False(result);
        }

        [Fact]
        public void DeleteTitle_ValidTitleId_DeletesTitle()
        {
            var service = new TitleDataService();
            // First create a title to delete
            var title = service.CreateTitle(1, "Title to Delete", "Title to Delete", false, "2023");

            var result = service.DeleteTitle(1, title.Id);
            Assert.True(result);

            var deletedTitle = service.GetTitle(1, title.Id);
            Assert.Null(deletedTitle);
        }

        [Fact]
        public void DeleteTitle_InvalidUserId_ThrowsArgumentException()
        {
            var service = new TitleDataService();
            Assert.Throws<ArgumentException>(() => service.DeleteTitle(-1, "tt0000001"));
        }

        [Fact]
        public void DeleteTitle_NonExistingTitleId_ReturnsFalse()
        {
            var service = new TitleDataService();
            var result = service.DeleteTitle(1, "non_existing_id");
            Assert.False(result);
        }



#if RUN_ALL_TESTS
#endif
    }

}
