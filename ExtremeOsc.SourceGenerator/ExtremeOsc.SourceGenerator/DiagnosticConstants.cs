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
    }
}
