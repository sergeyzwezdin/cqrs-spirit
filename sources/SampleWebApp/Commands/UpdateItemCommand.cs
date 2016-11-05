using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CqrsSpirit;
using CqrsSpirit.Objects;

namespace SampleWebApp.Commands
{
    public class UpdateItemCommand : ICommand
    {
        public UpdateItemCommand(int id, string value)
        {
            Id = id;
            Value = value;
        }

        public int Id { get; }

        public string Value { get; }
    }

    public class UpdateItemCommandHandler : ObjectCommandHandlerBase<UpdateItemCommand, Datasource.Datasource>
    {
        public UpdateItemCommandHandler(Datasource.Datasource dbContext)
            : base(dbContext)
        {
        }

        public override async Task ExecuteAsync(UpdateItemCommand command)
        {
            var item = DbContext.Items.Find(i => i.Id == command.Id);
            if (item != null)
            {
                item.Value = command.Value;
            }
        }
    }
}
