#region Copyright
//
// Author: Pablo Ruiz García (pablo.ruiz@gmail.com)
//
// (C) Pablo Ruiz García 2011
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WkHtmlToXSharp
{


	public class ImageGlobalSettings
	{
		public string Quality { get; set; }

		//public byte[] In { get; set; }
        public string In { get; set; }
		public string Out { get; set; }
		public string Fmt { get; set; }

		public string ScreenWidth { get; set; }
		public string SmartWidth { get; set; }
		public string Quiet { get; set; }
		public string Transparent { get; set;}
		public CropSettings Crop { get; set;}
		public LoadSettings LoadPage{get; set;}
	
		public ImageGlobalSettings(){
			Fmt = "";
			Transparent = "false";
			Crop = new CropSettings ();
			Crop.Left=0;
			Crop.Top=0;
			Crop.Width=0;
			Crop.Height=0;
			Quiet = "true";
			LoadPage = new LoadSettings();
			LoadPage.ZoomFactor = 1.5;
		}
		// TODO: Add as many as you need..
	}

	public class CropSettings{
		public int Left { get; set;}
		public int Width { get; set;}
		public int Top { get; set;}
		public int Height { get; set;}
	}
}
