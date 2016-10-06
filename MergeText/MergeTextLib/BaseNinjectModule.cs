using System.Collections.Generic;
using Ninject;
using Ninject.Modules;

namespace MergeTextLib
{
    public class BaseNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IComparer<string>>().To<StrComparerIgnoreSpace>();

            Bind<IMergeTwoChangedAndOriginalLines>().To<MergeTwoChangedAndOriginalLines>()
                .WithConstructorArgument("comparer", Kernel.Get<IComparer<string>>());

            Bind<IMergedLinesCreator>().To<MergedLinesCreator>()
                .WithConstructorArgument("merge", Kernel.Get<IMergeTwoChangedAndOriginalLines>());

            Bind<IMergedLineToStrListConverter>().To<MergedLineToStrListConverter>();

            Bind<ISurvivedTextTreeCreator>().To<SurvivedTextTreeCreator>()
                .WithConstructorArgument("comparer", Kernel.Get<IComparer<string>>());

            Bind<IChangedLinesCreator>().To<ChangedLinesCreator>()
                .WithConstructorArgument("survivedTextTreeCreator", Kernel.Get<ISurvivedTextTreeCreator>());

            Bind<IMergeTextFacade>().To<MergeTextFacade>()
                .WithConstructorArgument("mergedLinesCreator", Kernel.Get<IMergedLinesCreator>())
                .WithConstructorArgument("mergedToStrConverter", Kernel.Get<IMergedLineToStrListConverter>())
                .WithConstructorArgument("changedLinesCreator", Kernel.Get<IChangedLinesCreator>());

        }
    }
}
