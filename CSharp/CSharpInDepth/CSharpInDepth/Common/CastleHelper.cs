using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CSharpInDepth._10_ExtensionMethods;
using CSharpInDepth._11_QueryExpressionsAndLinqToObjects;
using CSharpInDepth._14_DynamicBindingInAStaticLanguage;
using CSharpInDepth._12_LinqBeyondCollections;
using CSharpInDepth._13_MinorChangesToSimplyCode;
using CSharpInDepth._5_FastTrackedDelegates;
using CSharpInDepth._6_ImplementingIteratorsTheEasyWay;
using CSharpInDepth._8_CuttingFluffWithASmartCompiler;
using CSharpInDepth._9_LambdaExpressionsAndExpressionTrees;
using CSharpInDepth.Chapter3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpInDepth._15_AsynchronyWithAsyncAwait;

namespace CSharpInDepth.Common
{
    static class CastleHelper
    {
        internal static WindsorContainer Container
        {
            get{
                return ConstructContainer();
            }
        }

        private static WindsorContainer ConstructContainer()
        {
            WindsorContainer container = new WindsorContainer();

            container.Register(
                Component
                    .For<Study>()
                    .ImplementedBy<InCompletionOrderAsync>());

            return container;
        }
    }
}
