using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Moq;

using Buaa.AIBot.Services;
using Buaa.AIBot.Controllers;
using Buaa.AIBot.Controllers.Models;
using Buaa.AIBot.Repository;
using Buaa.AIBot.Repository.Models;
using Buaa.AIBot.Repository.Exceptions;
using Microsoft.Extensions.Options;

namespace AIBotTest.Controller
{

    public class UserControllerTest
    {
        private Mock<IUserService> mockServ;
        
        private Mock<IUserRepository> mockRepo;

        private Mock<IOptions<TokenManagement>> mockTMO;

        private TokenManagement mockTM;

        private UserBody body;

        public UserControllerTest()
        {
            mockServ = new Mock<IUserService>();
            mockRepo = new Mock<IUserRepository>();
            mockTMO = new Mock<IOptions<TokenManagement>>();
            mockTM = new TokenManagement();
            mockTMO.Setup(tmo => tmo.Value).Returns(mockTM);
            body = new UserBody();
        }

        [Fact]
        public async Task SignupAsync_CheckFail()
        {
            StatusMessageResponse response = new StatusMessageResponse();
            mockServ.Setup(serv => serv.CheckSignUpBody(It.IsAny<UserBody>(), out response)).Returns(false);
            UserController controller = new UserController(mockServ.Object, mockRepo.Object, mockTMO.Object);
            
            var ret = await controller.SignUpAsync(body);
            var okRet = Assert.IsType<OkObjectResult>(ret);
            var resRet = Assert.IsType<StatusMessageResponse>(okRet.Value);
            Assert.Equal(resRet, response);
        }

        [Fact]
        public async Task SignupAsync_Success()
        {
            var response = new StatusMessageResponse();
            mockServ.Setup(serv => serv.CheckSignUpBody(It.IsAny<UserBody>(), out response)).Returns(true);
            mockRepo.Setup(repo => repo.InsertUserAsync(It.IsAny<UserInfo>())).ReturnsAsync(1);
            UserController controller = new UserController(mockServ.Object, mockRepo.Object, mockTMO.Object);

            var ret = await controller.SignUpAsync(body);
            var okRet = Assert.IsType<OkObjectResult>(ret);
            var resRet = Assert.IsType<StatusMessageResponse>(okRet.Value);
            Assert.Equal("success", resRet.Status);
            Assert.Equal("signup success", resRet.Message);
        }

        [Fact]
        public async Task SignupAsync_EmailHasExistException()
        {
            var response = new StatusMessageResponse();
            mockServ.Setup(serv => serv.CheckSignUpBody(It.IsAny<UserBody>(), out response)).Returns(true);
            mockRepo.Setup(repo => repo.InsertUserAsync(It.IsAny<UserInfo>())).
            ThrowsAsync(new EmailHasExistException(""));
            UserController controller = new UserController(mockServ.Object, mockRepo.Object, mockTMO.Object);

            var ret = await controller.SignUpAsync(body);
            var okRet = Assert.IsType<OkObjectResult>(ret);
            var resRet = Assert.IsType<StatusMessageResponse>(okRet.Value);
            Assert.Equal("emailExisted", resRet.Status);
            Assert.Equal("email has been used", resRet.Message);
        }

        [Fact]
        public async Task SignupAsync_NameHasExistException()
        {
            var response = new StatusMessageResponse();
            mockServ.Setup(serv => serv.CheckSignUpBody(It.IsAny<UserBody>(), out response)).Returns(true);
            mockRepo.Setup(repo => repo.InsertUserAsync(It.IsAny<UserInfo>())).
            ThrowsAsync(new NameHasExistException(""));
            UserController controller = new UserController(mockServ.Object, mockRepo.Object, mockTMO.Object);

            var ret = await controller.SignUpAsync(body);
            var okRet = Assert.IsType<OkObjectResult>(ret);
            var resRet = Assert.IsType<StatusMessageResponse>(okRet.Value);
            Assert.Equal("nameExisted", resRet.Status);
            Assert.Equal("name has been used", resRet.Message);
        }

