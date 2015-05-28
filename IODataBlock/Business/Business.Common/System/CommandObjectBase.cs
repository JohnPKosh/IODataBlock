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
                return String.IsNullOrWhiteSpace(_commandName) ? this.GetType().Name() : _commandName;
            }
            set { _commandName = value; }
        }

        public virtual string Description { get; set; }

        #region Default Response Codes

        private IResponseCode _uncompletedResponseCode = new ResponseCode(400, @"400 Bad Request");

        public virtual IResponseCode UncompletedResponseCode
        {
            get { return _uncompletedResponseCode; }
            set { _uncompletedResponseCode = value; }
        }

        private IResponseCode _successResponseCode = new ResponseCode(200, @"200 OK");

        public virtual IResponseCode SuccessResponseCode
        {
            get { return _successResponseCode; }
            set { _successResponseCode = value; }
        }

        private IResponseCode _errorResponseCode = new ResponseCode(500, @"500 Internal Server Error");

        public virtual IResponseCode ErrorResponseCode
        {
            get { return _errorResponseCode; }
            set { _errorResponseCode = value; }
        }

        #endregion Default Response Codes

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
                if (rv.GetType().Implements<IResponseObject>()) return rv as IResponseObject;
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