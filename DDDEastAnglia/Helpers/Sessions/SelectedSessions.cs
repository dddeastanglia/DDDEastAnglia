using System.Collections.Generic;

namespace DDDEastAnglia.Helpers.Sessions
{
    public static class SelectedSessions
    {
        public static IEnumerable<int> SessionIds => new[]
        {
            2184,   // Dependency Injection in ASP.NET Core 2. Why and How?
            2130,   // Automating your test infrastructure with Kubernetes
            2171,   // #FAIL - Lessons from infosec incidents
            2183,   // Async in C# - The Good, the Bad and the Ugly
            2188,   // HTTP API patterns
            2174,   // Docker for the complete beginner (Hands on workshop)
            2152,   // Who Will Test The Tests?
            2153,   // MediatR - bridging the gap between your SPA and your backend
            2135,   // Let's talk HTTP
            2164,   // Functional C#
            2187,   // Domain Modelling in F#
            2173,   // Architecting and coding for agility : a practical guide
            2142,   // Less server, less code, less hassle an introduction to Serverless
            2169,   // Sports Betting: How I made 500 Quid During Royal Ascot And What That Can Teach You About Machine Learning
            2160,   // Azure In Action - CosmosDB, Functions and ServiceBus in Perfect Harmony
            2178,   // Why you should consider Web Assembly in your next frontend project
            2181,   // Diving into Functional Programming - Beyond the Basics
            2179,   // SAFE Stack: Functional web programming in .Net
            2136,   // Hunting Typosquatters with F#
            2146    // Cognitive Services Extravaganza!
        };

        public static IEnumerable<int> SpeakerIds => new[]
        {
            8812,   // Don Wibier
            8701,   // Joe Stead
            8693,   // Robin Minto
            8811,   // Stuart Lang
            8814,   // Toby Henderson
            8705,   // Paul McGrath
            8823,   // Paula Muldoon
            8795,   // Oli Wennell
            8784,   // Russell Seamer
            8786,   // Steve Gordon
            8801,   // Simon Painter
            53,     // Ian Russell
            8806,   // Domenico Mustro
            8706,   // Adam Surgenor
            30,     // Gary Short
            6506,   // Joel Hammond-Turner
            8808,   // Håkan Silfvernagel
            5329,   // Ashic Mahtab
            8810,   // Anthony Brown
            8789,   // Chester Burbidge
            8796,   // Gosia Borzecka
        };
    }
}
