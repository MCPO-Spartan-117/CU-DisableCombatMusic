//using System;
//using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
//using UnityEngine;
//using CUCoreLib.Data;
//using CUCoreLib.Helpers;
//using CUCoreLib.Registries;
//using CUCoreLib.Saving;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace MCPO {
	[HarmonyPatch(typeof(MusicManager), nameof(MusicManager.Update))]
	class Update {
		static ManualLogSource Logger = MCPO.Plugin.Logger;
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
			Logger.LogInfo("Disabling Combat Music");
			var startidx = -1;
			var endidx = -1;
			bool fieldfound = false;
			List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
			for (var i = 0; i < codes.Count; i++) {
				if (!fieldfound) {
					if(codes[i].LoadsField(AccessTools.Field(typeof(PlayerCamera), nameof(PlayerCamera.main)))) {
						startidx = i;
						Logger.LogDebug("Found start " + i);
					} else if (codes[i].LoadsField(AccessTools.Field(typeof(PlayerCamera), nameof(PlayerCamera.threatMusicTime)))) {
						fieldfound = true;
						Logger.LogDebug("Found field " + i);
					}
				} else {
					if(codes[i].opcode == OpCodes.Stelem_R4) {
						endidx = i;
						Logger.LogDebug("Found end " + i);
						break;
					} else if(codes[i].IsLdarg()) {
						Logger.LogDebug("Past end " + i);
						break;
					}
				}
			}

			if (endidx != -1) {
				List<Label> iflabels = codes[startidx].labels;
				for(var i = endidx; i < codes.Count; i++) {
					if(codes[i].labels.Count > 0) {
						foreach(Label lab in iflabels) {
							codes[i].labels.Add(lab);
						}
						break;
					}
				}

				codes.RemoveRange(startidx, endidx - startidx + 1);
			} else {
				Logger.LogError("Not patching");
			}

			return (IEnumerable<CodeInstruction>)codes;
		}
	}
}
