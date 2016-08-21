namespace Business.Common.Responses
{
    public class ResponseCode : IResponseCode
    {
        public ResponseCode()
        {
        }

        public ResponseCode(int? id, string code = null)
        {
            Id = id;
            Code = code;
        }

        #region IResponseCode Members

        public int? Id { get; set; }

        public string Code { get; set; }

        public override string ToString()
        {
            return Code;
        }

        #endregion IResponseCode Members
    }
}