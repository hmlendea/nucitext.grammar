using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using ArchUnitNET.NUnit;
using NUnit.Framework;
using NuciText.Grammar;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace NuciText.Grammar.ArchitectureTests
{
    [TestFixture]
    public sealed class ArchitectureTests
    {
        private static readonly Architecture Architecture = new ArchLoader()
            .LoadAssemblies(typeof(GrammarCorrector).Assembly)
            .Build();

        [Test]
        public void GivenProductionAssembly_WhenInspectingTypes_ThenEveryTypeResidesInProductionNamespace()
        {
            IObjectProvider<IType> productionTypes = Types()
                .That()
                .ResideInAssembly(typeof(GrammarCorrector).Assembly)
                .As("production types");

            IArchRule rule = Classes().That().Are(productionTypes).Should()
                .ResideInNamespaceMatching("NuciText.Grammar.*");

            ArchUnitNET.NUnit.ArchRuleAssert.FulfilsRule(Architecture, rule);
        }

        [Test]
        public void GivenProductionAssembly_WhenInspectingConcreteRules_ThenEveryRuleExtendsGrammarRule()
        {
            IArchRule rule = Classes().That().HaveNameEndingWith("Rule").Should()
                .BeAssignableTo(typeof(GrammarRule));

            ArchUnitNET.NUnit.ArchRuleAssert.FulfilsRule(Architecture, rule);
        }

        [Test]
        public void GivenProductionAssembly_WhenInspectingPublicTypes_ThenNoTypeDependsOnTestAssembly()
        {
            IObjectProvider<IType> productionTypes = Types()
                .That()
                .ResideInAssembly(typeof(GrammarCorrector).Assembly)
                .As("production types");
            IArchRule rule = Types().That().Are(productionTypes).Should()
                .NotDependOnAny(Types().That().HaveFullNameContaining("NuciText.Grammar.ArchitectureTests"));

            ArchUnitNET.NUnit.ArchRuleAssert.FulfilsRule(Architecture, rule);
        }
    }
}
