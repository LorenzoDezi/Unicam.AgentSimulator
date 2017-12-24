using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unicam.AgentSimulator.Utility
{
    public class UTMUtility
    {
        //Constants based on the WGS 84 Datum
        const float equatorialRadius = 6378137f;
        const float polarRadius = 6356752.314247833f;
        const float flattening = 0.0033528106643315515f;
        const float meridianScale = 0.9996f;

        public static void GeogToUTMConvert(float latitude, float longitude, out float x, out float y)
        {
            //Radian conversion
            float latitudeRad = DegreeToRad(latitude);
            float longitudeRad = DegreeToRad(longitude);
            //Calculate the utmZone
            int utmZone = 1 + (int)Math.Floor((longitude + 180) / 6);
            int zoneCentralMeridian = 3 + 6 * (utmZone - 1) - 180;
            //Values needed
            float polarAxis = equatorialRadius * (1 - flattening);
            float eccentricity = (float)Math.Sqrt(1 - polarAxis * polarAxis / equatorialRadius * equatorialRadius);
            float eccentricitySquared = (1 - (polarRadius / equatorialRadius) * (polarRadius / equatorialRadius));
            float rifEccentricity = eccentricity / (float)Math.Sqrt(1 - eccentricity * eccentricity);
            float rifEccentricitySquared = eccentricity * eccentricity / (1 - eccentricity * eccentricity);
            float N = equatorialRadius / (float)Math.Sqrt(1 - Math.Pow(eccentricity * Math.Sin(latitudeRad), 2));
            float T = (float)Math.Pow(Math.Tan(latitudeRad), 2);
            float C = rifEccentricitySquared * (float)Math.Pow(Math.Cos(latitudeRad), 2);
            float A = (longitude - zoneCentralMeridian) * (float)Math.PI / 180f * (float)Math.Cos(latitudeRad);
            //M is the arc length along standard meridian
            float M = latitudeRad * (1 - eccentricitySquared * (1 / 4 + eccentricitySquared * (3 / 64 + 5 * eccentricitySquared / 256)));
            M -= (float)Math.Sin(2 * latitudeRad) * (eccentricitySquared * (3 / 8 + eccentricitySquared * (3 / 32 + 45 * eccentricitySquared / 1024)));
            M += (float)Math.Sin(4 * latitudeRad) * (eccentricitySquared * eccentricitySquared * (15 / 256 + eccentricitySquared * 45 / 1024));
            M -= (float)Math.Sin(6 * latitudeRad) * (eccentricitySquared * eccentricitySquared * eccentricitySquared * 35 / 3072);
            M = M * equatorialRadius;
            //Calculate UTM Values
            x = meridianScale * N * A * (1 + A * A * ((1 - T + C) / 6 + A * A * (5 - 18 * T + T * T + 72 * C - 58 * rifEccentricitySquared) / 120));
            //Easting standard
            x += 500000;
            y = meridianScale * (M + N * (float)Math.Tan(latitudeRad) * (A * A * (1 / 2 + A * A * ((5 - T + 9 * C + 4 * C * C) / 24 + A * A * (61 - 58 * T + T * T + 600 * C - 330 * rifEccentricitySquared) / 720))))
        }

        private static float DegreeToRad(float value)
        {
            return (float)(Math.PI / 180) * value;
        }
    }
}
