using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace EbonsContentMod.Utilities
{
    internal class TextureExporterator
    {
        // Skin Ramps
        
        internal static List<Texture2D> humanHeadRamps = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString()).MaleOptions.Heads[0].Load(true, false).PrimaryRamps;
        internal static List<Texture2D> tieflingHeadRamps = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.TieflingRace.ToString()).MaleOptions.Heads[0].Load(true, false).PrimaryRamps;
        internal static List<Texture2D> dhampirHeadRamps = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.DhampirRace.ToString()).MaleOptions.Heads[0].Load(true, false).PrimaryRamps;
        internal static List<Texture2D> oreadHeadRamps = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.OreadRace.ToString()).MaleOptions.Heads[0].Load(true, false).PrimaryRamps;
        internal static List<Texture2D> elfHeadRamps = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.ElfRace.ToString()).MaleOptions.Heads[0].Load(true, false).PrimaryRamps;
        internal static List<Texture2D> gnomeHeadRamps = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.GnomeRace.ToString()).MaleOptions.Heads[0].Load(true, false).PrimaryRamps;
        internal static List<Texture2D> aasimarHeadRamps = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.AasimarRace.ToString()).MaleOptions.Heads[0].Load(true, false).PrimaryRamps;
        internal static List<Texture2D> halfElfHeadRamps = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HalfElfRace.ToString()).MaleOptions.Heads[0].Load(true, false).PrimaryRamps;

        // Eye Ramps

        internal static List<Texture2D> humanEyeRamps = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString()).MaleOptions.Heads[0].Load(true, false).SecondaryColorsProfile.Ramps;
        internal static List<Texture2D> tieflingEyeRamps = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.TieflingRace.ToString()).MaleOptions.Heads[0].Load(true, false).SecondaryColorsProfile.Ramps;
        internal static List<Texture2D> dhampirEyeRamps = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.DhampirRace.ToString()).MaleOptions.Heads[0].Load(true, false).SecondaryColorsProfile.Ramps;
        internal static List<Texture2D> oreadEyeRamps = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.OreadRace.ToString()).MaleOptions.Heads[0].Load(true, false).SecondaryColorsProfile.Ramps;
        internal static List<Texture2D> elfEyeRamps = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.ElfRace.ToString()).MaleOptions.Heads[0].Load(true, false).SecondaryColorsProfile.Ramps;
        internal static List<Texture2D> gnomeEyeRamps = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.GnomeRace.ToString()).MaleOptions.Heads[0].Load(true, false).SecondaryColorsProfile.Ramps;
        internal static List<Texture2D> aasimarEyeRamps = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.AasimarRace.ToString()).MaleOptions.Heads[0].Load(true, false).SecondaryColorsProfile.Ramps;
        internal static List<Texture2D> halfElfEyeRamps = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HalfElfRace.ToString()).MaleOptions.Heads[0].Load(true, false).SecondaryColorsProfile.Ramps;

        // Hair Ramps

        internal static List<Texture2D> humanHairRamps = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString()).MaleOptions.Hair[0].Load(true, false).PrimaryColorsProfile.Ramps;
        internal static List<Texture2D> tieflingHairRamps = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.TieflingRace.ToString()).MaleOptions.Hair[0].Load(true, false).PrimaryColorsProfile.Ramps;
        internal static List<Texture2D> dhampirHairRamps = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.DhampirRace.ToString()).MaleOptions.Hair[0].Load(true, false).PrimaryColorsProfile.Ramps;
        internal static List<Texture2D> oreadHairRamps = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.OreadRace.ToString()).MaleOptions.Hair[0].Load(true, false).PrimaryColorsProfile.Ramps;
        internal static List<Texture2D> elfHairRamps = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.ElfRace.ToString()).MaleOptions.Hair[0].Load(true, false).PrimaryColorsProfile.Ramps;
        internal static List<Texture2D> gnomeHairRamps = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.GnomeRace.ToString()).MaleOptions.Hair[0].Load(true, false).PrimaryColorsProfile.Ramps;
        internal static List<Texture2D> aasimarHairRamps = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.AasimarRace.ToString()).MaleOptions.Hair[0].Load(true, false).PrimaryColorsProfile.Ramps;
        internal static List<Texture2D> halfElfHairRamps = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HalfElfRace.ToString()).MaleOptions.Hair[0].Load(true, false).PrimaryColorsProfile.Ramps;

        internal static List<BlueprintRace> raceBlueprints =
        [
            RaceRefs.HumanRace.Reference.Get(),
            RaceRefs.TieflingRace.Reference.Get(),
            RaceRefs.DhampirRace.Reference.Get(),
            RaceRefs.OreadRace.Reference.Get(),
            RaceRefs.ElfRace.Reference.Get(),
            RaceRefs.GnomeRace.Reference.Get(),
            RaceRefs.AasimarRace.Reference.Get(),
            RaceRefs.HalfElfRace.Reference.Get()
        ];

        internal static List<Texture2D> GetRaceSkinRampList(BlueprintRace race)
        {
            return race switch
            {
                var r when r == RaceRefs.HumanRace.Reference.Get() => humanHeadRamps,
                var r when r == RaceRefs.TieflingRace.Reference.Get() => tieflingHeadRamps,
                var r when r == RaceRefs.DhampirRace.Reference.Get() => dhampirHeadRamps,
                var r when r == RaceRefs.OreadRace.Reference.Get() => oreadHeadRamps,
                var r when r == RaceRefs.ElfRace.Reference.Get() => elfHeadRamps,
                var r when r == RaceRefs.GnomeRace.Reference.Get() => gnomeHeadRamps,
                var r when r == RaceRefs.AasimarRace.Reference.Get() => aasimarHeadRamps,
                var r when r == RaceRefs.HalfElfRace.Reference.Get() => halfElfHeadRamps,
                _ => new List<Texture2D>()
            };
        }

        internal static List<Texture2D> GetRaceEyeRampList(BlueprintRace race)
        {
            return race switch
            {
                var r when r == RaceRefs.HumanRace.Reference.Get() => humanEyeRamps,
                var r when r == RaceRefs.TieflingRace.Reference.Get() => tieflingEyeRamps,
                var r when r == RaceRefs.DhampirRace.Reference.Get() => dhampirEyeRamps,
                var r when r == RaceRefs.OreadRace.Reference.Get() => oreadEyeRamps,
                var r when r == RaceRefs.ElfRace.Reference.Get() => elfEyeRamps,
                var r when r == RaceRefs.GnomeRace.Reference.Get() => gnomeEyeRamps,
                var r when r == RaceRefs.AasimarRace.Reference.Get() => aasimarEyeRamps,
                var r when r == RaceRefs.HalfElfRace.Reference.Get() => halfElfEyeRamps,
                _ => new List<Texture2D>()
            };
        }

        internal static List<Texture2D> GetRaceHairRampList(BlueprintRace race)
        {
            return race switch
            {
                var r when r == RaceRefs.HumanRace.Reference.Get() => humanHairRamps,
                var r when r == RaceRefs.TieflingRace.Reference.Get() => tieflingHairRamps,
                var r when r == RaceRefs.DhampirRace.Reference.Get() => dhampirHairRamps,
                var r when r == RaceRefs.OreadRace.Reference.Get() => oreadHairRamps,
                var r when r == RaceRefs.ElfRace.Reference.Get() => elfHairRamps,
                var r when r == RaceRefs.GnomeRace.Reference.Get() => gnomeHairRamps,
                var r when r == RaceRefs.AasimarRace.Reference.Get() => aasimarHairRamps,
                var r when r == RaceRefs.HalfElfRace.Reference.Get() => halfElfHairRamps,
                _ => new List<Texture2D>()
            };
        }

        internal static void ExportSkinRampsForRace(BlueprintRace race)
        {
            var ramps = GetRaceSkinRampList(race);

            var raceName = race.name;
            
            foreach (Texture2D ramp in ramps)
            {
                var name = ramp.name;
                
                SpriteHelperators.ExportTexture(ramp, "SkinRamp_" + raceName + name + ".png");
            }
        }

        internal static void ExportEyeRampsForRace(BlueprintRace race)
        {
            var ramps = GetRaceEyeRampList(race);

            var raceName = race.name;

            foreach (Texture2D ramp in ramps)
            {
                var name = ramp.name;

                SpriteHelperators.ExportTexture(ramp, "EyeRamp_" + raceName + name + ".png");
            }
        }

        internal static void ExportHairRampsForRace(BlueprintRace race)
        {
            var ramps = GetRaceHairRampList(race);

            var raceName = race.name;

            foreach (Texture2D ramp in ramps)
            {
                var name = ramp.name;

                SpriteHelperators.ExportTexture(ramp, "HairRamp_" + raceName + name + ".png");
            }
        }

        internal static void ExportAllRampsForRace(BlueprintRace race)
        {
            ExportSkinRampsForRace(race);
            ExportEyeRampsForRace(race);
            ExportHairRampsForRace(race);
        }

        internal static void ExportAllRamps()
        {
            foreach (BlueprintRace bp in raceBlueprints)
            {
                ExportAllRampsForRace(bp);
            }
        }
    }
}
