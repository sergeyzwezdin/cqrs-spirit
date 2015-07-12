using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace CQSNET.Exceptions
{
    /// <summary>
    /// Domain exception
    /// </summary>
    public abstract class DomainException : Exception
    {
        /// <summary>
        /// Exception reason
        /// </summary>
        public abstract string ReasonName
        {
            get;
        }
    }

    /// <summary>
    /// Domain exception
    /// </summary>
    /// <typeparam name="TReason">Reason type</typeparam>
    public class DomainException<TReason> : DomainException
        where TReason : struct, IConvertible
    {
        private readonly TReason _reason;

        /// <summary>
        /// Constructs domain exception with some reason
        /// </summary>
        /// <param name="reason"></param>
        public DomainException(TReason reason)
        {
            if (typeof(TReason).IsEnum == false)
                throw new ArgumentException("The reason's type must be an enum.");

            _reason = reason;
        }

        /// <summary>
        /// Exception reason
        /// </summary>
        public TReason Reason
        {
            get { return _reason; }
        }

        /// <summary>
        /// Exception reason
        /// </summary>
        public override string ReasonName
        {
            get
            {
                return _reason.ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Exception message
        /// </summary>
        public override string Message
        {
            get
            {
                return ExtractDecriptionAttribute(_reason, _reason.ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Text representation of exception
        /// </summary>
        /// <returns></returns>
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
