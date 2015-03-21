﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Business.Common.Requests;
using Business.Common.System;

namespace Business.Test.TestUtility
{
    public class WriteToJsonFileCommand : CommandObjectBase
    {
        public override string CommandName
        {
            get { return "WriteToJsonFile"; }
        }

        public override string Description
        {
            get { return "WriteToJsonFile - reads object from json file."; }
        }

        public override object SuccessResponseCode
        {
            get { return "200"; }
        }

        public override object ErrorResponseCode
        {
            get { return "500"; }
        }

        public override Func<IRequestObject, object> CommandFunction { get; set; }

        public override ICommandObject Create(IRequestObject requestObject)
        {
            return new ReadFromFileCommand()
            {
                RequestObject = requestObject
                ,
                CommandFunction = o =>
                {
                    // add a command here!
                    if (o.RequestData is string)
                    {
                        return String.Format("hello {0} from WriteToJsonFile!", o.RequestData);
                    }
                    return "hello from WriteToJsonFile!";
                }
            
            };
        }
    }
}
