using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Blueprints.References;
using EbonsContentMod.Utilities;

namespace EbonsContentMod.Bloodlines
{
    internal class ConfigureBloodlines
    {
        internal static void Configure()
        {
            try
            {
                ProteanSorcererBloodline.Configure();
                OrcSorcererBloodline.Configure();
                BloodlineMutations.Configure();
            }
            finally
            {
                ComponentHelperators.CreateBloodlineSpellList();
            }
            

            /*bool bloodlinesadded = false;

            // This is dangerous, but just to be sure...
            while (!bloodlinesadded)
            {
                if (FeatureSelectionRefs.SorcererBloodlineSelection.Reference.Get().Features.Contains(ProteanBloodline[0]) &&
                    FeatureSelectionRefs.SorcererBloodlineSelection.Reference.Get().Features.Contains(OrcBloodline[0]) &&
                    FeatureSelectionRefs.CrossbloodedSecondaryBloodlineSelection.Reference.Get().Features.Contains(ProteanBloodline[1]) &&
                    FeatureSelectionRefs.CrossbloodedSecondaryBloodlineSelection.Reference.Get().Features.Contains(OrcBloodline[1]) &&
                    FeatureSelectionRefs.SeekerBloodlineSelection.Reference.Get().Features.Contains(ProteanBloodline[2]) &&
                    FeatureSelectionRefs.SeekerBloodlineSelection.Reference.Get().Features.Contains(OrcBloodline[2])) 
                    bloodlinesadded = true;
            }*/

            
        }
    }
}
