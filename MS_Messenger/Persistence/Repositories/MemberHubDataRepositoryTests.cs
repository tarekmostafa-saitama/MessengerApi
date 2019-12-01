using Microsoft.VisualStudio.TestTools.UnitTesting;
using MessengerApi.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerApi.Persistence.Repositories.Tests
{
    [TestClass()]
    public class MemberHubDataRepositoryTests
    {
        private MemberHubDataRepository _memberHubDataRepository;

        [TestMethod()]
        public void MemberHubDataRepositoryTest()
        {
             _memberHubDataRepository = new MemberHubDataRepository();
        }

        [TestMethod()]
        public void AddToOnlineMembers_UseRightParameters_Pass()
        {
            var connectionId = "testId";
            var userName = "testUserName";
            _memberHubDataRepository.AddToOnlineMembers(connectionId,userName);
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void CheckKeyExistOnlineMembersTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CheckValueExistOnlineMembersTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RemoveFromOnlineMembersTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AddToOnlineFriendsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetValueFromOnlineFriendsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CheckKeyExistOnlineFriendsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CheckValueExistOnlineFriendsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RemoveFromOnlineFriendsTest()
        {
            Assert.Fail();
        }
    }
}