using Inventor;

namespace InventorMockExample
{
    public class PartApplicationService : IApplicationService
    {
        private readonly Application application;

        public PartApplicationService(Application application)
        {
            this.application = application;
        }
        public AttributeSets GetCurrentDocumentAttributeSets()
        {
            return (application.ActiveDocument as PartDocument).AttributeSets;
        }
    }
}