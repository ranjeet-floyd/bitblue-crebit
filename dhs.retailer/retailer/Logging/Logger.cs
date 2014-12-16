using log4net;
using log4net.Config;
using System;

namespace api.dhs.Logging
{
    public enum LogLevelL4N
    {
        DEBUG = 1,
        ERROR = 2,
        FATAL = 3,
        INFO = 4,
        WARN = 5
    }
    public class Logger
    {
        #region Members
        private static readonly ILog logger = LogManager.GetLogger(typeof(Logger));
        #endregion

        #region Constructors
        static Logger()
        {
            XmlConfigurator.Configure();
        }
        #endregion

        #region Methods

        public static void WriteLog(LogLevelL4N logLevel, String log)
        {
            if (logLevel.Equals(LogLevelL4N.DEBUG))
            {
                logger.Debug(log);
            }

            else if (logLevel.Equals(LogLevelL4N.ERROR))
            {
                logger.Error(log);
            }

            else if (logLevel.Equals(LogLevelL4N.FATAL))
            {
                logger.Fatal(log);
            }

            else if (logLevel.Equals(LogLevelL4N.INFO))
            {
                logger.Info(log);
            }

            else if (logLevel.Equals(LogLevelL4N.WARN))
            {

                logger.Warn(log);

            }

        }

        #endregion

    }
}