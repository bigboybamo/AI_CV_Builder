namespace NewAI_CV_builder.Utilities
{
    /// <summary>
    /// The colours the resume stylesheet takes from the accent, each as <c>#rrggbb</c>.
    /// </summary>
    public readonly record struct ResumePalette(string Accent, string TagBackground, string TagText);

    /// <summary>
    /// Validates the user's accent colour for the resume PDF and works out the colours
    /// that follow from it. Every colour that reaches the stylesheet passes through
    /// <see cref="Normalize"/>, so only a plain hex value can end up in the CSS.
    /// </summary>
    public static class ResumeAccentColor
    {
        public const string DefaultHex = "#1f6feb";

        // The default tags are not an exact tint and shade of the default accent, so they
        // are kept as literals: with no colour picked the stylesheet is unchanged.
        private const string DefaultTagBackground = "#eef4ff";
        private const string DefaultTagText = "#1f4e8c";

        // Lightness of the default tag colours, reused so derived tags stay a pale tint
        // with dark text whatever the hue.
        private const double TagBackgroundLightness = 0.967;
        private const double MaxTagTextLightness = 0.335;

        private const double MinContrastWithWhite = 3.0;

        /// <summary>
        /// Returns <paramref name="value"/> as lowercase <c>#rrggbb</c>. Blank means the default
        /// accent; <c>#rgb</c> and a missing <c>#</c> are accepted. Anything else throws.
        /// </summary>
        public static string Normalize(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return DefaultHex;

            var hex = value.Trim();
            if (hex.StartsWith('#'))
                hex = hex[1..];

            if (hex.Length == 3 && hex.All(char.IsAsciiHexDigit))
                hex = string.Concat(hex.Select(c => new string(c, 2)));

            if (hex.Length != 6 || !hex.All(char.IsAsciiHexDigit))
                throw new ArgumentException("Accent colour must be a hex value such as #1f6feb.", nameof(value));

            return "#" + hex.ToLowerInvariant();
        }

        public static string ToHex(Color color) => $"#{color.R:x2}{color.G:x2}{color.B:x2}";

        public static Color ToColor(string? hex)
        {
            var (r, g, b) = Rgb(Normalize(hex));
            return Color.FromArgb(r, g, b);
        }

        /// <summary>WCAG contrast ratio of the colour against a white page (1 to 21).</summary>
        public static double ContrastWithWhite(string? hex)
        {
            var (r, g, b) = Rgb(Normalize(hex));
            var luminance = 0.2126 * Linear(r) + 0.7152 * Linear(g) + 0.0722 * Linear(b);
            return 1.05 / (luminance + 0.05);
        }

        /// <summary>True when headers and links in this colour would be faint on a white page.</summary>
        public static bool IsHardToReadOnWhite(string? hex) => ContrastWithWhite(hex) < MinContrastWithWhite;

        /// <summary>
        /// Builds the stylesheet colours for an accent. The default accent keeps its original
        /// tag colours; any other accent gets a pale tint and a dark shade of its own hue.
        /// </summary>
        public static ResumePalette PaletteFor(string? accent)
        {
            var normalized = Normalize(accent);
            if (normalized == DefaultHex)
                return new ResumePalette(DefaultHex, DefaultTagBackground, DefaultTagText);

            var (hue, saturation, lightness) = Hsl(normalized);
            return new ResumePalette(
                normalized,
                FromHsl(hue, saturation, TagBackgroundLightness),
                FromHsl(hue, saturation, Math.Min(lightness, MaxTagTextLightness)));
        }

        private static (int R, int G, int B) Rgb(string normalizedHex) => (
            Convert.ToInt32(normalizedHex.Substring(1, 2), 16),
            Convert.ToInt32(normalizedHex.Substring(3, 2), 16),
            Convert.ToInt32(normalizedHex.Substring(5, 2), 16));

        private static double Linear(int channel)
        {
            var c = channel / 255.0;
            return c <= 0.04045 ? c / 12.92 : Math.Pow((c + 0.055) / 1.055, 2.4);
        }

        private static (double Hue, double Saturation, double Lightness) Hsl(string normalizedHex)
        {
            var (red, green, blue) = Rgb(normalizedHex);
            double r = red / 255.0, g = green / 255.0, b = blue / 255.0;

            var max = Math.Max(r, Math.Max(g, b));
            var min = Math.Min(r, Math.Min(g, b));
            var lightness = (max + min) / 2;
            var delta = max - min;

            if (delta == 0)
                return (0, 0, lightness);

            var saturation = delta / (1 - Math.Abs(2 * lightness - 1));

            double hue;
            if (max == r)
                hue = ((g - b) / delta + 6) % 6;
            else if (max == g)
                hue = (b - r) / delta + 2;
            else
                hue = (r - g) / delta + 4;

            return (hue * 60, saturation, lightness);
        }

        private static string FromHsl(double hue, double saturation, double lightness)
        {
            var chroma = (1 - Math.Abs(2 * lightness - 1)) * saturation;
            var sector = hue / 60;
            var x = chroma * (1 - Math.Abs(sector % 2 - 1));

            var (r, g, b) = sector switch
            {
                < 1 => (chroma, x, 0.0),
                < 2 => (x, chroma, 0.0),
                < 3 => (0.0, chroma, x),
                < 4 => (0.0, x, chroma),
                < 5 => (x, 0.0, chroma),
                _ => (chroma, 0.0, x)
            };

            var m = lightness - chroma / 2;
            return $"#{Channel(r + m):x2}{Channel(g + m):x2}{Channel(b + m):x2}";
        }

        private static int Channel(double value) => Math.Clamp((int)Math.Round(value * 255), 0, 255);
    }
}
