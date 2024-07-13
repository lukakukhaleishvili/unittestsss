using FakeItEasy;
using FluentAssertions;
using FluentAssertions.Extensions;
using NetworkUtility.DNS;
using NetworkUtility.Ping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;



////githubze taviden uech mivcet it init brdzaneba

namespace NetworkUtility.Test.PingTests
{
    public class NetworkServiceTests
    {
        private readonly NetworkService _pingService;
        private readonly IDNS _dns;
        public NetworkServiceTests()
        {
            //dependencies

            _dns = A.Fake<IDNS>();

            _pingService = new NetworkService(_dns);
        }


        [Fact]
        public void NetworkService_SendPing_ReturnString()
        {
            //Arrange - varieables, classess, mocks
            A.CallTo(() => _dns.SendDns()).Returns(true);

            //Act
            var result = _pingService.SendPing();


            //Assert -- fluent extension is downloaded, anristan inmemory extension
            result.Should().NotBeNullOrWhiteSpace();
            result.Should().Be("Success: Ping sent!");
            result.Should().Contain("Success", Exactly.Once());

        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(2, 2, 4)]
        public void NetworkService_PingTimeout_ReturnInt(int a, int b, int expected)
        {
            //Arrange
            


            //Act
            var result = _pingService.PingTimeout(a, b);

            //Assert
            result.Should().Be(expected);
            result.Should().BeGreaterThanOrEqualTo(2);
            result.Should().NotBeInRange(-10000, 0);

        }


        [Fact]
        public void NetworkService_lastPingDate_returnDate()
        {
            var result = _pingService.LasPingDate();
            result.Should().BeAfter(1.January(2010));
            result.Should().BeBefore(1.January(2030));
        }


        [Fact]
        public void NetworkService_GetPingOptions_ReturnObject()
        {
            var expected = new PingOptions()
            {
                DontFragment = true,
                Ttl = 1
            };

            var result = _pingService.GetPingOptions();

            result.Should().BeOfType<PingOptions>();
            result.Should().BeEquivalentTo(expected);
            result.Ttl.Should().Be(1);
        }



        [Fact]
        public void NetworkServices_GetMostRecentPings_ReturnRecentPings()
        {
            var expected = new PingOptions()
            {
                DontFragment = true,
                Ttl = 1
            };


            var result = _pingService.MostRecentPings();



            //result.Should().BeOfType<IEnumerable<PingOptions>>();
            result.Should().ContainEquivalentOf(expected);
            result.Should().Contain(x => x.DontFragment == true);

        }


    }
}
