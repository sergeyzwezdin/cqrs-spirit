using System.Threading.Tasks;
using CqrsSpirit.Objects;

namespace CqrsSpirit.Tests.Stubs
{
    public class Command1 : ICommand
    {
    }

    public class Command1Handler : ObjectCommandHandlerBase<Command1, object>
    {
        public Command1Handler(object dbContext)
            : base(dbContext)
        {
        }

        public override async Task ExecuteAsync(Command1 command)
        {
        }
    }

    public class Command11Handler : ObjectCommandHandlerBase<Command1, object>
    {
        public Command11Handler(object dbContext)
            : base(dbContext)
        {
        }

        public override async Task ExecuteAsync(Command1 command)
        {
        }
    }

    public class Command2 : ICommand
    {
    }

    public class Command2Handler : ObjectCommandHandlerBase<Command2, object>
    {
        public Command2Handler(object dbContext)
            : base(dbContext)
        {
        }

        public override async Task ExecuteAsync(Command2 command)
        {
        }
    }

    public class Command3 : ICommand
    {
    }
}