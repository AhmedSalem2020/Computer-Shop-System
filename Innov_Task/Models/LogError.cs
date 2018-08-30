using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Innov_Task.Models
{
    public class LogError
    {
        public static void Error(Exception ex, string MethodName)
        {
            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            message += string.Format("Message: {0}", ex.Message);
            message += Environment.NewLine;
            if (ex.InnerException != null)
            {
                message += string.Format("Inner Exception: {0}", ex.InnerException);
                message += Environment.NewLine;
            }
            message += string.Format("Location: {0}", HttpContext.Current.Request.Url.LocalPath);
            message += Environment.NewLine;
            message += string.Format("Method: {0}", MethodName);
            message += Environment.NewLine;
            message += string.Format("Source: {0}", ex.Source);
            message += Environment.NewLine;
            message += string.Format("Target Site: {0}", ex.TargetSite);
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            string path = HttpContext.Current.Server.MapPath("~/ErrorLog.txt");
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(message);
                writer.Close();
            }
        }
    }
}