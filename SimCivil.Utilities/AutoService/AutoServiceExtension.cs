// Copyright (c) 2017 TPDT
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// 
// SimCivil - SimCivil.Utilities - AutoServiceExtension.cs
// Create Date: 2019/06/05
// Update Date: 2019/06/05

using System;
using System.Reflection;
using System.Text;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SimCivil.Utilities.AutoService
{
    public static class AutoServiceExtension
    {
        public static IServiceCollection AutoService(this IServiceCollection services, Assembly assembly)
        {
            foreach (Type type in assembly.GetExportedTypes())
            {
                var autoServiceAttributes = type.GetCustomAttributes<AutoServiceAttribute>();
                foreach (AutoServiceAttribute autoServiceAttribute in autoServiceAttributes)
                {
                    var serviceTypes = autoServiceAttribute.ServiceType == null
                                           ? type.GetInterfaces()
                                           : new[] {autoServiceAttribute.ServiceType};
                    foreach (Type serviceType in serviceTypes)
                        services.Add(new ServiceDescriptor(serviceType, type, autoServiceAttribute.Lifetime));
                }
            }

            return services;
        }

        public static IServiceCollection AutoOptions(
            this IServiceCollection services,
            Assembly                assembly,
            IConfiguration          rootConfiguration)
        {
            services.AddOptions();
            foreach (Type type in assembly.GetExportedTypes())
            {
                var autoOptionsAttributes = type.GetCustomAttributes<AutoOptionsAttribute>();

                Type changeTokenSourceType = typeof(ConfigurationChangeTokenSource<>).MakeGenericType(type);
                Type configureOptionsType =
                    typeof(NamedConfigureFromConfigurationOptions<>).MakeGenericType(type);
                Type changeTokenSourceInterfaceType = typeof(IOptionsChangeTokenSource<>).MakeGenericType(type);
                Type configureOptionsInterfaceType =
                    typeof(IConfigureOptions<>).MakeGenericType(type);

                foreach (AutoOptionsAttribute autoOptionsAttribute in autoOptionsAttributes)
                {
                    IConfiguration configuration =
                        rootConfiguration.GetSection(autoOptionsAttribute.Key ?? type.Name.Replace("Options", ""));

                    services.AddSingleton(
                        changeTokenSourceInterfaceType,
                        servicesProvider => Activator.CreateInstance(
                            changeTokenSourceType,
                            autoOptionsAttribute.Name,
                            configuration));

                    services.AddSingleton(
                        configureOptionsInterfaceType,
                        servicesProvider => Activator.CreateInstance(
                            configureOptionsType,
                            autoOptionsAttribute.Name,
                            configuration,
                            (Action<BinderOptions>) (_ => { })));
                }
            }

            return services;
        }
    }
}