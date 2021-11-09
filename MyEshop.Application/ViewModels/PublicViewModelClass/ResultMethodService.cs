using System.Collections.Generic;

namespace MyEshop.Application.ViewModels.PublicViewModelClass
{
    public class ResultMethodService
    {
        public ResultMethodService()
        {
            _isSuccess = true;
            _isNotFound = false;
            Errors = new List<ErrorResultMethodService>();
        }
        
        public ResultMethodService(bool isSuccess, bool isNotFound)
        {
            _isSuccess = isSuccess;
            _isNotFound = isNotFound;
        }


        private bool _isSuccess;
        
        private bool _isNotFound;


        public bool IsSuccess
        {
            get
            {
                return _isSuccess;
            }
        }
        
        public bool IsNotFound
        {
            get
            {
                return _isNotFound;
            }
        }
        
        public IList<ErrorResultMethodService> Errors { set; get; }


        public void AddError(string title, string message)
            => AddError(new ErrorResultMethodService(title, message));
        
        public void AddError(ErrorResultMethodService errorResultMethodService)
        {
            if (IsSuccess)
                _isSuccess = false;

            _isNotFound = false;

            AddError(errorResultMethodService.Title, errorResultMethodService.Message);
        }

        public void NotFound()
        {
            _isSuccess = false;
            _isNotFound = true;
        }
    }
}
