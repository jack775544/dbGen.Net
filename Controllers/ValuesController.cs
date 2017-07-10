using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace dbGen.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public string Get()
        {
            var cols = new List<DatabaseColumn>();
            cols.Add(new DatabaseColumn("id", new OrderedIntegerDataGenerator()));
            cols.Add(new DatabaseColumn("b", new RandomIntegerDataGenerator(0, 1000)));
            cols[0].PrimaryKey = true;
            DatabaseTable t = new DatabaseTable("table_a", 100, cols);

            var cols2 = new List<DatabaseColumn>();
            cols2.Add(new DatabaseColumn("id2", new OrderedIntegerDataGenerator()));
            cols2.Add(new ForeignKeyColumn("d", t, cols[0], true));
            cols2[0].PrimaryKey = true;
            DatabaseTable c = new DatabaseTable("table_b", 100, cols2);

            var schema = new Schema(new List<DatabaseTable>{t, c});
            var result = schema.GetAllSQLDDLStatements();
            foreach (var line in schema.Lines())
            {
                result += line;
            }
            
            return result;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
