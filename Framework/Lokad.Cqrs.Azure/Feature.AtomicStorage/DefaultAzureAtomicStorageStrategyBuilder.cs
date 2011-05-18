﻿#region (c) 2010-2011 Lokad - CQRS for Windows Azure - New BSD License 

// Copyright (c) Lokad 2010-2011, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Lokad.Cqrs.Evil;

namespace Lokad.Cqrs.Feature.AtomicStorage
{
    public sealed class DefaultAzureAtomicStorageStrategyBuilder : HideObjectMembersFromIntelliSense
    {
        Predicate<Type> _entityTypeFilter = type => typeof (Define.AtomicEntity).IsAssignableFrom(type);
        Predicate<Type> _singletonTypeFilter = type => typeof (Define.AtomicSingleton).IsAssignableFrom(type);
        readonly List<Assembly> _extraAssemblies = new List<Assembly>();

        string _folderForSingleton = "atomic-singleton";
        Func<Type, string> _nameForSingleton = type => CleanName(type.Name) + ".pb";
        Func<Type, string> _folderForEntity = type => CleanName("atomic-" +type.Name);
        Func<Type, object, string> _nameForEntity =
            (type, key) => (CleanName(type.Name) + "-" + Convert.ToString(key, CultureInfo.InvariantCulture).ToLowerInvariant()) + ".pb";
        
        public void FolderForSingleton(string folderName)
        {
            _folderForSingleton = folderName;
        }

        public static string CleanName(string typeName)
        {
            var sb = new StringBuilder();

            bool lastWasUpper = false;
            bool lastWasSymbol = true;

            foreach (var c in typeName)
            {
                var splitRequired = char.IsUpper(c) || !char.IsLetterOrDigit(c);
                if (splitRequired && !lastWasUpper && !lastWasSymbol)
                {
                    sb.Append('-');
                }
                lastWasUpper = char.IsUpper(c);
                lastWasSymbol = !char.IsLetterOrDigit(c);

                if (char.IsLetterOrDigit(c))
                {
                    sb.Append(char.ToLowerInvariant(c));
                }
            }
            return sb.ToString();
        }

        public void NameForSingleton(Func<Type,string> namer)
        {
            _nameForSingleton = namer;
        }
        public void FolderForEntity(Func<Type,string> naming)
        {
            _folderForEntity = naming;
        }

        public void NameForEntity(Func<Type,object,string> naming)
        {
            _nameForEntity = naming;
        }

        public void WhereEntityIs<TEntityBase>()
        {
            _entityTypeFilter = type => typeof (TEntityBase).IsAssignableFrom(type);
        }

        public void WhereEntity(Predicate<Type> predicate)
        {
            _entityTypeFilter = predicate;
        }

        public void WhereSingleton(Predicate<Type> predicate)
        {
            _singletonTypeFilter = predicate;
        }

        public void WhereSingletonIs<TSingletonBase>()
        {
            _singletonTypeFilter = type => typeof (TSingletonBase).IsAssignableFrom(type);
        }

        public void WithAssembly(Assembly assembly)
        {
            _extraAssemblies.Add(assembly);
        }

        public void WithAssemblyOf<T>()
        {
            _extraAssemblies.Add(typeof(T).Assembly);
        }

        public IAzureAtomicStorageStrategy Build()
        {
            var types = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(AssemblyScanEvil.IsUserAssembly)
                .Concat(_extraAssemblies)
                .Distinct()
                .SelectMany(t => t.GetExportedTypes())
                .Where(t => !t.IsAbstract)
                .ToArray();
            var entities = types
                .Where(t => _entityTypeFilter(t))
                .ToArray();

            var singletons = types
                .Where(t => _singletonTypeFilter(t))
                .ToArray();

            return new DefaultAzureAtomicStorageStrategy(
                entities, 
                singletons, 
                _folderForSingleton, 
                _nameForSingleton, 
                _folderForEntity, 
                _nameForEntity);
        }
    }
}