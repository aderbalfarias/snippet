//.csproj
//<Project Sdk="Microsoft.NET.Sdk">

//  <PropertyGroup>
//    <TargetFramework>netcoreapp2.2</TargetFramework>
//    <IsPackable>false</IsPackable>
//  </PropertyGroup>

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
            _mockPrintFileService = new Mock<IPrintFileService>();
            _mockAuditLoggingService = new Mock<IAuditLoggingService>();

            _handleResponseService = new HandleResponseService(_mockBaseRepository.Object,
                _mockPrintFileService.Object, _mockAuditLoggingService.Object);

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

        #region Setups 

        private Task HandleSetup()
        {
            _mockBaseRepository.Setup(s => s.Add(It.IsAny<PrintLetterEntity>()))
                .Returns(Task.FromResult(1));

            return Task.CompletedTask;
        }

        #endregion End Setups 

        #region Mocks

        private PpsnAllocation MockMessage() => new MockMessage
        {
            Id = 1
        };

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


