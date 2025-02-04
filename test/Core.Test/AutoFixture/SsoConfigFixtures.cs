﻿using System;
using System.Text.Json;
using AutoFixture;
using AutoFixture.Kernel;
using Bit.Core.Entities;
using Bit.Core.Models.Data;
using Bit.Core.Test.AutoFixture.EntityFrameworkRepositoryFixtures;
using Bit.Core.Test.AutoFixture.OrganizationFixtures;
using Bit.Infrastructure.EntityFramework.Repositories;
using Bit.Test.Common.AutoFixture;
using Bit.Test.Common.AutoFixture.Attributes;

namespace Bit.Core.Test.AutoFixture.SsoConfigFixtures
{
    internal class SsoConfigBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var type = request as Type;
            if (type == null || type != typeof(SsoConfig))
            {
                return new NoSpecimen();
            }

            var fixture = new Fixture();
            var ssoConfig = fixture.WithAutoNSubstitutions().Create<SsoConfig>();
            var ssoConfigData = fixture.WithAutoNSubstitutions().Create<SsoConfigurationData>();
            ssoConfig.SetData(ssoConfigData);
            return ssoConfig;
        }
    }

    internal class EfSsoConfig : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new IgnoreVirtualMembersCustomization());
            fixture.Customizations.Add(new GlobalSettingsBuilder());
            fixture.Customizations.Add(new OrganizationBuilder());
            fixture.Customizations.Add(new SsoConfigBuilder());
            fixture.Customizations.Add(new EfRepositoryListBuilder<SsoConfigRepository>());
            fixture.Customizations.Add(new EfRepositoryListBuilder<OrganizationRepository>());
        }
    }

    internal class EfSsoConfigAutoDataAttribute : CustomAutoDataAttribute
    {
        public EfSsoConfigAutoDataAttribute() : base(new SutProviderCustomization(), new EfSsoConfig())
        { }
    }

    internal class InlineEfSsoConfigAutoDataAttribute : InlineCustomAutoDataAttribute
    {
        public InlineEfSsoConfigAutoDataAttribute(params object[] values) : base(new[] { typeof(SutProviderCustomization),
            typeof(EfSsoConfig) }, values)
        { }
    }
}
