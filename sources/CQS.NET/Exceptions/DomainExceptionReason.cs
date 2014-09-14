namespace CQS.NET
{
	public enum DomainExceptionReason
	{
		InvalidData = 1,
		InvalidState = 2,
		NotFound = 3,
		AlreadyExist = 4,

		Forbidden = 50,
		Unauthorized = 51
	}
}
