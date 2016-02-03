using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Business.Common.IO;
using Business.Common.IO.Serialization;
using Business.Common.System.Args;

// ReSharper disable once CheckNamespace
namespace ExBaseArguments
{
    /// <summary>
    /// Extension method helper class.
    /// </summary>
    public static class ArgumentListExtensionBase
    {
        #region ArgumentList File Serialization Extensions

        public static FileInfo WriteXml(this ArgumentList value,
            String filePath,
            Int32 lockWaitMs = 60000,
            IoRollbackType rollbackType = IoRollbackType.None,
            XmlWriterSettings settings = null,
            Encoding encodingType = null)
        {
            return value.Serialize(filePath, lockWaitMs, rollbackType, settings, encodingType, new List<Type> { typeof(ArgumentList), typeof(Arguments) });
        }

        public static FileInfo WriteXml(this ArgumentList value,
            FileInfo fileInfo,
            Int32 lockWaitMs = 60000,
            IoRollbackType rollbackType = IoRollbackType.None,
            XmlWriterSettings settings = null,
            Encoding encodingType = null)
        {
            return value.Serialize(fileInfo, lockWaitMs, rollbackType, settings, encodingType, new List<Type> { typeof(ArgumentList), typeof(Arguments) });
        }

        #endregion ArgumentList File Serialization Extensions

        #region ArgumentList to Byte[] Serialization Extensions

        public static Byte[] WriteXmlToBytes(this ArgumentList value,
        XmlWriterSettings settings = null,
        Encoding encodingType = null)
        {
            return value.SerializeToBytes(settings, encodingType, new List<Type> { typeof(ArgumentList), typeof(Arguments) });
        }

        #endregion ArgumentList to Byte[] Serialization Extensions

        #region ArgumentList String Serialization Extensions

        public static String WriteXmlToString(this ArgumentList value,
        XmlWriterSettings settings = null,
        Encoding encodingType = null)
        {
            return value.SerializeToString(settings, encodingType, new List<Type> { typeof(ArgumentList), typeof(Arguments) });
        }

        #endregion ArgumentList String Serialization Extensions

        #region ArgumentList XElement Serialization Extensions

        public static XElement WriteXmlToXElement(this ArgumentList value,
        XmlWriterSettings settings = null,
        Encoding encodingType = null)
        {
            return value.SerializeToXElement(settings, encodingType, new List<Type> { typeof(ArgumentList), typeof(Arguments) });
        }

        #endregion ArgumentList XElement Serialization Extensions

        #region ArgumentList File GZip Serialization Extensions

        public static FileInfo WriteGZipXml(this ArgumentList value,
            String filePath,
            Int32 lockWaitMs = 60000,
            IoRollbackType rollbackType = IoRollbackType.None,
            XmlWriterSettings settings = null,
            Encoding encodingType = null)
        {
            return value.GZipSerialize(filePath, lockWaitMs, rollbackType, settings, encodingType, new List<Type> { typeof(ArgumentList), typeof(Arguments) });
        }

        public static FileInfo WriteGZipXml(this ArgumentList value,
            FileInfo fileInfo,
            Int32 lockWaitMs = 60000,
            IoRollbackType rollbackType = IoRollbackType.None,
            XmlWriterSettings settings = null,
            Encoding encodingType = null)
        {
            return value.GZipSerialize(fileInfo, lockWaitMs, rollbackType, settings, encodingType, new List<Type> { typeof(ArgumentList), typeof(Arguments) });
        }

        #endregion ArgumentList File GZip Serialization Extensions

        #region ArgumentList Base64String GZipSerialization Extensions

        public static String WriteGZipXmlToBase64String(this ArgumentList value,
        XmlWriterSettings settings = null,
        Encoding encodingType = null)
        {
            return value.GZipSerializeToBase64String(settings, encodingType, new List<Type> { typeof(ArgumentList), typeof(Arguments) });
        }

        #endregion ArgumentList Base64String GZipSerialization Extensions

        public static List<dynamic> ToExpandoList(this ArgumentList argList)
        {
            return argList.Items.Select(arg => arg.ToExpando()).ToList();
        }
    }
}