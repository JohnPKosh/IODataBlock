using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Business.Common.Requests;
using Business.Common.System;

namespace Business.Test.TestUtility
{
    public class ReadFromFileCommand: CommandObjectBase
    {
        public override string CommandName
        {
            get { return "ReadFromFile"; }
        }

        public override string Description
        {
            get { return "ReadFromFile Test"; }
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
                    // temporarily adding exception
                    // ReSharper disable once ConvertToConstant.Local
                    var my0 = 0;
                    // ReSharper disable once UnusedVariable
                    var db0 = 1 / my0;


                    // add a command here!
                    if (o.RequestData is string)
                    {
                        return String.Format("hello {0} from ReadFromFileCommand!", o.RequestData);
                    }
                    return "hello from ReadFromFileCommand!";
                }
            
            };
        }
    }
}
