using System.Xml;
using System;
using System.Reflection;
using System.Web;
using System.Collections.Generic;
using System.Xml.XPath;
using log4net;
using System.Threading;

namespace ExamineSystem.utility
{

    public static class SettingConfigUtility
    {
        private enum SettingKey
        {
            SessionTimeout,
            UploadFileMaxSize,
            ExaminationTime,
            ExaminationQuestion
        }

        private static ILog logger = TrackLogManager.GetLogger(typeof(SettingConfigUtility));
        private static ReaderWriterLockSlim rwlock = new ReaderWriterLockSlim();
        private static Dictionary<SettingKey, String> settingInfoMapping = null;
        private static readonly string settingFileName = "setting.config";

        static SettingConfigUtility()
        {
            InitSettingInfoMapping();
        }

        private static void InitSettingInfoMapping()
        {
            if (settingInfoMapping == null)
            {
                try
                {
                    rwlock.EnterWriteLock();
                    if (settingInfoMapping != null)
                        return;
                    settingInfoMapping = LoadConfig(HttpRuntime.AppDomainAppPath + settingFileName);
                }
                finally
                {
                    rwlock.ExitWriteLock();
                }
            }
        }

        private static Dictionary<SettingKey, String> LoadConfig(string configFilePath)
        {
            Dictionary<SettingKey, String> mapping = new Dictionary<SettingKey, String>();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(configFilePath);
                XPathNavigator nav = doc.CreateNavigator();
                XPathNodeIterator docItr = nav.Select("//SystemSettings//add");
                foreach (XPathNavigator docNav in docItr)
                {
                    string settingKeyStr = docNav.GetAttribute("key", "").Trim();
                    if (string.IsNullOrEmpty(settingKeyStr))
                        continue;
                    try
                    {
                        SettingKey settingKey = (SettingKey)Enum.Parse(typeof(SettingKey), settingKeyStr, true);
                        if (mapping.ContainsKey(settingKey))
                            continue;
                        string settingValStr = docNav.GetAttribute("value", "").Trim();
                        mapping.Add(settingKey, settingValStr);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                return mapping;
            }
            catch (Exception e)
            {
                logger.Error(string.Format("Failed to load {0}", configFilePath), e);
            }
            return mapping;
        }


        private static void SaveConfig(string configFilePath)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlDeclaration desc = doc.CreateXmlDeclaration("1.0", "utf-8", null);
                doc.AppendChild(desc);
                XmlElement root = doc.CreateElement("SystemSettings");
                doc.AppendChild(root);
                if (settingInfoMapping != null)
                {
                    foreach (KeyValuePair<SettingKey, String> pair in settingInfoMapping)
                    {
                        XmlElement child = doc.CreateElement("add");
                        child.SetAttribute("key", pair.Key.ToString());
                        child.SetAttribute("value", (pair.Value ?? string.Empty));
                        root.AppendChild(child);
                    }
                }
                using (XmlTextWriter writer = new XmlTextWriter(configFilePath, null))
                {
                    writer.Formatting = Formatting.Indented;
                    doc.WriteTo(writer);
                    writer.Flush();
                    writer.Close();
                }
            }
            catch (Exception e)
            {
                logger.Error(string.Format("Failed to save {0}", configFilePath), e);
            }
        }

        private static string LoadConfigItem(SettingKey key)
        {
            InitSettingInfoMapping();
            try
            {
                rwlock.EnterReadLock();
                if (!settingInfoMapping.ContainsKey(key))
                    return string.Empty;
                return settingInfoMapping[key];
            }
            finally
            {
                rwlock.ExitReadLock();
            }
        }

        private static void ModifyConfigItem(SettingKey key, String value)
        {
            InitSettingInfoMapping();
            try
            {
                rwlock.EnterWriteLock();
                if (settingInfoMapping.ContainsKey(key))
                    settingInfoMapping[key] = value;
                else
                    settingInfoMapping.Add(key, value);
                SaveConfig(HttpRuntime.AppDomainAppPath + settingFileName);
            }
            finally
            {
                rwlock.ExitWriteLock();
            }
        }

        public static int SessionTimeout
        {
            get
            {
                string timeoutStr = LoadConfigItem(SettingKey.SessionTimeout);
                int timeout = 30;
                if (!int.TryParse(timeoutStr, out timeout))
                    timeout = 30;
                return timeout;
            }
            set
            {
                if (value < 1)
                    value = 30;
                ModifyConfigItem(SettingKey.SessionTimeout, value.ToString());
            }
        }

        public static int UploadFileMaxSize
        {
            get
            {
                string fileMaxSizeStr = LoadConfigItem(SettingKey.UploadFileMaxSize);
                int fileMaxSize = 10;
                if (!int.TryParse(fileMaxSizeStr, out fileMaxSize))
                    fileMaxSize = 10;
                return fileMaxSize;
            }
            set
            {
                if (value < 1)
                    value = 10;
                ModifyConfigItem(SettingKey.UploadFileMaxSize, value.ToString());
            }
        }


        public static int ExaminationTime
        {
            get
            {
                string examTimeStr = LoadConfigItem(SettingKey.ExaminationTime);
                int examTime = 1;
                if (!int.TryParse(examTimeStr, out examTime))
                    examTime = 1;
                return examTime;
            }
            set
            {
                if (value < 1)
                    value = 1;
                ModifyConfigItem(SettingKey.ExaminationTime, value.ToString());
            }
        }


        public static int ExaminationQuestion
        {
            get
            {
                string examQuestionStr = LoadConfigItem(SettingKey.ExaminationQuestion);
                int examQuestion = 5;
                if (!int.TryParse(examQuestionStr, out examQuestion))
                    examQuestion = 5;
                return examQuestion;
            }
            set
            {
                if (value < 1)
                    value = 5;
                ModifyConfigItem(SettingKey.ExaminationQuestion, value.ToString());
            }
        }


    }
}