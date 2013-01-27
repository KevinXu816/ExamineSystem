using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using ExamineSystem.utility.eslog;

namespace ExamineSystem.utility
{
    public static class TrackLogManager
    {
        private static Dictionary<String, ESLogger> map = new Dictionary<string, ESLogger>();
        private static object lockObj = new object();

        public static ILog GetLogger(Type type)
        {
            if (type == null) return null;
            String typeName = type.FullName;
            lock (lockObj)
            {
                if (map.ContainsKey(typeName))
                {
                    return map[typeName];
                }
                else
                {
                    ESLogger logger = new ESLogger();
                    logger.Type = type;
                    map.Add(typeName, logger);
                    return logger;
                }
            }
        }
    }
}