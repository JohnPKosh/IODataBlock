namespace Business.Common.Responses
{
    public static class ResponseCodeExtensions
    {
        public static IResponseCode CreateFailureCode()
        {
            return new ResponseCode(500, "500 Internal Server Error");
        }

    }
}
