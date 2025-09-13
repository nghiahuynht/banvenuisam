using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class AppBaseController : Controller
    {
      public AuthenticatedModel AuthenInfo()
      {
            var authen = new AuthenticatedModel();
            if (User.Identity.IsAuthenticated)
            {
                authen.UserName = User.Identity.Name;
                authen.FullName = User.Claims.FirstOrDefault(x => x.Type == "FullName").Value;
                authen.Role = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
                authen.PartnerCode = User.Claims.FirstOrDefault(x => x.Type == "PartnerCode").Value;
                var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value;
                if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int userId))
                {
                    authen.UserId = userId;
                }
            }
           
            return authen;
      }

      

    
    }
}