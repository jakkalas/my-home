using MyHomeApi.Infrastructure.Smarthome.Providers.Ewelink;
using NUnit.Framework;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using System.Threading.Tasks;
using NExpect;
using static NExpect.Expectations;
using MyHomeApi.Infrastructure.Models;

namespace MyHomeApiTets.Infrastructure.Smarthome.Providers.EweLink
{
    [TestFixture]
    public class EweLinkServiceTests
    {
        [TestFixture]
        public class GetAllDevicesAsyncTests
        {
            [TestFixture]
            public class GivenResponseStatusCodeIsNotOK
            {
                [Test]
                public void ThenShouldThrowApiException()
                {
                    // setup
                    var configuration = Substitute.For<IConfiguration>();
                    configuration["EweLink:Url"].Returns("https://test.com");
                    var httpClient = Substitute.For<HttpClient>();
                    httpClient.GetAsync(Arg.Any<string>()).Returns(Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)));
                    var sut = Create(httpClient, configuration);

                    // execute expect
                    Expect(() => sut.GetAllDevicesAsync()).To.Throw<ApiException>();
                }
            }
        }

        private static EweLinkService Create(
            HttpClient httpClient = null,
            IConfiguration configuration = null)
        {
            return new EweLinkService(
                httpClient ?? Substitute.For<HttpClient>(),
                configuration ?? Substitute.For<IConfiguration>());
        }
    }
}
