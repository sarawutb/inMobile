using Innovation.Mobile.App.Constants;
using Innovation.Mobile.App.Contracts.Service.Genaral;
using NLog;
using NLog.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using Xamarin.Forms;

namespace Innovation.Mobile.App.Service.Genaral
{
    internal class LoggingService : ILoggingService
    {
        private ILogger _logger;

        public void Initialize(Assembly assembly)
        {
            var nlogConfigFile = GetEmbeddedResourceStream(assembly, "NLog.config");

            if (nlogConfigFile != null)
            {
                var xmlReader = System.Xml.XmlReader.Create(nlogConfigFile);
                NLog.LogManager.Configuration = new XmlLoggingConfiguration(xmlReader, null);
                _logger = LogManager.GetCurrentClassLogger();
            }
        }

        public Stream GetEmbeddedResourceStream(Assembly assembly, string resourceFileName)
        {
            var resourcePaths = assembly.GetManifestResourceNames()
              .Where(x => x.EndsWith(resourceFileName, StringComparison.OrdinalIgnoreCase))
              .ToList();
            if (resourcePaths.Count == 1)
            {
                return assembly.GetManifestResourceStream(resourcePaths.Single());
            }
            return null;
        }

        public void Error(string message) => _logger.Error(message);
    }
}
