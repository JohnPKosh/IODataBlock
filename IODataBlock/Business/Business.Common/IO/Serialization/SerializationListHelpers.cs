﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Business.Common.IO.Serialization
{
    public static class SerializationListHelpers
    {
        // ReSharper disable InconsistentNaming

        #region List Xml filePath Helper Extension Methods

        public static FileInfo AddToSerializedXml<T, K>(this T obj,
        K objToAdd,
        String filePath,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = new FileInfo(filePath).Deserialize<T>(lockWaitMs, readSettings, typs);
            args.Add(objToAdd);
            return args.Serialize(filePath, lockWaitMs, rollbackType, writeSettings, encodingType, typs);
        }

        public static FileInfo AddRangeToSerializedXml<T, K>(this T obj,
        IEnumerable<K> objToAdd,
        String filePath,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = new FileInfo(filePath).Deserialize<T>(lockWaitMs, readSettings, typs);
            args.AddRange(objToAdd);
            return args.Serialize(filePath, lockWaitMs, rollbackType, writeSettings, encodingType, typs);
        }

        public static Int32 CountInSerializedXml<T, K>(this T obj,
        String filePath,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlReaderSettings readSettings = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = new FileInfo(filePath).Deserialize<T>(lockWaitMs, readSettings, typs);
            return args.Count;
        }

        public static Boolean ExistsInSerializedXml<T, K>(this T obj,
        Predicate<K> match,
        String filePath,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlReaderSettings readSettings = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = new FileInfo(filePath).Deserialize<T>(lockWaitMs, readSettings, typs);
            return args.Exists(match);
        }

        public static K FindInSerializedXml<T, K>(this T obj,
        Predicate<K> match,
        String filePath,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlReaderSettings readSettings = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = new FileInfo(filePath).Deserialize<T>(lockWaitMs, readSettings, typs);
            return args.Find(match);
        }

        public static List<K> FindAllInSerializedXml<T, K>(this T obj,
        Predicate<K> match,
        String filePath,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlReaderSettings readSettings = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = new FileInfo(filePath).Deserialize<T>(lockWaitMs, readSettings, typs);
            return args.FindAll(match);
        }

        public static Int32 FindIndexInSerializedXml<T, K>(this T obj,
        Predicate<K> match,
        String filePath,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlReaderSettings readSettings = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = new FileInfo(filePath).Deserialize<T>(lockWaitMs, readSettings, typs);
            return args.FindIndex(match);
        }

        public static Int32 FindIndexInSerializedXml<T, K>(this T obj,
        Int32 startIndex,
        Predicate<K> match,
        String filePath,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlReaderSettings readSettings = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = new FileInfo(filePath).Deserialize<T>(lockWaitMs, readSettings, typs);
            return args.FindIndex(startIndex, match);
        }

        public static Int32 FindIndexInSerializedXml<T, K>(this T obj,
        Int32 startIndex,
        int count,
        Predicate<K> match,
        String filePath,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlReaderSettings readSettings = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = new FileInfo(filePath).Deserialize<T>(lockWaitMs, readSettings, typs);
            return args.FindIndex(startIndex, count, match);
        }

        public static K FindLastInSerializedXml<T, K>(this T obj,
        Predicate<K> match,
        String filePath,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlReaderSettings readSettings = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = new FileInfo(filePath).Deserialize<T>(lockWaitMs, readSettings, typs);
            return args.FindLast(match);
        }

        public static Int32 FindLastIndexInSerializedXml<T, K>(this T obj,
        Predicate<K> match,
        String filePath,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlReaderSettings readSettings = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = new FileInfo(filePath).Deserialize<T>(lockWaitMs, readSettings, typs);
            return args.FindLastIndex(match);
        }

        public static Int32 FindLastIndexInSerializedXml<T, K>(this T obj,
        Int32 startIndex,
        Predicate<K> match,
        String filePath,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlReaderSettings readSettings = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = new FileInfo(filePath).Deserialize<T>(lockWaitMs, readSettings, typs);
            return args.FindLastIndex(startIndex, match);
        }

        public static Int32 FindLastIndexInSerializedXml<T, K>(this T obj,
        Int32 startIndex,
        int count,
        Predicate<K> match,
        String filePath,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlReaderSettings readSettings = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = new FileInfo(filePath).Deserialize<T>(lockWaitMs, readSettings, typs);
            return args.FindLastIndex(startIndex, count, match);
        }

        public static FileInfo ForEachInSerializedXml<T, K>(this T obj,
        Action<K> action,
        String filePath,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = new FileInfo(filePath).Deserialize<T>(lockWaitMs, readSettings, typs);
            args.Reverse();
            return args.Serialize(filePath, lockWaitMs, rollbackType, writeSettings, encodingType, typs);
        }

        public static List<K> GetRangeInSerializedXml<T, K>(this T obj,
        int index,
        int count,
        String filePath,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlReaderSettings readSettings = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = new FileInfo(filePath).Deserialize<T>(lockWaitMs, readSettings, typs);
            return args.GetRange(index, count);
        }

        public static FileInfo InsertToSerializedXml<T, K>(this T obj,
        Int32 index,
        K objToAdd,
        String filePath,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = new FileInfo(filePath).Deserialize<T>(lockWaitMs, readSettings, typs);
            args.Insert(index, objToAdd);
            return args.Serialize(filePath, lockWaitMs, rollbackType, writeSettings, encodingType, typs);
        }

        public static FileInfo InsertRangeToSerializedXml<T, K>(this T obj,
        Int32 index,
        IEnumerable<K> objToAdd,
        String filePath,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = new FileInfo(filePath).Deserialize<T>(lockWaitMs, readSettings, typs);
            args.InsertRange(index, objToAdd);
            return args.Serialize(filePath, lockWaitMs, rollbackType, writeSettings, encodingType, typs);
        }

        //public static FileInfo OrderByInSerializedXml<T, K, TKey>(this T obj,
        //Func<K, TKey> keySelector,
        //String filePath,
        //Int32 lockWaitMs = 60000,
        //IoRollbackType rollbackType = IoRollbackType.none,
        //XmlWriterSettings writeSettings = null,
        //XmlReaderSettings readSettings = null,
        //Encoding encodingType = null,
        //IEnumerable<Type> knownTypes = null) where T : List<K>
        //{
        //    var typs = new List<Type>() { typeof(T), typeof(K) };
        //    if (knownTypes != null) typs.AddRange(knownTypes);
        //    T args = new FileInfo(filePath).Deserialize<T>(lockWaitMs, readSettings, typs);
        //    List<K> sorted = args.OrderBy(keySelector).ToList();
        //    args.Clear();
        //    args.AddRange(sorted);
        //    return args.Serialize<T>(filePath, lockWaitMs, rollbackType, writeSettings, encodingType, typs);
        //}

        public static FileInfo RemoveAnyInSerializedXml<T, K>(this T obj,
        Func<K, bool> predicate,
        String filePath,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = new FileInfo(filePath).Deserialize<T>(lockWaitMs, readSettings, typs);
            var removes = new List<K>(args.Where(predicate));
            foreach (var match in removes)
            {
                args.Remove(match);
            }
            return args.Serialize(filePath, lockWaitMs, rollbackType, writeSettings, encodingType, typs);
        }

        public static FileInfo RemoveAtInSerializedXml<T, K>(this T obj,
        Int32 index,
        String filePath,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = new FileInfo(filePath).Deserialize<T>(lockWaitMs, readSettings, typs);
            args.RemoveAt(index);
            return args.Serialize(filePath, lockWaitMs, rollbackType, writeSettings, encodingType, typs);
        }

        public static FileInfo RemoveRangeInSerializedXml<T, K>(this T obj,
        Int32 index,
        Int32 count,
        String filePath,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = new FileInfo(filePath).Deserialize<T>(lockWaitMs, readSettings, typs);
            args.RemoveRange(index, count);
            return args.Serialize(filePath, lockWaitMs, rollbackType, writeSettings, encodingType, typs);
        }

        public static FileInfo ReverseInSerializedXml<T, K>(this T obj,
        String filePath,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = new FileInfo(filePath).Deserialize<T>(lockWaitMs, readSettings, typs);
            args.Reverse();
            return args.Serialize(filePath, lockWaitMs, rollbackType, writeSettings, encodingType, typs);
        }

        public static FileInfo ReverseInSerializedXml<T, K>(this T obj,
        String filePath,
        int index,
        int count,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = new FileInfo(filePath).Deserialize<T>(lockWaitMs, readSettings, typs);
            args.Reverse();
            return args.Serialize(filePath, lockWaitMs, rollbackType, writeSettings, encodingType, typs);
        }

        public static Boolean TrueForAllInSerializedXml<T, K>(this T obj,
        Predicate<K> match,
        String filePath,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlReaderSettings readSettings = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = new FileInfo(filePath).Deserialize<T>(lockWaitMs, readSettings, typs);
            return args.TrueForAll(match);
        }

        #endregion List Xml filePath Helper Extension Methods

        #region List Xml FileInfo Helper Extension Methods

        public static FileInfo AddToSerializedXml<T, K>(this T obj,
        K objToAdd,
        FileInfo fileInfo,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = fileInfo.Deserialize<T>(lockWaitMs, readSettings, typs);
            args.Add(objToAdd);
            return args.Serialize(fileInfo, lockWaitMs, rollbackType, writeSettings, encodingType, typs);
        }

        public static FileInfo AddRangeToSerializedXml<T, K>(this T obj,
        IEnumerable<K> objToAdd,
        FileInfo fileInfo,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = fileInfo.Deserialize<T>(lockWaitMs, readSettings, typs);
            args.AddRange(objToAdd);
            return args.Serialize(fileInfo, lockWaitMs, rollbackType, writeSettings, encodingType, typs);
        }

        public static Int32 CountInSerializedXml<T, K>(this T obj,
        FileInfo fileInfo,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlReaderSettings readSettings = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = fileInfo.Deserialize<T>(lockWaitMs, readSettings, typs);
            return args.Count;
        }

        public static Boolean ExistsInSerializedXml<T, K>(this T obj,
        Predicate<K> match,
        FileInfo fileInfo,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlReaderSettings readSettings = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = fileInfo.Deserialize<T>(lockWaitMs, readSettings, typs);
            return args.Exists(match);
        }

        public static K FindInSerializedXml<T, K>(this T obj,
        Predicate<K> match,
        FileInfo fileInfo,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlReaderSettings readSettings = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = fileInfo.Deserialize<T>(lockWaitMs, readSettings, typs);
            return args.Find(match);
        }

        public static List<K> FindAllInSerializedXml<T, K>(this T obj,
        Predicate<K> match,
        FileInfo fileInfo,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlReaderSettings readSettings = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = fileInfo.Deserialize<T>(lockWaitMs, readSettings, typs);
            return args.FindAll(match);
        }

        public static Int32 FindIndexInSerializedXml<T, K>(this T obj,
        Predicate<K> match,
        FileInfo fileInfo,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlReaderSettings readSettings = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = fileInfo.Deserialize<T>(lockWaitMs, readSettings, typs);
            return args.FindIndex(match);
        }

        public static Int32 FindIndexInSerializedXml<T, K>(this T obj,
        Int32 startIndex,
        Predicate<K> match,
        FileInfo fileInfo,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlReaderSettings readSettings = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = fileInfo.Deserialize<T>(lockWaitMs, readSettings, typs);
            return args.FindIndex(startIndex, match);
        }

        public static Int32 FindIndexInSerializedXml<T, K>(this T obj,
        Int32 startIndex,
        int count,
        Predicate<K> match,
        FileInfo fileInfo,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlReaderSettings readSettings = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = fileInfo.Deserialize<T>(lockWaitMs, readSettings, typs);
            return args.FindIndex(startIndex, count, match);
        }

        public static K FindLastInSerializedXml<T, K>(this T obj,
        Predicate<K> match,
        FileInfo fileInfo,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlReaderSettings readSettings = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = fileInfo.Deserialize<T>(lockWaitMs, readSettings, typs);
            return args.FindLast(match);
        }

        public static Int32 FindLastIndexInSerializedXml<T, K>(this T obj,
        Predicate<K> match,
        FileInfo fileInfo,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlReaderSettings readSettings = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = fileInfo.Deserialize<T>(lockWaitMs, readSettings, typs);
            return args.FindLastIndex(match);
        }

        public static Int32 FindLastIndexInSerializedXml<T, K>(this T obj,
        Int32 startIndex,
        Predicate<K> match,
        FileInfo fileInfo,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlReaderSettings readSettings = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = fileInfo.Deserialize<T>(lockWaitMs, readSettings, typs);
            return args.FindLastIndex(startIndex, match);
        }

        public static Int32 FindLastIndexInSerializedXml<T, K>(this T obj,
        Int32 startIndex,
        int count,
        Predicate<K> match,
        FileInfo fileInfo,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlReaderSettings readSettings = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = fileInfo.Deserialize<T>(lockWaitMs, readSettings, typs);
            return args.FindLastIndex(startIndex, count, match);
        }

        public static FileInfo ForEachInSerializedXml<T, K>(this T obj,
        Action<K> action,
        FileInfo fileInfo,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = fileInfo.Deserialize<T>(lockWaitMs, readSettings, typs);
            args.Reverse();
            return args.Serialize(fileInfo, lockWaitMs, rollbackType, writeSettings, encodingType, typs);
        }

        public static List<K> GetRangeInSerializedXml<T, K>(this T obj,
        int index,
        int count,
        FileInfo fileInfo,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlReaderSettings readSettings = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = fileInfo.Deserialize<T>(lockWaitMs, readSettings, typs);
            return args.GetRange(index, count);
        }

        public static FileInfo InsertToSerializedXml<T, K>(this T obj,
        Int32 index,
        K objToAdd,
        FileInfo fileInfo,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = fileInfo.Deserialize<T>(lockWaitMs, readSettings, typs);
            args.Insert(index, objToAdd);
            return args.Serialize(fileInfo, lockWaitMs, rollbackType, writeSettings, encodingType, typs);
        }

        public static FileInfo InsertRangeToSerializedXml<T, K>(this T obj,
        Int32 index,
        IEnumerable<K> objToAdd,
        FileInfo fileInfo,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = fileInfo.Deserialize<T>(lockWaitMs, readSettings, typs);
            args.InsertRange(index, objToAdd);
            return args.Serialize(fileInfo, lockWaitMs, rollbackType, writeSettings, encodingType, typs);
        }

        //public static FileInfo OrderByInSerializedXml<T, K, TKey>(this T obj,
        //Func<K, TKey> keySelector,
        //FileInfo fileInfo,
        //Int32 lockWaitMs = 60000,
        //IoRollbackType rollbackType = IoRollbackType.none,
        //XmlWriterSettings writeSettings = null,
        //XmlReaderSettings readSettings = null,
        //Encoding encodingType = null,
        //IEnumerable<Type> knownTypes = null) where T : List<K>
        //{
        //    var typs = new List<Type>() { typeof(T), typeof(K) };
        //    if (knownTypes != null) typs.AddRange(knownTypes);
        //    T args = fileInfo.Deserialize<T>(lockWaitMs, readSettings, typs);
        //    List<K> sorted = args.OrderBy(keySelector).ToList();
        //    args.Clear();
        //    args.AddRange(sorted);
        //    return args.Serialize<T>(filePath, lockWaitMs, rollbackType, writeSettings, encodingType, typs);
        //}

        public static FileInfo RemoveAnyInSerializedXml<T, K>(this T obj,
        Func<K, bool> predicate,
        FileInfo fileInfo,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = fileInfo.Deserialize<T>(lockWaitMs, readSettings, typs);
            var removes = new List<K>(args.Where(predicate));
            foreach (var match in removes)
            {
                args.Remove(match);
            }
            return args.Serialize(fileInfo, lockWaitMs, rollbackType, writeSettings, encodingType, typs);
        }

        public static FileInfo RemoveAtInSerializedXml<T, K>(this T obj,
        Int32 index,
        FileInfo fileInfo,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = fileInfo.Deserialize<T>(lockWaitMs, readSettings, typs);
            args.RemoveAt(index);
            return args.Serialize(fileInfo, lockWaitMs, rollbackType, writeSettings, encodingType, typs);
        }

        public static FileInfo RemoveRangeInSerializedXml<T, K>(this T obj,
        Int32 index,
        Int32 count,
        FileInfo fileInfo,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = fileInfo.Deserialize<T>(lockWaitMs, readSettings, typs);
            args.RemoveRange(index, count);
            return args.Serialize(fileInfo, lockWaitMs, rollbackType, writeSettings, encodingType, typs);
        }

        public static FileInfo ReverseInSerializedXml<T, K>(this T obj,
        FileInfo fileInfo,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = fileInfo.Deserialize<T>(lockWaitMs, readSettings, typs);
            args.Reverse();
            return args.Serialize(fileInfo, lockWaitMs, rollbackType, writeSettings, encodingType, typs);
        }

        public static FileInfo ReverseInSerializedXml<T, K>(this T obj,
        FileInfo fileInfo,
        int index,
        int count,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = fileInfo.Deserialize<T>(lockWaitMs, readSettings, typs);
            args.Reverse();
            return args.Serialize(fileInfo, lockWaitMs, rollbackType, writeSettings, encodingType, typs);
        }

        public static Boolean TrueForAllInSerializedXml<T, K>(this T obj,
        Predicate<K> match,
        FileInfo fileInfo,
        Int32 lockWaitMs = 60000,
        IoRollbackType rollbackType = IoRollbackType.None,
        XmlReaderSettings readSettings = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = fileInfo.Deserialize<T>(lockWaitMs, readSettings, typs);
            return args.TrueForAll(match);
        }

        #endregion List Xml FileInfo Helper Extension Methods

        #region List Byte[] Helper Extension Methods

        public static Byte[] AddToSerializedBytes<T, K>(this T obj,
        K objToAdd,
        Byte[] data,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeBytes<T>(readSettings, encodingType, typs);
            args.Add(objToAdd);
            return args.SerializeToBytes(writeSettings, encodingType, typs);
        }

        public static Byte[] AddRangeToSerializedBytes<T, K>(this T obj,
        IEnumerable<K> objToAdd,
        Byte[] data,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeBytes<T>(readSettings, encodingType, typs);
            args.AddRange(objToAdd);
            return args.SerializeToBytes(writeSettings, encodingType, typs);
        }

        public static Int32 CountInSerializedBytes<T, K>(this T obj,
        Byte[] data,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeBytes<T>(readSettings, encodingType, typs);
            return args.Count;
        }

        public static Boolean ExistsInSerializedBytes<T, K>(this T obj,
        Predicate<K> match,
        Byte[] data,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeBytes<T>(readSettings, encodingType, typs);
            return args.Exists(match);
        }

        public static K FindInSerializedBytes<T, K>(this T obj,
        Predicate<K> match,
        Byte[] data,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeBytes<T>(readSettings, encodingType, typs);
            return args.Find(match);
        }

        public static List<K> FindAllInSerializedBytes<T, K>(this T obj,
        Predicate<K> match,
        Byte[] data,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeBytes<T>(readSettings, encodingType, typs);
            return args.FindAll(match);
        }

        public static Int32 FindIndexInSerializedBytes<T, K>(this T obj,
        Predicate<K> match,
        Byte[] data,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeBytes<T>(readSettings, encodingType, typs);
            return args.FindIndex(match);
        }

        public static Int32 FindIndexInSerializedBytes<T, K>(this T obj,
        Int32 startIndex,
        Predicate<K> match,
        Byte[] data,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeBytes<T>(readSettings, encodingType, typs);
            return args.FindIndex(startIndex, match);
        }

        public static Int32 FindIndexInSerializedBytes<T, K>(this T obj,
        Int32 startIndex,
        int count,
        Predicate<K> match,
        Byte[] data,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeBytes<T>(readSettings, encodingType, typs);
            return args.FindIndex(startIndex, count, match);
        }

        public static K FindLastInSerializedBytes<T, K>(this T obj,
        Predicate<K> match,
        Byte[] data,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeBytes<T>(readSettings, encodingType, typs);
            return args.FindLast(match);
        }

        public static Int32 FindLastIndexInSerializedBytes<T, K>(this T obj,
        Predicate<K> match,
        Byte[] data,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeBytes<T>(readSettings, encodingType, typs);
            return args.FindLastIndex(match);
        }

        public static Int32 FindLastIndexInSerializedBytes<T, K>(this T obj,
        Int32 startIndex,
        Predicate<K> match,
        Byte[] data,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeBytes<T>(readSettings, encodingType, typs);
            return args.FindLastIndex(startIndex, match);
        }

        public static Int32 FindLastIndexInSerializedBytes<T, K>(this T obj,
        Int32 startIndex,
        int count,
        Predicate<K> match,
        Byte[] data,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeBytes<T>(readSettings, encodingType, typs);
            return args.FindLastIndex(startIndex, count, match);
        }

        public static Byte[] ForEachInSerializedBytes<T, K>(this T obj,
        Action<K> action,
        Byte[] data,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeBytes<T>(readSettings, encodingType, typs);
            args.Reverse();
            return args.SerializeToBytes(writeSettings, encodingType, typs);
        }

        public static List<K> GetRangeInSerializedBytes<T, K>(this T obj,
        int index,
        int count,
        Byte[] data,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeBytes<T>(readSettings, encodingType, typs);
            return args.GetRange(index, count);
        }

        public static Byte[] InsertToSerializedBytes<T, K>(this T obj,
        Int32 index,
        K objToAdd,
        Byte[] data,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeBytes<T>(readSettings, encodingType, typs);
            args.Insert(index, objToAdd);
            return args.SerializeToBytes(writeSettings, encodingType, typs);
        }

        public static Byte[] InsertRangeToSerializedBytes<T, K>(this T obj,
        Int32 index,
        IEnumerable<K> objToAdd,
        Byte[] data,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeBytes<T>(readSettings, encodingType, typs);
            args.InsertRange(index, objToAdd);
            return args.SerializeToBytes(writeSettings, encodingType, typs);
        }

        //public static Byte[] OrderByInSerializedBytes<T, K, TKey>(this T obj,
        //Func<K, TKey> keySelector,
        //Byte[] data,
        //Int32 lockWaitMs = 60000,
        //IoRollbackType rollbackType = IoRollbackType.none,
        //XmlWriterSettings writeSettings = null,
        //XmlReaderSettings readSettings = null,
        //Encoding encodingType = null,
        //IEnumerable<Type> knownTypes = null) where T : List<K>
        //{
        //    var typs = new List<Type>() { typeof(T), typeof(K) };
        //    if (knownTypes != null) typs.AddRange(knownTypes);
        //    T args = data.DeserializeBytes<T>(readSettings, encodingType, typs);
        //    List<K> sorted = args.OrderBy(keySelector).ToList();
        //    args.Clear();
        //    args.AddRange(sorted);
        //    return args.SerializedBytes<T>(filePath, writeSettings, encodingType, typs);
        //}

        public static Byte[] RemoveAnyInSerializedBytes<T, K>(this T obj,
        Func<K, bool> predicate,
        Byte[] data,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeBytes<T>(readSettings, encodingType, typs);
            var removes = new List<K>(args.Where(predicate));
            foreach (var match in removes)
            {
                args.Remove(match);
            }
            return args.SerializeToBytes(writeSettings, encodingType, typs);
        }

        public static Byte[] RemoveAtInSerializedBytes<T, K>(this T obj,
        Int32 index,
        Byte[] data,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeBytes<T>(readSettings, encodingType, typs);
            args.RemoveAt(index);
            return args.SerializeToBytes(writeSettings, encodingType, typs);
        }

        public static Byte[] RemoveRangeInSerializedBytes<T, K>(this T obj,
        Int32 index,
        Int32 count,
        Byte[] data,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeBytes<T>(readSettings, encodingType, typs);
            args.RemoveRange(index, count);
            return args.SerializeToBytes(writeSettings, encodingType, typs);
        }

        public static Byte[] ReverseInSerializedBytes<T, K>(this T obj,
        Byte[] data,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeBytes<T>(readSettings, encodingType, typs);
            args.Reverse();
            return args.SerializeToBytes(writeSettings, encodingType, typs);
        }

        public static Byte[] ReverseInSerializedBytes<T, K>(this T obj,
        Byte[] data,
        int index,
        int count,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeBytes<T>(readSettings, encodingType, typs);
            args.Reverse();
            return args.SerializeToBytes(writeSettings, encodingType, typs);
        }

        public static Boolean TrueForAllInSerializedBytes<T, K>(this T obj,
        Predicate<K> match,
        Byte[] data,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeBytes<T>(readSettings, encodingType, typs);
            return args.TrueForAll(match);
        }

        #endregion List Byte[] Helper Extension Methods

        #region List String Helper Extension Methods

        public static String AddToSerializedString<T, K>(this T obj,
        K objToAdd,
        String data,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeString<T>(readSettings, typs);
            args.Add(objToAdd);
            return args.SerializeToString(writeSettings, encodingType, typs);
        }

        public static String AddRangeToSerializedString<T, K>(this T obj,
        IEnumerable<K> objToAdd,
        String data,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeString<T>(readSettings, typs);
            args.AddRange(objToAdd);
            return args.SerializeToString(writeSettings, encodingType, typs);
        }

        public static Int32 CountInSerializedString<T, K>(this T obj,
        String data,
        XmlReaderSettings readSettings = null,
            //Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeString<T>(readSettings, typs);
            return args.Count;
        }

        public static Boolean ExistsInSerializedString<T, K>(this T obj,
        Predicate<K> match,
        String data,
        XmlReaderSettings readSettings = null,
            //Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeString<T>(readSettings, typs);
            return args.Exists(match);
        }

        public static K FindInSerializedString<T, K>(this T obj,
        Predicate<K> match,
        String data,
        XmlReaderSettings readSettings = null,
            //Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeString<T>(readSettings, typs);
            return args.Find(match);
        }

        public static List<K> FindAllInSerializedString<T, K>(this T obj,
        Predicate<K> match,
        String data,
        XmlReaderSettings readSettings = null,
            //Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeString<T>(readSettings, typs);
            return args.FindAll(match);
        }

        public static Int32 FindIndexInSerializedString<T, K>(this T obj,
        Predicate<K> match,
        String data,
        XmlReaderSettings readSettings = null,
            //Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeString<T>(readSettings, typs);
            return args.FindIndex(match);
        }

        public static Int32 FindIndexInSerializedString<T, K>(this T obj,
        Int32 startIndex,
        Predicate<K> match,
        String data,
        XmlReaderSettings readSettings = null,
            //Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeString<T>(readSettings, typs);
            return args.FindIndex(startIndex, match);
        }

        public static Int32 FindIndexInSerializedString<T, K>(this T obj,
        Int32 startIndex,
        int count,
        Predicate<K> match,
        String data,
        XmlReaderSettings readSettings = null,
            //Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeString<T>(readSettings, typs);
            return args.FindIndex(startIndex, count, match);
        }

        public static K FindLastInSerializedString<T, K>(this T obj,
        Predicate<K> match,
        String data,
        XmlReaderSettings readSettings = null,
            //Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeString<T>(readSettings, typs);
            return args.FindLast(match);
        }

        public static Int32 FindLastIndexInSerializedString<T, K>(this T obj,
        Predicate<K> match,
        String data,
        XmlReaderSettings readSettings = null,
            //Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeString<T>(readSettings, typs);
            return args.FindLastIndex(match);
        }

        public static Int32 FindLastIndexInSerializedString<T, K>(this T obj,
        Int32 startIndex,
        Predicate<K> match,
        String data,
        XmlReaderSettings readSettings = null,
            //Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeString<T>(readSettings, typs);
            return args.FindLastIndex(startIndex, match);
        }

        public static Int32 FindLastIndexInSerializedString<T, K>(this T obj,
        Int32 startIndex,
        int count,
        Predicate<K> match,
        String data,
        XmlReaderSettings readSettings = null,
            //Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeString<T>(readSettings, typs);
            return args.FindLastIndex(startIndex, count, match);
        }

        public static String ForEachInSerializedString<T, K>(this T obj,
        Action<K> action,
        String data,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeString<T>(readSettings, typs);
            args.Reverse();
            return args.SerializeToString(writeSettings, encodingType, typs);
        }

        public static List<K> GetRangeInSerializedString<T, K>(this T obj,
        int index,
        int count,
        String data,
        XmlReaderSettings readSettings = null,
            //Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeString<T>(readSettings, typs);
            return args.GetRange(index, count);
        }

        public static String InsertToSerializedString<T, K>(this T obj,
        Int32 index,
        K objToAdd,
        String data,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeString<T>(readSettings, typs);
            args.Insert(index, objToAdd);
            return args.SerializeToString(writeSettings, encodingType, typs);
        }

        public static String InsertRangeToSerializedString<T, K>(this T obj,
        Int32 index,
        IEnumerable<K> objToAdd,
        String data,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeString<T>(readSettings, typs);
            args.InsertRange(index, objToAdd);
            return args.SerializeToString(writeSettings, encodingType, typs);
        }

        //public static String OrderByInSerializedString<T, K, TKey>(this T obj,
        //Func<K, TKey> keySelector,
        //String data,
        //Int32 lockWaitMs = 60000,
        //IoRollbackType rollbackType = IoRollbackType.none,
        //XmlWriterSettings writeSettings = null,
        //XmlReaderSettings readSettings = null,
        //Encoding encodingType = null,
        //IEnumerable<Type> knownTypes = null) where T : List<K>
        //{
        //    var typs = new List<Type>() { typeof(T), typeof(K) };
        //    if (knownTypes != null) typs.AddRange(knownTypes);
        //    T args = data.DeserializeString<T>(readSettings, typs);
        //    List<K> sorted = args.OrderBy(keySelector).ToList();
        //    args.Clear();
        //    args.AddRange(sorted);
        //    return args.SerializedString<T>(filePath, writeSettings, encodingType, typs);
        //}

        public static String RemoveAnyInSerializedString<T, K>(this T obj,
        Func<K, bool> predicate,
        String data,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeString<T>(readSettings, typs);
            var removes = new List<K>(args.Where(predicate));
            foreach (var match in removes)
            {
                args.Remove(match);
            }
            return args.SerializeToString(writeSettings, encodingType, typs);
        }

        public static String RemoveAtInSerializedString<T, K>(this T obj,
        Int32 index,
        String data,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeString<T>(readSettings, typs);
            args.RemoveAt(index);
            return args.SerializeToString(writeSettings, encodingType, typs);
        }

        public static String RemoveRangeInSerializedString<T, K>(this T obj,
        Int32 index,
        Int32 count,
        String data,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeString<T>(readSettings, typs);
            args.RemoveRange(index, count);
            return args.SerializeToString(writeSettings, encodingType, typs);
        }

        public static String ReverseInSerializedString<T, K>(this T obj,
        String data,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeString<T>(readSettings, typs);
            args.Reverse();
            return args.SerializeToString(writeSettings, encodingType, typs);
        }

        public static String ReverseInSerializedString<T, K>(this T obj,
        String data,
        int index,
        int count,
        XmlWriterSettings writeSettings = null,
        XmlReaderSettings readSettings = null,
        Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeString<T>(readSettings, typs);
            args.Reverse();
            return args.SerializeToString(writeSettings, encodingType, typs);
        }

        public static Boolean TrueForAllInSerializedString<T, K>(this T obj,
        Predicate<K> match,
        String data,
        XmlReaderSettings readSettings = null,
            //Encoding encodingType = null,
        IEnumerable<Type> knownTypes = null) where T : List<K>
        {
            var typs = new List<Type> { typeof(T), typeof(K) };
            if (knownTypes != null) typs.AddRange(knownTypes);
            var args = data.DeserializeString<T>(readSettings, typs);
            return args.TrueForAll(match);
        }

        #endregion List String Helper Extension Methods

        // ReSharper restore InconsistentNaming
    }
}