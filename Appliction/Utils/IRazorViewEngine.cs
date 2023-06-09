using Microsoft.AspNetCore.Mvc;

namespace MarcketAppliction.Utils
{
    internal interface IRazorViewEngine
    {
        object FindView(ActionContext actionContext, string viewName, bool v);
    }
}