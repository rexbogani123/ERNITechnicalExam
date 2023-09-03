using FakeItEasy;
using BankSystemAssessment.Model;
using BankSystemAssessment.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using BankSystemAssessment.Dto;
using BankSystemAssessment.Interfaces;
using Moq;

namespace BankSystemTestxNunit.Controller
{
    public class ControllerTest
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public ControllerTest()
        {
            _userRepository = A.Fake<IUserRepository>();
            _mapper = A.Fake<IMapper>();
        }

        [Fact]
        public void UserController_GetUser_ReturnOK()
        {
            //Arrange
            var users = A.Fake<ICollection<UserDto>>();
            var userList = A.Fake<List<UserDto>>();
            A.CallTo(() => _mapper.Map<List<UserDto>>(users)).Returns(userList);
            var controller = new UserController(_userRepository, _mapper);

            //Act
            var result = controller.GetUsers();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void UserController_CreateUser_ReturnOK()
        {
            //Arrange
            string UserName = "DataTest";
            string Password = "DataTest";
            var userMap = A.Fake<User>();
            var user = A.Fake<User>();
            var userCreate = A.Fake<User>();
            var users = A.Fake<ICollection<UserDto>>();
            var userList = A.Fake<IList<UserDto>>();
            A.CallTo(() => _userRepository.GetUserTrimToUpper(user)).Returns(user);
            A.CallTo(() => _mapper.Map<User>(userCreate)).Returns(user);
            A.CallTo(() => _userRepository.CreateUser(UserName, Password, userMap)).Returns(true);
            var controller = new UserController(_userRepository, _mapper);

            //Act
            var result = controller.CreateUser(UserName, Password, user);

            //Assert
            result.Should().NotBeNull();
        }
    }
}
