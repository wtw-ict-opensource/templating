// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Concurrent;
using System.IO;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Abstractions.Mount;
using Microsoft.TemplateEngine.Edge.Settings;
using Microsoft.TemplateEngine.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#nullable enable

namespace Microsoft.TemplateEngine.Cli
{
    public class HostSpecificDataLoader : IHostSpecificDataLoader
    {
        private readonly IEngineEnvironmentSettings _engineEnvironment;

        private readonly ConcurrentDictionary<ITemplateInfo, HostSpecificTemplateData> _cache =
            new ConcurrentDictionary<ITemplateInfo, HostSpecificTemplateData>();

        public HostSpecificDataLoader(IEngineEnvironmentSettings engineEnvironment)
        {
            _engineEnvironment = engineEnvironment;
        }

        public HostSpecificTemplateData ReadHostSpecificTemplateData(ITemplateInfo templateInfo)
        {
            return _cache.GetOrAdd(templateInfo, ReadHostSpecificTemplateDataUncached);
        }

        private HostSpecificTemplateData ReadHostSpecificTemplateDataUncached(ITemplateInfo templateInfo)
        {
            IMountPoint? mountPoint = null;

            if (templateInfo is ITemplateInfoHostJsonCache { HostData: JObject hostData })
            {
                return new HostSpecificTemplateData(hostData);
            }

            try
            {
                if (!string.IsNullOrEmpty(templateInfo.HostConfigPlace) && _engineEnvironment.TryGetMountPoint(templateInfo.MountPointUri, out mountPoint))
                {
                    var file = mountPoint!.FileInfo(templateInfo.HostConfigPlace);
                    if (file != null && file.Exists)
                    {
                        JObject jsonData;
                        using (Stream stream = file.OpenRead())
                        using (TextReader textReader = new StreamReader(stream, true))
                        using (JsonReader jsonReader = new JsonTextReader(textReader))
                        {
                            jsonData = JObject.Load(jsonReader);
                        }

                        return new HostSpecificTemplateData(jsonData);
                    }
                }
            }
            catch
            {
                // ignore malformed host files
            }
            finally
            {
                mountPoint?.Dispose();
            }

            return HostSpecificTemplateData.Default;
        }
    }
}
