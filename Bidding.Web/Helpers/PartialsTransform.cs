using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Optimization;

namespace Bidding.Web.Helpers
{
    public class PartialsTransform : IBundleTransform
    {
        private readonly Dictionary<string, string> _modulePath;

        public PartialsTransform(Dictionary<string, string> modulePath)
        {
            _modulePath = modulePath;
        }

        public void Process(BundleContext context, BundleResponse response)
        {
            Regex regex = new Regex(@"\s+");
            var strBundleResponse = new StringBuilder();
            // Javascript module for Angular that uses templateCache 
            foreach (var module in _modulePath)
            {
                strBundleResponse.AppendFormat(@"angular.module('{0}').run(['$templateCache',function(t){{", module.Key);

                foreach (var file in response.Files.Where(f => IsInPath(module.Value, f.VirtualFile.VirtualPath)))
                {
                    try
                    {
                        var content = file.ApplyTransforms().Replace("\r\n", "").Replace("'", "\\'");
                        content = regex.Replace(content, " ");

                        if (!string.IsNullOrWhiteSpace(content))
                        {
                            strBundleResponse.AppendFormat("\r\nt.put('{0}/{1}','{2}');", module.Value, file.VirtualFile.Name, content);
                        }
                    }
                    catch (Exception)
                    { }
                }

                strBundleResponse.Append(@"}]); ");
            }

            response.Files = new BundleFile[] { };
            response.Content = strBundleResponse.ToString();
            response.ContentType = "text/javascript";
        }

        private bool IsInPath(string path, string fileFullPath)
        {
            string[] pathParts = path.ToLowerInvariant().Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar).Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
            string[] fileParts = fileFullPath.ToLowerInvariant().Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar).Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);

            if (fileParts.Length < pathParts.Length)
                return false;

            for (int i = 0; i < pathParts.Length; i++)
            {
                if (pathParts[i] != fileParts[i])
                    return false;
            }
            return true;
        }
    }
}