using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AllMarkt.Data;
using Microsoft.EntityFrameworkCore;

namespace AllMarktTests.Queries
{
    public class AllMarktContextTests : IDisposable
    {
        protected AllMarktContext AllMarktContextIM { get; }
        protected AllMarktQueryContext AllMarktQueryContextIM { get; }

        public AllMarktContextTests()
        {
            AllMarktContextIM = new AllMarktContext(
                new DbContextOptionsBuilder<AllMarktContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options
            );

            AllMarktQueryContextIM = new AllMarktQueryContext(AllMarktContextIM);
        }

        public void Dispose()
        {
            AllMarktContextIM.Dispose();
        }

        public async Task Setup_Add2UsersForPrivateMessagesTest()
        {
            if (!AllMarktContextIM.Users.Any(user => user.Email == "mkI@email.com"))
                AllMarktContextIM.Users.Add(new AllMarkt.Entities.User
                {
                    Email = "mkI@email.com",
                    Password = "parola1",
                    DisplayName = "user1",
                    ReceivedMessages = new List<AllMarkt.Entities.PrivateMessage>(),
                    SentMessages = new List<AllMarkt.Entities.PrivateMessage>()
                });

            if (!AllMarktContextIM.Users.Any(user => user.Email == "mkII@email.com"))
                AllMarktContextIM.Users.Add(new AllMarkt.Entities.User
                {
                    Email = "mkII@email.com",
                    Password = "parola2",
                    DisplayName = "user2",
                    ReceivedMessages = new List<AllMarkt.Entities.PrivateMessage>(),
                    SentMessages = new List<AllMarkt.Entities.PrivateMessage>()
                });

            await AllMarktContextIM.SaveChangesAsync();
        }
    }
}
