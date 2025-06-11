using System;

namespace TUNING
{
	public class PLANTS
    {
		public class TARFUNNEL
		{
            // Input/Output rates (kg/s)
            public const float CRUDE_OIL_INPUT_RATE = 2f;           // 2kg/s crude oil irrigation
            public const float PETROLEUM_OUTPUT_RATE = 1.5f;        // 1.5kg/s petroleum (75% efficiency)
            public const float STEAM_OUTPUT_RATE = 0.01f;           // 10g/s steam
            
            // Temperatures
            public const float MIN_TEMP = 353.15f;                  // 80C minimum (your existing value)
            public const float MAX_TEMP = 423.15f;                  // 150C maximum (your existing value)
            public const float PETROLEUM_OUTPUT_TEMP = 373.15f;     // 100C petroleum output
            public const float STEAM_OUTPUT_TEMP = 423.15f;         // 150C steam output
            
            // Growth and production
            public const float GROWTH_TIME_CYCLES = 10f;            // 10 cycles to maturity
            public const float PRODUCTION_DURATION = 150f;          // Similar to SpaceTree (150s per production cycle)
            public const float STORAGE_CAPACITY = 10f;              // 20kg petroleum storage capacity
            
            // Atmosphere requirements
            public static readonly SimHashes[] SAFE_ATMOSPHERES = { SimHashes.Methane }; // Natural Gas atmosphere
        }
    }
}
