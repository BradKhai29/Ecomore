namespace BusinessLogic.Models.Base
{
    public interface IResult<TValue>
    {
        TValue Value { get; set; }

        bool IsSuccess { get; set; }
    }
}
