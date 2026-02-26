using System;
using System.ComponentModel.DataAnnotations.Schema;
using SqlSugar;

namespace GoIoFish.Entities
{
    [Table("t_goofish_product")]
    public class GooFishProductEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
    
        [SugarColumn(ColumnName = "name", Length = 200, IsNullable = false)]
        public string Name { get; set; }
    
        [SugarColumn(ColumnName = "price", ColumnDataType = "decimal(10,2)")]
        public decimal Price { get; set; }
    
        [SugarColumn(ColumnName = "create_time", IsNullable = false, DefaultValue = "CURRENT_TIMESTAMP")]
        public DateTime CreateTime { get; set; }
    }
}