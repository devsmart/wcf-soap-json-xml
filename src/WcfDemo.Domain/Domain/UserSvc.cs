using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfDemo.API.Domain.Contracts;
using WcfDemo.API.Domain.Models;

namespace WcfDemo.API.Domain
{
    public class UserSvc : ServiceBase, IUserSvc
    {

        /// <summary>
        /// Method for allow Options calls for CORS
        /// </summary>
        public void GetOptions()
        { }

        /// <summary>
        /// Ping Service
        /// </summary>
        /// <returns></returns>
        public string Ping()
        {
            return "Pong";
        }

        /// <summary>
        /// Gets the specified email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public User Get(string email)
        {
            return new User() { Id = new Random().Next(), Email = email, FirstName = "FirstName", LastName = "LastName" };
        }
    }
}
