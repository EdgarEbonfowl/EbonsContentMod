using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonsContentMod.Traits
{
    internal class KeenSenses
    {
        private static readonly string KeenSensesDescription = "EbonsContentMod.KeenSenses.Description";
        
        internal static void Configure()
        {
            FeatureConfigurator.For(FeatureRefs.KeenSenses)
                .SetDescription(KeenSensesDescription)
                .Configure();
        }
    }
}
