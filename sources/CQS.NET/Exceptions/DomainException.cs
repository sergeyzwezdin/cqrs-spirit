using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace CQS.NET
{
	public abstract class DomainException : Exception
	{
		public abstract string ReasonName
		{
			get;
		}
	}

	public class DomainException<TReason> : DomainException
		where TReason : struct, IConvertible
	{
		private readonly TReason _reason;

		public DomainException(TReason reason)
		{
			if (typeof(TReason).IsEnum == false)
				throw new ArgumentException("The reason's type must be an enum.");

			_reason = reason;
		}

		public TReason Reason
		{
			get { return _reason; }
		}

		public override string ReasonName
		{
			get
			{
				return _reason.ToString(CultureInfo.InvariantCulture);
			}
		}

		public override string Message
		{
			get
			{
				return ExtractDecriptionAttribute(_reason, _reason.ToString(CultureInfo.InvariantCulture));
			}
		}

		public override string ToString()
		{
			if (String.IsNullOrWhiteSpace(_reason.ToString(CultureInfo.InvariantCulture)) == false)
				return Reason.ToString(CultureInfo.InvariantCulture) + ": " + Message;
			else
				return Reason.ToString(CultureInfo.InvariantCulture);
		}

		private static string ExtractDecriptionAttribute(TReason reason, string defaultValue)
		{
			string result = defaultValue;

			MemberInfo member = typeof(TReason)
				.GetMember(reason.ToString(CultureInfo.InvariantCulture))
				.FirstOrDefault();

			if (member != null)
			{
				DescriptionAttribute attribute = Attribute.GetCustomAttribute(member, typeof(DescriptionAttribute)) as DescriptionAttribute;

				if (attribute != null)
					result = attribute.Description;
			}

			return result;
		}
	}
}
