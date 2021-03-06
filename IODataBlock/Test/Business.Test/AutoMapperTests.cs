﻿using AutoMapper;
using Business.Common.Extensions;
using Business.Test.TestUtility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Business.Test
{
    [TestClass]
    public class AutoMapperTests
    {
        [TestMethod]
        public void DynamicPetToFakePet()
        {
            var fido = FakePet.CreateBela();
            dynamic dynfido = fido.ToExpando();
            var dfido = (IDictionary<string, object>)dynfido;
            Assert.IsNotNull(dfido);

            //Mapper.CreateMap<IDictionary<String, Object>, FakePet>();
            Mapper.Initialize(cfg => {});
            var newfido = Mapper.Map<FakePet>(dynfido);

            Assert.IsNotNull(newfido);
        }
    }
}