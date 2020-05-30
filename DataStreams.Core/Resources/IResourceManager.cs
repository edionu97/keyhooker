using System.Collections.Generic;
using DataStreams.Utility;

namespace DataStreams.Core.Resources
{
    public interface IResourceManager
    {
        IEnumerable<FileInformation> GetLanguages();
    }
}
