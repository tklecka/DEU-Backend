using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEU_Lib.Model
{
    public interface IOperation
    {
        public int OperationId { get; set; }
    }

    public class Operation : IOperation
    {
        public int OperationId { get; set; }
    }
}
