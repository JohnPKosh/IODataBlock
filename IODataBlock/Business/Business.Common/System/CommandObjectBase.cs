using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Common.Exceptions;
using Business.Common.Requests;
using Business.Common.Responses;

namespace Business.Common.System
{
    public abstract class CommandObjectBase : ICommandObject, ICommandObjectFactory
    {
        public IRequestObject RequestObject { get; set; }

        //private string _CommandName;
        //public string CommandName
        //{
        //    get { return _CommandName; }
        //}

        //private string _Description;
        //public string Description
        //{
        //    get { return _Description; }
        //}

        public abstract string CommandName { get; }

        public abstract string Description { get; }

        public abstract object SuccessResponseCode { get; }

        public abstract object ErrorResponseCode { get; }

        public abstract Func<IRequestObject, object> CommandFunction { get; set; } 

        public IResponseObject Execute()
        {
            // if we needed to short circuit here because of some condition we could do it here!
            //var rv = RequestObject.ToUncompletedResponse(null, RequestObject.CorrelationId);
            try
            {
                // do some work

                var rv = CommandFunction.Invoke(RequestObject);
                return RequestObject.ToSuccessfullResponse(rv, SuccessResponseCode, RequestObject.CorrelationId);
            }
            catch (Exception ex)
            {
                return RequestObject.ToFailedResponse(ExceptionObjectListBase.Create(ex), ErrorResponseCode, RequestObject.CorrelationId);
            }
        }

        public abstract ICommandObject Create(IRequestObject requestObject);

    }
}
