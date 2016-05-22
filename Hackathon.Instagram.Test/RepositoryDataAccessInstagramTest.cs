using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hackathon.Instagram.Domain.EntityFramework.DataEntities;
using Hackathon.Instagram.Domain.EntityFramework.Repository;

namespace Hackathon.Instagram.Test
{
    [TestClass]
    public class RepositoryDataAccessInstagramTest
    {
        [TestMethod]
        public void Save()
        {
            DataAccessInstagram data = new DataAccessInstagram();

            data.AccessToken = Guid.NewGuid().ToString();
            data.RegistrationDate = DateTime.Now;
            data.Id = Guid.NewGuid();
            data.FullName = "Pomposo Filho";
            data.DueDate = null;
            data.IsValid = true;
            data.ProfilePicture = "http://www.bloblo.com.br";
            data.UserName = "Maxwell";

            RepositoryDataAccessInstagram _repo = new RepositoryDataAccessInstagram();

            var result = _repo.Save(data);

            Assert.IsTrue(result);

        }

        [TestMethod]
        public void GetById()
        {
            RepositoryDataAccessInstagram _repo = new RepositoryDataAccessInstagram();
            var result = _repo.GetByCode("5a078ad0-d051-4ba3-92ad-d5f9f55ea44c");

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetIsValid()
        {
            RepositoryDataAccessInstagram _repo = new RepositoryDataAccessInstagram();
            var result = _repo.GetIsValid();

            Assert.IsNotNull(result);
        }

    }
}
