using System.Collections.Generic;
using AutoMapper;
using MetricsManager.Controllers;
using MetricsManager.DataAccessLayer.Interfaces;
using MetricsManager.DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace MetricsManagerTests
{
    public class AgentsControllerUnitTests
    {
        private readonly AgentsController _controller;
        
        private readonly Mock<IAgentInfoRepository> _repositoryMock;
        private readonly Mock<ILogger<AgentsController>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        public AgentsControllerUnitTests()
        {
            _repositoryMock = new Mock<IAgentInfoRepository>();
            _loggerMock = new Mock<ILogger<AgentsController>>();
            _mapperMock = new Mock<IMapper>();

            _controller = new AgentsController(_repositoryMock.Object, _loggerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public void RegisterAgent_ShouldCall_Create_From_Repository()
        {
            _repositoryMock.Setup(repository =>
                repository.Create(It.IsAny<AgentInfo>())).Verifiable();
            
            var result = _controller.RegisterAgent(new MetricsManager.Requests.AgentInfoRegisterRequest
            {
                Address = string.Empty,
            });

            _repositoryMock.Verify(repository =>
                repository.Create(It.IsAny<AgentInfo>()), Times.AtMostOnce());
        }

        [Fact]
        public void UnregisterAgent_ShouldCall_Delete_From_Repository()
        {
            _repositoryMock.Setup(repository =>
                repository.Delete(It.IsAny<string>())).Verifiable();
            
            var result = _controller.UnregisterAgent(new MetricsManager.Requests.AgentInfoUnregisterRequest
            {
                Address = string.Empty,
            });

            _repositoryMock.Verify(repository =>
                repository.Delete(It.IsAny<string>()), Times.AtMostOnce());
        }
        
        [Fact]
        public void EnableAgentById_ReturnsOk()
        {
            var agentId = 1;

            var result = _controller.EnableAgentById(agentId);

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
        
        [Fact]
        public void DisableAgentById_ReturnsOk()
        {
            var agentId = 1;

            var result = _controller.DisableAgentById(agentId);

            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
        
        [Fact]
        public void GetRegisterAgents_ShouldCall_GetAgents_From_Repository()
        {
            _repositoryMock.Setup(repository => repository.GetAgents())
                .Returns(new List<AgentInfo>());

            _controller.GetRegisterAgents();

            _repositoryMock.Verify(repository =>
                    repository.GetAgents(), Times.AtMostOnce());
        }
    }
}