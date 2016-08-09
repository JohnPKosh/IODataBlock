using System.IO;
using Business.Test.TestUtility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Business.Common.Extensions;

namespace Business.Test.IO
{
    [TestClass]
    public class BsonFileTests
    {
        [TestMethod]
        public void CanSavePetToBsonFile()
        {
            var pet = FakePet.CreateBela();
            pet.WriteBsonToFilePath(@"C:\junk\testbson.bson");
            Assert.IsTrue(File.Exists(@"C:\junk\testbson.bson"));
        }

        [TestMethod]
        public void CanReadPetFromBsonFile()
        {
            FakePet bela;
            using (var fs = File.OpenRead(@"C:\junk\testbson.bson"))
            {
                bela = fs.BsonDeserialize<FakePet>();
            }
            Assert.IsNotNull(bela);
        }

        [TestMethod]
        public void CanReadPetFromBsonFileInfo()
        {
            FakePet bela;
            var file = new FileInfo(@"C:\junk\testbson.bson");
            bela = file.BsonDeserialize<FakePet>();
            Assert.IsNotNull(bela);
        }
    }
}
