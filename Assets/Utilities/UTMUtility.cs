using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Unicam.AgentSimulator.Utility
{
    /// <summary>
    /// Utility used in the bus simulation to convert Geog coordinates to Universal Trasverse Marcator values.
    /// </summary>
    public class UTMUtility
    {
        //Constants based on the WGS 84 Datum
        const float equatorialRadius = 6378137f;
        const float polarRadius = 6356752.314247833f;
        const float flattening = 0.0033528106643315515f;
        const float meridianScale = 0.9996f;

        /// <summary>
        /// It converts from <paramref name="latitude"/> and <paramref name="longitude"/>
        /// to the corresponding x and y Universal Trasverse Marcator coordinates.
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void GeogToUTMConvert(float latitude, float longitude, out float x, out float y)
        {
            //Radian conversion
            double latitudeRad = DegreeToRad(latitude);
            //Calculate the utmZone
            int utmZone = 1 + (int)Math.Floor((longitude + 180) / 6);
            int zoneCentralMeridian = 3 + 6 * (utmZone - 1) - 180;
            //Values needed
            float polarAxis = equatorialRadius * (1 - flattening);
            double eccentricity = Math.Sqrt(1 - (polarAxis / equatorialRadius) * (polarAxis/equatorialRadius));
            float eccentricitySquared = (1 - (polarAxis / equatorialRadius) * (polarAxis / equatorialRadius));
            double rifEccentricitySquared = eccentricity * eccentricity / (1 - eccentricity * eccentricity);
            double N = equatorialRadius / Math.Sqrt(1 - Math.Pow(eccentricity * Math.Sin(latitudeRad), 2));
            double T = Math.Pow(Math.Tan(latitudeRad), 2);
            double C = rifEccentricitySquared * Math.Pow(Math.Cos(latitudeRad), 2);
            double A = (longitude - zoneCentralMeridian) * (Math.PI / 180f) * Math.Cos(latitudeRad);
            //M is the arc length along standard meridian
            double M = latitudeRad * (1 - eccentricitySquared * (1 / 4 + eccentricitySquared * (3 / 64 + 5 * eccentricitySquared / 256)));
            M -= Math.Sin(2 * latitudeRad) * (eccentricitySquared * (3 / 8 + eccentricitySquared * (3 / 32 + 45 * eccentricitySquared / 1024)));
            M += Math.Sin(4 * latitudeRad) * (eccentricitySquared * eccentricitySquared * (15 / 256 + eccentricitySquared * 45 / 1024));
            M -= Math.Sin(6 * latitudeRad) * (eccentricitySquared * eccentricitySquared * eccentricitySquared * (35 / 3072));
            M = M * equatorialRadius;
            //Calculate UTM Values
            double xDouble = meridianScale * N * A * (1 + A * A * ((1 - T + C) / 6 + A * A * (5 - 18 * T + T * T + 72 * C - 58 * rifEccentricitySquared) / 120));
            //Easting standard
            xDouble += 500000;
            double yDouble = meridianScale * (M + N * (float)Math.Tan(latitudeRad) * (A * A * (1 / 2 + A * A * ((5 - T + 9 * C + 4 * C * C) / 24 + A * A * (61 - 58 * T + T * T + 600 * C - 330 * rifEccentricitySquared) / 720))));
            if (yDouble < 0) { yDouble = 10000000 + yDouble; }
            x = (float)xDouble;
            y = (float)yDouble;
        }

        /// <summary>
        /// Converts from Degree to Radiant
        /// </summary>
        /// <param name="value"> Degrees </param>
        /// <returns> Radiant </returns>
        private static double DegreeToRad(float value)
        {
            return (Math.PI / 180) * value;
        }

        /// <summary>
        /// Parse from <paramref name="positionValues"/> in longitude and latitude to 
        /// new UTM position values. To avoid value too high for simulation, <paramref name="rifOrigin"/>
        /// is used as a point of reference
        /// </summary>
        /// <param name="positionValues"> Longitude and Latitude position values </param>
        /// <param name="rifOrigin"> The origin point for value resizing </param>
        /// <returns>  </returns>
        public static string[] ParseLongLatToUTM(string[] positionValues, Vector2 rifOrigin)
        {
            float UTMXValue, UTMYValue;
            UTMUtility.GeogToUTMConvert(float.Parse(positionValues[0]), float.Parse(positionValues[1]),
                out UTMXValue, out UTMYValue);
            //We reset the origin of the scene, to avoid values too high for the simulation. The google maps coordinates 
            //for Edinburgh will be the new center of the simulation
            UTMXValue -= rifOrigin.x;
            UTMYValue -= rifOrigin.y;
            string[] newPositionValues = new string[]
            {
                UTMXValue.ToString(),
                "0",
                UTMYValue.ToString(),
            };
            return newPositionValues;
        }
    }
}
