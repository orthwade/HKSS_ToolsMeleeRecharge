using BepInEx.Logging;
using BepInEx.Configuration;
using System;

namespace owd
{
    public static class BepinexPluginLogger
    {
        private static bool enableLogging = true;
        private static ManualLogSource? logger = null;

        public static ManualLogSource GetLogger()
        {
            if (logger == null)
            {
                throw new Exception("Logger not initialized. Call Init() first.");
            }

            return logger;
        }


        public static void Init(ConfigFile config, string pluginName)
        {
            if (logger == null)
            {
                logger = BepInEx.Logging.Logger.CreateLogSource(pluginName);
                LoggingInit(config);
            }
        }

        public static void LogInfo(string message)
        {
            if (!enableLogging) return;
            logger.LogInfo(message);
        }

        public static void LogWarning(string message)
        {
            if (!enableLogging) return;
            logger.LogWarning(message);
        }

        public static void LogError(string message)
        {
            if (!enableLogging) return;
            logger.LogError(message);
        }

        private static ConfigEntry<bool> LoggingEnabled;
        private static void LoggingInit(ConfigFile config)
        {
            LoggingEnabled = config.Bind(
                "00 - General",
                "Logging",
                false,
                new ConfigDescription(
                    "Enable or disable logging to BepInEx log file",
                    null,
                    new ConfigurationManagerAttributes { Order = 1 }
                )
            );

            enableLogging = LoggingEnabled.Value;

            LoggingEnabled.SettingChanged += (sender, args) =>
            {
                enableLogging = LoggingEnabled.Value;
            };
        }
    }
}