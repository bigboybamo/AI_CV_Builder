using NewAI_CV_builder.Utilities;

namespace NewAI_CV_builder.Tests;

public class ResumeAccentColorTests
{
    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    public void Normalize_WhenValueIsBlank_ReturnsDefaultBlue(string? value)
    {
        //Act
        var result = ResumeAccentColor.Normalize(value);

        //Assert
        Assert.That(result, Is.EqualTo("#1f6feb"));
    }

    [TestCase("#1F6FEB", "#1f6feb")]
    [TestCase("1f6feb", "#1f6feb")]
    [TestCase("  #B91C1C  ", "#b91c1c")]
    [TestCase("#ABC", "#aabbcc")]
    [TestCase("abc", "#aabbcc")]
    public void Normalize_WhenValueIsHex_ReturnsLowercaseSixDigitHex(string value, string expected)
    {
        //Act
        var result = ResumeAccentColor.Normalize(value);

        //Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase("red")]
    [TestCase("#12345")]
    [TestCase("#1f6febff")]
    [TestCase("#gggggg")]
    [TestCase("#1f6feb; } body { display: none")]
    [TestCase("rgb(31, 111, 235)")]
    public void Normalize_WhenValueIsNotHex_ThrowsArgumentException(string value)
    {
        //Act + Assert
        Assert.Throws<ArgumentException>(() => ResumeAccentColor.Normalize(value));
    }

    [Test]
    public void PaletteFor_WhenAccentIsDefault_ReturnsOriginalTagColours()
    {
        //Act
        var palette = ResumeAccentColor.PaletteFor(null);

        //Assert
        Assert.That(palette, Is.EqualTo(new ResumePalette("#1f6feb", "#eef4ff", "#1f4e8c")));
    }

    [TestCase("#b91c1c")]
    [TestCase("#16a34a")]
    [TestCase("#ffe066")]
    [TestCase("#808080")]
    public void PaletteFor_WhenAccentIsCustom_DerivesPaleBackgroundAndReadableText(string accent)
    {
        //Act
        var palette = ResumeAccentColor.PaletteFor(accent);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(palette.Accent, Is.EqualTo(accent));
            Assert.That(ResumeAccentColor.ContrastWithWhite(palette.TagBackground), Is.LessThan(1.25));
            Assert.That(ResumeAccentColor.IsHardToReadOnWhite(palette.TagText), Is.False);
        });
    }

    [Test]
    public void PaletteFor_WhenAccentIsRed_KeepsTheHueInBothTagColours()
    {
        //Act
        var palette = ResumeAccentColor.PaletteFor("#ff0000");

        //Assert
        Assert.That(palette, Is.EqualTo(new ResumePalette("#ff0000", "#ffeeee", "#ab0000")));
    }

    [TestCase("#1f6feb", false)]
    [TestCase("#000000", false)]
    [TestCase("#b91c1c", false)]
    [TestCase("#ffffff", true)]
    [TestCase("#ffe066", true)]
    [TestCase("#d1d5db", true)]
    public void IsHardToReadOnWhite_FlagsOnlyVeryLightColours(string hex, bool expected)
    {
        //Act
        var result = ResumeAccentColor.IsHardToReadOnWhite(hex);

        //Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void ToHex_AndToColor_RoundTrip()
    {
        //Arrange
        var color = ResumeAccentColor.ToColor("#B91C1C");

        //Act
        var hex = ResumeAccentColor.ToHex(color);

        //Assert
        Assert.That(hex, Is.EqualTo("#b91c1c"));
    }
}
