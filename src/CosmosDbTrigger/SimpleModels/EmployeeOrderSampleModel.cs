using System;

namespace CosmosDbTrigger.SimpleModels
{
    public class EmployeeOrderSampleModel
    {
        public string employeeId { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string title { get; set; }
        public string titleOfCourtesy { get; set; }
        public DateTime birthDate { get; set; }
        public DateTime hireDate { get; set; }
        public string address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string homePhone { get; set; }
        public string extension { get; set; }
        public byte[] photo { get; set; }
        public string notes { get; set; }
        public int reportsTo { get; set; }
        public string photoPath { get; set; }
    }
}
