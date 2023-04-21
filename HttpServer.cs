using GTAVWebhook.Types;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace GTAVWebhook
{
    public class HttpServer
    {
        public readonly string Host = "127.0.0.1";
        public readonly int Port = 6721;

        private Queue<CommandInfo> commandInfoQueue = new Queue<CommandInfo>();

        private HttpListener _listener;

        public void Start()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://" + Host + ":" + Port.ToString() + "/");
            _listener.Start();
            Receive();
        }

        public void Stop()
        {
            _listener.Stop();
        }

        private void Receive()
        {
            _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
        }

        public CommandInfo DequeueCommand()
        {
            if (commandInfoQueue.Count > 0)
            {
                return commandInfoQueue.Dequeue();
            }
            else
            {
                return null;
            }
        }

        private void ListenerCallback(IAsyncResult result)
        {
            if (_listener.IsListening)
            {
                var context = _listener.EndGetContext(result);
                var request = context.Request;

                // do something with the request
                Console.WriteLine($"{request.Url}");

                byte[] responseData = new byte[0];

                if (request.HttpMethod == "OPTIONS" || request.RawUrl == "/favicon.ico")
                {
                    // ignore
                }
                else if (request.RawUrl == "/logs")
                {
                    responseData = Encoding.UTF8.GetBytes(
                        $"<h1>Logs</h1>" +
                        $"<pre>{Logger.GetLogContents()}</pre>"
                     );
                }
                else
                {
                    string debugInfo = "";
                    string rawPostData = null;

                    NameValueCollection parsedPostData = new NameValueCollection();

                    // Parse body if present
                    if (request.HttpMethod == "POST" && request.HasEntityBody)
                    {
                        var body = request.InputStream;
                        var encoding = request.ContentEncoding;
                        var reader = new StreamReader(body, encoding);
                        rawPostData = reader.ReadToEnd();
                        reader.Close();
                        body.Close();

                        parsedPostData = HttpUtility.ParseQueryString(rawPostData);
                    }

                    Logger.Log("Process HttpRequest: URL=" + request.RawUrl + " Data=" + (rawPostData ?? ""));


                    string[] urlSplit = request.RawUrl.Substring(1).Split(':');

                    if (urlSplit[0].Length > 0)
                    {
                        string cmd = urlSplit[0];
                        string username = parsedPostData["username"] ?? "";
                        string custom = "";

                        if (urlSplit.Length == 2)
                        {
                            custom = urlSplit[1].Trim();
                        }

                        debugInfo = $"<p>The following command has been executed with this request:</p>" +
                            $"<pre>" +
                            $"COMMAND: {cmd}\n" +
                            $"USERNAME: {username}\n" +
                            $"CUSTOM: {custom}" +
                            $"</pre>";

                        commandInfoQueue.Enqueue(new CommandInfo() { cmd = cmd, username = username, custom = custom });
                    }

                    responseData = Encoding.UTF8.GetBytes(
                        $"<h1>Its working!</h1>" +
                        $"<p>The GTA 5 integration is running!</p>" +
                        $"<p>Visit <a href=\"https://github.com/smeysu/GTA5-TikTok-Integration\">https://github.com/smeysu/GTA5-TikTok-Integration</a> to learn how to use this plugin.</p>" +
                        $"<p>{debugInfo}</p>" +
                        $"<a href=\"/logs\">Show Logs</a>"
                      );
                }

                // write response
                HttpListenerResponse response = context.Response;
                response.StatusCode = (int)HttpStatusCode.OK;
                response.ContentType = "text/html; charset=UTF-8";
                response.Headers.Add("Access-Control-Allow-Origin: *");
                response.Headers.Add("Access-Control-Allow-Methods: GET,POST");
                response.OutputStream.Write(responseData, 0, responseData.Length);
                response.OutputStream.Close();

                Receive();
            }
        }
    }
}