        [Fact]
        public async Task LoginAsync_Fail()
        {
            mockServ.Setup(serv => serv.AuthorizeAccountAsync(It.IsAny<UserBody>())).ReturnsAsync(new string[]
            {
               string.Empty,
               "" 
            });
            UserController controller = new UserController(mockServ.Object, mockRepo.Object, mockTMO.Object);

            var ret = await controller.LoginAsync(body);
            var okRet = Assert.IsType<OkObjectResult>(ret);
            var resRet = okRet.Value;
            Assert.Equal("fail", resRet.GetType().GetProperty("Status").GetValue(resRet, null));
            Assert.Equal("wrong email or password", resRet.GetType().GetProperty("Message").GetValue(resRet, null));
            Assert.Equal(0, resRet.GetType().GetProperty("Timeout").GetValue(resRet, null));
        }

        [Fact]
        public async Task LoginAsync_Success()
        {
            mockServ.Setup(serv => serv.AuthorizeAccountAsync(It.IsAny<UserBody>())).ReturnsAsync(new string[]
            {
                "temp",
                "Traveler"
            });
            UserController controller = new UserController(mockServ.Object, mockRepo.Object, mockTMO.Object);
            mockTM.AccessExpiration = 30;
            
            var ret = await controller.LoginAsync(body);
            var okRet = Assert.IsType<OkObjectResult>(ret);
            var resRet = okRet.Value;
            Assert.Equal("success", resRet.GetType().GetProperty("Status").GetValue(resRet, null));
            Assert.Equal("login success", resRet.GetType().GetProperty("Message").GetValue(resRet, null));
            Assert.Equal(30 * 60, resRet.GetType().GetProperty("Timeout").GetValue(resRet, null));
        }


        [Fact]
        public async Task Fresh_Async_TokenInvalid()
        {
            mockServ.Setup(serv => serv.GetExpirationFromToken(It.IsAny<UserBody>())).Throws(new ArgumentException());
            UserController controller = new UserController(mockServ.Object, mockRepo.Object, mockTMO.Object);

            var ret = await controller.FreshAsync(body);
            var okRet = Assert.IsType<UnauthorizedObjectResult>(ret);
            var resRet = okRet.Value;
            Assert.Equal("fail", resRet.GetType().GetProperty("Status").GetValue(resRet, null));
            Assert.Equal("invalid token", resRet.GetType().GetProperty("Message").GetValue(resRet, null));
        }

        [Fact]
        public async Task FreshAsync_Expired()
        {
            mockServ.Setup(serv => serv.GetExpirationFromToken(It.IsAny<UserBody>())).Returns(DateTime.Now.AddMinutes(-30));
            UserController controller = new UserController(mockServ.Object, mockRepo.Object, mockTMO.Object);

            var ret = await controller.FreshAsync(body);
            var okRet = Assert.IsType<UnauthorizedObjectResult>(ret);
            var resRet = okRet.Value;
            Assert.Equal("fail", resRet.GetType().GetProperty("Status").GetValue(resRet, null));
            Assert.Equal("token has been expried", resRet.GetType().GetProperty("Message").GetValue(resRet, null));
        }

        [Fact]
        public async Task FreshAsync_UserNotExist()
        {
            mockServ.Setup(serv => serv.GetExpirationFromToken(It.IsAny<UserBody>())).Returns(DateTime.Now.AddMinutes(30));
            mockServ.Setup(serv => serv.FreshTokenAsync(It.IsAny<UserBody>())).Throws(new UserNotExistException(-1));
            UserController controller = new UserController(mockServ.Object, mockRepo.Object, mockTMO.Object);

            var ret = await controller.FreshAsync(body);
            var okRet = Assert.IsType<UnauthorizedObjectResult>(ret);
            var resRet = okRet.Value;
            Assert.Equal("fail", resRet.GetType().GetProperty("Status").GetValue(resRet, null));
            Assert.Equal("user has been removed", resRet.GetType().GetProperty("Message").GetValue(resRet, null));
        }

        [Fact]
        public async Task FreshAsync_Success()
        {
            mockServ.Setup(serv => serv.GetExpirationFromToken(It.IsAny<UserBody>())).Returns(DateTime.Now.AddMinutes(30));
            mockServ.Setup(serv => serv.FreshTokenAsync(It.IsAny<UserBody>())).ReturnsAsync("");
            UserController controller = new UserController(mockServ.Object, mockRepo.Object, mockTMO.Object);

            var ret = await controller.FreshAsync(body);
            var okRet = Assert.IsType<OkObjectResult>(ret);
            var resRet = okRet.Value;
            Assert.Equal("success", resRet.GetType().GetProperty("Status").GetValue(resRet, null));
            Assert.Equal("token fresh", resRet.GetType().GetProperty("Message").GetValue(resRet, null));
        }
    }
}