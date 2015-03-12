namespace Business.Common.Requests
{
    public class RequestObject : IRequestObject
    {
        public string CommandName { get; set; }

        public object RequestData { get; set; }

        public string CorrelationId { get; set; }
    }
}