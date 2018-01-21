using System;

namespace AssemblyCSharp
{
	public static class Guassian
	{
		private static Random rand = new Random(); //reuse this if you are generating many

		public static float getGaussian(float mean, float stdDev){
			double u1 = 1.0-rand.NextDouble(); //uniform(0,1] random doubles
			double u2 = 1.0-rand.NextDouble();
			double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
				Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
			double randNormal =
				(double)mean + (double)stdDev * randStdNormal; //random normal(mean,stdDev^2)
			return (float)randNormal;
		}

		public static float getGaussian(){
			return getGaussian (0, 1);
		}

	}
}

