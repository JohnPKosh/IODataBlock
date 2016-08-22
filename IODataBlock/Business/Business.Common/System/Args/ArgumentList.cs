using Business.Common.IO.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Business.Common.System.Args
{
    [DataContract(Namespace = @"http://www.broadvox.com/Args/", Name = "ArgList")]
    [KnownType(typeof(Arguments))]
    public class ArgumentList //: List<Arguments>
    {
        public ArgumentList()
        {
            Items = new List<Arguments>();
        }

        /// <summary>
        /// public read only Arguments indexer
        /// </summary>
        /// <param name="k">Key</param>
        /// <param name="v">Value</param>
        /// <returns>IEnumerable</returns>
        public IEnumerable<Arguments> this[string k, string v]
        {
            get
            {
                return Items.Where(arg => arg.ContainsKey(k) && arg[k] == v);
            }
        }

        [DataMember(Name = "AL")]
        public List<Arguments> Items { get; set; }

        public void Add(Arguments value)
        {
            Items.Add(value);
        }

        #region Read String ArgumentList Methods

        public static ArgumentList ReadStringCollectionFromFile(string collectionFilePath)
        {
            return ReadStringCollectionFromFile(new FileInfo(collectionFilePath));
        }

        public static ArgumentList ReadStringCollectionFromFile(FileInfo collectionFileInfo)
        {
            if (!collectionFileInfo.Exists)
                throw new FileNotFoundException(
                    "Arguments.LoadStringArgumentsFromFile: FileNotFoundException - The file was not found!");
            FileStream fs = null;
            try
            {
                fs = new FileStream(collectionFileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
                using (var sr = new StreamReader(fs))
                {
                    var ac = new ArgumentList();
                    while (!sr.EndOfStream)
                    {
                        var readLine = sr.ReadLine();
                        if (readLine != null)
                            ac.Items.Add(Arguments.CreateArguments(readLine.Trim()));
                    }
                    return ac;
                }
            }
            finally
            {
                fs?.Dispose();
            }
        }

        public static ArgumentList ReadStringCollection(string lineArguments)
        {
            using (var sr = new StringReader(lineArguments))
            {
                var ac = new ArgumentList();
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    ac.Items.Add(Arguments.CreateArguments(line.Trim()));
                }
                return ac;
            }
        }

        public static ArgumentList ReadCollectionFromFile(string collectionFilePath)
        {
            return new ArgumentList().Deserialize(new FileInfo(collectionFilePath));
        }

        public static ArgumentList ReadCollectionFromFile(FileInfo collectionFileInfo)
        {
            return new ArgumentList().Deserialize(collectionFileInfo);
        }

        #endregion Read String ArgumentList Methods

        #region Expando To Argument List Conversion

        public static ArgumentList ExpandoToArgumentList(IEnumerable<dynamic> nodes)
        {
            var rv = new ArgumentList();
            foreach (var node in nodes)
            {
                var arg = Arguments.ExpandoToArguments(node);
                rv.Items.Add(arg);
            }
            return rv;
        }

        #endregion Expando To Argument List Conversion

        #region File to ArgumentList Deserialization Extensions

        public static ArgumentList ReadXml(FileInfo fileInfo,
        int lockWaitMs = 60000,
        XmlReaderSettings settings = null,
        IEnumerable<Type> knownTypes = null)
        {
            return new ArgumentList().Deserialize(fileInfo, lockWaitMs, settings, new List<Type> { typeof(ArgumentList), typeof(Arguments) });
        }

        public static ArgumentList ReadXml(string filePath,
        int lockWaitMs = 60000,
        XmlReaderSettings settings = null)
        {
            return new ArgumentList().Deserialize(filePath, lockWaitMs, settings, new List<Type> { typeof(ArgumentList), typeof(Arguments) });
        }

        //public static FileInfo AddToXml(Arguments ArgumentsToAdd,
        //String FilePath,
        //Int32 LockWaitMs = 60000,
        //IoRollbackType RollbackType = IoRollbackType.none,
        //XmlWriterSettings WriteSettings = null,
        //XmlReaderSettings ReadSettings = null,
        //Encoding EncodingType = null)
        //{
        //    var args = ReadXml(FilePath, LockWaitMs, ReadSettings);
        //    args.Add(ArgumentsToAdd);
        //    return args.WriteXml(FilePath, LockWaitMs, RollbackType, WriteSettings, EncodingType);
        //}

        //public static FileInfo AddRangeToXml(IEnumerable<Arguments> ArgumentsToAdd,
        //String FilePath,
        //Int32 LockWaitMs = 60000,
        //IoRollbackType RollbackType = IoRollbackType.none,
        //XmlWriterSettings WriteSettings = null,
        //XmlReaderSettings ReadSettings = null,
        //Encoding EncodingType = null)
        //{
        //    var args = ReadXml(FilePath, LockWaitMs, ReadSettings);
        //    args.AddRange(ArgumentsToAdd);
        //    return args.WriteXml(FilePath, LockWaitMs, RollbackType, WriteSettings, EncodingType);
        //}

        //public static FileInfo InsertToXml(Int32 index,
        //Arguments ArgumentsToInsert,
        //String FilePath,
        //Int32 LockWaitMs = 60000,
        //IoRollbackType RollbackType = IoRollbackType.none,
        //XmlWriterSettings WriteSettings = null,
        //XmlReaderSettings ReadSettings = null,
        //Encoding EncodingType = null)
        //{
        //    var args = ReadXml(FilePath, LockWaitMs, ReadSettings);
        //    args.Insert(index, ArgumentsToInsert);
        //    return args.WriteXml(FilePath, LockWaitMs, RollbackType, WriteSettings, EncodingType);
        //}

        //public static FileInfo InsertRangeToXml(Int32 index,
        //IEnumerable<Arguments> ArgumentsToInsert,
        //String FilePath,
        //Int32 LockWaitMs = 60000,
        //IoRollbackType RollbackType = IoRollbackType.none,
        //XmlWriterSettings WriteSettings = null,
        //XmlReaderSettings ReadSettings = null,
        //Encoding EncodingType = null)
        //{
        //    var args = ReadXml(FilePath, LockWaitMs, ReadSettings);
        //    args.InsertRange(index, ArgumentsToInsert);
        //    return args.WriteXml(FilePath, LockWaitMs, RollbackType, WriteSettings, EncodingType);
        //}

        //public static FileInfo RemoveAtInXml(Int32 index,
        //String FilePath,
        //Int32 LockWaitMs = 60000,
        //IoRollbackType RollbackType = IoRollbackType.none,
        //XmlWriterSettings WriteSettings = null,
        //XmlReaderSettings ReadSettings = null,
        //Encoding EncodingType = null)
        //{
        //    var args = ReadXml(FilePath, LockWaitMs, ReadSettings);
        //    args.RemoveAt(index);
        //    return args.WriteXml(FilePath, LockWaitMs, RollbackType, WriteSettings, EncodingType);
        //}

        //public static FileInfo RemoveRangeInXml(Int32 index,
        //Int32 count,
        //String FilePath,
        //Int32 LockWaitMs = 60000,
        //IoRollbackType RollbackType = IoRollbackType.none,
        //XmlWriterSettings WriteSettings = null,
        //XmlReaderSettings ReadSettings = null,
        //Encoding EncodingType = null)
        //{
        //    var args = ReadXml(FilePath, LockWaitMs, ReadSettings);
        //    args.RemoveRange(index, count);
        //    return args.WriteXml(FilePath, LockWaitMs, RollbackType, WriteSettings, EncodingType);
        //}

        //public static Boolean ExistsInXml(string filePath,
        //Predicate<Arguments> match,
        //Int32 LockWaitMs = 60000,
        //XmlReaderSettings settings = null)
        //{
        //    var args = new ArgumentList().Deserialize(filePath, LockWaitMs, settings, new List<Type>() { typeof(ArgumentList), typeof(Arguments) });
        //    return args.Exists(match);
        //}

        #endregion File to ArgumentList Deserialization Extensions

        #region Byte[] to ArgumentList Deserialization Extensions

        public static ArgumentList ReadXmlFromBytes(byte[] data,
        XmlReaderSettings settings = null,
        Encoding encodingType = null)
        {
            return data.DeserializeBytes<ArgumentList>(settings, encodingType, new List<Type> { typeof(ArgumentList), typeof(Arguments) });
        }

        #endregion Byte[] to ArgumentList Deserialization Extensions

        #region String to ArgumentList Deserialization Extensions

        public static ArgumentList ReadXmlFromString(string data,
        XmlReaderSettings settings = null)
        {
            return data.DeserializeString<ArgumentList>(settings, new List<Type> { typeof(ArgumentList), typeof(Arguments) });
        }

        #endregion String to ArgumentList Deserialization Extensions

        #region XElement to ArgumentList Deserialization Extensions

        public static ArgumentList ReadXmlFromXElement(XElement data,
        XmlReaderSettings settings = null)
        {
            return data.Deserialize<ArgumentList>(settings, new List<Type> { typeof(ArgumentList), typeof(Arguments) });
        }

        #endregion XElement to ArgumentList Deserialization Extensions

        #region File to ArgumentList GZipDeserialization Extensions

        public static ArgumentList ReadGZipXml(FileInfo fileInfo,
        int lockWaitMs = 60000,
        XmlReaderSettings settings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null)
        {
            return new ArgumentList().GZipDeserialize(fileInfo, lockWaitMs, settings, encodingType, new List<Type> { typeof(ArgumentList), typeof(Arguments) });
        }

        public static ArgumentList ReadGZipXml(string filePath,
        int lockWaitMs = 60000,
        XmlReaderSettings settings = null,
        Encoding encodingType = null)
        {
            return new ArgumentList().GZipDeserialize(filePath, lockWaitMs, settings, encodingType, new List<Type> { typeof(ArgumentList), typeof(Arguments) });
        }

        #endregion File to ArgumentList GZipDeserialization Extensions

        #region Base64String GZipDeserialization Extensions

        public static ArgumentList ReadGZipXmlFromStringBase64String(string value,
        XmlReaderSettings settings = null,
        Encoding encodingType = null)
        {
            return value.GZipDeserializeFromBase64String<ArgumentList>(settings, encodingType, new List<Type> { typeof(ArgumentList), typeof(Arguments) });
        }

        #endregion Base64String GZipDeserialization Extensions
    }
}