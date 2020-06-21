﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design",
    "CA1031:Do not catch general exception types",
    Justification = "Needed when executing steps.")]

[assembly: SuppressMessage("Globalization",
    "CA1303:Do not pass literals as localized parameters",
    Justification = "Temporary till the framework is completed.")]

[assembly: SuppressMessage("Major Code Smell",
    "S3264:Events should be invoked",
    Justification = "Event is being invoked through an extension method.")]
