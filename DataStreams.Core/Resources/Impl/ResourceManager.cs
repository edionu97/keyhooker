using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataStreams.Utility;

namespace DataStreams.Core.Resources.Impl
{
    public class ResourceManager : IResourceManager
    {
        public IEnumerable<FileInformation> GetLanguages()
        {
            return new DirectoryInfo(Path.Combine("Resources", "Files"))
                .GetDirectories()
                .Select(directory => new FileInformation
                {
                    Language = directory.Name,
                    FileAff = directory.GetFiles("*.aff").First().FullName,
                    FileDict = directory.GetFiles("*.dic").First().FullName
                });
        }
    }
}
