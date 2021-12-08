using System.Threading.Tasks;
using NUnit.Framework;
using static CrudTest.BddTdd.Tests.IntegrationTests.Testing;

namespace CrudTest.BddTdd.Tests.IntegrationTests
{
    public class TestBase
    {
        [SetUp]
        public async Task TestSetUp()
        {
            await ResetState();
        }
    }    
}



