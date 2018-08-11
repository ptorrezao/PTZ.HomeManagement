using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PTZ.HomeManagement.Utils
{
    public static class CoreUtils
    {
        internal static void GetConfigFromEnviromentVariable<T>(string enviromentVar, T defaultValue, out T result)
        {
            string value = Environment.GetEnvironmentVariable(enviromentVar);
            result = defaultValue;

            if (string.IsNullOrEmpty(value))
            {
                Console.WriteLine($"No EnviromentVar({enviromentVar}) was found");
            }
            else
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

                if (converter.CanConvertFrom(typeof(string)))
                {
                    result = (T)converter.ConvertFromString(value);
                }
                else
                {
                    Console.WriteLine($"Wasn't possible convert EnviromentVar({enviromentVar}).");
                }
            }
        }
    }
}
