using DAL.IService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Component
{
    public class ListMenuNavigationByRole: ViewComponent
    {
        private IUserInfoService userService;

        public ListMenuNavigationByRole(IUserInfoService userService)
        {
            this.userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string roleCode)
        {
            var lstMenu = await userService.LstMenuNavigationByRole(roleCode);
            return await Task.FromResult((IViewComponentResult)View("LeftMenuNavigation", lstMenu));
        }


    }
}
