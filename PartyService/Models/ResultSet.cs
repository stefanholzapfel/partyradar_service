namespace PartyService.Models
{
    public class Result
    {
        public bool Succeeded { get; protected set; }

        public string ErrorMessage { get; private set; }

        public Result( bool succeeded, string errorMessage = null )
        {
            Succeeded = succeeded;
            ErrorMessage = errorMessage;
        }
    }
    public class ResultSet<T>:Result
    {
        public T Result { get; set; }

        public ResultSet( bool succeeded, string errorMessage= null ) : base( succeeded,errorMessage )
        {
        }
    }
}