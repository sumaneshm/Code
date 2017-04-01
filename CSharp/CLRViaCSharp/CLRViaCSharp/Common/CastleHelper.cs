using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CLRViaCSharp._03_SharedAssembliesAndStronglyNamedAssemblies;
using CLRViaCSharp._11_Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLRViaCSharp.Common
{
    static class CastleHelper
    {
        internal static WindsorContainer Container
        {
            get
            {
                return ConstructContainer();
            }
        }

        private static WindsorContainer ConstructContainer()
        {
            WindsorContainer container = new WindsorContainer();

            container.Register(
                Component
                    .For<Study>()
                    .ImplementedBy<EventKeyStudy>());

            return container;
        }
    }
}
