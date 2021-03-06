﻿using System;
using System.ServiceProcess;

namespace Communication
{
    public class Utils
    {
        public static bool IsServiceActive(string serviceName)
        {
            try
            {
                using (ServiceController sc = new ServiceController(serviceName))
                {
                    return sc.Status == ServiceControllerStatus.Running;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Credit: https://stackoverflow.com/a/13266776
        public static string AbsoluteToRelativePath(string pathToFile, string referencePath)
        {
            Uri fileUri = new Uri(pathToFile);
            Uri referenceUri = new Uri(referencePath);
            return referenceUri.MakeRelativeUri(fileUri).ToString();
        }

        public static string TruncateString(string fullstring, int threshold, double ratio)
        {
            int c = (int) (threshold * ratio);
            if (fullstring.Length > threshold)
            {
                return fullstring.Substring(0, threshold - c) + "..." +
                       fullstring.Substring(fullstring.Length - c, c);
            }

            return fullstring;
        }
    }
}