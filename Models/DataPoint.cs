using System.Runtime.Serialization;

namespace Study_Tracker.Models
{
	[DataContract]
	public class DataPoint
	{
		public DataPoint(DateTime x, double y)
		{
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			TimeSpan diff = x.ToUniversalTime() - origin;

            this.X = diff.TotalMilliseconds;
			this.Y = y;
		}

		// JSON.
		[DataMember(Name = "x")]
		public Nullable<double> X = null;

		// JSON.
		[DataMember(Name = "y")]
		public Nullable<double> Y = null;
	}
}
