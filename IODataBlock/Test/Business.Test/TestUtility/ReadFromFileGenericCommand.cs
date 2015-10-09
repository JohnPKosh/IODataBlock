using System;
using Business.Common.Exceptions;
using Business.Common.Requests;
using Business.Common.Responses;
using Business.Common.System;

namespace Business.Test.TestUtility
{
    public class ReadFromFileGenericCommand : CommandObjectBase<string, string>
    {
        public override string Description
        {
            get { return "ReadFromFile - Writes to File."; }
        }

        public override Func<IRequestObject<string>, string> CommandFunction { get; set; }

        public override ICommandObject<string, string> Create(IRequestObject<string> requestObject)
        {
            return new ReadFromFileGenericCommand
            {
                RequestObject = requestObject
                ,
                CommandFunction = o =>
                {
                    // if we needed to short circuit here because of some condition we could do it here!
                    //var rv = RequestObject.ToUncompletedResponse(null, RequestObject.CorrelationId);
                    try
                    {
                        // do some work

                        var rv = "Did some fake work!";

                        rv = String.Format("hello {0} from ReadFromFileGenericCommand!", o.RequestData);
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


    //public class ReadFromFileCommand : CommandObjectBase
    //{
    //    //public override string CommandName
    //    //{
    //    //    get { return "ReadFromFile"; }
    //    //}

    //    public override string Description
    //    {
    //        get { return "ReadFromFile Test"; }
    //    }

    //    public override object SuccessResponseCode
    //    {
    //        get { return "200"; }
    //    }

    //    public override object ErrorResponseCode
    //    {
    //        get { return "500"; }
    //    }

    //    public override Func<IRequestObject, object> CommandFunction { get; set; }

    //    public override ICommandObject Create(IRequestObject requestObject)
    //    {
    //        return new ReadFromFileCommand
    //        {
    //            RequestObject = requestObject
    //            ,
    //            CommandFunction = o =>
    //            {
    //                // temporarily adding exception
    //                // ReSharper disable once ConvertToConstant.Local
    //                var my0 = 0;
    //                // ReSharper disable once UnusedVariable
    //                var db0 = 1 / my0;

    //                // add a command here!
    //                if (o.RequestData is string)
    //                {
    //                    return String.Format("hello {0} from ReadFromFileCommand!", o.RequestData);
    //                }
    //                return "hello from ReadFromFileCommand!";
    //            }
    //        };
    //    }
    //}
}