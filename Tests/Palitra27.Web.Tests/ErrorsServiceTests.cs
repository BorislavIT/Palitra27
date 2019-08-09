namespace Palitra27.Web.Tests
{
    using Palitra27.Services.Data;
    using Xunit;

    public class ErrorsServiceTests
    {
        [Theory]
        [InlineData("Test1", "Test2")]
        public void CreateBrandShouldCreateBrand(string errorMessage, string hyperLink)
        {
            var errorsService = new ErrorsService();

            var model = errorsService.CreateCreateionErrorViewModel(errorMessage, hyperLink);

            Assert.Equal(errorMessage, model.ErrorMessage);
            Assert.Equal(hyperLink, model.HyperLink);
        }
    }
}
