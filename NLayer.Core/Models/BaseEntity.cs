namespace NLayer.Core.Models
{
    public abstract class BaseEntity
    {
        //Normal Id diye tanımlandığında otomatik algılar
        //Eğer Id yerine Category_Id gibi PK larımızı tanımlarsak 
        //[Key] Attr tanımlamak gerekir.
        //Category_Id
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}
