﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.Dao
{
	public abstract record BaseDao
	{
		public Guid Id { get; set; }
	}
}