﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace WkHtmlToXSharp
{
	internal static class ImageNativeCalls
	{
		private const string DLL_NAME = "wkhtmltox0";

		static ImageNativeCalls()
		{
			// Deploy native assemblies..
			WkHtmlToXLibrariesManager.InitializeNativeLibrary();
		}

		#region Linux/Libc P/Invokes
		[DllImport("libc")]
		public static extern int uname(IntPtr buf);
		#endregion

		#region Linux/Libc Call Wrappers
		private static object _OsNameLock = new object();
		private static string _OsName = null;

		private static string GetOsNameInternal()
		{
			if (!string.IsNullOrEmpty(_OsName))
				return _OsName;

			if (Environment.OSVersion.Platform != PlatformID.Unix
				&& Environment.OSVersion.Platform != PlatformID.MacOSX)
			{
				return Environment.OSVersion.Platform.ToString();
			}

			IntPtr buf = IntPtr.Zero;
			try
			{
				buf = Marshal.AllocHGlobal(8192);
				// This is a hacktastic way of getting sysname from uname ()
				if (uname(buf) == 0)
				{
					return Marshal.PtrToStringAnsi(buf);
				}
			}
			catch { }
			finally
			{
				if (buf != IntPtr.Zero) Marshal.FreeHGlobal(buf);
			}

			return null;
		}

		public static string GetOsName()
		{
			lock (_OsNameLock)
			{
				if (_OsName == null)
					_OsName = GetOsNameInternal();

				return _OsName;
			}
		}
		#endregion

		#region wkhtmltoimage P/Invokes
		[DllImport(DLL_NAME)]
		public static extern IntPtr wkhtmltoimage_version();

		[DllImport(DLL_NAME)]
		public static extern int wkhtmltoimage_init(int use_graphics);

		[DllImport(DLL_NAME)]
		public static extern int wkhtmltoimage_deinit();

		[DllImport(DLL_NAME)]
		public static extern int wkhtmltoimage_extended_qt();

		[DllImport(DLL_NAME)]
		public static extern IntPtr wkhtmltoimage_create_global_settings();

		[DllImport(DLL_NAME)]
        public static extern IntPtr wkhtmltoimage_create_converter(IntPtr globalSettings, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Marshaler))] string htmlData);

		[DllImport(DLL_NAME)]
		public static extern IntPtr wkhtmltoimage_create_object_settings();

#if true
		[DllImport(DLL_NAME)]
		public static extern int wkhtmltoimage_set_global_setting(IntPtr globalSettings,
			[MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Marshaler))] string name,
			[MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Marshaler))] string value);


		[DllImport(DLL_NAME)]
		public static extern void wkhtmltoimage_add_object(IntPtr converter, IntPtr objectSettings,
			[MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Marshaler))] string htmlData);
#else
		[DllImport(DLL_NAME)]
		public static extern int wkhtmltoimage_set_global_setting(IntPtr globalSettings, string name, IntPtr value);

		[DllImport(DLL_NAME)]
		public static extern int wkhtmltoimage_set_object_setting(IntPtr objectSettings, string name, IntPtr value);

		[DllImport(DLL_NAME)]
		public static extern void wkhtmltoimage_add_object(IntPtr converter, IntPtr objectSettings, IntPtr htmlData);
#endif

		[DllImport(DLL_NAME)]
		public static extern int wkhtmltoimage_convert(IntPtr converter);

		[DllImport(DLL_NAME)]
		public static extern IntPtr wkhtmltoimage_get_output(IntPtr converter, out IntPtr data);

		[DllImport(DLL_NAME)]
		public static extern void wkhtmltoimage_destroy_converter(IntPtr converter);

		[DllImport(DLL_NAME)]
		public static extern int wkhtmltoimage_current_phase(IntPtr converter);

		[DllImport(DLL_NAME)]
		public static extern int wkhtmltoimage_phase_count(IntPtr converter);

		[DllImport(DLL_NAME)]
		// NOTE: Using IntPtr as return to avoid runtime from freeing returned string. (pruiz)
		public static extern IntPtr wkhtmltoimage_phase_description(IntPtr converter, int phase);

		[DllImport(DLL_NAME)]
		// NOTE: Using IntPtr as return to avoid runtime from freeing returned string. (pruiz)
		public static extern IntPtr wkhtmltoimage_progress_string(IntPtr converter);

		[DllImport(DLL_NAME)]
		public static extern int wkhtmltoimage_http_error_code(IntPtr converter);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void wkhtmltoimage_str_callback(IntPtr converter, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Marshaler))] string str);
		//public delegate void wkhtmltoimage_str_callback(IntPtr converter, IntPtr str);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void wkhtmltoimage_int_callback(IntPtr converter, int val);
		//[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		//public delegate void wkhtmltoimage_bool_callback(IntPtr converter, bool val);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void wkhtmltoimage_void_callback(IntPtr converter);

		[DllImport(DLL_NAME)]
		public static extern void wkhtmltoimage_set_error_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] wkhtmltoimage_str_callback cb);

		[DllImport(DLL_NAME)]
		public static extern void wkhtmltoimage_set_warning_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] wkhtmltoimage_str_callback cb);

		[DllImport(DLL_NAME)]
		public static extern void wkhtmltoimage_set_phase_changed_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] wkhtmltoimage_void_callback cb);

		[DllImport(DLL_NAME)]
		public static extern void wkhtmltoimage_set_progress_changed_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] wkhtmltoimage_int_callback cb);

		[DllImport(DLL_NAME)]
        public static extern void wkhtmltoimage_set_finished_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] wkhtmltoimage_int_callback cb);
		#endregion

		#region wkhtmltoimage Call Wrappers
		public static string wkhtmltoimageVersion()
		{
			var ptr = wkhtmltoimage_version();
			return Marshal.PtrToStringAnsi(ptr);
		}
		#endregion
	}
}
