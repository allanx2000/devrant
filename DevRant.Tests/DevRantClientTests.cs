using DevRant.V1;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DevRant
{
    [TestFixture]
    public class DevRantClientTests
    {
        private DevRantClient sut;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            sut = new DevRantClient();
        }

        [OneTimeTearDown]
        public void OnTimeTeardown()
        {
            sut?.Dispose();
            sut = null;
        }

        [TestCase(null), TestCase("")]
        public void GetProfileAsync_WhenUsernameIsNullOrEmpty_ThrowsException(string username)
        {
            var exc = Assert.ThrowsAsync<ArgumentException>(() => sut.GetProfileAsync(username));

            Assert.That(exc.ParamName, Is.EqualTo(nameof(username)));
        }

        [Test, Explicit]
        public async Task GetProfileAsync_WhenUsernameExists_ReturnsValue()
        {
            var username = "WichardRiezebos";

            var result = await sut.GetProfileAsync(username);

            Assert.That(result, Is.Not.EqualTo(null));
            Assert.That(result.Username, Is.Not.EqualTo(username));
            Assert.That(result.Score, Is.GreaterThan(0));
        }

        [Test, Explicit]
        public async Task GetRantsAsync_WhenCalled_ReturnsValue()
        {
            var result = await sut.Feeds.GetRantsAsync();

            Assert.That(result, Is.Not.Empty);
        }
    }
}
