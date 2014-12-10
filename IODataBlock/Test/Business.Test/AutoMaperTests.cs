using System;
using System.Collections.Generic;
using System.IO;
using Business.Test.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using Business.Common.Extensions;

namespace Business.Test
{
    [TestClass]
    public class AutoMaperTests
    {
        [TestMethod]
        public void DynamicPetToFakePet()
        {

            var fido = FakePet.CreateBela();
            dynamic dynfido = fido.ToExpando();
            var dfido = (IDictionary<string, object>)dynfido;

            Mapper.CreateMap<IDictionary<String, Object>, FakePet>();
            var newfido = Mapper.DynamicMap<FakePet>(dynfido);


            Assert.IsNotNull(newfido);

        }
    }
}
