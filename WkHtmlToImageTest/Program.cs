using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WkHtmlToXSharp;

namespace WkHtmlToImageTest
{
    class Program
    {

        Program() { 
        }

        private static readonly global::Common.Logging.ILog _Log = global::Common.Logging.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static void TryRegisterLibraryBundles()
        {
            var ignore = Environment.GetEnvironmentVariable("WKHTMLTOXSHARP_NOBUNDLES");

            if (ignore == null || ignore.ToLower() != "true")
            {
                // Register all available bundles..
                WkHtmlToXLibrariesManager.Register(new Linux32NativeBundle());
                WkHtmlToXLibrariesManager.Register(new Linux64NativeBundle());
                WkHtmlToXLibrariesManager.Register(new Win32NativeBundle());
                WkHtmlToXLibrariesManager.Register(new Win64NativeBundle());
            }
        }

        static void Main(string[] args)
        {
            Program p = new Program();
            TryRegisterLibraryBundles();
            p.CanConvertFromStringToImage();
            Console.WriteLine("Finished!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            //Environment.Exit(0);
        }

        private MultiplexingImageConverter _GetImageConverter()
        {
            var obj = new MultiplexingImageConverter();
            obj.Begin += (s, e) => _Log.DebugFormat("Conversion begin, phase count: {0}", e.Value);
            //obj.Error += (s, e) => _Log.Error(e.Value);
            obj.Warning += (s, e) => _Log.Warn(e.Value);
            //obj.PhaseChanged += (s, e) => _Log.InfoFormat("PhaseChanged: {0} - {1}", e.Value, e.Value2);
            //obj.ProgressChanged += (s, e) => _Log.InfoFormat("ProgressChanged: {0} - {1}", e.Value, e.Value2);
            obj.Finished += (s, e) => _Log.InfoFormat("Finished status code: {0}", e.Value.ToString());
            return obj;
        }

        private void _SimpleImageConversion()
        {
            using (var wk = _GetImageConverter())
            {
                _Log.DebugFormat("Performing to image conversion..");

                wk.GlobalSettings.In = "";
                wk.GlobalSettings.Out = "";
                wk.GlobalSettings.Fmt = "jpg";
                wk.GlobalSettings.ScreenWidth = "1000";
                wk.GlobalSettings.Quality = "90";

                var files = Directory.GetFiles("C:\\pics\\");

                foreach (var f in files)
                {
                    var htmlf = "file:///" + f.Replace("\\", "/");
                    var tmp = wk.Convert("<div>Hello World!<img class='border: solid 1px red;' src='" + htmlf + "'></img></div>");
                    File.WriteAllBytes(@"c:\\temp\\img" + Guid.NewGuid().ToString() + ".jpg", tmp);
                }
               

                
            }
        }

        public void CanConvertFromStringToImage()
        {
           // for (int i = 0; i < 10000; i++)
           // {
                //_SimpleImageConversion();
                //_SimpleImageConversion();
                //_SimpleImageConversion();
                //_SimpleImageConversion();
                MultiplexingImageConverter.ShutDown();
            
           // }
        }
    }
}
