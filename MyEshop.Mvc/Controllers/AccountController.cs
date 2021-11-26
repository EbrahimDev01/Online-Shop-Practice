using Microsoft.AspNetCore.Mvc;
using MyEshop.Application.Interfaces;

namespace MyEshop.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
    }
}
