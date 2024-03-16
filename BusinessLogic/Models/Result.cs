using BusinessLogic.Models.Base;

namespace BusinessLogic.Models
{
    public class Result<TValue> : IResult<TValue>
    {
        public TValue Value { get; set; }

        public bool IsSuccess { get; set; }

        public static IResult<TValue> Success(TValue value)
        {
            return new Result<TValue>
            {
                Value = value,
                IsSuccess = true
            };
        }

        public static IResult<TValue> Failed(TValue value = default(TValue))
        {
            return new Result<TValue>
            {
                Value = value,
                IsSuccess = false
            };
        }
    }
}
