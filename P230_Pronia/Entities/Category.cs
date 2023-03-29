using Microsoft.Build.Framework;


namespace P230_Pronia.Entities
{
    public class Category:BaseEntity
    {
        //[Required(ErrorMessage ="Please fill this field")]
        public string Name { get; set; }
        public List<PlantCategory> PlantCategories{ get; set; }
    }
}
