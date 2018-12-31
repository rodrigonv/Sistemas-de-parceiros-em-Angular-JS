using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Optimus.Web.Parceiros.RestServer.Util
{
    public class Log
    {
        public static ILog For(object LoggedObject, string pastacliente)
        {
            if (string.IsNullOrEmpty(pastacliente))
            {

                XmlConfigurator.Configure();
            }
            else
            {
                XmlConfigurator.Configure(ConfiglogManual(pastacliente));
            }

            if (LoggedObject != null)
                return For(LoggedObject.GetType(), pastacliente);
            else
                return For(null, pastacliente);
        }

        public static ILog For(Type ObjectType, string pastacliente)
        {
            if (string.IsNullOrEmpty(pastacliente))
            {
                XmlConfigurator.Configure();
            }
            else
            {
                XmlConfigurator.Configure(ConfiglogManual(pastacliente));
            }

            if (ObjectType != null)
                return LogManager.GetLogger(ObjectType.Name);
            else
                return LogManager.GetLogger(string.Empty);
        }

        public static Stream ConfiglogManual(string pastacliente)
        {
            string x = string.Format(@"
<?xml version=""1.0"" standalone=""yes""?>
<log4net>
<appender name=""TestLogFileAppender"" type=""log4net.Appender.RollingFileAppender"">
<file value=""{0}\log.txt"" />
<appendToFile value=""true"" />
<rollingStyle value=""Size"" />
<filter type=""log4net.Filter.LevelRangeFilter"">
<acceptOnMatch value=""true"" />
<levelMin value=""DEBUG"" />
<levelMax value=""FATAL"" />
</filter>
<maxSizeRollBackups value=""5"" />
<maximumFileSize value=""1MB"" />
<staticLogFileName value=""true"" />
<lockingModel type=""log4net.Appender.FileAppender+MinimalLock"" />
<layout type=""log4net.Layout.PatternLayout"">
<conversionPattern value=""%newline%date [%thread] %-5level - %message"" />
</layout>
</appender>
<root>
<level value=""ALL"" />
<appender-ref ref=""TestLogFileAppender"" />
</root>
</log4net>", pastacliente);

            return new MemoryStream(ASCIIEncoding.Default.GetBytes(x.Trim()));
        }

        public enum TipoLog
        {
            info = 0,
            erro = 1,
            warn = 2
        }
    }

    public class LogUtil
    {
        public static void GravaLog(object objlog, string msglog, string Cnpj, Optimus.Web.Parceiros.RestServer.Util.Log.TipoLog tplog)
        {
            switch (tplog)
            {
                case Log.TipoLog.info:
                    Log.For(objlog, ConfigurationManager.AppSettings.Get("PASTA_LOG_GLOBAL").ToString() + "\\" + Cnpj).Info(msglog);
                    break;
                case Log.TipoLog.erro:
                    Log.For(objlog, ConfigurationManager.AppSettings.Get("PASTA_LOG_GLOBAL").ToString() + "\\" +Cnpj).Error(msglog);
                    break;
                case Log.TipoLog.warn:
                    Log.For(objlog, ConfigurationManager.AppSettings.Get("PASTA_LOG_GLOBAL").ToString() + "\\" + Cnpj).Warn(msglog);
                    break;
                default:
                    break;
            }


        }

    }
}