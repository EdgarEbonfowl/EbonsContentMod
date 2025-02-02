using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem;
using System;
using System.Collections.Generic;
using TabletopTweaks.Core.Utilities;
using EbonsContentMod.Utilities;
using UnityEngine;
using Kingmaker.ResourceLinks;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Utils;
using EbonsContentMod.Races.Skinwalkers.SkinwalkerHeritage;
using Kingmaker.Blueprints.Classes.Selection;
using BlueprintCore.Blueprints.Configurators.Items.Weapons;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Visual.CharacterSystem;
using EbonsContentMod.Components;

namespace EbonsContentMod.Races
{
    internal class Mongrel
    {
        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString());

        private static readonly string MongrelName = "EbonsMongrelRace";

        internal const string MongrelDisplayName = "Mongrel.Name";
        private static readonly string MongrelDescription = "Mongrel.Description";
        public static readonly string RaceGuid = "{F507A867-40DE-4000-9CBD-981C4458D07E}";

        internal const string MongrelHornsRequisiteFeatureDisplayName = "Mongrel.MongrelHornsRequisiteFeature.Name";

        internal const string MongrelEyesRequisiteFeatureDisplayName = "Mongrel.MongrelEyesRequisiteFeature.Name";

        internal const string MongrelEarsRequisiteFeatureDisplayName = "Mongrel.MongrelEarsRequisiteFeature.Name";

        internal const string MongrelTailRequisiteFeatureDisplayName = "Mongrel.MongrelTailRequisiteFeature.Name";

        internal const string MongrelMajorMutationDisplayName = "Mongrel.MajorMutation.Name";
        private static readonly string MongrelMajorMutationDescription = "Mongrel.MajorMutation.Description";

        internal const string StagseedDisplayName = "Mongrel.Stagseed.Name";
        private static readonly string StagseedDescription = "Mongrel.Stagseed.Description";

        internal const string RockskinDisplayName = "Mongrel.Rockskin.Name";
        private static readonly string RockskinDescription = "Mongrel.Rockskin.Description";

        internal const string ColdbloodDisplayName = "Mongrel.Coldblood.Name";
        private static readonly string ColdbloodDescription = "Mongrel.Coldblood.Description";

        internal const string ForestkinDisplayName = "Mongrel.Forestkin.Name";
        private static readonly string ForestkinDescription = "Mongrel.Forestkin.Description";

        internal const string BullbredDisplayName = "Mongrel.Bullbred.Name";
        private static readonly string BullbredDescription = "Mongrel.Bullbred.Description";

        internal const string MonkeybornDisplayName = "Mongrel.Monkeyborn.Name";
        private static readonly string MonkeybornDescription = "Mongrel.Monkeyborn.Description";

        internal const string SpiderheartDisplayName = "Mongrel.Spiderheart.Name";
        private static readonly string SpiderheartDescription = "Mongrel.Spiderheart.Description";

        internal const string RaptorwingDisplayName = "Mongrel.Raptorwing.Name";
        private static readonly string RaptorwingDescription = "Mongrel.Raptorwing.Description";

        internal const string MongrelMinorMutationDisplayName = "Mongrel.MinorMutation.Name";
        private static readonly string MongrelMinorMutationDescription = "Mongrel.MinorMutation.Description";

        internal const string MismatchedEarsIDisplayName = "Mongrel.MismatchedEarsI.Name";

        internal const string MismatchedEarsIIDisplayName = "Mongrel.MismatchedEarsII.Name";

        internal const string CataractsDisplayName = "Mongrel.Cataracts.Name";

        internal const string AasimarEyesDisplayName = "Mongrel.AasimarEyes.Name";

        internal const string KitsuneEyesDisplayName = "Mongrel.KitsuneEyes.Name";

        internal const string DhampirEyesDisplayName = "Mongrel.DhampirEyes.Name";

        internal const string OreadEyesDisplayName = "Mongrel.OreadEyes.Name";

        internal const string ElfEyesDisplayName = "Mongrel.ElfEyes.Name";

        internal const string TieflingEyesDisplayName = "Mongrel.TieflingEyes.Name";

        internal const string CatEyesDisplayName = "Mongrel.CatEyes.Name";

        internal const string MongrelHornsIDisplayName = "Mongrel.MongrelHornsI.Name";

        internal const string MongrelHornsIIDisplayName = "Mongrel.MongrelHornsII.Name";

        internal const string MongrelHornsIIIDisplayName = "Mongrel.MongrelHornsIII.Name";

        internal const string MongrelHornsIVDisplayName = "Mongrel.MongrelHornsIV.Name";

        internal const string MongrelHornsVDisplayName = "Mongrel.MongrelHornsV.Name";

        internal const string TieflingTailDisplayName = "Mongrel.TieflingTail.Name";

        internal const string NaturalArmorDisplayName = "Mongrel.NaturalArmor.Name";
        private static readonly string NaturalArmorDescription = "Mongrel.NaturalArmor.Description";

