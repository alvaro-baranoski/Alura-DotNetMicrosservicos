using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemService.EventProcessor
{
    public interface IEventProcessor
    {
        void Process(string message);
    }
}