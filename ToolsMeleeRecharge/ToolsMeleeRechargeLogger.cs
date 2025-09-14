using BepInEx.Logging;
using UnityEngine;

namespace ToolsMeleeRecharge
{
    /// <summary>
    /// Centralized logger for the Tools Melee Recharge plugin.
    /// </summary>
    public static class Logger
    {
        // Logging is enabled by default
        public static bool enableLogging = true;
        private static ManualLogSource logger;

        public static void Init()
        {
            if (logger == null)
                logger = BepInEx.Logging.Logger.CreateLogSource("ToolsMeleeRecharge");
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
    }
}
