using System;
using System.Drawing;

namespace AppointmentBook
{
	public static class ColorHelper
	{
		public static Color ModifyColor(Color original, float brightnessFactor)
		{
			return FromAHSB(original.A, original.GetHue(), original.GetSaturation(), Math.Min(1, original.GetBrightness() * brightnessFactor));
		}

		public static Color FromAHSB(int alpha, float hue, float saturation, float brightness)
		{
			if (0 > alpha || 255 < alpha)
				throw new ArgumentOutOfRangeException( "alpha", alpha, "Value must be within a range of 0 - 255.");

			if (0f > hue || 360f < hue)
				throw new ArgumentOutOfRangeException("hue", hue, "Value must be within a range of 0 - 360.");

			if (0f > saturation || 1f < saturation)
				throw new ArgumentOutOfRangeException("saturation", saturation, "Value must be within a range of 0 - 1.");

			if (0f > brightness || 1f < brightness)
				throw new ArgumentOutOfRangeException("brightness", brightness, "Value must be within a range of 0 - 1.");

			if (Math.Abs(0 - saturation) < 0.0001)
			{
				return Color.FromArgb(
									alpha,
									Convert.ToInt32(brightness * 255),
									Convert.ToInt32(brightness * 255),
									Convert.ToInt32(brightness * 255));
			}

			float fMax, fMid, fMin;

			if (0.5 < brightness)
			{
				fMax = brightness - (brightness * saturation) + saturation;
				fMin = brightness + (brightness * saturation) - saturation;
			}
			else
			{
				fMax = brightness + (brightness * saturation);
				fMin = brightness - (brightness * saturation);
			}

			var iSextant = (int)Math.Floor(hue / 60f);
			if (300f <= hue)
			{
				hue -= 360f;
			}

			hue /= 60f;
			hue -= 2f * (float)Math.Floor(((iSextant + 1f) % 6f) / 2f);
			if (0 == iSextant % 2)
			{
				fMid = (hue * (fMax - fMin)) + fMin;
			}
			else
			{
				fMid = fMin - (hue * (fMax - fMin));
			}

			int iMax = Convert.ToInt32(fMax * 255);
			int iMid = Convert.ToInt32(fMid * 255);
			int iMin = Convert.ToInt32(fMin * 255);

			switch (iSextant)
			{
				case 1:
					return Color.FromArgb(alpha, iMid, iMax, iMin);
				case 2:
					return Color.FromArgb(alpha, iMin, iMax, iMid);
				case 3:
					return Color.FromArgb(alpha, iMin, iMid, iMax);
				case 4:
					return Color.FromArgb(alpha, iMid, iMin, iMax);
				case 5:
					return Color.FromArgb(alpha, iMax, iMin, iMid);
				default:
					return Color.FromArgb(alpha, iMax, iMid, iMin);
			}
		}
	}
}
