using System;
using Business.Common.Requests;
using Business.Common.System;

namespace Business.Test.TestUtility
{

    public class WriteToFileCommand : CommandObjectBase
    {
        public override string Description
        {
            get { return "WriteToFile - Writes to File."; }
        }

        public override Func<IRequestObject, object> CommandFunction { get; set; }

        public override ICommandObject Create(IRequestObject requestObject)
        {
            return new WriteToFileCommand
            {
                RequestObject = requestObject
                ,
                CommandFunction = o =>
                    {
                        return "Did some fake work!";

                        //// if we needed to short circuit here because of some condition we could do it here!
                        ////var rv = RequestObject.ToUncompletedResponse(null, RequestObject.CorrelationId);
                        //try
                        //{
                        //    // do some work

                        //    var rv = "Did some fake work!";
                        //    //var responseCode = new ResponseCode(200,@"OK");

                        //    return requestObject.ToSuccessfullResponse(rv, SuccessResponseCode, requestObject.CorrelationId);
                        //}
                        //catch (Exception ex)
                        //{
                        //    return requestObject.ToFailedResponse(ExceptionObjectListBase.Create(
                        //        ex
                        //        , "Some general Exception Title Goes HERE!"
                        //        , "A broader description could go HERE!"
                        //        , "If I had any Grouping I could specify HERE!"
                        //        , ExceptionLogLevelType.Debug
                        //        )
                        //        , ErrorResponseCode
                        //        , requestObject.CorrelationId
                        //        );
                        //}
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