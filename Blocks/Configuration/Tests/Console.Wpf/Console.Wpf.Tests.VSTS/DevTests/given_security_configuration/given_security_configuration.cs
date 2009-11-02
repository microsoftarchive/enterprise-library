using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Console.Wpf.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Security;

namespace Console.Wpf.Tests.VSTS.DevTests.given_security_configuration
{
    [TestClass]
    public class given_security_configuration : ContainerContext
    {
        SecuritySettings securitySettings;

        protected override void Arrange()
        {
            base.Arrange();

            ConfigurationSourceBuilder builder = new ConfigurationSourceBuilder();
            builder.ConfigureSecurity()
                 .AuthorizeUsingCustomProviderNamed("custom authz", typeof(IAuthorizationProvider))
                .AuthorizeUsingRuleProviderNamed("ruleProvider")
                        .SpecifyRule("rule1", "true")
                        .SpecifyRule("rule2", "false")
                .CacheSecurityInCacheStoreNamed("cache Storage").WithOptions.UseSharedCacheManager("cache");

            DictionaryConfigurationSource source = new DictionaryConfigurationSource();
            builder.UpdateConfigurationWithReplace(source);


            securitySettings = (SecuritySettings)source.GetSection(SecuritySettings.SectionName);
        }

        SectionViewModel viewModel;

        protected override void Act()
        {
            viewModel = SectionViewModel.CreateSection(Container, SecuritySettings.SectionName, securitySettings);
            viewModel.UpdateLayout();
        }

        [TestMethod]
        public void then_authz_providers_are_in_first_column()
        {
            var allAuthProviders = viewModel.GetDescendentsOfType<AuthorizationProviderData>();

            Assert.IsTrue(allAuthProviders.Any());
            Assert.IsFalse(allAuthProviders.Where(x => x.Column != 0).Any());
        }

        [TestMethod]
        public void then_authz_providers_start_at_row_1()
        {
            var allAuthProviders = viewModel.GetDescendentsOfType<AuthorizationProviderData>();

            Assert.IsTrue(allAuthProviders.Any());
            Assert.AreEqual(1, allAuthProviders.Min(x => x.Row));
        }

        [TestMethod]
        public void then_security_cache_providers_are_in_first_column()
        {
            var cacheProviders = viewModel.GetDescendentsOfType<SecurityCacheProviderData>();

            Assert.IsTrue(cacheProviders.Any());
            Assert.IsFalse(cacheProviders.Where(x => x.Column != 0).Any());
        }

        [TestMethod]
        public void then_rules_are_positioned_in_column_2()
        {
            var ruleProvider = viewModel.GetDescendentsOfType<AuthorizationRuleProviderData>().First();
            Assert.IsNotNull(ruleProvider);

            var rules = ruleProvider.GetDescendentsOfType<AuthorizationRuleData>();
            Assert.IsTrue(rules.Any());
            Assert.IsFalse(rules.Where(x => x.Column != 1).Any());
        }

        [TestMethod]
        public void then_rules_are_positioned_in_row_of_parent()
        {
            var ruleProvider = viewModel.GetDescendentsOfType<AuthorizationRuleProviderData>().First();
            Assert.IsNotNull(ruleProvider);

            var rules = ruleProvider.GetDescendentsOfType<AuthorizationRuleData>();
            Assert.IsTrue(rules.Any());
            Assert.AreEqual(ruleProvider.Row, rules.Min(x => x.Row));
        }

        [TestMethod]
        public void then_rules_provider_has_row_span_of_rules()
        {
            var ruleProvider = viewModel.GetDescendentsOfType<AuthorizationRuleProviderData>().First();
            Assert.IsNotNull(ruleProvider);

            var rules = ruleProvider.GetDescendentsOfType<AuthorizationRuleData>();
            Assert.IsTrue(rules.Any());
            Assert.AreEqual(ruleProvider.RowSpan, rules.Count());
        }

        [TestMethod]
        public void then_security_cache_providers_start_at_row_after_authz_providers()
        {
            var allAuthProviders = viewModel.GetDescendentsOfType<AuthorizationProviderData>();
            var cacheProviders = viewModel.GetDescendentsOfType<SecurityCacheProviderData>();

            Assert.IsTrue(cacheProviders.Any());
            Assert.AreEqual(allAuthProviders.Max(x => x.Row + x.RowSpan) + 1, cacheProviders.Min(x => x.Row));
        }
    }
}
