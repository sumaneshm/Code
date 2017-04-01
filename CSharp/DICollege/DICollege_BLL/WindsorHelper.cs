using Castle.MicroKernel.Registration;
using Castle.Windsor;
using DICollege_Common;
using DICollege_SQL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace DICollege_BLL
{
    internal static class WindsorHelper
    {
        static WindsorContainer GetContainer()
        {
            WindsorContainer container = new WindsorContainer();

            var connStr = ConfigurationManager.AppSettings["SqlConnectionString"];

            container.Register(
                Component
                    .For<ICollegeRepository>()
                    .ImplementedBy<SQLCollegeRepository>()
                    .LifeStyle.Singleton
                    .DependsOn(new
                    {
                        connectionString = connStr
                    }));

            

            return container;
        }
    }
}
