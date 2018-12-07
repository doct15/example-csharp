using System;
using System.Net;
using System.Threading;
using System.Linq;
using System.Text;
 
namespace SimpleWebServer
{
    public class WebServer
    {
        private readonly HttpListener _listener = new HttpListener();
 
        public WebServer()
        {
            if (!HttpListener.IsSupported)
                throw new NotSupportedException("Needs Windows XP SP2, Server 2003 or later.");
 
            _listener.Prefixes.Add("http://*:8080/");
        }
 
        public void start()
        {
            _listener.Start();
            for(;;)
            {
                HttpListenerContext ctx = _listener.GetContext();
                new Thread(new Worker(ctx).ProcessRequest).Start();
            }
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }

        static public void Main ()
        {
            WebServer webServer = new WebServer();
            webServer.start();
        }
    }

    public class Worker
    {
        private HttpListenerContext context;
 
       public Worker(HttpListenerContext context)
       {
          this.context = context;
       }
 
       public void ProcessRequest()
       {
          StringBuilder sb = new StringBuilder();
          sb.Append("<html><title>Puppet Pipelines CI/CD with Windows and C#</title><body><h1>Hello World from Puppet.</h1><br><p>Build, test, and deploy your Windows apps with Pipelines.</p></body></html>");
 
          byte[] b = Encoding.UTF8.GetBytes(sb.ToString());
          context.Response.ContentLength64 = b.Length;
          context.Response.OutputStream.Write(b, 0, b.Length);
          context.Response.OutputStream.Close();
       }
    }
}
