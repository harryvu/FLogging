﻿using Flogging.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloggingConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			var fd = GetFlogDetail("starting application", null);
			Flogger.WriteDiagnostic(fd);

			var perfTracker = new PerfTracker("FloggingConsole_Execution", "", fd.UserName, fd.Location, fd.Product, fd.Layer);

			try
			{
				var ex = new Exception("Something bad has happened!");
				ex.Data.Add("input param", "nothing to see here");
				throw ex;
			}
			catch (Exception ex)
			{
				fd = GetFlogDetail("", ex);
				Flogger.WriteError(fd);
			}

			fd = GetFlogDetail("used flogging console", null);
			Flogger.WriteUsage(fd);

			fd = GetFlogDetail("stopping the application", null);
			Flogger.WriteDiagnostic(fd);

			perfTracker.Stop();
		}

		private static FlogDetail GetFlogDetail(string message, Exception ex)
		{
			const string Product = "Flogger";
			const string Location = "FloggingConsole"; // this application
			const string Layer = "Job"; // unattended executable invoked somehow
			var user = Environment.UserName;
			var hostname = Environment.MachineName;

			return new FlogDetail
			{
				Product = Product,
				Location = Location,
				Layer = Layer,
				UserName = user,
				Hostname = hostname,
				Message = message,
				Exception = ex
			};
		}
	}
}
