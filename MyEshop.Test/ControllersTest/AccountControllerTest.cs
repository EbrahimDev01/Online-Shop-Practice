using Moq;
using MyEshop.Application.Interfaces;
using MyEshop.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyEshop.Test.ControllersTest
{
    internal class AccountControllerTest
    {
        private readonly AccountController _accountController;
        private readonly Mock<IAccountService> _mockAccountService;

        public AccountControllerTest()
        {
            _mockAccountService = new Mock<IAccountService>();

            _accountController = new AccountController(_mockAccountService.Object);
        }
    }
}
