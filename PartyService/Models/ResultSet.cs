namespace PartyService.Models
{
    public class Result
    {
        public bool Succeeded { get; protected set; }

        public Result( bool succeeded )
        {
            Succeeded = succeeded;
        }
    }
    public class ResultSet<T>:Result
    {
        public T Result { get; set; }

        public string ErrorMessage { get; protected set; }

        public ResultSet( bool succeeded, string errorMessage= null ) : base( succeeded )
        {
            ErrorMessage = errorMessage;
        }
    }
}