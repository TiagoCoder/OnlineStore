﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OnlineStore.Data.Entities
{
    public class TempOrder : IEntity
    {
        public int Id { get; set; }


        [Required]
        public int AnonymousUserId { get; set; }


        [Required]
        [DisplayName("Order date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime OrderDate { get; set; }



        [DisplayName("Delivery date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime? DeliveryDate { get; set; }


        public IEnumerable<TempOrderDetail> Items { get; set; }


        [DisplayFormat(DataFormatString = "{0:N2}")]
        public int Lines { get { return this.Items == null ? 0 : this.Items.Count(); } }



        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Quantity => this.Items == null ? 0 : this.Items.Sum(i => i.Quantity);



        public decimal Value => this.Items == null ? 0 : this.Items.Sum(i => i.Value);



        [DisplayName("Order date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime? OrderDateLocal
        {
            get
            {
                if (this.OrderDate == null)
                {
                    return null;
                }

                return this.OrderDate.ToLocalTime();
            }
        }
    }
}