        internal const string SkilledDisplayName = "Mongrel.Skilled.Name";
        private static readonly string SkilledDescription = "Mongrel.Skilled.Description";

        internal static void Configure()
        {
            // Prerequisite features

            var MongrelHornsRequisite = FeatureConfigurator.New("MongrelHornsRequisiteFeature", "{35352024-876D-459C-9EA8-D248B96269E1}")
                .SetDisplayName(MongrelHornsRequisiteFeatureDisplayName)
                .SetDescription(MongrelMinorMutationDescription)
                .SetHideInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .SetRanks(1)
                .Configure();

            var MongrelEyesRequisite = FeatureConfigurator.New("MongrelEyesRequisiteFeature", "{ECD63E1F-54C6-4CBF-A2F0-357F15B7D245}")
                .SetDisplayName(MongrelEyesRequisiteFeatureDisplayName)
                .SetDescription(MongrelMinorMutationDescription)
                .SetHideInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .SetRanks(1)
                .Configure();

            var MongrelEarsRequisite = FeatureConfigurator.New("MongrelEarsRequisiteFeature", "{9428BB4D-1121-4E2D-A694-DFA862C3E2CB}")
                .SetDisplayName(MongrelEarsRequisiteFeatureDisplayName)
                .SetDescription(MongrelMinorMutationDescription)
                .SetHideInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .SetRanks(1)
                .Configure();

            var MongrelTailRequisite = FeatureConfigurator.New("MongrelTailRequisiteFeature", "{A6D7A8CC-0794-42AA-B779-91026791A928}")
                .SetDisplayName(MongrelTailRequisiteFeatureDisplayName)
                .SetDescription(MongrelMinorMutationDescription)
                .SetHideInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .SetRanks(1)
                .Configure();

            // Mismatched Ears

            var MaleMismatchedEarsIEE = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "65d4388247109a841827b6ddd312a426" }, CopyRace.FemaleOptions.Heads[0].Load(true, false).PrimaryColorsProfile.Ramps, "{B75B260B-71F0-4935-8C4B-5C82D250A58A}", true);
            var FemaleMismatchedEarsIEE = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "be14de00304a17c4cb88dc78c377eb2a" }, CopyRace.FemaleOptions.Heads[0].Load(true, false).PrimaryColorsProfile.Ramps, "{ED2B86B4-E392-43A5-8AA7-3C67C6A61F72}", true);

            var FemaleMismatchedEarsI = FeatureConfigurator.New("MongrelFemaleMismatchedEarsI", "{52182CB6-9497-47CF-A98A-29614385A7B3}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Female;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity(FemaleMismatchedEarsIEE)
                .Configure();

            var MaleMismatchedEarsI = FeatureConfigurator.New("MongrelMaleMismatchedEarsI", "{2B38AC48-6D7F-4E0A-8040-F6EC1FFA55C2}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Male;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity(MaleMismatchedEarsIEE)
                .Configure();

            var MismatchedEarsI = ProgressionConfigurator.New("MismatchedEarsimongrel", "{0C117E68-66FB-4A56-963C-AE17B26C8B6D}")
                .SetDisplayName(MismatchedEarsIDisplayName)
                .SetDescription(MongrelMinorMutationDescription)
                .SetGiveFeaturesForPreviousLevels(true)
                .AddToLevelEntries(1, FemaleMismatchedEarsI, MaleMismatchedEarsI)
                .AddPrerequisiteNoFeature(MongrelEarsRequisite)
                .AddFacts([MongrelEarsRequisite])
                .Configure();

            var MaleMismatchedEarsIIEE = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "b5b2e84af4b3f4e429efaba22fb53789" }, CopyRace.FemaleOptions.Heads[0].Load(true, false).PrimaryColorsProfile.Ramps, "{792BF71F-5D35-46AB-9B0F-0A9B860EA118}", true);
            var FemaleMismatchedEarsIIEE = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "88fcdcdc8e9e8d24f9a7d7c91a6054a5" }, CopyRace.FemaleOptions.Heads[0].Load(true, false).PrimaryColorsProfile.Ramps, "{C109C1B8-2DB8-48F2-BF51-750D25415CC9}", true);

            var FemaleMismatchedEarsII = FeatureConfigurator.New("MongrelFemaleMismatchedEarsII", "{5FE8FBA2-3CB8-4F8C-B4CA-50E02ADA0B26}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Female;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity(FemaleMismatchedEarsIIEE)
                .Configure();

            var MaleMismatchedEarsII = FeatureConfigurator.New("MongrelMaleMismatchedEarsII", "{9681D25C-1962-4FE7-9B91-36F3C5D449C8}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Male;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity(MaleMismatchedEarsIIEE)
                .Configure();

            var MismatchedEarsII = ProgressionConfigurator.New("MismatchedEarsIImongrel", "{4FFF8A73-0605-4439-8BF7-D7EFBF0B2396}")
                .SetDisplayName(MismatchedEarsIIDisplayName)
                .SetDescription(MongrelMinorMutationDescription)
                .SetGiveFeaturesForPreviousLevels(true)
                .AddToLevelEntries(1, FemaleMismatchedEarsII, MaleMismatchedEarsII)
                .AddPrerequisiteNoFeature(MongrelEarsRequisite)
                .AddFacts([MongrelEarsRequisite])
                .Configure();

            // Cataracts

            var Cataracts = FeatureConfigurator.New("Cataractsmongrel", "{E9F86F52-1176-4586-8E38-4D5E06D8F4C5}")
                .SetDisplayName(CataractsDisplayName)
                .SetDescription(MongrelMinorMutationDescription)
                .AddEquipmentEntity("a47bac4deb099fc4b86a2e01bb425cc5")
                .AddPrerequisiteNoFeature(MongrelEyesRequisite)
                .AddFacts([MongrelEyesRequisite])
                .Configure();

            // Other Race Eyes

            var AasimarEyesEE = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "c5487e7e903d25a40be683767f3df0b4" }, CopyRace.FemaleOptions.Heads[0].Load(true, false).SecondaryColorsProfile.Ramps, "{DA57CDAC-AA02-4E08-B9C8-EA494A552C15}", true, true,
                    eyeEE: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.AasimarRace.ToString()).MaleOptions.Heads[0]);

            var AasimarEyes = FeatureConfigurator.New("AasimarEyesmongrel", "{79F32409-21F7-4049-A502-C8EEFDF30108}")
                .SetDisplayName(AasimarEyesDisplayName)
                .SetDescription(MongrelMinorMutationDescription)
                .AddEquipmentEntity(AasimarEyesEE)
                .AddPrerequisiteNoFeature(MongrelEyesRequisite)
                .AddFacts([MongrelEyesRequisite])
                .Configure();

            var KitsuneEyesEE = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "c5487e7e903d25a40be683767f3df0b4" }, CopyRace.FemaleOptions.Heads[0].Load(true, false).SecondaryColorsProfile.Ramps, "{7C56E8A4-5A3A-479F-BF55-C9B445398DC6}", true, true,
                    eyeEE: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.KitsuneRace.ToString()).MaleOptions.Heads[0]);

            var KitsuneEyes = FeatureConfigurator.New("KitsuneEyesmongrel", "{5DAE4408-C274-4281-B0DD-B93454193DF0}")
                .SetDisplayName(KitsuneEyesDisplayName)
                .SetDescription(MongrelMinorMutationDescription)
                .AddEquipmentEntity(KitsuneEyesEE)
                .AddPrerequisiteNoFeature(MongrelEyesRequisite)
                .AddFacts([MongrelEyesRequisite])
                .Configure();

            var DhampirEyesEE = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "c5487e7e903d25a40be683767f3df0b4" }, CopyRace.FemaleOptions.Heads[0].Load(true, false).SecondaryColorsProfile.Ramps, "{D3998283-60DF-459B-932F-153FA00700B9}", true, true,
                    eyeEE: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.DhampirRace.ToString()).MaleOptions.Heads[0]);

            var DhampirEyes = FeatureConfigurator.New("DhampirEyesmongrel", "{3A041F4D-726C-4ECB-A035-32E11C62A980}")
                .SetDisplayName(DhampirEyesDisplayName)
                .SetDescription(MongrelMinorMutationDescription)
                .AddEquipmentEntity(DhampirEyesEE)
                .AddPrerequisiteNoFeature(MongrelEyesRequisite)
                .AddFacts([MongrelEyesRequisite])
                .Configure();

            var OreadEyesEE = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "c5487e7e903d25a40be683767f3df0b4" }, CopyRace.FemaleOptions.Heads[0].Load(true, false).SecondaryColorsProfile.Ramps, "{0283DD79-404F-487B-97F2-4CABBBA500D7}", true, true,
                    eyeEE: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.OreadRace.ToString()).MaleOptions.Heads[0]);

            var OreadEyes = FeatureConfigurator.New("OreadEyesmongrel", "{848BFCF6-2D87-443B-877A-AFD7FA24E8EF}")
                .SetDisplayName(OreadEyesDisplayName)
                .SetDescription(MongrelMinorMutationDescription)
                .AddEquipmentEntity(OreadEyesEE)
                .AddPrerequisiteNoFeature(MongrelEyesRequisite)
                .AddFacts([MongrelEyesRequisite])
                .Configure();

            var ElfEyesEE = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "c5487e7e903d25a40be683767f3df0b4" }, CopyRace.FemaleOptions.Heads[0].Load(true, false).SecondaryColorsProfile.Ramps, "{94567F67-E635-4824-961B-B3FA3D0C7F8B}", true, true,
                    eyeEE: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.ElfRace.ToString()).MaleOptions.Heads[0]);

            var ElfEyes = FeatureConfigurator.New("ElfEyesmongrel", "{6BF21DBA-ACBB-40F6-A27E-6D1B42794748}")
                .SetDisplayName(ElfEyesDisplayName)
                .SetDescription(MongrelMinorMutationDescription)
                .AddEquipmentEntity(ElfEyesEE)
                .AddPrerequisiteNoFeature(MongrelEyesRequisite)
                .AddFacts([MongrelEyesRequisite])
                .Configure();

            var TieflingEyesEE = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "c5487e7e903d25a40be683767f3df0b4" }, CopyRace.FemaleOptions.Heads[0].Load(true, false).SecondaryColorsProfile.Ramps, "{886AEFE8-7277-4444-9E00-B3EFD1300012}", true, true,
                    eyeEE: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.TieflingRace.ToString()).MaleOptions.Heads[0]);

            var TieflingEyes = FeatureConfigurator.New("TieflingEyesmongrel", "{D944B39B-9F53-4F83-BC10-EAAB04BDD48C}")
                .SetDisplayName(TieflingEyesDisplayName)
                .SetDescription(MongrelMinorMutationDescription)
                .AddEquipmentEntity(TieflingEyesEE)
                .AddPrerequisiteNoFeature(MongrelEyesRequisite)
                .AddFacts([MongrelEyesRequisite])
                .Configure();

            // Cat Eyes

            var CatEyesEE = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "c5487e7e903d25a40be683767f3df0b4" }, CopyRace.FemaleOptions.Heads[0].Load(true, false).SecondaryColorsProfile.Ramps, "{8DB99E77-AE66-4806-9B33-6E0B6FC083D4}", true, true,
                    eyeEE: new EquipmentEntityLink() { AssetId = "1aeb459da29dca341a78317170eec262" });

            var CatEyes = FeatureConfigurator.New("CatEyesmongrel", "{D44E2C5D-E91B-4E45-B3F1-F17059E537C5}")
                .SetDisplayName(CatEyesDisplayName)
                .SetDescription(MongrelMinorMutationDescription)
                .AddEquipmentEntity(CatEyesEE)
                .AddPrerequisiteNoFeature(MongrelEyesRequisite)
                .AddFacts([MongrelEyesRequisite])
                .Configure();

            // Horns

            var FemaleMongrelHornsI = FeatureConfigurator.New("MongrelFemaleMongrelHornsI", "{F2AB579B-46AA-47FE-9DCD-0E5630BD7F7F}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Female;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity("784bb887aceb5a045996974a71e78875")
                .Configure();

            var MaleMongrelHornsI = FeatureConfigurator.New("MongrelMaleMongrelHornsI", "{B0AB144C-5455-4A35-BAFC-E4FFE5B5C56A}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Male;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity("7ae57d4a307b8cc47acd57cf56c99b5e")
                .Configure();

            var MongrelHornsI = ProgressionConfigurator.New("MongrelHornsimongrel", "{D532E960-628E-4389-8790-CEDBAA9C4242}")
                .SetDisplayName(MongrelHornsIDisplayName)
                .SetDescription(MongrelMinorMutationDescription)
                .SetGiveFeaturesForPreviousLevels(true)
                .AddToLevelEntries(1, FemaleMongrelHornsI, MaleMongrelHornsI)
                .AddPrerequisiteNoFeature(MongrelHornsRequisite)
                .AddFacts([MongrelHornsRequisite])
                .Configure();

            var FemaleMongrelHornsII = FeatureConfigurator.New("MongrelFemaleMongrelHornsII", "{CECB9EC9-7653-4812-BCAF-8573BABF8660}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Female;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity("ce26bc3e359a0b84c9095b2a012bc877")
                .Configure();

            var MaleMongrelHornsII = FeatureConfigurator.New("MongrelMaleMongrelHornsII", "{BF366499-FE89-40AA-96A5-1137CB4ED079}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Male;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity("fed897cada6bd7e4196260cd6543e31b")
                .Configure();

            var MongrelHornsII = ProgressionConfigurator.New("MongrelHornsiimongrel", "{98643425-65B2-4F33-A45B-1347CE830352}")
                .SetDisplayName(MongrelHornsIIDisplayName)
                .SetDescription(MongrelMinorMutationDescription)
                .SetGiveFeaturesForPreviousLevels(true)
                .AddToLevelEntries(1, FemaleMongrelHornsII, MaleMongrelHornsII)
                .AddPrerequisiteNoFeature(MongrelHornsRequisite)
                .AddFacts([MongrelHornsRequisite])
                .Configure();

            var FemaleMongrelHornsIII = FeatureConfigurator.New("MongrelFemaleMongrelHornsIII", "{EB9BA5A0-7099-4C04-8D05-715B0DCEC386}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Female;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity("10d6c6e9b22b3a64bb8fdca64b77ed2a")
                .Configure();

            var MaleMongrelHornsIII = FeatureConfigurator.New("MongrelMaleMongrelHornsIII", "{0F54D535-A80D-4B42-BF23-9F453C24C272}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Male;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity("e900a1a0ecd6d754ea05b63fb38334bb")
                .Configure();

            var MongrelHornsIII = ProgressionConfigurator.New("MongrelHornsiiimongrel", "{377672F4-D12C-4451-A07B-DFAE3EBCF4E9}")
                .SetDisplayName(MongrelHornsIIIDisplayName)
                .SetDescription(MongrelMinorMutationDescription)
                .SetGiveFeaturesForPreviousLevels(true)
                .AddToLevelEntries(1, FemaleMongrelHornsIII, MaleMongrelHornsIII)
                .AddPrerequisiteNoFeature(MongrelHornsRequisite)
                .AddFacts([MongrelHornsRequisite])
                .Configure();

            var FemaleMongrelHornsIV = FeatureConfigurator.New("MongrelFemaleMongrelHornsIV", "{08DE08DC-1BB9-41D7-89AE-2CA755325C4E}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Female;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity("67a781ca294d40441b5ae7a81330ca83")
                .Configure();

            var MaleMongrelHornsIV = FeatureConfigurator.New("MongrelMaleMongrelHornsIV", "{CF830E93-7078-4761-8B5D-7172FD2C9596}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Male;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity("3c1269cc50173ec4bbcaca741205ccd5")
                .Configure();

            var MongrelHornsIV = ProgressionConfigurator.New("MongrelHornsivmongrel", "{ED3463BA-24D3-4746-A39D-4C46438E54AC}")
                .SetDisplayName(MongrelHornsIVDisplayName)
                .SetDescription(MongrelMinorMutationDescription)
                .SetGiveFeaturesForPreviousLevels(true)
                .AddToLevelEntries(1, FemaleMongrelHornsIV, MaleMongrelHornsIV)
                .AddPrerequisiteNoFeature(MongrelHornsRequisite)
                .AddFacts([MongrelHornsRequisite])
                .Configure();

            var FemaleMongrelHornsV = FeatureConfigurator.New("MongrelFemaleMongrelHornsV", "{C0C2FA4B-DC30-475E-91CE-E8842E779111}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Female;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity("fe2d8bf332b38cf459ea989490228099") // These are marked as female tiefling
                .Configure();

            var MaleMongrelHornsV = FeatureConfigurator.New("MongrelMaleMongrelHornsV", "{3C37772D-6DFC-4AC6-BED0-6BF0D437ED07}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Male;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity("7ee2612f73b04724a8f145430ce67c2d")
                .Configure();

            var MongrelHornsV = ProgressionConfigurator.New("MongrelHornsvmongrel", "{C2B9EA74-FFD8-43CE-8C18-D49FA125619A}")
                .SetDisplayName(MongrelHornsVDisplayName)
                .SetDescription(MongrelMinorMutationDescription)
                .SetGiveFeaturesForPreviousLevels(true)
                .AddToLevelEntries(1, FemaleMongrelHornsV, MaleMongrelHornsV)
                .AddPrerequisiteNoFeature(MongrelHornsRequisite)
                .AddFacts([MongrelHornsRequisite])
                .Configure();

            // Tails - can I get these matched to body color?

            var TieflingTailEE = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "d15dbf2912cd6f94492a9e1053aa0ebd" }, CopyRace.FemaleOptions.Heads[0].Load(true, false).PrimaryColorsProfile.Ramps, "{8D292AE6-3C8D-47EF-B858-CF9114D3BEFE}", true, true, /*SetColorProfileRamps: true,*/ RemovePresets: true, SkipPrimaryColors: true, NewPrimaryColorsProfile: CopyRace.FemaleOptions.Heads[0].Load(true, false).PrimaryColorsProfile);

            var TieflingTail = FeatureConfigurator.New("TieflingTailmongrel", "{0EF7AA62-FCB5-4A10-99F7-C92415E7C3CA}")
                .SetDisplayName(TieflingTailDisplayName)
                .SetDescription(MongrelMinorMutationDescription)
                .AddEquipmentEntity(TieflingTailEE)
                .AddPrerequisiteNoFeature(MongrelTailRequisite)
                .AddFacts([MongrelTailRequisite])
                .Configure();

            var MongrelMinorMutation = FeatureSelectionConfigurator.New("EbonsMongrelMinorMutation", "{583FB299-8A87-42C4-8C08-1B7F75A10695}")
                .SetDisplayName(MongrelMinorMutationDisplayName)
                .SetDescription(MongrelMinorMutationDescription)
                .SetIcon(FeatureRefs.DrovierAspectOfTheFalconFeature.Reference.Get().Icon)
                .AddToAllFeatures(MismatchedEarsI, MismatchedEarsII, Cataracts, AasimarEyes, KitsuneEyes, DhampirEyes, OreadEyes, ElfEyes, TieflingEyes, CatEyes, MongrelHornsI, MongrelHornsII, MongrelHornsIII, MongrelHornsIV, MongrelHornsV/*, TieflingTail*/)
                .SetGroup(FeatureGroup.Racial)
                .SetObligatory(true)
                .Configure();

            var MinorMutationDummy = FeatureConfigurator.New("MongrelMinorMutationDummy", "{E1E4812F-6546-430C-92F3-EF1216EE9E92}")
                .SetDisplayName(MongrelMinorMutationDisplayName)
                .SetDescription(MongrelMinorMutationDescription)
                .SetIcon(FeatureRefs.DrovierAspectOfTheFalconFeature.Reference.Get().Icon)
                .Configure();

            // Stagseed

            var StagseedFur = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "1aeb459da29dca341a78317170eec262" }, new List<Texture2D>() { RaceRecolorizer.GetArmorRampByIndex(77) }, "{C4F8EA58-A3A3-40F5-8CC3-DB64BFDD7E82}", true, true,
                new List<Texture2D>() { RaceRecolorizer.GetArmorRampByIndex(75) }, BodyPartsToRemove: new List<BodyPartType>() { BodyPartType.Eyes });

            var FemaleStagseed = FeatureConfigurator.New("MongrelFemaleStagseed", "{EC2DB810-F60C-49BB-BDC5-66F1348EF60B}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Female;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity("61b1f8b3a96c93541a322d6c609ade56")
                .AddEquipmentEntity(StagseedFur)
                .Configure();

            var MaleStagseed = FeatureConfigurator.New("MongrelMaleStagseed", "{2EEC4920-9599-4110-90C3-42D02A008A5E}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Male;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity("fd29ef04a3577d64aaad460f6bb9d6af")
                .AddEquipmentEntity(StagseedFur)
                .Configure();

            var Stagseed = ProgressionConfigurator.New("StagSeedmongrel", "{FDEC12E2-B92B-4D90-9F6F-679D3A21B78C}")
                .SetDisplayName(StagseedDisplayName)
                .SetDescription(StagseedDescription)
                .SetGiveFeaturesForPreviousLevels(true)
                .AddToLevelEntries(1, FemaleStagseed, MaleStagseed, MongrelMinorMutation)
                .AddFacts([MongrelHornsRequisite])
                .Configure();

            // Rockskin

            var RockskinSkinEEL = new EquipmentEntityLink() { AssetId = "33c1acb9aaa60dc448691b6d63fcb22f" };

            var RockskinSkin = RaceRecolorizer.RecolorEELink(RockskinSkinEEL, new List<Texture2D>() { RockskinSkinEEL.Load().PrimaryColorsProfile.Ramps[0] }, "{86D35C9B-2839-463F-A061-C17A5A9C8DA5}", true, true,
                BodyPartsToRemove: new List<BodyPartType>() { BodyPartType.Eyes });

            var Rockskin = ProgressionConfigurator.New("RockSkinmongrel", "{E435D38D-6D18-427D-A3C8-CAAD48E668AD}")
                .SetDisplayName(RockskinDisplayName)
                .SetDescription(RockskinDescription)
                .SetGiveFeaturesForPreviousLevels(true)
                .AddEquipmentEntity(RockskinSkin)
                .AddToLevelEntries(1, MongrelMinorMutation)
                .AddFacts([MongrelMinorMutation])
                .Configure();

            // Coldblood

            var ColdbloodSkinEEL = new EquipmentEntityLink() { AssetId = "7109791d63944254589b908564604c79" };

            var ColdbloodSkin = RaceRecolorizer.RecolorEELink(ColdbloodSkinEEL, new List<Texture2D>() { ColdbloodSkinEEL.Load().PrimaryColorsProfile.Ramps[0] }, "{17653544-AC6B-4D9F-AD35-FF3D9BD4B2A0}", true, true,
                BodyPartsToRemove: new List<BodyPartType>() { BodyPartType.Eyes });

            var Coldblood = ProgressionConfigurator.New("ColdBloodmongrel", "{A2C1F698-61B1-4575-A205-138EEE1339E9}")
                .SetDisplayName(ColdbloodDisplayName)
                .SetDescription(ColdbloodDescription)
                .SetGiveFeaturesForPreviousLevels(true)
                .AddEquipmentEntity(ColdbloodSkin)
                .AddToLevelEntries(1, MongrelMinorMutation)
                .AddFacts([MongrelMinorMutation])
                .Configure();

            // Forestkin

            var ForestkinFur = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "1aeb459da29dca341a78317170eec262" }, new List<Texture2D>() { RaceRecolorizer.GetArmorRampByIndex(77) }, "{555FFEBF-5A9C-462A-ABCC-0E501A8E0DFC}", true, true,
                new List<Texture2D>() { RaceRecolorizer.GetArmorRampByIndex(75) }, BodyPartsToRemove: new List<BodyPartType>() { BodyPartType.Eyes, BodyPartType.Torso, BodyPartType.NeckTorso, BodyPartType.UpperArms, BodyPartType.Forearms, BodyPartType.Hands, BodyPartType.Head, BodyPartType.Ears });

            var FemaleForestkin = FeatureConfigurator.New("MongrelFemaleForestkin", "{95DF75BD-FCB8-4122-9CF3-D5EAEF5B4E10}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Female;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity("0d78bd95d563f3441bf8349bebfb48bf")
                .AddEquipmentEntity(ForestkinFur)
                .Configure();

            var MaleForestkin = FeatureConfigurator.New("MongrelMaleForestkin", "{2D6EB2CA-97A3-4F50-8A52-35812DA53D3C}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Male;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity("d1dde611ca8670845978721380de3bdc")
                .AddEquipmentEntity(ForestkinFur)
                .Configure();

            var Forestkin = ProgressionConfigurator.New("ForestKinmongrel", "{A05513B8-C3A8-4CE8-8A5A-34DB3342B542}")
                .SetDisplayName(ForestkinDisplayName)
                .SetDescription(ForestkinDescription)
                .SetGiveFeaturesForPreviousLevels(true)
                .AddToLevelEntries(1, FemaleForestkin, MaleForestkin, MongrelMinorMutation)
                .AddFacts([MongrelHornsRequisite])
                .Configure();

            // Bullbred

            var BullbredFur = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "1aeb459da29dca341a78317170eec262" }, new List<Texture2D>() { RaceRecolorizer.GetArmorRampByIndex(79) }, "{9B85E383-8058-4BDC-989B-513A1897FE36}", true, true,
                new List<Texture2D>() { RaceRecolorizer.GetArmorRampByIndex(1) }, BodyPartsToRemove: new List<BodyPartType>() { BodyPartType.Eyes });

            var FemaleBullbred = FeatureConfigurator.New("MongrelFemaleBullbred", "{2A1042E8-8DCA-4A2B-B2A3-D1F4E3FDD581}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Female;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity("ce26bc3e359a0b84c9095b2a012bc877")
                .AddEquipmentEntity(BullbredFur)
                .Configure();

            var MaleBullbred = FeatureConfigurator.New("MongrelMaleBullbred", "{D56113B5-F3A4-47B3-8334-90F5D151515F}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Male;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity("fed897cada6bd7e4196260cd6543e31b")
                .AddEquipmentEntity(BullbredFur)
                .Configure();

            var Bullbred = ProgressionConfigurator.New("BullBredmongrel", "{E5F66630-7919-4675-8663-E724AE34E5DB}")
                .SetDisplayName(BullbredDisplayName)
                .SetDescription(BullbredDescription)
                .SetGiveFeaturesForPreviousLevels(true)
                .AddToLevelEntries(1, FemaleBullbred, MaleBullbred, MongrelMinorMutation)
                .AddFacts([MongrelHornsRequisite])
                .Configure();

            // Monkeyborn

            var MonkeybornFur = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "1aeb459da29dca341a78317170eec262" }, new List<Texture2D>() { RaceRecolorizer.GetArmorRampByIndex(75) }, "{C411E54D-5622-4469-9745-C342834D9479}", true, true,
                new List<Texture2D>() { RaceRecolorizer.GetArmorRampByIndex(74) }, BodyPartsToRemove: new List<BodyPartType>() { BodyPartType.Eyes, /*BodyPartType.Ears, BodyPartType.Head, BodyPartType.NeckTorso,*/ BodyPartType.Hands, BodyPartType.Feet });

            var Monkeyborn = ProgressionConfigurator.New("MonkeyBornmongrel", "{956D5043-5170-4887-AB48-86F103D5379B}")
                .SetDisplayName(MonkeybornDisplayName)
                .SetDescription(MonkeybornDescription)
                .SetGiveFeaturesForPreviousLevels(true)
                .AddEquipmentEntity(MonkeybornFur)
                .AddToLevelEntries(1, MongrelMinorMutation)
                .AddFacts([MongrelMinorMutation])
                .Configure();

            // Spiderheart

            var Spiderheart = ProgressionConfigurator.New("SpiderHeartmongrel", "{308DC687-782E-4807-9078-23119742ED1D}")
                .SetDisplayName(SpiderheartDisplayName)
                .SetDescription(SpiderheartDescription)
                .SetGiveFeaturesForPreviousLevels(true)
                .AddEquipmentEntity("770e4c98dc77b1d44ada14e4d615c86b")
                .AddEquipmentEntity("e36b8a3c0ca453f44b2e2e25beb9a328")
                .AddToLevelEntries(1, MongrelMinorMutation)
                .AddFacts([MongrelEyesRequisite, MongrelMinorMutation])
                .Configure();

            // Raptorwing

            var MaleRaptorwingEE = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "632dbfaaf86513645a465598fa536892" }, [], "{71BB3892-22FE-4EFB-AA8D-0E10FEE06DD5}", SkipPrimaryColors: true, ScaleEE: [.65f, .65f, .9f]);
            var FemaleRaptorwingEE = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "419520afa5191ee4bb8d33fc75d2fd29" }, [], "{D3907AB8-20A7-4AA4-ADFF-C6D51D6864B0}", SkipPrimaryColors: true, ScaleEE: [.65f, .65f, .9f]);

            var FemaleRaptorwing = FeatureConfigurator.New("MongrelFemaleRaptorwing", "{B3B302C2-7F57-469D-90B2-00C3B07534DF}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Female;
                    c.CheckInProgression = true;
                })
                //.AddEquipmentEntity("419520afa5191ee4bb8d33fc75d2fd29")
                .AddEquipmentEntity(FemaleRaptorwingEE)
                .Configure();

            var MaleRaptorwing = FeatureConfigurator.New("MongrelMaleRaptorwing", "{6BD0ABA8-182E-4DF8-8391-FC5E8EF9342A}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Male;
                    c.CheckInProgression = true;
                })
                //.AddEquipmentEntity("632dbfaaf86513645a465598fa536892")
                .AddEquipmentEntity(MaleRaptorwingEE)
                .Configure();

            var Raptorwing = ProgressionConfigurator.New("RaptorWingmongrel", "{A99D1724-6357-4145-B2C6-051DF7C44EAB}")
                .SetDisplayName(RaptorwingDisplayName)
                .SetDescription(RaptorwingDescription)
                .SetGiveFeaturesForPreviousLevels(true)
                .AddToLevelEntries(1, FemaleRaptorwing, MaleRaptorwing)
                .AddFacts([MongrelMinorMutation])
                .Configure();

            var MongrelMajorMutation = FeatureSelectionConfigurator.New("EbonsMongrelMajorMutation", "{18BF0424-C26D-48D9-8493-754BD0D9403C}")
                .SetDisplayName(MongrelMajorMutationDisplayName)
                .SetDescription(MongrelMajorMutationDescription)
                .SetIcon(AbilityRefs.Shapechange.Reference.Get().Icon)
                .AddToAllFeatures(Stagseed, Forestkin, Bullbred, Rockskin, Coldblood, Spiderheart, Raptorwing, Monkeyborn)
                .SetGroup(FeatureGroup.Racial)
                .SetObligatory(true)
                .Configure();

            var NaturalArmor = FeatureConfigurator.New("EbonsMongrelNaturalArmor", "{071FC6FD-71C1-4C67-B831-A57CC10A3EDF}")
                .SetDisplayName(NaturalArmorDisplayName)
                .SetDescription(NaturalArmorDescription)
                .SetIcon(FeatureRefs.InvulnerableDefensesShifterFeature.Reference.Get().Icon)
                .AddStatBonus(ModifierDescriptor.NaturalArmor, stat: StatType.AC, value: 2)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            var Skilled = FeatureConfigurator.New("EbonsMongrelSkilled", "{15547075-FAC3-49A0-8904-DB6BDC516A64}")
                .SetDisplayName(SkilledDisplayName)
                .SetDescription(SkilledDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.HumanSkilled.ToString()).Icon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillStealth, value: 4)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillThievery, value: 4)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            var race =
            RaceConfigurator.New(MongrelName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(MongrelDisplayName)
                .SetDescription(MongrelDescription)
                .SetSelectableRaceStat(false)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Strength, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Dexterity, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Charisma, value: -2)
                .SetFeatures(MongrelMajorMutation, MinorMutationDummy, NaturalArmor, Skilled, FeatureRefs.KeenSenses.ToString())
                .SetRaceId(Race.Human)
                .Configure();

            EquipmentEntityLink[] SkinLinks = [MaleMismatchedEarsIIEE, MaleMismatchedEarsIEE, FemaleMismatchedEarsIIEE, FemaleMismatchedEarsIEE/*, TieflingTailEE*/];
            EquipmentEntityLink[] EyeLinks = [AasimarEyesEE, DhampirEyesEE, OreadEyesEE, ElfEyesEE, KitsuneEyesEE, TieflingEyesEE, CatEyesEE];

            // Register linked EEs
            EELinker.RegisterSkinLink(race, SkinLinks);
            EELinker.RegisterEyeLink(race, EyeLinks);

            // Add race to mount fixes
            RaceMountFixerizer.AddRaceToMountFixes(race, CopyRace);

            // Add race to race list
            var raceRef = race.ToReference<BlueprintRaceReference>();
            ref var races = ref BlueprintRoot.Instance.Progression.m_CharacterRaces;

            var length = races.Length;
            Array.Resize(ref races, length + 1);
            races[length] = raceRef;
        }
    }
}
