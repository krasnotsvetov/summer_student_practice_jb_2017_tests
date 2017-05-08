using PEExtractor.Info;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEExtractor.Tables
{
    /// <summary>
    /// Interface present a column type
    /// </summary>
    public interface IColumnElement
    {
        IColumnElement Clone();

        /// TODO : remove from interface. This method shoud not be public
        void ReadValue(BinaryReader reader, TablesContext context);

        //only for reporting
        void Report(StreamWriter sw);
    }
}
