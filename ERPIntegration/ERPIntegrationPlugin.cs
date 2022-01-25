using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace ERPIntegration
{
    public class ERPIntegrationPlugin
    {
        public static IERPIntegration GetERPIntegration(string url) 
        {
            return GetInstance<IERPIntegration>(url);
        }
        
        public static T GetInstance<T>(string URL) {
            T tmpInstance;
            tmpInstance = default(T);
            if (!File.Exists(URL)) {
                throw new Exception("File doesn\'t exist");
            }
            
            try {
                //Type assemblyType;
                foreach (Type assemblyType in Assembly.LoadFrom(URL).GetTypes()) {
                    
                    if (!(assemblyType.GetInterface(typeof(T).FullName) == null)) {
                        tmpInstance = (T)Activator.CreateInstance(assemblyType);
                        break;
                    }
                    
                }
                
            }
            catch (TargetInvocationException exp) {
                if (!(exp.InnerException == null)) {
                    throw exp.InnerException;
                }
                
            }
            
            return tmpInstance;
        }
    
        /*public static List<T> GetInstances<T>(List<string> urlList, ref List<string> lstFileNotExists)
    {
        T tmpInstance = null;
        List<T> lstInstance = new List<T>();
        foreach (string URL in urlList)
        {
            if (!File.Exists(URL))
            {
                lstFileNotExists.Add(URL);
            }

            tmpInstance = GetInstance(T)[URL];
            if (tmpInstance)
            {
                IsNot;
                null;
                lstInstance.Add(tmpInstance);
            }

        }

        return lstInstance;
    }*/

    }
}
