//.csproj
//  <ItemGroup>
//    <PackageReference Include="Microsoft.AspNetCore.App" />
//    <PackageReference Include="Microsoft.CodeCoverage" Version="16.3.0" />
//    <PackageReference Include="Moq" Version="4.13.0" />
//    <PackageReference Include="MSTest.TestAdapter" Version="1.4.0" />
//    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="15.3.0" />
//    <PackageReference Include="NServiceBus.Testing" Version="7.2.0" />
//    <PackageReference Include="xunit" Version="2.4.1" />
//    <PackageReference Include="xunit.runner.visualstudio" Version="2.2.0" />
//  </ItemGroup>
//</Project>

//--------------------------------------------------------------------------------

using Moq;
using Newtonsoft.Json;
using NServiceBus.Logging;
using NServiceBus.Testing;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.Services
{
    public class HandleResponseServiceTest
    {
        private readonly Mock<IBaseRepository> _mockBaseRepository;
        private readonly HandleResponseService _handleResponseService;
        private readonly TestableMessageHandlerContext _context;

        static StringBuilder logStatements = new StringBuilder();

        public HandleResponseServiceTest()
        {
            InitializeLogManager();

            _mockBaseRepository = new Mock<IBaseRepository>();

            _handleResponseService = new HandleResponseService(_mockBaseRepository.Object);

            _context = new TestableMessageHandlerContext();
        }

        [Fact]
        public async Task When_Handle_Receive_Message_Should_Log_Correctly()
        {
            await HandleSetup();

            var message = MockMessage();

            await _handleResponseService.Handle(message, _context).ConfigureAwait(false);

            var initialMesssage = $"Received message {message.Id}";
            var successMesssage = $"Message {message.Id} processed successfully";

            Assert.Contains(initialMesssage, LogStatements);
            Assert.Contains(successMesssage, LogStatements);
        }

        [Fact]
        public async Task When_Handle_Should_Log_Exception()
        {
            await HandleSetup();

            var message = MockMessage(); //make sure will throw a exception

            await _handleResponseService.Handle(message, _context).ConfigureAwait(false);

            var exceptionMessage = $"Error for message {message.Id}";

            Assert.Contains(exceptionMessage, LogStatements);
        }
        
        [Theory]
        [InlineData("Test", "Test", "Test", true, 1)]
        [InlineData("Test 2", "", "Test", false, 1)]
        [InlineData("Test 3", "Test", "Test", false, 1)]
        [InlineData("Test 4", "", "", true, 1)]
        public async Task Multiple_Test(string obj1, string obj2, string obj3, bool valid, int id)
        {
            //setups

            Assert.Equal(1, id);
            Assert.Contains("Test", obj1);
        }
        
        [Fact]
        public async Task When_Test_Should_Return_Without_Exceptions()
        {
            await ExecuteSetups();

            var exception = Record.ExceptionAsync(async () => await _consolidateJob.CallYourMethod());
            
            Assert.Null(exception.Result);
        }
        
        [Fact]
        public async Task When_Test1_Should_Log_Consolidated_File_Created()
        {
            await ExecuteSetups();

            await _consolidateJob.CallYourMethod();

            var successfulMessage = "Test created successfully";

            _mockLogger.Verify(x =>
                x.Log(LogLevel.Information, It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals(successfulMessage, o.ToString())),
                    It.IsAny<Exception>(), (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
        }
        
        [Fact]
        public async Task When_Handle_Should_Log_Exception()
        {
            await HandleSetup();

            var message = MockMessage();
            message.SerializedMessage = null;

            var exception = Assert.ThrowsAnyAsync<ArgumentNullException>(()
                    => _handleResponseService.Handle(message, _context));

            var exceptionMessage = $"Error for message id: {message.id}";

            Assert.Contains(exceptionMessage, LogStatements);
        }

        #region Setups 

        private Task HandleSetup()
        {
            _mockBaseRepository.Setup(s => s.Add(It.IsAny<SomeEntity>()))
                .Returns(Task.FromResult(1));

            return Task.CompletedTask;
        }

        private Task RepositoryGetObjectMethodSetup()
        {
            IQueryable<MyEntity> mocks = MockEntityDetail().AsQueryable();

            _mockBaseRepository.Setup(s
                    => s.GetObject(It.IsAny<Expression<Func<MyEntity, bool>>>(), It.IsAny<string>()))
                .Returns<Expression<Func<MyEntity, bool>>, string>((predicate, include)
                        => mocks.FirstOrDefault(predicate));

            return Task.CompletedTask;
        }

        private Task RepositoryGetWithIncludeMethodSetup()
        {
            IQueryable<MyEntity> mocks = MockEntityDetail().AsQueryable();

            _mockBaseRepository.Setup(s
                    => s.GetWithInclude(It.IsAny<Expression<Func<MyEntity, bool>>>(), It.IsAny<string>()))
                .Returns<Expression<Func<MyEntity, bool>>, string>((predicate, include)
                        => mocks.Where(predicate));

            return Task.CompletedTask;
        }

        private Task RepositoryGetWithIncludeMethodSetup()
        {
            var mocks = MockEntityDetail()
                .AsQueryable();

            _mockBaseRepository.Setup(s
                    => s.GetObjectAsync(It.IsAny<Expression<Func<MyEntity, bool>>>()))
                .Returns<Expression<Func<MyEntity, bool>>>(predicate
                        => Task.FromResult(mocks.FirstOrDefault(predicate)));

            return Task.CompletedTask;
        }
        
        #endregion End Setups 

        #region Mocks

        private MyEntity MockMessage() => new MockMessage
        {
            Id = 1
        };
        
        private Task MockAppSettings()
        {
            // Properties need to be virtual because we are mocking a concrete class
            _appSettings.SetupGet(s => s.Folder).Returns("Test");

            return Task.CompletedTask;
            
            // IOptions interface
            //_appSettings.SetupGet(s => s.Value).Returns(new AppSettings { Id = 3 });
        }

        #endregion End Mocks

        #region LogManager

        private Task InitializeLogManager()
        {
            logStatements.Clear();

            LogManager.Use<TestingLoggerFactory>()
                .WriteTo(new StringWriter(logStatements));

            return Task.CompletedTask;
        }

        public static string LogStatements => logStatements.ToString();

        #endregion End LogManager
    }
}


