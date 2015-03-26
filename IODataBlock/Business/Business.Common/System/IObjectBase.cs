﻿using System;
using Newtonsoft.Json;

namespace Business.Common.System
{
    public interface IObjectBase
    {
        String ToJson(Boolean indented = false);

        void PopulateFromJson(String value);

        void PopulateFromJson(string value, object target, JsonSerializerSettings settings);
    }

    //public interface IObjectBase<T>
    //{
    //    String ToJson(Boolean indented = false);

    //    void PopulateFromJson(String value);

    //    void PopulateFromJson(string value, object target, JsonSerializerSettings settings);
    //}
}