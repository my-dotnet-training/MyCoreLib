using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Internal;
using System.Net;
using System.IO;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.FileProviders;

namespace MyCoreLib.BaseWeb.Utils
{
    public static class HttpHelper
    {
        private static HostingEnvironment _env = new HostingEnvironment();
        /// <summary>
        /// Get the real IP address of the client.
        /// </summary>
        public static string GetUserIP(DefaultHttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            string value = request.Form["HTTP_VIA"];
            if (!string.IsNullOrEmpty(value))
            {
                value = request.Form["HTTP_X_FORWARDED_FOR"];
            }

            if (string.IsNullOrEmpty(value))
            {
                return request.Host.ToString();
            }
            return value;
        }

        public static string GetAbsoluteUrl(this UrlHelper urlHelper, string contentPath)
        {
            if (!string.IsNullOrEmpty(contentPath))
            {
                if (contentPath[0] == '~')
                {
                    contentPath = urlHelper.Content(contentPath);
                }

                if (contentPath[0] == '/')
                {
                    var uri = new Uri(new Uri(urlHelper.ActionContext.HttpContext.Request.PathBase), contentPath);
                    contentPath = uri.ToString();
                }
            }
            return contentPath;
        }

        public static string ResolveUrl(string vPath)
        {
            if (!string.IsNullOrEmpty(vPath) && vPath[0] == '~')
            {
                IFileInfo f = _env.ContentRootFileProvider.GetFileInfo(vPath);
                if (f != null)
                {
                    return f.PhysicalPath;
                }
            }

            return vPath;
        }

        public static string MapPath(string vPath)
        {
            return HostingEnvironment.MapPath(vPath);
        }

