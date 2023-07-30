using Microsoft.AspNetCore.Mvc.Filters;

namespace SampleApplicationCRUD.Filters
{
    // Inhereting from "Attribute" means it can be applied as an attribute to the action methods and controlers
    // Inhereting from "IFilterMetadata" means that this class acts a filter class
    public class SkipFilter : Attribute, IFilterMetadata
    {
    }
}
