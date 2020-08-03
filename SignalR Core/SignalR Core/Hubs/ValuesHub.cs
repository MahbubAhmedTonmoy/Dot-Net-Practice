using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR_Core.Hubs
{
    public class ValuesHub : Hub<IValuesClient>
    {
        /// <summary>
        /// Notify all users that a value has been added.
        /// </summary>
        /// <param name="value">The new value</param>
        public async Task Add(string value)
        {
            await Clients.All.Add(value);
        }

        public async Task Delete(string value)
        {
            await Clients.All.Delete(value);
        }
    }
}
