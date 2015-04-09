using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Business.Common.IO
{
    public class CompareDirectoryContent
    {
        public delegate IList<String> DirectoryComparer(String sourceDirectory, String destinationDirectory);

        public IList<String> CompareDirectories(String sourceDirectory, String destinationDirectory, DirectoryComparer comparer)
        {
            return comparer(sourceDirectory, destinationDirectory);
        }
    }

    public class MatchedNameDirectoryComparer
    {
        public IEnumerable<FileInfo> GetMatchedItems(DirectoryInfo sourceDirectory, DirectoryInfo destinationDirectory)
        {
            if (!sourceDirectory.Exists) throw new DirectoryNotFoundException();
            if (!destinationDirectory.Exists) throw new DirectoryNotFoundException();

            var srcfiles = sourceDirectory.GetFiles();
            return destinationDirectory.GetFiles()
                    .Where(f => srcfiles.Any(x => String.Equals(x.Name, f.Name, StringComparison.InvariantCultureIgnoreCase)));
        }

        public IEnumerable<FileInfo> GetUnMatchedDestinationItems(DirectoryInfo sourceDirectory, DirectoryInfo destinationDirectory)
        {
            if (!sourceDirectory.Exists) throw new DirectoryNotFoundException();
            if (!destinationDirectory.Exists) throw new DirectoryNotFoundException();

            var srcfiles = sourceDirectory.GetFiles();
            return destinationDirectory.GetFiles()
                    .Where(f => srcfiles.All(x => !String.Equals(x.Name, f.Name, StringComparison.InvariantCultureIgnoreCase)));
        }

        public IEnumerable<FileInfo> GetUnMatchedSourceItems(DirectoryInfo sourceDirectory, DirectoryInfo destinationDirectory)
        {
            if (!sourceDirectory.Exists) throw new DirectoryNotFoundException();
            if (!destinationDirectory.Exists) throw new DirectoryNotFoundException();

            var destfiles = destinationDirectory.GetFiles();
            return sourceDirectory.GetFiles()
                    .Where(f => destfiles.All(x => !String.Equals(x.Name, f.Name, StringComparison.InvariantCultureIgnoreCase)));
        }

        //public List<String> GetMatchedItems(String sourceDirectory, String destinationDirectory)
        //{
        //    var rv = new List<string>();
        //    var sourceDi = new DirectoryInfo(sourceDirectory);
        //    var destinationDi = new DirectoryInfo(destinationDirectory);

        //    if (!sourceDi.Exists || !destinationDi.Exists) return rv;
        //    var srcfiles = sourceDi.GetFiles().Select(x => x.Name);
        //    rv.AddRange(
        //        destinationDi.GetFiles()
        //            .Where(f => srcfiles.Any(x => String.Equals(x, f.Name, StringComparison.InvariantCultureIgnoreCase)))
        //            .Select(f => f.Name));
        //    return rv;
        //}

        //public List<String> GetUnMatchedDestinationItems(String sourceDirectory, String destinationDirectory)
        //{
        //    var rv = new List<string>();
        //    var sourceDi = new DirectoryInfo(sourceDirectory);
        //    var destinationDi = new DirectoryInfo(destinationDirectory);

        //    if (!sourceDi.Exists || !destinationDi.Exists) return rv;
        //    var srcfiles = sourceDi.GetFiles().Select(x => x.Name);
        //    rv.AddRange(
        //        destinationDi.GetFiles()
        //            .Where(f => srcfiles.All(x => !String.Equals(x, f.Name, StringComparison.InvariantCultureIgnoreCase)))
        //            .Select(f => f.Name));
        //    return rv;
        //}

        //public List<String> GetUnMatchedSourceItems(String sourceDirectory, String destinationDirectory)
        //{
        //    var rv = new List<string>();
        //    var sourceDi = new DirectoryInfo(sourceDirectory);
        //    var destinationDi = new DirectoryInfo(destinationDirectory);

        //    if (!sourceDi.Exists || !destinationDi.Exists) return rv;
        //    var destfiles = destinationDi.GetFiles().Select(x => x.Name);
        //    rv.AddRange(
        //        sourceDi.GetFiles()
        //            .Where(f => destfiles.All(x => !String.Equals(x, f.Name, StringComparison.InvariantCultureIgnoreCase)))
        //            .Select(f => f.Name));
        //    return rv;
        //}
    }

    public static class FileNameComparer
    {
        public delegate Boolean FileMatcher(FileInfo local, IEnumerable<FileInfo> remotefiles);

        public static FileMatcher NameAndExtMatch =
            (local, remotefiles) => remotefiles.Any(remote => local.Name == remote.Name);

        public static IEnumerable<FileInfo> GetLocalMatchingFiles(String localDirectory, String remoteDirectory)
        {
            return GetLocalMatchingFiles(localDirectory, remoteDirectory, NameAndExtMatch);
        }

        public static IEnumerable<FileInfo> GetLocalMatchingFiles(String localDirectory, String remoteDirectory, String extension)
        {
            Func<String, Boolean> ext = x => x.EndsWith(extension);
            //Func<String, Boolean> ext2 = x => x.EndsWith(Extension + ".gz");
            var criteria = new List<Func<string, bool>> { ext };
            return GetLocalMatchingFiles(localDirectory, remoteDirectory, criteria);
        }

        public static IEnumerable<FileInfo> GetLocalMatchingFiles(String localDirectory, String remoteDirectory, FileMatcher matcher)
        {
            var local = new DirectoryInfo(localDirectory).GetFiles().ToList();
            var remote = new DirectoryInfo(remoteDirectory).GetFiles().ToList();

            return local.Where(f => matcher(f, remote));
        }

        public static IEnumerable<FileInfo> GetLocalMatchingFiles(String localDirectory, String remoteDirectory, IEnumerable<Func<String, Boolean>> fileNameCriteria)
        {
            var local = new DirectoryInfo(localDirectory).GetFiles().ToList();
            var remote = new DirectoryInfo(remoteDirectory).GetFiles().ToList();

            return from f in local where remote.Any(x => x.Name == f.Name) from c in fileNameCriteria where c(f.Name) select f;
        }
    }
}