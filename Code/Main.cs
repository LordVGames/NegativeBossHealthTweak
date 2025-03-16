using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.UI;

namespace NegativeBossHealthTweak
{
    internal static class Main
    {
        private const int integerLimit = 2147483647;

        internal static void HUDBossHealthBarController_LateUpdate(ILContext il)
        {
            ILCursor c = new(il);

            if (!c.TryGotoNext(MoveType.AfterLabel,
                x => x.MatchLdarg(0),
                x => x.MatchLdfld<HUDBossHealthBarController>("healthLabel")
            ))
            {
                Log.Error("COULD NOT IL HOOK IL.RoR2.UI.HUDBossHealthBarController.LateUpdate PART 1");
                LogILStuff(il, c);
            }
            ILLabel skipOldStringBuild = c.DefineLabel();
            c.MarkLabel(skipOldStringBuild);
            c.Index = 0;



            if (!c.TryGotoNext(MoveType.AfterLabel,
                x => x.MatchLdsfld<HUDBossHealthBarController>("sharedStringBuilder"),
                x => x.MatchCallvirt<StringBuilder>("Clear")
            ))
            {
                Log.Error("COULD NOT IL HOOK IL.RoR2.UI.HUDBossHealthBarController.LateUpdate PART 2");
                LogILStuff(il, c);
            }

            c.Emit(OpCodes.Ldarg_0);
            c.Emit<HUDBossHealthBarController>(OpCodes.Ldsfld, "sharedStringBuilder");
            c.Emit(OpCodes.Ldloc_1);
            c.Emit(OpCodes.Ldloc_2);
            c.EmitDelegate<Action<HUDBossHealthBarController, StringBuilder, float, float>>((healthBarController, sharedStringBuilder, totalObservedHealth, totalMaxObservedMaxHealth) =>
            {
                sharedStringBuilder.Clear();

                if (totalObservedHealth > integerLimit)
                {
                    sharedStringBuilder.Append("??????????");
                }
                else
                {
                    // the old health values got set to the new health ones so i cann just use the old ones here
                    sharedStringBuilder.AppendInt(healthBarController.oldTotalHealth);
                }

                sharedStringBuilder.Append('/');

                if (totalMaxObservedMaxHealth > integerLimit)
                {
                    sharedStringBuilder.Append("??????????");
                }
                else
                {
                    sharedStringBuilder.AppendInt(healthBarController.oldTotalMaxHealth);
                }
            });
            c.Emit(OpCodes.Br, skipOldStringBuild);

#if DEBUG
            LogILStuff(il, c);
#endif
        }



        private static void LogILStuff(ILContext il, ILCursor c)
        {
            Log.Warning($"cursor is {c}");
            Log.Warning($"il is {il}");
        }
    }
}