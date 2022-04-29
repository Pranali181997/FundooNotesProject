using Microsoft.Extensions.Configuration;
using RepositoryLayer.FundooNoteContex;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Service
{
    public class CollabRL
    {
        public class CollabRL : ICollabRL
        {
            FundooContext dbContext;

            public CollabRL(FundooContext dbContext)
            {
                this.dbContext = dbContext;
            }
            public async Task<Collab> AddCollab(string Email)





        }
    }
}
