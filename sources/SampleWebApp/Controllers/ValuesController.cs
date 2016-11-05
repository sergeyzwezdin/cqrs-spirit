using System.Collections.Generic;
using System.Threading.Tasks;
using CqrsSpirit;
using Microsoft.AspNetCore.Mvc;
using SampleWebApp.Commands;
using SampleWebApp.Datasource;
using SampleWebApp.Queries;
using SampleWebApp.Workflows;

namespace SampleWebApp.Controllers
{
    public class ValuesController : Controller
    {
        protected readonly IGetValuesQuery GetValuesQuery;

        protected readonly IMakeEvenItemsLowercaseWorkflow MakeEvenItemsLowercaseWorkflow;

        protected readonly ICommandsDispatcher CommandsDispatcher;

        public ValuesController(IGetValuesQuery getValuesQuery,
            IMakeEvenItemsLowercaseWorkflow makeEvenItemsLowercaseWorkflow,
            ICommandsDispatcher commandsDispatcher)
        {
            GetValuesQuery = getValuesQuery;
            MakeEvenItemsLowercaseWorkflow = makeEvenItemsLowercaseWorkflow;
            CommandsDispatcher = commandsDispatcher;
        }

        [HttpGet]
        [Route("api/values")]
        public async Task<IEnumerable<Item>> Get(string filter = null)
        {
            var result = await GetValuesQuery.ExecuteAsync(filter);
            return result;
        }

        [HttpPost]
        [Route("api/values/update")]
        public async Task Update(int id, string value)
        {
            await CommandsDispatcher.ExecuteAsync(new UpdateItemCommand(id, value));
        }

        [HttpPost]
        [Route("api/values/dosomethings")]
        public async Task Workflow()
        {
            await MakeEvenItemsLowercaseWorkflow.ExecuteAsync();
        }
    }
}