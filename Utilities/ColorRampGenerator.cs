using UnityEngine;

namespace EbonsContentMod.Utilities
{
    /// <summary>
    /// Generates Owlcat-style 256x1 character color ramps.
    ///
    /// The class is partial so additional race-specific ramp generators
    /// can be added in separate files while continuing to use the same
    /// shared ramp-generation machinery.
    /// </summary>
    public static partial class ColorRampGenerator
    {
        // ============================================================
        // Shared
        // ============================================================

        private const int RampWidth = 256;

        // Human skin ramps use pixel 140 as the semantic "requested color"
        // anchor. Several other fitted race families were derived around 160,
        // so that value remains the default for those profiles.
        private const int HumanMidIndex = 140;
        private const int DefaultMidIndex = 160;


        /// <summary>
        /// Defines the interpolation behavior of a ramp family.
        /// </summary>
        private sealed class RampCurveProfile
        {
            public readonly int MidIndex;

            public readonly float[] RShadowToMid;
            public readonly float[] GShadowToMid;
            public readonly float[] BShadowToMid;

            public readonly float[] RMidToHighlight;
            public readonly float[] GMidToHighlight;
            public readonly float[] BMidToHighlight;

            public RampCurveProfile(
                int midIndex,
                float[] rShadowToMid,
                float[] gShadowToMid,
                float[] bShadowToMid,
                float[] rMidToHighlight,
                float[] gMidToHighlight,
                float[] bMidToHighlight)
            {
                MidIndex = midIndex;

                RShadowToMid = rShadowToMid;
                GShadowToMid = gShadowToMid;
                BShadowToMid = bShadowToMid;

                RMidToHighlight = rMidToHighlight;
                GMidToHighlight = gMidToHighlight;
                BMidToHighlight = bMidToHighlight;
            }
        }

        /// <summary>
        /// An official ramp used as a reference for automatically
        /// selecting shadow and highlight endpoints.
        /// </summary>
        private readonly struct RampReference
        {
            public readonly Color Midtone;
            public readonly Color Shadow;
            public readonly Color Highlight;

            public RampReference(
                Color midtone,
                Color shadow,
                Color highlight)
            {
                Midtone = midtone;
                Shadow = shadow;
                Highlight = highlight;
            }
        }

        /// <summary>
        /// Associates a requested-color intensity with an automatically selected
        /// endpoint color. Used for ramp families whose endpoints vary primarily
        /// with intensity rather than with categorical hue.
        /// </summary>
        private readonly struct EndpointStop
        {
            public readonly float MidtoneValue;
            public readonly Color Color;

            public EndpointStop(
                float midtoneValue,
                byte r,
                byte g,
                byte b)
            {
                MidtoneValue = midtoneValue;
                Color = new Color32(r, g, b, 255);
            }
        }

        /// <summary>
        /// Creates a 256x1 ramp using a supplied curve profile.
        /// </summary>
        private static Texture2D CreateRamp(
            Color shadow,
            Color midtone,
            Color highlight,
            RampCurveProfile profile)
        {
            Texture2D texture =
                new Texture2D(
                    RampWidth,
                    1,
                    TextureFormat.RGBA32,
                    false);

            texture.wrapMode =
                TextureWrapMode.Clamp;

            texture.filterMode =
                FilterMode.Bilinear;

            texture.anisoLevel = 0;

            Color[] pixels =
                new Color[RampWidth];

            int midIndex =
                profile.MidIndex;

            for (int x = 0; x < RampWidth; x++)
            {
                float r;
                float g;
                float b;

                if (x <= midIndex)
                {
                    float t =
                        x / (float)midIndex;

                    float tr =
                        EvaluateCurve(
                            profile.RShadowToMid,
                            t);

                    float tg =
                        EvaluateCurve(
                            profile.GShadowToMid,
                            t);

                    float tb =
                        EvaluateCurve(
                            profile.BShadowToMid,
                            t);

                    r =
                        Mathf.LerpUnclamped(
                            shadow.r,
                            midtone.r,
                            tr);

                    g =
                        Mathf.LerpUnclamped(
                            shadow.g,
                            midtone.g,
                            tg);

                    b =
                        Mathf.LerpUnclamped(
                            shadow.b,
                            midtone.b,
                            tb);
                }
                else
                {
                    float t =
                        (x - midIndex) /
                        (float)(
                            RampWidth -
                            1 -
                            midIndex);

                    float tr =
                        EvaluateCurve(
                            profile.RMidToHighlight,
                            t);

                    float tg =
                        EvaluateCurve(
                            profile.GMidToHighlight,
                            t);

                    float tb =
                        EvaluateCurve(
                            profile.BMidToHighlight,
                            t);

                    r =
                        Mathf.LerpUnclamped(
                            midtone.r,
                            highlight.r,
                            tr);

                    g =
                        Mathf.LerpUnclamped(
                            midtone.g,
                            highlight.g,
                            tg);

                    b =
                        Mathf.LerpUnclamped(
                            midtone.b,
                            highlight.b,
                            tb);
                }

                // Some of the empirical curves intentionally overshoot
                // slightly. Preserve that behavior during interpolation,
                // then clamp the final texture value.
                pixels[x] =
                    new Color(
                        Mathf.Clamp01(r),
                        Mathf.Clamp01(g),
                        Mathf.Clamp01(b),
                        1f);
            }

            texture.SetPixels(pixels);
            texture.Apply();

            return texture;
        }

        /// <summary>
        /// Evaluates an empirical curve whose control points are
        /// evenly spaced between 0 and 1.
        /// </summary>
        private static float EvaluateCurve(
            float[] curve,
            float t)
        {
            t =
                Mathf.Clamp01(t);

            float position =
                t * (curve.Length - 1);

            int index =
                Mathf.FloorToInt(position);

            if (index >= curve.Length - 1)
            {
                return curve[
                    curve.Length - 1];
            }

            float localT =
                position - index;

            return Mathf.LerpUnclamped(
                curve[index],
                curve[index + 1],
                localT);
        }

        /// <summary>
        /// Finds the official reference ramp whose midtone is closest
        /// to the requested midtone in RGB space.
        ///
        /// This is intentionally nearest-neighbor rather than blending
        /// endpoints, because several Owlcat ramps use categorical
        /// highlight colors that should not be averaged together.
        /// </summary>
        private static RampReference GetNearestReference(
            Color midtone,
            RampReference[] references)
        {
            int bestIndex = 0;

            float bestDistance =
                float.MaxValue;

            for (int i = 0; i < references.Length; i++)
            {
                float distance =
                    ColorDistanceSquared(
                        midtone,
                        references[i].Midtone);

                if (distance < bestDistance)
                {
                    bestDistance =
                        distance;

                    bestIndex = i;
                }
            }

            return references[bestIndex];
        }

        /// <summary>
        /// Estimates an endpoint from the intensity of the requested midtone.
        ///
        /// The maximum RGB channel is intentionally used rather than luminance:
        /// a vivid fantasy color such as (0, 0.5, 1) should be treated as a
        /// bright requested color instead of being forced toward a dark
        /// flesh-tone reference merely because its luminance is moderate.
        /// </summary>
        private static Color EvaluateEndpointStops(
            Color midtone,
            EndpointStop[] stops)
        {
            float value =
                Mathf.Max(
                    midtone.r,
                    Mathf.Max(
                        midtone.g,
                        midtone.b))
                * 255f;

            if (value <= stops[0].MidtoneValue)
            {
                return stops[0].Color;
            }

            for (int i = 1; i < stops.Length; i++)
            {
                if (value <= stops[i].MidtoneValue)
                {
                    EndpointStop previous =
                        stops[i - 1];

                    EndpointStop next =
                        stops[i];

                    float t =
                        Mathf.InverseLerp(
                            previous.MidtoneValue,
                            next.MidtoneValue,
                            value);

                    return Color.Lerp(
                        previous.Color,
                        next.Color,
                        t);
                }
            }

            return stops[stops.Length - 1].Color;
        }

        private static float ColorDistanceSquared(
            Color a,
            Color b)
        {
            float dr = a.r - b.r;
            float dg = a.g - b.g;
            float db = a.b - b.b;

            return
                dr * dr +
                dg * dg +
                db * db;
        }

        // ============================================================
        // Human
        // ============================================================

        private static readonly RampCurveProfile
            HumanProfile =
            new RampCurveProfile(
                HumanMidIndex,

                // R: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.011197f,
                    0.041551f,
                    0.090751f,
                    0.177093f,
                    0.302998f,
                    0.513894f,
                    0.775680f,
                    1.000000f
                },

                // G: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.004734f,
                    0.021004f,
                    0.039247f,
                    0.076150f,
                    0.163741f,
                    0.336318f,
                    0.589556f,
                    1.000000f
                },