        /// <summary>
        /// Read http response from the specified url.
        /// </summary>
        public static string Get(string url, int timeoutSeconds = 30)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            if (timeoutSeconds != -1) // -1 means to use the default timeout value.
            {
                request.ContinueTimeout = timeoutSeconds * 1000;
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponseAsync().Result)
            {
                // Read server response.
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return sr.ReadToEnd().Trim();
                }
            }
        }

        /// <summary>
        /// Read http response from the specified url.
        /// </summary>
        public static Stream GetAsBuffer(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponseAsync().Result)
            {
                const int MB = 1024 * 1024;

                if (response.ContentLength < 0)
                {
                    throw new InvalidOperationException(string.Format("Unexpected content length: {0}.", response.ContentLength));
                }
                if (response.ContentLength >= 2 * MB)
                {
                    throw new InvalidOperationException(string.Format("Not allowed to read 2M+ response: {0}.", response.ContentLength));
                }

                // Read server response.
                using (var responseStream = response.GetResponseStream())
                {
                    var content = new MemoryStream();
                    responseStream.CopyTo(content);

                    content.Position = 0;
                    return content;
                }
            }
        }

        /// <summary>
        /// Read http response from the specified url.
        /// </summary>
        public static async Task<HttpObjectInfo> GetAsBufferAsync(string url, int timeoutSeconds = 30)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            if (timeoutSeconds != -1) // -1 means to use the default timeout value.
            {
                request.ContinueTimeout = timeoutSeconds * 1000;
            }
            request.Method = "GET";

            // Simuate request from weixin browser
            request.Host = request.RequestUri.Host;
            request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 6_1_3 like Mac OS X) AppleWebKit/536.26 (KHTML, like Gecko) Mobile/10B329 MicroMessenger/5.0.1";

            using (HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync().ConfigureAwait(false)))
            {
                const int MB = 1024 * 1024;

                if (response.ContentLength < 0)
                {
                    throw new InvalidOperationException(string.Format("Unexpected content length: {0}.", response.ContentLength));
                }
                if (response.ContentLength >= 2 * MB)
                {
                    throw new InvalidOperationException(string.Format("Not allowed to read 2M+ response: {0}.", response.ContentLength));
                }

                // Read server response.
                Stream content = new MemoryStream();
                using (var responseStream = response.GetResponseStream())
                {
                    responseStream.CopyTo(content);
                    content.Position = 0;
                }

                return new HttpObjectInfo
                {
                    Content = content,
                    ContentLength = (int)response.ContentLength,
                    ContentType = response.ContentType
                };
            }
        }

        /// <summary>
        /// Read http response from the specified url.
        /// </summary>
        public static async Task<string> GetAsync(string url, int timeoutSeconds = 30)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            if (timeoutSeconds != -1) // -1 means to use the default timeout value.
            {
                request.ContinueTimeout = timeoutSeconds * 1000;
            }

            using (HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync().ConfigureAwait(false)))
            {
                // Read server response.
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return sr.ReadToEnd().Trim();
                }
            }
        }

        /// <summary>
        /// Write data to server using HTTP POST request.
        /// </summary>
        public static string Post(string url, string content, string contentType = "application/json; encoding=utf-8", int timeoutSeconds = 30, string certPath = null, string certPassword = null, string host = null)
        {
            HttpWebRequest request = PreparePostRequest(url, content, contentType, timeoutSeconds, certPath, certPassword, host);

            // Post data
            byte[] data = System.Text.Encoding.UTF8.GetBytes(content);
            request.ContentLength = data.Length;
            using (var reqStream = request.GetRequestStreamAsync().Result)
            {
                reqStream.Write(data, 0, data.Length);
            }

            // Read server response
            using (var response = (HttpWebResponse)request.GetResponseAsync().Result)
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return sr.ReadToEnd().Trim();
                }
            }
        }

        private static HttpWebRequest PreparePostRequest(string url, string content, string contentType, int timeoutSeconds, string certPath, string certPassword, string host)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            if (timeoutSeconds != -1) // -1 means to use the default timeout value.
            {
                request.ContinueTimeout = timeoutSeconds * 1000;
            }
            request.ContentType = contentType;
            if (!string.IsNullOrEmpty(host))
            {
                request.Host = host;
            }

            if (!string.IsNullOrEmpty(certPath))
            {
                if (string.IsNullOrEmpty(certPassword))
                    throw new InvalidOperationException("Password must be provided to access cert file.");

                /*
                 * Change to use MachineKeySet to fix bug:
                 * System.Security.Cryptography.CryptographicException: The system cannot find the file specified
                 * 
                 * Please refer to the link for the details:
                 * https://blogs.msdn.microsoft.com/alejacma/2007/12/03/rsacryptoserviceprovider-fails-when-used-with-asp-net/
                 */
                X509Certificate2 cert = new X509Certificate2(certPath, certPassword, X509KeyStorageFlags.MachineKeySet);
                request.ClientCertificates.Add(cert);
            }

            return request;
        }

        public static async Task<string> PostAsync(string url, string content, string contentType = "text/xml", int timeoutSeconds = 30, string certPath = null, string certPassword = null, string host = null)
        {
            ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
            HttpWebRequest request = PreparePostRequest(url, content, contentType, timeoutSeconds, certPath, certPassword, host);

            // Post data
            byte[] data = System.Text.Encoding.UTF8.GetBytes(content);
            request.ContentLength = data.Length;
            using (var reqStream = await request.GetRequestStreamAsync().ConfigureAwait(false))
            {
                await reqStream.WriteAsync(data, 0, data.Length).ConfigureAwait(false);
            }

            // Read server response
            using (var response = (HttpWebResponse)(await request.GetResponseAsync().ConfigureAwait(false)))
            {
                using (var stream = response.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
                    {
                        return sr.ReadToEnd().Trim();
                    }
                }
            }
        }

        public static async Task<string> PostFileAsync(string url, string fileName, Stream fileStream, string fileContentType = "image/jpeg", int timeoutSeconds = 15)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            if (timeoutSeconds != -1)
            {
                request.Timeout = timeoutSeconds * 1000;
            }

            // Create a random boundary
            string boundary = "---------------------" + DateTime.Now.Ticks.ToString("x");

            string formHeader = "--" + boundary + "\r\n"
                                    + "Content-Disposition: form-data; name=\"file\"; filename=\"" + Path.GetFileName(fileName) + "\"\r\n"
                                    + "Content-Type: " + fileContentType + "\r\n"
                                    + "\r\n";
            byte[] formHeaderBytes = Encoding.UTF8.GetBytes(formHeader);
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");

            // Set more request headers
            request.ContentType = "multipart/form-data; charset=utf-8; boundary=" + boundary;
            request.ContentLength = formHeaderBytes.Length + boundaryBytes.Length + fileStream.Length;

            using (var requestStream = await request.GetRequestStreamAsync().ConfigureAwait(false))
            {
                requestStream.Write(formHeaderBytes, 0, formHeaderBytes.Length);
                fileStream.CopyTo(requestStream);
                requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
            }

            // Read response
            using (var httpResponse = (HttpWebResponse)(await request.GetResponseAsync().ConfigureAwait(false)))
            {
                using (var stream = httpResponse.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
                    {
                        return sr.ReadToEnd().Trim();
                    }
                }
            }
        }

        public static string PostFile(string url, string fileName, Stream fileStream, string fileContentType = "image/jpeg", int timeoutSeconds = 15)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            if (timeoutSeconds != -1)
            {
                request.Timeout = timeoutSeconds * 1000;
            }

            // Create a random boundary
            string boundary = "---------------------" + DateTime.Now.Ticks.ToString("x");

            string formHeader = "--" + boundary + "\r\n"
                                    + "Content-Disposition: form-data; name=\"file\"; filename=\"" + Path.GetFileName(fileName) + "\"\r\n"
                                    + "Content-Type: " + fileContentType + "\r\n"
                                    + "\r\n";
            byte[] formHeaderBytes = Encoding.UTF8.GetBytes(formHeader);
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");

            // Set more request headers
            request.ContentType = "multipart/form-data; charset=utf-8; boundary=" + boundary;
            request.ContentLength = formHeaderBytes.Length + boundaryBytes.Length + fileStream.Length;

            // Write content out.
            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(formHeaderBytes, 0, formHeaderBytes.Length);
                fileStream.CopyTo(requestStream);
                requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
            }

            // Read response
            using (var httpResponse = (HttpWebResponse)(request.GetResponse()))
            {
                using (var stream = httpResponse.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
                    {
                        return sr.ReadToEnd().Trim();
                    }
                }
            }
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            // Simply returns true.
            return true;
        }
    }
}
