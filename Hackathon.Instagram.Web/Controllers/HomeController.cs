using Hackathon.Instagram.Core;
using Hackathon.Instagram.Core.Models.Response;
using Hackathon.Instagram.Core.Utils;
using Hackathon.Instagram.Domain.EntityFramework.DataEntities;
using Hackathon.Instagram.Domain.EntityFramework.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Hackathon.Instagram.Web.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            RepositoryDataAccessInstagram repo = new RepositoryDataAccessInstagram();
            var auth = new OAuth();
            OAuthResponse oauthResponse;

            var dataAccess = repo.GetIsValid();

            if (dataAccess != null)
            {
                oauthResponse = dataAccess.Map<DataAccessInstagram, OAuthResponse>();
            }
            else
            {
                LogonInstagram logon = new LogonInstagram();
                var token = await logon.GetToken();
                oauthResponse = await auth.RequestToken(token);

                if (oauthResponse != null)
                {
                    var data = oauthResponse.Map<OAuthResponse, DataAccessInstagram>();

                    if (data != null)
                        repo.Save(data);
                }
            }

            if (oauthResponse == null)
                return Redirect(auth.AuthLink());
            else            
                return View(oauthResponse.User);
        }

        public async Task<ActionResult> OAuth(string code)
        {
            // add this code to the auth object
            var auth = new OAuth();

            // now we have to call back to instagram and include the code they gave us
            // along with our client secret
            var oauthResponse = await auth.RequestToken(code);

            oauthResponse = await auth.RequestToken(code);

            if (oauthResponse != null)
            {
                var data = oauthResponse.Map<OAuthResponse, DataAccessInstagram>();
                RepositoryDataAccessInstagram repo = new RepositoryDataAccessInstagram();

                if (data != null)
                    repo.Save(data);
            }

            // all done, lets redirect to the home controller which will send some intial data to the app
           return View(oauthResponse.User);
        }

    }
}
