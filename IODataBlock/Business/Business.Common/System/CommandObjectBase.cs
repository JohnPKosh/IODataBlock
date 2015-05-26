using System;
using Business.Common.Exceptions;
using Business.Common.Requests;
using Business.Common.Responses;
using Fasterflect;

namespace Business.Common.System
{
    public abstract class CommandObjectBase : ICommandObject, ICommandObjectFactory
    {
        #region Fields and Properties

        public IRequestObject RequestObject { get; set; }

        private string _commandName;
        public virtual string CommandName
        {
            get
            {
                return String.IsNullOrWhiteSpace(_commandName)? this.GetType().Name(): _commandName;
            }
            set { _commandName = value; }
        }

        public abstract string Description { get; }

        public abstract object SuccessResponseCode { get; }

        public abstract object ErrorResponseCode { get; }

        public abstract Func<IRequestObject, object> CommandFunction { get; set; }

        #endregion Fields and Properties

        public IResponseObject Execute()
        {
            // TODO determine if we wanted to short circuit here because of some condition we could do it here!
            // var rv = RequestObject.ToUncompletedResponse(null, RequestObject.CorrelationId);
            try
            {
                // Execute the CommandFunction here!
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