namespace ChooseAndBuy.Web.Areas.Administration.Controllers
{
    using ChooseAndBuy.Common;
    using ChooseAndBuy.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
