namespace SimCivil.Orleans.Interfaces.Component
{
    public enum ErrorCode
    {
        Success = 0,
        ItemNotFound,
        InvalidOperation,
    }

    public interface IResult
    {
        ErrorCode Err { get; }

        string ErrMsg { get; }
    }



    public struct Result : IResult
    {
        public ErrorCode Err { get; set; }

        public string ErrMsg { get; set; }
    }

    public struct Result<T>
    {
        public ErrorCode Err { get; set; }

        public string ErrMsg { get; set; }

        public T Value { get; set; }
    }
}
