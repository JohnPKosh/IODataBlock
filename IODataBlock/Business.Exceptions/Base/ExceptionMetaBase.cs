﻿using System;
using System.Reflection;
using Business.Exceptions.Interfaces;
using Business.Utilities;

namespace Business.Exceptions.Base
{
    public class ExceptionMetaBase : IExceptionMeta
    {
        public DateTime DateCreatedUtc { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ExceptionGroup { get; set; }

        public string HostComputerName { get; set; }

        public string HostUserName { get; set; }

        public string HostUserDomain { get; set; }

        public string ExecutingAssemblyFullName { get; set; }

        public string CallingAssemblyFullName{ get; set; }

        public string EntryAssemblyFullName { get; set; }

        public string TypeName { get; set; }

        public string MemberName { get; set; }

        public string ParentName { get; set; }

        public static IExceptionMeta CreateExceptionMeta(
            string title = null
            , string description = null
            , string exceptionGroup = null
            , string hostComputerName = null
            , string hostUserName = null
            , string hostUserDomain = null
            , string executingAssemblyFullName = null
            , string callingAssemblyFullName = null
            , string entryAssemblyFullName = null
            , string typeName = null
            , string memberName = null
            , string parentName = null
            )
        {
            var meta = new ExceptionMetaBase
            {
                DateCreatedUtc = DateTime.UtcNow,
                Title = title,
                Description = description,
                ExceptionGroup = exceptionGroup,
                HostComputerName = hostComputerName ?? EnvironmentUtilities.GetComputerName(),
                HostUserName = hostUserName ?? EnvironmentUtilities.GetUserName(),
                HostUserDomain = hostUserDomain ?? EnvironmentUtilities.GetUserDomain(),
                ExecutingAssemblyFullName = executingAssemblyFullName,
                CallingAssemblyFullName = callingAssemblyFullName,
                EntryAssemblyFullName = entryAssemblyFullName,
                TypeName = typeName,
                MemberName = memberName,
                ParentName = parentName
            };
            return meta;
        }

    }
}