using Hackathon.Instagram.Core;
using Hackathon.Instagram.Core.Models;
using Hackathon.Instagram.Core.Models.Response;
using Hackathon.Instagram.Core.Utils;
using Hackathon.Instagram.Domain.EntityFramework.DataEntities;
using Hackathon.Instagram.Domain.EntityFramework.Repository;
using Hackathon.Instagram.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace Hackathon.Instagram.Web.Controllers.Api
{
    [AllowAnonymous]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Instagram")]
    public class InstagramController : ApiController
    {

        [HttpGet]
        [ResponseType(typeof(List<Media>))]
        [Route("GetVideoByGeoLocation/{numLati:double}/{numLong:double}/{desLocal}")]
        public async Task<IHttpActionResult> GetVideoByGeoLocation(Double numLati, Double numLong, String desLocal)
        {
            try
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

                var midia = new Hackathon.Instagram.Core.EndPoint.Media(auth.Config, oauthResponse);
                var result = await midia.Search(numLati, numLong);
                var videos = this.GetOnlyVideos(result.Data, desLocal);

                return Ok(videos);

            }
            catch (Exception ex)
            {
                return BadRequest("An has ocorred : " + ex.Message);
            }
        }

        [HttpGet]
        [ResponseType(typeof(List<Media>))]
        [Route("GetMidiasByGeoLocation/{numLati:double}/{numLong:double}/{desLocal}")]
        public async Task<IHttpActionResult> GetMidiasByGeoLocation(Double numLati, Double numLong, String desLocal)
        {
            try
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

                var midia = new Hackathon.Instagram.Core.EndPoint.Media(auth.Config, oauthResponse);
                var result = await midia.Search(numLati, numLong);

                if (String.IsNullOrEmpty(desLocal))
                    return Ok(result.Data);
                else
                {
                    var data = result.Data.Where(x => x.Location != null && x.Location.Name.Contains(desLocal)).ToList();
                    return Ok(data);
                }

            }
            catch (Exception ex)
            {
                return BadRequest("An has ocorred : " + ex.Message);
            }
        }

        private List<Media> GetOnlyVideos(List<Media> data, string desMidia)
        {
            if (data != null && data.Any())
            {
                if (String.IsNullOrEmpty(desMidia))
                    return data.Where(x => x.Type == "video").ToList();
                else
                    return data.Where(x => x.Type == "video" && x.Location.Name.Contains(desMidia)).ToList();
            }
            else
                return data;


        }
    }
}
