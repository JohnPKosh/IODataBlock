using System;
using System.Collections.Generic;
using System.Linq;
using Business.Common.Exceptions;
using Business.Common.Requests;
using Business.Common.Responses;

namespace Business.Common.System
{
    public class CommandCollectionParser<TIn, TOut> : ICommandCollectionParser<TIn, TOut>
    {
        #region Class Initialization

        public CommandCollectionParser(Dictionary<string, IEnumerable<ICommandObject<TIn, TOut>>> commandObjectDictionary)
        {
            _commandObjectDictionary = commandObjectDictionary;
        }

        #endregion Class Initialization

        #region Fields and Properties

        private readonly Dictionary<string, IEnumerable<ICommandObject<TIn, TOut>>> _commandObjectDictionary;

        #endregion Fields and Properties

        #region Parse Methods

        public ICommandObject<TIn, TOut> Parse(string collectionName, IRequestObject<TIn> requestObject)
        {
            try
            {
                var command = Find(collectionName, requestObject.CommandName);
                return ((ICommandObjectFactory<TIn, TOut>)command).Create(requestObject);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ICommandObject<TIn, TOut> Parse(string collectionName, string commandName, IRequestObject<TIn> requestObject)
        {
            var command = Find(collectionName, commandName);
            return ((ICommandObjectFactory<TIn, TOut>)command).Create(requestObject);
        }


        public bool TryParse(string collectionName, IRequestObject<TIn> requestObject, out ICommandObject<TIn, TOut> commandObject)
        {
            try
            {
                commandObject = Parse(collectionName, requestObject);
                return true;
            }
            catch (Exception)
            {
                commandObject = null;
                return false;
            }
        }

        public bool TryParse(string collectionName, string commandName, IRequestObject<TIn> requestObject, out ICommandObject<TIn, TOut> commandObject)
        {
            try
            {
                commandObject = Parse(collectionName, commandName, requestObject);
                return true;
            }
            catch (Exception)
            {
                commandObject = null;
                return false;
            }
        }

        #endregion Parse Methods

        #region Execute Methods

        public IResponseObject<TIn, TOut> Execute(string collectionName, IRequestObject<TIn> requestObject)
        {
            // TODO: Add overload with logger and validation functionality. Add TryExecute and/or Exception for Command not found scenario.
            // TODO: decide if logger functionality should be injected here!
            try
            {
                var command = Find(collectionName, requestObject.CommandName);
                return ((ICommandObjectFactory<TIn, TOut>)command).Create(requestObject).Execute();
            }
            catch (Exception ex)
            {
                return requestObject.RequestData.ToFailedGenericResponse<TIn, TOut>(default(TOut), ExceptionObjectListBase.Create(ex), new ResponseCode(500, @"500 Internal Server Error"), requestObject.CorrelationId);
            }
        }

        public IResponseObject<TIn, TOut> Execute(string collectionName, string commandName, TIn requestData, string correlationId = null)
        {
            // TODO: Add overload with logger and validation functionality. Add TryExecute and/or Exception for Command not found scenario.
            var requestObject = new RequestObject<TIn>
            {
                CommandName = commandName,
                RequestData = requestData,
                CorrelationId = correlationId ?? Guid.NewGuid().ToString()
            };
            return Execute(collectionName, requestObject);
            // TODO: decide if logger functionality should be injected here!
        }

        #endregion Execute Methods

        #region private Utility methods

        private ICommandObject<TIn, TOut> Find(string collectionName, string commandName)
        {
            try
            {
                var commands = _commandObjectDictionary[collectionName];
                return commands.First(c => c.CommandName == String.Format(@"{0}Command", commandName));
            }
            catch (Exception)
            {
                throw new CommandNameNotFoundException(@"CommandNameNotFoundException occurred in the command parser. {0} Command Name not found in the {1} command collection!", commandName, collectionName);
            }
        }

        #endregion private Utility methods
    }
}