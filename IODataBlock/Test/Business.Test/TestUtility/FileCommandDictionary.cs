using System.Collections.Generic;
using Business.Common.System;

namespace Business.Test.TestUtility
{
    public class FileCommandDictionary
    {
        private static FileCommandDictionary _instance = new FileCommandDictionary();

        private FileCommandDictionary()
        {
        }

        public static FileCommandDictionary Instance
        {
            get { return _instance ?? (_instance = new FileCommandDictionary()); }
        }

        public Dictionary<string, IEnumerable<ICommandObject>> CommandDictionary = new Dictionary<string, IEnumerable<ICommandObject>>
        {
            {"FileCommands", new List<ICommandObject> {new WriteToFileCommand(), new ReadFromFileCommand()}},
            {"JsonFileCommands",new List<ICommandObject> {new WriteToJsonFileCommand(), new ReadFromJsonFileCommand()}}
        };
    }
}