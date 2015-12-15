using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Common.Responses;

namespace HubSpot.Services
{
    public interface IFormsService
    {

        IResponseObject<string, string> GetForms();


    }
}
