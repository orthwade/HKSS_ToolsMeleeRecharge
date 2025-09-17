// using HarmonyLib;

// namespace ToolsMeleeRecharge.Patches
// {
//     [HarmonyPatch(typeof(CurrencyManager))]
//     [HarmonyPatch("TakeCurrency")]
//     internal static class CurrencyManager_TakeCurrency_Patch
//     {
//         private static bool Prefix(int amount, CurrencyType type, bool showCounter)
//         {
//             // Prevent shard spending
//             if (type == CurrencyType.Shard && GlobalToolConfig.GetBenchRestore())
//                 return false; // skip original TakeCurrency

//             return true; // allow spending of all other currencies
//         }
//     }
// }
