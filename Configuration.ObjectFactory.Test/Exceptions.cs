﻿using Cactus.Blade.Configuration.ObjectFactory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Configuration.ObjectFactory.Test
{
    internal static class Exceptions
    {
        private const string SpecifyType = "A type must be specified in configuration or by specifying a default type.";

        public static InvalidOperationException ResultCannotBeNull(Type targetType, Type declaringType,
            string memberName)
        {
            return new InvalidOperationException(
                $"A null {targetType} value was returned by the custom convert func for {(declaringType == null ? "the value" : $"the {declaringType}.{memberName} member")}");
        }

        public static InvalidOperationException CannotConvertSectionValueToTargetType(IConfigurationSection section,
            Type targetType, Exception innerException = null)
        {
            return new InvalidOperationException(
                $"Unable to convert value '{section.Value}' in {section.Description()} to target type '{targetType}'.",
                innerException);
        }

        public static InvalidOperationException ConfigurationSpecifiedTypeIsNotAssignableToTargetType(Type targetType,
            Type specifiedType)
        {
            return new InvalidOperationException(
                $"The configuration-specified type, '{specifiedType}' is not assignable to the target type {targetType}.");
        }

        public static InvalidOperationException TypeNotSpecifiedForReloadingProxy => new InvalidOperationException($"Type not specified for reloading proxy. {SpecifyType}");

        public static InvalidOperationException ConfigurationIsNotAList(IConfiguration configuration,
            Type targetCollectionType)
        {
            return new InvalidOperationException(
                $"The {configuration.Description()} does not represent a list. It should be convertible to target collection type '{targetCollectionType}'.");
        }

        public static InvalidOperationException ArrayRankGreaterThanOneIsNotSupported(Type type)
        {
            return new InvalidOperationException(
                $"Arrays with rank greater than zero are not supported. The type specified was '{type}'.");
        }

        public static InvalidOperationException
            CannotCreateAbstractType(IConfiguration configuration, Type targetType)
        {
            return new InvalidOperationException(
                $"Cannot create instance of abstract target type, '{targetType}' from {configuration.Description()}. {SpecifyType}");
        }

        public static InvalidOperationException CannotCreateObjectType =>
            new InvalidOperationException(
                $"Cannot create instance of target type {typeof(object)}. {SpecifyType}");

        public static InvalidOperationException UnsupportedCollectionType(Type targetCollectionType)
        {
            return new InvalidOperationException(
                $"The target collection type {targetCollectionType} is not supported. The following are the supported collection types: "
                + "List<T>, IList<T>, ICollection<T>, IEnumerable<T>, Dictionary<string, T>, and IDictionary<string, T>.");
        }

        public static InvalidOperationException ConfigurationIsAList(IConfiguration configuration,
            Type targetNonCollectionType)
        {
            return new InvalidOperationException(
                $"The {configuration.Description()} represents a list. It should be convertible to target non-collection type '{targetNonCollectionType}'.");
        }

        public static ArgumentException DefaultTypeIsNotAssignableToTargetType(Type targetType, Type defaultType)
        {
            return new ArgumentException(
                $"The specified default type, {defaultType}, is not assignable to the target type {targetType}.");
        }

        public static ArgumentException ReturnTypeOfConvertFuncIsNotAssignableToTargetType(Type targetType,
            Type returnType)
        {
            return new ArgumentException(
                $"The return type, {returnType}, of the specified convert function is not assignable to the target type {targetType}.");
        }

        public static InvalidOperationException
            InconsistentDefaultTypeAttributesForMultipleMembers(string memberName)
        {
            return new InvalidOperationException(
                $"More than one member (property or constructor parameter) matching the name '{memberName}' is decorated with a {nameof(DefaultTypeAttribute)} attribute. All decorated members matching the same name must have the same type.");
        }

        public static InvalidOperationException NoPublicConstructorsFound => new InvalidOperationException("No public constructors found.");

        public static InvalidOperationException AmbiguousConstructors(Type type)
        {
            return new InvalidOperationException(
                $"Ambiguous best-match constructors in '{type}' type. Constructors are ordered as following: 1) from "
                + "highest-to-lowest ratio of matched parameters to total parameters, 2) then from highest-to-lowest ratio of matched parameters "
                + "or parameters with a default value to total parameters, 3) then from the highest-to-lowest number of total parameters.");
        }

        public static ArgumentException DefaultTypeCannotBeAbstract(Type defaultType)
        {
            return new ArgumentException(
                $"Cannot define default type {defaultType}: abstract types cannot be instantiated.",
                nameof(defaultType));
        }

        public static ArgumentException DefaultTypeFromAttributeCannotBeAbstract(Type defaultType)
        {
            return new ArgumentException(
                $"Cannot define default type {defaultType} via {nameof(DefaultTypeAttribute)}: abstract types cannot be instantiated.",
                nameof(defaultType));
        }

        public static ArgumentException NoMatchingMembers(Type declaringType, string memberName)
        {
            return new ArgumentException(
                $"There are no properties or constructor parameters in declaring type {declaringType} that match member name '{memberName}'.");
        }

        public static ArgumentException DefaultTypeNotAssignableToMembers(Type declaringType, string memberName,
            Type defaultType, List<Member> notAssignableMembers)
        {
            return new ArgumentException(
                $"The specified default type {defaultType} is not assignable to the following member(s) in declaring type {declaringType} that match the name '{memberName}':"
                + string.Join("", notAssignableMembers.Select(m => $"{Environment.NewLine}- {m}")));
        }

        public static ArgumentException ReturnTypeOfConvertFuncNotAssignableToMembers(Type declaringType,
            string memberName, Type returnType, List<Member> notAssignableMembers)
        {
            return new ArgumentException(
                $"The return type, {returnType}, of the specified convert function is not assignable to the following member(s) in declaring type {declaringType} that match the name '{memberName}':"
                + string.Join("", notAssignableMembers.Select(m => $"{Environment.NewLine}- {m}")));
        }

        public static InvalidOperationException TargetTypeRequiresConfigurationValue(IConfiguration configuration,
            Type targetType, Type declaringType, string memberName)
        {
            return new InvalidOperationException(
                $"The {targetType.Description(declaringType, memberName)} requires the {configuration.Description()} to have a value and no children but it " +
                $"instead has no value and children: {string.Join(", ", configuration.GetChildren().Select(c => "'" + c.Key + "'"))}.");
        }

        public static ArgumentException ReturnTypeOfMethodFromAttributeIsNotAssignableToTargetType(Type targetType,
            Type returnType, string convertMethodName)
        {
            return new ArgumentException(
                $"The return type, {returnType}, of the method with name '{convertMethodName}' specified in the {nameof(ConvertMethodAttribute)}, is not assignable to target type {targetType}.");
        }

        public static ArgumentException NoMethodFound(Type declaringType, string methodName)
        {
            return new ArgumentException(
                $"No static method named '{methodName}' could be found in the {declaringType} type that has a single string parameter and returns a type other than System.Object.");
        }

        public static InvalidOperationException MissingRequiredConstructorParameters(IConfiguration configuration,
            ConstructorOrderInfo constructorOrderInfo)
        {
            return new InvalidOperationException(
                $"The best matching constructor - `{constructorOrderInfo.Constructor.Format()}` - is missing the {constructorOrderInfo.MissingParameterNames.Format()} from {configuration.Description()}.");
        }

        private static string Description(this Type targetType, Type declaringType, string memberName)
        {
            return declaringType == null || memberName == null
                ? $"target type '{targetType}'"
                : $"'{targetType} {declaringType}.{memberName}' member";
        }

        private static string Description(this IConfiguration configuration)
        {
            return configuration is IConfigurationSection section ? section.Description() : "configuration";
        }

        private static string Description(this IConfigurationSection section)
        {
            return $"section '{section.Key}' at path '{section.Path}'";
        }

        private static string Format(this MemberInfo constructor)
        {
            return constructor.ToString().Replace("Void .ctor", constructor.DeclaringType?.ToString());
        }

        private static string Format(this IEnumerable<string> parameterNames)
        {
            var list = parameterNames.ToList();
            switch (list.Count)
            {
                case 1:
                    return $"'{list[0]}' parameter";
                case 2:
                    return $"'{list[0]}' and '{list[1]}' parameters";
            }

            var builder = new StringBuilder();
            for (var i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1) builder.Append($"'{list[i]}'");
                else if (i == list.Count - 2) builder.Append($"'{list[i]}', and ");
                else builder.Append($"'{list[i]}', ");
            }

            builder.Append(" parameters");

            return builder.ToString();
        }
    }
}
