using System.Collections.Generic;

namespace MyEshop.Application.ViewModels.PublicViewModelClass
{
    public class ResultMethodService
    {
        public ResultMethodService()
        {
            IsSuccess = true;
            IsNotFound = false;
        }

        public ResultMethodService(bool isSuccess, bool isNotFound)
        {
            IsSuccess = isSuccess;
            IsNotFound = isNotFound;
        }


        public bool IsSuccess { set; get; }
        public bool IsNotFound { set; get; }

        public IList<ErrorResultMethodService> Errors { set; get; }
            = new List<ErrorResultMethodService>();


        public void AddError(string title, string message)
        {
            if (IsSuccess)
                IsSuccess = false;

            Errors.Add(new ErrorResultMethodService(title, message));
        }

        public void AddError(ErrorResultMethodService errorResultMethodService)
        {
            if (IsSuccess)
                IsSuccess = false;

            AddError(errorResultMethodService.Title, errorResultMethodService.Message);
        }


        public void NotFound()
        {
            IsSuccess = false;
            IsNotFound = true;
        }
    }
}
