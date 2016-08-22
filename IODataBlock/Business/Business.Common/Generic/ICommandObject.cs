using Business.Common.GenericRequests;
using Business.Common.GenericResponses;
using Business.Common.Responses;
using Newtonsoft.Json;
using System;

namespace Business.Common.Generic
{
    public interface ICommandObject<TIn, TOut>
    {
        string CommandName { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        string Description { get; }

        #region Default Response Codes

        [JsonIgnore]
        IResponseCode UncompletedResponseCode { get; set; }

        [JsonIgnore]
        IResponseCode SuccessResponseCode { get; set; }

        [JsonIgnore]
        IResponseCode ErrorResponseCode { get; set; }

        #endregion Default Response Codes

        [JsonIgnore]
        Func<IRequestObject<TIn>, TOut> CommandFunction { get; set; }

        IResponseObject<TIn, TOut> Execute();
    }
}