using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain;

public class DemoConfig
{
    public string SqlServerConnectionString { get; set; }
    public string SqlLiteDbFile { get; set; }
    public string DatabaseSource { get; set; }
}