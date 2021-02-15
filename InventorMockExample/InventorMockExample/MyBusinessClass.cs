using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventor;
using JetBrains.Annotations;

namespace InventorMockExample
{
    public class MyBusinessClass
    {
        private readonly IApplicationService applicationService;
        private const string myAttributeName = "one";
        public MyBusinessClass(IApplicationService applicationService)
        {
            this.applicationService = applicationService;
        }


        [CanBeNull]
        public string GetPersistedProperty(string id)
        {
            var attSets = applicationService.GetCurrentDocumentAttributeSets();
            foreach (AttributeSet attributeSet in attSets)
            {
                if (attributeSet.NameIsUsed[myAttributeName])
                {
                    return attributeSet[id].Value as string;
                }
            }

            return null;
        }

        public void SaveMyProperty(string value, string id)
        {
            var attSets = applicationService.GetCurrentDocumentAttributeSets();
            AttributeSet attset;
            if (!attSets.NameIsUsed[myAttributeName])
            {
                attset = attSets.Add(myAttributeName);
            }
            else
            {
                attset = attSets[myAttributeName];
            }
            if (!attset.NameIsUsed[id])
            {
                attset.Add(id, ValueTypeEnum.kStringType, value);
            }
            else
            {
                attset[id].Value = value;
            }
        }
    }
}
