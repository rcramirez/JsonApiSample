using Newtonsoft.Json;

namespace JsonApiSample.Models
{
    public class PatientFilterModel : FilterModelBase
    {
        public string Term { get; set; }

        public PatientFilterModel() : base()
        {
            this.Limit = 3;
        }


        public override object Clone()
        {
            var jsonString = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject(jsonString, this.GetType());
        }
    }
}