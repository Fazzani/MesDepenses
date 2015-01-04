// Guids.cs
// MUST match guids.h
using System;

namespace Heni.VSXbmcViewLog
{
    static class GuidList
    {
        public const string guidVSXbmcViewLogPkgString = "157a73e5-2b99-44f4-a1a8-107e820ef061";
        public const string guidVSXbmcViewLogCmdSetString = "03e90fed-1a1f-4982-b391-00ddf617f956";

        public static readonly Guid guidVSXbmcViewLogCmdSet = new Guid(guidVSXbmcViewLogCmdSetString);
    };
}