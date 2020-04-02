using System;

namespace Microsoft.TemplateEngine.Abstractions.PhysicalFileSystem
{
    public interface IFileLastWriteTimeSource
    {
        DateTime GetLastWriteTimeUtc(string file);

        void SetLastWriteTimeUtc(string file, DateTime lastWriteTimeUtc);
    }
}
