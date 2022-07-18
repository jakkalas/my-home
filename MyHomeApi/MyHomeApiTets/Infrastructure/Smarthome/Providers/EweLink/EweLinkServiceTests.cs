using MyHomeApi.Infrastructure.Smarthome.Providers.Ewelink;
using NUnit.Framework;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NExpect;
using static NExpect.Expectations;
using MyHomeApi.Infrastructure.Models;
using System.Net;
using static PeanutButter.RandomGenerators.RandomValueGen;
using MyHomeApi.Infrastructure.Smarthome.Models;
using MyHomeApi.Infrastructure.Smarthome.Providers.Ewelink.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;
using PeanutButter.Utils;

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
                    var messageHandler = new MockHttpMessageHandler("TEST VALUE", HttpStatusCode.BadRequest);
                    var httpClient = new HttpClient(messageHandler);
                    var sut = Create(httpClient, configuration);

                    // execute expect
                    Expect(() => sut.GetAllDevicesAsync()).To.Throw<ApiException>();
                }
            }

            [TestFixture]
            public class GivenResponseStatusIsOK
            {
                [TestFixture]
                public class GivenValidResult
                {
                    [Test]
                    public async Task ThenShouldReturnDevices()
                    {
                        // setup
                        var configuration = Substitute.For<IConfiguration>();
                        configuration["EweLink:Url"].Returns("https://test.com");
                        var expectedDevices = GetRandomArray<Device>();
                        expectedDevices.ForEach(x => x.Extra.Extra.Uiid = GetRandomInt(1, 10));
                        var expectedResult = GetRandom<DevicesResult>();
                        expectedResult.DeviceList = expectedDevices;
                        var messageHandler = new MockHttpMessageHandler(JsonConvert.SerializeObject(expectedResult), HttpStatusCode.OK);
                        var httpClient = new HttpClient(messageHandler);
                        var sut = Create(httpClient, configuration);

                        // execute
                        var result = await sut.GetAllDevicesAsync();

                        // expect
                        Expect(result).To.Intersection.Equal(expectedDevices);
                    }
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
