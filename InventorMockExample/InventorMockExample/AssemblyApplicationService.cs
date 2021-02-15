using Inventor;

namespace InventorMockExample
{
    public class AssemblyApplicationService : IApplicationService
    {
        private readonly Application application;

        public AssemblyApplicationService(Application application)
        {
            this.application = application;
        }
        public AttributeSets GetCurrentDocumentAttributeSets()
        {
            return (application.ActiveDocument as AssemblyDocument).AttributeSets;
        }
    }
}