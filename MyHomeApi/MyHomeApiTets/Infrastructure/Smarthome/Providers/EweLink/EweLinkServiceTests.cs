using MyHomeApi.Infrastructure.Smarthome.Providers.Ewelink;
using NUnit.Framework;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NExpect;
using static NExpect.Expectations;
using System.Net;
using static PeanutButter.RandomGenerators.RandomValueGen;
using MyHomeApi.Infrastructure.Smarthome.Models;
using MyHomeApi.Infrastructure.Smarthome.Providers.Ewelink.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;
using PeanutButter.Utils;
using MyHomeApi.Entities;

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
                        expectedDevices.ForEach(x => x.Extra.Extra.Uiid = GetRandomInt(1, 8));
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

        [TestFixture]
        public class GetDeviceAsyncTests
        {
            [TestFixture]
            public class GivenResponseStatusIsOK
            {
                [TestFixture]
                public class GivenValidResult
                {
                    [Test]
                    public async Task ThenShouldReturnDevice()
                    {
                        // setup
                        var configuration = Substitute.For<IConfiguration>();
                        configuration["EweLink:Url"].Returns("https://test.com");
                        var expectedDevice = GetRandom<Device>();
                        expectedDevice.Extra.Extra.Uiid = GetRandomInt(1, 8);
                        var messageHandler = new MockHttpMessageHandler(JsonConvert.SerializeObject(expectedDevice), HttpStatusCode.OK);
                        var httpClient = new HttpClient(messageHandler);
                        var sut = Create(httpClient, configuration);

                        // execute
                        var result = await sut.GetDeviceAsync(GetRandomString());

                        // expect
                        Expect(result).To.Deep.Equal(expectedDevice);
                    }
                }
            }
        }

        [TestFixture]
        public class GetIsDevicePowerOnTests
        {
            [TestFixture]
            public class GivenResponseStatusIsOK
            {
                [TestFixture]
                public class GivenResultHasNoSwitches
                {
                    [TestCase("on", true)]
                    [TestCase("off", false)]
                    public async Task ThenShouldReturnCorrectPoweredOnState(string switchStatus, bool expectedResult)
                    {
                        // setup
                        var configuration = Substitute.For<IConfiguration>();
                        configuration["EweLink:Url"].Returns("https://test.com");
                        var expectedDevice = GetRandom<Device>();
                        expectedDevice.Extra.Extra.Uiid = GetRandomInt(1, 8);
                        expectedDevice.Params.Switches = new Switch[0];
                        expectedDevice.Params.SwitchStatus = switchStatus;
                        var messageHandler = new MockHttpMessageHandler(JsonConvert.SerializeObject(expectedDevice), HttpStatusCode.OK);
                        var httpClient = new HttpClient(messageHandler);
                        var sut = Create(httpClient, configuration);

                        // execute
                        var result = await sut.GetIsDevicePowerOn(GetRandomString(), 0);

                        // expect
                        Expect(result).To.Equal(expectedResult);
                    }
                }

                [TestFixture]
                public class GivenResultHasSwitches
                {
                    [TestCase("on", true)]
                    [TestCase("off", false)]
                    public async Task ThenShouldReturnCorrectPoweredOnState(string switchStatus, bool expectedResult)
                    {
                        // setup
                        var configuration = Substitute.For<IConfiguration>();
                        configuration["EweLink:Url"].Returns("https://test.com");
                        var expectedDevice = GetRandom<Device>();
                        expectedDevice.Extra.Extra.Uiid = GetRandomInt(1, 8);
                        expectedDevice.Params.Switches = GetRandomArray<Switch>(1);
                        expectedDevice.Params.Switches[0].SwitchStatus = switchStatus;
                        var messageHandler = new MockHttpMessageHandler(JsonConvert.SerializeObject(expectedDevice), HttpStatusCode.OK);
                        var httpClient = new HttpClient(messageHandler);
                        var sut = Create(httpClient, configuration);

                        // execute
                        var result = await sut.GetIsDevicePowerOn(GetRandomString(), 0);

                        // expect
                        Expect(result).To.Equal(expectedResult);
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
