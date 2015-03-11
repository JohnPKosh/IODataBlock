namespace Business.Common.Requests
{
    public class RequestObject : IRequestObject
    {
        public object RequestData { get; set; }

        public string CorrelationId { get; set; }
    }
}