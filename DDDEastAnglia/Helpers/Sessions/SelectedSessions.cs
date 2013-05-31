using System.Collections.Generic;

namespace DDDEastAnglia.Helpers.Sessions
{
    public static class SelectedSessions
    {
        public static IEnumerable<int> SpeakerIds
        {
            get
            {
                return new[]
                    {
                                48, // Mark Rendle
                                62, // Liam Westley
                                53, // Ian Russell
                                66, // George Adamson
                                76, // Randolph Burt,
                                85, // Chris O'Dell
                                11, // David Simner
                                78, // Kevin Boyle
                                59, // Rob Ashton
                                29, // Ben Hall
                                82, // Ashic Mahtab
                                75, // Isaac Abraham
                                20, // Dave Sussman
                                47, // Thomas Petricek
                                23 // Joel Hammond-Turner
                    };
            }
        }

        public static IEnumerable<int> SessionIds
        {
            get
            {
                return new[]
                    {
                                44, // Mark Rendle
                                75, // Liam Westley
                                46, // Ian Russell
                                92, // George Adamson
                                64, // Randolph Burt,
                                88, // Chris O'Dell
                                22, // David Simner
                                68, // Kevin Boyle
                                55, // Rob Ashton
                                21, // Ben Hall
                                77, // Ashic Mahtab
                                63, // Isaac Abraham
                                13, // Dave Sussman
                                42, // Thomas Petricek
                                16 // Joel Hammond-Turner
                    };
            }
        }
    }
}