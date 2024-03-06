﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IEntitySystem : ISystem
{
    void RegisterEntity(Entity entity);
    void UnregisterEntity(Entity entity);
}
