using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Application.ViewModels.Public
{
    public class ErrorResultMethodService
    {
        public ErrorResultMethodService()
        {

        }

        public ErrorResultMethodService(string title, string message)
        {
            Title = title;
            Message = message;
        }

        public string Title { set; get; }
        public string Message { set; get; }
    }
}
