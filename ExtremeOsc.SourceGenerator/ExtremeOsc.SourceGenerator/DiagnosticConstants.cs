using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExtremeOsc.SourceGenerator
{
    internal static class DiagnosticConstants
    {
        public const string Category = "ExtremeOsc.SourceGenerator";

        public static readonly DiagnosticDescriptor MustBePartial = new(
            id: "EXTREMEOSC001",
            title: "OscPackable Object must be partial",
            messageFormat: "OscPackable {0} must be partial",
            category: Category,
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor MustBeRoot = new(
            id: "EXTREMEOSC002",
            title: "OscPackable Object must be root",
            messageFormat: "OscPackable {0} must be root",
            category: Category,
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor ElementAtIndexIsNotSequencial = new(
            id: "EXTREMEOSC003",
            title: "OscElementAt index is not sequencial",
            messageFormat: "OscElementAt index is not sequencial",
            category: Category,
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor NotSupportedType = new(
            id: "EXTREMEOSC004",
            title: "Not supported type",
            messageFormat: "Not supported type {0}",
            category: Category,
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor AbstractNotSuppoted = new(
            id: "EXTREMEOSC005",
            title: "Abstract type not supported.",
            messageFormat: "Not supported abstract type {0}",
            category: Category,
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor ExceptionError = new(
            id: "EXTREMEOSC006",
            title: "Exception error",
            messageFormat: "Exception error {0}",
            category: Category,
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor FirstArgumentMustBeString = new(
            id: "EXTREMEOSC007",
            title: "First argument must be string.",
            messageFormat: "First argument must be string.",
            category: Category,
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor FirstArgumentMustBeStringNamedAddress = new(
            id: "EXTREMEOSC007",
            title: "First argument must be string named 'address'.",
            messageFormat: "First argument must be string named 'address'.",
            category: Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor DuplicatedAddress = new(
            id: "EXTREMEOSC008",
            title: "Duplicated address",
            messageFormat: "Duplicated address {0}",
            category: Category,
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor NoArgument = new(
            id: "EXTREMEOSC009",
            title: "No argument",
            messageFormat: "{0} has no argument.",
            category: Category,
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor DefaultConstructorNotImplemented = new(
            id: "EXTREMEOSC010",
            title: "Default constructor not implemented",
            messageFormat: "{0} does not have default constructor.",
            category: Category,
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor ArgumentNotPrimitive = new(
            id: "EXTREMEOSC011",
            title: "Argument is not primitive",
            messageFormat: $"Argument not primitive. Please use {string.Join(", ", OscSyntax.TypeNames)}.",
            category: Category,
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);
    }
}
