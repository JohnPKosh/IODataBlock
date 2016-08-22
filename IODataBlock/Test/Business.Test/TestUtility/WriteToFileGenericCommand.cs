using Business.Common.Generic;
using Business.Common.GenericRequests;
using System;

namespace Business.Test.TestUtility
{
    public class WriteToFileGenericCommand : CommandObjectBase<string, string>
    {
        public override string Description
        {
            get { return "WriteToFile - Writes to File."; }
        }

        public override Func<IRequestObject<string>, string> CommandFunction { get; set; }

        public override ICommandObject<string, string> Create(IRequestObject<string> requestObject)
        {
            return new WriteToFileGenericCommand
            {
                RequestObject = requestObject
                ,
                CommandFunction = o =>
                    {
                        try
                        {
                            // do some work

                            var rv = "Did some fake work!";

                            rv = String.Format("hello {0} from WriteToFileGenericCommand!", o.RequestData);
                            return rv;
                            //return o.ToSuccessfullGenericResponse(rv, SuccessResponseCode, o.CorrelationId);
                            //return new ResponseObject<string, string>()
                            //{
                            //    CorrelationId = "11111",
                            //    ExceptionList = null,
                            //    RequestData = o.RequestData,
                            //    ResponseCode = new ResponseCode(200, "OK"),
                            //    ResponseData = rv
                            //};
                        }
                        catch (Exception ex)
                        {
                            throw;
                            //return o.ToFailedResponse(ExceptionObjectListBase.Create(
                            //    ex
                            //    , "Some general Exception Title Goes HERE!"
                            //    , "A broader description could go HERE!"
                            //    , "If I had any Grouping I could specify HERE!"
                            //    , ExceptionLogLevelType.Debug
                            //    )
                            //    , ErrorResponseCode
                            //    , o.CorrelationId
                            //    );
                        }
                    }
            };
        }
    }

    //public class WriteToFileCommand : ICommandObject, ICommandObjectFactory
    //{
    //    public IRequestObject RequestObject { get; set; }

    //    private string _commandName;
    //    public virtual string CommandName
    //    {
    //        get
    //        {
    //            return String.IsNullOrWhiteSpace(_commandName) ? this.GetType().Name() : _commandName;
    //        }
    //        set { _commandName = value; }
    //    }

    //    public string Description
    //    {
    //        get { return "WriteToFile"; }
    //    }

    //    public IResponseObject Execute()
    //    {
    //        // if we needed to short circuit here because of some condition we could do it here!
    //        //var rv = RequestObject.ToUncompletedResponse(null, RequestObject.CorrelationId);
    //        try
    //        {
    //            // do some work

    //            var rv = "Did some fake work!";
    //            var responseCode = "200";

    //            return RequestObject.ToSuccessfullResponse(rv, responseCode, RequestObject.CorrelationId);
    //        }
    //        catch (Exception ex)
    //        {
    //            return RequestObject.ToFailedResponse(ExceptionObjectListBase.Create(
    //                ex
    //                , "Some general Exception Title Goes HERE!"
    //                , "A broader description could go HERE!"
    //                , "If I had any Grouping I could specify HERE!"
    //                , ExceptionLogLevelType.Debug
    //                )
    //                , "500"
    //                , RequestObject.CorrelationId
    //                );
    //        }
    //    }

    //    public ICommandObject Create(IRequestObject requestObject)
    //    {
    //        return new WriteToFileCommand { RequestObject = requestObject };
    //    }
    //}
}