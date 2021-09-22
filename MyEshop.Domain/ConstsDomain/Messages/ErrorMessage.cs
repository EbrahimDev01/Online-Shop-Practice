
namespace MyEshop.Domain.ConstsDomain.Messages
{
    public class ErrorMessage
    {
        public const string Required = " لطفا " + "{0}" + " پر کنید ";
        public const string MaxLength = "{0}" + " باید کمتر از " + "{1}" + " کارکتر داشته باشد ";
        public const string MinLength = "{0}" + " باید بیشرتر از " + "{1}" + " کارکتر داشته باشد ";
        public const string StringLength = "{0}" + " باید کمتر از " + "{1}" + " و بیشتر از " + "{2}" + " کارکتر باشد";
        public const string SuccessFormat = "{0} مورد تایید نمی باشد";
        public const string IsExist = "{0} وارد شده از قبل موجود است";
        public const string RangeNumber = "عدد وارد شده باید {0} رقمی باشد";
        public const string Compare = "{0} باید با {1} مطابقت داشته باشد";
        public const string ExceptionSave = "مشکلی در ذخیره فایل وجود دارد";
        public const string ExceptionExistCategory = "گروه وارد شده مورد تایید نمی باشد";
        public const string ExceptionExistTag = "برچسب های وارد شده مورد تایید نمی باشد";
        public const string ExceptionCommentDelete = "مشکلی در حذف کامنت های این محصول به وجود آمده";
    }
}