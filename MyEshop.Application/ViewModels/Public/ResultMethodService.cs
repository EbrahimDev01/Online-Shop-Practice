using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEshop.Application.ViewModels.Public
{
    public class ResultMethodService
    {
        public ResultMethodService()
        {
        }

        public ResultMethodService(bool isSuccess, bool isNotFound)
        {
            IsSuccess = isSuccess;
            IsNotFound = isNotFound;
        }


        public bool IsSuccess { set; get; } = true;
        public bool IsNotFound { set; get; } = false;

        public IList<ErrorResultMethodService> Errors { set; get; }
            = new List<ErrorResultMethodService>();


        public void AddError(string title, string message)
        {
            if (IsSuccess)
                IsSuccess = false;

            Errors.Add(new ErrorResultMethodService(title, message));
        }

        public void NotFound() => IsNotFound = false;
    }
}
