using UnityEngine;

namespace Scripts.Extensions
{
    public static class ColorExtensions
    {
    	public static Color MultiplyAlpha(this Color color, float mul) => new(color.r, color.g, color.b, color.a * mul);
    	public static Color SetAlpha(this Color color, float alpha) => new(color.r, color.g, color.b, alpha);
    	public static Color MupliplyRGB(this Color color, float mul) => new(color.r * mul, color.g * mul, color.b * mul, color.a);
		public static Color AdjustBrightness(this Color color, float brightness)
		{
			var (h, s, l) = RGBToHSL(color);
			l += brightness;
			return HSLToRGB(h, s, l);
		}
		public static Color AdjustSaturation(this Color color, float saturation)
		{
			var (h, s, l) = RGBToHSL(color);
			l += saturation;
			return HSLToRGB(h, s, l);
		}
		public static Color AdjustSL(this Color color, float saturation, float brightness)
		{
			var (h, s, l) = RGBToHSL(color);
			s += saturation;
			l += brightness;
			return HSLToRGB(h, s, l);
		}

		public static (float h, float s, float l) RGBToHSL(Color color)
		{
			float r = color.r;
			float g = color.g;
			float b = color.b;

			float max = Mathf.Max(r, Mathf.Max(g, b));
			float min = Mathf.Min(r, Mathf.Min(g, b));

			float l = (max + min) * 0.5f;

			float h = 0f;
			float s = 0f;
			if (!Mathf.Approximately(max, min))
			{
				float d = max - min;
				s = l > 0.5f ? d / (2f - max - min) : d / (max + min);

				if (Mathf.Approximately(max, r))
					h = (g - b) / d + (g < b ? 6f : 0f);
				else if (Mathf.Approximately(max, g))
					h = (b - r) / d + 2f;
				else
					h = (r - g) / d + 4f;

				h /= 6f;
			}
			return (h, s, l);
		}

		public static Color HSLToRGB(float h, float s, float l)
		{
			float r, g, b;

			if (Mathf.Approximately(s, 0f))
			{
				r = g = b = l; // Achromatic
			}
			else
			{
				float q = l < 0.5f ? l * (1f + s) : l + s - l * s;
				float p = 2f * l - q;
				r = HueToRGB(p, q, h + 1f / 3f);
				g = HueToRGB(p, q, h);
				b = HueToRGB(p, q, h - 1f / 3f);
			}

			return new Color(r, g, b, 1f);

			float HueToRGB(float p, float q, float t)
			{
				if (t < 0f) t += 1f;
				if (t > 1f) t -= 1f;
				if (t < 1f / 6f) return p + (q - p) * 6f * t;
				if (t < 1f / 2f) return q;
				if (t < 2f / 3f) return p + (q - p) * (2f / 3f - t) * 6f;
				return p;
			}
		}

	}
}
