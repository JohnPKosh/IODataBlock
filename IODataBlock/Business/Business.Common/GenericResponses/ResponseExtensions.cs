﻿using System;
using Business.Common.Exceptions;
using Business.Common.Responses;

namespace Business.Common.GenericResponses
{
    public static class GenericResponseExtensions
    {
        public static IResponseObject<TIn, TOut> ToUncompletedGenericResponse<TIn, TOut>(this TIn requestData, int? id, string code, string correlationId = null)
        {
            ResponseCode rc;
            if (!id.HasValue && String.IsNullOrWhiteSpace(code)) rc = null;
            else rc = new ResponseCode(id,code);
            return new ResponseObject<TIn, TOut> { RequestData = requestData, ResponseCode = rc, CorrelationId = correlationId };
        }

        public static IResponseObject<TIn, TOut> ToUncompletedGenericResponse<TIn, TOut>(this TIn requestData, IResponseCode responseCode = null, string correlationId = null)
        {
            return new ResponseObject<TIn, TOut> { RequestData = requestData, ResponseCode = responseCode ?? new ResponseCode(400, @"400 Bad Request"), CorrelationId = correlationId };
        }

        public static IResponseObject<TIn, TOut> ToSuccessfullGenericResponse<TIn, TOut>(this TIn requestData, TOut responseData, IResponseCode responseCode = null, string correlationId = null)
        {
            return new ResponseObject<TIn, TOut> { RequestData = requestData, ResponseData = responseData, ResponseCode = responseCode, CorrelationId = correlationId };
        }

        public static IResponseObject<TIn, TOut> ToFailedGenericResponse<TIn, TOut>(this TIn requestData, TOut responseData, IExceptionObjectList exceptionObjectList, IResponseCode responseCode = null, string correlationId = null)
        {
            return new ResponseObject<TIn, TOut> { RequestData = requestData, ResponseCode = responseCode, CorrelationId = correlationId, ExceptionList = exceptionObjectList, ResponseData = responseData };
        }
    }
}