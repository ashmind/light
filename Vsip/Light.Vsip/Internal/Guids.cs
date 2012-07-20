// Guids.cs
// MUST match guids.h

using System;

namespace Light.Vsip.Internal
{
    public static class Guids
    {
        public const string PackageString = "77dd77f5-4680-494f-a7dc-9a27ebe2df01";
        public const string CmdSetString = "0df33538-a815-407a-a3bd-723e927ae45a";
        public const string LanguageServiceString = "7610eb4f-7cde-4cb1-8ce0-55d9b36a39f3";

        public static readonly Guid Package = new Guid(PackageString);
        public static readonly Guid CmdSet = new Guid(CmdSetString);
    };
}