                // B: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.000000f,
                    0.033402f,
                    0.068117f,
                    0.124995f,
                    0.238451f,
                    0.339147f,
                    0.540606f,
                    1.000000f
                },

                // R: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.079383f,
                    0.200384f,
                    0.378568f,
                    0.523008f,
                    0.710925f,
                    0.835637f,
                    0.906779f,
                    1.000000f
                },

                // G: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.121099f,
                    0.262338f,
                    0.421594f,
                    0.579974f,
                    0.726954f,
                    0.852625f,
                    0.944253f,
                    1.000000f
                },

                // B: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.081340f,
                    0.197283f,
                    0.349793f,
                    0.515790f,
                    0.684116f,
                    0.827217f,
                    0.932867f,
                    1.000000f
                });


        /*
         * Human endpoint progression sampled from the official ramps at the
         * pixel-140 anchor. Once the requested color is brighter than the
         * lightest official range, the endpoint simply remains at the light
         * end of the observed human palette instead of selecting an unrelated
         * official complexion.
         */

        private static readonly EndpointStop[]
            HumanShadowStops =
            {
                new EndpointStop(
                    10f,
                    0, 0, 0),

                new EndpointStop(
                    32f,
                    0, 0, 0),

                new EndpointStop(
                    56f,
                    2, 1, 1),

                new EndpointStop(
                    60f,
                    3, 1, 1),

                new EndpointStop(
                    90f,
                    8, 4, 3),

                new EndpointStop(
                    150f,
                    8, 4, 3)
            };


        private static readonly EndpointStop[]
            HumanHighlightStops =
            {
                new EndpointStop(
                    10f,
                    21, 24, 29),

                new EndpointStop(
                    32f,
                    47, 86, 129),

                new EndpointStop(
                    56f,
                    63, 140, 184),

                new EndpointStop(
                    60f,
                    65, 147, 190),

                new EndpointStop(
                    90f,
                    82, 214, 251),

                new EndpointStop(
                    118f,
                    84, 214, 253),

                new EndpointStop(
                    150f,
                    84, 214, 253)
            };


        /// <summary>
        /// Creates an Owlcat-style human skin color ramp from the supplied midtone.
        ///
        /// Human ramps have a relatively smooth, naturalistic progression. In the
        /// shadow region, red generally develops faster than green and blue, producing
        /// warm brown/red undertones rather than a simple darkened version of the
        /// requested color. After the main color anchor, green and especially blue
        /// become increasingly influential, causing the far end of the ramp to drift
        /// toward the cool blue/cyan behavior characteristic of the official human
        /// skin ramps.
        ///
        /// The supplied midtone is placed at pixel 140, which better preserves the
        /// requested color in the part of the ramp that strongly affects the visible
        /// base skin color in game. If shadow or highlight are omitted, their colors
        /// are estimated from the intensity of the requested midtone rather than by
        /// choosing the nearest official flesh tone. This makes the method suitable
        /// for custom fantasy colors such as saturated blue, green, or red without
        /// unexpectedly darkening them to match an unrelated natural complexion.
        ///
        /// Either endpoint may be overridden while retaining the human interpolation
        /// curve and pixel-140 requested-color anchor.
        /// </summary>
        /// <param name="midtone">
        /// The primary skin color. This exact color is placed at pixel 140 of the
        /// generated ramp.
        /// </param>
        /// <param name="shadow">
        /// Optional custom dark endpoint. If null, an Owlcat-style human shadow
        /// endpoint is estimated from the intensity of the supplied midtone.
        /// </param>
        /// <param name="highlight">
        /// Optional custom secondary/end color. If null, an Owlcat-style human
        /// secondary color is estimated from the intensity of the supplied midtone.
        /// </param>
        /// <returns>
        /// A new 256x1 Texture2D containing the generated human skin color ramp.
        /// </returns>
        public static Texture2D CreateHumanSkinRamp(
            Color midtone,
            Color? shadow = null,
            Color? highlight = null)
        {
            Color shadowColor =
                shadow ??
                EvaluateEndpointStops(
                    midtone,
                    HumanShadowStops);

            Color highlightColor =
                highlight ??
                EvaluateEndpointStops(
                    midtone,
                    HumanHighlightStops);

            return CreateRamp(
                shadowColor,
                midtone,
                highlightColor,
                HumanProfile);
        }


        // ============================================================
        // Dhampir
        // ============================================================

        private static readonly RampCurveProfile
            DhampirProfile =
            new RampCurveProfile(
                //DefaultMidIndex,
                140,

                new float[]
                {
                    0.000000f,
                    0.010917f,
                    0.042129f,
                    0.094242f,
                    0.185993f,
                    0.358698f,
                    0.656753f,
                    0.930344f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.003526f,
                    0.017962f,
                    0.040879f,
                    0.085743f,
                    0.212907f,
                    0.460765f,
                    0.787163f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.003499f,
                    0.018517f,
                    0.046211f,
                    0.103495f,
                    0.235426f,
                    0.459091f,
                    0.757970f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.122933f,
                    0.281357f,
                    0.458025f,
                    0.622221f,
                    0.768101f,
                    0.874893f,
                    0.937643f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.169847f,
                    0.324603f,
                    0.484036f,
                    0.629440f,
                    0.766882f,
                    0.872963f,
                    0.943455f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.161786f,
                    0.317784f,
                    0.484306f,
                    0.637376f,
                    0.775204f,
                    0.871946f,
                    0.939467f,
                    1.000000f
                });


        private static readonly RampReference[]
            DhampirReferences =
            {
                new RampReference(
                    new Color32(18, 20, 30, 255),
                    new Color32(0, 0, 0, 255),
                    new Color32(47, 47, 129, 255)),

                new RampReference(
                    new Color32(26, 35, 62, 255),
                    new Color32(0, 0, 0, 255),
                    new Color32(57, 97, 157, 255)),

                new RampReference(
                    new Color32(38, 44, 49, 255),
                    new Color32(0, 0, 0, 255),
                    new Color32(47, 86, 131, 255)),

                new RampReference(
                    new Color32(62, 84, 90, 255),
                    new Color32(8, 4, 3, 255),
                    new Color32(82, 214, 251, 255)),

                new RampReference(
                    new Color32(132, 132, 134, 255),
                    new Color32(5, 5, 6, 255),
                    new Color32(137, 178, 149, 255)),

                new RampReference(
                    new Color32(136, 131, 136, 255),
                    new Color32(6, 4, 5, 255),
                    new Color32(112, 184, 214, 255)),

                new RampReference(
                    new Color32(104, 101, 99, 255),
                    new Color32(8, 4, 3, 255),
                    new Color32(82, 173, 251, 255)),

                new RampReference(
                    new Color32(82, 91, 137, 255),
                    new Color32(5, 2, 3, 255),
                    new Color32(144, 190, 249, 255)),

                new RampReference(
                    new Color32(57, 66, 100, 255),
                    new Color32(1, 0, 0, 255),
                    new Color32(121, 173, 245, 255)),

                new RampReference(
                    new Color32(84, 115, 132, 255),
                    new Color32(6, 5, 5, 255),
                    new Color32(72, 184, 253, 255)),

                new RampReference(
                    new Color32(94, 103, 100, 255),
                    new Color32(8, 4, 3, 255),
                    new Color32(82, 214, 253, 255))
            };

        /// <summary>
        /// Creates an Owlcat-style dhampir skin color ramp from the supplied midtone.
        /// 
        /// Dhampir ramps use a colder and more desaturated progression than human
        /// ramps. Red tends to be restrained through the darker portion of the ramp,
        /// while green and blue gain influence earlier and more strongly as the ramp
        /// approaches its secondary color. This tends to produce gray, blue-gray,
        /// purple, or corpse-like transitions and gives even relatively warm midtones
        /// a noticeably cooler overall shading response.
        /// 
        /// If shadow or highlight are not supplied, their colors are selected from the
        /// nearest official dhampir ramp based on the requested midtone. Because
        /// official dhampir palettes vary substantially in hue, this uses the complete
        /// RGB midtone rather than brightness alone when choosing the reference ramp.
        /// Either endpoint may be overridden while retaining the dhampir interpolation
        /// curve.
        /// </summary>
        /// <param name="midtone">
        /// The primary skin color around which the ramp is constructed.
        /// </param>
        /// <param name="shadow">
        /// Optional custom dark endpoint. If null, an appropriate shadow color is
        /// selected automatically from the official dhampir ramps.
        /// </param>
        /// <param name="highlight">
        /// Optional custom secondary/end color. If null, an appropriate color is
        /// selected automatically from the official dhampir ramps.
        /// </param>
        /// <returns>
        /// A new 256x1 Texture2D containing the generated dhampir skin color ramp.
        /// </returns>
        public static Texture2D CreateDhampirSkinRamp(
            Color midtone,
            Color? shadow = null,
            Color? highlight = null)
        {
            RampReference reference =
                GetNearestReference(
                    midtone,
                    DhampirReferences);

            return CreateRamp(
                shadow ?? reference.Shadow,
                midtone,
                highlight ?? reference.Highlight,
                DhampirProfile);
        }


        // ============================================================
        // Tiefling
        // ============================================================

        private static readonly RampCurveProfile
            TieflingProfile =
            new RampCurveProfile(
                DefaultMidIndex,

                new float[]
                {
                    0.000000f,
                    0.006294f,
                    0.016575f,
                    0.034979f,
                    0.073974f,
                    0.187123f,
                    0.423553f,
                    0.749873f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    -0.000735f,
                    0.003560f,
                    0.008589f,
                    0.028563f,
                    0.096226f,
                    0.245225f,
                    0.584614f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.000000f,
                    0.008646f,
                    0.012597f,
                    0.032799f,
                    0.097952f,
                    0.253579f,
                    0.587097f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.113574f,
                    0.245451f,
                    0.399604f,
                    0.559675f,
                    0.717868f,
                    0.841589f,
                    0.934336f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.120612f,
                    0.259875f,
                    0.414449f,
                    0.572408f,
                    0.727544f,
                    0.845279f,
                    0.935788f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.120009f,
                    0.253852f,
                    0.407684f,
                    0.568247f,
                    0.722562f,
                    0.844306f,
                    0.937832f,
                    1.000000f
                });


        private static readonly RampReference[]
            TieflingReferences =
            {
                new RampReference(
                    new Color32(20, 22, 19, 255),
                    new Color32(1, 1, 0, 255),
                    new Color32(162, 155, 95, 255)),

                new RampReference(
                    new Color32(39, 66, 87, 255),
                    new Color32(5, 5, 2, 255),
                    new Color32(82, 214, 251, 255)),

                new RampReference(
                    new Color32(24, 27, 32, 255),
                    new Color32(1, 1, 0, 255),
                    new Color32(251, 137, 82, 255)),

                new RampReference(
                    new Color32(29, 26, 25, 255),
                    new Color32(5, 5, 2, 255),
                    new Color32(82, 214, 251, 255)),

                new RampReference(
                    new Color32(35, 41, 29, 255),
                    new Color32(2, 2, 1, 255),
                    new Color32(82, 251, 204, 255)),

                new RampReference(
                    new Color32(56, 51, 41, 255),
                    new Color32(5, 5, 2, 255),
                    new Color32(82, 214, 251, 255)),

                new RampReference(
                    new Color32(67, 97, 120, 255),
                    new Color32(5, 5, 2, 255),
                    new Color32(121, 226, 253, 255)),

                new RampReference(
                    new Color32(94, 81, 58, 255),
                    new Color32(5, 5, 2, 255),
                    new Color32(249, 251, 82, 255)),

                new RampReference(
                    new Color32(39, 42, 68, 255),
                    new Color32(1, 0, 0, 255),
                    new Color32(121, 171, 243, 255)),

                new RampReference(
                    new Color32(57, 66, 100, 255),
                    new Color32(1, 0, 0, 255),
                    new Color32(121, 173, 245, 255)),

                new RampReference(
                    new Color32(68, 18, 17, 255),
                    new Color32(2, 3, 4, 255),
                    new Color32(81, 168, 136, 255)),

                new RampReference(
                    new Color32(62, 30, 27, 255),
                    new Color32(3, 4, 3, 255),
                    new Color32(81, 190, 186, 255)),

                new RampReference(
                    new Color32(56, 45, 41, 255),
                    new Color32(5, 5, 2, 255),
                    new Color32(82, 214, 251, 255)),

                new RampReference(
                    new Color32(73, 67, 45, 255),
                    new Color32(5, 5, 2, 255),
                    new Color32(82, 214, 251, 255))
            };

        /// <summary>
        /// Creates an Owlcat-style tiefling skin color ramp from the supplied midtone.
        /// 
        /// Tiefling ramps preserve deep shadows for longer than the human or dhampir
        /// curves, with relatively little channel movement near the dark end followed
        /// by a stronger transition through the middle of the ramp. The second half
        /// can move aggressively toward a very different hue, allowing the characteristic
        /// red, blue, purple, greenish, or other strongly colored tiefling palettes.
        /// As a result, the final ramp may contain more dramatic hue contrast than the
        /// supplied midtone alone would suggest.
        /// 
        /// If shadow or highlight are not supplied, their colors are taken from the
        /// official tiefling ramp whose midtone is closest to the requested color.
        /// This is particularly important for tieflings because similarly dark
        /// midtones can intentionally transition toward very different secondary
        /// colors. Either endpoint may be overridden while retaining the tiefling
        /// interpolation curve.
        /// </summary>
        /// <param name="midtone">
        /// The primary skin color around which the ramp is constructed.
        /// </param>
        /// <param name="shadow">
        /// Optional custom dark endpoint. If null, an appropriate shadow color is
        /// selected automatically from the nearest official tiefling palette.
        /// </param>
        /// <param name="highlight">
        /// Optional custom secondary/end color. If null, an appropriate color is
        /// selected automatically from the nearest official tiefling palette.
        /// </param>
        /// <returns>
        /// A new 256x1 Texture2D containing the generated tiefling skin color ramp.
        /// </returns>
        public static Texture2D CreateTieflingSkinRamp(
            Color midtone,
            Color? shadow = null,
            Color? highlight = null)
        {
            RampReference reference =
                GetNearestReference(
                    midtone,
                    TieflingReferences);

            return CreateRamp(
                shadow ?? reference.Shadow,
                midtone,
                highlight ?? reference.Highlight,
                TieflingProfile);
        }

        // ============================================================
        // Oread
        // ============================================================

        private enum OreadProfile
        {
            DarkStone,
            Onyx,
            Blue,
            Gray,
            Jasper,
            Malachite,
            EarthSand
        }


        private readonly struct OreadReference
        {
            public readonly OreadProfile Profile;

            public readonly Color Midtone;
            public readonly Color Shadow;
            public readonly Color Highlight;

            public OreadReference(
                OreadProfile profile,
                Color midtone,
                Color shadow,
                Color highlight)
            {
                Profile = profile;

                Midtone = midtone;
                Shadow = shadow;
                Highlight = highlight;
            }
        }


        // ------------------------------------------------------------
        // Oread: Dark Stone
        // Black Marble / Dark Onyx
        // ------------------------------------------------------------

        private static readonly RampCurveProfile
            OreadDarkStoneProfile =
            new RampCurveProfile(
                DefaultMidIndex,

                new float[]
                {
                    0.000000f,
                    0.000000f,
                    0.132743f,
                    0.265487f,
                    0.336283f,
                    0.345133f,
                    0.548673f,
                    0.469027f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.000000f,
                    0.079585f,
                    0.079585f,
                    0.079585f,
                    0.131488f,
                    0.314879f,
                    0.498270f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.000000f,
                    0.000000f,
                    0.075410f,
                    0.075410f,
                    0.075410f,
                    0.308197f,
                    0.668852f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.056231f,
                    0.153302f,
                    0.276535f,
                    0.426955f,
                    0.613192f,
                    0.780821f,
                    0.910459f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.037160f,
                    0.096037f,
                    0.187068f,
                    0.330278f,
                    0.536596f,
                    0.733394f,
                    0.894497f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.011658f,
                    0.031048f,
                    0.089096f,
                    0.222383f,
                    0.441191f,
                    0.669276f,
                    0.870216f,
                    1.000000f
                });


        // ------------------------------------------------------------
        // Oread: Onyx
        // ------------------------------------------------------------

        private static readonly RampCurveProfile
            OreadOnyxProfile =
            new RampCurveProfile(
                DefaultMidIndex,

                new float[]
                {
                    0.000000f,
                    0.000000f,
                    0.019270f,
                    0.063895f,
                    0.153144f,
                    0.267748f,
                    0.279919f,
                    0.279919f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.000000f,
                    0.012517f,
                    0.020938f,
                    0.033455f,
                    0.079426f,
                    0.217569f,
                    0.506145f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.000000f,
                    0.008176f,
                    0.020126f,
                    0.020126f,
                    0.076101f,
                    0.501258f,
                    0.750314f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.240883f,
                    0.511701f,
                    0.724874f,
                    0.884858f,
                    1.000000f,
                    1.000000f,
                    1.000000f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.175155f,
                    0.353715f,
                    0.591099f,
                    0.821672f,
                    1.000000f,
                    1.000000f,
                    1.000000f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.113332f,
                    0.261123f,
                    0.501049f,
                    0.790944f,
                    1.000000f,
                    1.000000f,
                    1.000000f,
                    1.000000f
                });


        // ------------------------------------------------------------
        // Oread: Blue
        // ------------------------------------------------------------

        private static readonly RampCurveProfile
            OreadBlueProfile =
            new RampCurveProfile(
                DefaultMidIndex,

                new float[]
                {
                    0.000000f,
                    0.000000f,
                    0.000000f,
                    0.012976f,
                    0.065744f,
                    0.211073f,
                    0.190311f,
                    0.354671f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.000000f,
                    0.011711f,
                    0.018481f,
                    0.041903f,
                    0.113998f,
                    0.241537f,
                    0.399268f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.013798f,
                    0.022359f,
                    0.044717f,
                    0.061839f,
                    0.096082f,
                    0.414342f,
                    0.591802f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.182215f,
                    0.314733f,
                    0.582523f,
                    0.881378f,
                    1.000000f,
                    1.000000f,
                    1.000000f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.290157f,
                    0.516527f,
                    0.717739f,
                    0.882335f,
                    1.000000f,
                    1.000000f,
                    1.000000f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.537413f,
                    1.047203f,
                    1.034965f,
                    0.902098f,
                    1.000000f,
                    1.000000f,
                    1.000000f,
                    1.000000f
                });


        // ------------------------------------------------------------
        // Oread: Gray
        // ------------------------------------------------------------

        private static readonly RampCurveProfile
            OreadGrayProfile =
            new RampCurveProfile(
                DefaultMidIndex,

                new float[]
                {
                    0.000000f,
                    0.000000f,
                    0.000000f,
                    0.011773f,
                    0.078428f,
                    0.247057f,
                    0.299402f,
                    0.528165f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.000000f,
                    0.010925f,
                    0.017352f,
                    0.039203f,
                    0.106684f,
                    0.295630f,
                    0.528278f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.014911f,
                    0.024260f,
                    0.048521f,
                    0.057870f,
                    0.076568f,
                    0.313609f,
                    0.442249f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.252823f,
                    0.475189f,
                    0.688014f,
                    0.875757f,
                    1.000000f,
                    1.000000f,
                    1.000000f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.249105f,
                    0.469743f,
                    0.678367f,
                    0.867007f,
                    1.000000f,
                    1.000000f,
                    1.000000f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.336998f,
                    0.606402f,
                    0.785475f,
                    0.913019f,
                    1.000000f,
                    1.000000f,
                    1.000000f,
                    1.000000f
                });


        // ------------------------------------------------------------
        // Oread: Jasper
        // ------------------------------------------------------------

        private static readonly RampCurveProfile
            OreadJasperProfile =
            new RampCurveProfile(
                DefaultMidIndex,

                new float[]
                {
                    0.000000f,
                    0.000000f,
                    0.000000f,
                    0.008624f,
                    0.056350f,
                    0.190904f,
                    0.468636f,
                    0.710455f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.014058f,
                    0.036340f,
                    0.064456f,
                    0.086737f,
                    0.109019f,
                    0.173475f,
                    0.346950f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.006524f,
                    0.018980f,
                    0.050415f,
                    0.062871f,
                    0.075326f,
                    0.193950f,
                    0.287663f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.206216f,
                    0.397073f,
                    0.634656f,
                    0.847237f,
                    1.000000f,
                    1.000000f,
                    1.000000f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.322979f,
                    0.579107f,
                    0.766808f,
                    0.899212f,
                    1.000000f,
                    1.000000f,
                    1.000000f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.445664f,
                    0.776738f,
                    0.920979f,
                    0.964158f,
                    1.000000f,
                    1.000000f,
                    1.000000f,
                    1.000000f
                });


        // ------------------------------------------------------------
        // Oread: Malachite
        // ------------------------------------------------------------

        private static readonly RampCurveProfile
            OreadMalachiteProfile =
            new RampCurveProfile(
                DefaultMidIndex,

                new float[]
                {
                    0.000000f,
                    0.017850f,
                    0.035700f,
                    0.067140f,
                    0.130020f,
                    0.229412f,
                    0.148682f,
                    0.288844f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.000000f,
                    0.012500f,
                    0.012500f,
                    0.043750f,
                    0.100000f,
                    0.337500f,
                    0.587500f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.000000f,
                    0.041584f,
                    0.076733f,
                    0.091089f,
                    0.092574f,
                    0.247525f,
                    0.515842f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.386824f,
                    0.683461f,
                    0.838058f,
                    0.930670f,
                    1.000000f,
                    1.000000f,
                    1.000000f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.263912f,
                    0.490845f,
                    0.688517f,
                    0.870692f,
                    1.000000f,
                    1.000000f,
                    1.000000f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.216894f,
                    0.430166f,
                    0.646357f,
                    0.850926f,
                    1.000000f,
                    1.000000f,
                    1.000000f,
                    1.000000f
                });


        // ------------------------------------------------------------
        // Oread: Earth / Sand
        // ------------------------------------------------------------

        private static readonly RampCurveProfile
            OreadEarthSandProfile =
            new RampCurveProfile(
                DefaultMidIndex,

                new float[]
                {
                    0.000000f,
                    0.008491f,
                    0.016981f,
                    0.038679f,
                    0.082075f,
                    0.217925f,
                    0.420755f,
                    0.603774f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.006908f,
                    0.013816f,
                    0.031010f,
                    0.058489f,
                    0.120203f,
                    0.274793f,
                    0.556647f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.000000f,
                    0.025230f,
                    0.050459f,
                    0.075689f,
                    0.100919f,
                    0.135774f,
                    0.329444f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.320877f,
                    0.540637f,
                    0.729928f,
                    0.880954f,
                    1.000000f,
                    1.000000f,
                    1.000000f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.237027f,
                    0.440756f,
                    0.653149f,
                    0.849075f,
                    1.000000f,
                    1.000000f,
                    1.000000f,
                    1.000000f
                },

                new float[]
                {
                    0.000000f,
                    0.432447f,
                    0.696511f,
                    0.842703f,
                    0.934806f,
                    1.000000f,
                    1.000000f,
                    1.000000f,
                    1.000000f
                });


        private static readonly OreadReference[]
            OreadReferences =
            {
                // Black Marble
                new OreadReference(
                    OreadProfile.DarkStone,
                    new Color32(7, 8, 7, 255),
                    new Color32(0, 0, 0, 255),
                    new Color32(182, 169, 136, 255)),

                // Blue Onyx
                new OreadReference(
                    OreadProfile.Onyx,
                    new Color32(29, 57, 58, 255),
                    new Color32(4, 2, 1, 255),
                    new Color32(184, 171, 137, 255)),

                // Dark Blue
                new OreadReference(
                    OreadProfile.Blue,
                    new Color32(34, 38, 54, 255),
                    new Color32(2, 1, 2, 255),
                    new Color32(192, 149, 131, 255)),

                // Dark Gray
                new OreadReference(
                    OreadProfile.Gray,
                    new Color32(38, 41, 49, 255),
                    new Color32(2, 1, 2, 255),
                    new Color32(137, 155, 184, 255)),

                // Dark Jasper
                new OreadReference(
                    OreadProfile.Jasper,
                    new Color32(57, 33, 34, 255),
                    new Color32(1, 2, 1, 255),
                    new Color32(137, 155, 184, 255)),

                // Dark Malachite
                new OreadReference(
                    OreadProfile.Malachite,
                    new Color32(23, 33, 28, 255),
                    new Color32(2, 1, 2, 255),
                    new Color32(184, 139, 137, 255)),

                // Dark Onyx
                new OreadReference(
                    OreadProfile.DarkStone,
                    new Color32(8, 15, 16, 255),
                    new Color32(0, 0, 0, 255),
                    new Color32(182, 169, 136, 255)),

                // Earth
                new OreadReference(
                    OreadProfile.EarthSand,
                    new Color32(51, 47, 63, 255),
                    new Color32(1, 2, 2, 255),
                    new Color32(160, 157, 200, 255)),

                // Jasper
                new OreadReference(
                    OreadProfile.Jasper,
                    new Color32(74, 55, 65, 255),
                    new Color32(1, 2, 2, 255),
                    new Color32(137, 155, 184, 255)),

                // Malachite
                new OreadReference(
                    OreadProfile.Malachite,
                    new Color32(69, 65, 60, 255),
                    new Color32(2, 1, 2, 255),
                    new Color32(184, 139, 137, 255)),

                // Medium Blue
                new OreadReference(
                    OreadProfile.Blue,
                    new Color32(62, 65, 87, 255),
                    new Color32(2, 1, 2, 255),
                    new Color32(192, 149, 131, 255)),

                // Medium Gray
                new OreadReference(
                    OreadProfile.Gray,
                    new Color32(67, 69, 81, 255),
                    new Color32(2, 1, 2, 255),
                    new Color32(137, 155, 184, 255)),

                // Medium Onyx
                new OreadReference(
                    OreadProfile.Onyx,
                    new Color32(21, 38, 39, 255),
                    new Color32(2, 1, 0, 255),
                    new Color32(184, 171, 137, 255)),

                // Sand
                new OreadReference(
                    OreadProfile.EarthSand,
                    new Color32(92, 68, 58, 255),
                    new Color32(2, 1, 2, 255),
                    new Color32(184, 139, 137, 255))
            };

        /// <summary>
        /// Creates an Owlcat-style oread skin color ramp from the supplied midtone.
        /// 
        /// Oread ramps are substantially more varied than the other supported races
        /// and do not follow a single universal interpolation curve. Instead, the
        /// official ramps form several mineral-like families such as onyx, blue stone,
        /// gray stone, jasper, malachite, and earth/sand. These curves commonly retain
        /// very dark, compressed coloration through the early part of the ramp before
        /// transitioning much more strongly toward the secondary color. Depending on
        /// the selected family, individual RGB channels may accelerate at different
        /// rates or briefly plateau, producing the unusual stone, mineral, and
        /// contrasting-vein coloration characteristic of oread skins.
        /// 
        /// The generator automatically finds the official oread midtone nearest to the
        /// requested color and uses that reference to select both the most appropriate
        /// curve family and, unless overridden, its shadow and secondary/end colors.
        /// Consequently, changing the supplied midtone can select a qualitatively
        /// different mineral-style gradient rather than merely recoloring one fixed
        /// oread curve.
        /// </summary>
        /// <param name="midtone">
        /// The primary skin color around which the ramp is constructed. Its RGB color
        /// is also used to select the most appropriate oread curve family.
        /// </param>
        /// <param name="shadow">
        /// Optional custom dark endpoint. If null, the shadow from the selected
        /// official oread reference palette is used.
        /// </param>
        /// <param name="highlight">
        /// Optional custom secondary/end color. If null, the color from the selected
        /// official oread reference palette is used.
        /// </param>
        /// <returns>
        /// A new 256x1 Texture2D containing the generated oread skin color ramp.
        /// </returns>
        public static Texture2D CreateOreadSkinRamp(
            Color midtone,
            Color? shadow = null,
            Color? highlight = null)
        {
            OreadReference reference =
                GetNearestOreadReference(
                    midtone);

            RampCurveProfile profile =
                GetOreadProfile(
                    reference.Profile);

            return CreateRamp(
                shadow ?? reference.Shadow,
                midtone,
                highlight ?? reference.Highlight,
                profile);
        }

        private static OreadReference GetNearestOreadReference(
            Color midtone)
        {
            int bestIndex = 0;

            float bestDistance =
                float.MaxValue;

            for (int i = 0; i < OreadReferences.Length; i++)
            {
                float distance =
                    ColorDistanceSquared(
                        midtone,
                        OreadReferences[i].Midtone);

                if (distance < bestDistance)
                {
                    bestDistance =
                        distance;

                    bestIndex = i;
                }
            }

            return OreadReferences[bestIndex];
        }

        private static RampCurveProfile GetOreadProfile(
            OreadProfile profile)
        {
            switch (profile)
            {
                case OreadProfile.DarkStone:
                    return OreadDarkStoneProfile;

                case OreadProfile.Onyx:
                    return OreadOnyxProfile;

                case OreadProfile.Blue:
                    return OreadBlueProfile;

                case OreadProfile.Gray:
                    return OreadGrayProfile;

                case OreadProfile.Jasper:
                    return OreadJasperProfile;

                case OreadProfile.Malachite:
                    return OreadMalachiteProfile;

                case OreadProfile.EarthSand:
                    return OreadEarthSandProfile;

                default:
                    return OreadGrayProfile;
            }
        }

        // ============================================================
        // Human-compatible aliases
        // ============================================================

        /// <summary>
        /// Creates an Owlcat-style Aasimar skin color ramp.
        ///
        /// The exported Aasimar skin ramps reuse the same human-compatible
        /// gradient behavior, so this is a convenience alias for
        /// CreateHumanSkinRamp. The curve is warm through the shadow/midtone
        /// region and shifts toward a cool blue/cyan secondary color at the
        /// far end.
        /// </summary>
        /// <param name="midtone">
        /// The primary skin color. This exact color is placed at pixel 140.
        /// </param>
        /// <param name="shadow">
        /// Optional custom dark endpoint. If null, the human automatic
        /// endpoint estimator is used.
        /// </param>
        /// <param name="highlight">
        /// Optional custom secondary/end color. If null, the human automatic
        /// endpoint estimator is used.
        /// </param>
        /// <returns>
        /// A new 256x1 Texture2D containing the generated Aasimar skin color ramp.
        /// </returns>
        public static Texture2D CreateAasimarSkinRamp(
            Color midtone,
            Color? shadow = null,
            Color? highlight = null)
        {
            return CreateHumanSkinRamp(
                midtone,
                shadow,
                highlight);
        }


        /// <summary>
        /// Creates an Owlcat-style elf skin color ramp.
        ///
        /// The exported elf skin ramps use the same human-compatible
        /// interpolation behavior, so this is a convenience alias for
        /// CreateHumanSkinRamp. The curve develops warmer shadows first and
        /// then transitions toward a cooler blue/cyan secondary color.
        /// </summary>
        /// <param name="midtone">
        /// The primary skin color. This exact color is placed at pixel 140.
        /// </param>
        /// <param name="shadow">
        /// Optional custom dark endpoint. If null, the human automatic
        /// endpoint estimator is used.
        /// </param>
        /// <param name="highlight">
        /// Optional custom secondary/end color. If null, the human automatic
        /// endpoint estimator is used.
        /// </param>
        /// <returns>
        /// A new 256x1 Texture2D containing the generated elf skin color ramp.
        /// </returns>
        public static Texture2D CreateElfSkinRamp(
            Color midtone,
            Color? shadow = null,
            Color? highlight = null)
        {
            return CreateHumanSkinRamp(
                midtone,
                shadow,
                highlight);
        }


        // ============================================================
        // Gnome
        // ============================================================

        // ------------------------------------------------------------
        // Gnome Native Profile
        // Blue / Green / Pink / White
        // ------------------------------------------------------------

        private static readonly RampCurveProfile
            GnomeNativeProfile =
            new RampCurveProfile(
                158,

                // R: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.019226f,
                    0.057678f,
                    0.135878f,
                    0.255125f,
                    0.432594f,
                    0.712840f,
                    0.968810f,
                    1.000000f
                },

                // G: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.000000f,
                    0.016509f,
                    0.035706f,
                    0.074100f,
                    0.177639f,
                    0.402918f,
                    0.739794f,
                    1.000000f
                },

                // B: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.000000f,
                    0.013372f,
                    0.040117f,
                    0.080234f,
                    0.174009f,
                    0.333914f,
                    0.653921f,
                    1.000000f
                },

                // R: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.069956f,
                    0.266875f,
                    0.456228f,
                    0.652160f,
                    0.800231f,
                    0.923920f,
                    0.986486f,
                    1.000000f
                },

                // G: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.156017f,
                    0.317701f,
                    0.481562f,
                    0.634555f,
                    0.764946f,
                    0.867248f,
                    0.942706f,
                    1.000000f
                },

                // B: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.133305f,
                    0.282739f,
                    0.439599f,
                    0.597431f,
                    0.750906f,
                    0.859323f,
                    0.945049f,
                    1.000000f
                });


        private readonly struct GnomeReference
        {
            public readonly RampCurveProfile Profile;

            public readonly Color Midtone;
            public readonly Color Shadow;
            public readonly Color Highlight;

            public GnomeReference(
                RampCurveProfile profile,
                Color midtone,
                Color shadow,
                Color highlight)
            {
                Profile = profile;

                Midtone = midtone;
                Shadow = shadow;
                Highlight = highlight;
            }
        }


        private static readonly GnomeReference[]
            GnomeReferences =
            {
                // ----------------------------------------------------
                // Native GN palettes
                // ----------------------------------------------------

                // Blue_U_GN
                new GnomeReference(
                    GnomeNativeProfile,
                    new Color32(101, 111, 92, 255),
                    new Color32(8, 4, 3, 255),
                    new Color32(84, 214, 251, 255)),

                // Green_U_GN
                new GnomeReference(
                    GnomeNativeProfile,
                    new Color32(108, 118, 74, 255),
                    new Color32(8, 4, 3, 255),
                    new Color32(84, 214, 251, 255)),

                // Pink_U_GN
                new GnomeReference(
                    GnomeNativeProfile,
                    new Color32(121, 97, 74, 255),
                    new Color32(8, 4, 3, 255),
                    new Color32(84, 214, 251, 255)),

                // White_U_GN
                new GnomeReference(
                    GnomeNativeProfile,
                    new Color32(121, 109, 74, 255),
                    new Color32(8, 4, 3, 255),
                    new Color32(84, 214, 251, 255)),


                // ----------------------------------------------------
                // Reused HM palettes
                // ----------------------------------------------------

                // Medium_U_HM
                new GnomeReference(
                    HumanProfile,
                    new Color32(56, 29, 18, 255),
                    new Color32(2, 1, 1, 255),
                    new Color32(63, 140, 184, 255)),

                // White_U_HM
                new GnomeReference(
                    HumanProfile,
                    new Color32(90, 46, 28, 255),
                    new Color32(8, 4, 3, 255),
                    new Color32(82, 214, 251, 255)),

                // YellowLight_U_HM
                new GnomeReference(
                    HumanProfile,
                    new Color32(108, 76, 36, 255),
                    new Color32(8, 4, 3, 255),
                    new Color32(82, 214, 251, 255)),

                // YellowMedium_U_HM
                new GnomeReference(
                    HumanProfile,
                    new Color32(99, 60, 32, 255),
                    new Color32(8, 4, 3, 255),
                    new Color32(82, 214, 251, 255))
            };


        /// <summary>
        /// Creates an Owlcat-style Gnome skin color ramp from the supplied midtone.
        /// 
        /// Gnome skin options contain two different gradient families. Conventional
        /// human-like Gnome complexions reuse the standard human ramp curve, while
        /// the native colorful GN palettes (Blue, Green, Pink, and White) use a
        /// distinct Gnome-specific curve.
        /// 
        /// The native Gnome curve begins similarly to the human curve, keeping the
        /// lowest portion very dark while red develops more quickly than green and
        /// blue. As it approaches the midtone, however, green and blue accelerate
        /// more strongly. Beyond the midpoint the palette turns toward its secondary
        /// color comparatively quickly: red typically falls or changes only modestly
        /// while green and especially blue rise strongly. This gives the native
        /// Gnome ramps their characteristic colorful, pastel-like main coloration
        /// followed by a pronounced cool cyan/blue transition toward the far end.
        /// 
        /// The generator finds the nearest official Gnome palette and uses that
        /// reference to choose both the appropriate curve family and, unless
        /// overridden, the shadow and secondary/end colors. Consequently, a
        /// conventional flesh-toned input may use the human curve while a color
        /// resembling one of the native Gnome palettes will use the Gnome-specific
        /// curve.
        /// 
        /// Either endpoint may be overridden without changing the selected
        /// interpolation curve.
        /// </summary>
        /// <param name="midtone">
        /// The primary skin color around which the ramp is constructed. Its RGB
        /// value is also used to determine whether the Human or native Gnome
        /// gradient profile is the closest match.
        /// </param>
        /// <param name="shadow">
        /// Optional custom dark endpoint. If null, the shadow from the nearest
        /// official Gnome palette is used.
        /// </param>
        /// <param name="highlight">
        /// Optional custom secondary/end color. If null, the secondary color from
        /// the nearest official Gnome palette is used.
        /// </param>
        /// <returns>
        /// A new 256x1 Texture2D containing the generated Gnome skin color ramp.
        /// </returns>
        public static Texture2D CreateGnomeSkinRamp(
            Color midtone,
            Color? shadow = null,
            Color? highlight = null)
        {
            GnomeReference reference =
                GetNearestGnomeReference(
                    midtone);

            return CreateRamp(
                shadow ?? reference.Shadow,
                midtone,
                highlight ?? reference.Highlight,
                reference.Profile);
        }

        private static GnomeReference GetNearestGnomeReference(
            Color midtone)
        {
            int bestIndex = 0;

            float bestDistance =
                float.MaxValue;

            for (int i = 0; i < GnomeReferences.Length; i++)
            {
                float distance =
                    ColorDistanceSquared(
                        midtone,
                        GnomeReferences[i].Midtone);

                if (distance < bestDistance)
                {
                    bestDistance =
                        distance;

                    bestIndex = i;
                }
            }

            return GnomeReferences[bestIndex];
        }

        // ============================================================
        // Human Eyes
        // ============================================================

        private static readonly RampCurveProfile
            HumanEyeProfile =
            new RampCurveProfile(
                140,

                // R: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.009038f,
                    0.036024f,
                    0.087572f,
                    0.181833f,
                    0.335763f,
                    0.547777f,
                    0.783515f,
                    1.000000f
                },

                // G: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.012066f,
                    0.039186f,
                    0.090656f,
                    0.181657f,
                    0.327665f,
                    0.537713f,
                    0.770431f,
                    1.000000f
                },

                // B: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.011856f,
                    0.040189f,
                    0.095302f,
                    0.192996f,
                    0.348841f,
                    0.570696f,
                    0.804309f,
                    1.000000f
                },

                // R: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.145969f,
                    0.329063f,
                    0.490074f,
                    0.630906f,
                    0.761487f,
                    0.868392f,
                    0.952354f,
                    1.000000f
                },

                // G: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.177942f,
                    0.362187f,
                    0.519444f,
                    0.656971f,
                    0.778333f,
                    0.879589f,
                    0.953637f,
                    1.000000f
                },

                // B: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.182257f,
                    0.371086f,
                    0.521305f,
                    0.657703f,
                    0.776178f,
                    0.876802f,
                    0.950527f,
                    1.000000f
                });


        private static readonly RampReference[]
            HumanEyeReferences =
            {
                // Black
                new RampReference(
                    new Color32(5, 5, 5, 255),
                    new Color32(0, 0, 0, 255),
                    new Color32(13, 16, 16, 255)),

                // Blue0
                new RampReference(
                    new Color32(111, 128, 235, 255),
                    new Color32(3, 8, 13, 255),
                    new Color32(155, 206, 243, 255)),

                // Blue1
                new RampReference(
                    new Color32(30, 51, 188, 255),
                    new Color32(1, 3, 3, 255),
                    new Color32(99, 175, 239, 255)),

                // Blue2
                new RampReference(
                    new Color32(7, 27, 90, 255),
                    new Color32(1, 1, 1, 255),
                    new Color32(24, 109, 206, 255)),

                // Brown0
                new RampReference(
                    new Color32(160, 118, 77, 255),
                    new Color32(5, 3, 2, 255),
                    new Color32(255, 218, 160, 255)),

                // Brown1
                new RampReference(
                    new Color32(91, 63, 38, 255),
                    new Color32(5, 3, 2, 255),
                    new Color32(198, 166, 118, 255)),

                // Brown2
                new RampReference(
                    new Color32(39, 21, 10, 255),
                    new Color32(5, 3, 2, 255),
                    new Color32(126, 85, 35, 255)),

                // Brown5
                new RampReference(
                    new Color32(18, 15, 12, 255),
                    new Color32(2, 2, 1, 255),
                    new Color32(96, 72, 43, 255)),

                // Cyan1
                new RampReference(
                    new Color32(11, 117, 168, 255),
                    new Color32(8, 12, 16, 255),
                    new Color32(123, 180, 231, 255)),

                // Cyan2
                new RampReference(
                    new Color32(2, 57, 80, 255),
                    new Color32(5, 7, 10, 255),
                    new Color32(101, 162, 216, 255)),

                // Cyan3
                new RampReference(
                    new Color32(1, 20, 32, 255),
                    new Color32(1, 2, 2, 255),
                    new Color32(26, 59, 85, 255)),

                // Green1
                new RampReference(
                    new Color32(103, 169, 34, 255),
                    new Color32(3, 4, 3, 255),
                    new Color32(206, 224, 95, 255)),

                // Green2
                new RampReference(
                    new Color32(53, 99, 12, 255),
                    new Color32(3, 4, 3, 255),
                    new Color32(206, 224, 95, 255)),

                // Green3
                new RampReference(
                    new Color32(18, 51, 5, 255),
                    new Color32(2, 3, 1, 255),
                    new Color32(82, 97, 37, 255)),

                // Green5
                new RampReference(
                    new Color32(29, 35, 13, 255),
                    new Color32(2, 3, 1, 255),
                    new Color32(89, 97, 37, 255))
            };


        /// <summary>
        /// Creates an Owlcat-style human eye ramp. The curve stays very dark through
        /// the shadows, rises strongly into the supplied iris color, then brightens
        /// rapidly toward a lighter and generally less saturated secondary color.
        /// </summary>
        public static Texture2D CreateHumanEyeRamp(
            Color color,
            Color? shadow = null,
            Color? highlight = null)
        {
            RampReference reference =
                GetNearestReference(
                    color,
                    HumanEyeReferences);

            return CreateRamp(
                shadow ?? reference.Shadow,
                color,
                highlight ?? reference.Highlight,
                HumanEyeProfile);
        }

        // ============================================================
        // Oread Eyes
        // ============================================================

        // ------------------------------------------------------------
        // Oread Eye 0 Profile
        // ------------------------------------------------------------

        private static readonly RampCurveProfile
            OreadEye0Profile =
            new RampCurveProfile(
                140,

                // R: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.014362f,
                    0.049382f,
                    0.115128f,
                    0.226416f,
                    0.399734f,
                    0.625033f,
                    0.857690f,
                    1.000000f
                },

                // G: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.011383f,
                    0.038665f,
                    0.093015f,
                    0.188430f,
                    0.341915f,
                    0.570344f,
                    0.813946f,
                    1.000000f
                },

                // B: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.010929f,
                    0.041202f,
                    0.096127f,
                    0.193942f,
                    0.350507f,
                    0.584617f,
                    0.825398f,
                    1.000000f
                },

                // R: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.395295f,
                    0.649962f,
                    0.773836f,
                    0.839815f,
                    0.890453f,
                    0.928275f,
                    0.970999f,
                    1.000000f
                },

                // G: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.375926f,
                    0.590246f,
                    0.709286f,
                    0.801096f,
                    0.867014f,
                    0.923948f,
                    0.969680f,
                    1.000000f
                },

                // B: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.324789f,
                    0.543451f,
                    0.678969f,
                    0.775506f,
                    0.861853f,
                    0.916385f,
                    0.967535f,
                    1.000000f
                });


        // ------------------------------------------------------------
        // Oread Eye 1 Profile
        // ------------------------------------------------------------

        private static readonly RampCurveProfile
            OreadEye1Profile =
            new RampCurveProfile(
                140,

                // R: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.012411f,
                    0.046711f,
                    0.111854f,
                    0.223978f,
                    0.398489f,
                    0.623263f,
                    0.844683f,
                    1.000000f
                },

                // G: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.012094f,
                    0.041425f,
                    0.095920f,
                    0.192028f,
                    0.338617f,
                    0.560912f,
                    0.799400f,
                    1.000000f
                },

                // B: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.020139f,
                    0.051107f,
                    0.105146f,
                    0.187839f,
                    0.309181f,
                    0.497625f,
                    0.718978f,
                    1.000000f
                },

                // R: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.159532f,
                    0.380212f,
                    0.537122f,
                    0.671843f,
                    0.786194f,
                    0.882256f,
                    0.960554f,
                    1.000000f
                },

                // G: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.169951f,
                    0.337922f,
                    0.492639f,
                    0.634846f,
                    0.765183f,
                    0.866682f,
                    0.948626f,
                    1.000000f
                },

                // B: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.119608f,
                    0.274172f,
                    0.435552f,
                    0.597676f,
                    0.743945f,
                    0.863966f,
                    0.952182f,
                    1.000000f
                });


        private readonly struct OreadEyeReference
        {
            public readonly RampCurveProfile Profile;

            public readonly Color Midtone;
            public readonly Color Shadow;
            public readonly Color Highlight;

            public OreadEyeReference(
                RampCurveProfile profile,
                Color midtone,
                Color shadow,
                Color highlight)
            {
                Profile = profile;
                Midtone = midtone;
                Shadow = shadow;
                Highlight = highlight;
            }
        }


        private static readonly OreadEyeReference[]
            OreadEyeReferences =
            {
                // ----------------------------------------------------
                // Blue
                // ----------------------------------------------------

                // Blue0
                // Exact reuse of the Human Blue0 eye ramp.
                new OreadEyeReference(
                    HumanEyeProfile,
                    new Color32(111, 128, 235, 255),
                    new Color32(3, 8, 13, 255),
                    new Color32(155, 206, 243, 255)),

                // Blue1
                // Exact reuse of the Human Blue1 eye ramp.
                new OreadEyeReference(
                    HumanEyeProfile,
                    new Color32(30, 51, 188, 255),
                    new Color32(1, 3, 3, 255),
                    new Color32(99, 175, 239, 255)),


                // ----------------------------------------------------
                // Cyan
                // ----------------------------------------------------

                // Cyan0
                new OreadEyeReference(
                    OreadEye0Profile,
                    new Color32(139, 198, 218, 255),
                    new Color32(8, 12, 16, 255),
                    new Color32(171, 200, 220, 255)),

                // Cyan1
                // Exact reuse of the Human Cyan1 eye ramp.
                new OreadEyeReference(
                    HumanEyeProfile,
                    new Color32(11, 117, 168, 255),
                    new Color32(8, 12, 16, 255),
                    new Color32(123, 180, 231, 255)),


                // ----------------------------------------------------
                // Green
                // ----------------------------------------------------

                // Green0
                new OreadEyeReference(
                    OreadEye0Profile,
                    new Color32(166, 206, 131, 255),
                    new Color32(3, 4, 3, 255),
                    new Color32(224, 235, 152, 255)),

                // Green1
                // Exact reuse of the Human Green1 eye ramp.
                new OreadEyeReference(
                    HumanEyeProfile,
                    new Color32(103, 169, 34, 255),
                    new Color32(3, 4, 3, 255),
                    new Color32(206, 224, 95, 255)),


                // ----------------------------------------------------
                // Purple
                // ----------------------------------------------------

                // Purple0
                new OreadEyeReference(
                    OreadEye0Profile,
                    new Color32(178, 99, 175, 255),
                    new Color32(20, 4, 16, 255),
                    new Color32(206, 155, 243, 255)),

                // Purple1
                new OreadEyeReference(
                    OreadEye1Profile,
                    new Color32(147, 42, 80, 255),
                    new Color32(13, 4, 20, 255),
                    new Color32(206, 154, 243, 255)),


                // ----------------------------------------------------
                // Red
                // ----------------------------------------------------

                // Red0
                new OreadEyeReference(
                    OreadEye0Profile,
                    new Color32(194, 109, 101, 255),
                    new Color32(15, 1, 0, 255),
                    new Color32(194, 171, 142, 255)),

                // Red1
                new OreadEyeReference(
                    OreadEye1Profile,
                    new Color32(177, 42, 34, 255),
                    new Color32(15, 1, 0, 255),
                    new Color32(226, 152, 74, 255)),


                // ----------------------------------------------------
                // Turquoise
                // ----------------------------------------------------

                // Turquoise0
                new OreadEyeReference(
                    OreadEye0Profile,
                    new Color32(68, 169, 112, 255),
                    new Color32(8, 13, 12, 255),
                    new Color32(126, 239, 108, 255)),

                // Turquoise1
                new OreadEyeReference(
                    OreadEye1Profile,
                    new Color32(22, 152, 68, 255),
                    new Color32(8, 13, 12, 255),
                    new Color32(164, 210, 108, 255)),


                // ----------------------------------------------------
                // Yellow
                // ----------------------------------------------------

                // Yellow0
                new OreadEyeReference(
                    OreadEye0Profile,
                    new Color32(218, 200, 149, 255),
                    new Color32(14, 10, 2, 255),
                    new Color32(245, 233, 188, 255)),

                // Yellow1
                new OreadEyeReference(
                    OreadEye1Profile,
                    new Color32(222, 171, 40, 255),
                    new Color32(8, 6, 2, 255),
                    new Color32(253, 237, 164, 255))
            };


        /// <summary>
        /// Creates an Owlcat-style Oread eye ramp. Brighter palettes transition
        /// sharply after the iris color, while deeper palettes transition more gradually.
        /// </summary>
        public static Texture2D CreateOreadEyeRamp(
            Color color,
            Color? shadow = null,
            Color? highlight = null)
        {
            OreadEyeReference reference =
                GetNearestOreadEyeReference(
                    color);

            return CreateRamp(
                shadow ?? reference.Shadow,
                color,
                highlight ?? reference.Highlight,
                reference.Profile);
        }


        private static OreadEyeReference GetNearestOreadEyeReference(
            Color color)
        {
            int bestIndex = 0;

            float bestDistance =
                float.MaxValue;

            for (int i = 0; i < OreadEyeReferences.Length; i++)
            {
                float distance =
                    ColorDistanceSquared(
                        color,
                        OreadEyeReferences[i].Midtone);

                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestIndex = i;
                }
            }

            return OreadEyeReferences[bestIndex];
        }

        // ============================================================
        // Tiefling Eyes
        // ============================================================

        private static readonly RampCurveProfile
            TieflingEyeProfile =
            new RampCurveProfile(
                140,

                // R: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.008412f,
                    0.031448f,
                    0.077926f,
                    0.161997f,
                    0.303881f,
                    0.528274f,
                    0.778966f,
                    1.000000f
                },

                // G: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.009681f,
                    0.034506f,
                    0.082855f,
                    0.168944f,
                    0.311208f,
                    0.521927f,
                    0.756866f,
                    1.000000f
                },

                // B: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.011527f,
                    0.038872f,
                    0.093423f,
                    0.183451f,
                    0.327612f,
                    0.540473f,
                    0.776419f,
                    1.000000f
                },

                // R: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.196242f,
                    0.398612f,
                    0.558160f,
                    0.684126f,
                    0.796932f,
                    0.888153f,
                    0.956412f,
                    1.000000f
                },

                // G: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.209758f,
                    0.420893f,
                    0.583055f,
                    0.708396f,
                    0.817114f,
                    0.901687f,
                    0.960878f,
                    1.000000f
                },

                // B: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.182534f,
                    0.384739f,
                    0.552673f,
                    0.694405f,
                    0.814321f,
                    0.904622f,
                    0.963523f,
                    1.000000f
                });


        private static readonly RampReference[]
            TieflingEyeReferences =
            {
                // Purple 0
                new RampReference(
                    new Color32(48, 28, 86, 255),
                    new Color32(2, 1, 4, 255),
                    new Color32(146, 119, 211, 255)),

                // Purple 1
                new RampReference(
                    new Color32(80, 38, 108, 255),
                    new Color32(3, 1, 4, 255),
                    new Color32(182, 132, 229, 255)),

                // Magenta
                new RampReference(
                    new Color32(122, 36, 128, 255),
                    new Color32(6, 1, 6, 255),
                    new Color32(214, 129, 215, 255)),

                // Violet / Pink-purple
                new RampReference(
                    new Color32(87, 44, 126, 255),
                    new Color32(3, 1, 5, 255),
                    new Color32(170, 126, 210, 255)),

                // Red
                new RampReference(
                    new Color32(124, 45, 24, 255),
                    new Color32(6, 1, 1, 255),
                    new Color32(222, 156, 103, 255)),

                // Orange
                new RampReference(
                    new Color32(150, 91, 25, 255),
                    new Color32(7, 3, 1, 255),
                    new Color32(240, 191, 91, 255)),

                // Yellow-green
                new RampReference(
                    new Color32(130, 124, 24, 255),
                    new Color32(5, 4, 1, 255),
                    new Color32(225, 233, 94, 255)),

                // Golden yellow
                new RampReference(
                    new Color32(168, 126, 28, 255),
                    new Color32(8, 6, 1, 255),
                    new Color32(245, 223, 108, 255))
            };


        /// <summary>
        /// Creates an Owlcat-style tiefling eye ramp. Tiefling eyes stay dark
        /// through the shadows, then brighten strongly toward vivid end colors.
        /// </summary>
        public static Texture2D CreateTieflingEyeRamp(
            Color color,
            Color? shadow = null,
            Color? highlight = null)
        {
            RampReference reference =
                GetNearestReference(
                    color,
                    TieflingEyeReferences);

            return CreateRamp(
                shadow ?? reference.Shadow,
                color,
                highlight ?? reference.Highlight,
                TieflingEyeProfile);
        }

        // ============================================================
        // Dhampir Eyes
        // ============================================================

        private static readonly RampCurveProfile
            DhampirEyeProfile =
            new RampCurveProfile(
                140,

                // R: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.007824f,
                    0.029761f,
                    0.073667f,
                    0.154733f,
                    0.293511f,
                    0.515921f,
                    0.770514f,
                    1.000000f
                },

                // G: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.008936f,
                    0.032782f,
                    0.079123f,
                    0.162708f,
                    0.302694f,
                    0.511437f,
                    0.748217f,
                    1.000000f
                },

                // B: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.010422f,
                    0.036881f,
                    0.088744f,
                    0.179203f,
                    0.326221f,
                    0.540517f,
                    0.775436f,
                    1.000000f
                },

                // R: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.182654f,
                    0.372915f,
                    0.530508f,
                    0.667212f,
                    0.785994f,
                    0.884937f,
                    0.956988f,
                    1.000000f
                },

                // G: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.202441f,
                    0.399514f,
                    0.558685f,
                    0.696567f,
                    0.811247f,
                    0.901063f,
                    0.963213f,
                    1.000000f
                },

                // B: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.192382f,
                    0.390775f,
                    0.556793f,
                    0.699098f,
                    0.817506f,
                    0.905931f,
                    0.965971f,
                    1.000000f
                });


        private static readonly RampReference[]
            DhampirEyeReferences =
            {
                // Black 0
                new RampReference(
                    new Color32(8, 8, 10, 255),
                    new Color32(0, 0, 0, 255),
                    new Color32(36, 40, 49, 255)),

                // Black 1
                new RampReference(
                    new Color32(14, 16, 22, 255),
                    new Color32(0, 0, 0, 255),
                    new Color32(58, 66, 89, 255)),

                // Blue 0
                new RampReference(
                    new Color32(42, 78, 148, 255),
                    new Color32(2, 4, 8, 255),
                    new Color32(134, 177, 226, 255)),

                // Purple 0
                new RampReference(
                    new Color32(79, 41, 102, 255),
                    new Color32(4, 1, 5, 255),
                    new Color32(178, 131, 213, 255)),

                // Purple 1
                new RampReference(
                    new Color32(55, 23, 80, 255),
                    new Color32(3, 1, 4, 255),
                    new Color32(150, 115, 200, 255)),

                // Purple 2
                new RampReference(
                    new Color32(109, 47, 128, 255),
                    new Color32(6, 1, 6, 255),
                    new Color32(206, 152, 227, 255)),

                // Purple 3
                new RampReference(
                    new Color32(147, 70, 148, 255),
                    new Color32(8, 2, 6, 255),
                    new Color32(228, 176, 230, 255)),

                // Red 0
                new RampReference(
                    new Color32(115, 38, 42, 255),
                    new Color32(6, 1, 1, 255),
                    new Color32(210, 142, 146, 255)),

                // Red 1
                new RampReference(
                    new Color32(150, 54, 36, 255),
                    new Color32(7, 1, 1, 255),
                    new Color32(232, 157, 120, 255)),

                // Red 2 / Orange
                new RampReference(
                    new Color32(168, 92, 34, 255),
                    new Color32(8, 3, 1, 255),
                    new Color32(239, 189, 114, 255)),

                // Violet / Pink 0
                new RampReference(
                    new Color32(118, 63, 138, 255),
                    new Color32(5, 2, 6, 255),
                    new Color32(205, 165, 229, 255)),

                // Violet / Pink 1
                new RampReference(
                    new Color32(158, 84, 152, 255),
                    new Color32(7, 2, 6, 255),
                    new Color32(233, 190, 229, 255)),

                // Violet / Pink 2
                new RampReference(
                    new Color32(196, 108, 176, 255),
                    new Color32(8, 3, 7, 255),
                    new Color32(246, 209, 233, 255)),

                // White / Pale 0
                new RampReference(
                    new Color32(142, 130, 143, 255),
                    new Color32(6, 5, 6, 255),
                    new Color32(219, 221, 234, 255)),

                // White / Pale 1
                new RampReference(
                    new Color32(181, 169, 170, 255),
                    new Color32(7, 6, 6, 255),
                    new Color32(243, 241, 239, 255))
            };


        /// <summary>
        /// Creates an Owlcat-style dhampir eye ramp. Dhampir eyes stay darker and
        /// colder than human eyes, then brighten sharply toward pale or glowing ends.
        /// </summary>
        public static Texture2D CreateDhampirEyeRamp(
            Color color,
            Color? shadow = null,
            Color? highlight = null)
        {
            RampReference reference =
                GetNearestReference(
                    color,
                    DhampirEyeReferences);

            return CreateRamp(
                shadow ?? reference.Shadow,
                color,
                highlight ?? reference.Highlight,
                DhampirEyeProfile);
        }

        // ============================================================
        // Gnome Eyes
        // ============================================================

        // ------------------------------------------------------------
        // Gnome Eye 0 Profile
        // ------------------------------------------------------------

        private static readonly RampCurveProfile
            GnomeEye0Profile =
            new RampCurveProfile(
                140,

                // R: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.016204f,
                    0.048393f,
                    0.106691f,
                    0.205970f,
                    0.372321f,
                    0.585048f,
                    0.797702f,
                    1.000000f
                },

                // G: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.012655f,
                    0.040107f,
                    0.089647f,
                    0.185017f,
                    0.346404f,
                    0.565142f,
                    0.787931f,
                    1.000000f
                },

                // B: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.014434f,
                    0.050207f,
                    0.106798f,
                    0.207674f,
                    0.371696f,
                    0.587960f,
                    0.804969f,
                    1.000000f
                },

                // R: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.339674f,
                    0.570945f,
                    0.712581f,
                    0.802981f,
                    0.877080f,
                    0.915430f,
                    0.965781f,
                    1.000000f
                },

                // G: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.377166f,
                    0.583259f,
                    0.686741f,
                    0.780590f,
                    0.864128f,
                    0.922890f,
                    0.966508f,
                    1.000000f
                },

                // B: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.412822f,
                    0.661773f,
                    0.780797f,
                    0.849366f,
                    0.902210f,
                    0.920214f,
                    0.952992f,
                    1.000000f
                });


        // ------------------------------------------------------------
        // Gnome Eye 1 Profile
        // ------------------------------------------------------------

        private static readonly RampCurveProfile
            GnomeEye1Profile =
            new RampCurveProfile(
                140,

                // R: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.012930f,
                    0.042861f,
                    0.101393f,
                    0.186023f,
                    0.332984f,
                    0.521458f,
                    0.717922f,
                    1.000000f
                },

                // G: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.013205f,
                    0.044143f,
                    0.091277f,
                    0.178327f,
                    0.315375f,
                    0.517826f,
                    0.736544f,
                    1.000000f
                },

                // B: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.019602f,
                    0.050702f,
                    0.104210f,
                    0.187671f,
                    0.327311f,
                    0.518761f,
                    0.734913f,
                    1.000000f
                },

                // R: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.219905f,
                    0.442077f,
                    0.577233f,
                    0.702870f,
                    0.809440f,
                    0.889709f,
                    0.960601f,
                    1.000000f
                },

                // G: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.235859f,
                    0.442858f,
                    0.581583f,
                    0.706429f,
                    0.807166f,
                    0.886257f,
                    0.957521f,
                    1.000000f
                },

                // B: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.182327f,
                    0.379982f,
                    0.524593f,
                    0.663259f,
                    0.787056f,
                    0.874401f,
                    0.952732f,
                    1.000000f
                });


        // ------------------------------------------------------------
        // Gnome Eye 2 Profile
        // ------------------------------------------------------------

        private static readonly RampCurveProfile
            GnomeEye2Profile =
            new RampCurveProfile(
                140,

                // R: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.016791f,
                    0.045372f,
                    0.106056f,
                    0.196083f,
                    0.336639f,
                    0.519452f,
                    0.726210f,
                    1.000000f
                },

                // G: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.027009f,
                    0.054886f,
                    0.083842f,
                    0.147549f,
                    0.248357f,
                    0.403835f,
                    0.617390f,
                    1.000000f
                },

                // B: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.012802f,
                    0.031741f,
                    0.076180f,
                    0.120614f,
                    0.221868f,
                    0.362397f,
                    0.594955f,
                    1.000000f
                },

                // R: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.145376f,
                    0.331881f,
                    0.483306f,
                    0.626525f,
                    0.756416f,
                    0.864211f,
                    0.946008f,
                    1.000000f
                },

                // G: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.141418f,
                    0.322182f,
                    0.474605f,
                    0.621099f,
                    0.752768f,
                    0.865184f,
                    0.945803f,
                    1.000000f
                },

                // B: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.115388f,
                    0.275376f,
                    0.430162f,
                    0.590049f,
                    0.726333f,
                    0.849617f,
                    0.946643f,
                    1.000000f
                });


        private readonly struct GnomeEyeReference
        {
            public readonly RampCurveProfile Profile;

            public readonly Color Midtone;
            public readonly Color Shadow;
            public readonly Color Highlight;

            public GnomeEyeReference(
                RampCurveProfile profile,
                Color midtone,
                Color shadow,
                Color highlight)
            {
                Profile = profile;
                Midtone = midtone;
                Shadow = shadow;
                Highlight = highlight;
            }
        }


        private static readonly GnomeEyeReference[]
            GnomeEyeReferences =
            {
                // ----------------------------------------------------
                // Brown
                // ----------------------------------------------------

                new GnomeEyeReference(
                    GnomeEye0Profile,
                    new Color32(160, 118, 77, 255),
                    new Color32(5, 3, 2, 255),
                    new Color32(255, 218, 160, 255)),

                new GnomeEyeReference(
                    GnomeEye1Profile,
                    new Color32(91, 63, 38, 255),
                    new Color32(5, 3, 2, 255),
                    new Color32(198, 166, 118, 255)),


                // ----------------------------------------------------
                // Cyan
                // ----------------------------------------------------

                new GnomeEyeReference(
                    GnomeEye0Profile,
                    new Color32(139, 198, 218, 255),
                    new Color32(8, 12, 16, 255),
                    new Color32(171, 200, 220, 255)),

                new GnomeEyeReference(
                    GnomeEye1Profile,
                    new Color32(11, 117, 168, 255),
                    new Color32(8, 12, 16, 255),
                    new Color32(123, 180, 231, 255)),

                new GnomeEyeReference(
                    GnomeEye2Profile,
                    new Color32(2, 57, 80, 255),
                    new Color32(5, 7, 10, 255),
                    new Color32(101, 162, 216, 255)),


                // ----------------------------------------------------
                // Green
                // ----------------------------------------------------

                new GnomeEyeReference(
                    GnomeEye0Profile,
                    new Color32(166, 206, 131, 255),
                    new Color32(3, 4, 3, 255),
                    new Color32(224, 235, 152, 255)),

                new GnomeEyeReference(
                    GnomeEye1Profile,
                    new Color32(103, 169, 34, 255),
                    new Color32(3, 4, 3, 255),
                    new Color32(206, 224, 95, 255)),

                new GnomeEyeReference(
                    GnomeEye2Profile,
                    new Color32(53, 99, 12, 255),
                    new Color32(3, 4, 3, 255),
                    new Color32(206, 224, 95, 255)),


                // ----------------------------------------------------
                // Orange
                // ----------------------------------------------------

                new GnomeEyeReference(
                    GnomeEye1Profile,
                    new Color32(210, 111, 42, 255),
                    new Color32(15, 5, 0, 255),
                    new Color32(233, 214, 142, 255)),

                new GnomeEyeReference(
                    GnomeEye2Profile,
                    new Color32(184, 63, 1, 255),
                    new Color32(15, 5, 0, 255),
                    new Color32(222, 188, 74, 255)),


                // ----------------------------------------------------
                // Purple
                // ----------------------------------------------------

                new GnomeEyeReference(
                    GnomeEye0Profile,
                    new Color32(178, 99, 175, 255),
                    new Color32(20, 4, 16, 255),
                    new Color32(206, 155, 243, 255)),

                new GnomeEyeReference(
                    GnomeEye1Profile,
                    new Color32(147, 42, 80, 255),
                    new Color32(13, 4, 20, 255),
                    new Color32(206, 154, 243, 255)),

                new GnomeEyeReference(
                    GnomeEye2Profile,
                    new Color32(78, 13, 33, 255),
                    new Color32(10, 3, 16, 255),
                    new Color32(186, 123, 235, 255)),


                // ----------------------------------------------------
                // Turquoise
                // ----------------------------------------------------

                new GnomeEyeReference(
                    GnomeEye0Profile,
                    new Color32(68, 169, 112, 255),
                    new Color32(8, 13, 12, 255),
                    new Color32(126, 239, 108, 255)),

                new GnomeEyeReference(
                    GnomeEye1Profile,
                    new Color32(22, 152, 68, 255),
                    new Color32(8, 13, 12, 255),
                    new Color32(164, 210, 108, 255)),

                new GnomeEyeReference(
                    GnomeEye2Profile,
                    new Color32(2, 79, 38, 255),
                    new Color32(5, 9, 8, 255),
                    new Color32(173, 216, 121, 255)),


                // ----------------------------------------------------
                // Violet
                // ----------------------------------------------------

                new GnomeEyeReference(
                    GnomeEye0Profile,
                    new Color32(159, 127, 204, 255),
                    new Color32(17, 4, 20, 255),
                    new Color32(222, 206, 233, 255)),

                new GnomeEyeReference(
                    GnomeEye1Profile,
                    new Color32(68, 37, 108, 255),
                    new Color32(13, 4, 14, 255),
                    new Color32(204, 169, 228, 255)),

                new GnomeEyeReference(
                    GnomeEye2Profile,
                    new Color32(41, 21, 66, 255),
                    new Color32(7, 2, 5, 255),
                    new Color32(196, 144, 178, 255))
            };


        /// <summary>
        /// Creates an Owlcat-style Gnome eye ramp. Gnome eyes range from bright,
        /// rapidly transitioning palettes to deeper and more saturated variants.
        /// </summary>
        public static Texture2D CreateGnomeEyeRamp(
            Color color,
            Color? shadow = null,
            Color? highlight = null)
        {
            GnomeEyeReference reference =
                GetNearestGnomeEyeReference(
                    color);

            return CreateRamp(
                shadow ?? reference.Shadow,
                color,
                highlight ?? reference.Highlight,
                reference.Profile);
        }


        private static GnomeEyeReference GetNearestGnomeEyeReference(
            Color color)
        {
            int bestIndex = 0;

            float bestDistance =
                float.MaxValue;

            for (int i = 0; i < GnomeEyeReferences.Length; i++)
            {
                float distance =
                    ColorDistanceSquared(
                        color,
                        GnomeEyeReferences[i].Midtone);

                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestIndex = i;
                }
            }

            return GnomeEyeReferences[bestIndex];
        }

        // placeholder to continue eye ramp creators

        // ============================================================
        // Human Hair
        // ============================================================

        // ------------------------------------------------------------
        // Human Hair Profile 0
        // ------------------------------------------------------------

        private static readonly RampCurveProfile
            HumanHairProfile0 =
            new RampCurveProfile(
                140,

                // R: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.018688f,
                    0.058907f,
                    0.126933f,
                    0.243898f,
                    0.429067f,
                    0.645211f,
                    0.849665f,
                    1.000000f
                },

                // G: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.010836f,
                    0.040379f,
                    0.090322f,
                    0.190199f,
                    0.358266f,
                    0.582253f,
                    0.802850f,
                    1.000000f
                },

                // B: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.013554f,
                    0.046537f,
                    0.097923f,
                    0.195188f,
                    0.364993f,
                    0.595061f,
                    0.812193f,
                    1.000000f
                },

                // R: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.440180f,
                    0.707920f,
                    0.818012f,
                    0.883917f,
                    0.928103f,
                    0.949822f,
                    0.966298f,
                    1.000000f
                },

                // G: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.300325f,
                    0.512492f,
                    0.637010f,
                    0.754012f,
                    0.840748f,
                    0.903514f,
                    0.961203f,
                    1.000000f
                },

                // B: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.314875f,
                    0.539208f,
                    0.668957f,
                    0.776880f,
                    0.857586f,
                    0.906629f,
                    0.948801f,
                    1.000000f
                });


        // ------------------------------------------------------------
        // Human Hair Profile 1
        // ------------------------------------------------------------

        private static readonly RampCurveProfile
            HumanHairProfile1 =
            new RampCurveProfile(
                140,

                // R: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.017054f,
                    0.056779f,
                    0.118178f,
                    0.229980f,
                    0.409914f,
                    0.620129f,
                    0.822224f,
                    1.000000f
                },

                // G: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.011062f,
                    0.039136f,
                    0.085381f,
                    0.178955f,
                    0.333333f,
                    0.546977f,
                    0.765676f,
                    1.000000f
                },

                // B: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.016348f,
                    0.047507f,
                    0.098774f,
                    0.188655f,
                    0.326565f,
                    0.538630f,
                    0.750924f,
                    1.000000f
                },

                // R: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.190603f,
                    0.396631f,
                    0.554661f,
                    0.683466f,
                    0.797524f,
                    0.885942f,
                    0.956028f,
                    1.000000f
                },

                // G: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.173077f,
                    0.353256f,
                    0.502663f,
                    0.649301f,
                    0.777996f,
                    0.871352f,
                    0.953180f,
                    1.000000f
                },

                // B: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.140583f,
                    0.309797f,
                    0.459474f,
                    0.618875f,
                    0.755141f,
                    0.860922f,
                    0.948625f,
                    1.000000f
                });


        // ------------------------------------------------------------
        // Human Hair Profile 2
        // ------------------------------------------------------------

        private static readonly RampCurveProfile
            HumanHairProfile2 =
            new RampCurveProfile(
                140,

                // R: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.017766f,
                    0.049981f,
                    0.112802f,
                    0.222438f,
                    0.388998f,
                    0.594920f,
                    0.795201f,
                    1.000000f
                },

                // G: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.011086f,
                    0.041324f,
                    0.082492f,
                    0.169824f,
                    0.310034f,
                    0.493755f,
                    0.711617f,
                    1.000000f
                },

                // B: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.013766f,
                    0.041298f,
                    0.076598f,
                    0.152311f,
                    0.283972f,
                    0.472370f,
                    0.688987f,
                    1.000000f
                },

                // R: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.124199f,
                    0.297774f,
                    0.453336f,
                    0.609881f,
                    0.745061f,
                    0.854689f,
                    0.945412f,
                    1.000000f
                },

                // G: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.141544f,
                    0.316752f,
                    0.469163f,
                    0.619983f,
                    0.751821f,
                    0.861539f,
                    0.945017f,
                    1.000000f
                },

                // B: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.116523f,
                    0.268475f,
                    0.413256f,
                    0.576442f,
                    0.720943f,
                    0.847793f,
                    0.948059f,
                    1.000000f
                });


        // ------------------------------------------------------------
        // Human Hair Dark Profile
        // Gray3, Gray5, Orange3, Brown6
        // ------------------------------------------------------------

        private static readonly RampCurveProfile
            HumanHairDarkProfile =
            new RampCurveProfile(
                140,

                // R: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.012067f,
                    0.043207f,
                    0.088945f,
                    0.177890f,
                    0.304788f,
                    0.490463f,
                    0.703776f,
                    1.000000f
                },

                // G: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.000000f,
                    0.027877f,
                    0.067191f,
                    0.127949f,
                    0.229450f,
                    0.391708f,
                    0.621158f,
                    1.000000f
                },

                // B: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.000000f,
                    0.041192f,
                    0.087642f,
                    0.149869f,
                    0.258545f,
                    0.428571f,
                    0.639790f,
                    1.000000f
                },

                // R: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.105987f,
                    0.246202f,
                    0.399336f,
                    0.567703f,
                    0.719871f,
                    0.838314f,
                    0.939109f,
                    1.000000f
                },

                // G: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.090472f,
                    0.220652f,
                    0.370531f,
                    0.550907f,
                    0.706777f,
                    0.833546f,
                    0.937823f,
                    1.000000f
                },

                // B: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.092990f,
                    0.222782f,
                    0.374028f,
                    0.543492f,
                    0.704681f,
                    0.829936f,
                    0.944980f,
                    1.000000f
                });


        // ------------------------------------------------------------
        // Human Hair Cool Gray Profile
        // Gray4 is structurally different enough to retain separately.
        // ------------------------------------------------------------

        private static readonly RampCurveProfile
            HumanHairCoolGrayProfile =
            new RampCurveProfile(
                140,

                // R: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.000000f,
                    0.027778f,
                    0.069444f,
                    0.166667f,
                    0.319444f,
                    0.527778f,
                    0.750000f,
                    1.000000f
                },

                // G: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.000000f,
                    0.030303f,
                    0.075758f,
                    0.151515f,
                    0.303030f,
                    0.500000f,
                    0.727273f,
                    1.000000f
                },

                // B: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.017241f,
                    0.034483f,
                    0.086207f,
                    0.155172f,
                    0.293103f,
                    0.465517f,
                    0.689655f,
                    1.000000f
                },

                // R: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.185185f,
                    0.358025f,
                    0.530864f,
                    0.666667f,
                    0.777778f,
                    0.876543f,
                    0.962963f,
                    1.000000f
                },

                // G: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.125926f,
                    0.288889f,
                    0.444444f,
                    0.600000f,
                    0.740741f,
                    0.851852f,
                    0.940741f,
                    1.000000f
                },

                // B: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.113772f,
                    0.263473f,
                    0.419162f,
                    0.586826f,
                    0.736527f,
                    0.856287f,
                    0.952096f,
                    1.000000f
                });


        // ------------------------------------------------------------
        // Human Hair References
        // ------------------------------------------------------------

        private readonly struct HumanHairReference
        {
            public readonly RampCurveProfile Profile;

            public readonly Color Midtone;
            public readonly Color Shadow;
            public readonly Color Highlight;

            public HumanHairReference(
                RampCurveProfile profile,
                Color midtone,
                Color shadow,
                Color highlight)
            {
                Profile = profile;
                Midtone = midtone;
                Shadow = shadow;
                Highlight = highlight;
            }
        }


        private static readonly HumanHairReference[]
            HumanHairReferences =
            {
                // Brown6
                new HumanHairReference(
                    HumanHairDarkProfile,
                    new Color32(9, 7, 6, 255),
                    new Color32(0, 0, 0, 255),
                    new Color32(34, 27, 17, 255)),

                // ----------------------------------------------------
                // Cyan
                // ----------------------------------------------------

                new HumanHairReference(
                    HumanHairProfile0,
                    new Color32(139, 198, 218, 255),
                    new Color32(8, 12, 16, 255),
                    new Color32(171, 200, 220, 255)),

                new HumanHairReference(
                    HumanHairProfile1,
                    new Color32(11, 117, 168, 255),
                    new Color32(8, 12, 16, 255),
                    new Color32(123, 180, 231, 255)),

                new HumanHairReference(
                    HumanHairProfile2,
                    new Color32(2, 57, 80, 255),
                    new Color32(5, 7, 10, 255),
                    new Color32(101, 162, 216, 255)),

                // ----------------------------------------------------
                // Gray
                // ----------------------------------------------------

                new HumanHairReference(
                    HumanHairProfile2,
                    new Color32(61, 68, 73, 255),
                    new Color32(2, 3, 3, 255),
                    new Color32(231, 202, 155, 255)),

                new HumanHairReference(
                    HumanHairDarkProfile,
                    new Color32(24, 26, 27, 255),
                    new Color32(2, 3, 3, 255),
                    new Color32(166, 150, 124, 255)),

                new HumanHairReference(
                    HumanHairCoolGrayProfile,
                    new Color32(74, 69, 61, 255),
                    new Color32(2, 3, 3, 255),
                    new Color32(155, 204, 228, 255)),

                new HumanHairReference(
                    HumanHairDarkProfile,
                    new Color32(29, 28, 26, 255),
                    new Color32(2, 3, 3, 255),
                    new Color32(124, 145, 166, 255)),

                // ----------------------------------------------------
                // Green
                // ----------------------------------------------------

                new HumanHairReference(
                    HumanHairProfile0,
                    new Color32(166, 206, 131, 255),
                    new Color32(3, 4, 3, 255),
                    new Color32(224, 235, 152, 255)),

                new HumanHairReference(
                    HumanHairProfile1,
                    new Color32(103, 169, 34, 255),
                    new Color32(3, 4, 3, 255),
                    new Color32(206, 224, 95, 255)),

                new HumanHairReference(
                    HumanHairProfile2,
                    new Color32(53, 99, 12, 255),
                    new Color32(3, 4, 3, 255),
                    new Color32(206, 224, 95, 255)),

                // ----------------------------------------------------
                // Orange
                // ----------------------------------------------------

                new HumanHairReference(
                    HumanHairProfile0,
                    new Color32(222, 155, 111, 255),
                    new Color32(36, 10, 0, 255),
                    new Color32(235, 200, 137, 255)),

                new HumanHairReference(
                    HumanHairProfile1,
                    new Color32(210, 111, 42, 255),
                    new Color32(15, 5, 0, 255),
                    new Color32(233, 214, 142, 255)),

                new HumanHairReference(
                    HumanHairProfile2,
                    new Color32(184, 63, 1, 255),
                    new Color32(15, 5, 0, 255),
                    new Color32(222, 188, 74, 255)),

                new HumanHairReference(
                    HumanHairDarkProfile,
                    new Color32(67, 16, 1, 255),
                    new Color32(5, 2, 1, 255),
                    new Color32(208, 129, 30, 255)),

                // ----------------------------------------------------
                // Purple
                // ----------------------------------------------------

                new HumanHairReference(
                    HumanHairProfile0,
                    new Color32(178, 99, 175, 255),
                    new Color32(20, 4, 16, 255),
                    new Color32(206, 155, 243, 255)),

                new HumanHairReference(
                    HumanHairProfile1,
                    new Color32(147, 42, 80, 255),
                    new Color32(13, 4, 20, 255),
                    new Color32(206, 154, 243, 255)),

                new HumanHairReference(
                    HumanHairProfile2,
                    new Color32(78, 13, 33, 255),
                    new Color32(10, 3, 16, 255),
                    new Color32(186, 123, 235, 255)),

                // ----------------------------------------------------
                // Red
                // ----------------------------------------------------

                new HumanHairReference(
                    HumanHairProfile0,
                    new Color32(194, 109, 101, 255),
                    new Color32(15, 1, 0, 255),
                    new Color32(194, 171, 142, 255)),

                new HumanHairReference(
                    HumanHairProfile1,
                    new Color32(177, 42, 34, 255),
                    new Color32(15, 1, 0, 255),
                    new Color32(226, 152, 74, 255))
            };


        /// <summary>
        /// Creates an Owlcat-style human hair ramp. Human hair ranges from soft,
        /// bright gradients to deep colors with much more restrained highlights.
        /// </summary>
        public static Texture2D CreateHumanHairRamp(
            Color color,
            Color? shadow = null,
            Color? highlight = null)
        {
            HumanHairReference reference =
                GetNearestHumanHairReference(
                    color);

            return CreateRamp(
                shadow ?? reference.Shadow,
                color,
                highlight ?? reference.Highlight,
                reference.Profile);
        }


        private static HumanHairReference GetNearestHumanHairReference(
            Color color)
        {
            int bestIndex = 0;

            float bestDistance =
                float.MaxValue;

            for (int i = 0; i < HumanHairReferences.Length; i++)
            {
                float distance =
                    ColorDistanceSquared(
                        color,
                        HumanHairReferences[i].Midtone);

                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestIndex = i;
                }
            }

            return HumanHairReferences[bestIndex];
        }

        // ============================================================
        // Dhampir Hair
        // ============================================================

        // ------------------------------------------------------------
        // Dhampir Hair Deep Profile
        // Blue4 / Violet4
        // ------------------------------------------------------------

        private static readonly RampCurveProfile
            DhampirHairDeepProfile =
            new RampCurveProfile(
                140,

                // R: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.000000f,
                    0.000000f,
                    0.250000f,
                    0.333333f,
                    0.333333f,
                    0.416667f,
                    0.583333f,
                    1.000000f
                },

                // G: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.000000f,
                    0.000000f,
                    0.000000f,
                    0.000000f,
                    0.000000f,
                    0.000000f,
                    0.000000f,
                    1.000000f
                },

                // B: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.000000f,
                    0.021739f,
                    0.087596f,
                    0.175192f,
                    0.328645f,
                    0.554987f,
                    0.777494f,
                    1.000000f
                },

                // R: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.095238f,
                    0.203869f,
                    0.363095f,
                    0.535714f,
                    0.690476f,
                    0.827381f,
                    0.922619f,
                    1.000000f
                },

                // G: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.055060f,
                    0.161272f,
                    0.324870f,
                    0.488095f,
                    0.672154f,
                    0.797061f,
                    0.921131f,
                    1.000000f
                },

                // B: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.183984f,
                    0.342969f,
                    0.525000f,
                    0.645312f,
                    0.788672f,
                    0.857031f,
                    0.959375f,
                    1.000000f
                });


        // ------------------------------------------------------------
        // Dhampir Hair White Profile
        // White3
        // ------------------------------------------------------------

        private static readonly RampCurveProfile
            DhampirHairWhiteProfile =
            new RampCurveProfile(
                140,

                // R: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.006757f,
                    0.033784f,
                    0.084459f,
                    0.168919f,
                    0.317568f,
                    0.540541f,
                    0.773649f,
                    1.000000f
                },

                // G: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.009091f,
                    0.036364f,
                    0.087879f,
                    0.175758f,
                    0.330303f,
                    0.545455f,
                    0.775758f,
                    1.000000f
                },

                // B: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.010989f,
                    0.038462f,
                    0.090659f,
                    0.181319f,
                    0.337912f,
                    0.554945f,
                    0.782967f,
                    1.000000f
                },

                // R: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.253641f,
                    0.461165f,
                    0.621359f,
                    0.737864f,
                    0.825243f,
                    0.888350f,
                    0.961165f,
                    1.000000f
                },

                // G: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.306548f,
                    0.535714f,
                    0.683036f,
                    0.785714f,
                    0.857143f,
                    0.928571f,
                    0.967262f,
                    1.000000f
                },

                // B: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.399254f,
                    0.641791f,
                    0.791045f,
                    0.850746f,
                    0.910448f,
                    0.940299f,
                    0.970149f,
                    1.000000f
                });


        // ============================================================
        // Shared Bright White Hair Profiles
        // ============================================================

        // ------------------------------------------------------------
        // Bright White Hair 1
        // ------------------------------------------------------------

        private static readonly RampCurveProfile
            BrightWhiteHairProfile1 =
            new RampCurveProfile(
                140,

                // R: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.015789f,
                    0.063158f,
                    0.139474f,
                    0.252632f,
                    0.421053f,
                    0.621053f,
                    0.821053f,
                    1.000000f
                },

                // G: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.019553f,
                    0.061453f,
                    0.134078f,
                    0.251397f,
                    0.413408f,
                    0.620112f,
                    0.824022f,
                    1.000000f
                },

                // B: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.018072f,
                    0.066265f,
                    0.132530f,
                    0.240964f,
                    0.409639f,
                    0.608434f,
                    0.810241f,
                    1.000000f
                },

                // R: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.446429f,
                    0.714286f,
                    0.877551f,
                    0.918367f,
                    0.918367f,
                    0.959184f,
                    0.959184f,
                    1.000000f
                },

                // G: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.349206f,
                    0.619048f,
                    0.750000f,
                    0.841270f,
                    0.904762f,
                    0.936508f,
                    0.968254f,
                    1.000000f
                },

                // B: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.278846f,
                    0.493590f,
                    0.692308f,
                    0.769231f,
                    0.846154f,
                    0.923077f,
                    0.948718f,
                    1.000000f
                });


        // ------------------------------------------------------------
        // Bright White Hair 2
        // ------------------------------------------------------------

        private static readonly RampCurveProfile
            BrightWhiteHairProfile2 =
            new RampCurveProfile(
                140,

                // R: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.027660f,
                    0.089362f,
                    0.176596f,
                    0.314894f,
                    0.495745f,
                    0.723404f,
                    0.936170f,
                    1.000000f
                },

                // G: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.021930f,
                    0.083333f,
                    0.182018f,
                    0.315789f,
                    0.497807f,
                    0.723684f,
                    0.938596f,
                    1.000000f
                },

                // B: Shadow -> Mid
                new float[]
                {
                    0.000000f,
                    0.026549f,
                    0.084071f,
                    0.183628f,
                    0.314159f,
                    0.500000f,
                    0.721239f,
                    0.938053f,
                    1.000000f
                },

                // R: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.125000f,
                    0.250000f,
                    0.375000f,
                    0.500000f,
                    0.625000f,
                    0.750000f,
                    0.875000f,
                    1.000000f
                },

                // G: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.125000f,
                    0.250000f,
                    0.375000f,
                    0.500000f,
                    0.625000f,
                    0.750000f,
                    0.875000f,
                    1.000000f
                },

                // B: Mid -> Highlight
                new float[]
                {
                    0.000000f,
                    0.125000f,
                    0.250000f,
                    0.375000f,
                    0.500000f,
                    0.625000f,
                    0.750000f,
                    0.875000f,
                    1.000000f
                });


        // ------------------------------------------------------------
        // Dhampir Hair References
        // ------------------------------------------------------------

        private readonly struct DhampirHairReference
        {
            public readonly RampCurveProfile Profile;

            public readonly Color Midtone;
            public readonly Color Shadow;
            public readonly Color Highlight;

            public DhampirHairReference(
                RampCurveProfile profile,
                Color midtone,
                Color shadow,
                Color highlight)
            {
                Profile = profile;
                Midtone = midtone;
                Shadow = shadow;
                Highlight = highlight;
            }
        }


        private static readonly DhampirHairReference[]
            DhampirHairReferences =
            {
                // ----------------------------------------------------
                // Black
                // Exact reuse of the generic Black eye ramp.
                // ----------------------------------------------------

                new DhampirHairReference(
                    HumanEyeProfile,
                    new Color32(5, 5, 5, 255),
                    new Color32(0, 0, 0, 255),
                    new Color32(13, 16, 16, 255)),


                // ----------------------------------------------------
                // Blue
                // Blue0-2 are exact generic eye-ramp reuses.
                // ----------------------------------------------------

                new DhampirHairReference(
                    HumanEyeProfile,
                    new Color32(111, 128, 235, 255),
                    new Color32(3, 8, 13, 255),
                    new Color32(155, 206, 243, 255)),

                new DhampirHairReference(
                    HumanEyeProfile,
                    new Color32(30, 51, 188, 255),
                    new Color32(1, 3, 3, 255),
                    new Color32(99, 175, 239, 255)),

                new DhampirHairReference(
                    HumanEyeProfile,
                    new Color32(7, 27, 90, 255),
                    new Color32(1, 1, 1, 255),
                    new Color32(24, 109, 206, 255)),

                // Blue4
                new DhampirHairReference(
                    DhampirHairDeepProfile,
                    new Color32(2, 3, 24, 255),
                    new Color32(0, 1, 1, 255),
                    new Color32(14, 35, 56, 255)),


                // ----------------------------------------------------
                // Cyan
                // Cyan3 is an exact Human eye-ramp reuse.
                // ----------------------------------------------------

                new DhampirHairReference(
                    HumanEyeProfile,
                    new Color32(1, 20, 32, 255),
                    new Color32(1, 2, 2, 255),
                    new Color32(26, 59, 85, 255)),


                // ----------------------------------------------------
                // Gray
                // These are exact Human hair-ramp reuses.
                // ----------------------------------------------------

                // Gray2
                new DhampirHairReference(
                    HumanHairProfile2,
                    new Color32(61, 68, 73, 255),
                    new Color32(2, 3, 3, 255),
                    new Color32(231, 202, 155, 255)),

                // Gray3
                new DhampirHairReference(
                    HumanHairDarkProfile,
                    new Color32(24, 26, 27, 255),
                    new Color32(2, 3, 3, 255),
                    new Color32(166, 150, 124, 255)),

                // Gray4
                new DhampirHairReference(
                    HumanHairCoolGrayProfile,
                    new Color32(74, 69, 61, 255),
                    new Color32(2, 3, 3, 255),
                    new Color32(155, 204, 228, 255)),

                // Gray5
                new DhampirHairReference(
                    HumanHairDarkProfile,
                    new Color32(29, 28, 26, 255),
                    new Color32(2, 3, 3, 255),
                    new Color32(124, 145, 166, 255)),


                // ----------------------------------------------------
                // Purple
                // Exact Human hair-ramp reuse.
                // ----------------------------------------------------

                new DhampirHairReference(
                    HumanHairProfile2,
                    new Color32(78, 13, 33, 255),
                    new Color32(10, 3, 16, 255),
                    new Color32(186, 123, 235, 255)),


                // ----------------------------------------------------
                // Red
                // ----------------------------------------------------

                // Red1 - exact Human hair-ramp reuse.
                new DhampirHairReference(
                    HumanHairProfile1,
                    new Color32(177, 42, 34, 255),
                    new Color32(15, 1, 0, 255),
                    new Color32(226, 152, 74, 255)),

                // Red2 - exact reuse of a Dhampir eye ramp.
                new DhampirHairReference(
                    DhampirEyeProfile,
                    new Color32(140, 15, 10, 255),
                    new Color32(5, 4, 3, 255),
                    new Color32(208, 92, 45, 255)),

                // Red3 - exact reuse of a Dhampir eye ramp.
                new DhampirHairReference(
                    DhampirEyeProfile,
                    new Color32(42, 5, 4, 255),
                    new Color32(2, 1, 1, 255),
                    new Color32(150, 73, 58, 255)),


                // ----------------------------------------------------
                // Violet
                // ----------------------------------------------------

                // Violet2 - exact reuse of a Dhampir eye ramp.
                new DhampirHairReference(
                    DhampirEyeProfile,
                    new Color32(41, 21, 66, 255),
                    new Color32(7, 2, 5, 255),
                    new Color32(196, 144, 178, 255)),

                // Violet3 - exact reuse of a Dhampir eye ramp.
                new DhampirHairReference(
                    DhampirEyeProfile,
                    new Color32(20, 6, 43, 255),
                    new Color32(4, 1, 3, 255),
                    new Color32(82, 53, 72, 255)),

                // Violet4
                new DhampirHairReference(
                    DhampirHairDeepProfile,
                    new Color32(7, 1, 18, 255),
                    new Color32(1, 0, 1, 255),
                    new Color32(35, 22, 38, 255)),


                // ----------------------------------------------------
                // White
                // ----------------------------------------------------

                // White3
                new DhampirHairReference(
                    DhampirHairWhiteProfile,
                    new Color32(152, 171, 188, 255),
                    new Color32(4, 6, 6, 255),
                    new Color32(255, 255, 255, 255)),

                // BrightWhite1_U_Any
                new DhampirHairReference(
                    BrightWhiteHairProfile1,
                    new Color32(206, 192, 177, 255),
                    new Color32(16, 13, 11, 255),
                    new Color32(255, 255, 255, 255)),

                // BrightWhite2_U_Any
                new DhampirHairReference(
                    BrightWhiteHairProfile2,
                    new Color32(255, 255, 255, 255),
                    new Color32(20, 27, 29, 255),
                    new Color32(255, 255, 255, 255))
            };


        /// <summary>
        /// Creates an Owlcat-style Dhampir hair ramp. Dark shades remain heavily
        /// compressed while colored shades often transition toward vivid highlights.
        /// </summary>
        public static Texture2D CreateDhampirHairRamp(
            Color color,
            Color? shadow = null,
            Color? highlight = null)
        {
            DhampirHairReference reference =
                GetNearestDhampirHairReference(
                    color);

            return CreateRamp(
                shadow ?? reference.Shadow,
                color,
                highlight ?? reference.Highlight,
                reference.Profile);
        }

        private static DhampirHairReference GetNearestDhampirHairReference(
            Color color)
        {
            int bestIndex = 0;

            float bestDistance =
                float.MaxValue;

            for (int i = 0; i < DhampirHairReferences.Length; i++)
            {
                float distance =
                    ColorDistanceSquared(
                        color,
                        DhampirHairReferences[i].Midtone);

                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestIndex = i;
                }
            }

            return DhampirHairReferences[bestIndex];
        }

        // ============================================================
        // Oread Hair
        // ============================================================

        private readonly struct OreadHairReference
        {
            public readonly RampCurveProfile Profile;

            public readonly Color Midtone;
            public readonly Color Shadow;
            public readonly Color Highlight;

            public OreadHairReference(
                RampCurveProfile profile,
                Color midtone,
                Color shadow,
                Color highlight)
            {
                Profile = profile;
                Midtone = midtone;
                Shadow = shadow;
                Highlight = highlight;
            }
        }


        private static readonly OreadHairReference[]
            OreadHairReferences =
            {
                // ----------------------------------------------------
                // Black
                // Exact reuse of the generic Human eye Black ramp.
                // ----------------------------------------------------

                new OreadHairReference(
                    HumanEyeProfile,
                    new Color32(5, 5, 5, 255),
                    new Color32(0, 0, 0, 255),
                    new Color32(13, 16, 16, 255)),


                // ----------------------------------------------------
                // Blue
                // All three are exact Human eye-ramp reuses.
                // ----------------------------------------------------

                // Blue0
                new OreadHairReference(
                    HumanEyeProfile,
                    new Color32(111, 128, 235, 255),
                    new Color32(3, 8, 13, 255),
                    new Color32(155, 206, 243, 255)),

                // Blue1
                new OreadHairReference(
                    HumanEyeProfile,
                    new Color32(30, 51, 188, 255),
                    new Color32(1, 3, 3, 255),
                    new Color32(99, 175, 239, 255)),

                // Blue2
                new OreadHairReference(
                    HumanEyeProfile,
                    new Color32(7, 27, 90, 255),
                    new Color32(1, 1, 1, 255),
                    new Color32(24, 109, 206, 255)),


                // ----------------------------------------------------
                // Brown
                // All three are exact Human eye-ramp reuses.
                // ----------------------------------------------------

                // Brown0
                new OreadHairReference(
                    HumanEyeProfile,
                    new Color32(160, 118, 77, 255),
                    new Color32(5, 3, 2, 255),
                    new Color32(255, 218, 160, 255)),

                // Brown1
                new OreadHairReference(
                    HumanEyeProfile,
                    new Color32(91, 63, 38, 255),
                    new Color32(5, 3, 2, 255),
                    new Color32(198, 166, 118, 255)),

                // Brown2
                new OreadHairReference(
                    HumanEyeProfile,
                    new Color32(39, 21, 10, 255),
                    new Color32(5, 3, 2, 255),
                    new Color32(126, 85, 35, 255)),


                // ----------------------------------------------------
                // Cyan
                // ----------------------------------------------------

                // Cyan0 - exact Oread Eye Cyan0 reuse.
                new OreadHairReference(
                    OreadEye0Profile,
                    new Color32(139, 198, 218, 255),
                    new Color32(8, 12, 16, 255),
                    new Color32(171, 200, 220, 255)),

                // Cyan1 - exact Human/Oread eye reuse.
                new OreadHairReference(
                    HumanEyeProfile,
                    new Color32(11, 117, 168, 255),
                    new Color32(8, 12, 16, 255),
                    new Color32(123, 180, 231, 255)),

                // Cyan2 - exact Human Hair Cyan2 reuse.
                new OreadHairReference(
                    HumanHairProfile2,
                    new Color32(2, 57, 80, 255),
                    new Color32(5, 7, 10, 255),
                    new Color32(101, 162, 216, 255)),


                // ----------------------------------------------------
                // Gray
                // ----------------------------------------------------

                // Gray2 - exact Human Hair Gray2 reuse.
                new OreadHairReference(
                    HumanHairProfile2,
                    new Color32(61, 68, 73, 255),
                    new Color32(2, 3, 3, 255),
                    new Color32(231, 202, 155, 255)),


                // ----------------------------------------------------
                // Green
                // ----------------------------------------------------

                // Green0 - exact Oread Eye Green0 reuse.
                new OreadHairReference(
                    OreadEye0Profile,
                    new Color32(166, 206, 131, 255),
                    new Color32(3, 4, 3, 255),
                    new Color32(224, 235, 152, 255)),

                // Green1 - exact Human/Oread eye reuse.
                new OreadHairReference(
                    HumanEyeProfile,
                    new Color32(103, 169, 34, 255),
                    new Color32(3, 4, 3, 255),
                    new Color32(206, 224, 95, 255)),

                // Green2 - exact Human Hair Green2 reuse.
                new OreadHairReference(
                    HumanHairProfile2,
                    new Color32(53, 99, 12, 255),
                    new Color32(3, 4, 3, 255),
                    new Color32(206, 224, 95, 255)),


                // ----------------------------------------------------
                // Purple
                // ----------------------------------------------------

                // Purple0 - exact Oread Eye Purple0 reuse.
                new OreadHairReference(
                    OreadEye0Profile,
                    new Color32(178, 99, 175, 255),
                    new Color32(20, 4, 16, 255),
                    new Color32(206, 155, 243, 255)),

                // Purple1 - exact Oread Eye Purple1 reuse.
                new OreadHairReference(
                    OreadEye1Profile,
                    new Color32(147, 42, 80, 255),
                    new Color32(13, 4, 20, 255),
                    new Color32(206, 154, 243, 255)),

                // Purple2 - exact Human Hair Purple2 reuse.
                new OreadHairReference(
                    HumanHairProfile2,
                    new Color32(78, 13, 33, 255),
                    new Color32(10, 3, 16, 255),
                    new Color32(186, 123, 235, 255)),


                // ----------------------------------------------------
                // Red
                // ----------------------------------------------------

                // Red0 - exact Oread Eye Red0 reuse.
                new OreadHairReference(
                    OreadEye0Profile,
                    new Color32(194, 109, 101, 255),
                    new Color32(15, 1, 0, 255),
                    new Color32(194, 171, 142, 255)),

                // Red1 - exact Oread Eye Red1 reuse.
                new OreadHairReference(
                    OreadEye1Profile,
                    new Color32(177, 42, 34, 255),
                    new Color32(15, 1, 0, 255),
                    new Color32(226, 152, 74, 255)),

                // Red2 - exact Dhampir Eye Red2 reuse.
                new OreadHairReference(
                    DhampirEyeProfile,
                    new Color32(140, 15, 10, 255),
                    new Color32(5, 4, 3, 255),
                    new Color32(208, 92, 45, 255))
            };


        /// <summary>
        /// Creates an Owlcat-style Oread hair ramp. Oread hair reuses several
        /// existing ramp families, ranging from bright gradients to deep saturated ones.
        /// </summary>
        public static Texture2D CreateOreadHairRamp(
            Color color,
            Color? shadow = null,
            Color? highlight = null)
        {
            OreadHairReference reference =
                GetNearestOreadHairReference(
                    color);

            return CreateRamp(
                shadow ?? reference.Shadow,
                color,
                highlight ?? reference.Highlight,
                reference.Profile);
        }


        private static OreadHairReference GetNearestOreadHairReference(
            Color color)
        {
            int bestIndex = 0;

            float bestDistance =
                float.MaxValue;

            for (int i = 0; i < OreadHairReferences.Length; i++)
            {
                float distance =
                    ColorDistanceSquared(
                        color,
                        OreadHairReferences[i].Midtone);

                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestIndex = i;
                }
            }

            return OreadHairReferences[bestIndex];
        }

        // ============================================================
        // Tiefling Hair
        // ============================================================

        private readonly struct TieflingHairReference
        {
            public readonly RampCurveProfile Profile;

            public readonly Color Midtone;
            public readonly Color Shadow;
            public readonly Color Highlight;

            public TieflingHairReference(
                RampCurveProfile profile,
                Color midtone,
                Color shadow,
                Color highlight)
            {
                Profile = profile;
                Midtone = midtone;
                Shadow = shadow;
                Highlight = highlight;
            }
        }


        private static readonly TieflingHairReference[]
            TieflingHairReferences =
            {
                // ----------------------------------------------------
                // Black
                // ----------------------------------------------------

                new TieflingHairReference(
                    HumanEyeProfile,
                    new Color32(5, 5, 5, 255),
                    new Color32(0, 0, 0, 255),
                    new Color32(13, 16, 16, 255)),

                // ----------------------------------------------------
                // Blue
                // ----------------------------------------------------

                new TieflingHairReference(
                    HumanEyeProfile,
                    new Color32(111, 128, 235, 255),
                    new Color32(3, 8, 13, 255),
                    new Color32(155, 206, 243, 255)),

                new TieflingHairReference(
                    HumanEyeProfile,
                    new Color32(30, 51, 188, 255),
                    new Color32(1, 3, 3, 255),
                    new Color32(99, 175, 239, 255)),

                new TieflingHairReference(
                    HumanEyeProfile,
                    new Color32(7, 27, 90, 255),
                    new Color32(1, 1, 1, 255),
                    new Color32(24, 109, 206, 255)),

                // ----------------------------------------------------
                // Brown
                // ----------------------------------------------------

                new TieflingHairReference(
                    HumanEyeProfile,
                    new Color32(160, 118, 77, 255),
                    new Color32(5, 3, 2, 255),
                    new Color32(255, 218, 160, 255)),

                new TieflingHairReference(
                    HumanEyeProfile,
                    new Color32(91, 63, 38, 255),
                    new Color32(5, 3, 2, 255),
                    new Color32(198, 166, 118, 255)),

                new TieflingHairReference(
                    HumanEyeProfile,
                    new Color32(39, 21, 10, 255),
                    new Color32(5, 3, 2, 255),
                    new Color32(126, 85, 35, 255)),

                // ----------------------------------------------------
                // Green
                // ----------------------------------------------------

                new TieflingHairReference(
                    OreadEye0Profile,
                    new Color32(166, 206, 131, 255),
                    new Color32(3, 4, 3, 255),
                    new Color32(224, 235, 152, 255)),

                new TieflingHairReference(
                    HumanEyeProfile,
                    new Color32(103, 169, 34, 255),
                    new Color32(3, 4, 3, 255),
                    new Color32(206, 224, 95, 255)),

                new TieflingHairReference(
                    HumanHairProfile2,
                    new Color32(53, 99, 12, 255),
                    new Color32(3, 4, 3, 255),
                    new Color32(206, 224, 95, 255)),

                // ----------------------------------------------------
                // Dark / muted neutrals
                // ----------------------------------------------------

                new TieflingHairReference(
                    HumanHairProfile2,
                    new Color32(61, 68, 73, 255),
                    new Color32(2, 3, 3, 255),
                    new Color32(231, 202, 155, 255)),

                new TieflingHairReference(
                    HumanHairDarkProfile,
                    new Color32(9, 7, 6, 255),
                    new Color32(0, 0, 0, 255),
                    new Color32(34, 27, 17, 255)),

                // ----------------------------------------------------
                // Yellow / gold
                // ----------------------------------------------------

                new TieflingHairReference(
                    TieflingEyeProfile,
                    new Color32(130, 124, 24, 255),
                    new Color32(5, 4, 1, 255),
                    new Color32(225, 233, 94, 255)),

                new TieflingHairReference(
                    TieflingEyeProfile,
                    new Color32(168, 126, 28, 255),
                    new Color32(8, 6, 1, 255),
                    new Color32(245, 223, 108, 255)),

                // ----------------------------------------------------
                // Orange
                // ----------------------------------------------------

                new TieflingHairReference(
                    HumanHairProfile1,
                    new Color32(210, 111, 42, 255),
                    new Color32(15, 5, 0, 255),
                    new Color32(233, 214, 142, 255)),

                new TieflingHairReference(
                    HumanHairProfile2,
                    new Color32(184, 63, 1, 255),
                    new Color32(15, 5, 0, 255),
                    new Color32(222, 188, 74, 255)),

                // ----------------------------------------------------
                // Purple / magenta
                // ----------------------------------------------------

                new TieflingHairReference(
                    HumanHairProfile0,
                    new Color32(178, 99, 175, 255),
                    new Color32(20, 4, 16, 255),
                    new Color32(206, 155, 243, 255)),

                new TieflingHairReference(
                    HumanHairProfile1,
                    new Color32(147, 42, 80, 255),
                    new Color32(13, 4, 20, 255),
                    new Color32(206, 154, 243, 255)),

                new TieflingHairReference(
                    HumanHairProfile2,
                    new Color32(78, 13, 33, 255),
                    new Color32(10, 3, 16, 255),
                    new Color32(186, 123, 235, 255)),

                // ----------------------------------------------------
                // Red
                // ----------------------------------------------------

                new TieflingHairReference(
                    HumanHairProfile0,
                    new Color32(194, 109, 101, 255),
                    new Color32(15, 1, 0, 255),
                    new Color32(194, 171, 142, 255)),

                new TieflingHairReference(
                    HumanHairProfile1,
                    new Color32(177, 42, 34, 255),
                    new Color32(15, 1, 0, 255),
                    new Color32(226, 152, 74, 255))
            };


        /// <summary>
        /// Creates an Owlcat-style Tiefling hair ramp.
        /// </summary>
        public static Texture2D CreateTieflingHairRamp(
            Color color,
            Color? shadow = null,
            Color? highlight = null)
        {
            TieflingHairReference reference =
                GetNearestTieflingHairReference(
                    color);

            return CreateRamp(
                shadow ?? reference.Shadow,
                color,
                highlight ?? reference.Highlight,
                reference.Profile);
        }


        private static TieflingHairReference GetNearestTieflingHairReference(
            Color color)
        {
            int bestIndex = 0;

            float bestDistance =
                float.MaxValue;

            for (int i = 0; i < TieflingHairReferences.Length; i++)
            {
                float distance =
                    ColorDistanceSquared(
                        color,
                        TieflingHairReferences[i].Midtone);

                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestIndex = i;
                }
            }

            return TieflingHairReferences[bestIndex];
        }


    }
}