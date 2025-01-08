namespace api.Models
{
     public class ErrorModel
    {
        public int StatusCode { get; set; } // HTTP Status Code
        public string ErrorMessage { get; set; } // Description of the error

        public override string ToString()
        {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }
    }
}