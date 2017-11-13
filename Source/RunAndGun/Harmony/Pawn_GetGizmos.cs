﻿using System;
using System.Collections.Generic;
using System.Linq;

using Harmony;
using Verse;
using UnityEngine;
using RimWorld;

namespace RunAndGun.Harmony
{

    [HarmonyPatch(typeof(Pawn), "GetGizmos")]
    public class Pawn_DraftController_GetGizmos_Patch
    {
        public static void Postfix(ref IEnumerable<Gizmo> __result, ref Pawn __instance)
        {

            if (__instance == null || !__instance.Drafted || !__instance.Faction.Equals(Faction.OfPlayer) || !WorkGiver_HunterHunt.HasHuntingWeapon(__instance))
            {
                return;
            }
            if (__result == null || !__result.Any())
                return;


            CompRunAndGun data = __instance.TryGetComp<CompRunAndGun>();

            if(data == null)
            {
                return;
            }

            String uiElement = "enable_RG";
            String label = data.isEnabled ? "RG_Action_Disable_Label".Translate() : "RG_Action_Enable_Label".Translate();
            String description = data.isEnabled ? "RG_Action_Disable_Description".Translate() : "RG_Action_Enable_Description".Translate();


            var gizmoList = __result.ToList();
            var ourGizmo = new Command_Toggle
            {
                defaultLabel = label,
                defaultDesc = description,
                icon = ContentFinder<Texture2D>.Get(("UI/Buttons/" + uiElement), true),
                isActive = () => data.isEnabled,
                toggleAction = () => { data.isEnabled = !data.isEnabled; } 
            };


            gizmoList.Add(ourGizmo);
            __result = gizmoList;
        }
    }

}